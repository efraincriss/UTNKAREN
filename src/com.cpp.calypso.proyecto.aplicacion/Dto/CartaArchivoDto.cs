using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(CartaArchivo))]
    [Serializable]
    public class CartaArchivoDto :EntityDto
    {
        [DisplayName("Carta")]
        public int CartaId { get; set; }

        public virtual Carta Carta { get; set; }

        public int ArchivoId { get; set; }

        [DisplayName("Descripción")]
        public string descripcion { get; set; }


        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }

        public virtual CartaDto InfoCarta { get; set; }
        public virtual string nombreArchivo { get; set; }
}
}
