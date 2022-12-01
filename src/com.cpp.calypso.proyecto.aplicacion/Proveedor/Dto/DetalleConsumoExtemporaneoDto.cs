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
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto
{
    [AutoMap(typeof(DetalleConsumoExtemporaneo))]
    [Serializable]
    public class DetalleConsumoExtemporaneoDto : EntityDto
    {
        /// <summary>
        /// Id del consumo extemporaneo padre
        /// </summary>
        [Obligado]
        [DisplayName("Consumo Extemporaneo")]
        public int ConsumoExtemporaneoId { get; set; }

        /// <summary>
        /// Id del colaborador
        /// </summary>
        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }

        [MaxLength(800)]
        public string Observaciones { get; set; }

        public int Secuencia { get; set; }

        public bool Liquidado { get; set; } = false;

        public int? LiquidacionDetalleId { get; set; }

        public string ColaboradorNombre { get; set; }

        public string ColaboradorIdentificacion { get; set; }

        public string LiquidadoString { get; set; }
    }
}
