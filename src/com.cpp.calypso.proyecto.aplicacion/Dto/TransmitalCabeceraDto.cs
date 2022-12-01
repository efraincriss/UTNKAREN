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
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(TransmitalCabecera))]
    [Serializable]
    public class TransmitalCabeceraDto :EntityDto
    {
        [DisplayName("Empresa")]
        public int EmpresaId { get; set; }

        [DisplayName("Cliente")]
        public int? ClienteId { get; set; }

        [DisplayName("Contrato")]
        public int? ContratoId { get; set; }


        [DisplayName("Oferta Comercial")]
        public int? OfertaComercialId { get; set; }

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
        [DisplayName("Enviado por")]
        public string enviado_por { get; set; }

        [Obligado]
        [DisplayName("Dirigido a")]
        public string dirigido_a { get; set; }

        [CanBeNull]

        [DisplayName("Con Copia a")]
        public string copia_a { get; set; }

        //
 
        [DisplayName("Versión")]
        [LongitudMayorAttribute(50)]
        public string version { get; set; }


        [Obligado]
        [DisplayName("Tipo de Formato")]
        public string tipo_formato { get; set; }

        [Obligado]
        [DisplayName("Tipo Proposito")]
        public string tipo_proposito { get; set; }

        [Obligado]
        [DisplayName("Tipo")]
        public string tipo { get; set; }


   
        [DisplayName("Estado")]
        public int estado { get; set; }

        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }

        public String[] SelectedValues { get; set; }
        public virtual string nombreEstado { get; set; }



        //Transmital nuevo
        public virtual string empresa { get; set; }
        public virtual string cliente { get; set; }     
        public virtual string contrato { get; set; }
        public virtual string codigo_oferta_comercial { get; set; }
        public virtual string version_oferta_comercial { get; set; }
        public virtual bool tiene_oferta { get; set; }
        public virtual bool tiene_ofertacomercial { get; set; }
        public virtual string code { get; set; }

        public virtual string format_fecha_emision { get; set; }

        public virtual List<Colaborador> listdirigidos { get; set; }
        public virtual List<Colaborador> listcopia { get; set; }
        public virtual List<Colaborador> listcopiaoculta { get; set; }

    }
}
