using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.RecursosHumanos
{
    [Serializable]
   public  class ListaDistribucionResponsable : Entity, IFullAudited
    {


        [Obligado]
        [DisplayName("Lista Distribucion")]
        public int ListaDistribucionId { get; set; }
        public ListaDistribucion ListaDistribucion { get; set; }
  
        [Obligado]
        [DisplayName("Responsable")]
        public int ResponsableId { get; set; }
        public Catalogo Responsable { get; set; }

        [DisplayName("Proceso Notificacion")]
        public string ProcesoNotificacion { get; set; }
        public long? CreatorUserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long? LastModifierUserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? LastModificationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long? DeleterUserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? DeletionTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
