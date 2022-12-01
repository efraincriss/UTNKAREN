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

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(CorreoExterno))]
    [Serializable]
    public class CorreoExternoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Institución")]
        [LongitudMayor(100)]
        public string institucion { get; set; }


        [Obligado]
        [DisplayName("Nombres")]
        [LongitudMayor(200)]
        public string nombre { get; set; }

        [Obligado]
        [DisplayName("Correo")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string correo { get; set; }


        public bool vigente { get; set; } = true;
    }
}
