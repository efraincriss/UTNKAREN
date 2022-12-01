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

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ContactoEmergencia : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }
        public Colaboradores Colaborador { get; set; }

        [Obligado]
        [DisplayName("Nombres Completos")]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Obligado]
        [DisplayName("Identificación")]
        [StringLength(20)]
        public string Identificacion { get; set; }

        [DisplayName("Relación")]
        [ForeignKey("CatalogoRelacion")]
        public int Relacion { get; set; }
        public Catalogo CatalogoRelacion { get; set; }

        [DisplayName("Urbanización / Comuna")]
        [StringLength(100)]
        public string UrbanizacionComuna { get; set; }

        [DisplayName("Dirección")]
        [StringLength(150)]
        public string Direccion { get; set; }

        [DisplayName("Teléfono")]
        [StringLength(20)]
        public string Telefono { get; set; }

        [DisplayName("Celular")]
        [StringLength(20)]
        public string Celular { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }
    }
}
