using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Dto
{
    [AutoMap(typeof(RutaHorario))]
    [Serializable]
    public class RutaHorarioDto:EntityDto
    {

        [Obligado]
        [DisplayName("Ruta")]
        public int RutaId { get; set; }


        [Obligado]
        [DisplayName("Horario")]
        public TimeSpan Horario { get; set; }

        public virtual string NombreRuta { get; set; }
    }
}
