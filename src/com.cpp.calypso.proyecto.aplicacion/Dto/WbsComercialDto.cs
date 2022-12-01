using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(WbsComercial))]
    [Serializable]
    public class WbsComercialDto : EntityDto
    {
        [Obligado]
        [DisplayName("Oferta Comercial")]
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
        [LongitudMayor(50)]
        public virtual string id_nivel_codigo { get; set; }


        [Obligado]
        [DisplayName("Código Padre")]
        [LongitudMayor(50)]
        public virtual string id_nivel_padre_codigo { get; set; }

        [Obligado] [DisplayName("Estado")] public virtual bool estado { get; set; } = false;


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

        // Virtuales
        public virtual string nombre_padre { get; set; }

        [ForeignKey("Catalogo")]
        [DisplayName("Disciplina")]
        public int? DisciplinaId { get; set; }
        public virtual Catalogo Catalogo { get; set; }

        [DisplayName("Revision")]
        [LongitudMayor(50)]
        public string revision { get; set; }

        // listado virtual 
        public virtual List<CatalogoDto> ListadoDisciplinas { get; set; }

        public  int referencia_presupuesto { get; set; }
    }
}
