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
    public class WbsOferta : Entity
    { 
        [Obligado]
        [DisplayName("Oferta")]
         public int OfertaId { get; set; }

        
        [DisplayName("Area")]
        public int AreaId { get; set; }


        [DisplayName("Disciplina")]
        public int DisciplinaId { get; set; }


        [DisplayName("Elemento")]
        public int ElementoId { get; set; }


        [DisplayName("Actividad")]
        public int ActividadId { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public EstadoEnum estado { get; set; }

        [LongitudMayorAttribute(800)]
        [DisplayName("Observaciones")]
        public String observaciones { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha de inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_inicio { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha de finalización")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_fin { get; set; }

        [Obligado]
        [DisplayName("Es estructura de wbs")]
        public bool es_estructura { get; set; } = false;



        [Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; }
        public virtual Oferta Oferta { get; set; }
     
        

        public enum EstadoEnum
        {
            Activo = 1,
            Inactivo = 2
        }
    }
}
