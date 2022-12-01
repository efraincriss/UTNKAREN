using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ContratoProveedorAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<ContratoProveedor, ContratoProveedorDto,
            PagedAndFilteredResultRequestDto>,
        IContratoProveedorAsyncBaseCrudAppService
    {
        public IBaseRepository<TipoOpcionComida> RepositoryDetails { get; }
        public IArchivoAsyncBaseCrudAppService ArchivoService { get; }
        public IBaseRepository<ConsumoVianda> RepositoryConsumoVianda { get; }

        public ContratoProveedorAsyncBaseCrudAppService(
            IBaseRepository<ContratoProveedor> repository,
            IBaseRepository<TipoOpcionComida> repositoryDetails,
             IArchivoAsyncBaseCrudAppService archivoService,
             IBaseRepository<ConsumoVianda> repositoryConsumoVianda
            ) : base(repository)
        {
            RepositoryDetails = repositoryDetails;
            RepositoryConsumoVianda = repositoryConsumoVianda;
            ArchivoService = archivoService;
        }



        public override async Task<ContratoProveedorDto> Get(EntityDto<int> input)
        {
            CheckGetPermission();
            IBaseRepository<ContratoProveedor> repository = Repository as IBaseRepository<ContratoProveedor>;


            return await Task.Run(() =>
            {

                var entity = repository.Get(input.Id, c => c.tipo_opciones_comida);
                return MapToEntityDto(entity);
            });
        }

        public async Task<ContratoProveedorTipoOpcionesDto> GetInfo(EntityDto<int> input)
        {
            CheckGetPermission();
            IBaseRepository<ContratoProveedor> repository = Repository as IBaseRepository<ContratoProveedor>;


            return await Task.Run(() =>
            {

                var entity = repository.Get(input.Id, c => c.tipo_opciones_comida);
                return ObjectMapper.Map<ContratoProveedorTipoOpcionesDto>(entity);

            });
        }


        public async Task<ContratoProveedorTipoOpcionesDto> Create(ContratoProveedorTipoOpcionesDto input)
        {
            //Rules
            //bool overlap = tStartA < tEndB && tStartB < tEndA;
            var CheckDuplicate = await Repository.GetAll().Where(item => item.ProveedorId == input.ProveedorId &&
                                       input.fecha_inicio < item.fecha_inicio && item.fecha_inicio < item.fecha_fin
                                        ).LongCountAsync();

            if (CheckDuplicate > 0)
            {
                var msg = string.Format("Ya existe contratos con las fechas ingresadas. Sobreposición de Fechas de Inicio y Fin");
                throw new GenericException(msg, msg);
            }

            //Crear Archivo si existe
            if (input.documentacion_subida != null)
            {

                var archivoResult = await ArchivoService.Create(input.documentacion_subida);
                input.documentacion_id = archivoResult.Id;
            }
            List<TipoOpcionComidaDto> TipoOpcionComidaHorariosActualizados = new List<TipoOpcionComidaDto>();
            List<TipoOpcionComida> lista = ObjectMapper.Map<List<TipoOpcionComida>>(input.tipo_opciones_comida.ToList());
            foreach (var t in lista)
            {
                var tipoHorario = RepositoryDetails.GetAll().Where(c => c.tipo_comida_id == t.tipo_comida_id).FirstOrDefault();

                if (tipoHorario != null)
                {
                    t.hora_inicio = tipoHorario.hora_inicio;
                    t.hora_fin = tipoHorario.hora_fin;
                }
                TipoOpcionComidaHorariosActualizados.Add(AutoMapper.Mapper.Map<TipoOpcionComidaDto>(t));
            }

            input.tipo_opciones_comida = TipoOpcionComidaHorariosActualizados;


            var entity = ObjectMapper.Map<ContratoProveedor>(input);


           

            var result = await Repository.InsertAsync(entity);

            return ObjectMapper.Map<ContratoProveedorTipoOpcionesDto>(result);

        }

        public async Task<ContratoProveedorTipoOpcionesDto> Update(ContratoProveedorTipoOpcionesDto input)
        {
            var entity = ObjectMapper.Map<ContratoProveedor>(input);
            var result = await Repository.UpdateAsync(entity);
            return ObjectMapper.Map<ContratoProveedorTipoOpcionesDto>(result);
        }

        public async Task<ContratoProveedorTipoOpcionesDto> UpdateAndDelete(ContratoProveedorTipoOpcionesDto input)
        {


            //Rules
            //bool overlap = tStartA < tEndB && tStartB < tEndA;
            var CheckDuplicate = await Repository.GetAll().Where(item => item.ProveedorId == input.ProveedorId &&
                                       item.Id != input.Id &&
                                       input.fecha_inicio < item.fecha_inicio && item.fecha_inicio < item.fecha_fin
                                        ).LongCountAsync();

            if (CheckDuplicate > 0)
            {
                var msg = string.Format("Ya existe contratos con las fechas ingresadas. Sobreposición de Fechas de Inicio y Fin");
                throw new GenericException(msg, msg);
            }

            //1. Update
            //Crear Archivo si existe
            if (input.documentacion_subida != null)
            {
                var archivoResult = await ArchivoService.Create(input.documentacion_subida);
                input.documentacion_id = archivoResult.Id;
            }
            //Reviw


            List<TipoOpcionComida> lista = ObjectMapper.Map<List<TipoOpcionComida>>(input.tipo_opciones_comida.ToList());
            foreach (var t in lista)
            {

                var tipoHorario = RepositoryDetails.GetAll().Where(c => c.tipo_comida_id == t.tipo_comida_id).FirstOrDefault();

                if (tipoHorario != null)
                {
                    t.hora_inicio = tipoHorario.hora_inicio;
                    t.hora_fin = tipoHorario.hora_fin;
                }
                else
                {
                t.hora_inicio = DateTime.Now.Date.AddHours(1);
                t.hora_fin = DateTime.Now.Date.AddHours(2);
                }

                if (t.Id == 0)
                {
                   
                    var item = RepositoryDetails.Insert(t);
                }
                else
                {
                    var update = RepositoryDetails.Get(t.Id);
                    update.tipo_comida_id = t.tipo_comida_id;
                    update.opcion_comida_id = t.opcion_comida_id;
                    update.costo = t.costo;
                    update.hora_inicio = t.hora_inicio;
                    update.hora_fin = t.hora_fin;
                    var item = RepositoryDetails.Update(update);
                }
            }

            input.tipo_opciones_comida = null;
            var entity = ObjectMapper.Map<ContratoProveedor>(input);

            var result = await Repository.UpdateAsync(entity);


            //2. Delete 
            List<int> deleteIdsNotNull = input.deleteIds;
            var query = RepositoryDetails.GetAll();
            var itemsDelete = await (from item in query
                                     where
                                        item.ContratoId == input.Id
                                        && deleteIdsNotNull.Contains(item.Id)
                                     select item).ToListAsync();
            RepositoryDetails.Delete(itemsDelete);

            //3. Get Entiy Update..
            return await GetInfo(new EntityDto<int>(input.Id));
        }

        public bool CanDeleteTipoOpcionComida(int TipoOpcionComidaId)
        {

            var consumo_vianda = RepositoryConsumoVianda.GetAll()
                                                       .Where(c => c.TipoOpcionComidaId == TipoOpcionComidaId)
                                                       .ToList();
            return consumo_vianda.Count==0?true:false;

        }

        public string ActualizarHorarios(int TipoComidaId, TimeSpan HoraInicio, TimeSpan HoraFin)
        {
            var tipoOpcionComidas = RepositoryDetails.GetAll().Where(c => c.tipo_comida_id == TipoComidaId).ToList();

            if (HoraFin < HoraInicio)
            {
                return "La Hora de Fin no puede ser menor a la Hora de Inicio";
            }

            foreach (var tipo in tipoOpcionComidas)
            {
                var e = RepositoryDetails.Get(tipo.Id);
                e.hora_inicio = DateTime.Now.Date.Add(HoraInicio);
                e.hora_fin = DateTime.Now.Date.Add(HoraFin);

                RepositoryDetails.Update(e);

            }
            return "OK";
        }


    }


}

