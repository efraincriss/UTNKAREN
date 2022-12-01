using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(ServicioWeb))]
    [Serializable]
    public  class ServicioWebDto:EntityDto
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
