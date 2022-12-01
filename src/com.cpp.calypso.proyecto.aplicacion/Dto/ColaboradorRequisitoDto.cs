using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
	[AutoMap(typeof(ColaboradorRequisito))]
	[Serializable]
	public class ColaboradorRequisitoDto : EntityDto
    {
		[DisplayName("Colaborador")]
		public int ColaboradoresId { get; set; }
		public virtual Colaboradores Colaboradores { get; set; }

		[DisplayName("Requisito")]
		public int RequisitosId { get; set; }
		public virtual Requisitos Requisitos { get; set; }

        [DisplayName("Archivo")]
        public int? ArchivoId { get; set; }
        public virtual Archivo Archivo { get; set; }

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

       /* [DisplayName("Acción")]
        public int catalogo_accion_id { get; set; }*/
        
        [DisplayName("Activo")]
        public bool activo { get; set; } = true;

        public virtual string nombre_accion { get; set; }
    }
}
