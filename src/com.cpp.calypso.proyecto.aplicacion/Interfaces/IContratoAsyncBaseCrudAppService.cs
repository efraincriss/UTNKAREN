using com.cpp.calypso.comun.aplicacion;

using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IContratoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Contrato, ContratoDto, PagedAndFilteredResultRequestDto>
    {

        Task<ContratoDto> GetDetalle(int contratoId);

        Task CancelarVigencia(int contratoId);

        List<Contrato> GetContratos();

        List<ContratoDto> GetContratosDto();

        List<ContratoDocumentoBancarioDto> GetContratoDocumentoBancarios (int contratoId);
        List<CentrocostosContratoDto> GetCentrocostosContratos(int contratoId);
        
        List<AdendaDto> GetAdendas(int contratoId);
        List<ProyectoDto> GetProyectos (int contratoId);
        bool ComprobarYBorrarContrato(int ContratoId);

        ProyectoDto CrearProyectoporContratoAsync(ContratoDto Contrato,int idcontrato);

        List<ContratoDto> GetContratosporEC(int EmpresaId, int ClienteId);
        Task<bool> EliminarVigencia(int contratoId);

        EmpresasClientesContrato ListaEmpresaClienteporContrato(int ContratoId);

        ContratoDto InformacionContratoFromProyecto(int ProyectoId);

        List<InfoContrato> InfoContrato(int ClienteId);
        List<InfoCliente> InfoCliente();

        String OpenOutlook();
        String OpenOutlookProcess();

        ExcelPackage StackedColumn(ReportDto r);


        void ThisAddIn_Startup();
        


    }
}

