using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto
{
    public class EspacioLibreDto
    {

        public int Id { get; set; }
        public string codigo_espacio { get; set; }

        public bool ocupado { get; set; }

        public string nombres_colaborador { get; set; }
    }
}
