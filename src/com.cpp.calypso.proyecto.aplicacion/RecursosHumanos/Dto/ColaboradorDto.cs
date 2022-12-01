using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto
{
    [AutoMap(typeof(Colaboradores))]
    [Serializable]
    public class ColaboradorDto : EntityDto
    {
        public int secuencial { get; set; }

        public string area_nombre { get; set; }

        public int? empleado_id_sap { get; set; }

        public string numero_identificacion { get; set; }

        public string nombres_apellidos { get; set; }

        public string estado { get; set; }

        public DateTime? fecha_baja { get; set; }
    }
}
