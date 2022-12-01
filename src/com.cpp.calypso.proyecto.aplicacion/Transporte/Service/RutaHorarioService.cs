using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Service
{
 

    public class RutaHorarioAsyncBaseCrudAppService : AsyncBaseCrudAppService<RutaHorario, RutaHorarioDto, PagedAndFilteredResultRequestDto>, IRutaHorarioAsyncBaseCrudAppService
    {
       
        public RutaHorarioAsyncBaseCrudAppService(
            IBaseRepository<RutaHorario> repository
            ) : base(repository)
        {
   

        }

        public int Editar(RutaHorario rutaHorrario)
        {
            var ruta = Repository.Get(rutaHorrario.Id);
            ruta.Horario = rutaHorrario.Horario;
            var id = Repository.Update(ruta);
            return id.Id;
        }

        public int Eliminar(int id)
        {
          
                var r = Repository.Get(id);
                Repository.Delete(r);
                return r.Id;
            
        

        }

        public int Ingresar(RutaHorario rutaHorrario)
        {
            var nuevoid = Repository.InsertAndGetId(rutaHorrario);
            return nuevoid;
        }

        public bool mismohorario(int rutaid, TimeSpan hora)
        {
            var mismo = Repository.GetAll().Where(c => c.RutaId == rutaid)
                 .Where(c => c.Horario == hora).ToList();
            return mismo.Count > 0 ? true : false;
        }
    }
}
