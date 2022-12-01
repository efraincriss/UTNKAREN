using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class ReportHospedaje
    {
        public int Id { get; set; }
        public string identificacionProveedor { get; set; }
        public string razon_social { get; set; }
        public int ProveedorId { get; set; }
        public DateTime fechaConsumo { get; set; }
        public string NombresCopletos { get; set; }
        public string Identificacion { get; set; }
        public decimal tarifa { get; set; }
        public string nombretipoHabitacion { get; set; }
        public int TipoHabitacionId { get; set; }
        public string NumeroHabitacion { get; set; }
        public string EspacioHabitacion { get; set; }
        public string formatfechaConsumo { get; set; }
        public bool liquidado { get; set; }
        public string formatliquidado { get; set; }
    
        public string fechaInicioReserva { get; set; }

        public string fechaFinReserva { get; set; }
        public string fechaInicioConsumo { get; set; }
        public string fechaFinConsumo { get; set; }

        public string aplicaLavanderiaString { get; set; }

        public bool aplicaLavanderia { get; set; }

        public decimal tarifaLavanderia { get; set; }

        public string IdentificadorMovil { get; set; }
        public string nombreOrigen { get; set; }

    }
}
