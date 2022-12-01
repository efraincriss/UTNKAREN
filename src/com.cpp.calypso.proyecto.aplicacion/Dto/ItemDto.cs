using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(Item))]
    [Serializable]
    public class ItemDto: EntityDto
    {
        //[RegularExpression(@"^([1-9][0-9]*[.])$", ErrorMessage = "Ingresa un código de X. Ejemplo => 5. ")]
        [DisplayName("Código")]
        [LongitudMayorAttribute(100)]
        public virtual string codigo { get; set; }

        [DisplayName("Padre")]
        [LongitudMayorAttribute(50)]
        public virtual string item_padre { get; set; }

        [Obligado]
        [DisplayName("Nombre")]

        public virtual string nombre { get; set; }

        [Obligado]
        [DisplayName("Descripción")]
        public virtual string descripcion { get; set; }

        [Obligado]
        [DisplayName("Es para Oferta?(Si/No)")]
        public virtual bool para_oferta { get; set; }


        [ForeignKey("Catalogo")]
        [DisplayName("Unidad")]
        public virtual int UnidadId { get; set; }
        public virtual Catalogo Catalogo { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public virtual bool vigente { get; set; }

        [DisplayName("Grupo")]
        public virtual int GrupoId { get; set; }

        /*Formato Segundo Contrato*/
        [DisplayName("Especialidad")]
        public int? EspecialidadId { get; set; }

        /* */


        [DisplayName("Pendiente de Aprobación")]
        public bool PendienteAprobacion { get; set; }



        [DisplayName("Nombre Padre")]
        public string nombrepadre { get; set; }

        public virtual int OfertaId { get; set; }
        public virtual string apicodigo { get; set; }
        public virtual string NombreEspecialidad { get; set; }
    }
}
