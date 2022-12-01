using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Castle.Components.DictionaryAdapter.Xml;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class GananciaServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<Ganancia, GananciaDto, PagedAndFilteredResultRequestDto>, IGananciaAsyncBaseCrudAppService
    {
        public readonly IBaseRepository<DetalleGanancia> _detallegananciarepository;
        public GananciaServiceAsyncBaseCrudAppService(IBaseRepository<Ganancia> repository,
            IBaseRepository<DetalleGanancia> detallegananciarepository) : base(repository)
        {
            _detallegananciarepository = detallegananciarepository;
        }

        public GananciaDto GetDetalle(int GanaciaId)
        {
            var gananciaquery = Repository.GetAllIncluding(c => c.Contrato, c => c.Contrato.Cliente).Where(c => c.vigente == true).ToList();
            GananciaDto item = (from c in gananciaquery

                                where c.Id == GanaciaId
                                select new GananciaDto
                                {
                                    Id = c.Id,
                                    fecha_inicio = c.fecha_inicio,
                                    fecha_fin = c.fecha_fin,
                                    ContratoId = c.ContratoId,
                                    Contrato = c.Contrato,
                                    estado_ganacia = c.estado_ganacia,
                                    vigente = c.vigente,
                                }).FirstOrDefault();
            return item;

        }

        public List<Ganancia> GetGanacias()
        {
            var listaganancia = Repository.GetAllIncluding(c => c.Contrato, c => c.Contrato.Cliente).Where(c => c.vigente == true).ToList();
            return listaganancia;
        }

        public List<Ganancia> GetGanaciasporContrato(int ContradoId)
        {
            var listagananciacontrato = Repository.GetAllIncluding
                (c => c.Contrato, c => c.Contrato.Cliente).
                Where(c => c.ContratoId == ContradoId).
                Where(c => c.vigente == true).ToList();
            return listagananciacontrato;
        }

        public GananciaModel GetGananciasContrato(int ContratoId, DateTime FechaOferta)
        {
            //calculo de ganancia
            var ganancia = Repository.GetAll().Where(c => c.ContratoId == ContratoId)
                .Where(c => c.fecha_inicio <= FechaOferta)
                .Where(c => c.fecha_fin >= FechaOferta).Where(c => c.vigente == true).FirstOrDefault();

            if (ganancia != null && ganancia.Id > 0)
            {
                var detalleganancia = _detallegananciarepository.GetAll().Where(c => c.GananciaId == ganancia.Id).
                     Where(c => c.vigente == true).ToList();

                if (detalleganancia.Count > 0)
                {
                    GananciaModel a = new GananciaModel
                    {
                        gananciaingenieria = (from e in detalleganancia where e.GrupoItemId == 1 select e.valor).Sum(),
                        gananciaconstruccion = (from e in detalleganancia where e.GrupoItemId == 2 select e.valor).Sum(),
                        gananciaprocura = (from e in detalleganancia where e.GrupoItemId == 3 select e.valor).Sum(),

                    };
                    return a;

                }

            }
            return new GananciaModel { gananciaingenieria = 0, gananciaconstruccion = 0, gananciaprocura = 0 };
        }

        public string ValidacionesGanancia(GananciaDto dto)
        {


            if (dto.fecha_fin.HasValue)
            {
                var periodoFechas = Repository.GetAll()
                                            .Where(c => c.ContratoId == dto.ContratoId)
                                            .Where(c => c.fecha_fin.HasValue)
                                            .ToList();
                if (periodoFechas.Count > 0)
                {

                    var e = (from p in periodoFechas
                             where dto.fecha_inicio.Date >= p.fecha_inicio.Date && dto.fecha_inicio.Date <= p.fecha_fin.Value.Date || dto.fecha_fin.Value.Date >= p.fecha_inicio.Date && dto.fecha_fin.Value.Date <= p.fecha_fin.Value.Date
                             select p
                            ).FirstOrDefault();

                    if (e != null) {
                        return "ya existe un periodo de fechas dentro del rango que intenta registrar";
                    }

                }
            }
            else {

                var existeGananciaSNFecha = Repository.GetAll()
                                                      .Where(c => c.ContratoId == dto.ContratoId)
                                                      .Where(c => !c.fecha_fin.HasValue)
                                                      .FirstOrDefault();
                if (existeGananciaSNFecha != null)
                {
                    return "ya se encuentra activo un registro sin fecha Final para el Contrato";
                }
               
            }


            return "OK";

        }
    }
}

