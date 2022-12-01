using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
  public interface IAvanceProcuraAsyncBaseCrudAppService : IAsyncBaseCrudAppService<AvanceProcura, AvanceProcuraDto,
        PagedAndFilteredResultRequestDto>
  {
      decimal GetMontoPresupuestado(int ofertaId);
      AvanceProcuraDto getdetalles(int AvanceProcuraId);

        List<AvanceProcuraDto> ListarPorOferta(int ofertaId);

        int Eliminar(int avanceIngenieriaId);

      bool comprobarfecha(DateTime fechadesde,DateTime fechahasta);

      List<OfertaDto> ListarOfertasDeProyecto(int ProyectoId);
      List<AvanceProcuraDto> ListarAvancesDeOfertaSinCertificar(int OfertaId);
      List<DetalleAvanceProcuraDto> ListarPorAvanceProcura(int avanceIngenieriaId);
      List<DetalleAvanceProcuraDto> ListarDetallesAvanceProcuraProyecto(int ProyectoId);

      ExcelPackage ObtenerCertificadoProcura(int Id);
    }


}
