using Abp.Application.Services.Dto;
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
    public interface IHorarioAsyncBaseCrudAppService : IAsyncBaseCrudAppService<Horario, HorarioDto, PagedAndFilteredResultRequestDto>
    {
        List<HorarioDto> GetList();
        HorarioDto GetHorario(int Id);

		int EliminarHorario(int Id);
		string UniqueCodigo(string codigo);


	}

}
