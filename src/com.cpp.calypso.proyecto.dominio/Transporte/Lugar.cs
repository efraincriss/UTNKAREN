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
    public class Lugar : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Código")]
        [StringLength(20)]
        public string Codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [DisplayName("Latitud")]
        [Range(-180.000000, 180.000000, ErrorMessage = "Campo Latitud es inválido")]
        public decimal? Latitud { get; set; }

        [DisplayName("Longitud")]
        [Range(-180.000000, 180.000000, ErrorMessage = "Campo Longitud es inválido")]
        public decimal? Longitud { get; set; }

        [DisplayName("Descripción")]
        [StringLength(400)]
        public string Descripcion { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }
    }
}
