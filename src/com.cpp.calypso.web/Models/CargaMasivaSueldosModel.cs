using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Models
{
    public class CargaMasivaSueldosModel
    {
        public string observaciones { get; set; }

        public string fecha { get; set; }

        public HttpPostedFileBase file { get; set; }
    }
}