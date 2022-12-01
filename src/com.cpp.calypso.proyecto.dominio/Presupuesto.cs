using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class Presupuesto : Entity
    {
        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }

        public virtual Proyecto Proyecto { get; set; }

        [Obligado]
        [DisplayName("Requerimiento")]
        public int RequerimientoId { get; set; }

        public virtual Requerimiento Requerimiento { get; set; }


        [DisplayName("Clase")]
        public ClasePresupuesto? Clase { get; set; }


        [DisplayName("Descripción")]
        [LongitudMayor(800)]
        public virtual string descripcion { get; set; }

        [Obligado]
        [DisplayName("Versión")]
        [LongitudMayor(50)]
        public virtual string version { get; set; }

        [Obligado]
        [DisplayName("Código")]
        [LongitudMayor(50)]
        public string codigo { get; set; }

        [DisplayName("Alcance")]
        public string alcance { get; set; }

        [Obligado]
        [DisplayName("Definitiva")]
        public virtual bool es_final { get; set; }


        public EstadoAprobacion estado_aprobacion { get; set; }


        public EstadoEmision estado_emision { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_registro { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? fecha_actualizacion { get; set; }


        public int OrigenDeDatosId { get; set; }

        [ForeignKey("OrigenDeDatosId")]
        public Catalogo OrigenDatos { get; set; }

        [Obligado]
        public bool vigente { get; set; } = true;

        public decimal monto_ingenieria { get; set; }

        public decimal monto_construccion { get; set; }

        public decimal monto_suministros { get; set; }

        public decimal monto_subcontratos{ get; set; }
        public decimal monto_total { get; set; } = 0;

        [Obligado]
        [DisplayName("Computo Completo")]
        public bool computo_completo { get; set; } = false;

        public enum ClasePresupuesto
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

        [ForeignKey("Catalogo")]
        [DisplayName("Origen")]
        public int? origen { get; set; }
        public virtual Catalogo Catalogo { get; set; }


        public decimal descuento { get; set; }

        [DisplayName("Justificación Descuento")]
        public string justificacion_descuento { get; set; }

        [DisplayName("Asunto Mail")]
        public string asuntoCorreo { get; set; }

        [DisplayName("Descripcion Mail")]
        public string descripcionCorreo { get; set; }

        public enum EstadoAprobacion
        {
            [Description("Emitido")]
            [Display(Name = "Emitido")]
            Aprobado = 1,
            [Description("Pendiente de Emisión")]
            [Display(Name = "Pendiente de Emisión")]
            PendienteAprobacion = 2,

        }

        public enum EstadoEmision
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

    }
}
