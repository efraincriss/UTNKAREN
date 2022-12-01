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

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(Preciario))]
    [Serializable]
    public class PreciarioDto : EntityDto
    {

        [Obligado]
        [DisplayName("Contrato")]
        public int ContratoId { get; set; }

        public  Contrato Contrato { get; set; }
        [Obligado]
        [DisplayName("Fecha Desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_desde { get; set; }

        [Obligado]
        [DisplayName("Fecha Hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_hasta { get; set; }



        [Obligado]
        [DisplayName("Estado (Activo/Inactivo)")]
        [DefaultValue(true)]
        public bool estado { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        [DefaultValue(true)]
        public bool vigente { get; set; }


        public virtual ICollection<Contrato> Contratos { get; set; }
        public virtual List<DetallePreciario> DetallePreciarios { get; set; }
    }
}
