using Abp.Application.Services.Dto;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    /// <summary>
    /// Campusoft: Este controlador deberia ser realizado por cliente. Centralizacion de Archivos
    /// </summary>
    public class ArchivoController : BaseController
    {
        public IArchivoAsyncBaseCrudAppService ArchivoService { get; }

        public ArchivoController(IHandlerExcepciones manejadorExcepciones,
            IArchivoAsyncBaseCrudAppService archivoService) :
            base(manejadorExcepciones)
        {
            ArchivoService = archivoService;
        }

       

        public async  Task<ActionResult> Descargar(int id)
        {
            var entity = await ArchivoService.Get(new EntityDto<int>(id));

            if (entity == null)
            {
                var msg = string.Format("El Archivo con identificacion {0} no existe",
                    id);

                return HttpNotFound(msg);
            }

            return File(entity.hash, entity.tipo_contenido, entity.nombre);
        }


    }
}