using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.comun.aplicacion;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class Oferta : Entity
    {
        // Ofertas Presupuesto
        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

        public virtual Proyecto Proyecto { get; set; }

        [Obligado]
        [DisplayName("Requerimiento")]
        public int RequerimientoId { get; set; }

        public virtual Requerimiento Requerimiento { get; set; }


        [DisplayName("Clase")]
        public ClaseOferta? ClaseId { get; set; }


        [DisplayName("Descripción")]
        [LongitudMayorAttribute(800)]
        public virtual string descripcion { get; set; }

        [Obligado]
        [DisplayName("Versión")]
        [LongitudMayorAttribute(50)]
        public virtual string version { get; set; }

        [Obligado]
        [DisplayName("Código")]
        [LongitudMayorAttribute(50)]
        public string codigo { get; set; }

        [DisplayName("Alcance")]
        public string alcance { get; set; }

        [Obligado]
        [DisplayName("Definitiva")]
        public virtual bool es_final { get; set; }


        public Aprobacion estado_aprobacion { get; set; }


        public Emision estado_emision { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_registro { get; set; }


        public int OrigenDatosId { get; set; }

        [ForeignKey("OrigenDatosId")]
        public Catalogo OrigenDatos { get; set; }


        [Obligado]
        public bool vigente { get; set; } = true;

        public decimal monto_ingenieria { get; set; }

        public decimal monto_construccion { get; set; }

        public decimal monto_suministros { get; set; }

        public enum ClaseOferta
        {
            [Description("Budgetario")]
            [Display(Name = "Budgetario")]
            Budgetario = 1,

            [Description("Clase 1")]
            [Display(Name = "Clase 1")]
            Clase1 = 2,

            [Description("Clase 2")]
            [Display(Name = "Clase 2")]
            Clase2 = 3,

            [Description("Clase 3")]
            [Display(Name = "Clase 3")]
            Clase3 = 4,

            [Description("Clase 4")]
            [Display(Name = "Clase 4")]
            Clase4 = 5,

            [Description("Clase 5")]
            [Display(Name = "Clase 5")]
            Clase5 = 6,
        }

        public enum Aprobacion
        {
            [Description("Aprobado")]
            [Display(Name = "Aprobado")]
            Aprobado = 1,
            [Description("Pendiente de Aprobación")]
            [Display(Name = "Pendiente de Aprobación")]
            PendienteAprobacion = 2,

        }

        public enum Emision
        {
            [Description("Emitido")]
            [Display(Name = "Emitido")]
            Emitido = 1,
            [Description("En Preparación")]
            [Display(Name = "En Preparación")]
            EnPreparacion = 2,
            [Description("Por Emitir")]
            [Display(Name = "Por Emitir")]
            PorEmitir = 3,
        }

        public static string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }


        // Borrar desde aqui
        [DisplayName("Estatus del Proceso")]
        public virtual int estado_oferta { get; set; }

        [DisplayName("Estado")]
        public virtual int estado { get; set; }

        [DisplayName("Service Request")]
        public virtual bool service_request { get; set; }

        [DisplayName("Service Order")]
        public virtual bool service_order { get; set; }

        [DisplayName("Computo Completo")]
        public bool computo_completo { get; set; }

        [DisplayName("Monto total de orden de servicio")]
        public virtual decimal monto_so_referencia_total { get; set; }

        [DisplayName("Monto ofertado")]
        public virtual decimal monto_ofertado { get; set; }

        [DisplayName("Monto aprobado orden de servicio")]
        public virtual decimal monto_so_aprobado { get; set; }

        [DisplayName("Monto ofertado pendiente de aprobación")]
        public virtual decimal monto_ofertado_pendiente_aprobacion { get; set; }

        [DisplayName("Monto certifcado aprobado acumulado")]
        public virtual decimal monto_certificado_aprobado_acumulado { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Pliego")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_pliego { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha primer envío")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_primer_envio { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha de último envío")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_ultimo_envio { get; set; }

        [DisplayName("Dias de emisión oferta")]
        public virtual int dias_emision_oferta { get; set; }

        [DisplayName("Procetaje de avances")]
        public virtual decimal porcentaje_avance { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha última modificación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_ultima_modificacion { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha de recepción orden de servicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_recepcion_so { get; set; }

        [DisplayName("Días hasta recepcion de la orden de servicio")]
        public int dias_hasta_recepcion_so { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha de oferta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_oferta { get; set; }

        [DisplayName("Orden de Proceder")]
        public virtual bool orden_proceder { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Oden Proceder")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_orden_proceder { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Presupuesto")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_presupuesto { get; set; }

        [DisplayName("Presupuesto Emitido")]
        public bool presupuesto_emitido { get; set; }

        [DisplayName("OP Enviada por:")]
        public string orden_proceder_enviada_por { get; set; }

        
        [DisplayName("Tipo de Trabajo")]
        public int tipo_Trabajo_Id { get; set; }

        [DisplayName("Centro de Costos")]
        public int centro_de_Costos_Id { get; set; }

        [DisplayName("Estatus de Ejecución")]
        public int estatus_de_Ejecucion { get; set; }

        [DisplayName("Código SO Shaya")]
        public string codigo_shaya { get; set; }

        [DisplayName("Revisión de la Oferta")]
        public string revision_Oferta { get; set; }

        [DisplayName("Forma Contratación")]
        public int forma_contratacion { get; set; }

        [DisplayName("Acta de Cierre")]
        public int acta_cierre { get; set; }

        [DisplayName("Ingeniería")]
        public int Ingenieria { get; set; }
    }
}
