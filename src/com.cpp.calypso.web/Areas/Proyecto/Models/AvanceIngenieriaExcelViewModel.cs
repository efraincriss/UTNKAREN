using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class AvanceIngenieriaExcelViewModel
    {
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Presentación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechapresentacion { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechadesde { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Hasta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechahasta { get; set; }


        public HttpPostedFileBase UploadedFile { get; set; }
        public virtual List<AvanceIngenieriaExcel> ListaAvanceIngenieria { get; set; }
        public virtual List<AvanceIngenieriaExcel> ListaAvanceIngenieriaNoValidos { get; set; }

    }
}