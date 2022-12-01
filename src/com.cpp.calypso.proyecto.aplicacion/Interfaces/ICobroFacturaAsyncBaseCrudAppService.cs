using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ICobroFacturaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<CobroFactura, CobroFacturaDto, PagedAndFilteredResultRequestDto>
    {
        List<CobroFactura> ListadeCobros();
        List<CobroFactura> ListadeCobrosFactura(int Id); // ID=>FacturaId
        List<CobroFactura> ListaFacturaCobros(int Id); // ID=>CobroId
        CobroFacturaDto getdetalle(int Id);
        bool Eliminar(int Id);

        List<Cobro> ListaCobrosUnicos();
    }
}