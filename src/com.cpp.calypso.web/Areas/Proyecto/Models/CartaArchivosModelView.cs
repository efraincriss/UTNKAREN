using com.cpp.calypso.proyecto.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class CartaArchivosModelView
    {
        public CartaDto Carta{get; set; }
        public List<CartaArchivoDto> ListaArchivos { get; set; }
        public List<DestinatarioCartaDto> Track { get; set; }
    }
    
}