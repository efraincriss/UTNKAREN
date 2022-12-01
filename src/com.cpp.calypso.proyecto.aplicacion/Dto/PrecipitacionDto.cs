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

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{

    [AutoMap(typeof(Precipitacion))]
    [Serializable]
    public class PrecipitacionDto : EntityDto
    {

        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

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


        public string nombreproyecto { get; set; }
        public string nombretipo { get; set; }
        public string horainicioformat { get; set; }
        public string horafinformat { get; set; }
        public string fechaformat { get; set; }

    }
}
