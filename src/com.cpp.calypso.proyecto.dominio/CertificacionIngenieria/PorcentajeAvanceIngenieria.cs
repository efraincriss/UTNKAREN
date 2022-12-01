using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.CertificacionIngenieria
{
    [Serializable]
    public class PorcentajeAvanceIngenieria : Entity, IFullAudited
    {
        public int ProyectoId { get; set; }

        public Proyecto Proyecto { get; set; }

        public DateTime FechaAvance { get; set; }

        public int CatalogoProcentajeId { get; set; }

        public Catalogo CatalogoPorcentaje { get; set; }

        public decimal ValorPorcentaje { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
