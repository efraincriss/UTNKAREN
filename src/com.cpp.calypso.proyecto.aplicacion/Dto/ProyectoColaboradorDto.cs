using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ProyectoColaborador))]
    [Serializable]
    public class ProyectoColaboradorDto : EntityDto
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradoresId { get; set; }
        public virtual Colaboradores Colaboradores { get; set; }

        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }
        public virtual Proyecto Proyecto { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;
    }
}
