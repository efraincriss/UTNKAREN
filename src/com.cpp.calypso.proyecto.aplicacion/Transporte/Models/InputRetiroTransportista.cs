using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml.Utils;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Models
{
    public class InputRetiroTransportista
    {
        [Required]
        public DateTime fecha { get; set; }

        public bool check { get; set; }

        public int conductor_asignado_id { get; set; }

        [Required]
        public int tipoComidaId { get; set; }


    }
}
