using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto
{
    public class CategoriasEncargadoExcelModel
    {
        public int Item { get; set; }

        public string CodigoCategoria { get; set; }

        public string Cargo { get; set; }

        public decimal SueldoActual { get; set; }

        public decimal SueldoNuevo { get; set; }
    }
}
