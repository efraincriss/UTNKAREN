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

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(OrdenServicioTemp))]
    [Serializable]
    public class OrdenServicioTempDto : EntityDto
    {
        [Obligado]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha orden servicio")]
        public DateTime Fecha { get; set; }

        public string ProyectoReferencia { get; set; }

        public string CodigoProyecto { get; set; }


        [DisplayName("Proyecto")]
        public  int ProyectoId { get; set; }


        public string CodigoOrden { get; set; }


        public string ReferenciaOrdenes { get; set; }

        public int Anio { get; set; }

        public string Oferta { get; set; }

        public decimal Ingenieria { get; set; }

        public Decimal Compras { get; set; }

        public Decimal Construccion { get; set; }

        public Decimal Total { get; set; }

        public string MigracionExitosa { get; set; }
    }
}
