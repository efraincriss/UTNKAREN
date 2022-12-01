using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto
{
    [AutoMap(typeof(TarifaLavanderia))]
    [Serializable]
    public class TarifaLavanderiaDto: EntityDto
    {

        [Obligado]
        [DisplayName("Tipo Servicio")]
        public int TipoServicioId { get; set; }

        [Obligado]
        [DisplayName("Contrato Proveedor")]
        public int ContratoProveedorId { get; set; }



        [Obligado]
        [DisplayName("Valor por Servicio")]
        public decimal valor_servicio { get; set; }

        [Obligado] [DisplayName("Estado")] public bool estado { get; set; } = true;


        public string estado_nombre { get; set; }
        public string tipo_servicio_nombre { get; set; }

        public bool IsDeleted { get; set; }

        public int secuencial { get; set; }
        }
    }
