using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class GananciaDetalleViewModel
    {
        public GananciaDto Ganancia { get; set; }
        public List<DetalleGananciaDto> DetallesGanancia { get; set; }
    }
}