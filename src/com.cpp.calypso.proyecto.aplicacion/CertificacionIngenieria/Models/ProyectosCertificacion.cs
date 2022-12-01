using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models
{
    public class ProyectosCertificacion
    {
        public int Id { get; set; }
        public string nombre_contrato { get; set; }
        public string codigo_contrato { get; set; }
        public string nombre_proyecto { get; set; }
        public string codigo_proyecto { get; set; }

        public bool ProyectoCerrado { get; set; } = false;
        public bool ProyectoCertificable { get; set; } = false;
        public int? PortafolioId { get; set; }

        public int? UbicacionId { get; set; }

    }
}
