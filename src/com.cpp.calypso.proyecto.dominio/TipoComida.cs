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
    public class TipoComida : Entity
    {
        [Obligado]
        [DisplayName("Opciones Comida")]
        public int opciones_comida_id { get; set; }
        public virtual OpcionComida opciones_comidas { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        [LongitudMayorAttribute(200)]
        public string nombre { get; set; }

        [Obligado]
        [DisplayName("Costo")]
        public decimal costo { get; set; }

        [Obligado]
        [DisplayName("Empresa")]
        public int empresa_id { get; set; }
        public virtual Empresa empresas { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;
    }
}
