using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class CreateWbsOfertaViewModel
    {
        public List<WbsOfertaDto> WbsOfertas { get; set; }
        public int OfertaId { get; set; }

        public OfertaDto oferta { get; set; }
    }
}