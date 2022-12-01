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

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IColaboradorCargaSocialAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradorCargaSocial, ColaboradorCargaSocialDto, PagedAndFilteredResultRequestDto>
    {
		List<ColaboradorCargaSocialDto> GetCargas(int Id);
        string UniqueIdentification(int Id, string nro);
        ColaboradorCargaSocialDto GetInfoCargaSocialWS(int tipoIdentificacion, string cedula);

        ExcelPackage reporteInformacionGeneral(ColaboradorReporteDto colaborador);
    }
}
