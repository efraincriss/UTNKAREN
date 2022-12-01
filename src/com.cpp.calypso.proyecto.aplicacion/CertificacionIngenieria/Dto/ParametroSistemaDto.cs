using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto
{
    [AutoMap(typeof(ParametroSistema))]
    [Serializable]
    public class ParametroSistemaDto : EntityDto
    {
        [Obligado]
        [LongitudMayor(60)]
        public string Codigo { get; set; }

        [Obligado]
        [LongitudMayor(80)]
        public string Nombre { get; set; }

        [LongitudMayor(255)]
        public string Descripcion { get; set; }

        /// <summary>
        /// Categoria del parametro. 
        /// </summary>
        [Obligado]
        public CategoriaParametro Categoria { get; set; }

        /// <summary>
        /// Tipo del Parametro. (Valor Simple, Lista de Valores, Json)
        /// </summary>
        [Obligado]
        public TipoParametro Tipo { get; set; }

        /// <summary>
        /// Valor del parametro
        /// </summary>
        //[LongitudMayor(255)]
        [Obligado]
        public virtual string Valor { get; set; }

        /// <summary>
        /// Si el parametro es editable por el usuario (UI)
        /// </summary>
        [Obligado]
        public bool EsEditable { get; set; }

        /// <summary>
        /// Si el parametro posee listado de opciones permitidas para el valor del parametro. 
        /// Ejemplo Tipo Cadena. Listado de Opciones Permitidas
        /// </summary>
        [Obligado]
        public bool TieneOpciones { get; set; }

        public int? ModuloId { get; set; }
    }
}
