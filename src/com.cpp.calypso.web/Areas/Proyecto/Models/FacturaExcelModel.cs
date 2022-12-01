using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class FacturaExcelModel
    {
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadedFile { get; set; }
        public virtual List<FacturaExcel> ListaFacturas { get; set; }
        public virtual List<FacturaExcel> ListaFacturasNovalidas { get; set; }
    }
}