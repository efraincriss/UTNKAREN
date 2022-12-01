using System;
using Abp.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class CartaArchivo : Entity
    {

        [DisplayName("Carta")]
        public int CartaId { get; set; }

        public virtual Carta Carta { get; set; }

        [DisplayName("Archivo")]
        public int ArchivoId { get; set; }

        public Archivo Archivo { get; set; }

        [DisplayName("Descripción")]
        public string descripcion { get; set; }


        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }
    }
}
