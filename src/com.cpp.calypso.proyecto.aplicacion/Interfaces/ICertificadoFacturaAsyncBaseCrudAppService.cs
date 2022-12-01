﻿using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
    public interface ICertificadoFacturaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<CertificadoFactura, CertificadoFacturaDto, PagedAndFilteredResultRequestDto>
    {
        List<CertificadoFacturaDto> certificadosporfactura(int FacturaId);
    }
}
