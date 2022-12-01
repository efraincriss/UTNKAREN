using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class DetalleOrdenCompra : Entity
    {
        [Obligado]
        [DisplayName("Orden Compra")]
        public int OrdenCompraId { get; set; }

        public virtual OrdenCompra OrdenCompra { get; set; }

        [Obligado]
        [DisplayName("Seleccione Item")]
        public int ComputoId { get; set; }
        [JsonIgnore]
        public virtual Computo Computo { get; set; }

        [Obligado]
        [DisplayName("Tipo Registro")]
        public TipoFecha tipoFecha { get; set; }



        [Obligado]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha")]
        public virtual DateTime fecha { get; set; }

  
        [DisplayName("Porcentaje")]
        public virtual decimal porcentaje { get; set; }

        [DisplayName("Costo Porcentaje a pagar(USD)")]

        public virtual decimal valor { get; set; }

        [Obligado]
        public virtual bool vigente { get; set; } = true;
        [Obligado]
        [DisplayName("Estado")]
        public virtual EstadoDetalleOrdenCompra estado { get; set; }


        public enum TipoFecha
        {
            Compras = 1,
            Pruebas = 2,
            Final = 3,
        }
        public enum EstadoDetalleOrdenCompra
        {
            Registrado = 1,
            Aprobado = 2,
            Certificado = 3,
        }
    }
}
