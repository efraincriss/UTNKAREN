using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.RRHH.Controllers
{
    public class ColaboradoresQrController : BaseController
    {
        private readonly IQrColaboradoresAsyncBaseCrudAppService _QrService;
        private readonly IValidacionRequisitoAsyncBaseCrudAppService _service;
        private readonly ICatalogoAsyncBaseCrudAppService _catalgoService;
        public ColaboradoresQrController(IHandlerExcepciones manejadorExcepciones, IQrColaboradoresAsyncBaseCrudAppService QrService,
                 IValidacionRequisitoAsyncBaseCrudAppService service,
                ICatalogoAsyncBaseCrudAppService catalgoService) : base(manejadorExcepciones)
        {
            _QrService = QrService;
            _service = service;
            _catalgoService = catalgoService;
        }

        // GET: RRHH/ColaboradoresQr
        public ActionResult Index()
        {
            return View();
        }

        // GET: RRHH/ColaboradoresQr/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RRHH/ColaboradoresQr/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RRHH/ColaboradoresQr/Create
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

        // GET: RRHH/ColaboradoresQr/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RRHH/ColaboradoresQr/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RRHH/ColaboradoresQr/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RRHH/ColaboradoresQr/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult ObtenerColaborador(string search)
        {
            var list = _QrService.ListColaboradores(search);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }

        public ActionResult UpdateIdSapLocal(int Id, int empleado_id_sap_local)
        {
            var result = _QrService.ActualizarIdSapLocal(Id,empleado_id_sap_local);
       
            return Content(result?"OK":"ERROR");
        }

        public ActionResult ObtenerColaboradorHistorico(string search)
        {
            var list = _QrService.ListColaboradoresReingreso(search);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult ObtainUrl(List<QrColaboradorModel>rows)
        {

            var word = _QrService.GenerarTarjeta(rows);
            return Content(word);
        }
        public ActionResult DescargarTarjetas(string url)
        {
            string path = (url);
            string name = Path.GetFileName(path);
            string ext = Path.GetExtension(path);
            var type = WordHelper.GetExtension(ext);
            Response.AppendHeader("content-disposition", "inline; filename=" + name);
            if (type != "")
                Response.ContentType = type;
            Response.WriteFile(path);
            Response.End();

            return Content("");
        }
    }
}
