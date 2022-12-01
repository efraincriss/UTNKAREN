using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(DetalleItemIngenieria))]
    [Serializable]
    public class DetalleItemIngenieriaDto : EntityDto
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
        public virtual DetalleItemIngenieria.Etapa etapa { get; set; }

        [Obligado]
        [DisplayName("Especialidad")]
        public virtual int especialidad { get; set; }

        [Obligado]
        public virtual bool vigente { get; set; } = true;


        // Virtuales

        public virtual string nombre_especialidad { get; set; }

        public virtual string nombre_colaborador { get; set; }

        public virtual string nombre_etapa { get; set; }
    }
}
