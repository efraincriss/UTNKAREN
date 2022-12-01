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
    public class PorcentajeIncremento:  Entity
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
