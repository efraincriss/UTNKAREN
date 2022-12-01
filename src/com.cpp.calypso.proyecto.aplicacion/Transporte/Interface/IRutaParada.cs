using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Interface
{
    public interface IRutaParadaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<RutaParada, RutaParadaDto, PagedAndFilteredResultRequestDto>
    {
        int Ingresar(RutaParada rutaparada);
        int Editar(RutaParada rutaparada);
        int Eliminar(int id);

        List<RutaParadaDto> ListaParadaporRuta(int id);
        List<Parada> ListaParadas();
        List<RutaHorario> ListaHorariosporRuta(int id);




    }
}