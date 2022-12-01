using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class ColaboradorBajaTemp
    {
        public int Id { get; set; }
        public string empleado_id_sap { get; set; }
        public string numero_identificacion { get; set; }
        public string numero_legajo_temporal { get; set; }
        public string apellidos_nombres { get; set; }
        public string nombres { get; set; }
        public DateTime fecha_baja { get; set; }
        public string motivo_baja {get;set;}
    }
}
