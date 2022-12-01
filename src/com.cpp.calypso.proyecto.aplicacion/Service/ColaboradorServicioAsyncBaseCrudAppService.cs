using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
	public class ColaboradorServicioAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorServicio, ColaboradorServicioDto, PagedAndFilteredResultRequestDto>, IColaboradorServicioAsyncBaseCrudAppService
	{
        private readonly IBaseRepository<ColaboradoresComida> _colcomidarepository;
        private readonly IBaseRepository<ColaboradorMovilizacion> _colmovilizacionrepository;
        public ColaboradorServicioAsyncBaseCrudAppService(
			IBaseRepository<ColaboradorServicio> repository,
                        IBaseRepository<ColaboradoresComida> colcomidarepository,
            IBaseRepository<ColaboradorMovilizacion> colmovilizacionrepository

            ) : base(repository)
		{
            _colcomidarepository = colcomidarepository;
            _colmovilizacionrepository = colmovilizacionrepository;
        }

		public List<ColaboradorServicioDto> GetServicios(int Id)
		{
			var query = Repository.GetAll().Where(c => c.ColaboradoresId == Id);

			List<ColaboradorServicioDto> servicios =
			(from c in query
			 select new ColaboradorServicioDto
			 {
				 Id = c.Id,
				 ColaboradoresId = c.ColaboradoresId,
				 ServicioId = c.ServicioId,
                 Catalogo = c.Catalogo,
				 vigente = c.vigente,
                 Colaboradores = c.Colaboradores
			 }).ToList(); ;
			
			return servicios;
		}

		public string CreateServicios(ColaboradorServicio[] servicios) {

			foreach (var s in servicios) {
				Repository.Insert(s);
			}

			return "OK";
		}

        public bool ValidarEliminacionEInsercionServicio(int colaboradorId)
        {


            var servicios = Repository.GetAllIncluding(x => x.Catalogo)
                                                              .Where(x => x.ColaboradoresId == colaboradorId).ToList();
            /*Eliminamos*/
            if (servicios.Count > 0)
            {
                foreach (var s in servicios)
                {
                    var tiposcomida = _colcomidarepository.GetAllIncluding(l => l.ColaboradorServicio).Where(l => l.ColaboradorServicioId == s.Id).ToList();
                    if (tiposcomida.Count > 0)
                    {
                        _colcomidarepository.Delete(tiposcomida);
                    }
                    var transportes = _colmovilizacionrepository.GetAllIncluding(l => l.ColaboradorServicio).Where(l => l.ColaboradorServicioId == s.Id).ToList();
                    if (transportes.Count > 0)
                    {
                        _colmovilizacionrepository.Delete(transportes);
                    }
                }
                foreach (var s in servicios)
                {
                    Repository.Delete(s);
                }
                  
            }

            return true;
        }
    }
}
