using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class WbsOfertaCatalogoViewModel
    {
         
        public List<CatalogoDto> areas { get; set; }
        public List<CatalogoDto> disciplinas { get; set; }
        public List<CatalogoDto> elementos { get; set; }
        public List<CatalogoDto> actividades { get; set; }

        public WbsOfertaDto wbs { get; set; }
    }
}