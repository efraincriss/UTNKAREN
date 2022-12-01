using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ZonaFrenteAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<ZonaFrente, ZonaFrenteDto, PagedAndFilteredResultRequestDto>,
        IZonaFrenteAsyncBaseCrudAppService
    {
        public ZonaFrenteAsyncBaseCrudAppService(
            IBaseRepository<ZonaFrente> repository
            ) : base(repository)
        {

        }

        public List<ZonaFrenteDto> GetFrentesPorZona(int ZonaId)
        {
            var frenteQuery = Repository.GetAll().Where(c => c.vigente == true);
            List<ZonaFrenteDto> frente =
                        (from c in frenteQuery
                         where c.ZonaId == ZonaId
                         select new ZonaFrenteDto
                         {
                             Id = c.Id,
                             codigo = c.codigo,
                             nombre = c.nombre,
                             descripcion = c.descripcion,
                             vigente = c.vigente,
                         }).ToList();
            return frente;
        }

        public List<ZonaFrenteDto> GetFrentes()
        {
            var frenteQuery = Repository.GetAll().Where(c => c.vigente == true);
            List<ZonaFrenteDto> frente =
                        (from c in frenteQuery
                         select new ZonaFrenteDto
                         {
                             Id = c.Id,
                             ZonaId = c.ZonaId,
                             codigo = c.codigo,
                             nombre = c.nombre,
                             descripcion = c.descripcion,
                             cordenada_x = c.cordenada_x,
                             cordenada_y = c.cordenada_y,
                             vigente = c.vigente,
                         }).ToList();
            return frente;
        }

        public void CrearFrente(ZonaFrenteDto zonaFrente)
        {
            Repository.Insert(new ZonaFrente()
            {
                codigo = zonaFrente.codigo,
                nombre = zonaFrente.nombre,
                descripcion = zonaFrente.descripcion,
                cordenada_x = zonaFrente.cordenada_x,
                cordenada_y = zonaFrente.cordenada_y,
                ZonaId = zonaFrente.ZonaId,
                vigente = true
            });
        }


        public void DeleteFrente(int Id)
        {
            var frente = Repository.Get(Id);
            frente.vigente = false;
            Repository.Update(frente);
        }

        public void UpdateFrente(int Id, string nombre, 
            string descripcion, string cordenadax, string cordenaday)
        {
            var frente = Repository.Get(Id);
            frente.nombre = nombre;
            frente.descripcion = descripcion;
            frente.cordenada_x = cordenadax;
            frente.cordenada_y = cordenaday;
            Repository.Update(frente);
        }
    }
}
