using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto
{
    public class CapacitacionesColaboradorDto
    {
        public string NombresColaborador { get; set; }

        public string NumeroIdentificacion { get; set; }

        public string CodigoSap { get; set; }

        public string Estado { get; set; }

        public List<CapacitacionDto> DetalleCapacitaciones { get; set; }

        public List<CapacitacionDto> ResumenCapacitaciones { get; set; }

        public List<CatalogoDto> CatalogoTipoCapacitacion { get; set; }

        public List<CatalogoDto> CatalogoNombreCapacitacion { get; set; }

        public List<CapacitacionDto> Capacitaciones { get; set; }
    }
}
