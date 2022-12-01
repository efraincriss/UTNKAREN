using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Dto
{
    public class ValidacionRequisitoDto : EntityDto
    {
        public int ColaboradorRequisitoId { get; set; }

        public int RequisitoId { get; set; }

        public bool CumpleBool { get; set; }

        public string Obligatorio { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string Cumple { get; set; }

        public DateTime? FechaEmision { get; set; }

        public DateTime? FechaCaducidad { get; set; }

        public int? ArchivoId { get; set; }

        public string Observacion { get; set; }

        public bool Vigente { get; set; }

        public bool Editable { get; set; }

        public string AplicaCaducidad { get; set; }

        public string TiempoVigenciaMaximo { get; set; }

        public string Responsable { get; set; }
    }
}
