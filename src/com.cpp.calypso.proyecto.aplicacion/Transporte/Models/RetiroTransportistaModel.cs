using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Models
{
    public class RetiroTransportistaModel
    {
        public string Solicitante { get; set; }

        public string Transportista { get; set; }

        public string Disciplina { get; set; }

        public string Locacion { get; set; }

        public int Viandas { get; set; }

        public int Hielo { get; set; }
    }
}
