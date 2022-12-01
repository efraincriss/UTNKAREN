//using Abp.Application.Services.Dto;
//using Abp.AutoMapper;
//using com.cpp.calypso.comun.dominio;
//using com.cpp.calypso.proyecto.dominio;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace com.cpp.calypso.proyecto.aplicacion.Dto
//{
//    [AutoMap(typeof(OpcionComida))]
//    [Serializable]
//    public class OpcionComidaDto : EntityDto
//    {
//        [Obligado]
//        [DisplayName("Opción Comida")]
//        public int opcion_comida { get; set; }

//        [Obligado]
//        [StringLength(100)]
//        [DisplayName("Nombre")]
//        public string nombre { get; set; }

//        [Obligado]
//        [DisplayName("Costo")]
//        public decimal costo { get; set; }

//        [Obligado]
//        [DisplayName("Vigente")]
//        public bool vigente { get; set; } = true;
//    }
//}
