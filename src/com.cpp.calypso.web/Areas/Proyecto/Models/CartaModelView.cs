using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class CartaModelView
    {
        public List<Empresa>Empresas { get; set; }
        public int EmpresaId { get; set; }
       public int tipodestinatario{ get; set; }
        public int tipo { get; set; }
        public List<CartaDto> ListaCartasEnviadas{ get; set; }
      public  List<CartaDto> ListaCartasRecibidas { get; set; }
    }
}