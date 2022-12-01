﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IContactoEmergenciaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ContactoEmergencia, ContactoEmergenciaDto, PagedAndFilteredResultRequestDto>
    {
        List<ContactoEmergenciaDto> GetByColaboradorId(int colaboradorId);

        int CreateContacto(ContactoEmergenciaDto contacto);
        string EliminarContacto(int id);
    }
}
