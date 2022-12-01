using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using AutoMapper;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{

    public class ProyectoObservacionController : BaseController
    {
        private readonly IProyectoObservacionAsyncBaseCrudAppService _proyectoService;
        private readonly IProyectoAsyncBaseCrudAppService _proyecto;

        // GET: Proyecto/ProyectoObservacion
        public ProyectoObservacionController(
            IHandlerExcepciones manejadorExcepciones,
            IProyectoObservacionAsyncBaseCrudAppService proyectoService,
            IProyectoAsyncBaseCrudAppService Proyecto
            ) : base(manejadorExcepciones)
        {
            _proyectoService = proyectoService;
            _proyecto = Proyecto;
        }

        public async Task<ActionResult> Index(int? id, string flag = "") // Proyecto Id
        {
            if (id.HasValue)
            {
                ViewBag.Proyecto = await _proyecto.Get(new EntityDto<int>(id.Value));
                if (flag == "Edited")
                {
                    ViewBag.msg = "Observación Editada";
                }
                else if (flag == "Created")
                {
                    ViewBag.msg = "Observación Creada";
                }
                else if (flag == "Deleted")
                {
                    ViewBag.msg = "Observación Eliminada";
                }
                return View();
            }
            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> IndexProyectos()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

            var result = _proyecto.GetProyectos();
            return View(result);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var observacion = await _proyectoService.Get(new EntityDto<int>(id.Value));
                return View(observacion);
            }

            return RedirectToAction("Index", "Inicio", new { area = "" });
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(ProyectoObservacionDto ob)
        {
            if (ModelState.IsValid)
            {
                await _proyectoService.Update(ob);
                return Content("OK");
            }
            return Content("ERROR");
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Create(ProyectoObservacionDto ob)
        {
            if (ModelState.IsValid)
            {
                ob.vigente = true;
                ob.FechaObservacion = ob.FechaObservacion;

                await _proyectoService.Create(ob);
                return Content("OK");

            }
            return Content("ERROR");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                var proyectoId = _proyectoService.Eliminar(id.Value);
                if (proyectoId != 0)
                {
                    return Content("OK");
                }
            }

            return Content("ERROR");
        }


        public ActionResult GetAll(int Id)//Id Proyecto
        {
            var observaciones = _proyectoService.ListarPorProyectoTipo(Id, TipoComentario.Observacion);
            var actividadesre = _proyectoService.ListarPorProyectoTipo(Id, TipoComentario.ActividadRealizada);
            var actividadespr = _proyectoService.ListarPorProyectoTipo(Id, TipoComentario.ActividadProgramada);
            var precipitaciones = _proyectoService.ListarPrecipiatacionesPorProyecto(Id);
            var model = new
            {
                observaciones,
                actividadesre,
                actividadespr,
                precipitaciones
            };

            return WrapperResponseGetApi(ModelState, () => model);
        }

        public ActionResult GetTipoObservaciones(string Id)
        {
            var observaciones = _proyectoService.ObtenerCatalogos(Id);
            var result = JsonConvert.SerializeObject(observaciones,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
            return Content(result);
        }

        public ActionResult GetDetallesProyecto(int id)
        {
            var proyecto = _proyectoService.DetallesProyecto(id);
            if (proyecto != null)
            {
                var result = JsonConvert.SerializeObject(proyecto,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                return Content(result);
            }
            else
            {
                return Content("Error");
            }
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult EditPrecipitacion(Precipitacion pre)
        {
            if (ModelState.IsValid)
            {

                var e = _proyectoService.EditarPrecipitacion(pre);
                if (e > 0)
                {
                    return Content("OK");
                }
                else
                {

                    return Content("ERROR");
                }

            }
            return Content("ERROR");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CreatePrecipitacion(Precipitacion pre)
        {
            if (ModelState.IsValid)
            {

                var e = _proyectoService.NuevaPrecipitacion(pre);
                if (e > 0)
                {
                    return Content("OK");
                }
                else
                {

                    return Content("ERROR");
                }

            }
            return Content("ERROR");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult EliminarPrecipitacion(int Id)
        {
            if (ModelState.IsValid)
            {

                var e = _proyectoService.EliminarPrecipitacion(Id);
                if (e > 0)
                {
                    return Content("OK");
                }
                else
                {

                    return Content("ERROR");
                }

            }
            return Content("ERROR");
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult cambiarRSO(int Id, bool esRSO)
        {
            if (ModelState.IsValid)
            {
                var result = _proyectoService.CambiarRDOaRSO(Id, esRSO);
                return Content(result == "OK" ? result : "ERROR");
            }
            return Content("ERROR");
        }
    }
}