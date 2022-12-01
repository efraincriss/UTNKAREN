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
using Abp.AutoMapper;
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.aplicacion
{

    [AutoMap(typeof(WbsOferta))]
    [Serializable]
    public class WbsOfertaDto :EntityDto
    {
        [Obligado]
        [DisplayName("Oferta")]

        public int OfertaId { get; set; }


        [DisplayName("Area")]
        public int AreaId { get; set; }


        [DisplayName("Disciplina")]
        public int DisciplinaId { get; set; }


        [DisplayName("Elemento")]
        public int ElementoId{ get; set; }


        [DisplayName("Actividad")]
        public int ActividadId { get; set; }

        [Obligado]
        [DisplayName("Estado del Contrato")]
        public WbsOferta.EstadoEnum estado { get; set; }

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
        public virtual List<Computo> Computos { get; set; }

        // para sacar guardar los nombres

        public virtual string nombrearea { get; set; }
        public virtual string nombrediciplina { get; set; }
        public virtual string nombreelemento { get; set; }
        public virtual string nombreactividad { get; set; }
        public virtual string cliente { get; set; }
        public virtual string proyecto { get; set; }
        public virtual DateTime? fecha_oferta { get; set; }

        public virtual string nombre_area { get; set; }
        public virtual string nombre_disciplina { get; set; }
        public virtual string nombre_elemento { get; set; }
        public virtual string nombre_actividad { get; set; }


        public enum EstadoEnum
        {
            Activo = 1,
            Inactivo = 2
        }


    }
}


