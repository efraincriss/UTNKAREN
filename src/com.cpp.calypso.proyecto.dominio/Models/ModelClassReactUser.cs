using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class ModelClassReactUser
    {
        public int dataKey { get; set; }
        public string label { get; set; }
        public int value { get; set; }
        public bool read { get; set; }
        public bool write { get; set; }
        public bool both { get; set; }
        public string other { get; set; }
    }
}
