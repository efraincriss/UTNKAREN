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
    [AutoMap(typeof(Certificado))]
    [Serializable]
    public class CertificadoDto: EntityDto
    {

        [Obligado]
        [DisplayName("Proyecto")]
        public int ProyectoId { get; set; }
        public virtual Proyecto Proyecto { get; set; }

        [DisplayName("Fecha Corte")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime fecha_corte { get; set; }

        [DisplayName("Fecha Emisión")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime fecha_emision { get; set; }


        [DisplayName("Tipo Certificado")]
        public virtual int tipo_certificado { get; set; }


        [Obligado]
        [DisplayName("Número Certificado")]
        public virtual string numero_certificado { get; set; }

        [DisplayName("Porcentaje Avance")]
        public virtual decimal porcentaje_avance { get; set; }

        [DisplayName("Presupuesto Inicial")]
        public virtual decimal presupuesto_inicial { get; set; }

        [DisplayName("Monto Certificado")]
        public virtual decimal monto_certificado { get; set; }

        [DisplayName("Monto Pendiente")]
        public virtual decimal monto_pendiente { get; set; }

        [DisplayName("Estado")]
        public virtual int estado_actual { get; set; }

        [Obligado] public virtual bool vigente { get; set; } = true;

        [DisplayName("Tiene GR")]
        public virtual bool tiene_GR { get; set; }


        public virtual string periodo { get; set; }

        [DisplayName("Empresa")]
        public virtual int EmpresaId { get; set; }
        [DisplayName("Cliente")]
        public virtual int ClienteId { get; set; }
        [DisplayName("Contrato")]
        public virtual int ContratoId { get; set; }

        public virtual ICollection<Empresa> Empresas { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual ICollection<Contrato> Contratos { get; set; }
        public virtual ICollection<ProyectoDto> Proyectos { get; set; }

        //para facturas
        [DisplayName("Fecha Vencimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_vencimiento { get; set; }
    }
}
