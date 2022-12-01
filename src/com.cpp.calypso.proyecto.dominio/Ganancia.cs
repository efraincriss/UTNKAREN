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
    public class Ganancia: Entity
    {
        [Obligado]
        [DisplayName("Contrato")]
        public int ContratoId { get; set; }
        public virtual Contrato Contrato { get; set; }

       
        [Obligado]
        [DisplayName("Fecha de Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_inicio { get; set; }


        [DisplayName("Fecha de Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_fin { get; set; }


        [DisplayName("Estado(Activo/Inactivo)")]
        public bool estado_ganacia { get; set; } = true;

 
        [DefaultValue(true)]
        public bool vigente { get; set; }


     
    }
}
