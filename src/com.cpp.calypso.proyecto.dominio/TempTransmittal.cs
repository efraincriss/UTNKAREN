using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class TempTransmittal:Entity
    {
        public string codigoTransmital { get; set; }
        public string codigoOferta { get; set; }
        public string descripciondeltransmital { get; set; }
        public DateTime fechaEmision { get; set; }
    }
}
