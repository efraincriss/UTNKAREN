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
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ContactoEmergencia))]
    [Serializable]
    public class ContactoEmergenciaDto : EntityDto
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }

        [Obligado]
        [DisplayName("Nombres Completos")]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Obligado]
        [DisplayName("Identificación")]
        [StringLength(20)]
        public string Identificacion { get; set; }

        [DisplayName("Relación")]
        public int Relacion { get; set; }

        [DisplayName("Urbanización / Comuna")]
        [StringLength(100)]
        public string UrbanizacionComuna { get; set; }

        [DisplayName("Dirección")]
        [StringLength(150)]
        public string Direccion { get; set; }

        [DisplayName("Teléfono")]
        [StringLength(20)]
        public string Telefono { get; set; }

        [DisplayName("Celular")]
        [StringLength(20)]
        public string Celular { get; set; }

        public virtual string nombre_relacion { get; set; }
        public virtual int nro { get; set; }
    }
}
