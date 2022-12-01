using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class CreateCuentaEmpresaViewModel
    {
        public List<InstitucionFinanciera> InstitucionesFinancieras { get; set; }
        public CuentaEmpresaDto CuentaEmpresaDto { get; set; }
    }
}