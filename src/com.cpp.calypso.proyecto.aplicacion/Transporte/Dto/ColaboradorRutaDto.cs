using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
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
    [AutoMap(typeof(ColaboradorRuta))]
    [Serializable]
    public class ColaboradorRutaDto:EntityDto
    {

        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }

        [Obligado]
        [DisplayName("Rutas Horarios")]
        public int RutaHorarioId { get; set; }

        [DisplayName("Observacion")]
        [StringLength(100)]
        public string Observacion { get; set; }

        [DisplayName("Estado")]
        public ColaboradorRutaAsignada Estado { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Asignacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;

        [DisplayName("Usuario Asignacion")]
        [StringLength(100)]
        public string UsuarioAsignacion { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha DesAsignacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaDesAsignacion { get; set; }

        [DisplayName("Usuario Asignacion")]
        [StringLength(100)]
        public string UsuarioDesAsignacion { get; set; }

        public virtual string NombreOrigen { get; set; }
        public virtual string Horario { get; set; }
        public virtual string NombreDestino { get; set; }
        public virtual string Sector { get; set; }
        public virtual string NombreRuta{ get; set; }
        public virtual ColaboradoresDetallesDto Colaborador { get; set; }



}
}
