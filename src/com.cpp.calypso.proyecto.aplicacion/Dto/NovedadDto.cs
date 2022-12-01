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
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(Novedad))]
    [Serializable]
    public class NovedadDto : EntityDto
    {
        [Obligado]
        [DisplayName("Requerimiento")]
        public int RequerimientoId { get; set; }


        public virtual Requerimiento Requerimiento { get; set; }


        [Obligado]
        [DisplayName("Problema")]
        [LongitudMayor(800)]
        public string descripcion { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha de la Novedad")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_novedad { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha de la Solución")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_solucion { get; set; }

        [DisplayName("Solucionada? (Si/No)")]
        public bool solucionada { get; set; } = false;


      
        [CanBeNull]
        [DisplayName("Solución")]
        [LongitudMayor(800)]
        public string solucion { get; set; }


        [DisplayName("Versión")]
        [LongitudMayor(15)]
        public string version { get; set; }

        [Obligado]
        public bool vigente { get; set; }
    }
}
