using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class OrdenServicioViewModel
    {
        public List<OrdenServicioDto> OrdenesServicio { get; set; }

        // public OfertaDto Oferta { get; set; }
        public OfertaComercialDto Oferta { get; set; }
        public decimal MontoIngenieria { get; set; }

        public decimal MontoConstruccion { get; set; }

        public decimal MontoSuministros { get; set; }

        public decimal MontoTotal { get; set; }


    }
}