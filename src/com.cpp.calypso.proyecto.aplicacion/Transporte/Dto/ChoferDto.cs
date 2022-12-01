using Abp.Application.Services.Dto;
using Abp.AutoMapper;
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

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Dto
{

    [AutoMap(typeof(Chofer))]
    [Serializable]
    public class ChoferDto : EntityDto
    {
        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }

        [Obligado]
        [DisplayName("Tipo Identificación")]
        public int TipoIdentificacionId { get; set; }

        [DisplayName("Numero Identificación")]
        [StringLength(20)]
        public string NumeroIdentificacion { get; set; }

        [DisplayName("Apellidos Nombres")]
        public string ApellidosNombres { get; set; }

        [DisplayName("Nombres")]
        public string Nombres { get; set; }


        [DisplayName("Apellidos")]
        public string Apellidos { get; set; }


        [Obligado]
        [DisplayName("Genero")]
        public int GeneroId { get; set; }

        /* [Obligado]
        [DisplayName("Estado Civil")]
        public int EstadoCivilId { get; set; }
        */

        [DataType(DataType.Date)]
        [DisplayName("Fecha Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }

        [DisplayName("Celular")]
        [StringLength(20)]
        public string Celular { get; set; }

        [CanBeNull]
        [DisplayName("Mail")]
        [StringLength(100)]
        public string Mail { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public ChoferEstado Estado { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Estado")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaEstado { get; set; }

        public virtual string  NombreTipoIdentificacion { get; set; }
        public virtual string NombreEstado { get; set; }
        public virtual string NombreProveedor { get; set; }


    }
}
