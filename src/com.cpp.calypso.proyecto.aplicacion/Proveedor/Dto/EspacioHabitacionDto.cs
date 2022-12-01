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
using com.cpp.calypso.proyecto.dominio.Proveedor;
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto
{
    [AutoMap(typeof(EspacioHabitacion))]
    [Serializable]
    public class EspacioHabitacionDto : EntityDto
    {
        [Obligado]
        [DisplayName("Habitación")]
        public int HabitacionId { get; set; }


        [Obligado]
        [DisplayName("Código del Espacio")]
        [StringLength(50)]
        public string codigo_espacio { get; set; }


        [Obligado]
        [DisplayName("Estado")]
        public bool estado { get; set; }

        [Obligado] [DisplayName("Activo")]
        public bool activo { get; set; } = true;

        public string tipo_habitacion_nombre { get; set; }

        public string numero_habitacion { get; set; }

        public string estado_nombre { get; set; }

        public string activo_nombre { get; set; }

        public string proveedor_razon_social { get; set; }

        public string capacidad_habitacion { get; set; }
        public virtual int capacidadHabitacionConfig { get; set; }
        public virtual int EspaciosHabitacionConfig { get; set; }

        public int secuencial { get; set; }

    }
}
