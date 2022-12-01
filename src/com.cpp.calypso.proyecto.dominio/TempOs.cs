using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class TempOs:Entity
    {
        public string codigoProyecto { get; set; }
        public string codigoOferta { get; set; }
        public string status { get; set; }
        public string revOferta { get; set; }

        public string codigoOrden { get; set; }
        public string version { get; set; }
        public DateTime fechaOrden { get; set; }
        public decimal MontoAprobado { get; set; }
        public decimal montoIngenieria { get; set; }
        public decimal montoProcura { get; set; }
        public decimal montoContruccion { get; set; }
    }
}
