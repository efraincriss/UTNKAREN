using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class OfertaTranmitalCabeceraViewModel
    {
        public OfertaDto Oferta { get; set; }
        public List<TransmitalCabeceraDto> TransmitalCabeceras { get; set; }
    }
}