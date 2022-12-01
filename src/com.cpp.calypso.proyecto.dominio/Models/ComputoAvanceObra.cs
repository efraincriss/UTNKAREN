using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    public class ComputoAvanceObra
    {
        public int Id { get; set; }

        public string Actividad { get; set; }

        public string CodigoItem { get; set; }

        public string NombreItem { get; set; }

        public decimal CantidadAnterior { get; set; } = 0;

        public decimal CantidadAcumulada { get; set; } = 0;

        public int ComputoId { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal CantidadEAC { get; set; }

        public decimal Budget { get; set; }

        public virtual Wbs WbsActividad { get; set; }

         public virtual string padre_superior { get; set; }

        public virtual string padre_principal { get; set; }
        public virtual string unidad { get; set; }

        public virtual int GrupoId { get; set; }

        public virtual bool cantidadAjustada  { get; set; }
        public virtual bool Editado { get; set; }
        public virtual string tienecantidadAjustada { get; set; }
        public virtual string tipoCantidadAjustada { get; set; }
    }


}
