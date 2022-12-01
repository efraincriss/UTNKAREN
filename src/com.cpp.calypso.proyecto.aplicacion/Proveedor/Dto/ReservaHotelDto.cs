using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto
{
    [AutoMap(typeof(ReservaHotel))]
    [Serializable]
    public class ReservaHotelDto : EntityDto
    {

        public ReservaHotelDto()
        {
            estado = ReservaEstado.Activo;
            fecha_registro = new DateTime();
        }

        [Obligado]
        [DisplayName("Espacio Habitación")]
        public int EspacioHabitacionId { get; set; }


        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_registro { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_desde { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Hasta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_hasta { get; set; }

        public bool inicio_consumo { get; set; } = false;

        public DateTime? fecha_inicio_consumo { get; set; }
        public DateTime? fecha_fin_consumo { get; set; }

        public string justificacion_inicio_manual { get; set; }

        public string justificacion_finalizacion_manual { get; set; }

        public bool consumo_finalizado { get; set; } = false;
        [Obligado]
        [DisplayName("Estado")]
        public ReservaEstado estado { get; set; }

        [DisplayName("Extemporáneo")]
        public bool extemporaneo { get; set; } = false;


        [DisplayName("Documento Extemporáneo")]
        public int? DocumentoId { get; set; }
        //Archivos


        public bool aplica_lavanderia { get; set; } = false;



        public string estado_nombre { get; set; }


        public int? TipoHabitacionId { get; set; }

        public decimal Costo { get; set; }

        public string NombreTipoHabitacion { get; set; }
        public string NumeroHabitacion { get; set; }
        public string CodigoEspacio { get; set; }



        public string colaborador_nombres { get; set; }
        public string colaborador_identificacion { get; set; }
        public string colaborador_id_sap { get; set; }
        public string finalizado_manual { get; set; }
        public string iniciado_manual { get; set; }
        public string proveedor_razon_social { get; set; }

        public string numero_habitacion { get; set; }

        public string tipo_habitacion_nombre { get; set; }

        public string colaborador_grupo_personal { get; set; }

        public string es_externo { get; set; }
        public string es_extemporaneo { get; set; }

    }
}
