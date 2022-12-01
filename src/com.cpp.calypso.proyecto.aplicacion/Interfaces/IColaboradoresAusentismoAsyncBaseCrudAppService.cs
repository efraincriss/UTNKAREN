using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.seguridad.aplicacion;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IColaboradoresAusentismoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradoresAusentismo, ColaboradoresAusentismoDto, PagedAndFilteredResultRequestDto>
    {
        List<ColaboradoresAusentismoDto> GetAusentismos();
        ColaboradoresAusentismo GetAusentismo(int Id);

        string CrearAusentismoAsync(ColaboradoresAusentismoDto colaboradoresAusentismo);

        string ActualizarAusentismoAsync(ColaboradoresAusentismo colaboradoresAusentismo);
        bool EliminarAusentismo(int Id);

        List<ColaboradoresAusentismoDto> GetAusentismosColaborador(int id);
        ExcelPackage reporteInformacionGeneral(ColaboradorReporteDto colaborador);

        bool ActualizarAusentismoColaborador(int ColaboradorId);


        #region Editar y Recuperar Archivos Ausentismos-Busqueda

       
        ColaboradoresAusentismoRequisitosDto ObtenerArchivos(int Id); //Ausentismo Id


        void SubirPdf(int ColaboradorAusentismoRequisitoId, ArchivoDto archivo);

        int EditarAusentismo(ColaboradoresAusentismo e);

        bool ValidarExisteAusentimo(int tipoAusentimosId, int ColaboradorId, DateTime fechaDesde, DateTime fechaHasta, int Id);

        bool DeleteAusentimo(int Id);

        #endregion


    }
}
