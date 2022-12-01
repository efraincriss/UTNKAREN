using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class RequerimientoDetailsViewModel
    {
        public ProyectoDto Proyecto { get; set; }

        public List<OrdenServicioDto> OrdenesServicio { get; set; }
        public List<RequerimientoDto> Requerimientos { get; set; }
        public decimal monto_os_ingenieria { get; set; }

        public decimal monto_os_construccion { get; set; }

        public decimal monto_os_procura { get; set; }
        public decimal monto_os_subcontratos { get; set; }

        public decimal monto_os_total { get; set; }

        public decimal monto_ingenieria { get; set; }
        public decimal monto_construccion { get; set; }
        public decimal montoprocura { get; set; }
        public decimal montoSubcontratos{ get; set; }
        public decimal montopresupuesto_total { get; set; }

    }
}