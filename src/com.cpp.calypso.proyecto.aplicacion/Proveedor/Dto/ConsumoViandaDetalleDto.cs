using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{

    /// <summary>
    /// Informacion de Solicitud de Vianda, mas listado de consumos asociados a dicha solicitud
    /// </summary>
    [Serializable]
    public class ConsumoViandaDetalleDto : EntityDto
    {
        ///Id. (Id de Solicitud Vianda)

        [DataType(DataType.Date)]
        [DisplayName("Fecha Solicitud")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_solicitud { get; set; }

        [Obligado]
        [DisplayName("Solicitante")]
        public int solicitante_id { get; set; }

        [DisplayName("Solicitante")]
        public string solicitante_nombre { get; set; }

        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }
        public virtual string proveedor_nombre { get; set; }

        [DisplayName("Conductor Asignado")]
        public int? conductor_asignado_id { get; set; }
        public string conductor_asignado_nombre { get; set; }



        [DisplayName("Anotador")]
        public int? anotador_id { get; set; }
        public string anotador_nombre { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Hora Entrega Restaurante")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? hora_entrega_restaurante { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Hora Entrega Transportista")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? hora_entrega_transportista { get; set; }

        public int pedido_viandas;

        public int alcance_viandas;

        [Obligado]
        [DisplayName("Total Pedido")]
        public int total_pedido { get; set; }


        [DisplayName("Consumido")]
        public int consumido { get; set; }


        [DisplayName("Consumo Justificado")]
        public int consumo_justificado { get; set; }


        [DisplayName("Total Consumido")]
        public int total_consumido { get; set; }


        public virtual ICollection<ConsumoViandaDto> consumo_viandas { get; set; }
    }
}
