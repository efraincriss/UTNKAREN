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

namespace com.cpp.calypso.proyecto.dominio.Transporte
{
    [Serializable]
    public class VehiculoHistorico : Entity, ISoftDelete, ICreationAudited
    {
        [Obligado]
        [DisplayName("Vehículo")]
        public int VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        [StringLength(3)]
        public string Estado { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Estado")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaEstado { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }
    }
}
