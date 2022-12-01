using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ColaboradorIngenieriaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorIngenieria, ColaboradorIngenieriaDto, PagedAndFilteredResultRequestDto>, IColaboradorIngenieriaAsyncBaseCrudAppService
    {
        public IBaseRepository<Contrato> _contratoRepository;
        public IBaseRepository<Preciario> _preciarioRepository;
        public IBaseRepository<DetallePreciario> _detallePreciarioRepository;
        public IBaseRepository<Oferta> _ofertaRepository;
        public ColaboradorIngenieriaAsyncBaseCrudAppService(
        IBaseRepository<ColaboradorIngenieria> repository,
        IBaseRepository<Contrato> contratoRepository,
        IBaseRepository<Preciario> preciarioRepository,
        IBaseRepository<DetallePreciario> detallePreciarioRepository,
        IBaseRepository<Oferta> ofertaRepository
    ) : base(repository)
        {
            _contratoRepository = contratoRepository;
            _preciarioRepository = preciarioRepository;
            _detallePreciarioRepository = detallePreciarioRepository;
            _ofertaRepository = ofertaRepository;
        }
        public bool DeleteColaborador(int id)
        {
            var e = Repository.Get(id);
            e.vigente = false;
            var u = Repository.Update(e);
            return u != null && u.Id > 0 ? true : false;
        }

        public int EditColaborador(ColaboradorIngenieria e)
        {
            var c = Repository.Get(e.Id);
            c.numero_identificacion = e.numero_identificacion;
            c.apellidos = e.apellidos;
            c.nombres = e.nombres;
            c.ContratoId = e.ContratoId;
            c.CargoId = e.CargoId;
            c.tipo = e.tipo;
            c.vigente = e.vigente;
            var u = Repository.Update(c);
            return u != null && u.Id > 0 ? u.Id : 0;

        }

        public int InsertColaborador(ColaboradorIngenieria e)
        {
            var c = Repository.InsertAndGetId(e);
            return c > 0 ? c : 0;

        }

        public List<ColaboradorIngenieriaDto> Listado()
        {
            var query = Repository.GetAllIncluding(c => c.Contrato, c => c.Cargo.Preciario, c => c.Cargo.Item).Where(c => c.vigente).ToList();
            var list = (from c in query
                        select new ColaboradorIngenieriaDto()
                        {
                            Id = c.Id,
                            numero_identificacion = c.numero_identificacion,
                            apellidos = c.apellidos,
                            nombres = c.nombres,
                            CargoId = c.CargoId,
                            ContratoId = c.ContratoId,
                            nombreCargo = c.Cargo.Item.nombre,
                            codigoCargo = c.Cargo.Item.codigo,
                            nombreContrato = c.Contrato.descripcion,
                            tipo = c.tipo,
                            tipoColaborador = c.tipo > 0 ? "INDIRECTO" : "DIRECTO",
                            vigente = c.vigente
                        }).ToList();

            return list;

        }

        public List<DetallePreciario> ListarCargosByContrato(int id)
        {
            var query = _detallePreciarioRepository.GetAllIncluding(c => c.Preciario, c => c.Preciario.Contrato,c=>c.Item.Grupo)
                                                   .Where(c => c.Preciario.ContratoId == id)
                                                   .Where(c => c.vigente)
                                                   .Where(c => c.Preciario.vigente)
                                                   .Where(c => c.Preciario.Contrato.vigente)
                                                   .Where(c=>c.Item.Grupo.codigo==ProyectoCodigos.CODE_INGENIERIA)
                                                   .Where(c => DateTime.Now >= c.Preciario.fecha_desde)
                                                   .Where(c => DateTime.Now <= c.Preciario.fecha_hasta)
                                                   .ToList();
            return query;
        }
      

        public List<Contrato> ListarContratos()
        {
            var list = _contratoRepository.GetAll().Where(c => c.vigente).ToList();
            return list;
        }

        public List<ColaboradorIngenieriaDto> ListByContrato(int id)
        {
            var query = Repository.GetAllIncluding(c => c.Contrato, c => c.Cargo.Preciario, c => c.Cargo.Item)
                                  .Where(c => c.vigente)
                                  .Where(c => c.ContratoId == id)
                                  .ToList();
            var list = (from c in query
                        select new ColaboradorIngenieriaDto()
                        {
                            Id = c.Id,
                            numero_identificacion = c.numero_identificacion,
                            apellidos = c.apellidos,
                            nombres = c.nombres,
                            CargoId = c.CargoId,
                            ContratoId = c.ContratoId,
                            nombreCargo = c.Cargo.Item.nombre,
                            codigoCargo = c.Cargo.Item.codigo,
                            nombreContrato = c.Contrato.descripcion,
                            tipo = c.tipo,
                            tipoColaborador = c.tipo > 0 ? "INDIRECTO" : "DIRECTO",
                            vigente = c.vigente
                        }).ToList();

            return list;
        }

        public List<ColaboradorIngenieriaDto> ListByOferta(int id)
        {
            var oferta = _ofertaRepository.GetAllIncluding(c => c.Proyecto.Contrato).Where(c => c.vigente).Where(c=>c.Id==id).FirstOrDefault();

            var query = Repository.GetAllIncluding(c => c.Contrato, c => c.Cargo.Preciario, c => c.Cargo.Item)
                                    .Where(c => c.vigente)
                                    .Where(c => c.ContratoId == oferta.Proyecto.contratoId)
                                    .ToList();
            var list = (from c in query
                        select new ColaboradorIngenieriaDto()
                        {
                            Id = c.Id,
                            numero_identificacion = c.numero_identificacion,
                            apellidos = c.apellidos,
                            nombres = c.nombres,
                            CargoId = c.CargoId,
                            ContratoId = c.ContratoId,
                            nombreCargo = c.Cargo.Item.nombre,
                            codigoCargo = c.Cargo.Item.codigo,
                            nombreContrato = c.Contrato.descripcion,
                            tipo = c.tipo,
                            tipoColaborador = c.tipo > 0 ? "INDIRECTO" : "DIRECTO",
                            vigente = c.vigente
                        }).ToList();

            return list;
        }

        public ColaboradorIngenieria Search(string numero_identificacion)
        {
            var e = Repository.GetAll().Where(c => c.vigente).Where(c => c.numero_identificacion == numero_identificacion).FirstOrDefault();
            return e != null && e.Id > 0 ? e : new ColaboradorIngenieria();
        }
    }
}
