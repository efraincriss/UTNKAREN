using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{

    [AutoMap(typeof(Paciente))]
    [Serializable]
    public class PacienteDto : EntityDto
    {

        public string Identificacion { get; set; }
        public string NombresApellidos { get; set; }
        public decimal Peso { get; set; }

        public decimal Edad { get; set; }
        public int SexoId { get; set; }

        public Catalogo Sexo { get; set; }
        public decimal Talla { get; set; }



        public virtual string  sexoString { get; set;}

    public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
