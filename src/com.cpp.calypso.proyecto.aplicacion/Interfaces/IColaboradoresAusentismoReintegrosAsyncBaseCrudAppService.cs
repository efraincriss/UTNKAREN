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
    public interface IColaboradoresAusentismoReintegrosAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ColaboradoresAusentismoReintegros, ColaboradoresAusentismoReintegrosDto, PagedAndFilteredResultRequestDto>
    {
        Task<string> CrearReintegrosAsync(ColaboradoresAusentismoReintegrosDto colaboradoresAusentismoReintegrosDto, HttpPostedFileBase[] UploadedFile);
    }
}
