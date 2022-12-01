
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class DetalleOrdenServicio : Entity
    {
        [Obligado]
        [DisplayName("Orden de Servicio")]
        public virtual int OrdenServicioId { get; set; }

        public OrdenServicio OrdenServicio { get; set; }

        [DisplayName("Proyecto")]
        public  int ProyectoId { get; set; }

        public virtual Proyecto Proyecto { get; set; }


            [Obligado]
        [DisplayName("Grupo")]
        public virtual GrupoItems GrupoItemId { get; set; }


        [Obligado]
        [DisplayName("Valor Orden de Servicio")]
        public virtual decimal valor_os { get; set; }

        [Obligado] public virtual bool vigente { get; set; } = true;

        [DisplayName("Oferta Comercial")]
        public int OfertaComercialId { get; set; }
        public virtual OfertaComercial OfertaComercial { get; set; }

        public enum GrupoItems
        {
            Ingeniería = 1,
            Construcción = 2,
            Suministros = 3,
            SubContratos = 4,
        }
    }
}
