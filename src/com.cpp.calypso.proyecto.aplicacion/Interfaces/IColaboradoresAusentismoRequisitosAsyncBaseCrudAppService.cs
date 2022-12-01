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
    public interface IColaboradoresAusentismoRequisitosAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradoresAusentismoRequisitos, ColaboradoresAusentismoRequisitosDto, PagedAndFilteredResultRequestDto>
    {
        Task<string> CrearAusentismoRequisitoAsync(ColaboradoresAusentismoRequisitosDto colaboradoresAusentismoRequisito, HttpPostedFileBase[] UploadedFile);
        List<ColaboradoresAusentismoRequisitosDto> GetAusentismoRequisito(int id);

        Task<string> EditarAusentismoRequisitoAsync(ColaboradoresAusentismoRequisitosDto colaboradoresAusentismoRequisito, HttpPostedFileBase[] UploadedFile);

        ColaboradoresAusentismoRequisitosDto ObtenerArchivos(int Id); //Ausentismo Id
    }
}
