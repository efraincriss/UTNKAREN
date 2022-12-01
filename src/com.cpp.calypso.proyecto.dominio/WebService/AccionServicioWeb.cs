using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.WebService
{[Serializable]
    public class AccionServicioWeb: Entity
    {

    
        [DisplayName("Web Service")]
        public int ServicioWebId { get; set; }
        public virtual ServicioWeb ServicioWeb { get; set; }

        [DisplayName("Nombre Acción")]
        public string nombre_accion { get; set; }

        [DisplayName("Código")]
        public string codigo { get; set; }


        [DisplayName("vigente")]
        public bool vigente { get; set; } = true;
    }
}
