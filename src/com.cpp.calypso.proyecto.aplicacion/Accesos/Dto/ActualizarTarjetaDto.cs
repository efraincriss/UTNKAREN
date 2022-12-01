using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ActualizarTarjetaDto
    {
        [Obligado]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_vencimiento { get; set; }

        [DisplayName("Observaciones")]
        [StringLength(200)]
        public string observaciones { get; set; }
    }
}
