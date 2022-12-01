using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework.Extensions;
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
    /// Informacion de la distribucion, detall de la distribucion y la solicitud de vianda
    /// </summary>
    [Serializable]
    public class DistribucionSolicitudDto : EntityDto
    {
       ///Id. (Id de distriubuccion)

        [Obligado]
        [DisplayName("Detalle Distribuccion Id")]
        public int detalle_distribuccion_id { get; set; }

        [Obligado]
        [DisplayName("Solicitud")]
        public int solicitud_id { get; set; }

        
        [DataType(DataType.Date)]
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_solicitud { get; set; }

        [DisplayName("Solicitante")]
        public int solicitante_id { get; set; }

        [DisplayName("Solicitante")]
        public string solicitante_nombre { get; set; }

        [Obligado]
        [DisplayName("Locacion")]
        public int LocacionId { get; set; }
        public virtual string locacion_nombre { get; set; }

        [Obligado]
        [DisplayName("Disciplina")]
        public int disciplina_id { get; set; }
        public virtual string disciplina_nombre { get; set; }

        [Obligado]
        [DisplayName("Area")]
        public int AreaId { get; set; }
        public virtual string area_nombre { get; set; }


        [Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
		public virtual string proveedor_nombre { get; set; }


        [DisplayName("Conductor Asignado")]
        public int? conductor_asignado_id { get; set; }
        public string conductor_asignado_nombre { get; set; }

  
        [DisplayName("Anotador")]
        public int? anotador_id { get; set; }
        public string anotador_nombre { get; set; }

        [Obligado]
        [DisplayName("Tipo Comida")]
        public int tipo_comida_id { get; set; }
        public virtual string tipo_comida_nombre { get; set; }

        /// <summary>
        /// Fecha de Distribucion
        /// </summary>
        [DataType(DataType.Date)]
		[DisplayName("Fecha")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime fecha { get; set; }

        /// <summary>
        /// Total de pedido en la solicitud de vianda
        /// </summary>
		[Obligado]
		[DisplayName("Total Pedido")]
		public int total_pedido { get; set; }

        public int pedido_viandas;


        public int alcance_viandas;

        
        [DisplayName("Total Consumido")]
        public int total_consumido { get; set; }


        /// <summary>
        /// Estado de la distribucion
        /// </summary>
        [Obligado]
        [DisplayName("Estado")]
        public DistribucionViandaEstado estado { get; set; }

        public string estado_nombre
        {
            get { return estado.GetDescription(); }
        }

        /// <summary>
        /// Estado de solicitud
        /// </summary>
        [Obligado]
        [DisplayName("Estado de la Solicitud")]
        public SolicitudViandaEstado estado_solicitud { get; set; }

        public string estado_solicitud_nombre
        {
            get { return estado_solicitud.GetDescription(); }
        }


    }
}
