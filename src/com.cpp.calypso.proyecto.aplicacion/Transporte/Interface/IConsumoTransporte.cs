using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Models;
using com.cpp.calypso.proyecto.dominio.Transporte;
using Newtonsoft.Json.Linq;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Interface
{
    public interface IConsumoTransporteAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ConsumoTransporte, ConsumoTransporteDto, PagedAndFilteredResultRequestDto>
    {
        JArray Sync(int version, JArray registrosJson, List<int> usuariosId);
    }
}