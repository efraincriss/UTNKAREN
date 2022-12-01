using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto
{
    public class ResultadoColaboradorRubro
    {

        public bool Success { get; set; }

        public string Message { get; set; }

        public decimal Contador { get; set; } = 0;
    }
}
