using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Dto
{
    public class InputAlertaVencimientoReporteDto
    {
        public string ApellidosNombres { get; set; }

        [DisplayName("Departamento")]
        public int DepartamentoId { get; set; }

        public int DiasVencimiento { get; set; }

        public string Identificacion { get; set; }

        public int[] RequisitosId { get; set; }
    }
}
