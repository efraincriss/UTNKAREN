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
    [AutoMap(typeof(ObraAdicional))]
    [Serializable]
    public class ObraAdicionalDto : EntityDto
    {
        [Obligado]
        [DisplayName("Wbs")]
        public int WbsId { get; set; }

        public virtual Wbs Wbs { get; set; }

        [Obligado]
        [DisplayName("Avance de obra")]
        public int AvanceObraId { get; set; }

        public AvanceObra AvanceObra { get; set; }

        [Obligado]
        [DisplayName("Item")]
        public virtual int ItemId { get; set; }

        public Item Item { get; set; }


        [Obligado]
        [DisplayName("Cantidad")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "El valor debe ser >= a cero")]
        public virtual decimal cantidad { get; set; }


        [Obligado]
        [DisplayName("Precio Unitario")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "El valor debe ser >= a cero")]
        public decimal precio_unitario { get; set; }


        [Obligado]
        [DisplayName("Costo total")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "El valor debe ser >= a cero")]
        public decimal costo_total { get; set; }


        [Obligado]
        [DisplayName("Precio Base")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "El valor debe ser >= a cero")]
        public decimal precio_base { get; set; }


        [Obligado]
        [DisplayName("Precio Incrementado")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "El valor debe ser >= a cero")]
        public decimal precio_incrementado { get; set; }


        [Obligado]
        [DisplayName("Precio Ajustado")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "El valor debe ser >= a cero")]
        public decimal precio_ajustado { get; set; }

        [Obligado]
        [DisplayName("Tipo de Precio")]
        [LongitudMayor(10)]
        public string tipo_precio { get; set; }

        public virtual bool vigente { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public virtual int estado { get; set; }


        // Campos virtuales

        public string item_codigo { get; set; }

        public virtual string nombre_item { get; set; }

        public virtual string nombre_padre { get; set; }

    }
}
