using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class ModelTransmittal
    {
        public List<TransmitalCabeceraDto> list { get; set; }
        public string codigo_transmital { get; set; }
    }
}