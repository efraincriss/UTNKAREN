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
using com.cpp.calypso.proyecto.dominio.Accesos;
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Dto
{
    [AutoMap(typeof(ConsultaPublica))]
    [Serializable]
    public class ConsultaPublicaDto : EntityDto
    {
         /* [Obligado]
        [DisplayName("Ciudad Trabajo")]
        public int CiudadTrabajoId { get; set; }
        */

        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

        [Obligado]
        [DisplayName("Tipo Identificación")]
        public int TipoIdentificacionId { get; set; }

        [Obligado]
        [DisplayName("Identificación")]
        [StringLength(20)]
        public string identificacion { get; set; }

        [Obligado]
        [DisplayName("Nombres")]
        [StringLength(200)]
        public string nombres_completos { get; set; }

        [CanBeNull]
        [DisplayName("Condición Cedulado")]
        [StringLength(200)]
        public string condicion_cedulado { get; set; }


        [CanBeNull]
        [DisplayName("Calle")]
        [StringLength(200)]
        public string calle { get; set; }

        [CanBeNull]
        [DisplayName("Código Error")]
        [StringLength(3)]
        public string codigo_error { get; set; }
        
        [CanBeNull]
        [DisplayName("Conyugue")]
        [StringLength(200)]
        public string conyugue { get; set; }

        [CanBeNull]
        [DisplayName("Domicilio")]
        [StringLength(200)]
        public string domicilio { get; set; }


        [CanBeNull]
        [DisplayName("Error")]
        [StringLength(200)]
        public string error { get; set; }


        [CanBeNull]
        [DisplayName("Estado Civil")]
        [StringLength(200)]
        public string estado_civil { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Cedulación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_cedulacion { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Fallecimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_fallecimiento { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Matrimonio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_matrimonio { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_nacimiento { get; set; }


        [CanBeNull]
        [DisplayName("Instrucción")]
        [StringLength(200)]
        public string instruccion { get; set; }

        [CanBeNull]
        [DisplayName("Lugar Nacimiento")]
        [StringLength(200)]
        public string lugar_nacimiento { get; set; }

        [CanBeNull]
        [DisplayName("Nacionalidad")]
        [StringLength(200)]
        public string nacionalidad { get; set; }

        [CanBeNull]
        [DisplayName("Numero Casa")]
        [StringLength(200)]
        public string numero_casa { get; set; }

        [CanBeNull]
        [DisplayName("Profesión")]
        [StringLength(200)]
        public string profesion { get; set; }

        [CanBeNull]
        [DisplayName("Sexo")]
        [StringLength(200)]
        public string sexo { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Consulta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_consulta { get; set; }

        [DisplayName("Archivo Pdf")]
        public int? ArchivoPdfId { get; set; }

       // public int  PaisTrabajoId { get; set; }

       //public int  ProvinciaTrabajoId { get; set; }

        public string tipo_identificacion_nombre { get; set; }

        public virtual Archivo documentacion_subida { get; set; }

        public string fotografia { get; set; }
        public int? usuarioConsumoId { get; set; }
        public string tipoRC { get; set; } //Biometrico o Demografico
    }
}
