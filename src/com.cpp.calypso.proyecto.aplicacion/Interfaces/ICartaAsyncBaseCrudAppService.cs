using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ICartaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Carta, CartaDto, PagedAndFilteredResultRequestDto>
    {

        List<CartaDto> ListaCartasEmTi(int EmpresaId, int tipo_destinatario, int tipo);
        List<CartaDto> GetCartaporTipo(int tipo);

        bool RegistrarCartasporDestinatario(CartaDto c, int[] destinoseleccionados);

        CartaDto getdetalle(int CartaId);
        bool EliminarVigencia(int CartaId);
        int InsertCarta(Carta c);
        int EditCarta(Carta c);
        bool EliminarCarta(int id);

        List<Cliente> ListClientes();
        List<Empresa> ListEmpresa();

        List<CartaDto> ListCarta(int TipoCartaId);

        List<String> ListCartasExistentes();
        List<ModelClassReactString> ListaDistribucionCartas();

        string UsuarioActual();




        //Arhivos Detalles.
        bool CrearDetalle(CartaArchivo d);
        string EditDetalle(CartaArchivo d);
        int CrearArchivo(HttpPostedFileBase e);
        bool DeleteDetalle(int Id);

        Task<string> Send_Files_Cartas(int Id, int[] ListIds,string body = "");

        string cuerpocarta(int Id);//CartaId;

        List<Carta> ListadoCartas();
        int secuencialCarta();

        string hrefoutlook(int id, List<int> ListIds);


        List<CartaDto> ListByTipo(int tipo);


        /*ES Cartas*/


        ExcelPackage Reporte();

    }
}
