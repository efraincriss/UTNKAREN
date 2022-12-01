using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.RecursosHumanos.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public int[] selectComidas { get; set; }
        public int[] selectTransporte { get; set; }
        public bool Alimentacion { get; set; }
        public bool Hospedaje { get; set; }
        public bool Transporte { get; set; }
    }
}
