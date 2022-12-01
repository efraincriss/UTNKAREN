using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.dominio
{
    public class TreeWbs
    {

        public string label { get; set; }
        public string data { get; set; }
        public string expandedIcon { get; set; }
        public string collapsedIcon { get; set; }

        public string tipo { get; set; }

        public string nombres { get; set; }

        [CanBeNull]
        public List<TreeWbs> children { get; set; }

        [CanBeNull]
        public string icon { get; set; }

        public int key { get; set; }

        [CanBeNull]
        public bool draggable { get; set; }
        [CanBeNull]
        public bool droppable { get; set; }

        [CanBeNull]
        public bool selectable { get; set; }

        public bool expanded { get; set; } = false;

        public string orden { get; set; }
        public int indice { get; set; }

        public string nivel_nombre { get; set; }
    }
}
