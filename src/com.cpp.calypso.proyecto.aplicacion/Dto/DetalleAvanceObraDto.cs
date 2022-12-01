using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(DetalleAvanceObra))]
    [Serializable]
    public class DetalleAvanceObraDto : EntityDto
    {
        [Obligado]
        [DisplayName("Avance de obra")]
        public int AvanceObraId { get; set; }

        public AvanceObra AvanceObra { get; set; }

        [Obligado]
        [DisplayName("Computo")]
        public int ComputoId { get; set; }


        public virtual Computo Computo { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de registro")]
        public virtual DateTime? fecha_registro { get; set; }

        [Obligado]
        [DisplayName("Cantidad")]
        public virtual decimal cantidad_diaria { get; set; }


        [Obligado]
        [DisplayName("Cantidad acumulada anterior")]
        public virtual decimal cantidad_acumulada_anterior { get; set; }

        [Obligado]
        [DisplayName("Cantidad acumulada")]
        public virtual decimal cantidad_acumulada { get; set; }



        [Obligado]
        [DisplayName("Precio Unitario")]
        public virtual decimal precio_unitario { get; set; }


        [Obligado]
        [DisplayName("Total")]
        public virtual decimal total { get; set; }

        public virtual Wbs Wbs { get; set; }

        [Obligado]
        public virtual bool vigente { get; set; }




        [DisplayName("Ingreso Acumulado")]
        public virtual decimal ingreso_acumulado { get; set; }

        [DisplayName("Cálculo Diario")]
        public virtual decimal calculo_diario { get; set; }

        [DisplayName("Cálculo Anterior")]
        public virtual decimal calculo_anterior { get; set; }


        public bool estacertificado { get; set; } = false;
        // Campos virtuales
        public virtual string item_codigo { get; set; }

        public virtual string nombre_item { get; set; }
        public virtual string nombre_padre { get; set; }

        public virtual bool presupuestado { get; set; }

        public virtual decimal budget { get; set; }

        public virtual decimal cantidad_eac { get; set; }
        public virtual string fechar { get; set; }
        public virtual decimal cantidad_presupuestada    {      get; set;        }
        public virtual string codigoOferta { get; set; }
    }
}