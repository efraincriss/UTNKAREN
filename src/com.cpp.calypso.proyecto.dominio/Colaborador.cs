using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class Colaborador : Entity
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


       
    }
   
}
