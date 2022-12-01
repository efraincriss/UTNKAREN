using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ProcesoListaDistribucion : Entity
    {
        [Obligado]
        [DisplayName("Proceso")]
        public int ProcesoNotificacionId { get; set; }

        public ProcesoNotificacion ProcesoNotificacion { get; set; }

        [Obligado]
        [DisplayName("Lista de distribución")]
        public int ListaDistribucionId { get; set; }


        public ListaDistribucion ListaDistribucion { get; set; }

        public bool vigente { get; set; } = true;

    }
}
