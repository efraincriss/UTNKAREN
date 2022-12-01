using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    [AutoMap(typeof(ConsumoExtemporaneo))]
    [Serializable]
    public class ConsumoExtemporaneoDto : EntityDto
    {
        /// <summary>
        /// Id de Proveedor
        /// </summary>
        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }

        /// <summary>
        /// Fecha del Consumo
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Id de Tipo de Comida
        /// </summary>
        [Obligado]
        [DisplayName("Tipo Comida")]
        public int TipoComidaId { get; set; }

        /// <summary>
        /// Documento de Respaldo de consumo extemporaneo
        /// </summary>
        [DisplayName("Documento Respaldo")]
        public int? DocumentoRespaldoId { get; set; }

        public string ProveedorNombre { get; set; }

        public string TipoComidaNombre { get; set; }

        public int Secuencia { get; set; }

    }
}
