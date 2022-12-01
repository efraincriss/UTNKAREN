using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Models
{
    public class WbsOfertaComputoViewModel
    {
        public OfertaDto Oferta { get; set; }
        public WbsDto WbsOferta { get; set; }
        public Contrato Contrato { get; set; }
        public List<ComputoDto> Computo { get; set; }
        public string Area { get; set; }
        public string Elemento{ get; set; }
        public string Disciplina{ get; set; }
        public string Actividad { get; set; }


    }
}