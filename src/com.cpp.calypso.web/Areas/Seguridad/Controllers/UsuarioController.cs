using System;
using System.Web.Mvc;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.seguridad.aplicacion;
using CommonServiceLocator;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Abp.Application.Services.Dto;
using Abp;
using Abp.ObjectMapping;
using System.Net;
using Abp.Web.Models;
using System.Linq;
using Abp.Web.Mvc.Controllers.Results;
using JsonResult = com.cpp.calypso.framework.JsonResult;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace com.cpp.calypso.web.Areas.Seguridad
{
    public class UsuarioController :
        BaseCrudDtoController<Usuario, UsuarioDto, PagedAndFilteredResultRequestDto, CrearUsuarioDto>

    {

        //Logger de la aplicacion
        private static readonly ILogger Log =
            ServiceLocator.Current.GetInstance<ILoggerFactory>().Create(typeof(UsuarioController));


        private IRolService RolService;

        private readonly IModuloService ModuloService;
        private readonly IObjectMapper ObjectMapper;

        private readonly IColaboradoresAsyncBaseCrudAppService _colaboradorService;
        private readonly IProveedorAsyncBaseCrudAppService _proveedorService;

        public IUserExternalSouce UserExternalSouce { get; }

        public UsuarioController(IHandlerExcepciones manejadorExcepciones,
           IApplication application,
           ICreateObject createObject,
           IParametroService parametroService,
           PagedAndFilteredResultRequestDto getAllInput,
           IRolService rolService,
           IModuloService moduloService,
           IViewService viewService,
             IObjectMapper objectMapper,
            IUsuarioService entityService,
            IColaboradoresAsyncBaseCrudAppService colaboradorService,
                IProveedorAsyncBaseCrudAppService proveedorService,
        IUserExternalSouce userExternalSouce) :
            base(manejadorExcepciones, application, createObject, parametroService, getAllInput, viewService, entityService)
        {
            RolService = rolService;
            ModuloService = moduloService;
            this.ObjectMapper = objectMapper;
            UserExternalSouce = userExternalSouce;
            ///Configuration
            ApplySearch = true;
            ApplyPagination = true;

            this.Title = "Usuarios";
            _colaboradorService = colaboradorService;
            _proveedorService=proveedorService;
        }


        public override async Task<ActionResult> Create()
        {
            var modelView = new CrearUsuarioViewModel();

            modelView.Roles = new ReadOnlyCollection<RolDto>(await RolService.GetAll());
            modelView.Modulos = new ReadOnlyCollection<ModuloDto>(await ModuloService.GetAll());
            modelView.Model = new CrearUsuarioDto() { Estado = EstadoUsuario.Activo };
             return View(modelView);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(CrearUsuarioDto modelo, FormCollection formCollection)
        {
            var modelView = new CrearUsuarioViewModel();

            modelView.Roles = new ReadOnlyCollection<RolDto>(await RolService.GetAll());
            modelView.Modulos = new ReadOnlyCollection<ModuloDto>(await ModuloService.GetAll());
            modelView.Model = modelo;

            /*bool existeproveedor = _proveedorService.ExisteProveedor(modelo.Identificacion);
            bool existecolaborador = _colaboradorService.ExisteColaborador(modelo.Identificacion);


            if (!existeproveedor && !existecolaborador) {
                modelView.error = "Verifique si existe un Proveedor o Colaborador registrado con el Número de Identificación ingresado";
                return View(modelView);
            }*/

            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioDto = await Service.InsertOrUpdateAsync(modelo);
                    return RedirectToAction("Index", new { msg = "Proceso guardado exitosamente", TipoMensaje = TipoMensaje.Correcto });
                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }

      
            return View(modelView);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entity = await Service.Get(new EntityDto<int>(id.Value));
            if (entity == null)
            {
                var msg = string.Format("El Registro de {0} con identificacion {1} no existe, o sus datos asociados no existen",
                    "Usuario", id);
                return HttpNotFound(msg);
            }


            var modelView = new CrearUsuarioViewModel();

            modelView.Roles = new ReadOnlyCollection<RolDto>(await RolService.GetAll());
            modelView.Modulos = new ReadOnlyCollection<ModuloDto>(await ModuloService.GetAll());
            modelView.Model = ObjectMapper.Map<CrearUsuarioDto>(entity);

            return View(modelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Edit(CrearUsuarioDto modelo, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioDto = await Service.InsertOrUpdateAsync(modelo);
                    return RedirectToAction("Index", new { msg = "Proceso guardado exitosamente", TipoMensaje = TipoMensaje.Correcto });
                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }

            var modelView = new CrearUsuarioViewModel();

            modelView.Roles = new ReadOnlyCollection<RolDto>(await RolService.GetAll());
            modelView.Modulos = new ReadOnlyCollection<ModuloDto>(await ModuloService.GetAll());
            modelView.Model = modelo;

            return View(modelView);
        }

        [HttpPost]
        public async Task<ActionResult> MyInfo()
        {

            var user = Application.GetCurrentUser();

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var crearUsuarioDto = await Service.Get(new EntityDto<int>(user.Id));

            return View("_MyInfo", crearUsuarioDto);
        }



        [HttpPost]
        public async Task<JsonResult> MyInfoSave(MyUsuarioDto modelo, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioService = Service as IUsuarioService;


                    var usuarioDto = await usuarioService.Update(modelo);

                    //TODO: Estandarizar estructura de respuestas ajax
                    //return Json(new AjaxResponse());
                    //return new AbpJsonResult(new AjaxResponse());

                    return new JsonResult
                    {
                        Data = new { success = true }
                    };

                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }

            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };

        }

        [HttpPost]
        public ActionResult MyChangePassword()
        {
            return View("_MyChangePassword", new ChangeMyPasswordViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> MyChangePasswordSave(ChangeMyPasswordViewModel modelo, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioService = Service as IUsuarioService;
                    var loginResult = await usuarioService.ChangePassword(modelo.PasswordCurrent, modelo.Password);
                    if (loginResult.Succeeded)
                    {
                        return new JsonResult
                        {
                            Data = new { success = true }
                        };
                    }


                    ModelState.AddModelError("", string.Join(", ", loginResult.Errors));
                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }

            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };

        }



        [HttpPost]
        public async Task<ActionResult> Reseteo(int id, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioService = Service as IUsuarioService;
                    var loginResult = await usuarioService.ResetPassword(id);
                    if (loginResult.Result == LoginResultType.Success)
                    {
                        return new JsonResult
                        {
                            Data = new { success = true }
                        };
                    }

                    ModelState.AddModelError("", loginResult.Result.ToString());
                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }

            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };

        }


