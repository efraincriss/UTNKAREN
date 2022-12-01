using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;

namespace com.cpp.calypso.web.Areas.Proyecto.Controllers
{
    public class GrupoItemController : BaseController
    {
               private readonly IGrupoItemAsyncBaseCrudAppService _grupoService;
        public GrupoItemController(IHandlerExcepciones manejadorExcepciones,
            IGrupoItemAsyncBaseCrudAppService grupoService) : base(manejadorExcepciones)
        {
            _grupoService = grupoService;

        }

        // GET: Proyecto/GrupoItem
        public ActionResult Index()
        {
            var lista = _grupoService.lista();
            return View(lista);
        }

        // GET: Proyecto/GrupoItem/Details/5
        public ActionResult Details(int id)
        {
            var grupo = _grupoService.getdetalle(id);

            return View(grupo);
        }

        // GET: Proyecto/GrupoItem/Create
        public ActionResult Create()
        {
            GrupoItemDto n= new GrupoItemDto();
            n.vigente = true;
            return View(n);
        }

        // POST: Proyecto/GrupoItem/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(GrupoItemDto n)
        {
            var r = await _grupoService.Create(n);

                return RedirectToAction("Index");
            
        }

        // GET: Proyecto/GrupoItem/Edit/5
        public ActionResult Edit(int id)
        {
            var grupo = _grupoService.getdetalle(id);
            return View(grupo);
        }

        // POST: Proyecto/GrupoItem/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(GrupoItemDto n)
        {
            var r = await _grupoService.Update(n);

                return RedirectToAction("Index");
           
        }

        // POST: Proyecto/GrupoItem/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var r = _grupoService.Eliminar(id);

            if (r)
            {
              return  RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }
    }
    }
