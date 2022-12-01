using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public  class RdoViewModel
    {
        public RdoCabeceraDto RdoCabeceraDto { get; set; }
        public List<RdoDetalle>RdoDetalles{ get; set; }
        public List<RdoDetalleEacDto> DetallesEac { get; set; }
    }
}