using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class AvanceObraExcel
    {
        public int ComputoId { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string UnidadMedida { get; set; }
        public int Color { get; set; }

        public Decimal CantidadEAC { get; set; }
        public Decimal CantidadAnterior { get; set; }
    }
}
