using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class IndexWbsViewModel
    {
        public List<WbsDto> Wbs { get; set; }

        public OfertaDto oferta { get; set; }
    }
}