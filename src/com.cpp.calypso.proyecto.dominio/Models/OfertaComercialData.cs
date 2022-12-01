using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class OfertaComercialData
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int? TransmitalId { get; set; }
        public int estado { get; set; }
        public string descripcion { get; set; }
        public bool service_request { get; set; }
        public bool service_order { get; set; }
        public decimal monto_so_referencia_total { get; set; }
        public decimal monto_ofertado { get; set; }
        public decimal monto_so_aprobado { get; set; }
        public decimal monto_ofertado_pendiente_aprobacion { get; set; }
        public decimal monto_certificado_aprobado_acumulado { get; set; }
        public DateTime? fecha_pliego { get; set; }
        public DateTime? fecha_primer_envio { get; set; }
        public DateTime? fecha_ultimo_envio { get; set; }
        public virtual int dias_emision_oferta { get; set; }

       public virtual decimal porcentaje_avance { get; set; }
       public virtual DateTime? fecha_ultima_modificacion { get; set; }
        public string version { get; set; }

        public DateTime? fecha_recepcion_so { get; set; }
        
        public int dias_hasta_recepcion_so { get; set; }

        public string codigo { get; set; }

        public int alcance { get; set; }


        public virtual DateTime? fecha_oferta { get; set; }

        public int es_final { get; set; }
        
        public virtual bool orden_proceder { get; set; }


        public DateTime? fecha_orden_proceder { get; set; }

        public string orden_proceder_enviada_por { get; set; }
        
        public int? OfertaPadreId { get; set; }

        public bool vigente { get; set; } = true;


        public int tipo_Trabajo_Id { get; set; }

        public int centro_de_Costos_Id { get; set; }

        
        public int estatus_de_Ejecucion { get; set; }

        public string codigo_shaya { get; set; }

 
        public string revision_Oferta { get; set; }

     
        public int forma_contratacion { get; set; }

        public int acta_cierre { get; set; }

  
        public bool computo_completo { get; set; } = false;


        public int estado_oferta { get; set; }
      

        public string comentarios { get; set; }
        public string link_documentum { get; set; }
        public string link_ordenProceder { get; set; }
        public bool monto_editado { get; set; }


        public string codigoContrato { get; set; }
        public bool tieneTransmital { get; set; }
        public string nombreEstadoOferta { get; set; }
        public string fechaPrimerEnvio{ get; set; }
        public string fechaUltimoEnvio { get; set; }
        public string fechaOferta { get; set; }

        public bool tienePresupuestoLigado { get; set; }
        public bool tienePresupuestosAdicionales{ get; set; }
        public bool tieneRequerimientoPrincipal { get; set; }
        public bool puedeEditarMontoAprobado { get; set; }


        public string codigoTransmittal { get; set; }

        public decimal monto_ofertado_migracion_actual { get; set; } 
        public decimal monto_so_aprobado_migracion_anterior { get; set; } 
        public decimal monto_so_aprobado_migracion_actual { get; set; }
    }
}
