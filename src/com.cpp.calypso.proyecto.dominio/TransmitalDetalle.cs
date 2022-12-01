using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
   public class TransmitalDetalle : Entity
    {
        [Obligado]
        [DisplayName("Transmital")]
        public int TransmitalId { get; set; }
        public virtual TransmitalCabecera Transmital { get; set; }

        [Obligado]
        [DisplayName("Archivo")]
        public int ArchivoId { get; set; }
        public virtual Archivo Archivo { get; set; }

        [Obligado]
        [DisplayName("Código")]
        public string codigo_detalle{ get; set; }

        [Obligado]
        [DisplayName("Descripción de Archivos")]
        public string descripcion { get; set; }

      

        [Obligado]
        [DisplayName("Número Hojas")]
        public int nro_hojas { get; set; }

        [Obligado]
        [DisplayName("Número Copias")]
        public int nro_copias { get; set; }

        public string version { get; set; }
       
       public bool es_oferta { get; set; } = false;

        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }
    }
}
