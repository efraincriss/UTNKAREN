using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Proveedor
{
    [Serializable]
    public  class TarifaLavanderia : Entity, ISoftDelete
    {
            [Obligado]
            [DisplayName("Tipo Servicio")]
            public int TipoServicioId { get; set; }
            public Catalogo TipoServicio { get; set; }


            [Obligado]
            [DisplayName("Contrato Proveedor")]
            public int ContratoProveedorId { get; set; }
            public ContratoProveedor ContratoProveedor { get; set; }

           
            [Obligado]
            [DisplayName("Valor por Servicio")]
            public decimal valor_servicio { get; set; }

            [Obligado] [DisplayName("Estado")] public bool estado { get; set; } = true;

            public string GetEstadoNombre()
            {
                return this.estado ? "ACTIVO" : "INACTIVO";
            }
            public bool IsDeleted { get; set; }
        }
}
