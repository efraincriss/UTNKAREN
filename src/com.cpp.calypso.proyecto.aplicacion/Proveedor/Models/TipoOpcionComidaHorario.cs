using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Models
{
    public class TipoOpcionComidaHorario
    {
        public int tipoOpcionComidaId { get; set; }
        public string nombreTipoOpcionComida { get; set; }

        public DateTime fechaHorarioInicio { get; set; }
        public DateTime fechaHorarioFin { get; set; }
        public TimeSpan horarioInicio { get; set; }
        public TimeSpan horarioFin { get; set; }
        public string formathorarioInicio { get; set; }
        public string formathorarioFin { get; set; }
    }
}
