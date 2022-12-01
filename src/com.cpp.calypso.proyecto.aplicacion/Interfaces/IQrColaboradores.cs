using com.cpp.calypso.comun.aplicacion;
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
    public interface IQrColaboradoresAsyncBaseCrudAppService : IAsyncBaseCrudAppService<QrColaboradores, QrColaboradoresDto, PagedAndFilteredResultRequestDto>
    {
        List<QrColaboradorModel> ListColaboradores(string search);
  
        string EntregarQr(QrColaboradorModel row);
        bool ReemprimirQr(QrColaboradorModel row);
        bool DarDeBajaQr(QrColaboradorModel row);
        bool RegitrarPerdida(List<QrColaboradorModel> list);
        bool GenerarQrMasiva(List<QrColaboradorModel> list);
        bool GenerarQrIndividual(QrColaboradorModel row);

        string GenerarTarjeta(List<QrColaboradorModel> rows);




        List<ReingresoModel> ListColaboradoresReingreso(string search);


        bool ActualizarIdSapLocal(int ColaboradorId, int empleado_id_sap_local);
    }
}
