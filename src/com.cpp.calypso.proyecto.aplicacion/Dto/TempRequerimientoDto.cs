using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(TempRequerimiento))]
    [Serializable]
    public class TempRequerimientoDto  : EntityDto
    {
        [DisplayName("Proyecto Principal")]
        public string ProyectoPrincipal { get; set; }

        [DisplayName("Tipo Requerimiento")]
        public int TipoRequerimiento { get; set; }

        [DisplayName("Codigo")]
        public string Codigo { get; set; }


        [DisplayName("Fecha Recepción")]
        public DateTime? FechaRecepcion { get; set; }


        [DisplayName("Descripcion")]
        public string Descripcion { get; set; }

        [DisplayName("Solicitante")]
        public string Solicitante { get; set; }


        [DisplayName("Monto Ingeniería")]
        public string MontoIngenieria { get; set; }


        [DisplayName("Monto Construcción")]
        public string MontoConstruccion { get; set; }

        [DisplayName("Monto Suministro")]
        public string MontoSuministro { get; set; }

        public int RequerimeintoId { get; set; }

        public string CodigoOfertaAsiciada { get; set; }
    }
}
