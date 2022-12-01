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
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Dto
{
    [AutoMap(typeof(Parada))]
    [Serializable]
    public class ParadaDto : EntityDto
    {
        [Obligado]
        [DisplayName("Código")]
        [StringLength(20)]
        public string Codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [DisplayName("Latitud")]
        [Range(-180.000000, 180.000000, ErrorMessage = "Campo Latitud es inválido")]
        public decimal? Latitud { get; set; }

        [DisplayName("Longitud")]
        [Range(-180.000000, 180.000000, ErrorMessage = "Campo Longitud es inválido")]
        public decimal? Longitud { get; set; }

        [DisplayName("Referencia")]
        [StringLength(400)]
        public string Referencia { get; set; }

        public int  Secuencial { get; set; }
    }
}
