using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models
{
    public class DetallesDirectos
    {

        public List<DetallesDirectosIngenieriaDto> Directos { get;set; }
        public List<DetalleDirectoE500Dto> DirectosE500 { get; set; }
    }
}
