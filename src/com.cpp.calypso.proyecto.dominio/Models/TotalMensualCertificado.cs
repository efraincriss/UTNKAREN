using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
   public class TotalMensualCertificado
    {

        public int ProyectoId { get; set; }
        public decimal HH { get; set; }
        public decimal USD { get; set; }
        public DateTime FechaCertificado { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
    }
}
