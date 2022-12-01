using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(DetalleGanancia))]
    [Serializable]
    public class DetalleGananciaDto: EntityDto
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

        public virtual  List<PorcentajeIncremento> PorcentajesIncremento { get; set; }

        public virtual List<GrupoItem> GruposItem { get; set; }
    }
}
