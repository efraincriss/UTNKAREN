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
    [AutoMap(typeof(ConsumoAlimento))]
    [Serializable]
    public class ConsumoAlimentoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int colaborador_id { get; set; }
        public virtual Colaborador colaboradores { get; set; }

        [Obligado]
        [DisplayName("Proveedor")]
        public int proveedor_id { get; set; }
        public virtual Proveedor proveedores { get; set; }

        [Obligado]
        [DisplayName("Opcion Comida")]
        public int opcion_comida_id { get; set; }
        public virtual OpcionComida opciones_comida { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;
    }
}
