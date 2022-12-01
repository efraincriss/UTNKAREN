using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class TempCartas : Entity
    {
    public string cliente { get; set; }
        public string numerocarta { get; set; }
        public DateTime fecharecepcion { get; set; }
        public DateTime? fechasello { get; set; }
        public string asunto { get; set; }
        public string enviadopor { get; set; }
        public string dirigido { get; set; }
        public string requiererespuesta { get; set; }
        public string nrocartarecibida { get; set; }
        public string nrocartaenviada { get; set; }
        public string descripcion { get; set; }
        public string para { get; set; }
        public string commentarios { get; set; }
    }
}
