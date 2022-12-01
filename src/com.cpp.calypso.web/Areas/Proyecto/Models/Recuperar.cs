using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class Recuperar
    {
   
        public proyecto.dominio.Proyecto proyecto { get; set; }
        public OfertaDto oferta { get; set; }
        public int Areaid { get; set; }
        public int Disciplina { get; set; }
    }
}