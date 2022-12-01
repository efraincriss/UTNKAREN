using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models
{
    public class ValidacionColaboradorRubro
    {
       public List<ColaboradorDato> Parametrizacion { get; set; }
        public List<ColaboradorDato> Rubros { get; set; }
        public bool puedeCertificar { get; set; }
    }
}
