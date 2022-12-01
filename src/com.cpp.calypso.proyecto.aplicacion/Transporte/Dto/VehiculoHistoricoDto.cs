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
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Dto
{
    [AutoMap(typeof(VehiculoHistorico))]
    [Serializable]
    public class VehiculoHistoricoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Vehículo")]
        public int VehiculoId { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        [StringLength(3)]
        public string Estado { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Estado")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaEstado { get; set; }
    }
}
