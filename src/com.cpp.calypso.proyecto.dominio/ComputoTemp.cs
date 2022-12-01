using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ComputoTemp : Entity
    {
        [Obligado]
        [DisplayName("Wbs")]
        public int WbsId { get; set; }
        public virtual Wbs Wbs { get; set; }

        [Obligado]
        [DisplayName("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }


        [Obligado]
        [DisplayName("Cantidad")]
        public virtual decimal cantidad { get; set; }


        [DisplayName("Precio Unitario")]
        [DefaultValue(0.0)]
        public virtual decimal precio_unitario { get; set; }


        [DisplayName("Costo Total")]
        [DefaultValue(0.0)]
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
        public String precio_aplicarse { get; set; }

        [DisplayName("Codigo Primavera")]
        public string codigo_primavera { get; set; }


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
    }
}
