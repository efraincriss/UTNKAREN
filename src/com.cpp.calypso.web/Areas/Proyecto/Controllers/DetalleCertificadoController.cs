using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    
    public class DetalleCertificadoController : BaseController
    {

        private readonly ICertificadoAsyncBaseCrudAppService _certificadoService;
        private readonly IDetalleCertificadoAsyncBaseCrudAppService _detallescertificadoService;
        private readonly IDetalleAvanceObraAsyncBaseCrudAppService _davanceObraService;
        private readonly IDetalleAvanceIngenieriaAsyncBaseCrudAppService _davanceIngenieriaService;
        private readonly IDetalleAvanceProcuraAsyncBaseCrudAppService _davanceProcuraService;
        public DetalleCertificadoController(IHandlerExcepciones manejadorExcepciones,
            ICertificadoAsyncBaseCrudAppService certificadoService,
            IDetalleCertificadoAsyncBaseCrudAppService detallescertificadoService,
            IDetalleAvanceObraAsyncBaseCrudAppService davanceObraService,
            IDetalleAvanceIngenieriaAsyncBaseCrudAppService davanceIngenieriaService,
            IDetalleAvanceProcuraAsyncBaseCrudAppService davanceProcuraService
            ) : base(manejadorExcepciones)
        {
            _certificadoService = certificadoService;
            _detallescertificadoService = detallescertificadoService;
            _davanceObraService = davanceObraService;
            _davanceIngenieriaService = davanceIngenieriaService;
            _davanceProcuraService = davanceProcuraService; 
        }

        // GET: Proyecto/DetalleCertificado
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/DetalleCertificado/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyecto/DetalleCertificado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyecto/DetalleCertificado/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/DetalleCertificado/Edit/5
   
        // POST: Proyecto/DetalleCertificado/Edit/5
        [HttpPost]
        public ActionResult Aprobar(int id)
        {
            var certificado = _certificadoService.getdetalle(id);
            var r = _certificadoService.cambiarestadocertificado(certificado.Id);
            String mes = "Certificado Aprobado";
            return RedirectToAction("DetailsCertificado", "Certificado", new { id = certificado.Id, message=mes });
        }


        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            var detalle = _detallescertificadoService.getdetalle(id);
            var certificado = _certificadoService.getdetalle(detalle.CertificadoId);

            
            if (certificado.tipo_certificado == 1)
            {
                var procura = await _davanceIngenieriaService.Get(new EntityDto<int>(Int32.Parse(detalle.avanceid_referencia)));
                var r = _davanceIngenieriaService.cambiaracertificado(procura.Id);
                var s = _detallescertificadoService.Eliminar(detalle.Id);
                return RedirectToAction("DetailsCertificado", "Certificado", new {id = certificado.Id});
            }

            if (certificado.tipo_certificado == 2)
            {
                var obra =await _davanceObraService.Get(new EntityDto<int>(Int32.Parse(detalle.avanceid_referencia)));
                var r = _davanceObraService.cambiaracertificado(obra.Id);
                var s = _detallescertificadoService.Eliminar(detalle.Id);
                return RedirectToAction("DetailsCertificado", "Certificado", new { id = certificado.Id });

            }

            if (certificado.tipo_certificado == 3)
            {
                var procura = await _davanceProcuraService.Get(new EntityDto<int>(Int32.Parse(detalle.avanceid_referencia)));
                var r = _davanceProcuraService.cambiaracertificado(procura.Id);

                var s = _detallescertificadoService.Eliminar(detalle.Id);
                return RedirectToAction("DetailsCertificado", "Certificado", new { id = certificado.Id });
            }

            return RedirectToAction("DetailsCertificado", "Certificado", new { id = certificado.Id });

        }
    }
}
