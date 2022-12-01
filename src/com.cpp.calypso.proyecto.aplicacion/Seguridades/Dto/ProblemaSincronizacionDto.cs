using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace com.cpp.calypso.proyecto.aplicacion.Seguridades.Dto
{
    public class ProblemaSincronizacionDto : EntityDto
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
