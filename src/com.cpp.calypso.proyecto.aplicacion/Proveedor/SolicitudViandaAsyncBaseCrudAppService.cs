using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using com.cpp.calypso.framework;
using Abp.Application.Services.Dto;
using com.cpp.calypso.framework.Extensions;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class SolicitudViandaAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<SolicitudVianda, SolicitudViandaDto, PagedAndFilteredResultRequestDto>,
        ISolicitudViandaAsyncBaseCrudAppService
    {
        public IApplication Application { get; }

        public SolicitudViandaAsyncBaseCrudAppService(
            IBaseRepository<SolicitudVianda> repository,
            IApplication application
            ) : base(repository)
        {
            Application = application;
        }

        public async override Task<SolicitudViandaDto> Get(EntityDto<int> input)
        {
            var query = Repository.GetAllIncluding(s => s.solicitante);
            var entity = await (from item in query
                                where
                                    item.Id == input.Id
                                select item).SingleOrDefaultAsync();

            return MapToEntityDto(entity);

            //var entity = await (from item in query
            //                    where
            //                    item.Id == input.Id
            //                    select new SolicitudViandaDto
            //                    {
            //                        Id = item.Id,
            //                        alcance_viandas = item.alcance_viandas,
            //                        consumido = item.consumido,
            //                        consumo_justificado = item.consumo_justificado,
            //                        disciplina_id = item.disciplina_id,
            //                        disciplina_nombre = item.disciplina.nombre,
            //                        estado = item.estado,
            //                        estado_nombre = item.estado.ToString(),
            //                        fecha_alcancce = item.fecha_alcancce,
            //                        fecha_solicitud = item.fecha_solicitud,
            //                        LocacionId = item.LocacionId,
            //                        locacion_nombre = item.locacion.nombre,

                                //                        area_id = item.area_id,
                                //                        area_nombre = item.area.nombre,

                                //                        pedido_viandas = item.pedido_viandas,
                                //                        por_justificar = item.por_justificar,
                                //                        referencia_ubicacion = item.referencia_ubicacion,
                                //                        solicitante_id = item.solicitante_id,
                                //                        solicitante_nombre = item.solicitante.nombres,
                                //                        solicitud_original_id = item.solicitud_original_id,
                                //                        tipo_comida_id = item.tipo_comida_id,
                                //                        tipo_comida_nombre = item.tipo_comida.nombre,
                                //                        total_consumido = item.total_consumido,
                                //                        total_pedido = item.total_pedido
                                //                    }).FirstOrDefaultAsync();

           // return entity; 
        }

        public async Task<IList<SolicitudViandaDto>> GetSolicitudDiaria(DateTime? fecha)
        {
            //Si fecha es nula, obtener solicitudes de viandas de hoy
            var fechaFiltro = DateTime.Now;
            if (fecha.HasValue)
            {
                fechaFiltro = fecha.Value;
            }

            var query = Repository.GetAll();
            var items = await (from item in query
                               where
                                 DbFunctions.TruncateTime(item.fecha_solicitud) == DbFunctions.TruncateTime(fechaFiltro)
                               //&& item.fecha_solicitud <= DbFunctions.AddDays(fechaFiltro, 1)
                               select new SolicitudViandaDto
                               {
                                   Id = item.Id,
                                   alcance_viandas = item.alcance_viandas,
                                   consumido = item.consumido,
                                   consumo_justificado = item.consumo_justificado,
                                   disciplina_id = item.disciplina_id,
                                   disciplina_nombre = item.disciplina.nombre,
                                   estado = item.estado,
                                   fecha_alcancce = item.fecha_alcancce,
                                   fecha_solicitud = item.fecha_solicitud,
                                   LocacionId = item.LocacionId,
                                   locacion_nombre = item.locacion.nombre,

                                   area_id = item.area_id,
                                   area_nombre = item.area.nombre,

                                   pedido_viandas = item.pedido_viandas,
                                   referencia_ubicacion = item.referencia_ubicacion,
                                   solicitante_id = item.solicitante_id,
                                   solicitante_nombre = item.solicitante.nombres_apellidos,
                                   solicitud_original_id = item.solicitud_original_id,
                                   tipo_comida_id = item.tipo_comida_id,
                                   tipo_comida_nombre = item.tipo_comida.nombre,
                                   total_consumido = item.total_consumido,
                                   total_pedido = item.total_pedido
                               }).ToListAsync();
            return items;
        }

        public async Task<IList<SolicitudViandaDto>> GetSolicitudesPendientes(DateTime fecha, int tipoComidaId)
        {
             
            var query = Repository.GetAll();
            var items = await (from item in query
                               where
                                 DbFunctions.TruncateTime(item.fecha_solicitud) == DbFunctions.TruncateTime(fecha)
                                && item.tipo_comida_id == tipoComidaId
                                && item.estado == SolicitudViandaEstado.Registrado
                               select new SolicitudViandaDto
                               {
                                   Id = item.Id,
                                   alcance_viandas = item.alcance_viandas,
                                   consumido = item.consumido,
                                   consumo_justificado = item.consumo_justificado,
                                   disciplina_id = item.disciplina_id,
                                   disciplina_nombre = item.disciplina.nombre,
                                   estado = item.estado,
                                   //estado_nombre = item.estado.GetDescription(),
                                   fecha_alcancce = item.fecha_alcancce,
                                   fecha_solicitud = item.fecha_solicitud,
                                   LocacionId = item.LocacionId,
                                   locacion_nombre = item.locacion.nombre,

                                   area_id = item.area_id,
                                   area_nombre = item.area.nombre,

                                   pedido_viandas = item.pedido_viandas,
                                  
                                   referencia_ubicacion = item.referencia_ubicacion,
                                   solicitante_id = item.solicitante_id,
                                   solicitante_nombre = item.solicitante.nombres_apellidos,
                                   solicitud_original_id = item.solicitud_original_id,
                                   tipo_comida_id = item.tipo_comida_id,
                                   tipo_comida_nombre = item.tipo_comida.nombre,
                                   total_consumido = item.total_consumido,
                                   total_pedido = item.total_pedido
                               }).ToListAsync();
            return items;

        }

        public async Task<int> UpdateState(IEnumerable<int> solicitudesId, SolicitudViandaEstado estado)
        {
            IBaseRepository<SolicitudVianda> baseRespository = Repository as IBaseRepository<SolicitudVianda>;

            var solicitudes = await baseRespository.GetAllListAsync(c => solicitudesId.Contains(c.Id));
            foreach (var item in solicitudes)
            {
                item.estado = estado;
            }
            baseRespository.SaveOrUpdate(solicitudes);

            return solicitudes.Count;
        }


        public override async Task<SolicitudViandaDto> Create(SolicitudViandaDto input)
        {
            input.referencia_ubicacion = ".";
            //Compute
            //Total de pedido
            input.total_pedido = input.pedido_viandas + input.alcance_viandas;
            input.estado = SolicitudViandaEstado.Registrado;

            //Rules
            //CheckDuplicate
            var CheckDuplicate = await Repository.LongCountAsync(item =>
                                DbFunctions.TruncateTime(item.fecha_solicitud) == DbFunctions.TruncateTime(input.fecha_solicitud)
                                && item.tipo_comida_id == input.tipo_comida_id
                                && item.LocacionId == input.LocacionId
                                && item.area_id  == input.area_id
                                && item.disciplina_id==input.disciplina_id
                                && item.estado != SolicitudViandaEstado.Cancelado
                                );

            if (CheckDuplicate>0) {
                throw new GenericException(string.Format("Ya existe una solicitud para la Fecha, Tipo Comida, Localización , Área y Disciplina"),
                   "Ya existe una solicitud para la Fecha, Tipo Comida, Localización , Área y Disciplina");
            }

            return await base.Create(input);
        }

        public override async Task<SolicitudViandaDto> Update(SolicitudViandaDto input)
        {
            //Total de pedido (Rules)
            input.total_pedido = input.pedido_viandas + input.alcance_viandas;
            input.fecha_alcancce = DateTime.Now;

            //Rules
            //CheckDuplicate
            var CheckDuplicate = await Repository.LongCountAsync(item =>
                                DbFunctions.TruncateTime(item.fecha_solicitud) == DbFunctions.TruncateTime(input.fecha_solicitud)
                                && item.tipo_comida_id == input.tipo_comida_id
                                && item.LocacionId == input.LocacionId
                                && item.area_id == input.area_id
                                && item.estado != SolicitudViandaEstado.Cancelado
                                && item.Id != input.Id);

            if (CheckDuplicate > 0)
            {
                throw new GenericException(string.Format("Ya existe una solicitud para la Fecha, Tipo Comida, Localización y Área"),
                    "Ya existe una solicitud para la Fecha, Tipo Comida, Localización y Área");
            }

            return await base.Update(input);
        }

        
        public override async Task Delete(EntityDto<int> input)
        {
            //Rules
            var entity = await Get(new EntityDto<int>(input.Id));
            if (entity == null)
            {
                var msg = string.Format("El Registro con identificacion {0} no existe, o sus datos asociados no existen",
                   input.Id);

                throw new GenericException(msg, msg);
            }

            //Status
            if (entity.estado != SolicitudViandaEstado.Registrado) {
                var msg = string.Format("La solicitud se puede eliminar siempre y cuando su estado sea REGISTRADO");
                throw new GenericException(msg, msg);
            }
 
            await base.Delete(input);
        }


        protected override IQueryable<SolicitudVianda> ApplyFilter(IQueryable<SolicitudVianda> query,
            PagedAndFilteredResultRequestDto input)
        {

            //Try to sort query if available
            var filterInput = input as PagedAndFilteredResultRequestDto;
            if (filterInput != null && filterInput.Filter != null)
            {
                //TODO: Mejorar el proceso, mas reutilizable.
                //Campos fechas, aplicar Truncate.   Value>=CampoFecha && Value<=CampoFecha+1 

                //Exclude
                object valueFilterRequestDate = null;
                var newFilters = new List<FilterEntity>();
                foreach (var filter in filterInput.Filter)
                {
                    if (filter.Field.ToUpper() == "FECHA_SOLICITUD")
                    {
                        valueFilterRequestDate = filter.Value;
                        continue;
                    }

                    newFilters.Add(filter);
                }
                query = query.Where(newFilters);

                //Add Date
                if (valueFilterRequestDate != null)
                {
                    DateTime? CreatedDate = valueFilterRequestDate as DateTime?;
                    query = query.Where(p => p.fecha_solicitud >= DbFunctions.TruncateTime(CreatedDate) && p.fecha_solicitud <= DbFunctions.AddDays(CreatedDate, 1));

                }
            }

            //No filter
            return query;


        }

        public async Task<bool> Cancel(int id, string observaciones)
        {
           
            var entity = await Get(new EntityDto<int>(id));
            if (entity == null)
            {
                var msg = string.Format("El Registro con identificacion {0} no existe, o sus datos asociados no existen",
                  id);

                throw new GenericException(msg, msg);
            }

            //Rules
            if (entity.estado == SolicitudViandaEstado.Cancelado)
            {
                var msg = string.Format("La solicitud ya se encuentra en estado CANCELADA");
                throw new GenericException(msg, msg);
            }

            if (entity.estado != SolicitudViandaEstado.Registrado)
            {
                var msg = string.Format("La solicitud se puede cancelar siempre y cuando su estado sea REGISTRADO");
                throw new GenericException(msg, msg);
            }

            entity.observaciones = observaciones;
            entity.estado = SolicitudViandaEstado.Cancelado;

            var result = await base.Update(entity);
             return true;
        }

        public async Task<IList<SolicitudViandaDto>> GetMySolicitud(DateTime? fecha,List<SolicitudViandaEstado> incluir)
        {
            var userAutentificado = Application.GetCurrentUser();

            if (userAutentificado == null)
            {
                var msg = string.Format("Intento de recupera solicitudes de viandas");
                throw new GenericException(msg,
                    "Seguridad: No se puede obtener solicitudes de viandas, el usuario no esta autentificado");
            }

            //TODO: Pendiente.. que el cliente proporcione el modelo actualizado
            //Colaborador, asociado a un usuario

            //Si fecha es nula, obtener solicitudes de viandas de hoy
            var fechaFiltro = DateTime.Now;
            if (fecha.HasValue)
            {
                fechaFiltro = fecha.Value;
            }

            var query = Repository.GetAll();
            //foreach (var item in incluir)
            if (incluir.Count > 0)
            {
                query = query.Where(d => incluir.Contains(d.estado));
            }



            var items = await(from item in query
                              where
                                DbFunctions.TruncateTime(item.fecha_solicitud) == DbFunctions.TruncateTime(fechaFiltro)
                                //&& item.solicitante.usuario_id == userAutentificado.Id
                              select new SolicitudViandaDto
                              {
                                  Id = item.Id,
                                  alcance_viandas = item.alcance_viandas,
                                  consumido = item.consumido,
                                  consumo_justificado = item.consumo_justificado,
                                  disciplina_id = item.disciplina_id,
                                  disciplina_nombre = item.disciplina.nombre,
                                  estado = item.estado,
                                  fecha_alcancce = item.fecha_alcancce,
                                  fecha_solicitud = item.fecha_solicitud,
                                  LocacionId = item.LocacionId,
                                  locacion_nombre = item.locacion.nombre,
                                  //zona_nombre = item.locacion.Zona.nombre,
                                  zona_nombre ="",
                                  area_id = item.area_id,
                                  area_nombre = item.area.nombre,

                                  pedido_viandas = item.pedido_viandas,
                                  referencia_ubicacion = item.referencia_ubicacion,
                                  solicitante_id = item.solicitante_id,
                                  solicitante_nombre = item.solicitante.nombres,
                                  solicitud_original_id = item.solicitud_original_id,
                                  tipo_comida_id = item.tipo_comida_id,
                                  tipo_comida_nombre = item.tipo_comida.nombre,
                                  total_consumido = item.total_consumido,
                                  total_pedido = item.total_pedido
                              }).ToListAsync();
            return items;
        }
    }
}
