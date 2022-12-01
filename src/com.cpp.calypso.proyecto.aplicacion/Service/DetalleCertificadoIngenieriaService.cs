using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{

    public class DetalleCertificadoIngenieriaAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<DetalleCertificadoIngenieria, DetalleCertificadoIngenieriaDto, PagedAndFilteredResultRequestDto>,
        IDetalleCertificadoIngenieriaAsyncBaseCrudAppService
    {
        public readonly IBaseRepository<DetalleAvanceObra> _obrarepository;
        public readonly IBaseRepository<AvanceObra> _aobrarepository;
        public readonly IBaseRepository<AvanceIngenieria> _aingrepository;
        public readonly IBaseRepository<AvanceProcura> _aprorepository;
        public readonly IBaseRepository<DetalleAvanceIngenieria> _ingenieriarepository;
        public readonly IBaseRepository<DetalleAvanceProcura> _procurarepository;
        public DetalleCertificadoIngenieriaAsyncBaseCrudAppService(
            IBaseRepository<DetalleCertificadoIngenieria> repository,
            IBaseRepository<DetalleAvanceObra> obrarepository,
            IBaseRepository<AvanceObra> aobrarepository,
            IBaseRepository<AvanceIngenieria> aingrepository,
            IBaseRepository<AvanceProcura> aprorepository,
            IBaseRepository<DetalleAvanceIngenieria> ingenieriarepository,
            IBaseRepository<DetalleAvanceProcura> procurarepository
        ) : base(repository)
        {
            _obrarepository = obrarepository;
            _aobrarepository = aobrarepository;
            _aingrepository = aingrepository;
            _aprorepository = aprorepository;
            _ingenieriarepository = ingenieriarepository;
            _procurarepository = procurarepository;
        }

        public bool Eliminar(int Id)
        {
            var porcentaje = Repository.Get(Id);
            porcentaje.vigente = false;

            var r = Repository.Update(porcentaje);
            if (r.Id > 0)
            {
                return true;
            }

            return false;
        }

        public DetalleCertificadoIngenieriaDto getdetalle(int Id)
        {
            var query = Repository
                .GetAllIncluding(c => c.Certificado.Proyecto, c => c.DetalleAvanceIngenieria.Computo, c => c.Certificado.Proyecto.Contrato)
                .Where(e => e.vigente == true);
            var detalle = (from d in query
                           where d.Id == Id
                           where d.vigente == true
                           select new DetalleCertificadoIngenieriaDto
                           {
                               Id = d.Id,
                               vigente = d.vigente,
                               CertificadoId = d.CertificadoId,
                               cantidad_avance = d.cantidad_avance,
                               cantidad_certificada = d.cantidad_avance,
                               cantidad_pendiente_certificar = d.cantidad_pendiente_certificar,
                               cantidad_presupuestada = d.cantidad_presupuestada,
                               monto_a_certificar = d.monto_a_certificar,
                               DetalleAvanceIngenieriaId = d.DetalleAvanceIngenieriaId,
                               DetalleAvanceIngenieria = d.DetalleAvanceIngenieria,
                               Certificado = d.Certificado,
                               estatus_item = d.estatus_item

                           }
            ).FirstOrDefault();
            return detalle;

        }

        public List<DetalleCertificadoIngenieriaDto> Listar(int CertificadoId)
        {
            var query = Repository
                .GetAllIncluding(c => c.Certificado.Proyecto, c => c.DetalleAvanceIngenieria.Computo, c => c.DetalleAvanceIngenieria.Computo.Item, c => c.Certificado.Proyecto.Contrato,
                    c => c.DetalleAvanceIngenieria.Computo.Wbs.Oferta).Where(e => e.vigente == true);
            var detalle = (from d in query
                           where d.vigente == true
                           where d.CertificadoId == CertificadoId
                           where d.DetalleAvanceIngenieria.Computo.Item.GrupoId == 2
                           select new DetalleCertificadoIngenieriaDto
                           {
                               Id = d.Id,
                               vigente = d.vigente,
                               CertificadoId = d.CertificadoId,
                               DetalleAvanceIngenieriaId = d.DetalleAvanceIngenieriaId,
                               cantidad_avance = d.cantidad_avance,
                               cantidad_certificada = d.cantidad_avance,
                               cantidad_pendiente_certificar = d.cantidad_pendiente_certificar,
                               cantidad_presupuestada = d.cantidad_presupuestada,
                               monto_a_certificar = d.monto_a_certificar,
                               DetalleAvanceIngenieria=d.DetalleAvanceIngenieria,
                               Certificado = d.Certificado,
                               estatus_item = d.estatus_item,
                               Oferta = d.DetalleAvanceIngenieria.Computo.Wbs.Oferta

                           }
            ).ToList();
            return detalle;
        }

        public List<DetalleCertificadoIngenieriaDto> ListarI(int CertificadoId)
        {
            var query = Repository
                .GetAllIncluding(c => c.Certificado.Proyecto, c => c.DetalleAvanceIngenieria.Computo, c => c.DetalleAvanceIngenieria.Computo.Item, c => c.Certificado.Proyecto.Contrato,
                    c => c.DetalleAvanceIngenieria.Computo.Wbs.Oferta).Where(e => e.vigente == true);
            var detalle = (from d in query
                           where d.vigente == true
                           where d.CertificadoId == CertificadoId
                           where d.DetalleAvanceIngenieria.Computo.Item.GrupoId == 1
                           select new DetalleCertificadoIngenieriaDto
                           {
                               Id = d.Id,
                               vigente = d.vigente,
                               CertificadoId = d.CertificadoId,
                             DetalleAvanceIngenieriaId = d.DetalleAvanceIngenieriaId,
                               cantidad_avance = d.cantidad_avance,
                               cantidad_certificada = d.cantidad_avance,
                               cantidad_pendiente_certificar = d.cantidad_pendiente_certificar,
                               cantidad_presupuestada = d.cantidad_presupuestada,
                               monto_a_certificar = d.monto_a_certificar,
                              DetalleAvanceIngenieria = d.DetalleAvanceIngenieria,
                               Certificado = d.Certificado,
                               estatus_item = d.estatus_item,
                               Oferta = d.DetalleAvanceIngenieria.Computo.Wbs.Oferta

                           }
            ).ToList();
            return detalle;
        }
        public List<DetalleCertificadoIngenieriaDto> ListarP(int CertificadoId) //procura
        {
            var query = Repository
                .GetAllIncluding(c => c.Certificado.Proyecto, c => c.DetalleAvanceIngenieria.Computo, c => c.DetalleAvanceIngenieria.Computo.Item, c => c.Certificado.Proyecto.Contrato,
                    c => c.DetalleAvanceIngenieria.Computo.Wbs.Oferta).Where(e => e.vigente == true);
            var detalle = (from d in query
                           where d.vigente == true
                           where d.CertificadoId == CertificadoId
                           where d.DetalleAvanceIngenieria.Computo.Item.GrupoId == 3
                           select new DetalleCertificadoIngenieriaDto
                           {
                               Id = d.Id,
                               vigente = d.vigente,
                               CertificadoId = d.CertificadoId,
                             DetalleAvanceIngenieriaId = d.DetalleAvanceIngenieriaId,
                               cantidad_avance = d.cantidad_avance,
                               cantidad_certificada = d.cantidad_avance,
                               cantidad_pendiente_certificar = d.cantidad_pendiente_certificar,
                               cantidad_presupuestada = d.cantidad_presupuestada,
                               monto_a_certificar = d.monto_a_certificar,
                                DetalleAvanceIngenieria = d.DetalleAvanceIngenieria,
                               Certificado = d.Certificado,
                               estatus_item = d.estatus_item,
                               Oferta = d.Computo.Wbs.Oferta

                           }
            ).ToList();
            return detalle;
        }


        public bool InsertarDetallesObra(List<DetalleAvanceObraDto> data, int CertificadoId)
        {
            if (data.Count > 0)
            {


                foreach (var dt in data)
                {
                    var detalle = _obrarepository.Get(dt.Id);
                    DetalleCertificadoConstruccion n = new DetalleCertificadoConstruccion
                    {
                        Id = 0,
                        ComputoId = detalle.ComputoId,
                        CertificadoId = CertificadoId, // poner certificado del body,
                        cantidad_avance = detalle.cantidad_diaria, //Cantidad diaria de Avance
                        cantidad_presupuestada = detalle.Computo.cantidad, //Budget
                        estatus_item = 0,

                        vigente = true,

                    };
                    n.cantidad_pendiente_certificar = n.cantidad_presupuestada - n.cantidad_avance;// pendiente a cetificar :::  
                    n.cantidad_certificada = n.cantidad_avance;
                    n.monto_a_certificar = n.cantidad_avance * detalle.Computo.precio_unitario;


                    var r = Repository.Insert(n);
                    detalle.estacertificado = true;
                    var d = _obrarepository.Update(detalle);
                }

                return true;
            }

            return false;
        }
        public bool InsertarDetallesIngenieria(List<DetalleAvanceIngenieriaDto> data, int CertificadoId)
        {
            if (data.Count > 0)
            {


                foreach (var dt in data)
                {
                    var detalle = _ingenieriarepository.Get(dt.Id);
                    DetalleCertificadoConstruccion n = new DetalleCertificadoConstruccion
                    {
                        Id = 0,
                        ComputoId = detalle.ComputoId,
                        CertificadoId = CertificadoId, // poner certificado del body,
                        cantidad_avance = detalle.cantidad_horas,
                        cantidad_presupuestada = detalle.Computo.cantidad,
                        estatus_item = 0,
                        vigente = true,

                    };
                    n.cantidad_pendiente_certificar = n.cantidad_presupuestada - n.cantidad_avance;// pendiente a cetificar :::  
                    n.cantidad_certificada = n.cantidad_avance;
                    n.monto_a_certificar = n.cantidad_avance * detalle.Computo.precio_unitario;

                    var r = Repository.Insert(n);
                    detalle.estacertificado = true;
                    var d = _ingenieriarepository.Update(detalle);
                }

                return true;
            }

            return false;
        }

        public bool InsertarDetallesProcura(List<DetalleAvanceProcuraDto> data, int CertificadoId)
        {
            if (data.Count > 0)
            {


                foreach (var dt in data)
                {
                    var detalle = _procurarepository.Get(dt.Id);
                    DetalleCertificadoConstruccion n = new DetalleCertificadoConstruccion
                    {
                        Id = 0,
                        ComputoId = detalle.DetalleOrdenCompra.ComputoId,
                        CertificadoId = CertificadoId, // poner certificado del body,
                        cantidad_avance = detalle.cantidad,
                        cantidad_presupuestada = detalle.DetalleOrdenCompra.Computo.cantidad,
                        estatus_item = 0,
                        vigente = true,

                    };
                    n.cantidad_pendiente_certificar = n.cantidad_presupuestada - n.cantidad_avance;// pendiente a cetificar :::  
                    n.cantidad_certificada = n.cantidad_avance;
                    n.monto_a_certificar = n.cantidad_avance * detalle.DetalleOrdenCompra.Computo.precio_unitario;
                    var r = Repository.Insert(n);

                    detalle.estacertificado = true;
                    var d = _procurarepository.Update(detalle);

                }

                return true;
            }

            return false;
        }



    }
}

