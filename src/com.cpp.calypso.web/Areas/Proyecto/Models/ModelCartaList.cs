using com.cpp.calypso.proyecto.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class ModelCartaList
    {
        public List<CartaDto> list { get; set; }
        public string numero_carta { get; set; }
    }
}