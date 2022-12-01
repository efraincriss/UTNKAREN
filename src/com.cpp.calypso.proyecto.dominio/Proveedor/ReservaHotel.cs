using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio.Proveedor
{
    [Serializable]
    public class ReservaHotel : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Espacio Habitación")]
        public int EspacioHabitacionId { get; set; }
        public EspacioHabitacion EspacioHabitacion { get; set; }


        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }
        public Colaboradores Colaborador { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_registro { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_desde { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Hasta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_hasta { get; set; }

        public bool inicio_consumo { get; set; } = false;

        public DateTime? fecha_inicio_consumo { get; set; }
        public DateTime? fecha_fin_consumo { get; set; }


        public string justificacion_inicio_manual { get; set; }

        public string justificacion_finalizacion_manual { get; set; }

        public bool consumo_finalizado { get; set; } = false;

        [Obligado]
        [DisplayName("Estado")]
        public ReservaEstado estado { get; set; }


        [DisplayName("Extemporáneo")]
        public bool extemporaneo { get; set; } = false;

        //Archivos
        [DisplayName("Documento Extemporáneo")]
        public int? DocumentoId { get; set; }
        public Archivo Documento { get; set; }

        public bool aplica_lavanderia { get; set; } = false;


        public int? TipoHabitacionId { get; set; }
        public Catalogo TipoHabitacion { get; set; }
        public decimal Costo { get; set; }
        public string NombreTipoHabitacion { get; set; }
        public string NumeroHabitacion { get; set; }
        public string CodigoEspacio { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }
    }


    public enum ReservaEstado
    {
        [Description("Activo")]
        Activo = 1,

        [Description("Proceso")]
        Proceso = 2,

        [Description("Cerrado")]
        Cerrada = 3,

        [Description("Facturado")]
        Facturada = 4
    }
}
