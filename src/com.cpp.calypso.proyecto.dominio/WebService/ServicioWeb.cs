using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
   public  class ServicioWeb:Entity
    {
        
        [DisplayName("URL")]
        public string url { get; set; }

        [DisplayName("targetName")]
        public string name_space { get; set; }

        [DisplayName("Código")]
        public string codigo { get; set; }

        [DisplayName("vigente")]
        public bool vigente { get; set; } = true;
    }
}
