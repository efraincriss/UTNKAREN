using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Interface;
using com.cpp.calypso.proyecto.dominio.Documentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Documentos.Controllers
{
    public class UsuarioAutorizadoController : BaseAccesoSpaController<UsuarioAutorizado, UsuarioAutorizadoDto, PagedAndFilteredResultRequestDto>
    {
        private readonly IUsuarioAutorizadoAsyncBaseCrudAppService _usuarioAutorizadoService;

        public UsuarioAutorizadoController(
            IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
            IUsuarioAutorizadoAsyncBaseCrudAppService usuarioAutorizadoService
        ) : base(manejadorExcepciones, viewService)
        {
            _usuarioAutorizadoService = usuarioAutorizadoService;
        }

        public ActionResult Usuarios(int contratoId)
        {
            var model = new FormReactModelView();
            model.Id = "gestion_usuarios_autorizados_contratos";
            model.ReactComponent = "~/Scripts/build/gestion_usuarios_autorizados_contratos.js";

            if (!string.IsNullOrEmpty(Title))
            {
                model.Title = Title;
            }


            ViewBag.ruta = new string[] { "Inicio", "Gestión de Contratos", "Listado de Contratos", "Gestión de Usuarios" };

            return View(model);
        }


        #region Api
        [HttpGet]
        public ActionResult ObtenerUsuariosDisponibles(int id)
        {
            var usuarios = _usuarioAutorizadoService.ObtenerUsuariosDisponiblesPorContrato(id);
            return WrapperResponseGetApi(ModelState, () => usuarios);
        }

        [HttpGet]
        public ActionResult ObtenerUsuariosAsignados(int id)
        {
            var usuarios = _usuarioAutorizadoService.ObtenerUsuariosAutorizadosPorContratoId(id);
            return WrapperResponseGetApi(ModelState, () => usuarios);
        }

        [HttpPost]
        public ActionResult CrearUsuarioAutorizados(List<int> usuarios, int carpetaId)
        {
            _usuarioAutorizadoService.AgregarUsuarios(usuarios, carpetaId);
            return WrapperResponseGetApi(ModelState, () => true);
        }

        [HttpDelete]
        public ActionResult EliminarUsuarioAutorizado(int usuarioId, int carpetaId)
        {
            var result = _usuarioAutorizadoService.EliminarUsuarioAutorizado(usuarioId, carpetaId);
            return new System.Web.Mvc.JsonResult
            {
                Data = new { success = result.Eliminado, result = result.Error }
            };
        }
        #endregion
    }
}