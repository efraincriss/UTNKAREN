using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Models
{
    public class ComputoNuevoViewModel
    {
       
        public ProyectoDto Proyecto { get; set; }
      
        public OfertaDto Oferta { get; set; }
      
        public int AreaId { get; set; }
        public String NombreArea { get; set; }
      public String  NombreDisciplina{ get; set; }
       
        public int DisciplinaId { get; set; }
        public List<Catalogo>Elementos { get; set; }
        public SelectList Actividades { get; set; }
        
        public ComputoDto Computo{ get; set; }
        
        public int ElementoId { get; set; }
       
        public int ActividadId { get; set; }
       
        public int itemid { get; set; }

    }
}