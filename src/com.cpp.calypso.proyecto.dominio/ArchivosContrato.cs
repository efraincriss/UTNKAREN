using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ArchivosContrato : Entity
    {

        [DisplayName("Archivo")]
        public int ArchivoId { get; set; }

        public virtual Archivo Archivos { get; set; }

        [DisplayName("Contrato")]
        public int ContratoId { get; set; }

        public virtual Contrato Contratos { get; set; }

        [DefaultValue(true)]
        public bool vigente { get; set; }

    }
}
