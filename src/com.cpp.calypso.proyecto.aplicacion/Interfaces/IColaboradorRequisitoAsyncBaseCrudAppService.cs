using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
	public interface IColaboradorRequisitoAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradorRequisito, ColaboradorRequisitoDto, PagedAndFilteredResultRequestDto>
	{
		List<ColaboradorRequisitoDto> GetList(int Id);
		ColaboradorRequisitoDto GetRequisito(int Id);
        Task<int> CargarArchivoRequisito(ColaboradorRequisitoDto requisito, HttpPostedFileBase[] UploadedFile);
        Task<Archivo> ActualizaArchivoRequisito(ColaboradorRequisitoDto requisito, HttpPostedFileBase[] UploadedFile);

    }
}
