using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Dto
{
    [AutoMap(typeof(Ruta))]
    [Serializable]
    public class RutaDto:EntityDto
    {
        [Obligado]
        [DisplayName("Código")]
        [StringLength(20)]
        public string Codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [DisplayName("Descripcion")]
        [StringLength(400)]
        public string Descripcion { get; set; }


        [Obligado]
        [DisplayName("Sector")]
        public int SectorId { get; set; }

        [Obligado]
        [DisplayName("Origen")]
        public int OrigenId { get; set; }
     
        [Obligado]
        [DisplayName("Destino")]
        public int DestinoId { get; set; }


        [Obligado]
        [DisplayName("Duración")]
        public int Duracion { get; set; }

        [Obligado]
        [DisplayName("Distancia")]
        public decimal Distancia { get; set; }

        public virtual string NombreOrigen { get; set; }

        public virtual string NombreDestino { get; set; }
        public virtual string NombreSector { get; set; }
    }
}
