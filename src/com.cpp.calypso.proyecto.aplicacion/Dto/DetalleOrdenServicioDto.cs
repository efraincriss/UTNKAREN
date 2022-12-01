using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{

    [AutoMap(typeof(DetalleOrdenServicio))]
    [Serializable]
    public class DetalleOrdenServicioDto : EntityDto
    {
        [Obligado]
        [DisplayName("Orden de Servicio")]
        public virtual int OrdenServicioId { get; set; }

        public OrdenServicio OrdenServicio { get; set; }

        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

        public virtual Proyecto Proyecto { get; set; }



        [Obligado]
        [DisplayName("Grupo")]
        public virtual DetalleOrdenServicio.GrupoItems GrupoItemId { get; set; }


        [Obligado]
        [DisplayName("Valor Orden de Servicio")]
        public virtual decimal valor_os { get; set; }

        [Obligado] public virtual bool vigente { get; set; } = true;


        [DisplayName("Oferta Comercial")]
        public int OfertaComercialId { get; set; }
        public enum Item
        {
            Ingeniería = 1,
            Construcción = 2,
            Suministros = 3,
            SubContratos = 4,
        }

        public virtual string nombre_proyecto { get; set; }
        public virtual string codigo_proyecto { get; set; }
        public virtual string nombre_grupo { get; set; }

        public virtual string codigoOferta { get; set; }

    }
}
