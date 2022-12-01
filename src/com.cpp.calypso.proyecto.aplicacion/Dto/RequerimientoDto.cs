using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.aplicacion
{

    [AutoMap(typeof(Requerimiento))]
    [Serializable]
    public class RequerimientoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

        [Obligado]
        [DisplayName("Tipo de Requerimiento")]
        public virtual TipoRequerimiento tipo_requerimiento { get; set; }

        [Obligado]
        [DisplayName("Código")]
        [StringLength(100)]
        public virtual string codigo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Recepción")]
        public virtual DateTime fecha_recepcion { get; set; }

        [Obligado]
        [DisplayName("Descripción")]
        [StringLength(800)]
        public string descripcion { get; set; }

        [DisplayName("Nombre de Solicitante")]
        [StringLength(100)]
        public string solicitante { get; set; }

        [Obligado]
        [DisplayName("Estado(Activo/Inactivo)")]
        public virtual bool estado { get; set; }

        [Obligado]
        [DisplayName("Monto Ingeniería")]
        public virtual decimal monto_ingenieria { get; set; }

        [Obligado]
        [DisplayName("Monto Construcción")]
        public decimal monto_construccion { get; set; }

        [Obligado]
        [DisplayName("Monto Procura")]
        public decimal monto_procura { get; set; }

        [DisplayName("Monto Subcontrato")]
        public decimal monto_subcontrato { get; set; } = 0;

        [Obligado]
        [DisplayName("Total")]
        public decimal monto_total { get; set; }

        [Obligado]
        [DisplayName("Requiere Cronograma")]
        public bool requiere_cronograma { get; set; }

        [Obligado]
        public bool vigente { get; set; }

        public virtual Proyecto Proyecto { get; set; }

        [JsonIgnore]
        public List<Oferta> Ofertas { get; set; }

        [JsonIgnore]
        public List<Novedad> Novedades { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha Límite de Cronograma")]
        public DateTime? fecha_limite_cronograma { get; set; }



        public virtual string cliente { get; set; }

        public virtual int dias_plazo_requerimiento { get; set; }

        public virtual string MsgCodigo { get; set; }

        public virtual string EstatusEjecucion { get; set; }

        public virtual string contrato_descripcion { get; set; }

        public virtual string proyecto_codigo { get; set; }
        public virtual string estado_presupuesto_actual { get; set; }

        //Nuevo datos Presupuestos

        [ForeignKey("Catalogo")]
        [DisplayName("Estado Presupuesto")]
        public int? estado_presupuesto { get; set; }
        public virtual Catalogo Catalogo { get; set; }


        //Nuevos Presupuestos
        [DisplayName("Última Versión")]
        public string ultima_version { get; set; }

        [DisplayName("Última Origen")]
        public string ultimo_origen { get; set; }

        [DisplayName("Última Clase")]
        public string ultima_clase { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha Carga de Cronograma")]
        public DateTime? fecha_carga_cronograma { get; set; }


        // alcance campo nuevo 08022019

        [DisplayName("Alcance")]
        public string alcance { get; set; }

       //
       public virtual string tiene_presupuesto { get; set; }



        //////
        ///

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha Maxima Presupuesto")]
        public DateTime? fecha_maxima_presupuesto { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha Maxima Oferta")]
        public DateTime? fecha_maxima_oferta { get; set; }

        [DisplayName("Dias para Presupuesto")]
        public int dias_para_presupuesto { get; set; } = 6;

        [DisplayName("Dias para Cronograma")]
        public int dias_para_cronograma { get; set; } = 7;

        [DisplayName("Dias para Oferta")]
        public int dias_para_oferta { get; set; } = 8;

        [DisplayName("Estado Requerimiento")]
        public int EstadoOfertaId { get; set; } = 0;

        [DisplayName("Alcance")]
        public virtual string nombreEstadoRequerimiento { get; set; }

        public virtual string correos_lista_distribucion { get; set; }
        public virtual OfertaComercial ofertaComercialLigada { get; set; }
    }
}
