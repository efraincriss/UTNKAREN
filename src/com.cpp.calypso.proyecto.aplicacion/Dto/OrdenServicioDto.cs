using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(OrdenServicio))]
    [Serializable]
    public class OrdenServicioDto : EntityDto
    {

        [Obligado]
        [DisplayName("Código orden de servicio")]
        public string codigo_orden_servicio { get; set; }

        [DisplayName("Año")]
        public int anio { get; set; }

        [DisplayName("Referencias PO")]
        public string referencias_po { get; set; }
        public string comentarios { get; set; }

        [Obligado]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha orden servicio")]
        public virtual DateTime fecha_orden_servicio { get; set; }


        [DisplayName("Monto Aprobado")]
        public virtual decimal monto_aprobado_os { get; set; } = 0;


  
        [DisplayName("Monto Aprobado Suministros")]
        public virtual decimal monto_aprobado_suministros { get; set; } = 0;


        [DisplayName("Monto Aprobado Construcción")]
        public virtual decimal monto_aprobado_construccion { get; set; } = 0;

        
        [DisplayName("Monto Aprobado Ingeniería")]
        public virtual decimal monto_aprobado_ingeniería { get; set; }

        [DisplayName("Monto Aprobado Ingeniería")]
        public virtual decimal monto_aprobado_subcontrato { get; set; } = 0;

        [Obligado]
        [LongitudMayorAttribute(10)]
        [DisplayName("Version orden de servicio")]
        public string version_os { get; set; }

        [Obligado] public virtual bool vigente { get; set; } = true;


        [DisplayName("Estado")] public int EstadoId { get; set; }

        // Virtuales

        public virtual decimal monto_os_suministros { get; set; }

        public virtual decimal monto_os_construccion { get; set; }

        public virtual decimal monto_os_ingeniería { get; set; }

        public virtual decimal monto_os_total { get; set; }

        //Visualizacion de Orden de Servicio
        public virtual decimal ValorTotal { get; set; }
        public virtual decimal monto_presupuestado { get; set; }
        public virtual decimal monto_construccion { get; set; }
        public virtual decimal monto_ingenieria { get; set; }
        public virtual decimal total { get; set; }
        public virtual decimal oSmonto_presupuestado { get; set; }
        public virtual decimal oSmonto_construccion { get; set; }
        public virtual decimal oSmonto_ingenieria { get; set; }
        public virtual decimal oStotal { get; set; }
        public virtual decimal Pmonto_presupuestado { get; set; }
        public virtual decimal Pmonto_construccion { get; set; }
        public virtual decimal Pmonto_ingenieria { get; set; }
        public virtual decimal Ptotal { get; set; }
        public virtual string  ref_oferta { get; set; }
    }
}
