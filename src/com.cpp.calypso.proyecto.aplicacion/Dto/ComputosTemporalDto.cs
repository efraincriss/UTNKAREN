using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(ComputosTemporal))]
    [Serializable]
    public class ComputosTemporalDto : EntityDto
    {
        [Obligado]
        [DisplayName("Computo")]
        public int id_computo { get; set; }

        [Obligado]
        [DisplayName("Oferta Original")]
        public int OfertaId { get; set; }
        public virtual Oferta Oferta { get; set; }

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
        public decimal cantidad { get; set; }

        [DisplayName("Precio Unitario")]
        [DefaultValue(0.0)]
        public decimal precio_unitario { get; set; }

        [DisplayName("Costo Total")]
        [DefaultValue(0.0)]
        public decimal costo_total { get; set; }

        [Obligado]
        [DisplayName("Estado (Activo/ Inactivo)")]
        public bool estado { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; }

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
        public decimal cantidad_eac { get; set; }

        [DisplayName("Fecha EAC")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_eac { get; set; }

        [DisplayName("Presupuestado")]
        public bool presupuestado { get; set; } = true;

        //virtual
        public virtual string actividad_nombre { get; set; }
        public virtual string item_codigo { get; set; }
        public virtual string item_nombre { get; set; }
        public virtual string item_padre_nombre { get; set; }
        public virtual string item_padre_codigo { get; set; }
        public virtual decimal total_pu { get; set; }
        public virtual decimal total_pu_aui { get; set; }
        public virtual bool diferente { get; set; }
    }
}
