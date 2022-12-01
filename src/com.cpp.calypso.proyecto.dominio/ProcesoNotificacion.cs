using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ProcesoNotificacion : Entity
    {

        [LongitudMayor(100)]
        [Obligado]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [LongitudMayor(1000)]
        [Obligado]
        [DisplayName("Formato")]
        public string formato { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public bool estado { get; set; } = true;

        [DisplayName("Tipo de Proceso")]
        public TipoProceso Tipo { get; set; }

        [Obligado]
        public bool vigente { get; set; } = true;


        public enum TipoProceso
        {
            [Display(Name = "Oferta")]
            Oferta = 1,
            [Display(Name = "Requerimiento")]
            Requerimiento = 2,
            [Display(Name = "Consulta Pública")]
            ConsultaPublica = 3
        }
    }
}
