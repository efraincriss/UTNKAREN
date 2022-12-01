using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ColaboradorIngenieria))]
    [Serializable]
    public class ColaboradorIngenieriaDto : EntityDto
    {

        [Obligado]
        [DisplayName("Número de Identifación")]
        public string numero_identificacion { get; set; }

        [Obligado]
        [DisplayName("Apellidos")]
        public string apellidos { get; set; }

        [Obligado]
        [DisplayName("Nombres")]
        public string nombres { get; set; }

        [DisplayName("Contrato")]
        public int ContratoId { get; set; }

        [DisplayName("Cargo")]
        public int CargoId { get; set; }
        public DetallePreciario Cargo { get; set; }

        [DisplayName("Estado")]
        public TipoColaborador tipo { get; set; }

        public bool vigente { get; set; } = true;

        public string nombreContrato { get; set; }
        public string tipoColaborador { get; set; }
        public string nombreCargo { get; set; }
        public string codigoCargo { get; set; }
    }
}
