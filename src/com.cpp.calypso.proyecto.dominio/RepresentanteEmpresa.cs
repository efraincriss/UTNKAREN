﻿using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class RepresentanteEmpresa : Entity
    {
       
        [Obligado]
        [DisplayName("Tipo de Identificación")]
        public string tipo_identificacion { get; set; }

        [LongitudMayorAttribute(20)]
        [Obligado]
        [DisplayName("Identificación")]
        public string identificacion { get; set; }

        [LongitudMayorAttribute(50)]
        [Obligado]
        [DisplayName("Nombres Completos")]
        public string nombre { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Inicio")]
        public DateTime fecha_inicio { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Finalización")]
        public DateTime fecha_fin { get; set; }

        [Obligado]
        [DisplayName("Tipo de Representantes")]
        public string tipo_representante { get; set; }

        [Obligado]
        [DefaultValue(true)]
        [DisplayName("Estado del Representante(Activo/Inactivo)")]
        public bool estado_representante { get; set; }

        [Obligado]
        [DefaultValue(true)]
        public bool vigente { get; set; }

        public int EmpresaId { get; set; }

        public virtual Empresa Empresa { get; set; }

        public enum TipoIdentificacion
        {
            Ruc,
            Cedula,
            Passaporte,
        }

        public enum TipoRepresentante
        {
            Presidente,
            Representante
        }
    }
}
