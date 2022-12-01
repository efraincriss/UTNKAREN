using Abp.Domain.Entities;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IItemAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Item, ItemDto, PagedAndFilteredResultRequestDto>
    {
        Task<ItemDto> GetDetalle(int ItemId);
        List<Item> GetItems();
        List<Item> GetItemsporContratoActivo(int ContratoId,DateTime FechaOferta);
        List<Item> GetItemsparaOferta();

        List<Item> GetItemsHijos(String item_padre);
        List<Item> GetItemsPor(String item_padre);
        List<Item> GetItemsHijosContenido(String item_padre);
        int buscaridentificadorpadre(string item_padre);
        bool siexisteid(String codigo);
        bool siexisteidEdit(String codigo, int id);
        List<TreeItem> GenerarArbol();

        List<ItemDto> GetItemsParaOferta();

        bool comprobaritemmovimiento(string padre);


        List<Item> JerarquiaItem(int id);


        Item ObtenerPadre(string codigopadre);

        ItemDto DatosItem(int id);

        Item FastDetalle(int id);
        List<Item> ArbolWbsExcel(int ContratoId, DateTime fechaoferta);
        List<Item> GetItemsporContratoActivo2(int ContratoId, DateTime FechaOferta);

        List<Item> ItemsReporteExcel();


        // Arbol Wbs Presupuesto

        List<Item> ArbolWbsExcelPresupuesto(int ContratoId, DateTime fechaoferta);

        List<Item> ObtenerItemsProcura();

        List<Item> ArbolItemsCertificadoSinPendientes(int ProyectoId,DateTime fechaCorte);

        List<Item> ArbolItemsCertificadoSinPendientesUltimoRdo(int ProyectoId, DateTime fechaCorte,List<RdoDetalleEac>detallesUltimoRdo);
        List<Item> ArbolItemsCertificadoPendientesAprobacionUltimoRdo(int ProyectoId, DateTime fechaCorte, List<RdoDetalleEac> detallesUltimoRdo);
        List<Item> ArbolItemsCertificadoPendientesAprobacion(int ProyectoId,DateTime fechaCorte);
        List<Item> ArbolItemsComputoComercial(int OfertaComercialId);

        List<Item> EstructuraItems(int ContratoId, DateTime Fecha);
        List<Item> EstructuraItemsHijos(string codigo, List<Item> detalleitems);


        List<Item> ObtenerItemsProcuraporPresupuesto(int PresupuestoId);


        //ARBOL ITEMS TODOS 

        List<Item> EstructuraArbolItems(int ContratoId, DateTime Fecha);
        List<Item> EstructuraArbolItemsHijos(string codigo, List<Item> detalleitems);





        //ES: CONTRATO 2 ITEMS ESPECIALIDAD

        List<NodeItem> TreeDataArbol();
        InfoItem DetailsAPIItem(int Id);

        bool Eliminar(int Id);


        List<Item> EstructuraItemMatrizPresupuesto(int ContratoId, DateTime Fecha);
        List<Item> ItemsHijos(string codigo, List<Item> detalleitems);

        //ARBOL ITEMS TODOS 
        List<Item> ItemsMatrizPresupuesto(int ContratoId, DateTime Fecha, List<ComputoPresupuestoDto> computos);
        List<Item> ItemsMatrizPresupuestoHijos(string codigo, List<Item> detalleitems);



        //ARBOL ITEMS TODOS 
        List<Item> ItemsMatrizComercial(int ContratoId, DateTime Fecha, List<ComputoComercialDto> computos);

    }
    
 
}
