﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Dto
{
    [AutoMap(typeof(ConsumoTransporte))]
    [Serializable]
    public class ConsumoTransporteDto : EntityDto
    {
        public string TipoConsumo { get; set; }

        public int OperacionDiariaRutaId { get; set; }

        public string OperacionDiariaRutaRef { get; set; }

        public DateTime? FechaEmbarque { get; set; }

        public DateTime? FechaDesembarque { get; set; }

        public decimal CoordenadaXEmbarque { get; set; }

        public decimal CoordenadaYEmbarque { get; set; }

        public decimal CoordenadaXDesembarque { get; set; }

        public decimal CoordenadaYDesembarque { get; set; }

        public int? ColaboradorId { get; set; }


        public string Huella { get; set; }

        public DateTime? fs { get; set; }

        public DateTime? fr { get; set; }

        public int Version { get; set; }

        public string Ref { get; set; }

        public bool IsDeleted { get; set; }
    }
}
