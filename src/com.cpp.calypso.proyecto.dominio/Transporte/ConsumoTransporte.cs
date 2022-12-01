﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Transporte
{
    [Serializable]
    public class ConsumoTransporte : Entity, ISoftDelete
    {
        public string TipoConsumo { get; set; }

        public int OperacionDiariaRutaId { get; set; }

        public OperacionDiariaRuta OperacionDiariaRuta { get; set; }

        public string OperacionDiariaRutaRef { get; set; }

        public DateTime? FechaEmbarque { get; set; }

        public DateTime? FechaDesembarque { get; set; }

        public decimal CoordenadaXEmbarque { get; set; }

        public decimal CoordenadaYEmbarque { get; set; }

        public decimal CoordenadaXDesembarque { get; set; }

        public decimal CoordenadaYDesembarque { get; set; }

        public int? ColaboradorId { get; set; }

        public Colaboradores Colaborador { get; set; }

        public string Huella { get; set; }

        public DateTime? fs { get; set; }

        public DateTime? fr { get; set; }

        public bool IsDeleted { get; set; }

        public string uid { get; set; }
    }
}
