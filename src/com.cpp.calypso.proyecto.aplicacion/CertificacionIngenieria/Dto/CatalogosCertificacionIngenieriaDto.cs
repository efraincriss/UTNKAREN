using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto
{
    public class CatalogosCertificacionIngenieriaDto
    {
        public List<CatalogoDto> Disciplina { get; set; }

        public List<CatalogoDto> Ubicacion { get; set; }

        public List<CatalogoDto> Modalidad { get; set; }
    }
}
