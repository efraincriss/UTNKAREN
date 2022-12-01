using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class CorreoLista : Entity
    {
        [Obligado]
        [DisplayName("Lista de Distribución")]
        public int ListaDistribucionId { get; set; }


        public ListaDistribucion ListaDistribucion { get; set; }

        [Obligado]
        public bool externo { get; set; }

        [LongitudMayor(100)]
        [Obligado]
        [DisplayName("Nombres")]
        public string nombres { get; set; }


        [DisplayName("Usuario")]
        public int UsuarioId { get; set; }

        [LongitudMayor(10)]
        [DisplayName("Identificación")]
        public string identificacion { get; set; }

        [LongitudMayor(100)]
        [Obligado]
        [DisplayName("Correo")]
        public string correo { get; set; }

        public bool vigente { get; set; } = true;

        public int orden { get; set; }

        public SeccionCorreo? seccion { get; set; } = SeccionCorreo.copia;

    }

    public enum SeccionCorreo
{
        dirigido = 1, // 
        copia= 2, //

    }
}
