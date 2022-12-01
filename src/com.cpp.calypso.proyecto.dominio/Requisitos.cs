using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
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
    public class Requisitos : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Tipo Requisito")]
        [ForeignKey("TipoRequisito")]
        public int requisitoId { get; set; }
        public virtual Catalogo TipoRequisito { get; set; }

        [Obligado]
        [LongitudMayor(10)]
        [DisplayName("Código")]
        public string codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Obligado]
        [DisplayName("Descripción")]
        public string descripcion { get; set; }

        [Obligado]
        [DisplayName("Responsable de requisito")]
        [ForeignKey("Responsable")]
        public int responsableId { get; set; }
        public virtual Catalogo Responsable { get; set; }

        [DisplayName("Aplica caducidad")]
        public bool? caducidad { get; set; }

        [DisplayName("Tiempo de vigencia (días)")]
        public int? tiempo_vigencia { get; set; }

        [DisplayName("Inicio Alerta (días)")]
        public int? dias_inicio_alerta { get; set; }

        [Obligado]
        [DisplayName("Estado (Activo/Inactivo)")]
        public bool vigente { get; set; } = true;

        [Obligado]
        [DisplayName("Activo")]
        public bool activo { get; set; } = true;

        public string GetAplicaCaducidad()
        {
            if (caducidad.HasValue)
            {
                if (caducidad.Value)
                    return "SI";
            }

            return "NO";
        }

        public string GetTiempoVigencia()
        {
            if (tiempo_vigencia.HasValue)
            {
                return tiempo_vigencia.Value + " MES(ES)";
            }

            return "NO ESPECIFICADO";
        }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<RequisitoColaborador> RequisitosColaboradores { get; set; }



        //Campos Sincronizacion
        
        public bool genera_notificacion { get; set; } = false;
        public int? CatalogoFrecuenciaNotificacionId { get; set; }
        public Catalogo CatalogoFrecuenciaNotificacion { get; set; }

        public int? tiempo_inicio_notificacion { get; set; }
        public int? dia_envio_notificacion { get; set; }


    }
}
