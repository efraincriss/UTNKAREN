using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class CuentaEmpresa : Entity
    {
        [Obligado]
        [DisplayName("Tipo de Cuenta")]
        public string tipo_cuenta { get; set; }

        [Obligado]
        [LongitudMayorAttribute(20)]
        [DisplayName("Número de Cuenta")]
        public string numero_cuenta { get; set; }

        [Obligado]
        [DisplayName("Estado(Activo/Inactivo)")]
        public bool estado { get; set; }

        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }

        public int EmpresaId { get; set; }
        public virtual Empresa Empresa { get; set; }

        [DisplayName("Institución Financiera")]
        public int InstitucionFinancieraId { get; set; }

        public virtual InstitucionFinanciera InstitucionFinanciera { get; set; }
    }
}
