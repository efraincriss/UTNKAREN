using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto
{
    public class ResponseColaboradorItemIngenieriaDto
    {
        public bool Valido { get; set; }

        public List<string> Mensajes { get; set; }
    }
}
