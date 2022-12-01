using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface
{

    public interface IPacienteAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Paciente, PacienteDto, PagedAndFilteredResultRequestDto>
    {

         List<PacienteDto> pacientes();
        bool insertarEntidad(Paciente entity);
         bool Editar(Paciente entity);
        bool eliminarPaciente(int id);

        HijosPaciente detallePaciente(int id);

        bool insertarMNA(MNA entity);
        bool EditarMNA(MNA entity);
        bool eliminarMNA(int id);

        bool insertarKat(Katz entity);
        bool EditarKat(Katz entity);
        bool eliminarKat(int id);

        ExcelPackage Reporte();
        decimal ObtenerTotales(int PerdidaApetitoId, int PerdidaPesoId, int MovilidadId, int EnfermedadAgudaId, int ProblemasNeuroId, int IndiceMasaId, int ViveDomicilioId
            , int MedicamentoDiaId, int UlceraLesionId, int ComidaDiariaId, int ConsumoPersonaId, int ConsumoFrutasVerdurasId, int NumeroVasosAguaId, int ModoAlimentarseId, int ConsideracionEnfermoId, int EstadoSaludId,

       int CircunferenciaBraquialId, int CircunferenciaPiernaId);

        decimal ObtenerTotales2(int PerdidaApetitoId, int PerdidaPesoId, int MovilidadId, int EnfermedadAgudaId, int ProblemasNeuroId, int IndiceMasaId, int ViveDomicilioId
         , int MedicamentoDiaId, int UlceraLesionId, int ComidaDiariaId, int ConsumoPersonaId, int ConsumoFrutasVerdurasId, int NumeroVasosAguaId, int ModoAlimentarseId, int ConsideracionEnfermoId, int EstadoSaludId,


    int CircunferenciaBraquialId, int CircunferenciaPiernaId);



    }
}
