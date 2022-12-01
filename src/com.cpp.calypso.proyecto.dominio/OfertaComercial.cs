using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class OfertaComercial : Entity
    {
        [Obligado]
        [DisplayName("Contrato")]
        public int ContratoId { get; set; }
        public virtual Contrato Contrato { get; set; }

        [DisplayName("Transmital")]
        public int? TransmitalId { get; set; }

        [DisplayName("Estado Oferta")]
        public int estado { get; set; }

        [DisplayName("Descripción")]
        public string descripcion { get; set; }

        [DisplayName("Service Request")]
        public bool service_request { get; set; }

        [DisplayName("Service Order")]
        public bool service_order { get; set; }

        [DisplayName("Monto total de orden de servicio")]
        public  decimal monto_so_referencia_total { get; set; }

        [DisplayName("Monto ofertado")]
        public  decimal monto_ofertado { get; set; }

        [DisplayName("Monto aprobado orden de servicio")]
        public  decimal monto_so_aprobado { get; set; }

        [DisplayName("Monto ofertado pendiente de aprobación")]
        public  decimal monto_ofertado_pendiente_aprobacion { get; set; }

        [DisplayName("Monto certifcado aprobado acumulado")]
        public  decimal monto_certificado_aprobado_acumulado { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Pliego")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public  DateTime? fecha_pliego { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha primer envío")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public  DateTime? fecha_primer_envio { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha de último envío")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public  DateTime? fecha_ultimo_envio { get; set; }

        [DisplayName("Dias de emisión oferta")]
        public virtual int dias_emision_oferta { get; set; }

        [DisplayName("Procetaje de avances")]
        public virtual decimal porcentaje_avance { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha última modificación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_ultima_modificacion { get; set; }

        [DisplayName("Versión")]
        public string version { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha de recepción orden de servicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_recepcion_so { get; set; }

        [DisplayName("Días hasta recepcion de la orden de servicio")]
        public int dias_hasta_recepcion_so { get; set; }

        [DisplayName("Código")]
        public string codigo { get; set; }

        [DisplayName("Alcance")]
        public int alcance { get; set; }
              
        

        [DataType(DataType.Date)]
        [DisplayName("Fecha de oferta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_oferta { get; set; }

        [DisplayName("Es final")]
        public int es_final { get; set; }

        [DisplayName("Orden de Proceder")]
        public virtual bool orden_proceder { get; set; } = false;


        [DataType(DataType.Date)]
        [DisplayName("Fecha Oden Proceder")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_orden_proceder { get; set; }

        [DisplayName("OP Enviada por:")]
        public string orden_proceder_enviada_por { get; set; }

        [DisplayName("Version Anterior")]
        public int? OfertaPadreId { get; set; }
        public virtual OfertaComercial OfertaPadre { get; set; }

    [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;



        [DisplayName("Tipo de Trabajo")]
        public int tipo_Trabajo_Id { get; set; }

        [DisplayName("Centro de Costos")]
        public int centro_de_Costos_Id { get; set; }

        [DisplayName("Estatus de Ejecución")]
        public int estatus_de_Ejecucion { get; set; }

        [DisplayName("Código SO Shaya")]
        public string codigo_shaya { get; set; }

        [DisplayName("Revisión de la Oferta")]
        public string revision_Oferta { get; set; }

        [DisplayName("Forma Contratación")]
        public int forma_contratacion { get; set; }

        [DisplayName("Acta de Cierre")]
        public int acta_cierre { get; set; }

        [DisplayName("Computo Completo")]
        public bool computo_completo { get; set; } = false;

        public bool monto_editado { get; set; }=false;

        [ForeignKey("Catalogo")]
        [DisplayName("Estado Oferta")]
        public int estado_oferta { get; set; }
        public virtual Catalogo Catalogo { get; set; }



        public string comentarios { get; set; }
        public string link_documentum { get; set; }
        public string link_ordenProceder{ get; set; }


        public decimal monto_ofertado_migracion_actual { get; set; }
        public decimal monto_so_aprobado_migracion_anterior { get; set; } 
        public decimal monto_so_aprobado_migracion_actual { get; set; } 
    }
}
