using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(Secuencial))]
    [Serializable]
    public class SecuencialDto : EntityDto
    {
        [Obligado]
        public string nombre { get; set; }

        [Obligado]
        public int secuencia { get; set; }
    }
}
