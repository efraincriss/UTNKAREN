using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(ProcesoListaDistribucion))]
    [Serializable]
    public class ProcesoListaDistribucionDto : EntityDto
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


        // Virtuales

        public virtual string nombre_lista { get; set; }


        public virtual string nombre_proceso{ get; set; }
    }
}
