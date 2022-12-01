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

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(Ganancia))]
    [Serializable]
    public class GananciaDto :EntityDto
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


        public virtual List<Contrato> ListaContratos { get; set; }


    }
}
