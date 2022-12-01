using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace com.cpp.calypso.proyecto.dominio.RecursosHumanos
{
    [Serializable]
    public class ActualizacionSueldo : Entity, IFullAudited
    {
        public DateTime FechaCarga { get; set; }

        public string Observaciones { get; set; }

        public string UrlArchivo { get; set; }

        public int NumeroRegistros { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public DateTime CreationTime { get; set; }

        public List<DetalleActualizacionSueldo> DetalleActualizacionSueldos { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }
    }
}
