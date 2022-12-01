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

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(ComputoPresupuesto))]
    [Serializable]
    public class ComputoPresupuestoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Wbs Presupuesto")]
        public int WbsPresupuestoId { get; set; }
        public virtual WbsPresupuesto WbsPresupuesto { get; set; }


        [Obligado]
        [DisplayName("Item")]
        public virtual int ItemId { get; set; }

        public virtual Item Item { get; set; }


        [Obligado]
        [DisplayName("Cantidad")]
        public virtual decimal cantidad { get; set; }


        [DefaultValue(0.0)]
        [DisplayName("Precio Unitario")]
        public virtual decimal precio_unitario { get; set; }


        [DefaultValue(0.0)]
        [DisplayName("Costo Total")]
        public virtual decimal costo_total { get; set; }

        [Obligado]
        [DisplayName("Estado (Activo/ Inactivo)")]
        public virtual bool estado { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public virtual bool vigente { get; set; }

        [DisplayName("Precio Base")]
        [DefaultValue(0.0)]
        public decimal precio_base { get; set; }

        [DisplayName("Precio Incrementado")]
        [DefaultValue(0.0)]
        public decimal precio_incrementado { get; set; }
        [DisplayName("Precio Ajustado")]
        [DefaultValue(0.0)]
        public decimal precio_ajustado { get; set; }


        [DisplayName("Tipo de precio")]
        public string precio_aplicarse { get; set; }

        [DisplayName("Fecha registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_registro { get; set; }

        [DisplayName("Fecha Actualización")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_actualizacion { get; set; }


        [DisplayName("Cantidad EAC")]
        public virtual decimal cantidad_eac { get; set; }


        [DisplayName("Fecha EAC")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_eac { get; set; }

        [DisplayName("Presupuestado")]
        public virtual bool presupuestado { get; set; } = true;

        [DisplayName("Código Item Alterno")]
        public string codigo_item_alterno { get; set; }

        public ComputoPresupuesto.TipoCambioComputo? Cambio { get; set; }


        public int? ComputoId { get; set; }


        // Virtual
        public virtual string wbs_actividad { get; set; }
        public virtual string item_codigo { get; set; }
        public virtual string item_nombre { get; set; }

        public virtual string item_padre_nombre { get; set; }
        public virtual string item_padre_codigo { get; set; }
        public virtual int item_UnidadId { get; set; }
        public virtual int item_GrupoId { get; set; }
        public virtual string nombre_unidad { get; set; }
        public virtual bool diferente { get; set; }
        public virtual decimal total_pu { get; set; }
        public virtual decimal total_pu_aui { get; set; }

        public virtual string codigo_especialidad { get; set; }
        public virtual string codigo_grupo { get; set; }


    }
}
