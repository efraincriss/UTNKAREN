﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface IDetalleGananciaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<DetalleGanancia,
        DetalleGananciaDto, PagedAndFilteredResultRequestDto>
    {
        List<DetalleGanancia> Listar();
        List<DetalleGananciaDto> ListarporGanancia(int Id);
        DetalleGananciaDto getdetalle(int Id);
        bool Eliminar(int Id);
    }
}