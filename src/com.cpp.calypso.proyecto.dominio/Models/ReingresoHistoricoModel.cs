using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class ReingresoHistoricoModel
    {
        public int ColaboradorId { get; set; } 
        public virtual string Cargo { get; set; }
         public virtual string FechaUltimoIngreso { get; set; }
        public virtual string Estado { get; set; }
        public virtual string MotivoBaja { get; set; }
        public virtual string FechaSalida { get; set; }

   }
}
