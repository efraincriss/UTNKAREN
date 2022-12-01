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
    public class ArchivosAvanceObra : Entity
    {

        [DisplayName("Archivo")] public int ArchivoId { get; set; }

        public virtual Archivo Archivo { get; set; }

        [DisplayName("Avance Obra")] public int AvanceObraId { get; set; }

        public virtual AvanceObra AvanceObra { get; set; }

        [DefaultValue(true)] public bool vigente { get; set; }



        public string descripcion { get; set; }
    }
}
