using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.proyecto.dominio
{
    public class Facturas
    {
        public  List<FacturaExcel> Validas { get; set; }
        public List<FacturaExcel> NoValidas { get; set; }
    }
}