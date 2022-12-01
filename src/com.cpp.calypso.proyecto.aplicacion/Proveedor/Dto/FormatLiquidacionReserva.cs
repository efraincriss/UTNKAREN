using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto
{
    public class FormatLiquidacionReserva
    {
         public int Id { get; set; }
         public string NombreProveedor { get; set; }
         public string FechaConsumo { get; set; }
         public string Legajo { get; set; }
         public string Identificacion { get; set; }
         public string Nombres { get; set; }
         public string Cargo { get; set; }
         public string Habitacion { get; set; }
         public string Espacio { get; set; }
         public string Tipo { get; set; }
         public int TipoHabitacionId { get; set; }
        public string FechaSalida { get; set; }
         public decimal Tarifa { get; set; }
         public bool liquidado { get; set; }

    }
}
