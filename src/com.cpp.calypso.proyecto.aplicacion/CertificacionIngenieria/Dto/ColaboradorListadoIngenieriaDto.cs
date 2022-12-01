using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto
{
    public class ColaboradorListadoIngenieriaDto
    {
        public int Id { get; set; }

        public string Nombres { get; set; }

        public string Identificacion { get; set; }

        public string Externo { get; set; }

        public string CodigoSap { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string Estado { get; set; }


    }
}
