using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{

    [Serializable]
    public class WbsComercial : Entity
    {
        [Obligado]
        [DisplayName("Oferta")]
        public int OfertaComercialId { get; set; }

        public OfertaComercial OfertaComercial { get; set; }

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
         public virtual string id_nivel_codigo { get; set; }


        [Obligado]
        [DisplayName("Código Padre")]
        public virtual string id_nivel_padre_codigo { get; set; }

        [DisplayName("Estado")] public virtual bool estado { get; set; } = false;


        [DisplayName("Observaciones")]
        public virtual string observaciones { get; set; }

        [Obligado]
        [DisplayName("Nombre Nivel")]
        public virtual string nivel_nombre { get; set; }


        [Obligado]
        [DisplayName("Es Actividad")]
        public virtual bool es_actividad { get; set; }

        [Obligado]
        public virtual bool vigente { get; set; } = true;

        [ForeignKey("Catalogo")]
        [DisplayName("Disciplina")]
        public int? DisciplinaId { get; set; }
        public virtual Catalogo Catalogo { get; set; }

        [DisplayName("Revision")]
        public string revision { get; set; }

        public int referencia_presupuesto { get; set; }
    }
}
