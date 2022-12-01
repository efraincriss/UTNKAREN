using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(TarjetaAcceso))]
    [Serializable]
    public class TarjetaAccesoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }

        [Obligado]
        [DisplayName("Secuencial")]
        public int secuencial { get; set; }

        [Obligado]
        [DisplayName("Solicitud PAM")]
        [StringLength(50)]
        public string solicitud_pam { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Emisión")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_emision { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_vencimiento { get; set; }

        [Obligado]
        [DisplayName("Entregada?")]
        public bool entregada { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Reporte")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_reporte { get; set; }

        [DisplayName("Observaciones")]
        [StringLength(200)]
        public string observaciones { get; set; }

        [DisplayName("Estado")]
        public TarjetaEstado estado { get; set; }

        [DisplayName("Archivo Pdf")]
        public int? DocumentoRespaldoId { get; set; }

        public long? CreatorUserId { get; set; }

        public string secuencial_format { get; set; }

        public string usuario_nombres { get; set; }

        public string estado_nombre { get; set; }

        public string entregada_nombre { get; set; }


    }
}
