using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class TempRequerimiento : Entity
    {
        [DisplayName("Proyecto Principal")]
        public string ProyectoPrincipal { get; set; }

        [DisplayName("Tipo Requerimiento")]
        public TipoRequerimiento TipoRequerimiento { get; set; }

        [DisplayName("Codigo")]
        public string Codigo { get; set; }


        [DisplayName("Fecha Recepción")]
        public DateTime FechaRecepcion { get; set; }


        [DisplayName("Descripcion")]
        public string Descripcion { get; set; }

        [DisplayName("Solicitante")]
        public string Solicitante { get; set; }


        [DisplayName("Monto Ingeniería")]
        public decimal MontoIngenieria { get; set; }


        [DisplayName("Monto Construcción")]
        public decimal MontoConstruccion { get; set; }

        [DisplayName("Monto Suministro")]
        public decimal MontoSuministro { get; set; }

        public int? RequerimeintoId { get; set; } =null;

        public string  estadoRequerimiento { get; set; }

        public string CodigoOfertaAsiciada { get; set; }
        
    }
}
