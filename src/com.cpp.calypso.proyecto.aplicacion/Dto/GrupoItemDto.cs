using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{

    [AutoMap(typeof(GrupoItem))]
    [Serializable]
    public class GrupoItemDto :EntityDto
    {

        [LongitudMayor(100)]
        [Obligado]
        [DisplayName("Descripción")]
        public string descripcion { get; set; }

        [DisplayName("Código")]
        public string codigo { get; set; } = "";
        [DefaultValue(true)] public bool vigente { get; set; } = true;
    }
}
