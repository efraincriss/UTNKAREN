using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Dto
{
    public class InputRequisitosReporteDto
    {

        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }

        public List<ColaboradoresDetallesDto>  Colaboradores { get; set; }

        [DisplayName("Accion")]
        public int AccionId { get; set; }

        [DisplayName("Departamento")]
        public int DepartamentoId { get; set; }

        public bool Vencidos { get; set; }
        public bool Obligatorios { get; set; }

        public int DiasVencimiento { get; set; }

        public string NombreColaborador { get; set; }
        public string Identificacion { get; set; }
    }
}