#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> GetExternal(string userName)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var externalUser = UserExternalSouce.GetUser(userName);

                    if (externalUser != null)
                    {
                        return new JsonResult
                        {
                            Data = new { success = true, user = externalUser }
                        };
                    }
                    else
                    {
                        return new JsonResult
                        {
                            Data = new { success = false, error = string.Format("Usuario {0} no existe", userName) }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                var result = ManejadorExcepciones.HandleException(ex);
                ModelState.AddModelError("", result.Message);
            }

            return new JsonResult
            {
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() }
            };

        }

        public override Tuple<bool, string> CanRemoved(UsuarioDto entity)
        {
            return new Tuple<bool, string>(true, "ok");
        }

        public ActionResult Reporte()
        {
            return View();
        }

            public ActionResult GetReporteSeguimientoOferta()
        {
            var excel = _proveedorService.ReporteUsuarios();
            string excelName = "UsuariosMovil" + DateTime.Now.ToShortDateString();
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }

        public ActionResult GetReporteSeguimientoMovil()
        {
            var excel = _proveedorService.ReporteUsuariosSincronizacion();
            string excelName = "UsuariosSincronizados" + DateTime.Now.ToShortDateString();
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }
        public ActionResult GetReporteValidacionCedula()
        {
            var excel = _proveedorService.AplicaCedulaColaboradores();
            string excelName = "ValidacionCedulaColaboradores" + DateTime.Now.ToShortDateString();
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }

        public ActionResult ReporteSincronizaciones(string Ids, DateTime fechaInicio, DateTime fechaFin) //RdoCabeceraId
        {
            List<int> ProveedoresId = new List<int>();
            var a = Ids.Split(',');
            foreach (var item in a)
            {
                if (item != "") { 
                ProveedoresId.Add(Int32.Parse(item));
                }

            }
            var excelPackage = _proveedorService.ReporteUsuariosSincronizacionTiempo(fechaInicio, fechaFin,ProveedoresId);

            string excelName = "Sincronizacion-" + DateTime.Now.Date.ToShortDateString();
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excelPackage.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return Content("");
            }
        }

    }
}
