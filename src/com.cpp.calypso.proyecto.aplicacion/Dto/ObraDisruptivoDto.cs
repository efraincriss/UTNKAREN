using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(ObraDisruptivo))]
    [Serializable]
    public class ObraDisruptivoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

        [JsonIgnore]
        public Proyecto Proyecto { get; set; }

        [Obligado]
        [ForeignKey("Catalogo")]
        [DisplayName("Tipo de Recurso")]
        public int TipoRecursoId { get; set; }

        public virtual Catalogo Catalogo { get; set; }


        [Obligado]
        [DisplayName("Tipo improductividad")]
        public int tipo_improductividad { get; set; }


        [DisplayName("Hora inicio")]
        public TimeSpan hora_inicio { get; set; }

        [DisplayName("Hora de finalización")]
        public TimeSpan hora_fin { get; set; }


        [Obligado]
        [DisplayName("Número de horas")]
        public decimal numero_horas { get; set; }

        [DisplayName("Número de Recursos")]
        public int numero_recursos { get; set; }

        [Obligado]
        [DisplayName("Número de horas hombre")]
        public decimal numero_horas_hombres { get; set; }

        [DisplayName("Observaciones")]
        [LongitudMayor(800)]
        public string observaciones { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_inicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Fin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_fin { get; set; }

        public bool vigente { get; set; } = true;

        public int porcentaje_disruptivo { get; set; } = 100;


        public virtual string nombre_improductividad { get; set; }
        public virtual string nombre_recurso { get; set; }
        public virtual int numero_dias { get; set; }
        public virtual decimal dias_real { get; set; }
    }
}
