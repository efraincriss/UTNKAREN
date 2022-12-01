using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class DatosSeguimiento
    {
        public string codigoOferta { get; set; }
        public string versionOferta { get; set; }
        public int requerimientoId { get; set; }
        public string tipoTrabajo { get; set; }
        public string alcance { get; set; }
        public string codigoProyecto { get; set; }
        public string ccShaya { get; set; }
        public string descripcionProyecto { get; set; }
        public string statusOferta { get; set; }
        public string claseAACE { get; set; }
        public decimal montoOfertado { get; set; } = 0;
        public decimal montoOSConstruccion { get; set; } = 0;
        public decimal montoOSIngenieria { get; set; } = 0;
        public decimal montoOSSuministros { get; set; } = 0;
        public decimal montoOSSubcontratos { get; set; } = 0;
        public decimal montoOSTotal { get; set; } = 0;

        public decimal montoOPAIngenieria { get; set; } = 0;
        public decimal montoOPAConstruccion { get; set; } = 0;
        public decimal montoOPASuministros { get; set; } = 0;
        public decimal montoOPASubcontratos { get; set; } = 0;
        public decimal montoOPA { get; set; } = 0;

        public string fechaSR { get; set; }
        public string fechaPrimerEnvio { get; set; }
        public string fechaUltimoEnvio { get; set; }
        public int diasEmisionPrimieraB { get; set; } = 0;
        public string codigoSO { get; set; }
        public string fechaSOUltimaRegistrada { get; set; }
        public int diasAprobacion { get; set; }
        public int diasActualizacion { get; set; }
        public string transmittals { get; set; }
        public string formatoContratacion { get; set; }
        public string statusejecucion { get; set; }
        public string principaloadicional { get; set; }
        public string comentarios { get; set; }
    }
}
