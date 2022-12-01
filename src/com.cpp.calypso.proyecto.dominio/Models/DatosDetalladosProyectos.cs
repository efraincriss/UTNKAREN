using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class DatosDetalladosProyectos
    {
        public int contratoId { get; set; }
        public string contrato { get; set; }
        public int proyectoId { get; set; }
        public int requerimientoId { get; set; }
        public string clasificacion { get; set; }
        public string codigoProyecto { get; set; }
        public string nombreProyecto { get; set; }
        public string descripcionProyecto { get; set; }
        public string codigoRequerimiento { get; set; }
        public string descripcionRequerimiento { get; set; }
        public string estadoRequerimiento { get; set; }
        public string codigoEstadoRequerimiento { get; set; }
        public decimal monto { get; set; } = 0;
      
        public string fechaSolicitud { get; set; }
        public string fechaPresentacion { get; set; }
        public string fechaSO { get; set; }
        public string numeroOferta { get; set; }

    }
}
