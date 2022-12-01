using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class DetalleItemIngenieria : Entity
    {
        [Obligado]
        [DisplayName("Detalle Avance Ingeniería")]
        public int DetalleAvanceIngenieriaId { get; set; }

        public DetalleAvanceIngenieria DetalleAvanceIngenieria { get; set; }

        [Obligado]
        [DisplayName("Colaborador")]
        public virtual int ColaboradorId { get; set; }

        public ColaboradorIngenieria Colaborador { get; set; }


        [Obligado]
        [DisplayName("Tipo de Registro")]
        public virtual int tipo_registro { get; set; }

        [Obligado]
        [DisplayName("Cantidad Horas")]
        public virtual decimal cantidad_horas { get; set; }

        [Obligado]
        [DisplayName("Fecha Registro")]
        public virtual DateTime fecha_registro { get; set; }

        [Obligado]
        [DisplayName("Etapa")]
        public virtual Etapa etapa { get; set; }

        [Obligado]
        [DisplayName("Especialidad")]
        public virtual int especialidad { get; set; }

        [Obligado]
        public virtual bool vigente { get; set; } = true;


        public enum Etapa
        {
            [Display(Name = "Ingeniería Básica")]
            ID = 1,
            [Display(Name = "Ingeniería Detalle")]
            IB = 2,
           [Display(Name = "Ingeniera Basica")]
            AB= 3,
            [Display(Name = "N/A")]
            NA = 4
        }
    }
}
