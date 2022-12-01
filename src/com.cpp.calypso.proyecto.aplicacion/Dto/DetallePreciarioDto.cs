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
    [AutoMap(typeof(DetallePreciario))]
    [Serializable]
    public class DetallePreciarioDto :EntityDto
    {
        [Obligado]
        [DisplayName("Preciario")]
        public int PreciarioId { get; set; }

        [Obligado]
        [DisplayName("Item")]
        public virtual int ItemId { get; set; }


        [Obligado]

        [DisplayName("Precio Unitario")]
        public virtual decimal precio_unitario { get; set; }

      
        [LongitudMayorAttribute(200)]
        [DisplayName("Comentario")]
        public string comentario { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        [DefaultValue(true)]
        public virtual bool vigente { get; set; }

        public virtual string nombreitempadre { get; set; }
        public virtual Preciario Preciario { get; set; }
        public virtual Item Item { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<ItemDto> ItemsDto { get; set; }
    }
}
