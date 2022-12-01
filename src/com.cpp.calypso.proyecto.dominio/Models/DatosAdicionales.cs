using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class DatosAdicionales

    {
        public int requerimientoId { get; set; }
        public string contrato { get; set; }
        public string codigoProyecto { get; set; }
        public string nombreProyecto { get; set; }
        public string descripcionProyecto { get; set; }
        public string codigoRequerimiento { get; set; }
        public string descripcionRequerimiento { get; set; }
        public string estadoRequerimiento { get; set; }
        public string estadoRequerimientoCodigo { get; set; }
        public string requiereCronograma { get; set; }
        public string fechaRegistroRequerimiento { get; set; }
        public string fechaCargaPresupuesto { get; set; }
        public string estadoOferta { get; set; }
        public string estadoOfertaCodigo { get; set; }
        public string versionOferta { get; set; }
        public string aprobadoShaya { get; set; }
        public string ofertaPresentada { get; set; }
        public string fechaEmisionOferta { get; set; }
        public string fechaUltimoEnvio { get; set; }
        public decimal monto { get; set; } = 0;
        public decimal montoConstruccion { get; set; } = 0;
        public decimal montoIngenieria { get; set; } = 0;
        public decimal montoSuministros { get; set; } = 0;
        public decimal montoSubcontratos { get; set; } = 0;
        public string fechaRecepcionSolicitud { get; set; }
        public string fechadePresupuesto { get; set; }
        public string responsable { get; set; }
        public string fechaOferta { get; set; }
        public string clasificacion { get; set; }
        public string comentarios { get; set; }
        public decimal porcentajeingenieria { get; set; }
        public decimal porcentajesuministros { get; set; }
        public decimal porcentajeconstruccion { get; set; }
        public decimal porcentajesubcontratos { get; set; }
        public decimal totalUsdEjecutado { get; set; }
        public string comentariosVarios { get; set; }
        public string solicitante { get; set; }
        public string medioSolicitud { get; set; }
        public decimal montoofertado { get; set; }
        public decimal montoConstruccionPresupuesto { get; set; } = 0;
        public decimal montoIngenieriaPresupuesto { get; set; } = 0;
        public decimal montoSuministrosPresupuesto { get; set; } = 0;
        public decimal montoSubcontratosPresupuesto { get; set; } = 0;
        public decimal montoTotalPresupuesto { get; set; } = 0;


        public decimal monto_ofertado_migracion_actual { get; set; }
        public decimal monto_so_aprobado_migracion_anterior { get; set; }
        public decimal monto_so_aprobado_migracion_actual { get; set; }
    }
}
