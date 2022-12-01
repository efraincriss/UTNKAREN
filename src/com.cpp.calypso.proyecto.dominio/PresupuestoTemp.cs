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

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class PresupuestoTemp : Entity
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
        public ClaseOferta? ClaseId { get; set; }


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

        public enum EstadoAprobacion
        {
            [Description("Aprobado")]
            [Display(Name = "Aprobado")]
            Aprobado = 1,
            [Description("Pendiente de Aprobación")]
            [Display(Name = "Pendiente de Aprobación")]
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

        public static string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }
    }
}
