using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class HistoricosOfertaAsyncBaseCrudAppService : AsyncBaseCrudAppService<HistoricosOferta,
        HistoricosOfertaDto, PagedAndFilteredResultRequestDto>, IHistoricosOfertaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Oferta> _ofertaRepository;
        private readonly IBaseRepository<Catalogo> _catalogo;

        public HistoricosOfertaAsyncBaseCrudAppService(
            IBaseRepository<HistoricosOferta> repository,
            IBaseRepository<Oferta> ofertaRepository,
            IBaseRepository<Catalogo> catalogo
            ) : base(repository)
        {
            _ofertaRepository = ofertaRepository;
            _catalogo = catalogo;
        }

        public void CreateHistoricoOferta(HistoricosOferta historicosOferta, int estado, int IdestadoTemp)
        {
            var catalogo = _catalogo.Get(IdestadoTemp);

            if (estado == 0)
            {
                Repository.Insert(historicosOferta);
            }
            else
            {
                Oferta oferta = _ofertaRepository.Get(historicosOferta.OfertaId);
                oferta.estado_oferta = estado;
                _ofertaRepository.Update(oferta);
                var catalogoTemp = _catalogo.Get(estado);
                historicosOferta.observaciones = historicosOferta.observaciones + 
                    " -- Estado Cambió de: " + catalogoTemp.nombre +" a el Estado: "
                    + catalogo.nombre;
                Repository.Insert(historicosOferta);
            }
        }

        public int EliminarVigencia(int historicoId)
        {
            var historico = Repository.Get(historicoId);
            historico.vigente = false;
            Repository.Update(historico);
            return historico.OfertaId;
        }

        public List<HistoricosOfertaDto> ListarHistoricosOferta(int idOferta) //ofertaID
        {
            var ho = Repository.GetAll();

            var items = (from o in ho
                         where o.vigente == true
                         where o.OfertaId == idOferta
                         select new HistoricosOfertaDto()
                         {
                             fecha = o.fecha,
                             observaciones = o.observaciones,
                             OfertaId = o.OfertaId,
                             vigente = o.vigente,
                             usuario = o.usuario,
                             Id = o.Id
                         }).ToList();
            return items;
        }

        public HistoricosOfertaDto GetHistoricosOferta(int id)
        {
            var ho = Repository.Get(id);

            HistoricosOfertaDto n = new HistoricosOfertaDto
            {
                fecha = ho.fecha,
                OfertaId = ho.OfertaId,
                Id = ho.Id,
                observaciones = ho.observaciones,
                usuario = ho.usuario,
                vigente = ho.vigente
            };
            return n;
        }

    }
}
