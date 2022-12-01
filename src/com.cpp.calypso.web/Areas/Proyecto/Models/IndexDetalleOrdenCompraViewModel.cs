using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class IndexDetalleOrdenCompraViewModel
    {

        public List<DetalleOrdenCompraDto> DetalleOrdenServicioDto { get; set; }

        public OrdenCompraDto OrdenCompraDto { get; set; }
    }
}