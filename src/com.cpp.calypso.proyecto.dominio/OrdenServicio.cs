using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using Xunit.Sdk;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class OrdenServicio : Entity
    {

        /*[Obligado]
        [DisplayName("Oferta Comercial")]
        public int OfertaComercialId { get; set; }
        public virtual OfertaComercial OfertaComercial { get; set; }*/

        [DisplayName("Archivo")] public int? ArchivoId { get; set; }
        public virtual Archivo Archivo { get; set; }

        [Obligado]
        [DisplayName("Código orden de servicio")]
        [LongitudMayorAttribute(50)]
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
        public virtual decimal monto_aprobado_ingeniería { get; set; } = 0;


        [DisplayName("Monto Aprobado Ingeniería")]
        public virtual decimal monto_aprobado_subcontrato { get; set; } = 0;

        [DisplayName("Version orden de servicio")]
        public string version_os { get; set; }

        [Obligado] public virtual bool vigente { get; set; } = true;


        [DisplayName("Estado")] public int EstadoId { get; set; }
        public Catalogo Estado { get; set; }


    }
}
