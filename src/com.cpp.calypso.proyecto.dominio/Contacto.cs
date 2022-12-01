using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;

namespace com.cpp.calypso.proyecto.dominio
{

    [Serializable]
    public class Contacto : Entity, IFullAudited
    {
        [DisplayName("Parroquia")]
        public int? ParroquiaId { get; set; }
        public virtual Parroquia Parroquia { get; set; }

        [DisplayName("Comunidad")]
        public int? ComunidadId { get; set; }
        public virtual Comunidad Comunidad { get; set; }

        [DisplayName("Parroquia Laboral")]
        public int? parroquia_parroquia_laboral_id { get; set; }

        [DisplayName("Comunidad Laboral")]
        public int? comunidad_comunidad_laboral_id { get; set; }

        [CanBeNull]
        [StringLength(100)]
        [DisplayName("Calle principal")]
        public string calle_principal { get; set; }

        [CanBeNull]
        [StringLength(10)]
        [DisplayName("Número")]
        public string numero { get; set; }

        [CanBeNull]
        [StringLength(100)]
        [DisplayName("Calle Secundaria")]
        public string calle_secundaria { get; set; }

        [CanBeNull]
        [StringLength(200)]
        [DisplayName("Referencia")]
        public string referencia { get; set; }

        [CanBeNull]
        [DisplayName("Teléfono Convencional")]
        public string telefono_convencional { get; set; }

        [CanBeNull]
        [StringLength(10)]
        [DisplayName("Celular")]
        public string celular { get; set; }

        [CanBeNull]
        [StringLength(50)]
        [DisplayName("Correo Electrónico")]
        public string correo_electronico { get; set; }

        [CanBeNull]
        [DisplayName("Comunidad Detalle")]
        public string detalle_comunidad { get; set; }

        [CanBeNull]
        [DisplayName("Parroquia Detalle")]
        public string detalle_parroquia { get; set; }

        [CanBeNull]
        [DisplayName("Comunidad Detalle")]
        public string detalle_comunidad_laboral { get; set; }

        [CanBeNull]
        [DisplayName("Parroquia Detalle")]
        public string detalle_parroquia_laboral { get; set; }

        [CanBeNull]
        [DisplayName("Código Postal")]
        public string codigo_postal { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;

        public string GetTelefonos()
        {
            if (!telefono_convencional.IsNullOrEmpty() && !celular.IsNullOrEmpty())
                return telefono_convencional + " / " + celular;

            if (celular.IsNullOrEmpty())
                return telefono_convencional;

            return celular;
        }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
