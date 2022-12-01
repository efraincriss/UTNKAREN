using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(Requerimiento))]
    [Serializable]
    public class ArchivosRequerimientoDto:EntityDto
    {
        [DisplayName("Archivo")] public int ArchivoId { get; set; }

        public virtual Archivo Archivo { get; set; }

        [DisplayName("Requerimiento")] public int RequerimientoId { get; set; }

        public virtual Requerimiento Requerimiento { get; set; }

        public bool vigente { get; set; } = true;

        public bool tipo { get; set; } =true;

    }
}
