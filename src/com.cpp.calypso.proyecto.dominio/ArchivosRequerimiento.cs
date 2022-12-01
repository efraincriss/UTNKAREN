using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{

    [Serializable]
    public class ArchivosRequerimiento : Entity
    {

        [DisplayName("Archivo")] public int ArchivoId { get; set; }

        public virtual Archivo Archivo { get; set; }

        [DisplayName("Requerimiento")] public int RequerimientoId { get; set; }

        public virtual Requerimiento Requerimiento { get; set; }

        public bool vigente { get; set; } = true;

        [DisplayName("Tipo")]
        public bool tipo { get; set; } = true;

    }
}
