using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    public class RequisitoAccesoDto : EntityDto
    {
        public string RequisitoNombre { get; set; }

        public bool Cumple { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaVencimiento { get; set; }

        public bool obligatorio { get; set; }

        public int? ArchivoId { get; set; }
    }
}
