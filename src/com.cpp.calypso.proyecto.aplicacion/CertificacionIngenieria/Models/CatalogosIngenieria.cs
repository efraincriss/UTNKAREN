using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio.Models;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models
{
    public class CatalogosIngenieria
    {
        public List<ModelClassReact> catalogoEstado { get; set; }
        public List<ModelClassReact> catalogoTipoRegistro { get; set; }
        public List<ModelClassReact> catalogoEstadoRegistro { get; set; }
        public List<ModelClassReact> catalogoEtapa { get; set; }
        public List<ModelClassReact> catalogoEspecialidad { get; set; }

        public List<ModelClassReact> catalogoLocacion { get; set; }
        public List<ModelClassReact> catalogoModalidad { get; set; }



    }
}

