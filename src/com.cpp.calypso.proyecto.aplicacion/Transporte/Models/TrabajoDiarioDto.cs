using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Models
{
    public class TrabajoDiarioDto
    {
        public string Fecha { get; set; }
        public string Proveedor { get; set; }
        public string Conductor { get; set; }
        public string CodigoInventarioVehiculo { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFinRuta { get; set; }
        public string KilometrajeVehiculoInicio { get; set; }
        public string KilometrajeVehiculoFin { get; set; }

        public string KilometrajeVehiculoTotal { get; set; }

    }
}
