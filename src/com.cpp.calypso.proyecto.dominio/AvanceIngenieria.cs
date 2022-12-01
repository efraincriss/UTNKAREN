﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class AvanceIngenieria : Entity
    {
        [Obligado]
        [DisplayName("Oferta")]
        public int OfertaId { get; set; }

  
        public virtual Oferta Oferta { get; set; }

        [DisplayName("Certificado")]
        public int CertificadoId { get; set; }

        [DisplayName("Descripción")]
        [LongitudMayor(800)]
        public string descripcion { get; set; }

        [DisplayName("Alcance")]
        public string alcance { get; set; }

        [DisplayName("Comentario")]
        [LongitudMayor(800)]
        public string comentario { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Presentación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_presentacion { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_desde { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Hasta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_hasta { get; set; }

        [Obligado]
        [DisplayName("Monto Ingeniería")]
        public decimal monto_ingenieria { get; set; }

        [Obligado]
        [DisplayName("Aprobado")]
        public bool aprobado { get; set; } = false;


        [DisplayName("Estado")]
        public EstadoIngenieria estado { get; set; }

        [Obligado] public bool vigente { get; set; } = true;





        public enum EstadoIngenieria
        {
            Generado = 1,
            Aprobado = 2,
            CertEmitido = 3,
            certAprobado = 4,
        }


    }
}
