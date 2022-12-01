using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using Castle.Core.Internal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Web;
using System.Data.Entity.Validation;
using System.Net.Mail;
using com.cpp.calypso.framework;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor
{
    public class ReservaHotelAsyncBaseCrudAppService : AsyncBaseCrudAppService<ReservaHotel, ReservaHotelDto, PagedAndFilteredResultRequestDto>, IReservaHotelAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<EspacioHabitacion> _espacioHabitacionRepository;
        private readonly IBaseRepository<DetalleReserva> _detalleReservaRepository;
        private readonly IBaseRepository<Colaboradores> _colaboradorRepository;
        private readonly IBaseRepository<ColaboradorServicio> _colabordorServicioRepository;
        private readonly IDetalleReservaAsyncBaseCrudAppService _detalleReservaService;
        private readonly IProveedorAsyncBaseCrudAppService _proveedorService;
        private readonly IBaseRepository<ColaboradoresVisita> _colaboradorVisita;
        private readonly IBaseRepository<ContratoProveedor> _contratoProveedorRepository;
        private readonly IBaseRepository<TarifaHotel> _tarifaHotelRepository;
        private readonly IBaseRepository<Archivo> _archivoService;
        private readonly IBaseRepository<ColaboradoresAusentismo> _colaboradorausentismorepository;
        private readonly IBaseRepository<NovedadColaborador> _novedadColaboradorRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IdentityEmailMessageService _correoservice;
        private readonly IBaseRepository<ParametroSistema> _parametrorepository;
        private readonly IBaseRepository<com.cpp.calypso.proyecto.dominio.Proveedor.Proveedor> _proveedorrepository;
        private readonly IBaseRepository<ServicioProveedor> _proveedorServiciorepository;

        private readonly IBaseRepository<ContratoProveedor> _contratoproveedorrepository;
        private readonly IBaseRepository<TarifaHotel> _tarifahotelrepository;

        public ReservaHotelAsyncBaseCrudAppService(

            IBaseRepository<ReservaHotel> repository,
            IBaseRepository<EspacioHabitacion> espacioHabitacionRepository,
            IBaseRepository<DetalleReserva> detalleReservaRepository,
            IBaseRepository<Colaboradores> colaboradorRepository,
            IBaseRepository<ColaboradorServicio> colabordorServicioRepository,
            IDetalleReservaAsyncBaseCrudAppService detalleReservaService,
            IProveedorAsyncBaseCrudAppService proveedorService,
            IBaseRepository<ColaboradoresVisita> colaboradorVisita,
            IBaseRepository<ContratoProveedor> contratoProveedorRepository,
            IBaseRepository<TarifaHotel> tarifaHotelRepository,
            IBaseRepository<Archivo> archivoService,
            IBaseRepository<ColaboradoresAusentismo> colaboradorausentismorepository,
            IBaseRepository<NovedadColaborador> novedadColaboradorRepository,
            IBaseRepository<Catalogo> catalogoRepository,
IBaseRepository<ContratoProveedor> contratoproveedorrepository,
       IBaseRepository<TarifaHotel> tarifahotelrepository,
        IBaseRepository<ParametroSistema> parametroRepository,
               IdentityEmailMessageService correoservice,
                   IBaseRepository<com.cpp.calypso.proyecto.dominio.Proveedor.Proveedor> proveedorrepository,
                   IBaseRepository<ServicioProveedor> proveedorServiciorepository
            ) : base(repository)
        {
            _contratoproveedorrepository = contratoproveedorrepository;
            _tarifahotelrepository= tarifahotelrepository;
            _parametrorepository = parametroRepository;
            _espacioHabitacionRepository = espacioHabitacionRepository;
            _detalleReservaRepository = detalleReservaRepository;
            _colaboradorRepository = colaboradorRepository;
            _colabordorServicioRepository = colabordorServicioRepository;
            _detalleReservaService = detalleReservaService;
            _proveedorService = proveedorService;
            _colaboradorVisita = colaboradorVisita;
            _contratoProveedorRepository = contratoProveedorRepository;
            _tarifaHotelRepository = tarifaHotelRepository;
            _archivoService = archivoService;
            _colaboradorausentismorepository = colaboradorausentismorepository;
            _novedadColaboradorRepository = novedadColaboradorRepository;
            _catalogoRepository = catalogoRepository;
            _correoservice = correoservice;
            _proveedorrepository = proveedorrepository;
            _proveedorServiciorepository = proveedorServiciorepository;
        }



        // Buscar espacios disponibles entre fechas
        public List<EspacioHabitacionDto> BuscarEspaciosLibres(DateTime fechaInicio, DateTime fechaFin)
        {
            // Todos los espacios Activos y Libres
            var queryEspacios = _espacioHabitacionRepository.GetAll()
                .Include(o => o.Habitacion)
                .Include(o => o.Habitacion.Proveedor)
                .Include(o => o.Habitacion.TipoHabitacion)
                .Where(o=>o.estado)
                .Where(o => o.activo);

            var queryDetalles = _detalleReservaRepository.GetAll()
                .Include(o => o.ReservaHotel)
                .Where(o => o.fecha_reserva >= fechaInicio && o.fecha_reserva <= fechaFin)
                .GroupBy(o => o.ReservaHotel.EspacioHabitacionId)
                .Select(group => group.FirstOrDefault());

            var temp = queryEspacios.ToList();
            // Reservas realizadas en las fechas
            /*var queryReservas = Repository.GetAll()
                .Where(o => o.fecha_desde >= fechaInicio && o.fecha_desde <= fechaFin || o.fecha_hasta >= fechaInicio && o.fecha_hasta<=fechaFin);*/


            // Espacios Libres
            var espaciosLibres = (from e in queryEspacios
                                  where !(from ocupados in queryDetalles
                                          select ocupados.ReservaHotel.EspacioHabitacionId).Contains(e.Id)
                                  orderby e.Habitacion.Proveedor.razon_social, e.Habitacion.numero_habitacion
                                  select e).ToList();

            var dtos = Mapper.Map<List<EspacioHabitacion>, List<EspacioHabitacionDto>>(espaciosLibres);
            return dtos;
        }


        public int ObtenerContratoProveedor(int ProveedorId, DateTime? fechaDesde)
        {
            if (fechaDesde.HasValue)
            {

                var fecha = fechaDesde.Value.Date;
                var contrato = _contratoproveedorrepository.GetAll()
                                                            .Where(c => c.ProveedorId == ProveedorId)
                                                            .Where(c => fecha >= c.fecha_inicio)
                                                            .Where(c => fecha <= c.fecha_fin)
                                                            .Where(c => c.estado == ContratoEstado.Activo || c.estado == ContratoEstado.Inactivo)
                                                            .OrderByDescending(c => c.fecha_fin)
                                                            .FirstOrDefault();

                return contrato != null && contrato.Id > 0 ? contrato.Id : -1;

            }
            else
            {

                var contrato = _contratoproveedorrepository.GetAll()
                                                            .Where(c => c.ProveedorId == ProveedorId)
                                                            .Where(c => c.fecha_inicio <= DateTime.Today)
                                                            .Where(c => c.fecha_fin >= DateTime.Today)
                                                            .Where(c => c.estado == ContratoEstado.Activo)
                                                            .OrderByDescending(c => c.fecha_fin)
                                                            .FirstOrDefault();
                return contrato != null && contrato.Id > 0 ? contrato.Id : -1;
            }

        }


        public decimal ObtenerTarifaHotel(int ContratoProveedorId, int TipoHabitacionId)
        {
            var tarifa = _tarifahotelrepository.GetAll()
                                               .Where(c => c.ContratoProveedorId == ContratoProveedorId)
                                               .Where(c => c.TipoHabitacionId == TipoHabitacionId)
                                               .Where(c => c.estado)
                                               .FirstOrDefault();
            if (tarifa != null)
            {
                return tarifa.costo_persona;
            }
            else
            {
                return 0;
            }
        }
        public void CrearDetallesReserva(int ReservaHotelId, DateTime fechaInicio, DateTime fechaFin)
        {
            //var entity = Mapper.Map<ReservaHotelDto, ReservaHotel>(dto);
            //var id = Repository.InsertAndGetId(entity);

            var reservaHotel = Repository.Get(ReservaHotelId);
            var reservaHotelAll = Repository.GetAllIncluding(c => c.EspacioHabitacion.Habitacion.TipoHabitacion).Where(x => x.Id == ReservaHotelId).FirstOrDefault();


            for (var dt = fechaInicio; dt <= fechaFin; dt = dt.AddDays(1))
            {
                var detalle = new DetalleReserva()
                {
                    ReservaHotelId = ReservaHotelId,
                    consumido = false,
                    facturado = false,
                    liquidado = false,
                    liquidacion_detalle_id = 0,
                    fecha_reserva = dt,
                    extemporaneo = reservaHotel != null && reservaHotel.extemporaneo ? true : false,
                    tiene_derecho = reservaHotel != null && reservaHotel.extemporaneo ? false : true
                };
                _detalleReservaRepository.Insert(detalle);
            }
            if (reservaHotelAll != null) {
                var ContratoProveedorId = this.ObtenerContratoProveedor(reservaHotelAll.EspacioHabitacion.Habitacion.ProveedorId, reservaHotel.fecha_desde.Date);
                var tarifa = this.ObtenerTarifaHotel(ContratoProveedorId, reservaHotelAll.EspacioHabitacion.Habitacion.TipoHabitacionId); ;

                reservaHotel.TipoHabitacionId = reservaHotelAll.EspacioHabitacion.Habitacion.TipoHabitacionId;
                reservaHotel.NombreTipoHabitacion= reservaHotelAll.EspacioHabitacion.Habitacion.TipoHabitacion.nombre;
                reservaHotel.NumeroHabitacion = reservaHotelAll.EspacioHabitacion.Habitacion.numero_habitacion;
                reservaHotel.CodigoEspacio = reservaHotelAll.EspacioHabitacion.codigo_espacio;
                reservaHotel.Costo = tarifa;
                

            }
          


        }

        public List<ColaboradorNombresDto> BuscarPorIdentificacionNombre(string identificacion = "", string nombre = "")
        {
            var query = _colaboradorRepository.GetAll()
                .Include(o => o.GrupoPersonal)
                ;

            if (!identificacion.IsNullOrEmpty())
            {
                query = query.Where(o => o.numero_identificacion.StartsWith(identificacion));
            }

            if (!nombre.IsNullOrEmpty())
            {
                query = query.Where(o =>
                    o.nombres_apellidos.Contains(nombre));
            }

            var entities = query.OrderByDescending(c => c.fecha_ingreso).ToList();

            var dto = Mapper.Map<List<Colaboradores>, List<ColaboradorNombresDto>>(entities);

            return dto;
        }

        public List<ColaboradorNombresDto> BuscarColaboradoresHospsdaje(string identificacion = "", string nombre = "")
        {
            var query = _colaboradorRepository.GetAll()
                                              .Where(c => c.estado == RRHHCodigos.ESTADO_ACTIVO)
                                              .Include(o => o.GrupoPersonal);

            if (!identificacion.IsNullOrEmpty())
            {
                query = query.Where(o => o.numero_identificacion.StartsWith(identificacion));
            }

            if (!nombre.IsNullOrEmpty())
            {
                query = query.Where(o =>
                    o.nombres_apellidos.Contains(nombre));
            }

            var entities = query.ToList();
            var dto = Mapper.Map<List<Colaboradores>, List<ColaboradorNombresDto>>(entities);

            var count = 1;
            foreach (var co in dto)
            {
                co.secuencial = count;
                count++;

                var servicio = _colabordorServicioRepository.GetAll()
                    .Where(o => o.vigente)
                    .Where(o => o.ColaboradoresId == co.Id)
                    .FirstOrDefault(o => o.Catalogo.codigo == CatalogosCodigos.SERVICIO_HOSPEDAJE);

                if (servicio != null)
                {
                    co.tiene_derecho = "SI";
                }
                else
                {
                    co.tiene_derecho = "NO";
                }

            }

            return dto;
        }

        public List<ReservaHotelDto> ListarReservas(DateTime fechaInicio, DateTime fechaFin)
        {
            var query = Repository.GetAll()
                .Include(o => o.Colaborador)
                .Include(o => o.EspacioHabitacion.Habitacion.Proveedor)
                .Include(o => o.EspacioHabitacion.Habitacion.TipoHabitacion)
                .Where(o => o.fecha_desde >= fechaInicio && o.fecha_desde <= fechaFin ||
                            o.fecha_hasta >= fechaInicio && o.fecha_hasta <= fechaFin);

            var entities = query.ToList();
            var dto = Mapper.Map<List<ReservaHotel>, List<ReservaHotelDto>>(entities);

            return dto;
        }

        public bool EliminarReserva(int reservaId)
        {
            var entity = Repository.Get(reservaId);

            /*if (entity.fecha_desde >= DateTime.Now)
            {
                return false;
            }*/


            var eliminados = _detalleReservaService.EliminarDetallesPorReservaId(reservaId);
            if (eliminados)
            {
                Repository.Delete(reservaId);

                return true;
            }
            return false;
        }

        public bool EditarReserva(int reservaId, DateTime fechaHasta)
        {
            var entity = Repository.Get(reservaId);
            if (fechaHasta > entity.fecha_hasta)
            {
                this.CrearDetallesReserva(entity.Id, entity.fecha_hasta.AddDays(1), fechaHasta);
            }

            var detallesEliminados = _detalleReservaService.EliminarReservasPosterioresA(reservaId, fechaHasta);

            if (detallesEliminados)
            {
                entity.fecha_hasta = fechaHasta;
                Repository.Update(entity);
                return true;
            }

            return false;

        }

        public List<TarifaTipoHabitacionDto> ListarTarifasTipoHabitacion(int proveedorId)
        {
            var currentDate = DateTime.Today;
            var contratoVigente = _contratoProveedorRepository.GetAll()
                    .Where(o => o.ProveedorId == proveedorId)
                    .Where(o => o.estado == ContratoEstado.Activo)
                    .FirstOrDefault(o => currentDate >= o.fecha_inicio && currentDate <= o.fecha_fin)
                ;

            if (contratoVigente == null)
                return new List<TarifaTipoHabitacionDto>();

            var tarifas = _tarifaHotelRepository.GetAll()
                .Include(o => o.TipoHabitacion)
                .Where(o => o.ContratoProveedorId == contratoVigente.Id)
                .Where(o => o.estado)
                .ToList();

            var dtos = Mapper.Map<List<TarifaHotel>, List<TarifaTipoHabitacionDto>>(tarifas);

            return dtos;
        }

        public string ReservaValidada(int colaboradorId, DateTime fechaInicio, DateTime fechaFin, int espacioId)
        {
            var espacio = _espacioHabitacionRepository.GetAll()
                .Include(o => o.Habitacion)
                .FirstOrDefault(o => o.Id == espacioId);

            var tipoHabitacionId = espacio.Habitacion.TipoHabitacionId;
            var proveedorId = espacio.Habitacion.ProveedorId;
            var currentDate = DateTime.Today;

            var contratoVigente = _contratoProveedorRepository.GetAll()
                .Where(o => o.ProveedorId == proveedorId)
                .Where(o => o.estado == ContratoEstado.Activo)
                .FirstOrDefault(o => currentDate >= o.fecha_inicio && currentDate <= o.fecha_fin)
                ;

            if (contratoVigente == null)
                return "Revisa las tarifas de hospedaje del proveedor";



            var tarifa = _tarifaHotelRepository.GetAll()
                .Where(o => o.TipoHabitacionId == tipoHabitacionId)
                .FirstOrDefault(o => o.ContratoProveedorId == contratoVigente.Id);

            if (tarifa == null)
                return "No existe una tarifa registrada en el contrato para este tipo de habitación";

            if (!tarifa.estado)
                return "La tarifa de esta habitación se encuentra deshabilitada";


            var reservas = _detalleReservaRepository.GetAll()
                .Where(o => o.ReservaHotel.ColaboradorId == colaboradorId)
                .Where(o => o.fecha_reserva.HasValue)
                .Count(o => o.fecha_reserva.Value >= fechaInicio && o.fecha_reserva.Value <= fechaFin);

            if (reservas > 0)
                return "El Colaborador ya tiene una reserva registrada entre las fechas: " + fechaInicio.ToShortDateString() + " - " + fechaFin.ToShortDateString();

            var colaborador = _colaboradorRepository.Get(colaboradorId);

            if (!colaborador.es_externo.HasValue || !colaborador.es_externo.Value)
                return "OK";

            var visita = _colaboradorVisita.GetAll()
                .Where(o => o.ColaboradoresId == colaboradorId)
                .Where(o => o.fecha_desde.HasValue)
                .Where(o => o.fecha_hasta.HasValue)
                .Count(o => fechaInicio >= o.fecha_desde.Value && fechaFin <= o.fecha_hasta.Value)
                ;

            if (visita == 0)
                return
                    "El colaborador visitante no tiene registrada su visita en el perido de fechas de la reserva, solicite el registro";

            return "OK";

        }

        #region PanelControl

        public List<HotelHabitacionDto> ListarHoteles(DateTime fecha)
        {
            var proveedores = _proveedorService.ListProveedorHospedaje();

            var list = new List<HotelHabitacionDto>();
            foreach (var proveedor in proveedores)
            {

                var espaciosTotales = _espacioHabitacionRepository
                    .GetAll()
                    .Include(o => o.Habitacion)
                    .Where(o => o.activo)

                    .Count(o => o.Habitacion.ProveedorId == proveedor.Id);

                var espaciosOcupados = _detalleReservaRepository
                    .GetAll()
                    .Where(o => o.ReservaHotel.EspacioHabitacion.Habitacion.ProveedorId == proveedor.Id)
                    .Count(o => o.fecha_reserva == fecha);

                var dto = new HotelHabitacionDto()
                {
                    razon_social = proveedor.razon_social,
                    espacio_ocupados = espaciosOcupados,
                    espacios_libres = espaciosTotales - espaciosOcupados,
                    espacios_totales = espaciosTotales,
                    Id = proveedor.Id
                };
                list.Add(dto);
            }

            return list;

        }


        public string CambiarEstadoDetallesLavanderia(List<DetalleReservaDto> data)
        {
            List<DateTime> fechas = new List<DateTime>();
   
            foreach (var item in data)
            {
                var detalle = _detalleReservaRepository.Get(item.Id);
                 detalle.aplica_lavanderia = true;
                detalle.extemporaneo = true;
                var update = _detalleReservaRepository.Update(detalle);
                fechas.Add(detalle.fecha_reserva.Value);
  
            }
       


            return "OK";
        }

        public string CambiarEstadoDetallesNoLavanderia(List<DetalleReservaDto> data)
        {
            foreach (var item in data)
            {
                var detalle = _detalleReservaRepository.Get(item.Id);     
                detalle.aplica_lavanderia = false;

                var update = _detalleReservaRepository.Update(detalle);
            }
            return "OK";
        }

        public string CambiarEstadoDetallesConsumidoE(List<DetalleReservaDto> data)
        {
            List<DateTime> fechas = new List<DateTime>();
            int ReservaHotelId = 0;
            foreach (var item in data)
            {
                var detalle = _detalleReservaRepository.Get(item.Id);
                detalle.fecha_consumo = detalle.fecha_reserva;
                detalle.consumido = true;
                detalle.extemporaneo = true;
                var update = _detalleReservaRepository.Update(detalle);
                fechas.Add(detalle.fecha_reserva.Value);
                ReservaHotelId = detalle.ReservaHotelId;
            }
            if (ReservaHotelId > 0)
            {
                var fechaMinima = fechas.Min();
                var ReservaHotel = Repository.Get(ReservaHotelId);
                ReservaHotel.inicio_consumo = true;
                ReservaHotel.fecha_inicio_consumo = fechaMinima;
                ReservaHotel.justificacion_inicio_manual = "Inicio Manual desde detalles reservas";
                Repository.Update(ReservaHotel);
            }


            return "OK";
        }

        public string CambiarEstadoDetallesNoConsumidoE(List<DetalleReservaDto> data)
        {
            foreach (var item in data)
            {
                var detalle = _detalleReservaRepository.Get(item.Id);
                detalle.fecha_consumo = null;
                detalle.consumido = false;
                detalle.extemporaneo = false;
                var update = _detalleReservaRepository.Update(detalle);
            }
            return "OK";
        }

        public string ReservaExtemporaneaValidada(int colaboradorId, DateTime fechaInicio, DateTime fechaFin, int espacioId)
        {
            var espacio = _espacioHabitacionRepository.GetAll()
               .Include(o => o.Habitacion)
               .FirstOrDefault(o => o.Id == espacioId);

            var tipoHabitacionId = espacio.Habitacion.TipoHabitacionId;
            var proveedorId = espacio.Habitacion.ProveedorId;
            var currentDate = DateTime.Today;

            var contratoVigente = _contratoProveedorRepository.GetAll()
                .Where(o => o.ProveedorId == proveedorId)
                .Where(o => o.estado == ContratoEstado.Activo)
                .FirstOrDefault(o => currentDate >= o.fecha_inicio && currentDate <= o.fecha_fin)
                ;

            if (contratoVigente == null)
                return "Revisa las tarifas de hospedaje del proveedor";



            var tarifa = _tarifaHotelRepository.GetAll()
                .Where(o => o.TipoHabitacionId == tipoHabitacionId)
                .FirstOrDefault(o => o.ContratoProveedorId == contratoVigente.Id);

            if (tarifa == null)
                return "No existe una tarifa registrada en el contrato para este tipo de habitación";

            if (!tarifa.estado)
                return "La tarifa de esta habitación se encuentra deshabilitada";


            var reservas = _detalleReservaRepository.GetAll()
                .Where(o => o.ReservaHotel.ColaboradorId == colaboradorId)
                .Where(o => o.fecha_reserva.HasValue)
                .Count(o => o.fecha_reserva.Value >= fechaInicio && o.fecha_reserva.Value <= fechaFin);

            if (reservas > 0)
                return "El Colaborador ya tiene una reserva registrada entre las fechas ingresadas";

            var colaborador = _colaboradorRepository.Get(colaboradorId);

            /*
           if (!colaborador.es_externo.HasValue || !colaborador.es_externo.Value)
               return "OK";




          var visita = _colaboradorVisita.GetAll()
               .Where(o => o.ColaboradoresId == colaboradorId)
               .Where(o => o.fecha_desde.HasValue)
               .Where(o => o.fecha_hasta.HasValue)
               .Count(o => fechaInicio >= o.fecha_desde.Value && fechaFin <= o.fecha_hasta.Value)
               ;

           if (visita == 0)
               return
                   "El colaborador visitante no tiene registrada su visita en el perido de fechas de la reserva, solicite el registro";
           */

            var catalogohospedaje = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.SERVICIO_HOSPEDAJE).FirstOrDefault();

            var servicio = _colabordorServicioRepository.GetAll()
                   .Where(o => o.vigente)
                   .Where(o => o.ColaboradoresId == colaborador.Id)
                   .FirstOrDefault(o => o.Catalogo.codigo == CatalogosCodigos.SERVICIO_HOSPEDAJE);

            if (servicio != null)//SI TIENE DERECHO 
            {
                var ausentismo = _colaboradorausentismorepository.GetAll()

                   .Where(o => o.colaborador_id == colaboradorId)
                   .Where(o => o.Colaborador.estado == "ACTIVO")
                   .Where(o => o.estado == "ACTIVO")
                   .Where(o => o.fecha_inicio.HasValue)
                   .Where(o => o.fecha_fin.HasValue)
                   .Count(o => fechaInicio >= o.fecha_inicio.Value && fechaFin <= o.fecha_fin.Value)
                   ;

                if (ausentismo > 0)
                    return
                        "El periódo de fechas de la reserva coincide con la fecha de ausentismo, no se puede realizar la reserva";

                return "OK";

            }
            else //No TIENE DERECHO
            {
                NovedadColaborador reg = new NovedadColaborador()
                {
                    ColaboradorId = colaboradorId,
                    ProveedorId = espacio.Habitacion.ProveedorId,
                    ServicioId = null,//verificar catalogo
                    OpcionComidaId = null,//Verificar no puede tener opcion comida
                    fecha = DateTime.Now,
                    observacion = "Hospedaje Consumido sin derecho",
                    vigente = true


                };
                var novedad = _novedadColaboradorRepository.Insert(reg);

                return "OK";
            }
        }

        public int GuardarArchivo(HttpPostedFileBase archivo)
        {
            var contador = _archivoService.GetAll().Where(c => c.vigente).ToList().Count() + 1;
            string fileName = archivo.FileName;
            string fileContentType = archivo.ContentType;
            byte[] fileBytes = new byte[archivo.ContentLength];
            var data = archivo.InputStream.Read(fileBytes, 0,
                Convert.ToInt32(archivo.ContentLength));

            Archivo n = new Archivo
            {
                Id = 0,
                codigo = "REXTEMP" + contador,
                nombre = fileName,
                vigente = true,
                fecha_registro = DateTime.Now,
                hash = fileBytes,
                tipo_contenido = fileContentType,
            };
            var archivoid = _archivoService.InsertAndGetId(n);
            return archivoid;
        }

        public int CrearReservaExtemporanea(ReservaHotelDto dto, HttpPostedFileBase archivo)
        {

            try
            {
                if (archivo != null)
                {
                    Guid myuuid = Guid.NewGuid();
                    string UUID = myuuid.ToString();

                    var contador = UUID;
                    //_archivoService.GetAll().Where(c => c.vigente).ToList().Count() + 1;
                    string fileName = archivo.FileName;
                    string fileContentType = archivo.ContentType;
                    byte[] fileBytes = new byte[archivo.ContentLength];
                    var data = archivo.InputStream.Read(fileBytes, 0,
                        Convert.ToInt32(archivo.ContentLength));

                    Archivo nueva = new Archivo()
                    {
                        Id = 0,
                        codigo = "REXTEMP" + contador,
                        nombre = fileName,
                        vigente = true,
                        fecha_registro = DateTime.Now,
                        hash = fileBytes,
                        tipo_contenido = fileContentType,
                    };

                    var archivoid = _archivoService.InsertAndGetId(nueva);
                    ReservaHotel nuevo = new ReservaHotel()
                    {
                        Id = 0,
                        EspacioHabitacionId = dto.EspacioHabitacionId,
                        ColaboradorId = dto.ColaboradorId,
                        fecha_registro = DateTime.Now,
                        fecha_desde = dto.fecha_desde,
                        fecha_hasta = dto.fecha_hasta,
                        estado = ReservaEstado.Activo,
                        extemporaneo = true,
                        DocumentoId = archivoid
                    };
                    var ReservaHotelId = Repository.InsertAndGetId(nuevo);


                    return ReservaHotelId;
                }


            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                return -1;
            }

            return -1;

        }

        public string UpdateServicioHospedajeColaborador(int id)
        {
            var ServicioHospedaje = _catalogoRepository.GetAll().Where(c => c.codigo == CatalogosCodigos.SERVICIO_HOSPEDAJE).FirstOrDefault();
            if (ServicioHospedaje != null && ServicioHospedaje.Id > 0)
            {
                var tieneservicioactivo = _colabordorServicioRepository.GetAll().Where(c => c.vigente).Where(c => c.ColaboradoresId == id).FirstOrDefault();
                if (tieneservicioactivo != null && tieneservicioactivo.Id > 0)
                {
                    tieneservicioactivo.vigente = false;
                    var tieneservicio = _colabordorServicioRepository.Update(tieneservicioactivo);
                    return "UPDATE";
                }
                else
                {
                    ColaboradorServicio cs = new ColaboradorServicio()
                    {
                        Id = 0,
                        ServicioId = ServicioHospedaje.Id,
                        ColaboradoresId = id,
                        vigente = true
                    };
                    var serviciohospedaje = _colabordorServicioRepository.InsertAndGetId(cs);
                    return "INSERT";
                }
            }
            else
            {
                return "SNCATALOGO";
            }

        }

        public async Task<bool> SendMessageAsync(int id, string tipo)
        {
            var reserva = Repository.GetAllIncluding(c => c.Colaborador.Contacto, c => c.EspacioHabitacion.Habitacion.Proveedor.contacto
                                                      , c => c.EspacioHabitacion.Habitacion.TipoHabitacion)
                                 .Where(c => c.Id == id)
                                 .FirstOrDefault();
            if (reserva != null && reserva.Id > 0)
            {
                var proveedor = reserva.EspacioHabitacion.Habitacion.Proveedor;
                /* ES: Envio de Archi*/
                MailMessage message = new MailMessage();
                message.Subject = "PMDIS:" + tipo.ToUpper() + " RESERVA HOTEL " + proveedor.razon_social + " " + reserva.fecha_desde.ToShortDateString() + "-" + reserva.fecha_hasta.ToShortDateString();
                var body = "SE CONFIRMA LA " + tipo + " DE LA RESERVA CON LA SIGUIENTE INFORMACIÓN. <br/>" +
                            "PROVEEDOR: " + proveedor.razon_social.ToUpper() + "<br/>" +
                            "COLABORADOR: " + reserva.Colaborador.numero_identificacion + " " + reserva.Colaborador.nombres_apellidos + "<br/>" +
                            "HABITACIÓN: " + reserva.EspacioHabitacion.Habitacion.numero_habitacion + " " + reserva.EspacioHabitacion.Habitacion.TipoHabitacion.nombre + "<br/>" +
                            "DESDE: " + reserva.fecha_desde.ToShortDateString() + "<br/>" +
                            "HASTA: " + reserva.fecha_hasta.ToShortDateString();

                message.Body = body;
                message.IsBodyHtml = true;
                if (proveedor.contacto != null && proveedor.contacto_id > 0)
                {
                    message.To.Add(proveedor.contacto.correo_electronico);
                    ElmahExtension.LogToElmah(new Exception("Send Rerva to : " + proveedor.contacto.correo_electronico));

                }

                if (reserva.Colaborador.Contacto != null && reserva.Colaborador.ContactoId > 0)
                {
                    if (reserva.Colaborador.Contacto.correo_electronico != null)
                    {
                        message.To.Add(reserva.Colaborador.Contacto.correo_electronico);
                        ElmahExtension.LogToElmah(new Exception("Send Rerva to : " + reserva.Colaborador.Contacto.correo_electronico));
                    }


                }


                try
                {
                    await _correoservice.SendWithFilesAsync(message);

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }




        #endregion

        public string DiasJornadaCampo()
        {
            var dias = _parametrorepository.GetAll().Where(c => c.Codigo == "NUM.DIAS.CAMPO").Select(c => c.Valor).FirstOrDefault();
            if (dias != null)
            {
                return dias;
            }
            else
            {
                return "0";
            }
        }

        public bool Activar(string pass)
        {

            var pass_param = _parametrorepository.GetAll().Where(c => c.Codigo == "ACTIVAR.FINALIZAR.CONSUMO").Select(c => c.Valor).FirstOrDefault();

            if (pass_param != null && pass_param == pass)
            {


                return true;

            }
            else
            {
                return false;
            }
        }

        public string Iniciar_FinalizarConsumo(int id, bool inicio, DateTime fecha, string justificacion)
        {
            var reserva = Repository.Get(id);

            if (inicio)
            {
                reserva.inicio_consumo = true;
                reserva.fecha_inicio_consumo = fecha;
                reserva.justificacion_inicio_manual = justificacion;
                Repository.Update(reserva);
            }
            else
            {



                var query_detalles = _detalleReservaRepository.GetAll().Where(c => c.ReservaHotelId == reserva.Id).ToList();

                var detalles = (from d in query_detalles
                                where d.fecha_reserva.HasValue
                                where d.fecha_reserva.Value.Date <= fecha.Date
                                select d).ToList();


                //Detalles Anteriores
                foreach (var registro in detalles)
                {
                    var d = _detalleReservaRepository.Get(registro.Id);
                    d.consumido = true;
                    d.fecha_consumo = d.fecha_reserva;
                    _detalleReservaRepository.Update(d);

                }

                //DetallesSuperiores
                var detallesSuperioresFecha = (from d in query_detalles
                                               where d.fecha_reserva.HasValue
                                               where d.fecha_reserva.Value.Date > fecha.Date
                                               select d).ToList();

                foreach (var registro in detallesSuperioresFecha)
                {
                    var d = _detalleReservaRepository.Get(registro.Id);
                    //d.consumido = true;

                    // d.fecha_consumo = d.fecha_reserva;                 
                    _detalleReservaRepository.Delete(d);

                }

                reserva.consumo_finalizado = true;
                reserva.justificacion_finalizacion_manual = justificacion;
                reserva.fecha_fin_consumo = fecha;
                Repository.Update(reserva);


            }

            return "OK";

        }


        public string EditarFecha(int id, bool inicio, DateTime fecha, string justificacion)
        {
            var reserva = Repository.Get(id);

            if (inicio)
            {
                reserva.inicio_consumo = true;
                reserva.fecha_inicio_consumo = fecha;
                reserva.justificacion_inicio_manual = justificacion;
                Repository.Update(reserva);
            }
            else
            {



                var query_detalles = _detalleReservaRepository.GetAll().Where(c => c.ReservaHotelId == reserva.Id).ToList();

                var detalles = (from d in query_detalles
                                where d.fecha_reserva.HasValue
                                where d.fecha_reserva.Value.Date <= fecha.Date
                                select d).ToList();

                foreach (var registro in detalles)
                {
                    var d = _detalleReservaRepository.Get(registro.Id);
                    d.consumido = true;
                    d.fecha_consumo = d.fecha_reserva;
                    _detalleReservaRepository.Update(d);

                }

                reserva.consumo_finalizado = true;
                reserva.justificacion_finalizacion_manual = justificacion;
                Repository.Update(reserva);


            }

            return "OK";

        }

    }
}
