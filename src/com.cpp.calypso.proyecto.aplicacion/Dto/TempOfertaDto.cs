using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(TempOferta))]
    [Serializable]
    public class TempOfertaDto : EntityDto
    {
        [DisplayName("TransmitalId")]
        public int TransmitalId { get; set; }


        [DisplayName("ClaseId")]
        public string ClaseId { get; set; }


        [DisplayName("Estado Oferta")]
        public string EstadoOferta { get; set; }


        [DisplayName("Service Order")]
        public string ServiceOrder { get; set; }


        [DisplayName("Service Request")]
        public string ServiceRequest { get; set; }


        [DisplayName("Monto SO Referencial Total")]
        public decimal monto_so_referencial_total { get; set; }


        [DisplayName("Monto Ofertado")]
        public decimal monto_ofertado { get; set; }


        [DisplayName("Monto SO Aprobado")]
        public decimal monto_so_aprobado { get; set; }

        public string forma_contratacion { get; set; }


        [DisplayName("Monto Ofertado Pendiente Aprobacion")]
        public decimal monto_ofertado_pendiente_aprobacion { get; set; }


        [DisplayName("Monto Certificado Aprobado Acumulado")]
        public decimal monto_certificado_acumulado { get; set; }


        [DisplayName("Fecha Pliego")]
        public DateTime? fecha_pliego { get; set; }


        [DisplayName("Fecha Primer Envío")]
        public DateTime? fecha_primer_envio { get; set; }


        [DisplayName("Fecha Último Envío")]
        public DateTime? fecha_ultimo_envio { get; set; }


        [DisplayName("Días emisión oferta")]
        public int dias_emisión_oferta { get; set; }


        [DisplayName("Porcentaje Avance")]
        public decimal porcentaje_avance { get; set; }


        [DisplayName("Fecha última modificación")]
        public DateTime? fecha_ultima_modificacion { get; set; }


        [DisplayName("Versión")]
        public string version { get; set; }


        [DisplayName("Código")]
        public string codigo { get; set; }


        [DisplayName("Alcance")]
        public string alcance { get; set; }


        [DisplayName("Tipo de Trabajo")]
        public string tipo_Trabajo_Id { get; set; }


        [DisplayName("Centro de Costos")]
        public string centro_de_Costos_Id { get; set; }


        [DisplayName("Código SO Shaya")]
        public string codigo_shaya { get; set; }


        [DisplayName("Acta de Cierre")]
        public string acta_cierre { get; set; }

        public string descripcion { get; set; }

        public string UsuarioUltimaModificacion { get; set; }
    }
}
