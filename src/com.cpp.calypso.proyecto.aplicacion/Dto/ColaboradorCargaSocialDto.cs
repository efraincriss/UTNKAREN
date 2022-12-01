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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ColaboradorCargaSocial))]
    [Serializable]
    public class ColaboradorCargaSocialDto : EntityDto
    {
		[DisplayName("Colaborador")]
		public int ColaboradoresId { get; set; }
		public virtual Colaboradores Colaboradores { get; set; }

		[CanBeNull]
		[DisplayName("Parentesco")]
		public int parentesco_id { get; set; }

		[CanBeNull]
		[DisplayName("Tipo de identificacion")]
		public int idTipoIdentificacion { get; set; }

        [CanBeNull]
        [MaxLength(25)]
        [DisplayName("Nro Identificación:")]
        public string nro_identificacion { get; set; }

        [CanBeNull]
		[DisplayName("Prmer Apellido:")]
		public string primer_apellido { get; set; }

		[CanBeNull]
		[DisplayName("Segundo Apellido:")]
		public string segundo_apellido { get; set; }

		[CanBeNull]
		[DisplayName("Nombres")]
		public string nombres { get; set; }

        [CanBeNull]
        [DisplayName("Nombres")]
        public string nombres_apellidos { get; set; }

        [DataType(DataType.Date)]
		[DisplayName("Fecha Nacimiento")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? fecha_nacimiento { get; set; }

		[DisplayName("Género")]
		public int idGenero { get; set; }

		[DisplayName("Nacionalidad")]
		public int PaisId { get; set; }
		public virtual Pais Pais { get; set; }

		[DisplayName("País de Nacimiento")]
		public int pais_nacimiento { get; set; }

		[DisplayName("Estado Civil")]
		public int estado_civil { get; set; }

		[DataType(DataType.Date)]
		[DisplayName("Fecha Matrimonio")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? fecha_matrimonio { get; set; }
	
		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;

        [DisplayName("Es sustituto")]
        public bool por_sustitucion { get; set; } = false;

        [DisplayName("Viene Registro Civil")]
        public bool viene_registro_civil { get; set; } = false;
                              
        public virtual string apellidos { get; set; }
		public virtual string tipoIdentificacion { get; set; }
		public virtual string parentesco { get; set; }
        public virtual int nro { get; set; }


        //variables para discapacidad
        public virtual bool? discapacidad { get; set; }
        public virtual int? catalogo_tipo_discapacidad_id { get; set; }
        public virtual int? catalogo_porcentaje_id { get; set; }
        public virtual string nombre_dis { get; set; }
        public virtual string nombre_genero { get; set; }
        public virtual string nombre_sustituto { get; set; }

       

    }
}
