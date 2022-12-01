using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Interfaces
{
  public interface IWbsOfertaAsyncBaseCrudAppService : IAsyncBaseCrudAppService<WbsOferta, WbsOfertaDto, PagedAndFilteredResultRequestDto>
    {
     
         WbsOferta GetWbsOfertaporOferta(int OfertaId);

        List<WbsOfertaDto> GetWbsOfertas(int OfertaId);

        WbsOfertaDto GetDetalle(int WbsOfertaId);

        List<WbsOfertaDto> ListarPorOferta(int ofertaId);

        int EliminarVigencia(int WbsOfertaId);

        List<string[]> ObtenerWbsDistintos(int ofertaId);

        string nombrecatalogo2(int tipocatagoid);

        List<CatalogoDto> GetActtividades();

        List<CatalogoDto> GetAreas();

        List<Catalogo> GetAreasWbsRegistrado(int OfertaId);
        List<Catalogo> GetDisciplinasWbsRegistrado(int OfertaId, int AreaId);
        List<Catalogo> GetElementosWbsRegistrado(int OfertaId, int AreaId, int DisciplinaId);
        List<Catalogo> GetActividadesWbsRegistrado(int OfertaId, int AreaId, int DisciplinaId, int ElementoId);
        WbsOfertaDto GetWbsOfertaIdpor(int OfertaId, int Area, int Disc, int Elem, int Act);
        List<CatalogoDto> GetDisciplinas();

        List<CatalogoDto> GetElementos();

        OfertaDto GetClienteProyectoFecha(int ofertaId);

        List<TreeWbs> GenerarArbol(int ofertaId);

        List<JerarquiaWbs> GenerarJerarquia(int ofertaId);

        List<TreeWbs> GenerarArbolComputo(int ofertaId);

        WbsOferta Get(int wbsId);

        string ObtenerNombreDiciplina(int id);

        bool ComprobarExistenciaWbs(int Area, int Disciplina, int Elemento, int Actividad, int Oferta);

        WbsOfertaDto GetWbsOfertaIngenieria(int OfertaId);

    }
   
}
