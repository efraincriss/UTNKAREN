using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Interface
{
    public interface IValidacionRequisitoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Colaboradores, ColaboradoresDto, PagedAndFilteredResultRequestDto>
    {
        List<ValidacionRequisitoDto> ObtenerRequisitos(InputRequisitosDto input);

        string FechasValidas(int requisitoId, DateTime fechaEmision, DateTime fechaCaducidad);

        int UpdateApi(CreateColaboradorRequisitoDto input);


        #region ES: Requisitos Reportes
        //REQUISITOS REPORTES
        ExcelPackage ExcelCumplimientoIndividual(InputRequisitosReporteDto input);
        ExcelPackage ExcelListaCumplimiento(InputRequisitosReporteDto input);

        List<Requisitos> ListaRequisitosReporte(int tipoaccion);
        List<RequisitosReporteDto> ObtenerRequisitosAcceso(InputRequisitosReporteDto input);
        List<RequisitosReporteDto> ObtenerRequisitosAccesoColaborador(InputRequisitosReporteDto input);
        List<ColaboradoresDetallesDto> BuscarPorIdentificacionNombre(string identificacion = "", string nombre = "");
        
        string AlertaFechas(DateTime? FechaCaducidad, int DiasVencimiento);

        #endregion

        #region ES: Asignacion Usuarios

        List<ModelAsiganciones> ListaAsignados(int colaboradorId);
        String DeleteAsigancion(int Id);
        String Asignar(int CatalogoResponsableId, int ColaboradorId, string acceso);
        List<ModelClassReactUser> buscarColaborador(string search);
        List<ModelClassReactUser> SearchUsuario(int colaboradorId, int catalogoResponsableId);
        List<ColaboradorResponsabilidad> SearchAsignacionesUsuario(int ColaboradorId);

        string ActualizaryCrear(ModelAsiganciones m);
        #endregion
    }
}
