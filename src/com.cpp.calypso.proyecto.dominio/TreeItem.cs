using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.dominio
{
  public class TreeItem
    {
        public string label { get; set; }
        public string data { get; set; }
        public string expandedIcon { get; set; }
        public string collapsedIcon { get; set; }
        public int id { get; set; }
        [CanBeNull]
        public List<TreeItem> children { get; set; }

        [CanBeNull]
        public string icon { get; set; }

        [CanBeNull]
        public string labelcompleto{ get; set; }
    }
}
