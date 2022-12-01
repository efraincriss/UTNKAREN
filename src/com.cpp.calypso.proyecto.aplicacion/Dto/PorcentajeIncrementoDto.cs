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
    [AutoMap(typeof(PorcentajeIncremento))]
    [Serializable]
    public class PorcentajeIncrementoDto: EntityDto
    {
        [LongitudMayor(100)]
        [Obligado]
        [DisplayName("Descripción")]
        public string descripcion { get; set; }


        [DisplayName("Valor")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "El valor debe ser >= a cero")]
        public decimal valor { get; set; }


        [DefaultValue(true)] public bool vigente { get; set; } = true;
    }
}
