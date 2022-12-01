using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public  class DetalleGanancia:  Entity
    {
        [Obligado]
        [DisplayName("Ganancia")]
        public int GananciaId { get; set; }
        public virtual Ganancia Ganancia { get; set; }

        [Obligado]
        [DisplayName("Porcentaje de Incremento")]
        public int PorcentajeIncrementoId { get; set; }
        public virtual PorcentajeIncremento PorcentajeIncremento { get; set; }

        [Obligado]
        [DisplayName("Grupo")]
        public int GrupoItemId { get; set; }
        public virtual GrupoItem GrupoItem { get; set; }


        [DisplayName("Valor")]
        
        public decimal valor { get; set; }

        [DefaultValue(true)]
        public bool vigente { get; set; }

    }
}
