using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
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
    public interface IOfertaComercialPresupuestoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<OfertaComercialPresupuesto, OfertaComercialPresupuestoDto, PagedAndFilteredResultRequestDto>
    {
        List<OfertaComercialPresupuesto> Listar(int Id); // Oferta Comercial Id;

        string CrearOfertaComercialPresupuesto(OfertaComercialPresupuesto ofertapresupuesto); //Id Presupuesto;

        int ActualizarDatos(OfertaComercialPresupuesto ofertapresupuesto);
        // Oferta
        ExcelPackage GenerarExcelCabecera(int Id, int nivel_maximo);
        List<ComputoComercialDto> GetComputosPorOfertaComercial(int id);
        List<WbsComercialDto> ListaWbs(int Id); //OfertaCOmercialID

         string nombrecatalogo2(int tipocatagoid);


        decimal MontoPresupuestoIngenieria(int OfertaId);
        decimal MontoPresupuestoConstruccion(int OfertaId);
        decimal MontoPresupuestoProcura(int OfertaId);
        decimal sumacantidades(int OfertaId, int Itemid);


        ///


        //carga fechas y excel
        WbsComercialDto ObtenerPadre(string id_nivel_padre_codigo);
        List<WbsComercialDto> Jerarquiawbs(int id);
        WbsComercialDto DatosWbs(int id);

        ExcelPackage GenerarExcelCargaFechas(int id);

        List<WbsComercialDto> ArbolWbsExcel(int OfertaId);

        int Eliminar(int Id);


        //

        bool CambiarEmitidosRequerimientosOferta(int Id);

        int nivel_mas_alto(int Id,int presupuestoid);
        int contarnivel(int id,int presupuestoid);


    
    int mas_alto_multiple(int id);



        //Nueva Estructura Wbs
        List<WbsComercial> EstructuraWbs(int Id); //Oferta Comercial
        List<WbsComercial> ObtenerWbsHijos(int Id, string codigo_padre, List<WbsComercial> estructura); //Hijo


        ExcelPackage GenerarPropuestaEconomica(int Id, int nivel_maximo);

        ExcelPackage SecondFormatPropuestaEconomica(int Id, int nivel_maximo);

        //Nueva de Estructura 

        string nombreexcelofertaeconomica(int Id);

        List<Proyecto> ListadoProyectos(int OfertaComercialId);

        TotalesSegundoContrato TotalesSecondFormat(List<ComputoComercial> computos);
        decimal montoOfertado(int OfertaId);
    }
}
