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
    public class WbsTemp : Entity
    {
        [Obligado]
        [DisplayName("Oferta")]
        public int OfertaId { get; set; }

        public Oferta Oferta { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_inicial { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Fin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_final { get; set; }

        [Obligado]
        [DisplayName("Código Nivel")]
        [LongitudMayor(50)]
        public virtual string id_nivel_codigo { get; set; }


        [Obligado]
        [DisplayName("Código Padre")]
        [LongitudMayor(50)]
        public virtual string id_nivel_padre_codigo { get; set; }

        [DisplayName("Estado")] public virtual bool estado { get; set; } = false;


        [DisplayName("Observaciones")]
        [LongitudMayor(800)]
        public virtual string observaciones { get; set; }

        [Obligado]
        [DisplayName("Nombre Nivel")]
        [LongitudMayor(50)]
        public virtual string nivel_nombre { get; set; }


        [Obligado]
        [DisplayName("Es Actividad")]
        public virtual bool es_actividad { get; set; }

        [Obligado]
        public virtual bool vigente { get; set; } = true;
    }
}
