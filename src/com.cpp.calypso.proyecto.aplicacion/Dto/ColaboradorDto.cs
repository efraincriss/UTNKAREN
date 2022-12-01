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
    [AutoMap(typeof(Colaborador))]
    [Serializable]
    public class ColaboradorDto : EntityDto
    {
        [Obligado]
        [DisplayName("Cédula")]
        public string cedula { get; set; }

        [DisplayName("Apellidos")]
        public string apellidos { get; set; }

        [DisplayName("Nombres")]
        public string nombres { get; set; }

        [DisplayName("Correo")]
        public string correo { get; set; }

        [DisplayName("Cédula")]
        public int ClienteId { get; set; }
        public Catalogo Cliente { get; set; }

        public bool vigente { get; set; } = true;

        public virtual string nombre_cliente { get; set;}
    }
}
