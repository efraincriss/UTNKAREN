using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(Colaboradores))]
    [Serializable]
    public class ColaboradorNombresDto : EntityDto
    {
        public int secuencial { get; set; }

        public string nombres_apellidos { get; set; }

        public string numero_identificacion { get; set; }

        public string tipo_identificacion_nombre { get; set; }

        public string departamento { get; set; }

        public string condicion { get; set; }

        public string grupo_personal { get; set; }
        public int grupo_personal_id { get; set; }

        public string estado { get; set; }

        public string tiene_derecho { get; set; }

        public string fechaIngreso { get; set; }

    }
}
