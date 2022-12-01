using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Dto
{
    [AutoMap(typeof(RutaParada))]
    [Serializable]
    public class RutaParadaDto:EntityDto
    {
        [Obligado]
        [DisplayName("Ruta")]
        public int RutaId { get; set; }

        [Obligado]
        [DisplayName("Parada")]
        public int ParadaId { get; set; }

        [DisplayName("Ordinal")]
        public int ordinal { get; set; } = 1;

        [Obligado]
        [DisplayName("Distancia")]
        public decimal Distancia { get; set; }

        public virtual decimal? Latitud { get; set; }
        public virtual decimal? Longitud { get; set; }
        public virtual string Referencia { get; set; }
        public virtual string NombreParada { get; set; }

    }
}
