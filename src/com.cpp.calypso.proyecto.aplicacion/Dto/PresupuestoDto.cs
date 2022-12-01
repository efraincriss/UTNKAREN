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
    [AutoMap(typeof(Presupuesto))]
    [Serializable]
    public class PresupuestoDto : EntityDto
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
        public Presupuesto.ClasePresupuesto? Clase { get; set; }


        [DisplayName("Descripción")]
        [LongitudMayorAttribute(800)]
        public virtual string descripcion { get; set; }

        [Obligado]
        [DisplayName("Versión")]
        [LongitudMayorAttribute(50)]
        public virtual string version { get; set; }

        [Obligado]
        [DisplayName("Código")]
        [LongitudMayorAttribute(50)]
        public string codigo { get; set; }

        [DisplayName("Alcance")]
        public string alcance { get; set; }

        [Obligado]
        [DisplayName("Definitiva")]
        public virtual bool es_final { get; set; }


        public Presupuesto.EstadoAprobacion estado_aprobacion { get; set; }


        public Presupuesto.EstadoEmision estado_emision { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_registro { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_actualizacion { get; set; }

        public int OrigenDatosId { get; set; }

        [ForeignKey("OrigenDatosId")]
        public Catalogo OrigenDatos { get; set; }

        [Obligado]
        public bool vigente { get; set; } = true;

        public decimal monto_ingenieria { get; set; }

        public decimal monto_construccion { get; set; }

        public decimal monto_suministros { get; set; }

        public decimal monto_subcontratos { get; set; }
        public decimal monto_total { get; set; } = 0;
        [Obligado]
        [DisplayName("Computo Completo")]
        public bool computo_completo { get; set; } = false;

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


        [ForeignKey("Catalogo")]
        [DisplayName("Origen")]
        public int? origen { get; set; }
        public virtual Catalogo Catalogo { get; set; }


        public decimal descuento { get; set; }

        [DisplayName("Justificación Descuento")]
        public string justificacion_descuento { get; set; }

        [DisplayName("Asunto Mail")]
        public string asuntoCorreo { get; set; }

        [DisplayName("Descripcion Mail")]
        public string descripcionCorreo { get; set; }

        public virtual string NombreEstadoAprobacion { get; set; }

        public virtual string NombreEstadoEmision { get; set; }

        public virtual string NombreClase { get; set; }

        public virtual string codigo_requerimiento { get; set; }
        public virtual string codigo_proyecto { get; set; }

        public virtual string codigo_oferta { get; set; }
        public virtual string estado_oferta { get; set; }
        public virtual string formatFechaActualizacionPresupuesto { get; set; }
        public virtual string formatFechaUltimoEnvioOferta { get; set; }
    }
}
