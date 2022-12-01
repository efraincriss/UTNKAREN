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
    [AutoMap(typeof(ProyectoObservacion))]
    [Serializable]
    public class ProyectoObservacionDto : EntityDto
    {
        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

        public virtual Proyecto Proyecto { get; set; }

        [Obligado]
        [DisplayName("Observacion")]
        public string Observacion { get; set; }

        [DisplayName("Vigente")] public bool vigente { get; set; } = true;

        [DisplayName("Fecha de observación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaObservacion { get; set; }

        [DisplayName("Tipo de Comentario")]
        public TipoComentario Tipo { get; set; }

        [Obligado]
        [DisplayName("Tipo de Observación")]
        public int TipoObservacionId { get; set; }


        public virtual string NombreTipoObservacion { get; set; }
        public virtual string NombreProyecto { get; set; }
        public virtual string FormatFecha { get; set; }
        public virtual string Codigo { get; set; }

    }
}
