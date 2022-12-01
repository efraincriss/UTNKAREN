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

namespace com.cpp.calypso.proyecto.dominio.RecursosHumanos
{
    [Serializable]
    public class ColaboradorNotificacion : Entity, IFullAudited
    {

        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }
        public Colaboradores Colaborador { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Notificacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaNotificacion { get; set; }

        [DisplayName("Proceso Notificacion")]
        public string ProcesoNotificacion { get; set; }

        [Obligado]
        [DisplayName("Lista Distribucion")]
        public int ListaDistribucionResponsableId { get; set; }
        public ListaDistribucionResponsable ListaDistribucionResponsable { get; set; }
        public long? CreatorUserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long? LastModifierUserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? LastModificationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long? DeleterUserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? DeletionTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
