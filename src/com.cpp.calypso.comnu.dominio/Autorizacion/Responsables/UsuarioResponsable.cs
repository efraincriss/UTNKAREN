using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace com.cpp.calypso.comun.dominio
{
    [Serializable]
    public class UsuarioResponsable : Entity
    {
        [DisplayName("Usuario")]
        [ForeignKey("Usuario")]
        public int usuario_id { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
