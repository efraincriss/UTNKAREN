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
    [AutoMap(typeof(CurvasProyecto))]
    [Serializable]
    public class CurvasProyectoDto: EntityDto
    {
        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }
        public virtual Proyecto Proyecto { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha { get; set; }

        [DisplayName("Valor Previsto")]
        [DefaultValue(0.0)]
        public decimal valor_previsto { get; set; }

        [DisplayName("Valor Previsto Acumulado")]
        [DefaultValue(0.0)]
        public decimal valor_previsto_acumulado { get; set; }


        [DisplayName("Valor Real")]
        [DefaultValue(0.0)]
        public decimal valor_real { get; set; }

        [DisplayName("Valor Real  Acumulado")]
        [DefaultValue(0.0)]
        public decimal valor_real_acumulado { get; set; }
    }
}
