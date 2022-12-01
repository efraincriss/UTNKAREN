using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
   
    [Serializable]
    public class ArchivoOrdenProceder: Entity //, IFullAudited
    {
        [Obligado]
        public int ofertaComercialId { get; set; }
        public OfertaComercial OfertaComercial { get; set; }

        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Obligado]
        [DisplayName("Fecha de Registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_registro { get; set; }

        public byte[] hash { get; set; }

        [DisplayName("Tipo Contenido")]
        public string tipo_contenido { get; set; }

    }
}
