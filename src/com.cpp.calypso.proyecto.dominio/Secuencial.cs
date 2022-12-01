using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class Secuencial : Entity
    {
        [Obligado]
        public string nombre { get; set; }

        [Obligado]
        public int ProyectoId { get; set; }
        public Proyecto Proyecto { get; set; }

        [Obligado]
        public int secuencia { get; set; }
    }
}
