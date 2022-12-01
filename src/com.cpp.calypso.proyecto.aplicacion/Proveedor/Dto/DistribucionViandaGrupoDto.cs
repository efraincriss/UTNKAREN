using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    
    /// <summary>
    /// Distribucion de viandas agrupados por fecha y tipo de comida
    /// </summary>
    [Serializable]
    public class DistribucionViandaGrupoDto : EntityDto
    {

        [Obligado]
        [DisplayName("Tipo Comida")]
        public int tipo_comida_id { get; set; }
        public virtual string tipo_comida_nombre { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha { get; set; }

        [Obligado]
        [DisplayName("Total Solicitudes")]
        public int total_solicitudes { get; set; }

        [Obligado]
		[DisplayName("Total Pedido")]
		public int total_pedido { get; set; }

       

    }
}
