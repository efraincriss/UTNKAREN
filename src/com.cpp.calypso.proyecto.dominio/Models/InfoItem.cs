using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class InfoItem
    {
        public int Id { get; set; }
        public string codigo { get; set; }
        public string item_padre { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public bool para_oferta { get; set; }
        public int UnidadId { get; set; }
        public bool vigente { get; set; }
        public virtual int GrupoId { get; set; }
        public int EspecialidadId { get; set; }
        public bool PendienteAprobacion { get; set; }
        public string apicodigo { get; set; }
        public string NombreEspecialidad { get; set; }
        public int tieneHijos { get; set; }
    }
}
