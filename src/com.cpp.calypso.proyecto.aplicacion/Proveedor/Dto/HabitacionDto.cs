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
using com.cpp.calypso.proyecto.dominio.Proveedor;
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto
{
    [AutoMap(typeof(Habitacion))]
    [Serializable]
    public class HabitacionDto  : EntityDto
    {
        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }

        [Obligado]
        [DisplayName("Número de Habitación")]
        [StringLength(50)]
        public string numero_habitacion { get; set; }


        [Obligado]
        [DisplayName("Tipo de Habitación")]
        public int TipoHabitacionId { get; set; }


        [Obligado]
        [DisplayName("Capacidad")]
        public int capacidad { get; set; }


        [Obligado]
        [DisplayName("Estado")]
        public bool estado { get; set; }


        [Obligado]
        [DisplayName("Aprobado")]
        public bool aprobado { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha de Aprobación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_aprobacion { get; set; }

        public string tipo_habitacion_nombre { get; set; }

        public string estado_nombre { get; set; }

        public int secuencial { get; set; }
    }
}
