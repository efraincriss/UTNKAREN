using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(PresupuestoTemp))]
    [Serializable]
    public class PresupuestoTempDto : EntityDto
    {
        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

        public virtual Proyecto Proyecto { get; set; }

        [Obligado]
        [DisplayName("Requerimiento")]
        public int RequerimientoId { get; set; }

        public virtual Requerimiento Requerimiento { get; set; }


        [DisplayName("Clase")]
        public PresupuestoTemp.ClaseOferta? ClaseId { get; set; }


        [DisplayName("Descripción")]
        [LongitudMayor(800)]
        public virtual string descripcion { get; set; }

        [Obligado]
        [DisplayName("Versión")]
        [LongitudMayor(50)]
        public virtual string version { get; set; }

        [Obligado]
        [DisplayName("Código")]
        [LongitudMayor(50)]
        public string codigo { get; set; }

        [DisplayName("Alcance")]
        public string alcance { get; set; }

        [Obligado]
        [DisplayName("Definitiva")]
        public virtual bool es_final { get; set; }


        public PresupuestoTemp.EstadoAprobacion estado_aprobacion { get; set; }


        public PresupuestoTemp.EstadoEmision estado_emision { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_registro { get; set; }


        public int OrigenDatosId { get; set; }

        [ForeignKey("OrigenDatosId")]
        public Catalogo OrigenDatos { get; set; }

        [Obligado]
        public bool vigente { get; set; } = true;

        public decimal monto_ingenieria { get; set; }

        public decimal monto_construccion { get; set; }

        public decimal monto_suministros { get; set; }

        public string GetDisplayName(Enum enumValue)
        {
            if (!Enum.IsDefined(enumValue.GetType(), enumValue))
            {
                return " ";
            }
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }

        public virtual string NombreEstadoAprobacion { get; set; }

        public virtual string NombreEstadoEmision { get; set; }

        public virtual string NombreClase { get; set; }
    }
}
