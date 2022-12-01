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
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(ComputoTemp))]
    [Serializable]
    public class ComputoTempDto : EntityDto
    {
        [Obligado]
        [DisplayName("Wbs")]
        public int WbsId { get; set; }
        public virtual Wbs Wbs { get; set; }


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
        public String precio_aplicarse { get; set; }


        [JsonIgnore]
        public virtual ICollection<Item> Items { get; set; }

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
    }
}
