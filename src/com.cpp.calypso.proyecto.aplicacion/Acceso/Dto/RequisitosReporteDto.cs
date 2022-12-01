using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Dto
{
    public class RequisitosReporteDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Departamento { get; set; }
        public string Identificacion { get; set; }

        public string NombresCompletos { get; set; }

        public string Requisito { get; set; }
        public string TieneCaducidad { get; set; }

        public string Obligatorio { get; set; }
        public string Cumple { get; set; }

        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaCaducidad { get; set; }

        public string Estado { get; set; }
        public string fecha_registro { get; set; }
        public string fecha_caducidad { get; set; }
    }
}
