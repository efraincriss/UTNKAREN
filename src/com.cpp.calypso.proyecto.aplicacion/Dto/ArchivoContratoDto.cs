using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ArchivosContrato))]
    [Serializable]
    public class ArchivosContratoDto : EntityDto
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
