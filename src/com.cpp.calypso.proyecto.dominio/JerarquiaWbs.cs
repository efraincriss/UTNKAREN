using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    public class JerarquiaWbs
    {
        public string label { get; set; }
        public string type { get; set; }
        public string className { get; set; }
        public bool expanded { get; set; }
        public string  data { get; set; }
        public List<JerarquiaWbs> children { get; set; }
    }

    
}
