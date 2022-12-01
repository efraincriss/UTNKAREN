using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ColaboradorResponsabilidad : Entity
    {
        [DisplayName("Colaborador")]
        [ForeignKey("Colaboradores")]
        public int colaborador_id { get; set; }
        public virtual Colaboradores Colaboradores { get; set; }

        [DisplayName("Tipo de Cuenta")]
        [ForeignKey("Responsable")]
        public int catalogo_responsable_id { get; set; }
        public virtual Catalogo Responsable { get; set; }

        [DisplayName("Acceso")]
        [LongitudMayor(1)]
        public string acceso { get; set; }

    }

    //public enum AccesoPermiso
    //{
    //    [Description("Modificar")]
    //    Modificar = 'M',

    //    [Description("Visualizar")]
    //    Visualizar = 'R'
    //}
}
