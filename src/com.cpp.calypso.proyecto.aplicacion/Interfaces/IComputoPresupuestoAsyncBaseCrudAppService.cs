using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public interface IComputoPresupuestoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ComputoPresupuesto, ComputoPresupuestoDto, PagedAndFilteredResultRequestDto>
    {
        List<ComputoPresupuestoDto> GetComputosPorPresupuesto(int id);

        bool EliminarVigencia(int ComputoId);

        bool comprobarexistenciaitem(int WbsPresupuestoId, int ItemId);

        int GetComputosporOfertaProcura(int PresupuestoId);

        bool EditarCantidadComputo(int id, decimal cantidad, decimal cantidad_eac);

        ExcelPackage GenerarExcelCabecera(PresupuestoDto presupuesto);
        string nombrecatalogo2(int tipocatagoid);

        ComputoPresupuesto editarcomputoexiste(int WbsPresupuestoId, int ItemId,int PresupuestoId);


        //Cálculo de Presupuesto

        ComputoPresupuesto ActualizarprecioAjustado(ComputoPresupuesto seleccionado);
        String ActualizarCostoTotal(int ofertaid, int ContratoId, int PreciarioId, 
                                    DateTime FechaOferta, 
                                    Boolean Validado);
        decimal sumacantidades(int OfertaId, int Itemid);
        decimal MontoPresupuestoIngenieria(int OfertaId);
        decimal MontoPresupuestoConstruccion(int OfertaId);
        decimal MontoPresupuestoProcura(int OfertaId);

        
    }
}
