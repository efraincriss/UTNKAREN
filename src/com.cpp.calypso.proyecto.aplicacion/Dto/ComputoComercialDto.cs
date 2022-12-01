using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{

    [AutoMap(typeof(ComputoComercial))]
    [Serializable]
    public class ComputoComercialDto : EntityDto
    {
        [Obligado]
        [DisplayName("Wbs Comercial")]
        public int WbsComercialId { get; set; }
        public virtual WbsComercial WbsComercial { get; set; }


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

        //Nombres
        public virtual string nombrearea { get; set; }
        public virtual string nombrediciplina { get; set; }
        public virtual string nombreelemento { get; set; }
        public virtual string nombreactividad { get; set; }
        public virtual string nombreitem { get; set; }

        // Campos virtuales
        public virtual int area { get; set; }
        public virtual int diciplina { get; set; }
        public virtual int elemento { get; set; }
        public virtual int actividad { get; set; }
        public virtual string actividad_nombre { get; set; }
        public virtual string item_codigo { get; set; }
        public virtual string item_nombre { get; set; }
        public virtual string item_padre_nombre { get; set; }
        public virtual string item_padre_codigo { get; set; }
        public virtual string oferta { get; set; }
        //virtuales cabecera
        public virtual string clase { get; set; }
        public virtual string monto_total { get; set; }
        public virtual string monto_construccion { get; set; }
        public virtual string monto_ingenieria { get; set; }
        public virtual string monto_procura { get; set; }
        public virtual decimal total_pu { get; set; }
        public virtual decimal total_pu_aui { get; set; }
        public virtual string nombre_unidad { get; set; }

        public virtual bool diferente { get; set; }

        [DisplayName("Codigo Primavera")]
        public string codigo_primavera { get; set; }

        public virtual decimal cantidad_acumulada_anterior { get; set; }

        [DisplayName("Código Item Alterno")]
        public string codigo_item_alterno { get; set; }


        public virtual string codigo_especialidad { get; set; }
        public virtual string codigo_grupo { get; set; }
    }
}
