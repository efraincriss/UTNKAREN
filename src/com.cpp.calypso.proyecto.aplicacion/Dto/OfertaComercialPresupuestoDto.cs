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
    [AutoMap(typeof(OfertaComercialPresupuesto))]
    [Serializable]
   public class OfertaComercialPresupuestoDto: EntityDto
    {
        
        // Ofertas Comercial
        [Obligado]
        [DisplayName("OfertaComercial")]
        public int OfertaComercialId { get; set; }

        public virtual OfertaComercial OfertaComercial { get; set; }
        //  Presupuesto
     
        [DisplayName("Presupuesto")]
        public int? PresupuestoId { get; set; }

        public virtual Presupuesto Presupuesto { get; set; }


        [DisplayName("Requerimiento")]
        public int RequerimientoId { get; set; }

        public virtual Requerimiento Requerimiento { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Asignación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_asignacion { get; set; }


        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;
    }
}
