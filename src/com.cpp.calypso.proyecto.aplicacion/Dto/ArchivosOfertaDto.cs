using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ArchivosOferta))]
    [Serializable]
    public class ArchivosOfertaDto : EntityDto
    {
        [DisplayName("Archivo")] public int ArchivoId { get; set; }

        public virtual Archivo Archivo { get; set; }
        [DisplayName("Oferta")] public int OfertaId { get; set; }

        public virtual OfertaComercial OfertaComercial { get; set; }

        [DefaultValue(true)] public bool vigente { get; set; }

        public virtual string codigo { get; set; }
        public virtual string descripcion { get; set; }

    }
}

