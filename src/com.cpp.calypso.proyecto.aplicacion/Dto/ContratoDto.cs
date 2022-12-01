
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.aplicacion
{

    [AutoMap(typeof(Contrato))]
    [Serializable]
    public class ContratoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Código")]
        [LongitudMayorAttribute(200)]
        public string Codigo { get; set; }

        [Obligado]
        [DisplayName("Empresa")]
       
        public int EmpresaId { get; set; }
        public virtual Empresa Empresa { get; set; }
       
        [Obligado]
        [DisplayName("Cliente")]
        public int ClienteId { get; set; }

        [JsonIgnore]
        public virtual Cliente Cliente { get; set; }

        [Obligado]
        [DisplayName("Modalidad")]
        public int id_modalidad { get; set; }

        [Obligado]
        [LongitudMayorAttribute(200)]
        [DisplayName("Objeto")]
        public string objeto { get; set; }

        [Obligado]
        [LongitudMayorAttribute(800)]
        [DisplayName("Descripcion")]
        public string descripcion { get; set; }

        [Obligado]
        [DisplayName("Fecha Firma")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_firma { get; set; }

        [Obligado]
        [DisplayName("Fecha Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_inicio { get; set; }


        [DisplayName("Fecha Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_fin { get; set; }

        [Obligado]
        [DisplayName("Plazo para emisión de oferta(Días)")]
        public int dias_emision_oferta { get; set; }

        [Obligado]
        [DisplayName("Plazo para emisión de requerimiento")]
        public int dias_plazo_oferta_requerimiento { get; set; }

        [Obligado]
        [DisplayName("Plazo para facturación (Días)")]
        public int dias_plazo_factura { get; set; }

        [Obligado]
        [DisplayName("Plazo de pago (Días)")]
        public int dias_plazo_pago { get; set; }
        [Obligado]
        [DisplayName("Documento base para plazo de pago")]
        public DocumentoFacturaPlazo documento_factura_plazo { get; set; }

        [Obligado]
        [DisplayName("Plazo de Certificación Mensual (Días)")]
        public int dias_plazo_certificacion_mensual { get; set; }

        [Obligado]
        [DisplayName("Plazo de Garantía Ingeniería (Días)")]
        public int dias_garantia_ingenieria { get; set; }

        [Obligado]
        [DisplayName("Plazo de Garantía Suministros (Días)")]
        public int dias_garantia_suministros { get; set; }
        [Obligado]
        [DisplayName("Plazo de Garantía Construcción (Días)")]
        public int dias_garantia_construccion { get; set; }

        [Obligado]
        [DisplayName("Estado(Activo /Inactivo)")]
        [DefaultValue(true)]
        public bool estado_contrato { get; set; }

        [DisplayName("Fecha Acta Entrega-recepción")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_acta_entrega_recepcion { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        [DefaultValue(true)]
        public bool vigente { get; set; }

        [DisplayName("Código Generado")]
        [StringLength(100)]
        public string codigo_generado { get; set; }

        [DisplayName("Sitio Referencia")]
        public string sitio_referencia { get; set; }

        [DisplayName("Formato Contrato")]
        public FormatoContrato? Formato { get; set; }

        public virtual string NombreFormato { get; set; }



        [DisableValidation]
        [JsonIgnore]
        public virtual ICollection<ContratoDocumentoBancario> ContratoDocumentoBancarios { get; set; }



        [DisableValidation]
        [JsonIgnore]
        public virtual ICollection<CentrodecostosContrato> CentrodecostosContratos { get; set; }

        [DisableValidation]
        [JsonIgnore]
        public virtual ICollection<comun.dominio.Menu> Adendas { get; set; }

        [JsonIgnore]
        public virtual ICollection<Empresa> Empresas { get; set; }

        [JsonIgnore]
        public virtual ICollection<Cliente> Clientes { get; set; }

        [JsonIgnore]
        public virtual List<Proyecto> Proyectos { get; set; }

        public virtual string  nombrecompleto {get;set; }
      

    }
   

}