using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class ModelAsiganciones
    {
        public int readId { get; set; }
        public int writeId { get; set; }
        public int colaboradorId { get; set; }
        public int catalogoReponsabilidadId { get; set; }
        public string nombreResponsabilidad { get; set; }
        public bool read { get; set; }
        public bool write { get; set; }

    }
}
