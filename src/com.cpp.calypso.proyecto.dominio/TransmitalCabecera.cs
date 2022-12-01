using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.dominio
{

    [Serializable]
    public class TransmitalCabecera : Entity
    {
        [DisplayName("Empresa")]
        public int EmpresaId { get; set; }
        
        public virtual Empresa Empresa { get; set; }

        [DisplayName("Cliente")]
        public int? ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

        [DisplayName("Contrato")]
        public int? ContratoId { get; set; }
        public virtual Contrato Contrato{ get; set; }

        [DisplayName("Oferta Comercial")]
        public int? OfertaComercialId { get; set; }
        
        public virtual OfertaComercial OfertaComercial { get; set; }


        [Obligado]
        [LongitudMayorAttribute(100)]
        [DisplayName("Código del Transmital")]
        public string codigo_transmital { get; set; }

      
        [LongitudMayorAttribute(100)]
        [DisplayName("Código Carta")]
        public string codigo_carta { get; set; }


        [LongitudMayorAttribute(800)]
        [DisplayName("Descripción")]
        public string descripcion { get; set; }

        [Obligado]
        [DisplayName("Fecha de Emisión")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_emision { get; set; }

        [DisplayName("Fecha de Recepción")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_recepcion { get; set; }

        [DisplayName("Fecha de Ultima Módificacion")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_ultima_modificacion { get; set; }


        [Obligado]
        [LongitudMayorAttribute(100)]
        [DisplayName("Enviado por")]
        public string enviado_por { get; set; }

        [Obligado]
        [LongitudMayorAttribute(100)]
        [DisplayName("Dirigido a")]
        public string dirigido_a { get; set; }

        [CanBeNull]
        [LongitudMayorAttribute(100)]
        [DisplayName("Con Copia a")]
        public string copia_a { get; set; }

     
        [DisplayName("Estado")]
        public int estado { get; set; }

        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }



        [DisplayName("Versión")]
        [LongitudMayorAttribute(50)]
        public string version { get; set; }


      
        [DisplayName("Tipo de Formato")]
        public string tipo_formato { get; set; }

       
        [DisplayName("Tipo Proposito")]
        public string tipo_proposito { get; set; }

        
        [DisplayName("Tipo")]
        public string tipo { get; set; }

        
    }
}
