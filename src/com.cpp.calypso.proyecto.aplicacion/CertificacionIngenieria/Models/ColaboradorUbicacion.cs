using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models
{
    public class ColaboradorUbicacion
    {
        public int ColaboradorId { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombresApellidos { get; set; }
        public string Ubicacion   { get; set; }
        public decimal HorasLaboradas { get; set; }
    }
}
