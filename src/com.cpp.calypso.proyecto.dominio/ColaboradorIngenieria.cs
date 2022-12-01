
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ColaboradorIngenieria : Entity
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
        public Contrato Contrato { get; set; }

        [DisplayName("Cargo")]
        public int CargoId { get; set; }
        public DetallePreciario Cargo { get; set; }

        [DisplayName("Estado")]
        public TipoColaborador tipo { get; set; }

        public bool vigente { get; set; } = true;



    }
    public enum TipoColaborador
    {
        [Description("Directo")]
        Directo = 0,

        [Description("Indirecto")]
        Indirecto = 1,
    }

}
