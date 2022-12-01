using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class ItemModelo
    {
        public string codigocliente { get; set; }
        public string descripcionproyecto{ get; set; }
        public string descripcionoferta { get; set; }
        public int contador { get; set; }
        public string contrato { get; set; }
    }
}