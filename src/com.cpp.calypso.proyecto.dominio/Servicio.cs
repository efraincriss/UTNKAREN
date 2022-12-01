using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    /// <summary>
    /// Campusoft: Obsoleto, no utilizar esta clase ya que el servicio es un catalogo.
    /// (El cliente debe realizar refactorizacion del codigo en las partes que esta utilizando esta clase. Modelo realizado por el cliente) 
    /// </summary>
    [Serializable]
   //[Obsolete("Servicio es un catalogo")]
    public class Servicio : Entity, ISoftDelete
    {
        [Obligado]
        [DisplayName("Código")]
        public string codigo { get; set; }

        [Obligado]
        [DisplayName("Nombre")]
        [LongitudMayorAttribute(200)]
        public string nombre { get; set; }

        public bool IsDeleted { get; set; }
    }
}
