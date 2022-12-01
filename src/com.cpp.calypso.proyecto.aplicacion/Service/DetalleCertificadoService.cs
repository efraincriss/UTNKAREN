using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class DetalleCertificadoAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<DetalleCertificado, DetalleCertificadoDto, PagedAndFilteredResultRequestDto>,
        IDetalleCertificadoAsyncBaseCrudAppService
    {
        public readonly IBaseRepository<Certificado> _certificadorepository;
        public readonly IBaseRepository<DetalleAvanceObra> _obrarepository;
        public readonly IBaseRepository<AvanceObra> _aobrarepository;
        public readonly IBaseRepository<AvanceIngenieria> _aingrepository;
        public readonly IBaseRepository<AvanceProcura> _aprorepository;
        public readonly IBaseRepository<DetalleAvanceIngenieria> _ingenieriarepository;
        public readonly IBaseRepository<DetalleAvanceProcura> _procurarepository;
        public DetalleCertificadoAsyncBaseCrudAppService(
            IBaseRepository<DetalleCertificado> repository,
            IBaseRepository<DetalleAvanceObra> obrarepository,
            IBaseRepository<AvanceObra> aobrarepository,
            IBaseRepository<AvanceIngenieria> aingrepository,
            IBaseRepository<AvanceProcura> aprorepository,
            IBaseRepository<DetalleAvanceIngenieria> ingenieriarepository,
            IBaseRepository<DetalleAvanceProcura> procurarepository,
            IBaseRepository<Certificado> certificadorepository
        ) : base(repository)
        {
            _obrarepository = obrarepository;
            _aobrarepository=aobrarepository;
            _aingrepository = aingrepository;
            _aprorepository = aprorepository;
            _ingenieriarepository = ingenieriarepository;
            _procurarepository = procurarepository;
            _certificadorepository= certificadorepository;
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

        public DetalleCertificadoDto getdetalle(int Id)
        {
            var query = Repository
                .GetAllIncluding(c => c.Certificado.Proyecto, c => c.Computo, c => c.Certificado.Proyecto.Contrato)
                .Where(e => e.vigente == true);
            var detalle = (from d in query
                where d.Id == Id
                where d.vigente == true
                select new DetalleCertificadoDto
                {
                    Id = d.Id,
                    vigente = d.vigente,
                    CertificadoId = d.CertificadoId,
                    ComputoId = d.ComputoId,
                    cantidad_avance = d.cantidad_avance,
                    cantidad_certificada = d.cantidad_avance,
                    cantidad_pendiente_certificar = d.cantidad_pendiente_certificar,
                    cantidad_presupuestada = d.cantidad_presupuestada,
                    monto_a_certificar = d.monto_a_certificar,
                    Computo = d.Computo,
                    Certificado = d.Certificado,
                    estatus_item = d.estatus_item,
                    avanceid_referencia = d.avanceid_referencia,
                    tipoavance = d.tipoavance,


                }
            ).FirstOrDefault();
            return detalle;

        }

        public List<DetalleCertificadoDto> Listar(int CertificadoId, int tipo)
        {
            var query = Repository
                .GetAllIncluding(c => c.Certificado.Proyecto, c => c.Computo, c => c.Computo.Item, c => c.Certificado.Proyecto.Contrato,
                    c => c.Computo.Wbs.Oferta).Where(e => e.vigente == true);
            var detalle = (from d in query
                where d.vigente == true
                where d.CertificadoId == CertificadoId
                where d.tipoavance == tipo
                           select new DetalleCertificadoDto
                {
                    Id = d.Id,
                    vigente = d.vigente,
                    CertificadoId = d.CertificadoId,
                    ComputoId = d.ComputoId,
                    cantidad_avance = d.cantidad_avance,
                    cantidad_certificada = d.cantidad_avance,
                    cantidad_pendiente_certificar = d.cantidad_pendiente_certificar,
                    cantidad_presupuestada = d.cantidad_presupuestada,
                    monto_a_certificar = d.monto_a_certificar,
                    Computo = d.Computo,
                    Certificado = d.Certificado,
                    estatus_item = d.estatus_item,
                    Oferta = d.Computo.Wbs.Oferta,
                    Item = d.Computo.Item,
                    tipoavance = d.tipoavance,
                    avanceid_referencia = d.avanceid_referencia
                    
                }
            ).ToList();
            return detalle;
        }

        public List<DetalleCertificadoDto> ListarI(int CertificadoId)
        {
            var query = Repository
                .GetAllIncluding(c => c.Certificado.Proyecto, c => c.Computo, c => c.Computo.Item,c => c.Certificado.Proyecto.Contrato,
                    c => c.Computo.Wbs.Oferta).Where(e => e.vigente == true);
            var detalle = (from d in query
                where d.vigente == true
                where d.CertificadoId == CertificadoId
                           where d.Computo.Item.GrupoId==1
                select new DetalleCertificadoDto
                {
                    Id = d.Id,
                    vigente = d.vigente,
                    CertificadoId = d.CertificadoId,
                    ComputoId = d.ComputoId,
                    cantidad_avance = d.cantidad_avance,
                    cantidad_certificada = d.cantidad_avance,
                    cantidad_pendiente_certificar = d.cantidad_pendiente_certificar,
                    cantidad_presupuestada = d.cantidad_presupuestada,
                    monto_a_certificar = d.monto_a_certificar,
                    Computo = d.Computo,
                    Certificado = d.Certificado,
                    estatus_item = d.estatus_item,
                    Oferta = d.Computo.Wbs.Oferta,
                    Item = d.Computo.Item
                }
            ).ToList();
            return detalle;
        }
        public List<DetalleCertificadoDto> ListarP(int CertificadoId) //procura
        {
            var query = Repository
                .GetAllIncluding(c => c.Certificado.Proyecto, c => c.Computo, c => c.Computo.Item, c => c.Certificado.Proyecto.Contrato,
                    c => c.Computo.Wbs.Oferta).Where(e => e.vigente == true);
            var detalle = (from d in query
                where d.vigente == true
                where d.CertificadoId == CertificadoId
                           where d.Computo.Item.GrupoId==3
                select new DetalleCertificadoDto
                {
                    Id = d.Id,
                    vigente = d.vigente,
                    CertificadoId = d.CertificadoId,
                    ComputoId = d.ComputoId,
                    cantidad_avance = d.cantidad_avance,
                    cantidad_certificada = d.cantidad_avance,
                    cantidad_pendiente_certificar = d.cantidad_pendiente_certificar,
                    cantidad_presupuestada = d.cantidad_presupuestada,
                    monto_a_certificar = d.monto_a_certificar,
                    Computo = d.Computo,
                    Certificado = d.Certificado,
                    estatus_item = d.estatus_item,
                    Oferta = d.Computo.Wbs.Oferta,
                    Item = d.Computo.Item
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
                    DetalleCertificado n = new DetalleCertificado
                    {
                        Id = 0,
                        ComputoId = detalle.ComputoId,
                        CertificadoId = CertificadoId, // poner certificado del body,
                        cantidad_avance = detalle.cantidad_diaria, //Cantidad diaria de Avance
                        cantidad_presupuestada = detalle.Computo.cantidad, //Budget
                        estatus_item = 0,
                       
                        vigente = true,
                        avanceid_referencia = "" + detalle.Id,
                        tipoavance = 2,
                    };
                    n.cantidad_pendiente_certificar = n.cantidad_presupuestada - n.cantidad_avance;// pendiente a cetificar :::  
                    n.cantidad_certificada = n.cantidad_avance;
                    n.monto_a_certificar =n.cantidad_avance*detalle.Computo.precio_unitario;

                    
                    var r = Repository.Insert(n);
                    detalle.estacertificado = true;
                   // var d = _obrarepository.Update(detalle);
                }

                return true;
            }

            return false;
        }



        public bool InsertarDetallesObraFast(int [] data, int CertificadoId)
        {
            if (data.Length > 0)
            {


                foreach (var dt in data)
                {
                    var detalle = _obrarepository.Get(dt);
                    DetalleCertificado n = new DetalleCertificado
                    {
                        Id = 0,
                        ComputoId = detalle.ComputoId,
                        CertificadoId = CertificadoId, // poner certificado del body,
                        cantidad_avance = detalle.cantidad_diaria, //Cantidad diaria de Avance
                        cantidad_presupuestada = detalle.Computo.cantidad, //Budget
                        estatus_item = 0,

                        vigente = true,
                        avanceid_referencia = "" + detalle.Id,
                        tipoavance = 2,
                    };
                    n.cantidad_pendiente_certificar = n.cantidad_presupuestada - n.cantidad_avance;// pendiente a cetificar :::  
                    n.cantidad_certificada = n.cantidad_avance;
                    n.monto_a_certificar = n.cantidad_avance * detalle.Computo.precio_unitario;


                    var r = Repository.Insert(n);
                    detalle.estacertificado = true;
                    // var d = _obrarepository.Update(detalle);
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
                    DetalleCertificado n = new DetalleCertificado
                    {
                        Id = 0,
                        ComputoId = detalle.ComputoId,
                        CertificadoId = CertificadoId, // poner certificado del body,
                        cantidad_avance = detalle.cantidad_horas,
                        cantidad_presupuestada = detalle.Computo.cantidad,
                        estatus_item = 0,
                        vigente = true,
                        avanceid_referencia = ""+detalle.Id,
                        tipoavance =1,

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
                    DetalleCertificado n = new DetalleCertificado
                    {
                        Id = 0,
                        ComputoId = detalle.DetalleOrdenCompra.ComputoId,
                        CertificadoId = CertificadoId, // poner certificado del body,
                        cantidad_avance = detalle.cantidad,
                        cantidad_presupuestada =detalle.DetalleOrdenCompra.Computo.cantidad,
                        estatus_item = 0,
                        vigente = true,
                        avanceid_referencia = "" + detalle.Id,
                        tipoavance = 3,

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

        public decimal montocertificadototal(int CertificadoId, int tipo)
        {
            decimal montototal = 0;
            var query = Repository
                .GetAllIncluding(c => c.Certificado.Proyecto, c => c.Computo, c => c.Computo.Item, c => c.Certificado.Proyecto.Contrato,
                    c => c.Computo.Wbs.Oferta).Where(e => e.vigente == true);
            var detalle = (from d in query
                where d.vigente == true
                where d.CertificadoId == CertificadoId
                where d.tipoavance == tipo
                select new DetalleCertificadoDto
                {
                    Id = d.Id,
                    vigente = d.vigente,
                    CertificadoId = d.CertificadoId,
                    ComputoId = d.ComputoId,
                    cantidad_avance = d.cantidad_avance,
                    cantidad_certificada = d.cantidad_avance,
                    cantidad_pendiente_certificar = d.cantidad_pendiente_certificar,
                    cantidad_presupuestada = d.cantidad_presupuestada,
                    monto_a_certificar = d.monto_a_certificar,
                    Computo = d.Computo,
                    Certificado = d.Certificado,
                    estatus_item = d.estatus_item,
                    Oferta = d.Computo.Wbs.Oferta,
                    Item = d.Computo.Item,
                    tipoavance = d.tipoavance,
                    avanceid_referencia = d.avanceid_referencia

                }
            ).ToList();


            if (detalle != null && detalle.Count >= 0)
            {
                montototal = (from x in detalle select x.monto_a_certificar).Sum(); //Presupuesto Total

            }
            return montototal;
        }

        public bool actualizarmontoscertificados()
        {
      
           
            var x = _certificadorepository.GetAllIncluding(c => c.Proyecto).Where(c=>c.vigente==true).ToList();

            foreach (var cert in x)
            {
                decimal montototal = 0;
                decimal montopendiente = 0;

                var query = Repository
                    .GetAllIncluding(c => c.Certificado.Proyecto, c => c.Computo, c => c.Computo.Item, c => c.Certificado.Proyecto.Contrato,
                        c => c.Computo.Wbs.Oferta).Where(e => e.vigente == true);
                var detalle = (from d in query
                    where d.vigente == true
                    where d.CertificadoId == cert.Id
                    where d.tipoavance == cert.tipo_certificado
                    select new DetalleCertificadoDto
                    {
                        Id = d.Id,
                        vigente = d.vigente,
                        CertificadoId = d.CertificadoId,
                        ComputoId = d.ComputoId,
                        cantidad_avance = d.cantidad_avance,
                        cantidad_certificada = d.cantidad_avance,
                        cantidad_pendiente_certificar = d.cantidad_pendiente_certificar,
                        cantidad_presupuestada = d.cantidad_presupuestada,
                        monto_a_certificar = d.monto_a_certificar,
                        Computo = d.Computo,
                        Certificado = d.Certificado,
                        estatus_item = d.estatus_item,
                        Oferta = d.Computo.Wbs.Oferta,
                        Item = d.Computo.Item,
                        tipoavance = d.tipoavance,
                        avanceid_referencia = d.avanceid_referencia

                    }
                ).ToList();


                if (detalle != null && detalle.Count >= 0)
                {
                    montototal = (from y in detalle select y.monto_a_certificar).Sum(); //Presupuesto Total
                    montopendiente=(from y in detalle select (y.cantidad_pendiente_certificar*y.Computo.precio_unitario)).Sum(); //Presupuesto Tota
                }

                var certificado = _certificadorepository.Get(cert.Id);
                certificado.monto_certificado = montototal;
                certificado.monto_pendiente = montopendiente;
                var s = _certificadorepository.Update(certificado);

            }

            return true;

        }



        public bool actualizarmontoscertificadosCerficado(int certificadoId)
        {


            var x = _certificadorepository.GetAllIncluding(c => c.Proyecto)
                                           .Where(c=>c.Id== certificadoId).Where(c => c.vigente == true).ToList();

            foreach (var cert in x)
            {
                decimal montototal = 0;
                decimal montopendiente = 0;

                var query = Repository
                    .GetAllIncluding(c => c.Certificado.Proyecto, c => c.Computo, c => c.Computo.Item, c => c.Certificado.Proyecto.Contrato,
                        c => c.Computo.Wbs.Oferta).Where(e => e.vigente == true)
                        .Where(c=>c.CertificadoId==cert.Id);
                var detalle = (from d in query
                               where d.vigente == true
                               where d.CertificadoId == cert.Id
                               where d.tipoavance == cert.tipo_certificado
                               select new DetalleCertificadoDto
                               {
                                   Id = d.Id,
                                   vigente = d.vigente,
                                   CertificadoId = d.CertificadoId,
                                   ComputoId = d.ComputoId,
                                   cantidad_avance = d.cantidad_avance,
                                   cantidad_certificada = d.cantidad_avance,
                                   cantidad_pendiente_certificar = d.cantidad_pendiente_certificar,
                                   cantidad_presupuestada = d.cantidad_presupuestada,
                                   monto_a_certificar = d.monto_a_certificar,
                                   Computo = d.Computo,
                                   Certificado = d.Certificado,
                                   estatus_item = d.estatus_item,
                                   Oferta = d.Computo.Wbs.Oferta,
                                   Item = d.Computo.Item,
                                   tipoavance = d.tipoavance,
                                   avanceid_referencia = d.avanceid_referencia

                               }
                ).ToList();


                if (detalle != null && detalle.Count >= 0)
                {
                    montototal = (from y in detalle select y.monto_a_certificar).Sum(); //Presupuesto Total
                    montopendiente = (from y in detalle select (y.cantidad_pendiente_certificar * y.Computo.precio_unitario)).Sum(); //Presupuesto Tota
                }

                var certificado = _certificadorepository.Get(cert.Id);
                certificado.monto_certificado = montototal;
                certificado.monto_pendiente = montopendiente;
                var s = _certificadorepository.Update(certificado);

            }

            return true;

        }


    }
}
        
    