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

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(Archivo))]
    [Serializable]
    public class ArchivoDto : EntityDto
    {
   
        [Obligado]
        [DisplayName("Código")]
        public string codigo { get; set; }


        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Obligado]
        [DisplayName("Fecha de Registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_registro { get; set; }

        public byte[] hash { get; set; }

        [DisplayName("Tipo Contenido")]
        public string tipo_contenido { get; set; }

        [DefaultValue(true)]
        public bool vigente { get; set; }

    }
}
