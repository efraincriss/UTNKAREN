using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
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
    public class MNA : Entity, IFullAudited

    {
        [Required]
        public int PacienteId { get; set; }

        public Paciente Paciente { get; set; }


        
        [DisplayName("FechaTrabajo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }


        public int PerdidaApetitoId { get; set; }

        public Catalogo PerdidaApetito { get; set; }

        public int PerdidaPesoId { get; set; }

        public Catalogo PerdidaPeso { get; set; }


        public int MovilidadId { get; set; }

        public Catalogo Movilidad { get; set; }

        public int EnfermedadAgudaId { get; set; }

        public Catalogo EnfermedadAguda { get; set; }

        public int ProblemasNeuroId { get; set; }

        public Catalogo ProblemasNeuro { get; set; }

        public int IndiceMasaId { get; set; }

        public Catalogo IndiceMasa { get; set; }


        public int ViveDomicilioId { get; set; }

        public Catalogo ViveDomicilio { get; set; }



        public int MedicamentoDiaId { get; set; }

        public Catalogo MedicamentoDia { get; set; }


        public int UlceraLesionId { get; set; }

        public Catalogo UlceraLesion { get; set; }


        public int ComidaDiariaId { get; set; }

        public Catalogo ComidaDiaria { get; set; }


        public int ConsumoPersonaId { get; set; }

        public Catalogo ConsumoPersona { get; set; }


        public bool  ConsumeLacteos { get; set; }

        public bool ConsumeLegumbres { get; set; }

        public bool ConsumeCarne { get; set; }

        public int ConsumoFrutasVerdurasId { get; set; }

        public Catalogo ConsumoFrutasVerduras { get; set; }

        public int NumeroVasosAguaId { get; set; }

        public Catalogo NumeroVasosAgua { get; set; }

        public int ModoAlimentarseId { get; set; }

        public Catalogo ModoAlimentarse { get; set; }

        public int ConsideracionEnfermoId { get; set; }

        public Catalogo ConsideracionEnfermo { get; set; }


        public int EstadoSaludId { get; set; }

        public Catalogo EstadoSalud { get; set; }


        public int CircunferenciaBraquialId { get; set; }

        public Catalogo CircunferenciaBraquial { get; set; }

        public int CircunferenciaPiernaId { get; set; }

        public Catalogo CircunferenciaPierna { get; set; }


        public decimal Puntuacion { get; set; }

        public string DetallePuntuacion { get; set; }

        public bool ValoracionCompleta { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }

}