using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(Carta))]
    [Serializable]
    public class CartaDto : EntityDto
    {
        [DisplayName("Tipo Carta")]
        public int TipoCartaId { get; set; }

        [Obligado]
        [DisplayName("Numero Carta")]
        public string numeroCarta { get; set; }

        [DisplayName("Tipo Destinatario")]
        public int TipoDestinatarioId { get; set; }
     
        [DisplayName("Clasificacion")]
        public int ClasificacionId { get; set; }
      
        [DisplayName("Fecha de Envio/Recepcion")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha { get; set; }
 
        [DisplayName("Fecha de Sello")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fechaSello { get; set; }

        [DisplayName("Asunto")]
        public string asunto { get; set; }


        [DisplayName("Enviado por")]
        public string enviadoPor { get; set; }

        [DisplayName("Dirigido A")]
        public string dirigidoA { get; set; }


        [DisplayName("Requiere Respuesta")]
        public bool requiereRespuesta { get; set; } = false;

        [DisplayName("Numero Carta Recibida ")]
        public string numeroCartaRecibida { get; set; }

        [DisplayName("Numero Carta Enviada ")]
        public string numeroCartaEnviada { get; set; }

        [DisplayName("Descripción")]
        public string descripcion { get; set; }

        [DisplayName("Link")]
        public string linkCarta { get; set; }

        [DisplayName("Ref")]
        public string referencia { get; set; }

        public bool vigente { get; set; } = true;

        public virtual string formatFecha { get; set; }
        public virtual string formatFechaSello { get; set; }
        public virtual string nombretipoCarta { get; set; }
        public virtual string nombretipoDestinatario { get; set; }
        public virtual string nombreClasificacion { get; set; }

        public virtual List<ListaDistribucion> listDistribuciones { get; set; }
    }
}
