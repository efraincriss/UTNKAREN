using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(ArchivosAvanceObra))]
    [Serializable]
    public class ArchivosAvanceObraDto : EntityDto
    {
        [DisplayName("Archivo")] public int ArchivoId { get; set; }

        public virtual Archivo Archivo { get; set; }

        [DisplayName("Avance Obra")] public int AvanceObraId { get; set; }

        public virtual AvanceObra AvanceObra { get; set; }

        [DefaultValue(true)] public bool vigente { get; set; }

        public string descripcion { get; set; }

        public virtual string  filebase64 { get; set; }

        public virtual HttpPostedFileBase UploadedFile { get; set; }

    }
}
