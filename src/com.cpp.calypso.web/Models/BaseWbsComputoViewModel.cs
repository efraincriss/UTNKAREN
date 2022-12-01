using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Models
{
    public class BaseWbsComputoViewModel
    {
        // public WbsOferta Wbsoferta { get; set; }
         public OfertaDto Oferta { get; set; }

        public Cliente Cliente { get; set; }
        public Proyecto Proyecto { get; set; }
        public Contrato Contrato { get; set; }
        public Preciario Preciario { get; set; }
         public List<ComputoDto> Computo { get; set; }

   
        public string Area { get; set; }
        public string Elemento { get; set; }
        public string Disciplina { get; set; }
        public string Actividad { get; set; }
        public List<Catalogo> Areas { get; set; }
        public int AreaId { get; set; }
        public int DisciplinaId { get; set; }

        public bool activado { get; set; }
    }
}