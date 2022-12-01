using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Proveedor
{
    [Serializable]
    public class RegistroSincronizacion : Entity
    {
        public int UsuarioId { get; set; }

        public string Identificador { get; set; }

        public DateTime FechaSincronizacion { get; set; }
    }
}
