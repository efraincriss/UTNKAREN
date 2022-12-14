using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ColaboradorCargaSocial : Entity
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

    }
}
