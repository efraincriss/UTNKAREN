using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
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
    [AutoMap(typeof(ColaboradoresAusentismo))]
    [Serializable]
    public class ColaboradoresAusentismoDto : EntityDto, IFullAudited
    {
        //internal object lsAusentismos;

        [Obligado]
        [DisplayName("Colaborador")]
        public int colaborador_id { get; set; }
        public virtual Colaboradores Colaborador { get; set; }

        [Obligado]
        [DisplayName("Tipo Ausentismo")]
        public int catalogo_tipo_ausentismo_id { get; set; }

        public virtual Catalogo TipoAusentismo { get; set; }


        [Obligado]
        [DataType(DataType.Date)]
        [DisplayName("Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_inicio { get; set; }

        [Obligado]
        [DataType(DataType.Date)]
        [DisplayName("Fecha Fin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_fin { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public String estado { get; set; }

        [Obligado]
        [DisplayName("Vigencia")]
        public bool vigente { get; set; } = true;

        public string observacion { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public virtual String nombres { get; set; }

        public virtual String nombre_ausentismo { get; set; }
        public virtual String codigo_ausentismo { get; set; }

        public virtual String tipo_identificacion { get; set; }

        public virtual String nro_identificacion { get; set; }

        public virtual String grupo_personal { get; set; }

        public virtual String nro_legajo { get; set; }

        public virtual List<ColaboradoresAusentismoRequisitosDto> requisitos { get; set; }

        public virtual int nro { get; set; }

        public virtual String estado_colaborador { get; set; }

        /* */
        public virtual ColaboradoresAusentismoRequisitosDto RequisitoArchivos { get; set; }

        public virtual string formatFechaInicio { get; set; }
        public virtual string formatFechaFin{ get; set; }
    }
}
