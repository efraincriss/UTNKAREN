﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class OfertaOrdenCompraViewModel
    {
        public OfertaDto oferta { get; set; }
        public List<OrdenCompraDto> listaordenes{ get; set; }
    }
}