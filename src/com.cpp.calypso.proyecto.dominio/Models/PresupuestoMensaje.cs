using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
   public class PresupuestoMensaje
    {
        public string mensaje { get; set; }
        public List<String> errores { get; set;}
    }
}
