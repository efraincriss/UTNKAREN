using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.RecursosHumanos.Models
{
    public class ColaboradorModel
    {
        public int Id { get; set; }
        public string tipoIdentificacion { get; set; }
        public string identificacion { get; set; }
        public string nombresCompletos { get; set; }
        public string estado { get; set; }
        public string idLegajo { get; set; }
        public string idSap { get; set; }
        public string idSapLocal { get; set; }
        public string tipoColaborador { get; set; }

        public bool Alimentacion { get; set; }
        public bool Hospedaje { get; set; }
        public bool Transporte { get; set; }
        public List<Catalogo> selectComidas { get; set; }
        public List<Catalogo> selectTansporte { get; set; }

    }
}
