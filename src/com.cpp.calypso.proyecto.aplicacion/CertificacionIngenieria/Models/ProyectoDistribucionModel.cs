using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models
{
    public class ProyectoDistribucionModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        public bool AplicaViatico { get; set; }
        public bool AplicaIndirecto { get; set; }
        public bool AplicaE500 { get; set; }
    }
}
