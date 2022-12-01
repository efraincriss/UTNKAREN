using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Models
{
    public class ItemHijosViewModel
    {
        public int PadreId { get; set; }
        public ItemDto ItemPadre { get; set; }
        public List<Item> ItemsHijos { get; set; }
    
    }
}