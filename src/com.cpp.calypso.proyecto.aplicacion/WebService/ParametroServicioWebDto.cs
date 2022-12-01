using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.WebService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{

    [AutoMap(typeof(ParametroServicioWeb))]
    [Serializable]
    public class ParametroWebServiceDto:EntityDto

    {
        [DisplayName("Web Service")]
        public int ServicioWebId { get; set; }
        public virtual ServicioWeb ServicioWeb { get; set; }

        [DisplayName("Nombre Parámetro")]
        public string nombre { get; set; }

        [DisplayName("Valor")]
        public string valor { get; set; }

        [DisplayName("Código")]
        public string codigo { get; set; }

        [DisplayName("vigente")]
        public bool vigente { get; set; } = true;
    }
}
