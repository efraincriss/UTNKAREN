using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
 
    [Serializable]
    public class Precipitacion : Entity
    {
        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

        public Proyecto Proyecto { get; set; }


        [DisplayName("Tipo Precipitacion")]
        public TipoPrecipitacion Tipo { get; set; }

        [DisplayName("Fecha Precipitación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [DisplayName("Hora inicio")]
        public TimeSpan Hora_inicio { get; set; }

        [DisplayName("Hora de finalización")]
        public TimeSpan Hora_fin { get; set; }

        [DisplayName("Cantidad")]
        public decimal CantidadDiaria { get; set; }

        [DisplayName("Cantidad")]
        public decimal CantidadAnterior { get; set; }

        [DisplayName("Cantidad")]
        public decimal CantidadAcumulada { get; set; }

        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;


    }
    public enum TipoPrecipitacion
    {
        [Description("Diurna")]
        Diurna = 1,

        [Description("Noctura")]
        Norturna = 2,
    }

}
