using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Models
{
    public class OfertaComputoViewModel
    {
        public OfertaDto Oferta { get; set; }
        public Contrato Contrato { get; set; }
        public List<ComputosTemporalDto> computosTemporal { get; set; }
    }

}