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
   public  class Adenda: Entity
    {
        [Obligado]
        [DisplayName("Contrato")]
        public int ContratoId { get; set; }

        public  virtual Contrato Contrato { get; set; }

        [Obligado]
        [DisplayName("Archivo")]
        public int ArchivosContratoId { get; set; }
        public virtual ArchivosContrato ArchivosContrato { get; set; }

        [Obligado]
        [DisplayName("Código")]
        public string codigo { get; set; }

        [Obligado]
        [DisplayName("Fecha Adenda")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha { get; set; }

        [Obligado]
        [LongitudMayorAttribute(800)]
        [DisplayName("Descripción")]
        public string descripcion { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        [DefaultValue(true)]
        public bool vigente { get; set; }
      
    }
}
