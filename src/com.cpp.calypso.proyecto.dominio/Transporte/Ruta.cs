using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Transporte
{
    [Serializable]
    public class Ruta : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Código")]
        [StringLength(20)]
        public string Codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; }


        [Obligado]
        [DisplayName("Sector")]
        public int SectorId { get; set; }
        public Catalogo Sector { get; set; }

        [DisplayName("Descripcion")]
        [StringLength(400)]
        public string Descripcion { get; set; }


        [Obligado]
        [DisplayName("Origen")]
        public int OrigenId { get; set; }
        public Lugar Origen { get; set; }

        [Obligado]
        [DisplayName("Destino")]
        public int DestinoId { get; set; }
        public Lugar Destino { get; set; }



        [Obligado]
        [DisplayName("Duración")]
        public int Duracion { get; set; }

        [Obligado]
        [DisplayName("Distancia")]
        public decimal Distancia { get; set; }


        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set ; }
    }
}
