using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using CommonServiceLocator;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace com.cpp.calypso.web.Areas.Proveedor.Controllers
{
    public class ConsumoController : BaseSPAController<ConsumoVianda, ConsumoViandaDto, PagedAndFilteredResultRequestDto>
    {
        private static readonly ILogger log =
ServiceLocator.Current.GetInstance<ILoggerFactory>().Create(typeof(ConsumoController));

        public ConsumoController(IHandlerExcepciones manejadorExcepciones, 
            IViewService viewService,
             IConsumoViandaAsyncBaseCrudAppService entityService) : base(manejadorExcepciones, viewService, entityService)
        {
        }

       
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public virtual async Task<ActionResult> IndexConciliacionDiaria()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

            //1. Model
            var model = new TreeReactModelView();
            model.Id = "conciliacion_diaria";
            model.ReactComponent = "~/Scripts/build/conciliacion.js";

            model.Title = "Conciliación Diaria";
           

           // 2.Get View
           //var view = GetViewTree();

           // var treeView = view.Layout as Tree;
           // model.View = treeView;

            //5. Message
            model.Mensaje = new MensajeHelper();
            

            return View(model);
             
        }

        #region API

        public  async Task<ActionResult> GetConciliacionDiariaApi(DateTime? fecha)
        {
            var consumoSevice = EntityService as IConsumoViandaAsyncBaseCrudAppService;
            var list = await consumoSevice.GetConciliacionDiaria(fecha);

            return WrapperResponseGetApi(ModelState, () => list);
        }

        public async Task<ActionResult> GetDetalleConsumoSolicitudApi(int solicitudId)
        {
            var consumoSevice = EntityService as IConsumoViandaAsyncBaseCrudAppService;
            var entityDto = await consumoSevice.GetConsumoDetalle(solicitudId);

            return WrapperResponseGetApi(ModelState, () => entityDto);
        }



        #endregion API
    }
}