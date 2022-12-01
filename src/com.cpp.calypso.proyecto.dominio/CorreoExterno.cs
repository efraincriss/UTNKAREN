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
    public class CorreoExterno : Entity
    {
        [Obligado]
        [DisplayName("Institución")]
        [LongitudMayor(100)]
        public string institucion { get; set; }


        [Obligado]
        [DisplayName("Nombres")]
        [LongitudMayor(200)]
        public string  nombre { get; set; }

        [Obligado]
        [DisplayName("Correo")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string correo { get; set; }


        public bool vigente { get; set; } = true;
    }
}
