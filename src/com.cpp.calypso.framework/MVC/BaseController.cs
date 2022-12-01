using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace com.cpp.calypso.framework
{
    /// <summary>
    /// Clase base para los controladores de MVC 
    /// </summary>
    /// 
    //[UserInfo]
    public abstract class BaseController : Controller
    {
        private readonly IHandlerExcepciones _manejadorExcepciones;

        protected BaseController(IHandlerExcepciones manejadorExcepciones)
        {
            _manejadorExcepciones = manejadorExcepciones;
        }

        /// <summary>
        /// Objeto para manejar excepciones
        /// </summary>
        protected IHandlerExcepciones ManejadorExcepciones
        {
            get { return _manejadorExcepciones; }
        }

        //protected override System.Web.Mvc.JsonResult Json(object data, string contentType, Encoding contentEncoding)
        //{
        //    return base.Json(data, contentType, contentEncoding);
        //}
          

        //protected override JsonResult Json(object data, string contentType, Encoding contentEncoding )
        //{
        //    return new com.cpp.calypso.framework.JsonResult
        //    {
        //        Data = data,
        //        ContentType = contentType,
        //        ContentEncoding = contentEncoding,
        //        //JsonRequestBehavior = behavior
        //    };
        //}

        protected JsonResult WrapperResponseGetApi(ModelStateDictionary modelStateDictionary, Func<object> process)
        {
            try
            {
                if (modelStateDictionary.IsValid)
                {

                    var result =  process();

                   
                    return new JsonResult
                    {
                        Data = new { success = true, result },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
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
                Data = new { success = false, errors = ModelState.ToSerializedDictionary() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// Estandarizar la respuesta de API desde un controller
        /// </summary>
        /// <param name="modelStateDictionary"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        protected JsonResult WrapperResponsePostApi(ModelStateDictionary modelStateDictionary, Func<Task<object>> process)
        {
            try
            {
                if (modelStateDictionary.IsValid)
                {

                    var result =  process();

                    return new JsonResult
                    {
                        Data = new { success = true, result }
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

    }

    
    
}
