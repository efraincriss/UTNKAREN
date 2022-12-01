using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Service;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    public class ColaboradorController : BaseController
    {
        private readonly IColaboradorAsyncBaseCrudAppService _colaboradorsService;
        private readonly IColaboradorIngenieriaAsyncBaseCrudAppService _Service;
        public ColaboradorController(
            IHandlerExcepciones manejadorExcepciones,
            IColaboradorAsyncBaseCrudAppService colaboradorsService,
            IColaboradorIngenieriaAsyncBaseCrudAppService Service

            ) : base(manejadorExcepciones)
        {
            _colaboradorsService = colaboradorsService;
            _Service = Service;
        }

        // GET: Proyecto/Colaborador
        public ActionResult Index()
        {
            return View();
        }

        // GET: Proyecto/Colaborador/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyecto/Colaborador/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyecto/Colaborador/Create
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

        // GET: Proyecto/Colaborador/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/Colaborador/Edit/5
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

        // GET: Proyecto/Colaborador/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/Colaborador/Delete/5
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


        [HttpPost]
        public ActionResult GetColaoradoresApi()
        {
            var colaboradores = _Service.Listado();
            var result = JsonConvert.SerializeObject(colaboradores);
            return Content(result);
        }


        [HttpPost]
        public ActionResult GetUsuarioporcedula(string cedula)
        {
            var colaboradores = _colaboradorsService.buscarusuarioporcedula(cedula);
            if (colaboradores != null)
            {
                var result = JsonConvert.SerializeObject(colaboradores);
                return Content(result);
            }
            else
            {
                return Content("Error");
            }

        }


        [HttpPost]
        public ActionResult GetItemsIngenieria(int id)
        {
            var colaboradores = _colaboradorsService.items_ingenieria_contrato(id);
            var result = JsonConvert.SerializeObject(colaboradores);
            return Content(result);
        }

        [HttpPost]
        public ActionResult Crear(Colaborador colaborador)
        {

            var x = _colaboradorsService.CrearColaborador(colaborador);

            if (x > 0)
            {
                return Content("o");
            }
            else
            {
                return Content("e");
            }
        }

        public ActionResult Obtain()
        {
            var list = _Service.Listado();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult Obtainby(int id) //Contrato Id
        {
            var list = _Service.ListByContrato(id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult ObtainbyOferta(int id) //Contrato Id
        {
            var list = _Service.ListByOferta(id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult ObtainContratos() //Contrato Id
        {
            var list = _Service.ListarContratos();
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult ObtainDetails(int id) //Contrato Id
        {
            var list = _Service.ListarCargosByContrato(id);
            var result = JsonConvert.SerializeObject(list);
            return Content(result);
        }
        public ActionResult ObtainCrear(ColaboradorIngenieria e) //Contrato Id
        {
            var r = _Service.InsertColaborador(e);
            return Content(r > 0 ? "OK" : "ERROR");
        }
        public ActionResult ObtainEditar(ColaboradorIngenieria e) //Contrato Id
        {
            var r = _Service.EditColaborador(e);
            return Content(r > 0 ? "OK" : "ERROR");
        }
        public ActionResult ObtainEliminar(int id) //Contrato Id
        {
            var r = _Service.DeleteColaborador(id);
            return Content(r ? "OK" : "ERROR");
        }
    }
}
