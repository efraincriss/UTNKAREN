using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{

    public class RegistroCivilDto
    {
       
        public string Calle { get; set; }
     
        public string CodigoError { get; set; }
    
        public string CondicionCedulado { get; set; }
      
        public string Conyuge { get; set; }

        public string Domicilio { get; set; }
    
        public string Error { get; set; }
    
        public string EstadoCivil { get; set; }
     
        public DateTime? FechaCedulacion { get; set; }

        public DateTime? FechaFallecimiento { get; set; }
    
        public DateTime? FechaMatrimonio { get; set; }
    
        public DateTime? FechaNacimiento { get; set; }

        public string Firma { get; set; }
   
        public string Fotografia { get; set; }

        public string Instruccion { get; set; }
       
        public string LugarNacimiento { get; set; }
     
        public string NUI { get; set; }

        public string Nacionalidad { get; set; }
        
        public string Nombre { get; set; }

        public string NumeroCasa { get; set; }
    
        public string Profesion { get; set; }
      
        public string Sexo { get; set; }

    }
}
