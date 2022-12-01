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
    public interface ILugarAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Lugar, LugarDto, PagedAndFilteredResultRequestDto>
    {
        Task<List<LugarDto>> Listar();

        string canCreate(LugarDto input);

        string canDelete(int lugarId);

        string canUpdate(LugarDto input);

        string nextcode();

        bool existe(string name);
    }
}
