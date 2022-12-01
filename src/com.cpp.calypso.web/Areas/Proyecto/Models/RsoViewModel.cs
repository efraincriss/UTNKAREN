using com.cpp.calypso.proyecto.aplicacion.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class RsoViewModel
    {
        public RsoCabeceraDto RdoCabeceraDto { get; set; }
        public List<RsoDetalleEacDto> DetallesEac { get; set; }
    }
}