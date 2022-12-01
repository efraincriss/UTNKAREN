using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class SeguimientoComercial: Entity
    {
       
        public string codigo { get; set; }
        public string version { get; set; }
        public string claseAACE { get; set; }
    }
}
