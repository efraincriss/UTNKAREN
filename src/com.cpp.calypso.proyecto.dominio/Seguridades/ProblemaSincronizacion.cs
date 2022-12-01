using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace com.cpp.calypso.proyecto.dominio.Seguridades
{
    public class ProblemaSincronizacion : Entity
    {
        public DateTime Fecha { get; set; }

        public string Fuente { get; set; }

        public string Entidad { get; set; }

        public string Problema { get; set; }

        public bool Solucionado { get; set; }

        public DateTime? FechaSolucion { get; set; }

        public int? UsuarioId { get; set; }

        public string Observaciones { get; set; }

        public string Uid { get; set; }

        public string Resumen { get; set; }
    }
}
