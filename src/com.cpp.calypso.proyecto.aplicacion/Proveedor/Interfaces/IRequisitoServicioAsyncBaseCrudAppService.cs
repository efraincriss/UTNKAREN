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
    public interface IRequisitoServicioAsyncBaseCrudAppService : IAsyncBaseCrudAppService<RequisitoServicio, RequisitoServicioDto, PagedAndFilteredResultRequestDto>
    {

        List<RequisitoServicioDto> GetList();
        RequisitoServicioDto GetRequisitoServicio(int Id);

        string UniqueServicio(int servicio, int requisito, int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="servicioId"></param>
        /// <returns></returns>
        Task<List<RequisitoServicioDto>> GetRequisitos(int servicioId);
    }
}
