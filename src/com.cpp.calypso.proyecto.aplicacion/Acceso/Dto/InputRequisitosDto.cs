using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Dto
{
    public class InputRequisitosDto
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }

        [Obligado]
        [DisplayName("Agrupación para Requisitos")]
        public int GrupoPersonalId { get; set; }

        [Obligado]
        [DisplayName("Accion")]
        public int AccionId { get; set; }

        [DisplayName("Tipo Ausentismo")]
        public int? TipoBajaId { get; set; }
    }
}
