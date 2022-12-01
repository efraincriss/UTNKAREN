using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class EmpresaRepresentanteCuentaViewModel
    {

        public EmpresaDto Empresa { get; set; }
        public List<RepresentanteEmpresaDto> RepresentnatesEmpresa { get; set; }
        public List<CuentaEmpresaDto> CuentasEmpresa { get; set; }
    }
}