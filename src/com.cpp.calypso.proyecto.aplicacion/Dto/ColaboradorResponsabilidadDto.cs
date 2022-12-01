using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ColaboradorResponsabilidad))]
    [Serializable]
    public class ColaboradorResponsabilidadDto : EntityDto
    {
        [DisplayName("Colaborador")]
        public int colaborador_id { get; set; }

        [DisplayName("Tipo de Cuenta")]
        public int catalogo_responsable_id { get; set; }

        [DisplayName("Acceso")]
        [LongitudMayor(1)]
        public string acceso { get; set; }
        
        public virtual Colaboradores Colaboradores { get; set; }
        
    }
}
