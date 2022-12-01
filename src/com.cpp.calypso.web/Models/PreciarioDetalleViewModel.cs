using com.cpp.calypso.proyecto.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Models
{
    public class PreciarioDetalleViewModel
    {
        public PreciarioDto Preciario{ get; set; }
        //public DetallePreciarioDto dp { get; set; }
        public List<DetallePreciarioDto> DetallesPreciario { get; set; }

    }
}