using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class Cobro : Entity
    {
        [Obligado]
        public int EmpresaId { get; set; }
        public virtual Empresa Empresa { get; set; }

        [Obligado]
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

        [DisplayName("Documento Sap")]
        public string documento_sap { get; set; }

        [DisplayName("Descripción")]
        public string descripcion { get; set; }

        [DisplayName("Fecha Cobro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_documento { get; set; }

        [DisplayName("Fecha Carga")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_carga { get; set; }
        [DisplayName("Valor")]
        public decimal monto { get; set; }

        [DisplayName("Valor")]
        public decimal monto_detalle { get; set; } = 0;

        [DisplayName("Documento Compensación")]
        public string documento_compensacion { get; set; } 

        [DefaultValue(true)]
        public bool vigente { get; set; } = true;
    }
}
