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
    [AutoMap(typeof(QrColaboradores))]
    [Serializable]
    public class QrColaboradoresDto : EntityDto
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Matrimonio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaEntrega { get; set; }

        public int NumeroQrGenerados { get; set; } = 0;

        [DataType(DataType.Date)]
        [DisplayName("Fecha Reimpresión")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaReimpresion { get; set; }

        public bool vigente { get; set; } = true;


    }
}
