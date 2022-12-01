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
    public class TempProyecto : Entity
    {
        [DisplayName("Código")]
        public string codigo { get; set; }

        [DisplayName("Descripcion")]
        public string descripcion { get; set; }


        [DisplayName("Budget")]
        public decimal budget { get; set; }

        [DisplayName("comentarios")]
        public string comentarios { get; set; }

        [DisplayName("total Trabajos")]
        public decimal total_trabajos { get; set; }

        [DisplayName("total Os")]
        public decimal totalos { get; set; }
    }
}
