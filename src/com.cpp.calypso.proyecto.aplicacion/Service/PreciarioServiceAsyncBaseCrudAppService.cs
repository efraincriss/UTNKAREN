using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{

    public class PreciarioServiceAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<Preciario, PreciarioDto, PagedAndFilteredResultRequestDto>,
        IPreciarioAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetallePreciario> repositorydpreciario;
        private readonly IBaseRepository<Preciario> repositorypreciario;
        private readonly IBaseRepository<Catalogo> _repositorycatalogo;
        private readonly IDetallePreciarioAsyncBaseCrudAppService detallePreciariosService;
        private readonly IBaseRepository<Item> _itemrepository;

        private readonly IBaseRepository<Oferta> _baserdoRepository;

        public PreciarioServiceAsyncBaseCrudAppService(IBaseRepository<Preciario> repository,
            IBaseRepository<DetallePreciario> repositorydpreciario,
            IBaseRepository<Preciario> repositorypreciario,
            IBaseRepository<Item> itemrepository,
            IBaseRepository<Oferta> baserdoRepository,
        IBaseRepository<Catalogo> repositorycatalogo) : base(repository)
        {
            _itemrepository = itemrepository;
            _baserdoRepository = baserdoRepository;
            this.repositorydpreciario = repositorydpreciario;
            _repositorycatalogo = repositorycatalogo;
            detallePreciariosService = new DetallePreciarioServiceAsyncBaseCrudAppService(repositorydpreciario, repositorypreciario, itemrepository, repositorycatalogo);
        }



        public bool ComprobarExistenciaPreciarioContrato(DateTime fechainicio, DateTime fechafin, int ContratoId)
        {
            var preciariocontrato = Repository.GetAllIncluding(c => c.Contrato).Where(c => c.estado == true)
                .Where(c => c.vigente == true).Where(c => c.ContratoId == ContratoId).ToList();
            bool result = false;
            foreach (var item in preciariocontrato)
            {
                if (fechainicio >= item.fecha_desde && fechafin <= item.fecha_hasta)  //Cuando el rango de las fechas esta entre las que estan registradas existentes;
                {
                    result = true;
                    ;
                    break;
                }

                if (item.fecha_desde > fechainicio && item.fecha_hasta < fechafin)  //Cuando las Fechas son Sobrepuesta 
                {
                    result = true;
                    ;
                    break;
                }

                if (fechainicio > item.fecha_desde && fechainicio < item.fecha_hasta) //Fecha Inicio entre alguno del preciario
                {
                    result = true;
                    ;
                    break;
                }

                if (fechafin > item.fecha_desde && fechafin < item.fecha_hasta) //Fecha Fin entre algunos de los del preciario
                {
                    result = true;
                    ;
                    break;
                }
            }

            return result;
        }

        public bool ComprobarExistenciaPreciarioContratoEdit(int idpreciario, DateTime fechainicio, DateTime fechafin, int ContratoId)
        {
            var preciariocontrato = Repository.GetAllIncluding(c => c.Contrato).Where(c => c.estado == true)
                .Where(c => c.vigente == true).Where(c => c.ContratoId == ContratoId).Where(r => r.Id != idpreciario).ToList();
            bool result = false;
            foreach (var item in preciariocontrato)
            {
                if (fechainicio >= item.fecha_desde && fechafin <= item.fecha_hasta)
                {
                    break;
                }

                if (item.fecha_desde > fechainicio && item.fecha_hasta < fechafin)
                {
                    result = true;
                    break;
                }

                if (fechainicio > item.fecha_desde && fechainicio < item.fecha_hasta)
                {
                    result = true;
                    break;
                }

                if (fechafin > item.fecha_desde && fechafin < item.fecha_hasta)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public PreciarioDto GetDetalle(int PreciarioId)
        {
            var cuentaQuery = Repository.GetAllIncluding(c => c.Contrato, c => c.Contrato.Cliente);
            var item = (from c in cuentaQuery
                        where c.Id == PreciarioId
                        select new PreciarioDto()
                        {
                            Id = c.Id,
                            Contrato = c.Contrato,
                            ContratoId = c.ContratoId,
                            fecha_desde = c.fecha_desde,
                            fecha_hasta = c.fecha_hasta,
                            estado = c.estado,
                            vigente = c.vigente,


                        }).SingleOrDefault();
            return item;
        }

        public Preciario GetPreciarioContrato(int ContratoId)
        {

            var preciario = Repository.GetAll().Where(e => e.vigente == true).Where(e => e.ContratoId == ContratoId).FirstOrDefault();

            return preciario;
        }

        public List<Preciario> GetPreciarios()
        {
            var preciarioQuery = Repository.GetAllIncluding(c => c.Contrato, c => c.Contrato.Cliente)
                .Where(e => e.vigente == true).OrderBy(c => c.fecha_hasta).ToList();
            return preciarioQuery;
        }

        public int EliminarVigenciaAsync(int preciarioId)
        {
            int r = 0;
            var requerimiento = this.GetDetalle(preciarioId);

            if (requerimiento != null)
            {
                var dpreciario = repositorydpreciario.GetAllIncluding(c => c.Preciario).
                    Where(c => c.PreciarioId == preciarioId).
                    Where(e => e.vigente == true).ToList();
                if (dpreciario.Count > 0)
                {

                    r = 1;
                }
                else
                {
                    requerimiento.vigente = false;
                    var reqActualizado = Repository.Update(MapToEntity(requerimiento));
                    return r;
                }
            }

            return r;
        }

        public PreciarioDto preciarioporcontratofecha(int ContratoId, DateTime? FechaOferta)
        {
            var preciario = Repository.GetAllIncluding(c => c.Contrato)
               .Where(c => c.ContratoId == ContratoId)
               .Where(c => c.fecha_desde <= FechaOferta)
               .Where(c => c.fecha_hasta >= FechaOferta)
               .Where(c => c.vigente == true).FirstOrDefault();


            if (preciario != null)
            {
                return MapToEntityDto(preciario);

            }
            else
            {
                return new PreciarioDto();
            }

        }

        public int ClonaPreciario(int preciarioid)
        {
            int idNuevoPreciario = 0;
            var preciario = GetDetalle(preciarioid);
            if (preciario.Id > 0)
            {
                //creo nuevo preciario con fechas actuales//
                preciario.Id = 0;
                preciario.fecha_desde = new DateTime(1990, 01, 01);
                preciario.fecha_hasta = new DateTime(1990, 01, 01);
                var result = Repository.InsertAndGetId(MapToEntity(preciario));
                idNuevoPreciario = result; // Insertar Id
                //Busco si hay detalles
                var dpreciario = repositorydpreciario.GetAllIncluding(c => c.Preciario).
                    Where(c => c.PreciarioId == preciarioid).
                    Where(e => e.vigente == true).ToList();
                if (dpreciario.Count > 0)
                {
                    foreach (var dp in dpreciario)
                    {
                        DetallePreciario a = new DetallePreciario
                        {
                            Id = 0,
                            PreciarioId = result,
                            precio_unitario = dp.precio_unitario,
                            vigente = dp.vigente,
                            ItemId = dp.ItemId,
                            comentario = "",
                        };

                        repositorydpreciario.Insert(a);

                    }

                }


            }
            return idNuevoPreciario;
        }

        public decimal ObtenerPrecioUnitarioItem(int ItemId, int OfertaId)
        {
            decimal PU = 0;

            var baserdo = _baserdoRepository.GetAllIncluding(c => c.Proyecto).Where(c => c.vigente).Where(c => c.Id == OfertaId).FirstOrDefault();

            if (baserdo != null && baserdo.Id > 0)
            {
                DateTime fecha = Convert.ToDateTime(baserdo.fecha_oferta);

                var item = repositorydpreciario.GetAllIncluding(c => c.Preciario)
                                              .Where(c => c.vigente)
                                              .Where(c => c.ItemId == ItemId)
                                              .Where(c => c.Preciario.vigente)
                                              .Where(c => fecha >= c.Preciario.fecha_desde)
                                              .Where(c => fecha <= c.Preciario.fecha_hasta)
                                              .Where(c => c.Preciario.ContratoId == baserdo.Proyecto.contratoId).FirstOrDefault();

                if (item != null && item.Id > 0)
                {
                    PU = item.precio_unitario;
                }

            }
            return PU;
        }

    }
}


