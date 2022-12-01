using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(TransmitalDetalle))]
    [Serializable]
   public  class TransmitalDetalleDto :EntityDto
    {
        [Obligado]
        [DisplayName("Transmital")]
        public int TransmitalId { get; set; }


        public virtual TransmitalCabecera Transmital { get; set; }

        [Obligado]
        [DisplayName("Archivo")]
        public int ArchivoId { get; set; }


        [Obligado]
        [DisplayName("Código")]
        public string codigo_detalle { get; set; }

        [Obligado]
        [DisplayName("Descripción de Archivos")]
        public string descripcion { get; set; }
        public string version { get; set; }

        [Obligado]
        [DisplayName("Número Hojas")]
        public int nro_hojas { get; set; }

        [Obligado]
        [DisplayName("Número Copias")]
        public int nro_copias { get; set; }

        [DisplayName("Es Oferta")]
        public bool es_oferta { get; set; } = false;

        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }
         
        public virtual string estado_es_oferta { get; set; }
        public virtual string nombre_archivo { get; set; }
        public virtual HttpPostedFileBase UploadedFile { get; set; }
    }
}
