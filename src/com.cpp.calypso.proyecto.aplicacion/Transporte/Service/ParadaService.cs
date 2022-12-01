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
    public class ParadaAsyncBaseCrudAppService : AsyncBaseCrudAppService<Parada, ParadaDto, PagedAndFilteredResultRequestDto>, IParadaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<RutaParada> _rutaParadaRepository;

        public ParadaAsyncBaseCrudAppService(
            IBaseRepository<Parada> repository,
            IBaseRepository<RutaParada> rutaParadaRepository
            ) : base(repository)
        {
            _rutaParadaRepository = rutaParadaRepository;
        }

        public async Task<List<ParadaDto>> Listar()
        {
            var entities = await Repository.GetAll()
                .OrderBy(o => o.Codigo)
                .ToListAsync();

            var dtos = Mapper.Map<List<Parada>, List<ParadaDto>>(entities);
            var count = 1;
            foreach (var paradaDto in dtos)
            {
                paradaDto.Secuencial = count;
                count++;
            }

            return dtos;
        }


        public string canCreate(ParadaDto input)
        {
            var e = this.existe(input.Nombre);
            if (e) {
                return "El nombre de la Parada ya se encuentra registrado";
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

        public string canUpdate(ParadaDto input)
        {
            var countNombre = Repository.GetAll()
                .Where(o => o.Nombre == input.Nombre)
                .Count(o => o.Id != input.Id)
                ;

            if (countNombre > 0)
                return "El nombre ya se encuentra registrado";

            return "OK";
        }


        public string canDelete(int paradaId)
        {
            var count = _rutaParadaRepository.GetAll()
                .Count(o => o.ParadaId == paradaId);

            if (count > 0)
                return "No se puede eliminar la parada debido a que está siendo usado en ruta(s)";

            return "OK";
        }

        public string nextcode()
        {
            int sec_number = 1;
            var list_code = Repository.GetAll().Where(c => !c.IsDeleted).Select(c => c.Codigo).ToList();
            if (list_code.Count > 0)
            {
                List<int> numeracion = (from l in list_code
                                        where l.Length == 8
                                        select Convert.ToInt32(l.Substring(3, 5))).ToList();

                if (numeracion.Count > 0)
                {
                    sec_number = numeracion.Max() + 1;
                }


            }
            return "PAR" + String.Format("{0:00000}", sec_number);
        }

        public bool existe(string name)
        {
            var e = Repository.GetAll().Where(c => c.Nombre == name).FirstOrDefault();
            return e != null && e.Id > 0 ? true : false;
        }
    }
}
