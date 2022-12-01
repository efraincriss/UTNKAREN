using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class NodeItem
    {
        public string label { get; set; }
        public string data { get; set; }
        public string expandedIcon { get; set; }
        public string collapsedIcon { get; set; }

        public string tipo { get; set; }

        public string nombres { get; set; }

        [CanBeNull]
        public List<NodeItem> children { get; set; }

        [CanBeNull]
        public string icon { get; set; }

        public int key { get; set; }

        [CanBeNull]
        public bool draggable { get; set; }
        [CanBeNull]
        public bool droppable { get; set; }

        [CanBeNull]
        public bool selectable { get; set; }


        [CanBeNull]
        public string labelcompleto { get; set; }

        /* Data Node*/
        public int id { get; set; }
        public int EspecialidadId { get; set; }

        public string NombreEspecialidad { get; set; }
        public int UnidadId { get; set; }

        public int GrupoId { get; set; }
        public bool para_oferta { get; set; }

      

    }
}
