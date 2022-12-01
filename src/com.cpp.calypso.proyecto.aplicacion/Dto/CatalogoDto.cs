using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(Catalogo))]
    [Serializable]
    public class CatalogoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Tipo Catálogo")]
        public int TipoCatalogoId { get; set; }

        public virtual TipoCatalogo TipoCatalogo { get; set; }

        [Obligado]
        [LongitudMayor(200)]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Obligado]
        [LongitudMayor(800)]
        [DisplayName("Descripción")]
        public string descripcion { get; set; }


        [Obligado]
        [LongitudMayor(20)]
        [DisplayName("Código")]
        public string codigo { get; set; }


        [Obligado]
        [DisplayName("Predeterminado")]
        public bool predeterminado { get; set; }


        [Obligado]
        public bool vigente { get; set; }


        [Obligado]
        [DisplayName("Ordinal")]
        public int ordinal { get; set; }

        [LongitudMayor(100)]
        [DisplayName("Valor Texto")]
        public string valor_texto { get; set; }

        [DisplayName("Valor Numerico")]
        public decimal? valor_numerico { get; set; }

        [DisplayName("Valor Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? valor_fecha { get; set; }

        [DisplayName("Valor Binario")]
        public bool? valor_binario { get; set; }

        [DisplayName("Visualiza Móvil")]
        public bool visualiza_movil { get; set; } = false;


        public virtual string  NombreTipoCatalogo{ get; set; }
        public virtual string CodigoTipoCatalogo { get; set; }
    }
}
