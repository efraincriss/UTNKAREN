using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace com.cpp.calypso.comun.dominio
{

    /// <summary>
    /// Representa un Parametros del sistema
    /// </summary>
    [Serializable]
    [DisplayName("Parámetros del Sistema")]
    public class ParametroSistema : AuditedEntity, IEntityNamed
    {
        public ParametroSistema()
        {
            Opciones = new List<ParametroOpcion>();
        }

        //public int Id { get; set; }

        [Obligado]
        [LongitudMayor(60)]
        [DisplayName("Código")]
        public string Codigo { get; set; }

        [Obligado]
        [LongitudMayor(80)]
        public string Nombre { get; set; }

        [LongitudMayor(255)]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Categoria del parametro. 
        /// </summary>
        [Obligado]
        [DisplayName("Categoría")]
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
        [DisplayNameAttribute("Es editable?")]
        public bool EsEditable
        {
            get;
            set;
        }

        /// <summary>
        /// Si el parametro posee listado de opciones permitidas para el valor del parametro. 
        /// Ejemplo Tipo Cadena. Listado de Opciones Permitidas
        /// </summary>
        [Obligado]
        [DisplayNameAttribute("Posee Opciones")]
        public bool TieneOpciones
        {
            get;
            set;
        }
        public int? ModuloId { get; set; }
        public Modulo Modulo { get; set; }

        /// <summary>
        /// Listado de opciones, de valores permitidas para el valor del parametro. Relacionada con la propiedad "TieneOpciones"
        /// </summary>
        public virtual ICollection<ParametroOpcion> Opciones { get; set; }

  
    }

    /// <summary>
    /// Tipos de Parametros
    /// </summary>
    public enum TipoParametro
    {
        Numero = 1,
        Cadena = 2,
        Booleano = 3,
        Listado = 4,
        Fecha = 5,
        /// <summary>
        /// Objetos Serializados. (Json, XML)
        /// </summary>
        Objeto_Serializado = 6,
        Imagen = 7
    }

    

    /// <summary>
    /// Categoria de Parametros
    /// </summary>
    public enum CategoriaParametro
    {
        General = 1
    }


}
