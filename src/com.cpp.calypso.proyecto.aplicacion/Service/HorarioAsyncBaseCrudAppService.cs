using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class HorarioAsyncBaseCrudAppService : AsyncBaseCrudAppService<Horario, HorarioDto, PagedAndFilteredResultRequestDto>, IHorarioAsyncBaseCrudAppService
    {


	
		public HorarioAsyncBaseCrudAppService(
			
			IBaseRepository<Horario> repository
			
            ) : base(repository)
        {
			
		}

        public List<HorarioDto> GetList()
        {
            var i = 1;
            var query = Repository.GetAll();

            var horarios = (from d in query
                            where d.vigente == true
                            select new HorarioDto
                            {
                                Id = d.Id,
                                codigo = d.codigo,
                                nombre = d.nombre,
                                hora_inicio = d.hora_inicio,
                                hora_fin = d.hora_fin,
                                vigente = d.vigente
                            }).ToList();

            foreach (var e in horarios)
            {
                e.nro = i++;
                e.h_inicio = string.Format("{0:00}:{1:00}", e.hora_inicio.Hours, e.hora_inicio.Minutes);

                e.h_fin = string.Format("{0:00}:{1:00}", e.hora_fin.Hours, e.hora_fin.Minutes);

                if (e.vigente)
                {
                    e.nombre_estado = "Activo";
                }
                else
                {
                    e.nombre_estado = "Inactivo";
                }
            }

            return horarios;
        }


        public HorarioDto GetHorario(int Id) {
            var d = Repository.Get(Id);
            HorarioDto horario = new HorarioDto()
            {
                Id = d.Id,
                codigo = d.codigo,
                nombre = d.nombre,
                hora_inicio = d.hora_inicio,
                hora_fin = d.hora_fin,
                vigente = d.vigente
            };

            horario.h_inicio = string.Format("{0:00}:{1:00}", horario.hora_inicio.Hours, horario.hora_inicio.Minutes);

            horario.h_fin = string.Format("{0:00}:{1:00}", horario.hora_fin.Hours, horario.hora_fin.Minutes);

            if (horario.vigente)
            {
                horario.nombre_estado = "Activo";
            }
            else
            {
                horario.nombre_estado = "Inactivo";
            }


            return horario;
        }

		public int EliminarHorario(int id)
		{
			var horario = Repository.Get(id);
			horario.vigente = false;

			var resultado = Repository.Update(horario);

		
			return resultado.Id;
		}

		public string UniqueCodigo(string codigo)
		{
			var c = Repository.GetAll().Where(d => d.codigo == codigo && d.vigente == true).FirstOrDefault();
			if (c != null)
			{
				return "SI";
			}
			else
			{
				return "NO";
			}
		}
	}
}
