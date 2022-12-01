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

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(DetalleAvanceIngenieria))]
    [Serializable]
    public class DetalleAvanceIngenieriaDto : EntityDto
    {
        [Obligado]
        [DisplayName("Avance Ingeniería")]
        public int AvanceIngenieriaId { get; set; }


        public AvanceIngenieria AvanceIngenieria { get; set; }



        [Obligado]
        [DisplayName("Item")]
        public int ComputoId { get; set; }


        public virtual Computo Computo { get; set; }


        [Obligado]
        [DisplayName("Cantidad Horas")]
        public virtual decimal cantidad_horas { get; set; }

        [Obligado]
        [DisplayName("Cantidad acumulada anterior")]
        public virtual decimal cantidad_acumulada_anterior { get; set; }

        [Obligado]
        [DisplayName("Cantidad acumulada")]
        public virtual decimal cantidad_acumulada { get; set; }


        [Obligado]
        public bool vigente { get; set; } = true;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Presentación")]
        public virtual DateTime fecha_real { get; set; }

        [Obligado]
        [DisplayName("Precio Unitario")]
        public virtual decimal precio_unitario { get; set; }

        [Obligado]
        [DisplayName("Valor Real")]
        public decimal valor_real { get; set; }

        [DisplayName("Ingreso Acumulado")]
        public decimal ingreso_acumulado { get; set; }

        [DisplayName("Cálculo Diario")]
        public virtual decimal calculo_diario { get; set; }

        [DisplayName("Cálculo Anterior")]
        public virtual decimal calculo_anterior { get; set; }

        public bool estacertificado { get; set; } = false;
        // Virtuales
        public string codigo_item { get; set; }

        public string descripcion_item { get; set; }

        public virtual string fechar { get; set; }

        public virtual decimal  horas_presupuestadas { get; set; } //cantidad de computo
    }
}
