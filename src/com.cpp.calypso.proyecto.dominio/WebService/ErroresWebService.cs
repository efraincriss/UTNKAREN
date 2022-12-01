using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.WebService
{
    [Serializable]
    public class ErroresWebService:Entity
    {

        [DisplayName("Web Service")]
        public int ServicioWebId { get; set; }
        public virtual ServicioWeb ServicioWeb { get; set; }

        [DisplayName("Código Error")]
        public string codigo_error { get; set; }

        [DisplayName("Error")]
        public string error { get; set; }


    }
}
