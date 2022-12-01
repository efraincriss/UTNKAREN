using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models
{
    public class E500Distribucion
    {
        public string key { get; set; }
        public int Id { get; set; }

        public int ProyectoADistribuirId { get; set; }
        public string nombreProyecto { get; set; }

        public decimal Horas { get; set; }

    }
}
