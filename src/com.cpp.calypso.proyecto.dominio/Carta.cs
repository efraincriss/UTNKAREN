using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class Carta : Entity
    {
        [DisplayName("Tipo Carta")]
        public int TipoCartaId { get; set; }
        public virtual Catalogo TipoCarta { get; set; }

        [Obligado]
        [DisplayName("Numero Carta")]
        public string numeroCarta { get; set; }

        [DisplayName("Tipo Destinatario")]
        public int TipoDestinatarioId { get; set; }
        public virtual Catalogo TipoDestinatario { get; set; }

        [DisplayName("Clasificacion")]
        public int ClasificacionId { get; set; }
        public virtual Catalogo Clasificacion { get; set; }

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

    }
}
