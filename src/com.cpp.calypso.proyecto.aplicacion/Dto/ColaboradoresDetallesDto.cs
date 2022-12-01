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
    public class ColaboradoresDetallesDto : EntityDto
    {
        public string TipoIdentificacionNombre { get; set; }

        public string Identificacion { get; set; }

        public int GrupoPersonalId { get; set; }

        public string Departamento { get; set; }

        public string PrimerNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoNombre { get; set; }

        public string SegundoApellido { get; set; }

        public string NombresApellidos { get; set; }

        public string CargoNombre { get; set; }

        public string PaisNombre { get; set; }

        public string ProvinciaNombre { get; set; }

        public string Calle { get; set; }

        public string Interseccion { get; set; }

        public string ContactoEmergencia { get; set; } //Todo

        public string Comuna { get; set; } //Todo

        public string TelefonoDomicilio { get; set; }

        public string TelefonoCelular { get; set; }

        public string FechaInduccion { get; set; } //Todo

        public string Nacionalidad { get; set; } //ToDo

        public string  Ciudad { get; set; }

        public string Parroquia { get; set; }

        public string NumeroCasa { get; set; }

        public string Correo { get; set; }

        public string IngresoBpm { get; set; } //Todo

        public string RelacionFamiliar { get; set; } //Todo

        public string Direccion { get; set; } //Todo

        public string Genero { get; set; }

        public string GrupoPersonal { get; set; }
        public string EncargadoPersonal { get; set; }

        public string IdSap { get; set; }

        public string Discapacidad { get; set; }

        public string TipoDiscapacidad { get; set; }

        public string Telefonos { get; set; }

        public string FechaRegistro{ get; set; } //Todo


    }
}
