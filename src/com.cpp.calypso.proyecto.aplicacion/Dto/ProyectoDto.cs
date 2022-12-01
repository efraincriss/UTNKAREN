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
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(Proyecto))]
    [Serializable]
    public class ProyectoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Contrato")]
        public int contratoId { get; set; }
        public virtual Contrato Contrato { get; set; }
        [Obligado]
        [DisplayName("Código")]
        [StringLength(100)]
        public virtual string codigo { get; set; }

      
        [DisplayName("Nombre del Proyecto")]
        [StringLength(255)]
        public virtual string nombre_proyecto { get; set; }

        
        [DisplayName("Descripción del Proyecto")]
        [StringLength(800)]
        public string descripcion_proyecto { get; set; }

       
        [DisplayName("Centro de Costos")]
        public int centroCostosId { get; set; }

      
        [DisplayName("Alcance básico")]
        [StringLength(255)]
        public string alcance_basico { get; set; }

       
        [DisplayName("Presupuesto Referencial")]
        [DisplayFormat(DataFormatString = "{0:0,0.0}")]
        public virtual decimal presupuesto { get; set; }

        [DisplayName("Comentarios")]
        [StringLength(800)]
        public string comentarios { get; set; }

      
        [DisplayName("Monto Ofertado")]
       
        public virtual decimal monto_ofertado { get; set; }

    
        [DisplayName("Monto aprobado de la orden de trabajo")]
        public decimal monto_aprobado_orden_trabajo { get; set; }

     
        [DisplayName("Monto certificado de orden de trabajo")]
        public decimal monto_certificado_orden_trabajo { get; set; }

      
        [DisplayName("Monto facturado")]
        public decimal monto_facturado { get; set; }

        
        [DisplayName("Monto Cobrado")]
        public decimal monto_cobrado { get; set; }

       
        [DisplayName("Monto Aprobado Orden de Servicio")]
        public decimal monto_aprobado_os { get; set; }

     
        [DisplayName("Monto Aprobado Ingeniería")]
        public decimal monto_aprobado_os_ingenieria { get; set; }


   
        [DisplayName("Monto Aprobado Suministros")]
        public decimal monto_aprobado_os_suministros { get; set; }

            [DisplayName("Monto Aprobado Construcción")]
        public decimal monto_aprobado_os_construccion { get; set; }


 
        [DisplayName("Fecha estimada de inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_estimada_inicio { get; set; }



        [DisplayName("Fecha de Acta Entrega")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_acta_entrega { get; set; }



        [DisplayName("Fecha estimada de finalización")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_estimada_fin { get; set; }


        [DisplayName("Fecha de recepción provisional")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_recepcion_provisoria { get; set; }


        [DisplayName("Fecha recepción definitiva")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_recepcion_definitiva { get; set; }

     
        [DisplayName("Estado (Activo / Inactivo)")]
        public bool estado_proyecto { get; set; }

        [DisplayName("Periodo Garantía")]
        [Range(1, 12)]
        public int periodo_garantia { get; set; } = 12;


        [DefaultValue(true)]
        public bool vigente { get; set; }


        [DisplayName("Locación")]
        public int LocacionId { get; set; }

        [DisplayName("Código Generado")]
        [StringLength(100)]
        public string codigo_generado { get; set; }

        [DisplayName("RDO/RSO")]
        public bool es_RSO { get; set; } = false;

        [DisplayName("Código Reporte RDO")]

        public string codigo_reporte_RDO { get; set; }


        [DisplayName("Activar Logo Predeterminado RDO")]
        public bool usar_logo_prederminado { get; set; } = false;
        [DisplayName("Periodo de Fecha en Gráfico de Curva RDO")]
        public int periodo_curva { get; set; } = 20;



        [DisplayName("Locación Proyecto")]
        public string locacion { get; set; }

        [DisplayName("Código Reporte Certificación")]

        public string codigo_reporte_certificacion { get; set; }


        [DisplayName("Código Interno")]
        public string codigo_interno { get; set; }

        [DisplayName("Código Cliente")]
        public string codigo_cliente { get; set; }

        public int orden_timesheet { get; set; } = 1;

        public bool certificable_ingenieria { get; set; } = true;
        public int anio_certificacion_ingenieria { get; set; }


        [DisplayName("Proyecto Cerrado")]
        public bool ProyectoCerrado { get; set; } = false;

        public int? PortafolioId { get; set; }

        public int? UbicacionId { get; set; }



        public List<Requerimiento> Requerimientos { get; set; }

        [JsonIgnore]
        public List<Oferta> Ofertas { get; set; }

        [DisplayName("Año")]
        public int anio { get; set; }

        [DisplayName("TI PAM EP")]
        public string ti_pam_ep { get; set; }

        [DisplayName("TI TECHINT")]
        public string ti_techint { get; set; }

        [DisplayName("Desarrollado por")]
        public string desarrollador { get; set; }

        [DisplayName("Código Ingeniería")]
        public string codigo_ingenieria { get; set; }

        [DisplayName("Responsable de Generación de Cómputos")]
        public Proyecto.Reponsable? responsable { get; set; }

        public virtual decimal monto_ingenieria { get; set; }
        public virtual decimal monto_construccion { get; set; }
        public virtual decimal monto_procura { get; set; }
        public virtual decimal monto_total { get; set; }


        public List<OrdenServicio> ListadoOrdenServicios { get; set; }
        public virtual List<RequerimientoDto> RequerimientosLigados { get; set; }
    }
}
