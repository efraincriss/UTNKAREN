using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio.Proveedor
{
    [Serializable]
    public class ConsumoExtemporaneo : Entity, IFullAudited
    {
        /// <summary>
        /// Id de Proveedor
        /// </summary>
        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }
        public virtual dominio.Proveedor.Proveedor Proveedor { get; set; }

        /// <summary>
        /// Fecha del Consumo
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Id de Tipo de Comida
        /// </summary>
        [Obligado]
        [DisplayName("Tipo Comida")]
        public int TipoComidaId { get; set; }
        public virtual Catalogo TipoComida { get; set; }

        /// <summary>
        /// Documento de Respaldo de consumo extemporaneo
        /// </summary>
        [DisplayName("Documento Respaldo")]
        [ForeignKey("DocumentoRespaldo")]
        public int? DocumentoRespaldoId { get; set; }
        public virtual Archivo DocumentoRespaldo { get; set; }


        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }
    }
}
