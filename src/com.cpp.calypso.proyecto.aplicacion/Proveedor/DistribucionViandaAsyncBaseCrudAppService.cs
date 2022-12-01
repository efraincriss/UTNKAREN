using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.framework.Extensions;
using com.cpp.calypso.framework;
using Microsoft.AspNet.Identity;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class DistribucionViandaAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<DistribucionVianda, DistribucionViandaDto,
            PagedAndFilteredResultRequestDto>, IDistribucionViandaAsyncBaseCrudAppService
    {

        public IBaseRepository<DetalleDistribucion> RepositoryDetalle { get; }
        public ISolicitudViandaAsyncBaseCrudAppService SolicitudViandaService { get; }
        public IBaseRepository<SolicitudVianda> RepositorySolicitudVianda { get; }

        private readonly IBaseRepository<dominio.Proveedor.Proveedor> _proveedorRepository;
        private readonly IBaseRepository<Contacto> _contactoRepository;
        private readonly IdentityEmailMessageService _correoservice;
        public DistribucionViandaAsyncBaseCrudAppService(
            IBaseRepository<DistribucionVianda> repository,
            IBaseRepository<DetalleDistribucion> repositoryDetalle,
            ISolicitudViandaAsyncBaseCrudAppService solicitudViandaService,
            IBaseRepository<SolicitudVianda> repositorySolicitudVianda,
            IdentityEmailMessageService correoservice,
            IBaseRepository<dominio.Proveedor.Proveedor> proveedorRepository,
            IBaseRepository<Contacto> contactoRepository

            ) : base(repository)
        {
            RepositoryDetalle = repositoryDetalle;
            SolicitudViandaService = solicitudViandaService;
            RepositorySolicitudVianda = repositorySolicitudVianda;
            _correoservice = correoservice;
            _proveedorRepository = proveedorRepository;
            _contactoRepository = contactoRepository;
        }



        public async Task<IList<DistribucionViandaProveedorDto>>
            GetSolicitudesAsignadas(DateTime fecha, int tipoComidaId)
        {

            var query = Repository.GetAll();
            var detalleQuery = RepositoryDetalle.GetAll();

            var items = await (from distribucion in query
                               join detalle in detalleQuery
                               on distribucion.Id equals detalle.DistribucionViandaId
                               where
                                  DbFunctions.TruncateTime(distribucion.fecha) == DbFunctions.TruncateTime(fecha)
                                  && distribucion.tipo_comida_id == tipoComidaId
                               select new DistribucionViandaProveedorDto
                               {
                                   Id = distribucion.Id,
                                   detalle_distribuccion_id = detalle.Id,
                                   ProveedorId = distribucion.ProveedorId,
                                   proveedor_identificacion = distribucion.Proveedor.identificacion,
                                   proveedor_nombre = distribucion.Proveedor.razon_social,
                                   proveedor_zona = distribucion.Proveedor.zonas.FirstOrDefault(p => p.ProveedorId == distribucion.ProveedorId).Zona.nombre,
                                   fecha = distribucion.fecha,
                                   tipo_comida_id = distribucion.tipo_comida_id,
                                   tipo_comida_nombre = distribucion.tipo_comida.nombre,
                                   solicitud_id = detalle.SolicitudViandaId,

                                   solicitante_nombre = detalle.SolicitudVianda.solicitante.nombres_apellidos,
                                   LocacionId = detalle.SolicitudVianda.LocacionId,
                                   locacion_nombre = detalle.SolicitudVianda.locacion.nombre,

                                   pedido_viandas = detalle.SolicitudVianda.pedido_viandas,
                                   alcance_viandas = detalle.SolicitudVianda.alcance_viandas,

                                   total_pedido = detalle.total_asignado,
                                   estado = distribucion.estado,

                                   disciplina_id = detalle.SolicitudVianda.disciplina_id,
                                   disciplina_nombre = detalle.SolicitudVianda.disciplina.nombre,

                                   AreaId = detalle.SolicitudVianda.area.Id,
                                   area_nombre = detalle.SolicitudVianda.area.nombre,

                                   estado_solicitud = detalle.SolicitudVianda.estado,
                                   anotador_nombre=detalle.SolicitudVianda.anotador.nombres_apellidos

                               }).ToListAsync();

            return items;
        }



        public async Task<PagedResultDto<DistribucionViandaGrupoDto>>
            GetSolicitudesAsignadasGrupo(PagedAndSortedResultRequestDto pagedAndSorted, DateTime? fecha, int? tipoComidaId)
        {



            //var query = Repository.GetAll();
            var detalleQuery = RepositoryDetalle.GetAll();


            //Filter
            //IQueryable<DetalleDistribucion> where;
            if (fecha.HasValue)
            {
                detalleQuery = detalleQuery.Where(
                                 detalle => DbFunctions.TruncateTime(detalle.DistribucionVianda.fecha) == DbFunctions.TruncateTime(fecha.Value)
                               );
            }

            if (tipoComidaId.HasValue)
            {
                detalleQuery = detalleQuery.Where(
                    detalle => detalle.DistribucionVianda.tipo_comida_id == tipoComidaId.Value
                    );
            }

            //Group 
            var queryDto = (from item in detalleQuery
                            group item by new
                            {
                                item.DistribucionVianda.fecha,
                                item.DistribucionVianda.tipo_comida_id,
                                item.DistribucionVianda.tipo_comida.nombre
                            } into totales
                            select new DistribucionViandaGrupoDto
                            {
                                fecha = totales.Key.fecha,
                                tipo_comida_id = totales.Key.tipo_comida_id,
                                tipo_comida_nombre = totales.Key.nombre,
                                total_solicitudes = totales.Count(),
                                total_pedido = totales.Sum(a => a.total_asignado)
                            });

            var totalCount = await queryDto.CountAsync();

            //TODO: Aplicar ordenamiento... 
            //Order
            queryDto = queryDto.OrderByDescending(c => c.fecha);

            //Paged
            queryDto = queryDto.Skip(pagedAndSorted.SkipCount).Take(pagedAndSorted.MaxResultCount);

            return new PagedResultDto<DistribucionViandaGrupoDto>(
                 totalCount,
                 await queryDto.ToListAsync()
             );

        }



        public async Task<bool> Distribute(DateTime fecha, int tipoComidaId, List<DistribucionViandaProveedorDto> model, List<int> deleteIds)
        {
            IBaseRepository<DistribucionVianda> baseRepository = Repository as IBaseRepository<DistribucionVianda>;


            //TODO: Mejorar

            //var detalleQuery = RepositoryDetalle.GetAll();
            var query = baseRepository.GetAllIncluding(c => c.Detalle);

            var distribucionesExistentes = await (from distribucion in query
                                                  where
                                                   DbFunctions.TruncateTime(distribucion.fecha) == DbFunctions.TruncateTime(fecha)
                                                   && distribucion.tipo_comida_id == tipoComidaId
                                                  select distribucion).ToListAsync();


            var verificarEstado = distribucionesExistentes.Where(d => d.estado != DistribucionViandaEstado.Registrado && d.estado != DistribucionViandaEstado.Aprobado).Count();

            if (verificarEstado != 0)
            {
                var msg = string.Format("Para guardar la distribución de pedidos, el estado debe ser REGISTRADO o APROBADO");
                throw new GenericException(msg, msg);
            }

            var solicitudesId = new List<int>();
            var solicitudesDeleteId = new List<int>();

            foreach (var item in model)
            {
                var distribucionProveedor = distribucionesExistentes.Where(p => p.ProveedorId == item.ProveedorId).SingleOrDefault();
                if (distribucionProveedor == null)
                {
                    //Add Distribucion
                    distribucionProveedor = new DistribucionVianda();
                    distribucionProveedor.ProveedorId = item.ProveedorId;
                    distribucionProveedor.fecha = fecha;
                    distribucionProveedor.tipo_comida_id = tipoComidaId;
                    distribucionProveedor.estado = DistribucionViandaEstado.Registrado;

                    distribucionesExistentes.Add(distribucionProveedor);
                }

                var detalleSolicitud = distribucionProveedor.Detalle.Where(c => c.SolicitudViandaId == item.solicitud_id).SingleOrDefault();
                if (detalleSolicitud == null)
                {

                    //Add Distribucion Detalle
                    detalleSolicitud = new DetalleDistribucion();
                    detalleSolicitud.SolicitudViandaId = item.solicitud_id;
                    detalleSolicitud.total_asignado = item.total_pedido;
                    distribucionProveedor.Detalle.Add(detalleSolicitud);
                }
                //else {
                //    if (deleteIds!=null && deleteIds.Count > 0 && deleteIds.Contains(detalleSolicitud.SolicitudViandaId)) {

                //        //distribucionProveedor.Detalle.Remove(detalleSolicitud);

                //    }
                //}

                solicitudesId.Add(item.solicitud_id);
            }


            //Ajustar Totales
            foreach (var item in distribucionesExistentes)
            {
                item.total_pedido = item.Detalle.Sum(c => c.total_asignado);
                item.estado = DistribucionViandaEstado.Registrado;
            }


            //1. New / Update
            var result = baseRepository.SaveOrUpdate(distribucionesExistentes);


            //2. Delete 
            if (deleteIds != null && deleteIds.Count > 0)
            {

                var detalleDistribucions = (from item in distribucionesExistentes
                                                //where
                                                //    item.Detalle.Any(c => deleteIds.Value.Contains(c.SolicitudViandaId))
                                            select item.Detalle.Where(c => deleteIds.Contains(c.Id)).ToList())
                                           .ToList();

                var detalleEliminar = new List<DetalleDistribucion>();
                foreach (var items in detalleDistribucions)
                {
                    detalleEliminar.AddRange(items);
                }

                solicitudesDeleteId.AddRange(detalleEliminar.Select(s => s.SolicitudViandaId).ToList());

                RepositoryDetalle.Delete(detalleEliminar);

                foreach (var item in distribucionesExistentes)
                {
                    foreach (var detalle in item.Detalle)
                    {
                        if (solicitudesDeleteId.Contains(detalle.SolicitudViandaId))
                        {
                            item.Detalle.Remove(detalle);
                        }
                    }
                }

                var distribucionesSinDetalles = (from item in distribucionesExistentes
                                                 where
                                                     item.Detalle.Count == 0
                                                 select item)
                                           .ToList();



                //2.1 Existe Distribuccion sin detalles.
                baseRepository.Delete(distribucionesSinDetalles);

            }


            //3. Estados Solicitudes
            if (solicitudesDeleteId.Count > 0)
            {

                await SolicitudViandaService.UpdateState(solicitudesDeleteId.Distinct(),
                SolicitudViandaEstado.Registrado);
            }

            var solicitudesUpdate = await SolicitudViandaService.UpdateState(solicitudesId.Distinct(),
                SolicitudViandaEstado.Asignada);



            return true;
        }

        /// <summary>
        /// Verificar si existe una distribucion creada para la fecha y el tipo de comida enviado como parametro
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipoComidaId"></param>
        /// <returns></returns>
        public async Task<bool> Exists(DateTime fecha, int tipoComidaId)
        {

            var total = await Repository.LongCountAsync(distribucion =>
                                   DbFunctions.TruncateTime(distribucion.fecha) == DbFunctions.TruncateTime(fecha)
                                  && distribucion.tipo_comida_id == tipoComidaId);
            return total > 0;
        }

        public async Task<IList<DistribucionTransporteDto>> GetDistribucionTransporte(DateTime fecha, int tipoComidaId)
        {

            var query = Repository.GetAll();
            var detalleQuery = RepositoryDetalle.GetAll();

            var items = await (from distribucion in query
                               where
                                  DbFunctions.TruncateTime(distribucion.fecha) == DbFunctions.TruncateTime(fecha)
                                  && distribucion.tipo_comida_id == tipoComidaId
                               select new DistribucionTransporteDto
                               {
                                   Id = distribucion.Id,
                                   ProveedorId = distribucion.ProveedorId,
                                   proveedor_nombre = distribucion.Proveedor.razon_social,
                                   fecha = distribucion.fecha,
                                   tipo_comida_id = distribucion.tipo_comida_id,
                                   tipo_comida_nombre = distribucion.tipo_comida.nombre,
                                   total_pedido = distribucion.total_pedido,
                                   estado = distribucion.estado,
                                   conductor_asignado_id = (distribucion.conductor_asignado_id != null) ? distribucion.conductor_asignado_id : null,
                                   conductor_asignado_nombre = (distribucion.conductor_asignado != null) ? distribucion.conductor_asignado.nombres_apellidos : string.Empty
                               }).ToListAsync();

            return items;
        }

        public async Task<bool> DistributeTransport(DateTime fecha, int tipoComidaId, List<DistribucionTransporteUpdateDto> model)
        {

            IBaseRepository<DistribucionVianda> baseRepository = Repository as IBaseRepository<DistribucionVianda>;

            var distribuccionIds = model.Select(c => c.Id).ToList();

            var query = baseRepository.GetAll();

            var distribucionesExistentes = await (from distribucion in query
                                                  where
                                                   DbFunctions.TruncateTime(distribucion.fecha) == DbFunctions.TruncateTime(fecha)
                                                   && distribucion.tipo_comida_id == tipoComidaId
                                                   && distribuccionIds.Contains(distribucion.Id)
                                                  select distribucion).ToListAsync();

            //Rules
            var verificar = distribucionesExistentes.Where(d => d.estado != DistribucionViandaEstado.Aprobado && d.estado != DistribucionViandaEstado.AsignadoTransporte).Count();

            if (verificar != 0)
            {
                var msg = string.Format("Para asignar transportistas, el estado de la distribucción de la fecha y tipo de comida seleccionado, debe ser APROBADO o ASIGNADO TRANSPORTE");
                throw new GenericException(msg, msg);
            }

            foreach (var item in distribucionesExistentes)
            {
                var asignacionTransporte = model.Where(p => p.Id == item.Id).SingleOrDefault();
                if (asignacionTransporte != null)
                {
                    //Asignar transportista...
                    //Si es nulo, se lo quita
                    item.conductor_asignado_id = asignacionTransporte.conductor_asignado_id;

                    //Cambiar estado...
                    if (item.conductor_asignado_id == null)
                        item.estado = DistribucionViandaEstado.Aprobado;
                }
            }


            //Update
            var result = baseRepository.SaveOrUpdate(distribucionesExistentes);

            return true;
        }

        public async Task<bool> ApproveDistributeTransport(DateTime fecha, int tipoComidaId)
        {
            IBaseRepository<DistribucionVianda> baseRepository = Repository as IBaseRepository<DistribucionVianda>;

            var query = baseRepository.GetAllIncluding(c => c.Detalle);

            var distribucionesExistentes = await (from distribucion in query
                                                  where
                                                   DbFunctions.TruncateTime(distribucion.fecha) == DbFunctions.TruncateTime(fecha)
                                                   && distribucion.tipo_comida_id == tipoComidaId
                                                  select distribucion).ToListAsync();

            //Rules
            //Todas las distribucciones deben tener asignado transporte
            var verificar = distribucionesExistentes.Where(d => d.conductor_asignado_id == null).Count();

            if (verificar != 0)
            {
                var msg = string.Format("Para aprobar e imprimir, todos los pedidos deben tener asignado un transportista");
                throw new GenericException(msg, msg);
            }


            var verificarEstado = distribucionesExistentes.Where(d => d.estado != DistribucionViandaEstado.Aprobado && d.estado != DistribucionViandaEstado.AsignadoTransporte).Count();

            if (verificarEstado != 0)
            {
                var msg = string.Format("Para aprobar e imprimir, el estado de los pedidos deben ser APROBADO o ASIGNADO TRANSPORTE");
                throw new GenericException(msg, msg);
            }

            var solicitudesId = new List<int>();
            var solicitudesDeleteId = new List<int>();

            foreach (var item in distribucionesExistentes)
            {

                item.estado = DistribucionViandaEstado.AsignadoTransporte;

            }

            //Solicitudes
            var detalleDistribucions = (from item in distribucionesExistentes
                                        select item.Detalle.Select(d => d.SolicitudViandaId))
                                         .ToList();

            foreach (var items in detalleDistribucions)
            {
                solicitudesId.AddRange(items);
            }

            //Update Distribuccion
            var result = baseRepository.SaveOrUpdate(distribucionesExistentes);



            //Estados Solicitudes
            var solicitudesUpdate = await SolicitudViandaService.UpdateState(solicitudesId.Distinct(),
                SolicitudViandaEstado.AsignadaTransporte);



            return true;
        }

        public async Task<DistribucionSolicitudDto> GetDistribucionSolicitudVianda(int solicitudId)
        {
            var detalleQuery = RepositoryDetalle.GetAll();

            var item = await (from detalle in detalleQuery
                              where
                                 detalle.SolicitudViandaId == solicitudId
                              select new DistribucionSolicitudDto
                              {
                                  Id = detalle.DistribucionViandaId,
                                  detalle_distribuccion_id = detalle.Id,
                                  ProveedorId = detalle.DistribucionVianda.ProveedorId,
                                  proveedor_nombre = detalle.DistribucionVianda.Proveedor.razon_social,
                                  fecha = detalle.DistribucionVianda.fecha,
                                  tipo_comida_id = detalle.SolicitudVianda.tipo_comida_id,
                                  tipo_comida_nombre = detalle.SolicitudVianda.tipo_comida.nombre,
                                  solicitud_id = detalle.SolicitudViandaId,
                                  fecha_solicitud = detalle.SolicitudVianda.fecha_solicitud,

                                  solicitante_nombre = detalle.SolicitudVianda.solicitante.nombres,
                                  solicitante_id = detalle.SolicitudVianda.solicitante_id,
                                  LocacionId = detalle.SolicitudVianda.LocacionId,
                                  locacion_nombre = detalle.SolicitudVianda.locacion.nombre,

                                  pedido_viandas = detalle.SolicitudVianda.pedido_viandas,
                                  alcance_viandas = detalle.SolicitudVianda.alcance_viandas,

                                  total_pedido = detalle.SolicitudVianda.total_pedido,

                                  total_consumido = detalle.SolicitudVianda.total_consumido,

                                  estado = detalle.DistribucionVianda.estado,

                                  disciplina_id = detalle.SolicitudVianda.disciplina_id,
                                  disciplina_nombre = detalle.SolicitudVianda.disciplina.nombre,

                                  AreaId = detalle.SolicitudVianda.area.Id,
                                  area_nombre = detalle.SolicitudVianda.area.nombre,

                                  estado_solicitud = detalle.SolicitudVianda.estado,

                                  conductor_asignado_id = (detalle.DistribucionVianda.conductor_asignado_id != null) ? detalle.DistribucionVianda.conductor_asignado_id : null,
                                  conductor_asignado_nombre = (detalle.DistribucionVianda.conductor_asignado != null) ? detalle.DistribucionVianda.conductor_asignado.nombres_apellidos : string.Empty,

                                  anotador_id = (detalle.SolicitudVianda.anotador_id) != null ? detalle.SolicitudVianda.anotador_id : null,
                                  anotador_nombre = (detalle.SolicitudVianda.anotador_id) != null ? detalle.SolicitudVianda.anotador.nombres : string.Empty,

                              }).SingleOrDefaultAsync();

            return item;
        }

        public async Task<bool> ApproveDistribute(DateTime fecha, int tipoComidaId)
        {
            var solicitudesPendientes = await SolicitudViandaService.GetSolicitudesPendientes(fecha, tipoComidaId);

            //Rules
            if (solicitudesPendientes.Count > 0)
            {
                var msg = string.Format("Solicitudes pendientes de asignación. Se debe asignar todos los pedidos, para aprobar");
                throw new GenericException(msg, msg);
            }


            IBaseRepository<DistribucionVianda> baseRepository = Repository as IBaseRepository<DistribucionVianda>;

            var query = baseRepository.GetAllIncluding(c => c.Detalle);

            var distribucionesExistentes = await (from distribucion in query
                                                  where
                                                   DbFunctions.TruncateTime(distribucion.fecha) == DbFunctions.TruncateTime(fecha)
                                                   && distribucion.tipo_comida_id == tipoComidaId
                                                  select distribucion).ToListAsync();

            var verificarEstado = distribucionesExistentes.Where(d => d.estado == DistribucionViandaEstado.Registrado).Count();

            if (verificarEstado == 0)
            {
                var msg = string.Format("No existen pedidos para aprobar");
                throw new GenericException(msg, msg);
            }

            verificarEstado = distribucionesExistentes.Where(d => d.estado != DistribucionViandaEstado.Registrado && d.estado != DistribucionViandaEstado.Aprobado).Count();

            if (verificarEstado != 0)
            {
                var msg = string.Format("Para aprobar, el estado de los pedidos deben ser REGISTRADO o APROBADO");
                throw new GenericException(msg, msg);
            }




            var solicitudesId = new List<int>();

            foreach (var item in distribucionesExistentes.Where(d => d.estado == DistribucionViandaEstado.Registrado))
            {
                item.estado = DistribucionViandaEstado.Aprobado;
            }

            //Solicitudes
            var detalleDistribucions = (from item in distribucionesExistentes
                                        select item.Detalle.Select(d => d.SolicitudViandaId))
                                         .ToList();

            foreach (var items in detalleDistribucions)
            {
                solicitudesId.AddRange(items);
            }

            //Update Distribuccion
            var result = baseRepository.SaveOrUpdate(distribucionesExistentes);


            //Estados Solicitudes
            var solicitudesUpdate = await SolicitudViandaService.UpdateState(solicitudesId.Distinct(),
                SolicitudViandaEstado.Aprobada);


            return true;
        }

        public async Task<TotalesDistribuccionDto> GetTotales(DateTime? fecha, int? tipoComidaId)
        {
            var result = new TotalesDistribuccionDto()
            {
                TotalConsumidos = 0,
                TotalDistribucciones = 0,
                TotalPedidos = 0,
                TotalPorConsumir = 0
            };

            //repositorySolicitudVianda

            var solicitudesQuery = RepositorySolicitudVianda.GetAll();


            //Filter
            //IQueryable<DetalleDistribucion> where;
            if (fecha.HasValue)
            {
                solicitudesQuery = solicitudesQuery.Where(
                                 solicitud => DbFunctions.TruncateTime(solicitud.fecha_solicitud) == DbFunctions.TruncateTime(fecha.Value)
                               );
            }

            if (tipoComidaId.HasValue)
            {
                solicitudesQuery = solicitudesQuery.Where(
                    solicitud => solicitud.tipo_comida_id == tipoComidaId.Value
                    );
            }

            //Group 
            var queryDto = (from item in solicitudesQuery
                            group item by new
                            {
                                item.fecha_solicitud,
                                item.tipo_comida_id,
                                item.tipo_comida.nombre
                            } into totales
                            select new TotalesDistribuccionDto
                            {
                                TotalConsumidos = totales.Sum(s => s.total_consumido),
                                TotalPedidos = totales.Sum(s => s.total_pedido)
                            });

            var resultDb = queryDto.SingleOrDefault();
            if (resultDb != null)
                result = resultDb;

            return result;
        }

        public async Task<bool> EnviarDistribucionaProveedores(DateTime fecha)
        {
            IdentityMessage cuerpo = new IdentityMessage();
            cuerpo.Subject = "PMDIS: Distribuciones de Viandas a Proveedor";
            

            try
            {

                var distribuciones_viandas = Repository.GetAll().Where(c => c.fecha == fecha)
                                                               .Where(c => c.estado == DistribucionViandaEstado.Aprobado)
                                                               .ToList();

                foreach (var d in distribuciones_viandas)
                {
                    string lineasolicitud = "";
                    var solicitudes = RepositoryDetalle.GetAllIncluding(c=>c.SolicitudVianda.disciplina,c=>c.SolicitudVianda.solicitante,
                        c=>c.SolicitudVianda.locacion).Where(c => c.DistribucionViandaId == d.Id)
                                                              .Where(c => c.SolicitudVianda.estado == SolicitudViandaEstado.Aprobada)

                                                              .ToList();
                    //item.SolicitudVianda.locacion.nombre 
                    if (solicitudes.Count > 0) {
                        foreach (var item in solicitudes)
                        {
                            string Locacion = "";
                            var locacion = item.SolicitudVianda.locacion;
                            if (locacion != null && locacion.Id > 0) {
                                Locacion = locacion.nombre;
                            }

                            lineasolicitud = lineasolicitud + "<br/>*" + item.SolicitudVianda.solicitante.nombres_apellidos + "<br/>- " + " Locación "+ Locacion + "<br/>- " + item.SolicitudVianda.disciplina.nombre + "<br/>- " + item.SolicitudVianda.total_pedido + " viandas<br/><br/> ";
                        }
                
                    }

                    var proveedor = _proveedorRepository.Get(d.ProveedorId);

                   

                    if (proveedor != null)
                    {

                        cuerpo.Body = "Estimado: " + proveedor.razon_social + " para el día :"+fecha.ToShortDateString() +".<br/>"
                                                   + "Se informa que tiene solicitudes asignadas:"
                                                   + lineasolicitud
                                                   + "<br/>";

                        var contacto = _contactoRepository.GetAll().Where(c => c.Id == proveedor.contacto_id).Where(c => c.vigente).FirstOrDefault();
                        if (contacto != null && contacto.correo_electronico != null && contacto.correo_electronico.Length > 0)
                        {
                            cuerpo.Destination = contacto.correo_electronico.ToLower();
                            await _correoservice.SendAsync(cuerpo);

                        }
             
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
                return false;
            }

        }
    }
}
