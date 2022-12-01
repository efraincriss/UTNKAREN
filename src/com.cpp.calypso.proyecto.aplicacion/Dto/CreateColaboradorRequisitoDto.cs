using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ColaboradorRequisito))]
    [Serializable]
    public class CreateColaboradorRequisitoDto : EntityDto
    {
        [DisplayName("Colaborador")]
        public int ColaboradoresId { get; set; }
 
        [DisplayName("Requisito")]
        public int RequisitosId { get; set; }

        [DisplayName("Archivo")]
        public int? ArchivoId { get; set; }

        [DisplayName("Cumple")]
        public bool cumple { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Emision")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_emision { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Caducidad")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_caducidad { get; set; }

        [CanBeNull]
        [DisplayName("Observacion")]
        public string observacion { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;
    }
}
