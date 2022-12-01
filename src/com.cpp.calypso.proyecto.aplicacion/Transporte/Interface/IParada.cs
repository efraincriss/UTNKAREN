using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Interface
{
    public interface IParadaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Parada, ParadaDto, PagedAndFilteredResultRequestDto>
    {
        string canCreate(ParadaDto input);

        string canUpdate(ParadaDto input);

        Task<List<ParadaDto>> Listar();

        string canDelete(int paradaId);

        string nextcode();

        bool existe(string name);
        
    }
}
