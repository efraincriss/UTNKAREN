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
    public class ArchivosOferta : Entity
    {

        [DisplayName("Archivo")] public int ArchivoId { get; set; }

        public virtual Archivo Archivo { get; set; }

        [DisplayName("Oferta")] public int OfertaId { get; set; }

        public virtual OfertaComercial OfertaComercial { get; set; }

        [DefaultValue(true)] public bool vigente { get; set; }

    }
}
