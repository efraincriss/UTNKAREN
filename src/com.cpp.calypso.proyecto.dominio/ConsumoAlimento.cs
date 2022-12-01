using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ConsumoAlimento : Entity
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int colaborador_id { get; set; }
        public virtual Colaborador colaboradores { get; set; }

        [Obligado]
        [DisplayName("Proveedor")]
        public int proveedor_id { get; set; }
        public virtual Proveedor proveedores { get; set; }

        [Obligado]
        [DisplayName("Opcion Comida")]
        public int opcion_comida_id { get; set; }
        public virtual OpcionComida opciones_comida { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;
    }
}
