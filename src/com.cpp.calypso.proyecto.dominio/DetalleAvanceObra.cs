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
    public class DetalleAvanceObra : Entity
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

        [Obligado]
        public bool vigente { get; set; }


        [DisplayName("Ingreso Acumulado")]
        public virtual decimal ingreso_acumulado { get; set; }

        [DisplayName("Cálculo Diario")]
        public virtual decimal calculo_diario { get; set; }

        [DisplayName("Cálculo Anterior")]
        public virtual decimal calculo_anterior { get; set; }

        public bool estacertificado { get; set; } = false;
    }
}
