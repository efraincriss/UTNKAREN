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
    public class DetalleConsumoExtemporaneo : Entity, IFullAudited
    {
        /// <summary>
        /// Id del consumo extemporaneo padre
        /// </summary>
        [Obligado]
        [DisplayName("Consumo Extemporaneo")]
        public int ConsumoExtemporaneoId { get; set; }

        public ConsumoExtemporaneo ConsumoExtemporaneo { get; set; }
        

        /// <summary>
        /// Id del colaborador
        /// </summary>
        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }
        public Colaboradores Colaborador { get; set; }

        [MaxLength(800)]
        public string Observaciones { get; set; }

        public bool Liquidado { get; set; } = false;

        public int? LiquidacionDetalleId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }

        public string GetLiquidadoString()
        {
            if (Liquidado) return "SI";
            return "NO";
        }

        public string NombresCompletos()
        {
            return Colaborador.primer_apellido + Colaborador.segundo_apellido + " " +  Colaborador.nombres;
        }
    }
}
