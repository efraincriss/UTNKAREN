using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Models
{
    public class PersonasTransportadasDto
    {
        public int Id { get; set; }
        public string Fecha { get; set; }
        public string Ruta { get; set; }

        public string Sector { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFinRuta { get; set; }
        public string Proveedor { get; set; }
        public string Conductor { get; set; }
        public string CodigoInventarioVehiculo { get; set; }
        public int TotalPersonasTransportadas { get; set; }
        public string TotalCapacidad { get; set; }
    }
}
