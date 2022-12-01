using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Models
{
    public class ContratoDocumentoCentrosViewModel
    {
        public ContratoDto Contrato { get; set; }
        
        public ClienteDto Cliente { get; set; }
        public InstitucionFinancieraDto InstitucionFinanciera { get; set; }
        public EmpresaDto Empresa { get; set; }
        public List<ContratoDocumentoBancarioDto> ContratoDocumentoBancario { get; set; }
        public List<CentrocostosContratoDto> CentrocostosContrato { get; set; }
        public List<AdendaDto> Adenda{ get; set; }
        public List<ProyectoDto> Proyecto { get; set; }
        public List<Ganancia> Ganancias { get; set; }
    }
}