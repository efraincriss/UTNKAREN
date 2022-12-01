using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class TransmitalCabeceraDetalleViewModel
    {
        public TransmitalCabeceraDto TransmitalCabecera{ get; set; }
        public List<TransmitalDetalleDto> DetallesTransmital { get; set; }
    }
}