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
    public class ProyectoObservacion : Entity
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
        public TipoComentario Tipo{ get; set; }


        [Obligado]
        [DisplayName("Tipo de Observación")]
        public int TipoObservacionId { get; set; }
        public Catalogo TipoObservacion { get; set; }

    }
    public enum TipoComentario
    {
        [Description("Observación")]
        Observacion = 1,

        [Description("Actividades Realizadas")]
        ActividadRealizada = 2,

        [Description("Actividades Programadas")]
        ActividadProgramada = 3,
    }

}
