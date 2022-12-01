using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Service
{
    public class LugarAsyncBaseCrudAppService : AsyncBaseCrudAppService<Lugar, LugarDto, PagedAndFilteredResultRequestDto>, ILugarAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Ruta> _rutaRepository;

        public LugarAsyncBaseCrudAppService(
            IBaseRepository<Lugar> repository,
            IBaseRepository<Ruta> rutaRepository
            ) : base(repository)
        {
            _rutaRepository = rutaRepository;
        }

        public async Task<List<LugarDto>> Listar()
        {
            var entities = await Repository.GetAll()
                .OrderBy(o => o.Nombre)
                .ToListAsync();

            var dtos = Mapper.Map<List<Lugar>, List<LugarDto>>(entities);
            var count = 1;
            foreach (var paradaDto in dtos)
            {
                paradaDto.Secuencial = count;
                count++;
            }

            return dtos;
        }


        public string canCreate(LugarDto input)
        {
            var e = this.existe(input.Nombre);
            if (e)
            {
                return "El nombre de la Lugar ya se encuentra registrado";
            }

            var count = Repository.GetAll()
                .Count(o => o.Codigo == input.Codigo);

            if (count > 0)
            {
                return "El código ingresado ya se encuentra registrado";
            }

            var countNombre = Repository.GetAll()
                .Count(o => o.Nombre == input.Nombre);

            if (countNombre > 0)
                return "El nombre ingresado ya se encuentra registrado";

            return "OK";
        }

        public string canUpdate(LugarDto input)
        {
            var countNombre = Repository.GetAll()
                    .Where(o => o.Nombre == input.Nombre)
                    .Count(o => o.Id != input.Id)
                ;

            if (countNombre > 0)
                return "El nombre ya se encuentra registrado";

            return "OK";
        }


        public string canDelete(int lugarId)
        {
            var count = _rutaRepository.GetAll()
                .Count(o => o.OrigenId == lugarId || o.DestinoId == lugarId);

            if (count > 0)
                return "No se puede eliminar el lugar debido a que está siendo usado en ruta(s)";

            return "OK";
        }

        public string nextcode()
        {
            int sec_number = 1;
            var list_code = Repository.GetAll().Where(c => !c.IsDeleted).Select(c => c.Codigo).ToList();
            if (list_code.Count > 0)
            {
                List<int> numeracion = (from l in list_code
                                        where l.Length==8    
                                        select Convert.ToInt32(l.Substring(3, 5))).ToList();

                if (numeracion.Count > 0){sec_number = numeracion.Max() + 1;}
            }
            return "LUG" + String.Format("{0:00000}", sec_number); 
        }

        public bool existe(string name)
        {
            var e = Repository.GetAll().Where(c => c.Nombre == name).FirstOrDefault();
            return e != null && e.Id > 0 ? true : false;
        }
    }

    
}
