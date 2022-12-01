using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Dto
{
    public class EstructuraArbol
    {
        public string label { get; set; }

        public string data { get; set; }

        public string expandedIcon { get; set; }

        public string collapsedIcon { get; set; }

        public string tipo { get; set; }

        public string nombres { get; set; }

        public List<EstructuraArbol> children { get; set; }

        public string icon { get; set; }

        public int key { get; set; }

        public bool draggable { get; set; }

        public bool droppable { get; set; }

        public bool selectable { get; set; }

        public bool expanded { get; set; } = false;

        public string orden { get; set; }
        public int indice { get; set; }

        public string nivel_nombre { get; set; }
    }
}
