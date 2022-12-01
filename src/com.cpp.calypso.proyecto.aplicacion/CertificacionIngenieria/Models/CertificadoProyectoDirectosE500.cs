using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models
{
    public class CertificadoProyectoDirectosE500
    {

        public int CertificadoProyectoId { get; set; }

        public int ProyectoId { get; set; }

        public decimal MontoActualCertificado { get; set; }
        public decimal HorasActualCertificado { get; set; }
    }
}
