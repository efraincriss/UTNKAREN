using Abp.Application.Services.Dto;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Models;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;

using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ProveedorAsyncBaseCrudAppService : AsyncBaseCrudAppService<dominio.Proveedor.Proveedor, ProveedorDto, PagedAndFilteredResultRequestDto>,
        IProveedorAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<ServicioProveedor> _servicioProveedorRespRepository;
        private readonly IBaseRepository<ConsumoVianda> _consumoVianda;
        private readonly IBaseRepository<Consumo> _consumo;
        private readonly IBaseRepository<DobleConsumo> _dobleconsumo;
        private readonly IBaseRepository<Catalogo> _catalogo;
        private readonly IBaseRepository<SolicitudVianda> _solicitudVianda;
        private readonly IBaseRepository<ContratoProveedor> _contratoproveedorrepository;
        private readonly IBaseRepository<TarifaHotel> _tarifahotelrepository;
        private readonly IBaseRepository<DetalleReserva> _detalleReservarepository;
        private readonly IBaseRepository<ReservaHotel> _reservarepository;
   
        private readonly IBaseRepository<TarifaLavanderia> _tarifaLavanderiarepository;
        private readonly IBaseRepository<Usuario> _usuarioRepository;
        private readonly IBaseRepository<RegistroSincronizacion> _registroSincronizaciones;
        private readonly IBaseRepository<ZonaProveedor> _zonaProveedor;
        private readonly IBaseRepository<Zona> _zona;
        private readonly IBaseRepository<Colaboradores> _colaboradoresRepository;
        private readonly IBaseRepository<ColaboradoresVisita> _colaboradoresVisitaRepository;
        public INovedadProveedorAsyncBaseCrudAppService NovedadProveedorService { get; }
        public IContactoAsyncBaseCrudAppService ContactoService { get; }
        public IArchivoAsyncBaseCrudAppService ArchivoService { get; }

        private readonly IBaseRepository<ParametroSistema> _parametrorepository;
        private readonly IBaseRepository<Contacto> _contactorepository;
        private readonly IBaseRepository<TipoOpcionComida> _tipoOpcionComida;

        public ProveedorAsyncBaseCrudAppService(
                IBaseRepository<dominio.Proveedor.Proveedor> repository,
                INovedadProveedorAsyncBaseCrudAppService novedadProveedorService,
                IContactoAsyncBaseCrudAppService contactoService,
                IArchivoAsyncBaseCrudAppService archivoService,
                IBaseRepository<ServicioProveedor> servicioProveedorRespRepository,
                IBaseRepository<ParametroSistema> parametroRepository,
                IBaseRepository<Contacto> contactorepository,
                IBaseRepository<Consumo> consumo,
                IBaseRepository<SolicitudVianda> solicitudVianda,
                IBaseRepository<ContratoProveedor> contratoproveedorrepository,
                IBaseRepository<ConsumoVianda> consumoVianda,
                IBaseRepository<TipoOpcionComida> tipoOpcionComida,
                IBaseRepository<Catalogo> catalogo,
                IBaseRepository<DetalleReserva> detalleReservarepository,
                IBaseRepository<TarifaHotel> tarifahotelrepository,
                IBaseRepository<Usuario> usuarioRepository,
                IBaseRepository<RegistroSincronizacion> registroSincronizaciones,
                IBaseRepository<ReservaHotel> reservarepository,
                IBaseRepository<DobleConsumo> dobleconsumo,
                IBaseRepository<ZonaProveedor> zonaProveedor,
                IBaseRepository<TarifaLavanderia> tarifaLavanderiarepository,
                 IBaseRepository<Zona> zona,
                 IBaseRepository<Colaboradores> colaboradoresRepository,
                  IBaseRepository<ColaboradoresVisita> colaboradoresVisitaRepository

            ) : base(repository)
        {
            _servicioProveedorRespRepository = servicioProveedorRespRepository;
            NovedadProveedorService = novedadProveedorService;
            ContactoService = contactoService;
            ArchivoService = archivoService;
            _parametrorepository = parametroRepository;
            _contactorepository = contactorepository;
            _consumoVianda = consumoVianda;
            _solicitudVianda = solicitudVianda;
            _contratoproveedorrepository = contratoproveedorrepository;
            _tipoOpcionComida = tipoOpcionComida;
            _consumo = consumo;
            _catalogo = catalogo;
            _detalleReservarepository = detalleReservarepository;
            _tarifahotelrepository = tarifahotelrepository;
            _usuarioRepository = usuarioRepository;
            _registroSincronizaciones = registroSincronizaciones;
            _reservarepository = reservarepository;
            _dobleconsumo = dobleconsumo;
            _zonaProveedor = zonaProveedor;
            _tarifaLavanderiarepository = tarifaLavanderiarepository;
            _zona = zona;
            _colaboradoresRepository = colaboradoresRepository;
            _colaboradoresVisitaRepository = colaboradoresVisitaRepository;

        }

        public override async Task<ProveedorDto> Get(EntityDto<int> input)
        {
            var query = Repository.GetAllIncluding(c => c.contacto);
            var entity = await (from item in query
                                where item.Id == input.Id
                                select item).SingleOrDefaultAsync();

            return MapToEntityDto(entity);
        }

        public async Task<ProveedorDetalleDto> GetDetalle(int id)
        {
            var query = Repository.GetAll()
                    .Include(c => c.contacto)
                    .Include(c => c.contratos)
                    .Include(c => c.servicios.Select(s => s.Servicio))
                    .Include(c => c.novedades)
                    .Include(c => c.zonas.Select(z => z.Zona))
                    //.Include(c => c.Zonas.Select(z => z.Zona.nombre))
                    .Include(c => c.requisitos)
                    .Where(i => i.Id == id);

            var entity = await query.SingleOrDefaultAsync();

            var dto = Mapper.Map<dominio.Proveedor.Proveedor, ProveedorDetalleDto>(entity);

            var tieneServicio = false;
            var tieneServicioLavanderia = false;
            foreach (var s in dto.servicios)
            {
                if (s.servicio_codigo == CatalogosCodigos.SERVICIO_HOSPEDAJE)
                    tieneServicio = true;

                if (s.servicio_codigo == CatalogosCodigos.SERVICIO_LAVANDERIA)
                    tieneServicioLavanderia = true;
            }

            dto.tiene_servicio_hospedaje = tieneServicio;
            dto.tiene_servicio_lavanderia = tieneServicioLavanderia;
            dto.tipo_identificacion_nombre = dto.tipo_identificacion.ToString();
            return dto;
        }

        /// <summary>
        ///  Obtener un proveedor simplificado (Info)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProveedorInfoDto> GetInfo(int id)
        {
            var item = await Repository.GetAll()
                .Where(c => c.Id == id)
                .Select(c => new ProveedorInfoDto()
                {
                    Id = c.Id,
                    identificacion = c.identificacion,
                    codigo_sap = c.codigo_sap,
                    estado_nombre = c.estado.ToString(),
                    razon_social = c.razon_social,
                    tipo_identificacion = c.tipo_identificacion,
                    tipo_identificacion_nombre = c.tipo_identificacion.ToString(),
                    tipo_proveedor_nombre = c.tipo_proveedor.nombre
                }).SingleOrDefaultAsync();

            return item;
        }

        public async override Task<ProveedorDto> Create(ProveedorDto input)
        {

            //Rules
            //CheckDuplicate
            var CheckDuplicate = await Repository.LongCountAsync(item =>
                                item.identificacion == input.identificacion);

            if (CheckDuplicate > 0)
            {
                var msg = string.Format("Ya existe un proveedor con el identificador {0}", input.identificacion);
                throw new GenericException(msg, msg);
            }

            //1. Crear Contacto.
            var contactoDto = Mapper.Map<ProveedorDto, ContactoDto>(input);

            var contactoResult = await ContactoService.Create(contactoDto);


            //Crear Archivo si existe
            if (input.documentacion_subida != null)
            {

                var archivoResult = await ArchivoService.Create(input.documentacion_subida);
                input.documentacion_id = archivoResult.Id;
            }

            //2. Crear Proveedor
            input.contacto_id = contactoResult.Id;
            return await base.Create(input);

        }

        public async override Task<ProveedorDto> Update(ProveedorDto input)
        {
            //Rules
            //CheckDuplicate
            var CheckDuplicate = await Repository.LongCountAsync(item =>
                                item.identificacion == input.identificacion
                                && item.Id != input.Id);

            if (CheckDuplicate > 0)
            {
                var msg = string.Format("Ya existe un proveedor con el identificador {0}", input.identificacion);
                throw new GenericException(msg, msg);
            }

            //Update Contacto.
            var contactoDto = Mapper.Map<ProveedorDto, ContactoDto>(input);
            contactoDto.Id = input.contacto_id;
            var contactoResult = await ContactoService.Update(contactoDto);

            //Crear Archivo si existe
            if (input.documentacion_subida != null)
            {

                var archivoResult = await ArchivoService.Create(input.documentacion_subida);
                input.documentacion_id = archivoResult.Id;
            }

            return await base.Update(input);
        }

        /// <summary>
        /// Activar o Desactivar proveedor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="opcionActivar">True: Activar. False: Desactivar</param>
        /// <returns></returns>
        public async Task<bool> Activar(int id, bool opcionActivar, string pass)
        {
            var pass_param = _parametrorepository.GetAll().Where(c => c.Codigo == "ACT.BAJA.PROVEEDOR").Select(c => c.Valor).FirstOrDefault();

            if (pass_param == pass)
            {

                var query = Repository.GetAll();
                var entity = await query.Where(p => p.Id == id).SingleOrDefaultAsync();

                if (entity == null)
                {
                    var msg = string.Format("No existe proveedor con Id : {0}", id);
                    throw new GenericException(msg, msg);
                }

                //1. Registrar Novedades
                var novedadProveedorDto = new NovedadProveedorDto();
                novedadProveedorDto.ProveedorId = entity.Id;
                novedadProveedorDto.fecha_registro = DateTime.Now;
                if (opcionActivar)
                    novedadProveedorDto.descripcion = "Proveedor activo";
                else
                    novedadProveedorDto.descripcion = "Proveedor dado de baja";
                novedadProveedorDto.resuelta = NovedadResuelto.Pendiente;

                var resulNovedad = await NovedadProveedorService.Create(novedadProveedorDto);

                //2. Actualizar Estado. 
                if (opcionActivar)
                    entity.estado = dominio.Proveedor.ProveedorEstado.Activo;
                else
                    entity.estado = dominio.Proveedor.ProveedorEstado.Inactivo;

                var result = await Repository.UpdateAsync(entity);

                return true;

            }
            else
            {
                return false;
            }
        }


        #region Proveedor / Hospedaje

        public List<ProveedorDto> ListProveedorHospedaje()
        {
            var query = _servicioProveedorRespRepository.GetAll()
                .Include(o => o.Proveedor)
                .Where(o => o.Servicio.codigo == CatalogosCodigos.SERVICIO_HOSPEDAJE).ToList();

            var proveedores = (from sp in query
                               select sp.Proveedor).ToList();

            var proveedoresDto = Mapper.Map<List<dominio.Proveedor.Proveedor>, List<ProveedorDto>>(proveedores);

            var count = 1;
            foreach (var proveedorDto in proveedoresDto)
            {
                proveedorDto.secuencial = count;
                count++;
            }
            return proveedoresDto;

        }

        #endregion

        #region Proveedor / Transporte

        public List<ProveedorDto> ListProveedorTransporte()
        {
            var query = _servicioProveedorRespRepository.GetAll()
                .Include(o => o.Proveedor)
                .Where(o => o.estado == ServicioEstado.Activo)
                .Where(o => o.Proveedor.estado == dominio.Proveedor.ProveedorEstado.Activo)
                .Where(o => o.Servicio.codigo == CatalogosCodigos.SERVICIO_TRANSPORTE).ToList();

            var proveedores = (from sp in query
                               select sp.Proveedor).ToList();

            var proveedoresDto = Mapper.Map<List<dominio.Proveedor.Proveedor>, List<ProveedorDto>>(proveedores);

            return proveedoresDto;

        }

        public List<ProveedorDto> ListProveedorLiquidacionServicios()
        {
            var proveedores = Repository.GetAll().ToList();
            var proveedoresDto = Mapper.Map<List<dominio.Proveedor.Proveedor>, List<ProveedorDto>>(proveedores);

            return proveedoresDto;

        }


        #endregion


        public int EditarProveedor(ProveedorDto p)
        {
            var proveedor = Repository.Get(p.Id);
            proveedor.tipo_identificacion = p.tipo_identificacion;
            proveedor.identificacion = p.identificacion;
            proveedor.razon_social = p.razon_social;
            proveedor.codigo_sap = p.codigo_sap;
            proveedor.usuario = p.usuario;
            proveedor.contacto_id = p.contacto_id;
            proveedor.coordenadas = p.coordenadas;
            proveedor.tipo_proveedor_id = p.tipo_proveedor_id;
            proveedor.documentacion_id = p.documentacion_id;
            proveedor.estado = (dominio.Proveedor.ProveedorEstado)Enum.Parse(typeof(dominio.Proveedor.ProveedorEstado), p.estado_nombre);
            proveedor.es_externo = p.es_externo;
            var update = Repository.Update(proveedor);

            var contacto = _contactorepository.Get(p.contacto_id);
            contacto.ParroquiaId = p.ParroquiaId;
            contacto.correo_electronico = p.correo_electronico;
            contacto.telefono_convencional = p.telefono_convencional;
            contacto.celular = p.celular;
            var up = _contactorepository.Update(contacto);
            return proveedor.Id;
        }

        public bool ExisteProveedor(string NumeroIdentificacion)
        {
            var e = Repository.GetAll().Where(c => c.identificacion == NumeroIdentificacion).Where(c => c.estado == dominio.Proveedor.ProveedorEstado.Activo).FirstOrDefault();

            if (e != null && e.Id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool ValidarEmailUnico(string email)
        {
            var proveedor = Repository.GetAll().Where(c => c.contacto.correo_electronico == email).FirstOrDefault();
            if (proveedor != null && proveedor.Id > 0)
            {
                return true;
            }

            return false;
        }

        public bool ValidarEmailUnicoEdit(string email, int Id)
        {
            var proveedor = Repository.GetAll().Where(c => c.contacto.correo_electronico == email).Where(c => c.Id != Id).FirstOrDefault();
            if (proveedor != null && proveedor.Id > 0)
            {
                return true;
            }

            return false;
        }

        public List<ProveedorDto> ListProveedorAlimentacion()
        {
            var query = _servicioProveedorRespRepository.GetAll()
                .Include(o => o.Proveedor)
                .Where(o => o.estado == ServicioEstado.Activo)
                .Where(o => o.Proveedor.estado == dominio.Proveedor.ProveedorEstado.Activo || o.Proveedor.estado == dominio.Proveedor.ProveedorEstado.Inactivo)
                .Where(o => o.Servicio.codigo == CatalogosCodigos.SERVICIO_ALMUERZO).ToList();

            var proveedores = (from sp in query
                               select sp.Proveedor).ToList();

            var proveedoresDto = Mapper.Map<List<dominio.Proveedor.Proveedor>, List<ProveedorDto>>(proveedores);

            return proveedoresDto;
        }

        public ExcelPackage ReporteDiarioVianda(int proveedorId, DateTime fecha)
        {
            var proveedor = Repository.GetAll().Where(c => c.Id == proveedorId).FirstOrDefault();
            var query_consumos = _consumoVianda.GetAllIncluding(c => c.colaborador, c => c.SolicitudVianda)

                                                             .ToList();

            var ContratoProveedorId = this.ObtenerContratoProveedor(proveedorId, fecha);
            var TiposOpcionesComida = _tipoOpcionComida.GetAll().Where(c => c.ContratoId == ContratoProveedorId)
                                                             .ToList();

            var data = (from d in query_consumos
                        where d.fecha_consumo_vianda.Date == fecha.Date
                        select new DatosViandas
                        {
                            SolicitudViandaId = d.SolicitudViandaId,
                            FechaSolicitud = d.SolicitudVianda.fecha_solicitud.Date,
                            FechaConsumo = d.fecha_consumo_vianda.Date,
                            NombreComida = "",
                            Apellidos = d.colaborador.primer_apellido + " " + d.colaborador.segundo_apellido,
                            Nombres = d.colaborador.nombres,
                            NumeroIdentificacion = d.colaborador.numero_identificacion,

                            tipoComidaId = d.SolicitudVianda.tipo_comida_id,
                            TotalConsumido = d.SolicitudVianda.consumido,
                            TotalPedido = d.SolicitudVianda.total_pedido

                        }).OrderBy(c => c.FechaSolicitud).ToList();

            foreach (var d in data)
            {

                var val = (from v in TiposOpcionesComida
                           where v.tipo_comida_id == d.tipoComidaId
                           where v.ContratoId == ContratoProveedorId
                           select v).FirstOrDefault();


                d.PU = val != null && val.Id > 0 ? val.costo : 0;

            }
            var solicitudesId = (from s in data select s.SolicitudViandaId).Distinct().ToList();

            decimal valor = 0;

            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaReporteVianda.xlsx");
            if (File.Exists((string)filename))
            {


                FileInfo newFile = new FileInfo(filename);

                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("Reporte Diario", pck.Workbook.Worksheets[1]);

            }
            ExcelWorksheet h = excel.Workbook.Worksheets[1];
            string cell = "C3";
            h.Cells[cell].Value = proveedor.identificacion;
            cell = "C4";
            h.Cells[cell].Value = proveedor.razon_social;
            cell = "B2";
            h.Cells[cell].Value = DateTime.Now.Date.ToShortDateString();



            var count = 7;

            foreach (var s in solicitudesId)
            {
                var solicitud = _solicitudVianda.GetAll().Where(c => c.Id == s).FirstOrDefault();
                cell = "B" + count + ":F" + count;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;

                cell = "B" + count;
                h.Cells[cell].Value = "Fecha Solicitud: ";
                cell = "C" + count;
                h.Cells[cell].Value = solicitud.fecha_solicitud.ToShortDateString();
                h.Cells[cell].Style.Font.Bold = true;
                count++;

                cell = "B" + count + ":F" + count;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "B" + count;
                h.Cells[cell].Value = "Fecha Consumo";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "C" + count;
                h.Cells[cell].Value = "Identificacion";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "D" + count;
                h.Cells[cell].Value = "Apellidos";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "E" + count;
                h.Cells[cell].Value = "Nombres";
                count++;
                var consumos_viandas = (from c in data where c.SolicitudViandaId == s select c).ToList();
                decimal tarifa = 0;
                foreach (var c in consumos_viandas)
                {


                    cell = "B" + count;
                    h.Cells[cell].Value = c.FechaConsumo.ToShortDateString();
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = c.NumeroIdentificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "D" + count;
                    h.Cells[cell].Value = c.Apellidos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "E" + count;
                    h.Cells[cell].Value = c.Nombres;
                    tarifa = c.PU;
                    count++;

                }
                cell = "E" + count;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Value = "Tipo Comida";
                h.Cells[cell].Style.Font.Bold = true;
                cell = "F" + count;

                h.Cells[cell].Value = solicitud.tipo_comida.nombre;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                count++;

                cell = "E" + count;
                h.Cells[cell].Value = "Tarifa";
                h.Cells[cell].Style.Font.Bold = true;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));

                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "F" + count;
                h.Cells[cell].Value = tarifa;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                count++;
                cell = "E" + count;
                h.Cells[cell].Value = "Cantidad Solicitada";
                h.Cells[cell].Style.Font.Bold = true;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "F" + count;
                h.Cells[cell].Value = solicitud.total_pedido;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                count++;
                cell = "E" + count;
                h.Cells[cell].Value = "Cantidad Consumida";
                h.Cells[cell].Style.Font.Bold = true;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "F" + count;
                h.Cells[cell].Value = solicitud.consumido;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                count++;
                cell = "E" + count;
                h.Cells[cell].Value = "Total";
                h.Cells[cell].Style.Font.Bold = true;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                cell = "F" + count;
                h.Cells[cell].Value = (from c in data where c.SolicitudViandaId == s select c.PU * c.TotalPedido).ToList().Sum();
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                count = count + 2;


            }

            var monto = (from m in data select m.TotalPedido * m.PU).ToList().Sum();
            cell = "C5";
            h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
            h.Cells[cell].Value = monto;

            return excel;
        }

        public ExcelPackage ReporteDiarioViandaMensula(int proveedorIdM, DateTime fechaInicio, DateTime fechaFin)
        {

            var proveedor = Repository.GetAll().Where(c => c.Id == proveedorIdM).FirstOrDefault();
            var query_consumos = _consumoVianda.GetAllIncluding(c => c.colaborador, c => c.SolicitudVianda)

                                                             .ToList();

            var ContratoProveedorId = this.ObtenerContratoProveedor(proveedorIdM, fechaInicio);
            var TiposOpcionesComida = _tipoOpcionComida.GetAll().Where(c => c.ContratoId == ContratoProveedorId)
                                                             .ToList();


            var data = (from d in query_consumos
                        where d.fecha_consumo_vianda.Date >= fechaInicio.Date
                        where d.fecha_consumo_vianda.Date <= fechaFin.Date
                        select new DatosViandas
                        {
                            SolicitudViandaId = d.SolicitudViandaId,
                            FechaSolicitud = d.SolicitudVianda.fecha_solicitud.Date,
                            FechaConsumo = d.fecha_consumo_vianda.Date,
                            NombreComida = "",
                            Apellidos = d.colaborador.primer_apellido + " " + d.colaborador.segundo_apellido,
                            Nombres = d.colaborador.nombres,
                            NumeroIdentificacion = d.colaborador.numero_identificacion,

                            tipoComidaId = d.SolicitudVianda.tipo_comida_id,
                            TotalConsumido = d.SolicitudVianda.consumido,
                            TotalPedido = d.SolicitudVianda.total_pedido

                        }).OrderBy(c => c.FechaSolicitud).ToList();

            foreach (var d in data)
            {

                var val = (from v in TiposOpcionesComida
                           where v.tipo_comida_id == d.tipoComidaId
                           where v.ContratoId == ContratoProveedorId
                           select v).FirstOrDefault();


                d.PU = val != null && val.Id > 0 ? val.costo : 0;

            }
            var solicitudesId = (from s in data select s.SolicitudViandaId).Distinct().ToList();

            decimal valor = 0;

            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaReporteVianda.xlsx");
            if (File.Exists((string)filename))
            {


                FileInfo newFile = new FileInfo(filename);

                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("Reporte Mensual", pck.Workbook.Worksheets[1]);

            }
            ExcelWorksheet h = excel.Workbook.Worksheets[1];
            string cell = "C3";
            h.Cells[cell].Value = proveedor.identificacion;
            cell = "C4";
            h.Cells[cell].Value = proveedor.razon_social;
            cell = "B2";
            h.Cells[cell].Value = DateTime.Now.Date.ToShortDateString();



            var count = 7;

            foreach (var s in solicitudesId)
            {
                var solicitud = _solicitudVianda.GetAll().Where(c => c.Id == s).FirstOrDefault();
                cell = "B" + count + ":F" + count;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;

                cell = "B" + count;
                h.Cells[cell].Value = "Fecha Solicitud: ";
                cell = "C" + count;
                h.Cells[cell].Value = solicitud.fecha_solicitud.ToShortDateString();
                h.Cells[cell].Style.Font.Bold = true;
                count++;

                cell = "B" + count + ":F" + count;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "B" + count;
                h.Cells[cell].Value = "Fecha Consumo";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "C" + count;
                h.Cells[cell].Value = "Identificacion";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "D" + count;
                h.Cells[cell].Value = "Apellidos";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "E" + count;
                h.Cells[cell].Value = "Nombres";
                count++;
                var consumos_viandas = (from c in data where c.SolicitudViandaId == s select c).ToList();
                decimal tarifa = 0;
                foreach (var c in consumos_viandas)
                {


                    cell = "B" + count;
                    h.Cells[cell].Value = c.FechaConsumo.ToShortDateString();
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = c.NumeroIdentificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "D" + count;
                    h.Cells[cell].Value = c.Apellidos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "E" + count;
                    h.Cells[cell].Value = c.Nombres;
                    tarifa = c.PU;
                    count++;

                }
                cell = "E" + count;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Value = "Tipo Comida";
                h.Cells[cell].Style.Font.Bold = true;
                cell = "F" + count;

                h.Cells[cell].Value = solicitud.tipo_comida.nombre;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                count++;

                cell = "E" + count;
                h.Cells[cell].Value = "Tarifa";
                h.Cells[cell].Style.Font.Bold = true;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));

                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "F" + count;
                h.Cells[cell].Value = tarifa;
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                count++;
                cell = "E" + count;
                h.Cells[cell].Value = "Cantidad Solicitada";
                h.Cells[cell].Style.Font.Bold = true;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "F" + count;
                h.Cells[cell].Value = solicitud.total_pedido;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                count++;
                cell = "E" + count;
                h.Cells[cell].Value = "Cantidad Consumida";
                h.Cells[cell].Style.Font.Bold = true;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "F" + count;
                h.Cells[cell].Value = solicitud.consumido;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                count++;
                cell = "E" + count;
                h.Cells[cell].Value = "Total";
                h.Cells[cell].Style.Font.Bold = true;
                h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(214, 220, 228));
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                cell = "F" + count;
                h.Cells[cell].Value = (from c in data where c.SolicitudViandaId == s select c.PU * c.TotalPedido).ToList().Sum();
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                count = count + 2;


            }

            var monto = (from m in data select m.TotalPedido * m.PU).ToList().Sum();
            cell = "C5";
            h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
            h.Cells[cell].Value = monto;

            return excel;
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

        public bool tieneServicioLavanderiaProveedor(int ProveedorId)
        {

            var servicio = _servicioProveedorRespRepository.GetAllIncluding(c => c.Servicio).Where(c => c.estado == ServicioEstado.Activo)
                                                         .Where(c => c.ProveedorId == ProveedorId)
                                                         .Where(c => c.Servicio.codigo == CatalogosCodigos.SERVICIO_LAVANDERIA)
                                                         .FirstOrDefault();
            return servicio != null ? true : false;

        }

        public string ProvedorEmailUnico(string email)
        {

            var proveedor = Repository.GetAll().Where(c => c.contacto.correo_electronico == email).FirstOrDefault();

            if (proveedor != null && proveedor.Id > 0)
            {
                return "El Proveedor: " + proveedor.razon_social + " tiene el mismo correo electrónico registrado";
            }

            return "NO_ENCONTRADO";
        }

        public string ProvedorEmailUnicoEdit(string email, int Id)
        {

            var proveedor = Repository.GetAll().Where(c => c.contacto.correo_electronico == email).Where(c => c.Id != Id).FirstOrDefault();

            if (proveedor != null && proveedor.Id > 0)
            {
                return "El Proveedor: " + proveedor.razon_social + " tiene el mismo correo electrónico registrado";
            }

            return "NO_ENCONTRADO";
        }

        public bool ValidarDuplicadosConsumo()
        {
            /*select ProveedorId, colaborador_id, Tipo_Comida_Id, fecha, count(*) from SCH_SERVICIOS.consumos
                 group by ProveedorId, colaborador_id, Tipo_Comida_Id, fecha
            HAVING  count(*) > 1*/
            return false;
        }

        public bool ActualizarCamposNuevos()
        {
           int hasta= 1000;

            var reservas = _reservarepository.GetAllIncluding(r => r.EspacioHabitacion.Habitacion.TipoHabitacion)
                                             .Where(r => !r.TipoHabitacionId.HasValue)
                                             .OrderByDescending(r => r.fecha_hasta)
                                          .ToList();

            int i = 1;
            foreach (var r in reservas)
            {
                if (i <= hasta)
                {
                    var entity = _reservarepository.Get(r.Id);
                    entity.NombreTipoHabitacion = r.EspacioHabitacion.Habitacion.TipoHabitacion.nombre;
                    entity.TipoHabitacionId = r.EspacioHabitacion.Habitacion.TipoHabitacionId;
                    entity.CodigoEspacio = r.EspacioHabitacion.codigo_espacio;
                    entity.NumeroHabitacion = r.EspacioHabitacion.Habitacion.numero_habitacion;
                    decimal Costo = Convert.ToDecimal(0);
                    var ContratoProveedorId = this.ObtenerContratoProveedor(r.EspacioHabitacion.Habitacion.ProveedorId, r.fecha_desde);

                    if (ContratoProveedorId > 0)
                    {
                        Costo = this.ObtenerTarifaHotel(ContratoProveedorId, r.EspacioHabitacion.Habitacion.TipoHabitacionId);
                    }
                    entity.Costo = Costo;
                    
                }
                else {
                    return true;
                }

                i++;
            }


      
            return true;
        }

        public String ObtenerNombreOrigenConsumo(int? origen_consumo) {

            string origen = "Global";
            if (!origen_consumo.HasValue) {

                return origen;
            }
            if (origen_consumo.HasValue) {

                if (origen_consumo == 1)
                {
                    origen = "Cédula";

                }
                if (origen_consumo == 2)
                {
                    origen = "Qr";
                }
                if (origen_consumo ==3)
                {
                    origen = "Huella digital";
                }
            }
            
            return origen;
        }

        public ExcelPackage ReporteDiarioConsumo(List<int> Ids, DateTime fecha)
        {
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet h = null;
            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaReporteCConsumo.xlsx");

            foreach (var proveedorId in Ids)
            {

                var proveedor = Repository.GetAll().Where(c => c.Id == proveedorId).FirstOrDefault();

                if (File.Exists((string)filename))
                {
                    FileInfo newFile = new FileInfo(filename);
                    ExcelPackage pck = new ExcelPackage(newFile);
                    h = excel.Workbook.Worksheets.Add("RD_" + proveedor.razon_social, pck.Workbook.Worksheets[2]);

                }

                var query_consumos = _consumo.GetAllIncluding(c => c.colaborador).Where(c => c.ProveedorId == proveedorId).ToList();
                var ContratoProveedorId = this.ObtenerContratoProveedor(proveedorId, fecha);
                var TiposOpcionesComida = _tipoOpcionComida.GetAll().Where(c => c.ContratoId == ContratoProveedorId)
                                                                 .ToList();
                var result = new List<ReportConsumo>();
                var data = new List<ReportConsumo>();
                data = (from d in query_consumos
                        where d.fecha.Date == fecha.Date

                        select new ReportConsumo
                        {
                            //Id = d.Id,
                            fechaConsumo = d.fecha,
                            formatfechaConsumo = d.fecha.ToString("dd/MM/yyyy HH:mm:ss"),
                            ProveedorId = d.ProveedorId,
                            Identificacion = d.colaborador.numero_identificacion,
                            NombresCopletos = d.colaborador.nombres_apellidos,
                            TipoComidaId = d.Tipo_Comida_Id,
                            OpcionComidaId = d.Opcion_Comida_Id,
                            IdentificadorMovil = d.identificador,
                            identificador = d.identificador,
                            nombreOrigen = this.ObtenerNombreOrigenConsumo(d.origen_consumo)


                        }).OrderBy(c => c.fechaConsumo)
                             .OrderBy(c => c.nombretipoComida)
                              .OrderBy(c => c.NombresCopletos)
                           .ToList();

                foreach (var d in data)
                {
                    var duplicado = (from r in result
                                     where r.fechaConsumo == d.fechaConsumo
                                     where r.formatfechaConsumo == d.formatfechaConsumo
                                     where r.ProveedorId == d.ProveedorId
                                     where r.Identificacion == d.Identificacion
                                     where r.NombresCopletos == d.NombresCopletos
                                     where r.TipoComidaId == d.TipoComidaId
                                     where r.OpcionComidaId == d.OpcionComidaId
                                     select r).FirstOrDefault();
                    if (duplicado != null)
                    {
                        continue;
                    }
                    else
                    {

                        var val = (from v in TiposOpcionesComida
                                   where v.tipo_comida_id == d.TipoComidaId
                                   where v.opcion_comida_id == d.OpcionComidaId
                                   where v.ContratoId == ContratoProveedorId
                                   select v).FirstOrDefault();


                        d.precio = val != null && val.Id > 0 ? val.costo : 0;

                        var TipoComida = _catalogo.GetAll().Where(c => c.Id == d.TipoComidaId).FirstOrDefault();
                        var opcionComida = _catalogo.GetAll().Where(c => c.Id == d.OpcionComidaId).FirstOrDefault();

                        if (TipoComida != null)
                        {
                            d.nombretipoComida = TipoComida.nombre;
                        }
                        if (opcionComida != null)
                        {
                            d.nombreopcionComida = opcionComida.nombre;
                        }
                        result.Add(d);
                    }
                }

                decimal valor = 0;




                string cell = "C3";
                h.Cells[cell].Value = proveedor.identificacion;
                cell = "C4";
                h.Cells[cell].Value = proveedor.razon_social;
                cell = "C6";
                h.Cells[cell].Value = fecha.ToShortDateString();

                decimal monto = Convert.ToDecimal(0);
                var count = 7;

                cell = "B" + count;
                h.Cells[cell].Value = "Fecha";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "C" + count;
                h.Cells[cell].Value = "Tipo";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "D" + count;
                h.Cells[cell].Value = "Opción";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "E" + count;
                h.Cells[cell].Value = "Identificación";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "F" + count;
                h.Cells[cell].Value = "Nombres";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;

                cell = "G" + count;
                h.Cells[cell].Value = "Identificador";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "H" + count;
                h.Cells[cell].Value = "Consumo por";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;

                count++;
                foreach (var c in result)
                {


                    cell = "B" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("dd/MM/yyyy HH:mm:ss");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = c.nombretipoComida;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "D" + count;
                    h.Cells[cell].Value = c.nombreopcionComida;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "E" + count;
                    h.Cells[cell].Value = c.Identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "F" + count;
                    h.Cells[cell].Value = c.NombresCopletos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    cell = "G" + count;
                    h.Cells[cell].Value = c.IdentificadorMovil;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "H" + count;
                    h.Cells[cell].Value = c.nombreOrigen;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    monto = monto + c.precio;

                    count++;



                }
                cell = "C5";
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Value = monto;

                h.Cells[7, 2, h.Dimension.End.Row, 8].AutoFilter = true;
            }


            /*PESTAÑAS NUEVAS CONSUMOS DUPLICADOS E INTENTOS DE DOBLES CONSUMOS*/
            var controlAlimentacion = this.ReporteDuplicados(Ids, fecha, fecha);//Solo una fecha para consumos diarios
            if (controlAlimentacion != null)
            {

                excel.Workbook.Worksheets.Add("CONSUMOS DUPLICADOS", controlAlimentacion.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("INTENTO DOBLES CONSUMOS", controlAlimentacion.Workbook.Worksheets[2]);
            }
            return excel;
        }


        public ExcelPackage ReporteDiarioConsumoMensual(List<int> Ids, DateTime fechaInicio, DateTime fechaFin)
        {

            ExcelPackage excel = new ExcelPackage();
            /* ExcelWorksheet h = null;
             int principalcount = 7;*/

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaReporteCConsumo.xlsx");

            /**if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                h = excel.Workbook.Worksheets.Add("RH_RESUMEN", pck.Workbook.Worksheets[1]);

            }
            decimal valor = 0;
    */
            foreach (var proveedorIdM in Ids)
            {
                ExcelWorksheet h = null;


                var proveedor = Repository.GetAll().Where(c => c.Id == proveedorIdM).FirstOrDefault();
                var query_consumos = _consumo.GetAllIncluding(c => c.colaborador)
                    .Where(c => c.ProveedorId == proveedorIdM).ToList();

                var ContratoProveedorId = this.ObtenerContratoProveedor(proveedorIdM, fechaInicio);
                var TiposOpcionesComida = _tipoOpcionComida.GetAll().Where(c => c.ContratoId == ContratoProveedorId)
                                                                 .ToList();
                var result = new List<ReportConsumo>();
                var data = (from d in query_consumos
                            where d.fecha.Date >= fechaInicio.Date
                            where d.fecha.Date <= fechaFin.Date
                            select new ReportConsumo
                            {
                                //Id = d.Id,
                                fechaConsumo = d.fecha,
                                formatfechaConsumo = d.fecha.ToString("dd/MM/yyyy HH:mm:ss"),
                                ProveedorId = d.ProveedorId,
                                Identificacion = d.colaborador.numero_identificacion,
                                NombresCopletos = d.colaborador.nombres_apellidos,
                                TipoComidaId = d.Tipo_Comida_Id,
                                OpcionComidaId = d.Opcion_Comida_Id,
                                IdentificadorMovil = d.identificador,
                                identificador = d.identificador,
                                nombreOrigen=this.ObtenerNombreOrigenConsumo(d.origen_consumo)

                            }).OrderBy(c => c.fechaConsumo)
                             .OrderBy(c => c.nombretipoComida)
                              .OrderBy(c => c.NombresCopletos)
                           .ToList();



                foreach (var d in data)
                {
                    var duplicado = (from r in result
                                     where r.fechaConsumo == d.fechaConsumo
                                     where r.formatfechaConsumo == d.formatfechaConsumo
                                     where r.ProveedorId == d.ProveedorId
                                     where r.Identificacion == d.Identificacion
                                     where r.NombresCopletos == d.NombresCopletos
                                     where r.TipoComidaId == d.TipoComidaId
                                     where r.OpcionComidaId == d.OpcionComidaId
                                     select r).FirstOrDefault();
                    if (duplicado != null)
                    {
                        continue;
                    }
                    else
                    {



                        var val = (from v in TiposOpcionesComida
                                   where v.tipo_comida_id == d.TipoComidaId
                                   where v.opcion_comida_id == d.OpcionComidaId
                                   where v.ContratoId == ContratoProveedorId
                                   select v).FirstOrDefault();


                        d.precio = val != null && val.Id > 0 ? val.costo : 0;

                        var TipoComida = _catalogo.GetAll().Where(c => c.Id == d.TipoComidaId).FirstOrDefault();
                        var opcionComida = _catalogo.GetAll().Where(c => c.Id == d.OpcionComidaId).FirstOrDefault();

                        if (TipoComida != null)
                        {
                            d.nombretipoComida = TipoComida.nombre;
                        }
                        if (opcionComida != null)
                        {
                            d.nombreopcionComida = opcionComida.nombre;
                        }
                        result.Add(d);

                    }
                }

                decimal valor = 0;

                if (File.Exists((string)filename))
                {


                    FileInfo newFile = new FileInfo(filename);

                    ExcelPackage pck = new ExcelPackage(newFile);
                    h = excel.Workbook.Worksheets.Add("RM_" + proveedor.razon_social, pck.Workbook.Worksheets[2]);

                }

                string cell = "C3";
                h.Cells[cell].Value = proveedor.identificacion;
                cell = "C4";
                h.Cells[cell].Value = proveedor.razon_social;
                cell = "C6";
                h.Cells[cell].Value = fechaInicio.ToShortDateString() + " - " + fechaFin.ToShortDateString();

                decimal monto = Convert.ToDecimal(0);




                var count = 7;

                cell = "B" + count;
                h.Cells[cell].Value = "Fecha";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "C" + count;
                h.Cells[cell].Value = "Tipo";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "D" + count;
                h.Cells[cell].Value = "Opción";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "E" + count;
                h.Cells[cell].Value = "Identificación";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "F" + count;
                h.Cells[cell].Value = "Nombres";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;

                cell = "G" + count;
                h.Cells[cell].Value = "Identificador";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "H" + count;
                h.Cells[cell].Value = "Consumo por";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;


                count++;



                foreach (var c in result)
                {


                    cell = "B" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("dd/MM/yyyy HH:mm:ss");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = c.nombretipoComida;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "D" + count;
                    h.Cells[cell].Value = c.nombreopcionComida;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "E" + count;
                    h.Cells[cell].Value = c.Identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "F" + count;
                    h.Cells[cell].Value = c.NombresCopletos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "G" + count;
                    h.Cells[cell].Value = c.IdentificadorMovil;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "H" + count;
                    h.Cells[cell].Value = c.nombreOrigen;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    /* cell = "G" + count;
                     h.Cells[cell].Value = c.precio;
                     h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);*/
                    monto = monto + c.precio;

                    count++;



                }
                cell = "C5";
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Value = monto;
                h.Cells[7, 2, h.Dimension.End.Row, 8].AutoFilter = true;
            }

            /*PESTAÑAS NUEVAS CONSUMOS DUPLICADOS E INTENTOS DE DOBLES CONSUMOS*/
            var controlAlimentacion = this.ReporteDuplicados(Ids, fechaInicio, fechaFin);//Rangos fechas Consumo Mensual
            if (controlAlimentacion != null)
            {

                excel.Workbook.Worksheets.Add("CONSUMOS DUPLICADOS", controlAlimentacion.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("INTENTO DOBLES CONSUMOS", controlAlimentacion.Workbook.Worksheets[2]);
            }
            return excel;

        }



        public ExcelPackage ConsumoMensualConsolidado(List<int> Ids, DateTime fechaInicio, DateTime fechaFin, List<int> ZonaId)
        {

            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet h = null;

            string cell = "C4";


            var count = 7;

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/ReporteConsolidadoConsumo.xlsx");

            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                h = excel.Workbook.Worksheets.Add("RCONSOLIDADO", pck.Workbook.Worksheets[1]);
                h.Cells[cell].Value = fechaInicio.ToShortDateString() + " - " + fechaFin.ToShortDateString();
            }
            decimal valor = 0;

            foreach (var proveedorIdM in Ids)
            {

                var proveedor = Repository.GetAll().Where(c => c.Id == proveedorIdM).FirstOrDefault();
                var zonaProveedor = _zonaProveedor.GetAllIncluding(z => z.Zona).Where(c => c.ProveedorId == proveedorIdM).ToList();
                if (ZonaId.Count > 0)
                {
                    zonaProveedor.Where(z => ZonaId.Contains(z.ZonaId)).ToList();
                }
                var zonaNombre = zonaProveedor.Count == 0 ? "":String.Join(",", zonaProveedor.Select(y=>y.Zona.nombre).ToList());
                var fechaInicioParam = fechaInicio.Date;
                var fechaFinParam = fechaFin.Date;
    

                var query_consumos = _consumo.GetAllIncluding(c => c.colaborador)
                    .Where(c => c.ProveedorId == proveedorIdM).
                    Where(c => DbFunctions.TruncateTime(c.fecha)>=fechaInicioParam).Where(c => DbFunctions.TruncateTime(c.fecha)<=fechaFinParam).
                    ToList();

                var ContratoProveedorId = this.ObtenerContratoProveedor(proveedorIdM, fechaInicio);
                var TiposOpcionesComida = _tipoOpcionComida.GetAll().Where(c => c.ContratoId == ContratoProveedorId)
                                                                 .ToList();
                var result = new List<ReportConsumo>();
                var data = (from d in query_consumos
                            where d.fecha.Date >= fechaInicio.Date
                            where d.fecha.Date <= fechaFin.Date
                            select new ReportConsumo
                            {
                                //Id = d.Id,
                                fechaConsumo = d.fecha,
                                formatfechaConsumo = d.fecha.ToString("dd/MM/yyyy HH:mm:ss"),
                                ProveedorId = d.ProveedorId,
                                Identificacion = d.colaborador.numero_identificacion,
                                NombresCopletos = d.colaborador.nombres_apellidos,
                                TipoComidaId = d.Tipo_Comida_Id,
                                OpcionComidaId = d.Opcion_Comida_Id,
                                IdentificadorMovil = d.identificador,
                                nombreOrigen=this.ObtenerNombreOrigenConsumo(d.origen_consumo)

                            }).OrderBy(c => c.fechaConsumo)
                             .OrderBy(c => c.nombretipoComida)
                              .OrderBy(c => c.NombresCopletos)
                           .ToList();



                foreach (var d in data)
                {
                    var duplicado = (from r in result
                                     where r.fechaConsumo == d.fechaConsumo
                                     where r.formatfechaConsumo == d.formatfechaConsumo
                                     where r.ProveedorId == d.ProveedorId
                                     where r.Identificacion == d.Identificacion
                                     where r.NombresCopletos == d.NombresCopletos
                                     where r.TipoComidaId == d.TipoComidaId
                                     where r.OpcionComidaId == d.OpcionComidaId
                                     select r).FirstOrDefault();
                    if (duplicado != null)
                    {
                        continue;
                    }
                    else
                    {



                        var val = (from v in TiposOpcionesComida
                                   where v.tipo_comida_id == d.TipoComidaId
                                   where v.opcion_comida_id == d.OpcionComidaId
                                   where v.ContratoId == ContratoProveedorId
                                   select v).FirstOrDefault();


                        d.precio = val != null && val.Id > 0 ? val.costo : 0;

                        var TipoComida = _catalogo.GetAll().Where(c => c.Id == d.TipoComidaId).FirstOrDefault();
                        var opcionComida = _catalogo.GetAll().Where(c => c.Id == d.OpcionComidaId).FirstOrDefault();

                        if (TipoComida != null)
                        {
                            d.nombretipoComida = TipoComida.nombre;
                        }
                        if (opcionComida != null)
                        {
                            d.nombreopcionComida = opcionComida.nombre;
                        }
                        result.Add(d);

                    }
                }

       

                foreach (var c in result)
                {
                    cell = "B" + count;
                    h.Cells[cell].Value = proveedor.razon_social;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = proveedor.identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    cell = "D" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("dd/MM/yyyy HH:mm:ss");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "E" + count;
                    h.Cells[cell].Value = c.nombretipoComida;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "F" + count;
                    h.Cells[cell].Value = c.nombreopcionComida;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "G" + count;
                    h.Cells[cell].Value = c.Identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "H" + count;
                    h.Cells[cell].Value = c.NombresCopletos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "I" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("dd");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    cell = "J" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("MM");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "K" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("yyyy");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    cell = "L" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("HH:mm:ss");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "M" + count;
                    h.Cells[cell].Value = zonaNombre;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    cell = "N" + count;
                    h.Cells[cell].Value = c.IdentificadorMovil;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    cell = "O" + count;
                    h.Cells[cell].Value = c.nombreOrigen;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                    valor = valor + c.precio;
                    count++;



                }
                cell = "C3";
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Value = valor;
               /* h.Cells[7, 2, h.Dimension.End.Row, 6].AutoFilter = true;*/
            }

            /*PESTAÑAS NUEVAS CONSUMOS DUPLICADOS E INTENTOS DE DOBLES CONSUMOS*
            var controlAlimentacion = this.ReporteDuplicados(Ids, fechaInicio, fechaFin);//Rangos fechas Consumo Mensual
            if (controlAlimentacion != null)
            {

                excel.Workbook.Worksheets.Add("CONSUMOS DUPLICADOS", controlAlimentacion.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("INTENTO DOBLES CONSUMOS", controlAlimentacion.Workbook.Worksheets[2]);
            }
            */
            return excel;

        }







        public ExcelPackage ReporteDiarioHospedaje(List<int> Ids, DateTime fecha)
        {
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet h = null;
            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaReporteHospedaje.xlsx");
            foreach (var ProveedorId in Ids)
            {
                var proveedor = Repository.GetAll().Where(c => c.Id == ProveedorId).FirstOrDefault();
                if (File.Exists((string)filename))
                {
                    FileInfo newFile = new FileInfo(filename);
                    ExcelPackage pck = new ExcelPackage(newFile);
                    h = excel.Workbook.Worksheets.Add("RH_" + proveedor.razon_social, pck.Workbook.Worksheets[2]);

                }
                var tieneServicioLavanderia = this.tieneServicioLavanderiaProveedor(ProveedorId);

                var ContratoProveedorId = this.ObtenerContratoProveedor(ProveedorId, fecha);
                var detallesreserva = _detalleReservarepository
                                        .GetAllIncluding(c => c.ReservaHotel.EspacioHabitacion.Habitacion.Proveedor,
                                                        c => c.ReservaHotel.Colaborador,
                                                        c => c.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion
                                                        )
                                        .Where(c => c.ReservaHotel.EspacioHabitacion.Habitacion.ProveedorId == ProveedorId)
                                        .Where(c => c.fecha_reserva.HasValue)
                                        .Where(c => c.fecha_consumo.HasValue)
                                        .Where(c => c.consumido)
                                        //.Where(c => c.fecha_consumo == fecha)
                                        .ToList();

                var list = (from d in detallesreserva
                            where d.fecha_consumo.Value.Date == fecha.Date
                            select new ReportHospedaje()
                            {
                                Id = d.Id,
                                fechaConsumo = d.fecha_consumo.Value,
                                formatfechaConsumo = d.fecha_consumo.HasValue ? d.fecha_consumo.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                identificacionProveedor = proveedor.identificacion,
                                ProveedorId = proveedor.Id,
                                razon_social = proveedor.razon_social,
                                Identificacion = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.numero_identificacion : "",
                                NombresCopletos = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.nombres_apellidos : "",
                                TipoHabitacionId = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.Id,
                                /*
                                nombretipoHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.nombre,
                                NumeroHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.numero_habitacion,
                                EspacioHabitacion = d.ReservaHotel.EspacioHabitacion.codigo_espacio,*/
                                
                                //Nuevos Campos
                                nombretipoHabitacion = d.ReservaHotel.NombreTipoHabitacion,
                                NumeroHabitacion = d.ReservaHotel.NumeroHabitacion,
                                EspacioHabitacion = d.ReservaHotel.CodigoEspacio,
                                

                                tarifa =d.ReservaHotel.Costo,
                               
                                liquidado = d.liquidado,
                                formatliquidado = d.liquidado ? "SI" : "NO",
                                aplicaLavanderia = d.aplica_lavanderia,
                                aplicaLavanderiaString = d.aplica_lavanderia ? "SI" : "NO",
                                tarifaLavanderia = Convert.ToDecimal(0),
                                nombreOrigen = this.ObtenerNombreOrigenConsumo(Convert.ToInt32(d.origen_consumo_id))
                            }).OrderBy(c => c.fechaConsumo)
                             .OrderBy(c => c.nombretipoHabitacion)
                              .OrderBy(c => c.NombresCopletos)
                           .ToList();

                if (ContratoProveedorId > 0)
                {
                    foreach (var l in list)
                    {
                       // l.tarifa = this.ObtenerTarifaHotel(ContratoProveedorId, l.TipoHabitacionId);

                        l.tarifa = l.tarifa;
                        l.tarifaLavanderia = this.ObtenerTarifaLavanderia(ContratoProveedorId);
                    }
                }
                decimal valor = 0;

                string cell = "C3";
                h.Cells[cell].Value = proveedor.identificacion;
                cell = "C4";
                h.Cells[cell].Value = proveedor.razon_social;
                cell = "C6";
                h.Cells[cell].Value = fecha.ToShortDateString();

                decimal monto = Convert.ToDecimal(0);
                var count = 7;

                cell = "B" + count;
                h.Cells[cell].Value = "Fecha";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "C" + count;
                h.Cells[cell].Value = "Tipo Habitación";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "D" + count;
                h.Cells[cell].Value = "Número Habitación";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "E" + count;
                h.Cells[cell].Value = "Espacio Habitación";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;

                cell = "F" + count;
                h.Cells[cell].Value = "Identificación";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "G" + count;
                h.Cells[cell].Value = "Nombres";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                count++;
                foreach (var c in list)
                {


                    cell = "B" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("dd/MM/yyyy HH:mm:ss");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = c.nombretipoHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "D" + count;
                    h.Cells[cell].Value = c.NumeroHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    cell = "E" + count;
                    h.Cells[cell].Value = c.EspacioHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    cell = "F" + count;
                    h.Cells[cell].Value = c.Identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "G" + count;
                    h.Cells[cell].Value = c.NombresCopletos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    cell = "H" + count;
                    h.Cells[cell].Value = 1;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "I" + count;
                    h.Cells[cell].Value = c.tarifa;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;


                    cell = "J" + count;
                    h.Cells[cell].Value = c.aplicaLavanderia ? "SI" : "NO";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "K" + count;
                    h.Cells[cell].Value = c.tarifaLavanderia;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;


                    cell = "L" + count;
                    h.Cells[cell].Value = tieneServicioLavanderia && c.aplicaLavanderia ? (c.tarifa + c.tarifaLavanderia) : (c.tarifa * 1);
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    cell = "M" + count;
                    h.Cells[cell].Value = c.nombreOrigen; //c.liquidado ? "SI" : "NO";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;



                    monto = monto + (tieneServicioLavanderia && c.aplicaLavanderia ? (c.tarifa + c.tarifaLavanderia) : (c.tarifa * 1));

                    count++;



                }
                cell = "C5";
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Value = monto;

                h.Cells[7, 2, h.Dimension.End.Row, 6].AutoFilter = true;

                if (!tieneServicioLavanderia)
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        h.DeleteColumn(10);
                    }
                }
            }
            return excel;


        }



        public ExcelPackage ReporteHospedajeMensual(List<int> Ids, DateTime fechaInicio, DateTime fechaFin)
        {
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet h = null;
            int principalcount = 7;

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaReporteHospedaje.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                h = excel.Workbook.Worksheets.Add("RH_RESUMEN", pck.Workbook.Worksheets[1]);

            }
            decimal valor = 0;
            bool LavanderiaenConsolidado = false;
            foreach (var ProveedorId in Ids)
            {
                var proveedor = Repository.GetAll().Where(c => c.Id == ProveedorId).FirstOrDefault();

                var tieneServicioLavanderia = this.tieneServicioLavanderiaProveedor(ProveedorId);

                if (tieneServicioLavanderia)
                {
                    LavanderiaenConsolidado = true;
                }

                var ContratoProveedorId = this.ObtenerContratoProveedor(ProveedorId, fechaInicio);
                var detallesreserva = _detalleReservarepository
                                        .GetAllIncluding(c => c.ReservaHotel.EspacioHabitacion.Habitacion.Proveedor,
                                                        c => c.ReservaHotel.Colaborador,
                                                        c => c.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion
                                                        )
                                        .Where(c => c.ReservaHotel.EspacioHabitacion.Habitacion.ProveedorId == ProveedorId)
                                        .Where(c => c.fecha_reserva.HasValue)
                                        .Where(c => c.fecha_consumo.HasValue)
                                        .Where(c => c.consumido)
                                        .ToList();

                var list = (from d in detallesreserva
                            where d.fecha_consumo.Value.Date >= fechaInicio.Date
                            where d.fecha_consumo.Value.Date <= fechaFin.Date
                            select new ReportHospedaje()
                            {
                                Id = d.Id,
                                fechaConsumo = d.fecha_consumo.Value,
                                formatfechaConsumo = d.fecha_consumo.HasValue ? d.fecha_consumo.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                identificacionProveedor = proveedor.identificacion,
                                ProveedorId = proveedor.Id,
                                razon_social = proveedor.razon_social,
                                Identificacion = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.numero_identificacion : "",
                                NombresCopletos = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.nombres_apellidos : "",


                                TipoHabitacionId = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.Id,
                                /*nombretipoHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.nombre,
                                NumeroHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.numero_habitacion,
                                EspacioHabitacion = d.ReservaHotel.EspacioHabitacion.codigo_espacio,*/
                                tarifa = d.ReservaHotel.Costo,

                                nombretipoHabitacion = d.ReservaHotel.NombreTipoHabitacion,
                                NumeroHabitacion = d.ReservaHotel.NumeroHabitacion,
                                EspacioHabitacion = d.ReservaHotel.CodigoEspacio,

                                liquidado = d.liquidado,
                                formatliquidado = d.liquidado ? "SI" : "NO",
                                aplicaLavanderia = d.aplica_lavanderia,
                                aplicaLavanderiaString = d.aplica_lavanderia ? "SI" : "NO",
                                tarifaLavanderia = Convert.ToDecimal(0),
                                nombreOrigen = this.ObtenerNombreOrigenConsumo(Convert.ToInt32(d.origen_consumo_id))
                            }).ToList();

                if (ContratoProveedorId > 0)
                {
                    foreach (var l in list)
                    {
                        //l.tarifa = this.ObtenerTarifaHotel(ContratoProveedorId, l.TipoHabitacionId);

                        l.tarifa = l.tarifa;
                        l.tarifaLavanderia = this.ObtenerTarifaLavanderia(ContratoProveedorId);
                    }
                }

                string cell = "";
                var count = 7;
                foreach (var c in list)
                {
                    cell = "B" + count;
                    h.Cells[cell].Value = proveedor.razon_social;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = proveedor.identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "D" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("dd/MM/yyyy HH:mm:ss");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "E" + count;
                    h.Cells[cell].Value = c.nombretipoHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "F" + count;
                    h.Cells[cell].Value = c.NumeroHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "G" + count;
                    h.Cells[cell].Value = c.EspacioHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    cell = "H" + count;
                    h.Cells[cell].Value = c.Identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "I" + count;
                    h.Cells[cell].Value = c.NombresCopletos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "J" + count;
                    h.Cells[cell].Value = 1;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "K" + count;
                    h.Cells[cell].Value = c.tarifa;
                    //h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "L" + count;
                    h.Cells[cell].Value = c.aplicaLavanderia ? "SI" : "NO";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "M" + count;
                    h.Cells[cell].Value = c.tarifaLavanderia;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;


                    cell = "N" + count;
                    h.Cells[cell].Value = tieneServicioLavanderia && c.aplicaLavanderia ? (c.tarifa + c.tarifaLavanderia) : (c.tarifa * 1);
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    cell = "O" + count;
                    h.Cells[cell].Value =c.nombreOrigen; //c.liquidado ? "SI" : "NO";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


                    valor = valor + (tieneServicioLavanderia && c.aplicaLavanderia ? (c.tarifa + c.tarifaLavanderia) : (c.tarifa * 1));

                    count++;



                }

            }
            string cabecera = "C3";
            h.Cells[cabecera].Style.Numberformat.Format = "#,##0.00";
            h.Cells[cabecera].Value = valor;
            cabecera = "C4";
            h.Cells[cabecera].Value = fechaInicio.ToShortDateString() + " - " + fechaFin.ToShortDateString();
            h.Cells[6, 2, h.Dimension.End.Row, 12].AutoFilter = true;

            if (!LavanderiaenConsolidado)
            {
                for (int i = 1; i <= 2; i++)
                {
                    h.DeleteColumn(12);
                }
            }


            foreach (var ProveedorId in Ids)
            {
                var proveedor = Repository.GetAll().Where(c => c.Id == ProveedorId).FirstOrDefault();
                if (File.Exists((string)filename))
                {
                    FileInfo newFile = new FileInfo(filename);
                    ExcelPackage pck = new ExcelPackage(newFile);
                    h = excel.Workbook.Worksheets.Add("RH_" + proveedor.razon_social, pck.Workbook.Worksheets[2]);

                }
                valor = 0;
                var tieneServicioLavanderia = this.tieneServicioLavanderiaProveedor(ProveedorId);
                var ContratoProveedorId = this.ObtenerContratoProveedor(ProveedorId, fechaInicio);
                var detallesreserva = _detalleReservarepository
                                        .GetAllIncluding(c => c.ReservaHotel.EspacioHabitacion.Habitacion.Proveedor,
                                                        c => c.ReservaHotel.Colaborador,
                                                        c => c.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion
                                                        )
                                        .Where(c => c.ReservaHotel.EspacioHabitacion.Habitacion.ProveedorId == ProveedorId)
                                        .Where(c => c.fecha_reserva.HasValue)
                                        .Where(c => c.fecha_consumo.HasValue)
                                        .Where(c => c.consumido)
                                        //  .Where(c => c.fecha_consumo == fecha)
                                        .ToList();

                var list = (from d in detallesreserva
                            where d.fecha_consumo.Value.Date >= fechaInicio.Date
                            where d.fecha_consumo.Value.Date <= fechaFin.Date
                            select new ReportHospedaje()
                            {
                                Id = d.Id,
                                fechaConsumo = d.fecha_consumo.Value,
                                formatfechaConsumo = d.fecha_consumo.HasValue ? d.fecha_consumo.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                identificacionProveedor = proveedor.identificacion,
                                ProveedorId = proveedor.Id,
                                razon_social = proveedor.razon_social,
                                Identificacion = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.numero_identificacion : "",
                                NombresCopletos = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.nombres_apellidos : "",
                                TipoHabitacionId = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.Id,


                                /*nombretipoHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.nombre,
                                NumeroHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.numero_habitacion,
                                EspacioHabitacion = d.ReservaHotel.EspacioHabitacion.codigo_espacio,
                                tarifa = Convert.ToDecimal(0),*/

                                tarifa = d.ReservaHotel.Costo,

                                nombretipoHabitacion = d.ReservaHotel.NombreTipoHabitacion,
                                NumeroHabitacion = d.ReservaHotel.NumeroHabitacion,
                                EspacioHabitacion = d.ReservaHotel.CodigoEspacio,

                                liquidado = d.liquidado,
                                formatliquidado = d.liquidado ? "SI" : "NO",
                                aplicaLavanderia = d.aplica_lavanderia,
                                aplicaLavanderiaString = d.aplica_lavanderia ? "SI" : "NO",
                                tarifaLavanderia = Convert.ToDecimal(0),
                                nombreOrigen = this.ObtenerNombreOrigenConsumo(Convert.ToInt32(d.origen_consumo_id))
                            }).ToList();

                if (ContratoProveedorId > 0)
                {
                    foreach (var l in list)
                    {
                        //l.tarifa = this.ObtenerTarifaHotel(ContratoProveedorId, l.TipoHabitacionId);

                        l.tarifaLavanderia = this.ObtenerTarifaLavanderia(ContratoProveedorId);
                    }
                }


                string cell = "C3";
                h.Cells[cell].Value = proveedor.identificacion;
                cell = "C4";
                h.Cells[cell].Value = proveedor.razon_social;
                cell = "C6";
                h.Cells[cell].Value = fechaInicio.ToShortDateString() + " - " + fechaFin.ToShortDateString();
                decimal monto = Convert.ToDecimal(0);
                var count = 7;

                count++;
                foreach (var c in list)
                {


                    cell = "B" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("dd/MM/yyyy HH:mm:ss");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = c.nombretipoHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "D" + count;
                    h.Cells[cell].Value = c.NumeroHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "E" + count;
                    h.Cells[cell].Value = c.EspacioHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "F" + count;
                    h.Cells[cell].Value = c.Identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "G" + count;
                    h.Cells[cell].Value = c.NombresCopletos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    cell = "H" + count;
                    h.Cells[cell].Value = 1;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "I" + count;
                    h.Cells[cell].Value = c.tarifa;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    //h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "J" + count;
                    h.Cells[cell].Value = c.aplicaLavanderia ? "SI" : "NO";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    cell = "K" + count;
                    h.Cells[cell].Value = c.tarifaLavanderia;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;


                    cell = "L" + count;
                    h.Cells[cell].Value = tieneServicioLavanderia && c.aplicaLavanderia ? (c.tarifa + c.tarifaLavanderia) : (c.tarifa * 1);
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    cell = "M" + count;
                    h.Cells[cell].Value = c.nombreOrigen;// c.liquidado ? "SI" : "NO";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    h.Column(6).Width = 63;
                    monto = monto + (tieneServicioLavanderia && c.aplicaLavanderia ? (c.tarifa + c.tarifaLavanderia) : (c.tarifa * 1));

                    count++;



                }
                cell = "C5";
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Value = monto;


                h.Cells[7, 2, h.Dimension.End.Row, 10].AutoFilter = true;

                if (!tieneServicioLavanderia)
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        h.DeleteColumn(10);
                    }
                }
            }
            return excel;
        }


        public ExcelPackage HospedajeMensualConsolidado(List<int> Ids, DateTime fechaInicio, DateTime fechaFin, List<int> ZonaId)
        {
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet h = null;
            int principalcount = 7;

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/NHospedaje.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                h = excel.Workbook.Worksheets.Add("RH_RESUMEN", pck.Workbook.Worksheets[1]);

            }
            decimal valor = 0;
            bool LavanderiaenConsolidado = false;
            foreach (var ProveedorId in Ids)
            {
                var proveedor = Repository.GetAll().Where(c => c.Id == ProveedorId).FirstOrDefault();
                var zona = _zonaProveedor.GetAllIncluding(z => z.Zona).Where(z => z.ProveedorId == ProveedorId).ToList() ;
                if (ZonaId.Count > 0)
                {
                    zona.Where(z => ZonaId.Contains(z.ZonaId)).ToList();
                }
                var zonaNombre = zona.Count == 0 ? "" : String.Join(",", zona.Select(y=>y.Zona.nombre).ToList());
                var tieneServicioLavanderia = this.tieneServicioLavanderiaProveedor(ProveedorId);

                if (tieneServicioLavanderia)
                {
                    LavanderiaenConsolidado = true;
                }

                var ContratoProveedorId = this.ObtenerContratoProveedor(ProveedorId, fechaInicio);
                var detallesreserva = _detalleReservarepository
                                        .GetAllIncluding(c => c.ReservaHotel.EspacioHabitacion.Habitacion.Proveedor,
                                                        c => c.ReservaHotel.Colaborador,
                                                        c => c.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion
                                                        )
                                        .Where(c => c.ReservaHotel.EspacioHabitacion.Habitacion.ProveedorId == ProveedorId)
                                        .Where(c => c.fecha_reserva.HasValue)
                                        .Where(c => c.fecha_consumo.HasValue)
                                        .Where(c => c.consumido)
                                        .ToList();

                var list = (from d in detallesreserva
                            where d.fecha_consumo.Value.Date >= fechaInicio.Date
                            where d.fecha_consumo.Value.Date <= fechaFin.Date
                            select new ReportHospedaje()
                            {
                                Id = d.Id,
                                fechaConsumo = d.fecha_consumo.Value,
                                formatfechaConsumo = d.fecha_consumo.HasValue ? d.fecha_consumo.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                identificacionProveedor = proveedor.identificacion,
                                ProveedorId = proveedor.Id,
                                razon_social = proveedor.razon_social,
                                Identificacion = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.numero_identificacion : "",
                                NombresCopletos = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.nombres_apellidos : "",
                                TipoHabitacionId = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.Id,


                                /*nombretipoHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.nombre,
                                NumeroHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.numero_habitacion,
                                EspacioHabitacion = d.ReservaHotel.EspacioHabitacion.codigo_espacio,
                                tarifa = Convert.ToDecimal(0),*/

                                tarifa = d.ReservaHotel.Costo,

                                nombretipoHabitacion = d.ReservaHotel.NombreTipoHabitacion,
                                NumeroHabitacion = d.ReservaHotel.NumeroHabitacion,
                                EspacioHabitacion = d.ReservaHotel.CodigoEspacio,

                                liquidado = d.liquidado,
                                formatliquidado = d.liquidado ? "SI" : "NO",
                                aplicaLavanderia = d.aplica_lavanderia,
                                aplicaLavanderiaString = d.aplica_lavanderia ? "SI" : "NO",
                                tarifaLavanderia = Convert.ToDecimal(0),
                                nombreOrigen = this.ObtenerNombreOrigenConsumo(Convert.ToInt32(d.origen_consumo_id))

                            }).ToList();

                if (ContratoProveedorId > 0)
                {
                    foreach (var l in list)
                    {
                       // l.tarifa = this.ObtenerTarifaHotel(ContratoProveedorId, l.TipoHabitacionId);
                        l.tarifaLavanderia = this.ObtenerTarifaLavanderia(ContratoProveedorId);
                    }
                }

                string cell = "";
                var count = 7;
                foreach (var c in list)
                {
                    cell = "B" + count;
                    h.Cells[cell].Value = proveedor.razon_social;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = proveedor.identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "D" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("dd/MM/yyyy HH:mm:ss");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "E" + count;
                    h.Cells[cell].Value = c.nombretipoHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "F" + count;
                    h.Cells[cell].Value = c.NumeroHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "G" + count;
                    h.Cells[cell].Value = c.EspacioHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    cell = "H" + count;
                    h.Cells[cell].Value = c.Identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "I" + count;
                    h.Cells[cell].Value = c.NombresCopletos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "J" + count;
                    h.Cells[cell].Value = 1;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "K" + count;
                    h.Cells[cell].Value = c.tarifa;
                    //h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "L" + count;
                    h.Cells[cell].Value = c.aplicaLavanderia ? "SI" : "NO";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "M" + count;
                    h.Cells[cell].Value = c.tarifaLavanderia;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;


                    cell = "N" + count;
                    h.Cells[cell].Value = tieneServicioLavanderia && c.aplicaLavanderia ? (c.tarifa + c.tarifaLavanderia) : (c.tarifa * 1);
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    cell = "O" + count;
                    h.Cells[cell].Value = c.nombreOrigen;//;c.liquidado ? "SI" : "NO";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


                    cell = "P" + count;
                    h.Cells[cell].Value = zonaNombre;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;



                    valor = valor + (tieneServicioLavanderia && c.aplicaLavanderia ? (c.tarifa + c.tarifaLavanderia) : (c.tarifa * 1));

                    count++;



                }

            }
            string cabecera = "C3";
            h.Cells[cabecera].Style.Numberformat.Format = "#,##0.00";
            h.Cells[cabecera].Value = valor;
            cabecera = "C4";
            h.Cells[cabecera].Value = fechaInicio.ToShortDateString() + " - " + fechaFin.ToShortDateString();
            h.Cells[6, 2, h.Dimension.End.Row, 12].AutoFilter = true;

            if (!LavanderiaenConsolidado)
            {
                for (int i = 1; i <= 2; i++)
                {
                    h.DeleteColumn(12);
                }
            }


              return excel;
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



        public decimal ObtenerTarifaLavanderia(int ContratoProveedorId)
        {
            var tarifa = _tarifaLavanderiarepository.GetAll()
                                               .Where(c => c.ContratoProveedorId == ContratoProveedorId)
                                               .Where(c => c.estado)
                                               .FirstOrDefault();
            if (tarifa != null)
            {
                return tarifa.valor_servicio;
            }
            else
            {
                return 0;
            }
        }

        public ExcelPackage ReporteUsuarios()
        {
            ExcelPackage package = new ExcelPackage();
            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.Add("UsuarioMovil");
            ExcelWorksheet h = package.Workbook.Worksheets[1];
            h.Column(1).Width = 20;
            h.Column(2).Width = 35;
            h.Column(3).Width = 60;
            h.Column(4).Width = 35;

            int count = 1;
            string cell = "A" + count;
            h.Cells[cell].Value = "ZONA";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "B" + count;
            h.Cells[cell].Value = "USUARIO";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "C" + count;
            h.Cells[cell].Value = "NOMBRES";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "D" + count;
            h.Cells[cell].Value = "PIN";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "E" + count;
            h.Cells[cell].Value = "ROL";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            count++;

            var queryUsuarios = _usuarioRepository.GetAllIncluding(c => c.Roles).ToList();
            var prov = (from q in queryUsuarios
                            // where  q.Cuenta.ToUpper().StartsWith("PROV")

                        select q).ToList();
            cell = "";
            foreach (var i in prov)
            {
                var zonaString = "";

                if (i != null)
                {
                    var ZonasProveedor = _zonaProveedor.GetAll().Where(c => c.Proveedor.usuario == i.Cuenta).Select(c => c.Zona.nombre).ToList();


                    zonaString = String.Join(",", ZonasProveedor);
                }
                foreach (var item in i.Roles.Select(c => c.Codigo).Distinct())
                {
                    if (item == "PRO" || item == "CAP" || item == "TRA" || item == "ANO" || item == "CHO")
                    {

                        cell = "A" + count;
                        h.Cells[cell].Value = zonaString;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                        cell = "B" + count;
                        h.Cells[cell].Value = i.Correo;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                        cell = "C" + count;
                        h.Cells[cell].Value = i.NombresCompletos;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                        cell = "D" + count;
                        h.Cells[cell].Value = i.pin;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                        cell = "E" + count;
                        h.Cells[cell].Value = item;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                        count++;
                    }
                }
            }
            h.Cells[1, 1, h.Dimension.End.Row, h.Dimension.End.Column].AutoFilter = true;
            return package;
        }


        public List<Zona> GetZonas() {

            var queryIdsZona = _zonaProveedor.GetAll().Where(c => c.vigente).Select(c => c.ZonaId).ToList().Distinct().ToList();
            var zonas = _zona.GetAll().Where(c => queryIdsZona.Contains(c.Id)).ToList();
            return zonas;
               
        }
        public List<TipoOpcionComidaHorario> GetList(int ZonaId)
        {
            var tipoOpcionComidas = _tipoOpcionComida.GetAll().Select(c => c.tipo_comida_id).Distinct().ToList();
            var proveedorZonaId = _zonaProveedor.GetAll().Where(x => x.ZonaId == ZonaId).Select(c => c.ProveedorId).ToList().Distinct().ToList();
            var list = new List<TipoOpcionComidaHorario>();
            foreach (var tipoId in tipoOpcionComidas)
            {

                var e = _tipoOpcionComida.GetAllIncluding(c => c.tipo_comida,c=>c.contrato)
                                         .Where(c => c.tipo_comida_id == tipoId)
                                         .Where(c=>proveedorZonaId.Contains(c.contrato.ProveedorId))
                                         .FirstOrDefault();
                if (e != null)
                {
                    var data = new TipoOpcionComidaHorario();
                    data.tipoOpcionComidaId = e.tipo_comida_id;
                    data.nombreTipoOpcionComida = e.tipo_comida != null ? e.tipo_comida.nombre : "";
                    data.fechaHorarioInicio = e.hora_inicio;
                    data.horarioInicio = e.hora_inicio.TimeOfDay;
                    data.horarioFin = e.hora_fin.TimeOfDay;
                    data.formathorarioInicio = e.hora_inicio.TimeOfDay.ToString();
                    data.formathorarioFin = e.hora_fin.TimeOfDay.ToString();

                    list.Add(data);
                    this.ActualizarHorarios(e.tipo_comida_id, data.horarioInicio, data.horarioFin, ZonaId);
                }

            }
            return list;
        }

        public List<Catalogo> TipoOpcionComida()
        {
            var tipoOpcionComidas = _tipoOpcionComida.GetAll().Select(c => c.tipo_comida_id).Distinct().ToList();
            var catalogos = new List<Catalogo>();
            foreach (var tipoId in tipoOpcionComidas)
            {
                var e = _catalogo.GetAll().Where(c => c.Id == tipoId).FirstOrDefault();
                if (e != null)
                {
                    catalogos.Add(e);
                }
            }
            return catalogos;
        }

        public string ActualizarHorarios(int TipoComidaId, TimeSpan HoraInicio, TimeSpan HoraFin, int ZonaId)
        {
            var proveedorZonaId = _zonaProveedor.GetAll().Where(x => x.ZonaId == ZonaId).Select(c => c.ProveedorId).ToList().Distinct().ToList();


            var tipoOpcionComidas = _tipoOpcionComida.GetAll().Where(c=>proveedorZonaId.Contains(c.contrato.ProveedorId)).Where(c => c.tipo_comida_id == TipoComidaId).ToList();
             if (HoraFin < HoraInicio)
            {
                return "La Hora de Fin no puede ser menor a la Hora de Inicio";
            }

            foreach (var tipo in tipoOpcionComidas)
            {
                var e = _tipoOpcionComida.Get(tipo.Id);
                e.hora_inicio = DateTime.Now.Date.Add(HoraInicio);
                e.hora_fin = DateTime.Now.Date.Add(HoraFin);

                _tipoOpcionComida.Update(e);

            }
            return "OK";
        }

        public ExcelPackage AplicaCedulaColaboradores()
        {
            ExcelPackage package = new ExcelPackage();
            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.Add("AplicaValidacionCedula");
            ExcelWorksheet h = package.Workbook.Worksheets[1];

           h.Column(1).Width = 20;
            h.Column(2).Width = 20;
            h.Column(3).Width = 40;
            h.Column(4).Width = 20;

            int count = 1;
            string cell = "A" + count;
            h.Cells[cell].Value = "PROYECTO";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "B" + count;
            h.Cells[cell].Value = "IDENTIFICACIÓN";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "C" + count;
            h.Cells[cell].Value = "NOMBRES Y APELLIDOS";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "D" + count;
            h.Cells[cell].Value = "FECHA INGRESO";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "E" + count;
            h.Cells[cell].Value = "ESTADO";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "F" + count;
            h.Cells[cell].Value = "TIPO";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "G" + count;
            h.Cells[cell].Value = "EMPRESA";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "H" + count;
            h.Cells[cell].Value = "APLICA VALIDACIÓN CÉDULA";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);


            count++;

            var queryColaboradores = _colaboradoresRepository.GetAllIncluding(c=>c.Proyecto).Where(c => c.vigente)
                                                                     .Where(c => c.validacion_cedula)
                                                                     .ToList() ;
                                                                   

            foreach (var data in queryColaboradores)
            {

                if (data != null)
                {


                    cell = "A" + count;
                    h.Cells[cell].Value = data.ContratoId.HasValue?data.Proyecto.nombre:"";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                    cell = "B" + count;
                    h.Cells[cell].Value = data.numero_identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                    cell = "C" + count;
                    h.Cells[cell].Value = data.nombres_apellidos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                    cell = "D" + count;
                    h.Cells[cell].Value = data.fecha_ingreso.HasValue?data.fecha_ingreso.Value.ToString("dd/MM/yyyy"):data.CreationTime.ToString("dd/MM/yyyy");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                    cell = "E" + count;
                    h.Cells[cell].Value = data.estado;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                    cell = "F" + count;
                    h.Cells[cell].Value = data.es_externo.HasValue &&data.es_externo.Value==true ? "EXTERNO":"INTERNO" ;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                    cell = "G" + count;
                    string empresa= "";

                    if (data.es_externo.HasValue && data.es_externo.Value == true) {

                        var colaboradoresVisita = _colaboradoresVisitaRepository.GetAll().Where(c => c.vigente)
                                                                                         .Where(c => c.ColaboradoresId == data.Id)
                                                                                         .OrderByDescending(c => c.fecha_hasta)
                                                                                         .FirstOrDefault();
                        if (colaboradoresVisita != null) {
                            empresa = colaboradoresVisita.empresa;
                        }
                    }
                    else {
                        empresa = "CPP"; 
                    }

                        h.Cells[cell].Value = empresa;


                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);



                    cell = "H" + count;
                    h.Cells[cell].Value = data.validacion_cedula ? "SI":"NO" ;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                    count++;
                }



            }
            h.Cells[1, 1, h.Dimension.End.Row, h.Dimension.End.Column].AutoFilter = true;



            return package;
        }


        public ExcelPackage ReporteUsuariosSincronizacion()
        {
            ExcelPackage package = new ExcelPackage();
            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.Add("UsuarioMovil");
            ExcelWorksheet h = package.Workbook.Worksheets[1];

            h.Column(1).Width = 20;
            h.Column(2).Width = 35;
            h.Column(3).Width = 30;
            h.Column(4).Width = 30;

            int count = 1;
            string cell = "A" + count;
            h.Cells[cell].Value = "ZONA";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "B" + count;
            h.Cells[cell].Value = "USUARIO";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "C" + count;
            h.Cells[cell].Value = "IDENTIFICADOR";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "D" + count;
            h.Cells[cell].Value = "FECHA ULTIMA SINCRONIZACION";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);


            count++;

            var queryUsuariosSincronizacos = _registroSincronizaciones.GetAll().Select(c => c.Identificador).Distinct().ToList();

            foreach (var ra in queryUsuariosSincronizacos)
            {

                var data = _registroSincronizaciones.GetAll().Where(c => c.Identificador == ra).OrderByDescending(c => c.FechaSincronizacion).FirstOrDefault();

                if (data != null)
                {

                    var user = _usuarioRepository.GetAll().Where(c => c.Id == data.UsuarioId).FirstOrDefault();
                    var zonaString = "";

                    if (user != null) {
                    var ZonasProveedor = _zonaProveedor.GetAll().Where(c=>c.Proveedor.usuario==user.Cuenta).Select(c => c.Zona.nombre).ToList();


                    zonaString = String.Join(",", ZonasProveedor);
                    }

                    cell = "A" + count;
                    h.Cells[cell].Value = user != null ? zonaString:"";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                    cell = "B" + count;
                    h.Cells[cell].Value = user != null ? user.NombresCompletos : "";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                    cell = "C" + count;
                    h.Cells[cell].Value = ra;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                    cell = "D" + count;
                    h.Cells[cell].Value = data.FechaSincronizacion.ToString("dd/MM/yyyy HH:mm:ss");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                    count++;
                }



            }
            h.Cells[1, 1, h.Dimension.End.Row, h.Dimension.End.Column].AutoFilter = true;



            return package;
        }

        public ExcelPackage ReporteHospedajeSerge(List<int> Ids, DateTime fechaInicio, DateTime fechaFin)
        {
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet h = null;
            int principalcount = 7;

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaHospedajeSerge.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                h = excel.Workbook.Worksheets.Add("RH", pck.Workbook.Worksheets[1]);

            }
            decimal valor = 0;
            string cellperiodo = "C4";
            h.Cells[cellperiodo].Value = fechaInicio.ToString("dd/MM/yyyy") + " - " + fechaFin.ToString("dd/MM/yyyy");
            foreach (var ProveedorId in Ids)
            {
                var proveedor = Repository.GetAll().Where(c => c.Id == ProveedorId).FirstOrDefault();



                var ContratoProveedorId = this.ObtenerContratoProveedor(ProveedorId, fechaInicio);
                var detallesreserva = _detalleReservarepository.GetAll()
                                        .Where(c => c.ReservaHotel.EspacioHabitacion.Habitacion.ProveedorId == ProveedorId)
                                        .Where(c => c.fecha_reserva.HasValue)
                                        .Where(c => c.fecha_consumo.HasValue)
                                        .Where(c => c.consumido)
                                        //  .Where(c => c.fecha_consumo == fecha)
                                        .ToList();
                var details = (from d in detallesreserva
                               where d.fecha_consumo.Value.Date >= fechaInicio.Date
                               where d.fecha_consumo.Value.Date <= fechaFin.Date

                               select d)
                                     .ToList();

                var reservas = (from r in details select r.ReservaHotelId).Distinct().ToList();

                var datos = new List<ReportHospedajeSerge>();
                foreach (var Id in reservas)
                {

                    var reserva = _reservarepository.GetAllIncluding(c => c.EspacioHabitacion.Habitacion.Proveedor,
                                                        c => c.Colaborador,
                                                        c => c.EspacioHabitacion.Habitacion.TipoHabitacion
                                                        )
                                                        .Where(c => c.fecha_inicio_consumo.HasValue)
                                                        .Where(c => c.fecha_fin_consumo.HasValue)
                                                        .Where(c => c.Id == Id)
                                                        .FirstOrDefault();
                    if (reserva != null)
                    {
                        var detailsreserva = (from d in detallesreserva
                                              where d.fecha_consumo.Value.Date >= fechaInicio.Date
                                              where d.fecha_consumo.Value.Date <= fechaFin.Date
                                              where d.ReservaHotelId == reserva.Id
                                              select d)
                                       .ToList();
                        // var fechaConsumoMin = (from m in details select m.fecha_consumo).Min();
                        //var fechaConsumoMax = (from m in details select m.fecha_consumo).Max();

                        var data = new ReportHospedajeSerge()
                        {
                            Id = reserva.Id,
                            fechaInicioReserva = reserva.fecha_desde.ToString("dd/MM/yyyy"),
                            fechaFinReserva = reserva.fecha_hasta.ToString("dd/MM/yyyy"),
                            identificacionProveedor = proveedor.identificacion,
                            ProveedorId = proveedor.Id,
                            razon_social = proveedor.razon_social,
                            Identificacion = reserva.Colaborador != null ? reserva.Colaborador.numero_identificacion : "",
                            NombresCopletos = reserva.Colaborador != null ? reserva.Colaborador.nombres_apellidos : "",
                            TipoHabitacionId = reserva.EspacioHabitacion.Habitacion.TipoHabitacion.Id,

                           // nombretipoHabitacion = reserva.EspacioHabitacion.Habitacion.TipoHabitacion.nombre,
                         //   NumeroHabitacion = reserva.EspacioHabitacion.Habitacion.numero_habitacion,
                            nombretipoHabitacion = reserva.NombreTipoHabitacion,
                            NumeroHabitacion = reserva.NumeroHabitacion+ " - "+ reserva.CodigoEspacio,


                            fechaInicioConsumo = reserva.fecha_inicio_consumo.HasValue ? reserva.fecha_inicio_consumo.Value.ToString("dd/MM/yyyy HH:mm:ss") : "", //fechaConsumoMin != null && fechaConsumoMin.HasValue ? fechaConsumoMin.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                            fechaFinConsumo = reserva.fecha_fin_consumo.HasValue ? reserva.fecha_fin_consumo.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",//fechaConsumoMax != null && fechaConsumoMax.HasValue ? fechaConsumoMax.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                            diasConsumidos = detailsreserva.Count()
                        };
                        datos.Add(data);
                    }


                }



                string cell = "";
                var count = 7;
                foreach (var c in datos)
                {
                    cell = "B" + count;
                    h.Cells[cell].Value = proveedor.razon_social;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = proveedor.identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "D" + count;
                    h.Cells[cell].Value = c.fechaInicioReserva;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "E" + count;
                    h.Cells[cell].Value = c.fechaFinReserva;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "F" + count;
                    h.Cells[cell].Value = c.nombretipoHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "G" + count;
                    h.Cells[cell].Value = c.NumeroHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "H" + count;
                    h.Cells[cell].Value = c.Identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "I" + count;
                    h.Cells[cell].Value = c.NombresCopletos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "J" + count;
                    h.Cells[cell].Value = c.fechaInicioConsumo;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "K" + count;
                    h.Cells[cell].Value = c.fechaFinConsumo;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    cell = "L" + count;
                    h.Cells[cell].Value = c.diasConsumidos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                    count++;



                }

            }


            /*  foreach (var ProveedorId in Ids)
              {
                  var proveedor = Repository.GetAll().Where(c => c.Id == ProveedorId).FirstOrDefault();
                  if (File.Exists((string)filename))
                  {
                      FileInfo newFile = new FileInfo(filename);
                      ExcelPackage pck = new ExcelPackage(newFile);
                      h = excel.Workbook.Worksheets.Add("RH_" + proveedor.razon_social, pck.Workbook.Worksheets[2]);

                  }
                  valor = 0;

                  var ContratoProveedorId = this.ObtenerContratoProveedor(ProveedorId);
                  var detallesreserva = _detalleReservarepository
                                          .GetAllIncluding(c => c.ReservaHotel.EspacioHabitacion.Habitacion.Proveedor,
                                                          c => c.ReservaHotel.Colaborador,
                                                          c => c.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion
                                                          )
                                          .Where(c => c.ReservaHotel.EspacioHabitacion.Habitacion.ProveedorId == ProveedorId)
                                          .Where(c => c.fecha_reserva.HasValue)
                                          .Where(c => c.fecha_consumo.HasValue)
                                          .Where(c => c.consumido)
                                          //  .Where(c => c.fecha_consumo == fecha)
                                          .ToList();

                  var list = (from d in detallesreserva
                              where d.fecha_consumo.Value.Date >= fechaInicio.Date
                              where d.fecha_consumo.Value.Date <= fechaFin.Date
                              select new ReportHospedaje()
                              {
                                  Id = d.Id,
                                  fechaConsumo = d.fecha_consumo.Value,
                                  formatfechaConsumo = d.fecha_consumo.HasValue ? d.fecha_consumo.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                  identificacionProveedor = proveedor.identificacion,
                                  ProveedorId = proveedor.Id,
                                  razon_social = proveedor.razon_social,
                                  Identificacion = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.numero_identificacion : "",
                                  NombresCopletos = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.nombres_apellidos : "",
                                  TipoHabitacionId = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.Id,
                                  nombretipoHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.nombre,
                                  NumeroHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.numero_habitacion,
                                  tarifa = Convert.ToDecimal(0)
                              }).ToList();

                  if (ContratoProveedorId > 0)
                  {
                      foreach (var l in list)
                      {
                          l.tarifa = this.ObtenerTarifaHotel(ContratoProveedorId, l.TipoHabitacionId);
                      }
                  }


                  string cell = "C3";
                  h.Cells[cell].Value = proveedor.identificacion;
                  cell = "C4";
                  h.Cells[cell].Value = proveedor.razon_social;
                  cell = "C6";
                  h.Cells[cell].Value = fechaInicio.ToShortDateString() + " - " + fechaFin.ToShortDateString();
                  decimal monto = Convert.ToDecimal(0);
                  var count = 7;

                  count++;
                  foreach (var c in list)
                  {


                      cell = "B" + count;
                      h.Cells[cell].Value = c.fechaConsumo.ToString("dd/MM/yyyy HH:mm:ss");
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      cell = "C" + count;
                      h.Cells[cell].Value = c.nombretipoHabitacion;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      cell = "D" + count;
                      h.Cells[cell].Value = c.NumeroHabitacion;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      cell = "E" + count;
                      h.Cells[cell].Value = c.Identificacion;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      cell = "F" + count;
                      h.Cells[cell].Value = c.NombresCopletos;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      h.Cells[cell].Style.WrapText = true;
                      h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                      cell = "G" + count;
                      h.Cells[cell].Value = 1;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      h.Cells[cell].Style.WrapText = true;
                      h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                      cell = "H" + count;
                      h.Cells[cell].Value = c.tarifa;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      h.Cells[cell].Style.WrapText = true;
                      h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                      h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                      cell = "I" + count;
                      h.Cells[cell].Value = 1 * c.tarifa;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      h.Cells[cell].Style.WrapText = true;
                      h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                      h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                      cell = "J" + count;
                      h.Cells[cell].Value = c.formatliquidado;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      h.Cells[cell].Style.WrapText = true;
                      h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                      h.Column(6).Width = 63;
                      monto = monto + c.tarifa;

                      count++;



                  }
                  cell = "C5";
                  h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                  h.Cells[cell].Value = monto;

                  h.Cells[7, 2, h.Dimension.End.Row, 10].AutoFilter = true;
              }*/
            return excel;
        }


        public ExcelPackage ReporteHospedajeFinalizados(List<int> Ids, DateTime fechaInicio, DateTime fechaFin)
        {
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet h = null;
            int principalcount = 7;

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaHospedajeSerge.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                h = excel.Workbook.Worksheets.Add("RH", pck.Workbook.Worksheets[1]);

            }
            decimal valor = 0;
            string cellperiodo = "C4";
            h.Cells[cellperiodo].Value = fechaInicio.ToString("dd/MM/yyyy") + " - " + fechaFin.ToString("dd/MM/yyyy");
            foreach (var ProveedorId in Ids)
            {
                var proveedor = Repository.GetAll().Where(c => c.Id == ProveedorId).FirstOrDefault();



                var ContratoProveedorId = this.ObtenerContratoProveedor(ProveedorId, fechaInicio);
                var detallesreserva = _detalleReservarepository.GetAll()
                                        .Where(c => c.ReservaHotel.EspacioHabitacion.Habitacion.ProveedorId == ProveedorId)
                                        .Where(c => c.fecha_reserva.HasValue)
                                        .Where(c => c.ReservaHotel.inicio_consumo)
                                        .Where(c => c.ReservaHotel.consumo_finalizado == false)
                                        .Where(c => c.ReservaHotel.fecha_hasta < DateTime.Now)
                                        // .Where(c => c.fecha_consumo.HasValue)
                                        .Where(c => c.consumido)
                                        //  .Where(c => c.fecha_consumo == fecha)
                                        .ToList();
                var details = (from d in detallesreserva
                               where d.fecha_consumo.Value.Date >= fechaInicio.Date
                               where d.fecha_consumo.Value.Date <= fechaFin.Date

                               select d)
                                     .ToList();

                var reservas = (from r in details select r.ReservaHotelId).Distinct().ToList();

                var datos = new List<ReportHospedajeSerge>();
                foreach (var Id in reservas)
                {

                    var reserva = _reservarepository.GetAllIncluding(c => c.EspacioHabitacion.Habitacion.Proveedor,
                                                        c => c.Colaborador,
                                                        c => c.EspacioHabitacion.Habitacion.TipoHabitacion
                                                        )
                                                        .Where(c => c.Id == Id)
                                                        .FirstOrDefault();
                    if (reserva != null)
                    {
                        var detailsreserva = (from d in detallesreserva
                                              where d.fecha_consumo.Value.Date >= fechaInicio.Date
                                              where d.fecha_consumo.Value.Date <= fechaFin.Date
                                              where d.ReservaHotelId == reserva.Id
                                              select d)
                                       .ToList();
                        var fechaConsumoMin = (from m in details select m.fecha_consumo).Min();
                        //var fechaConsumoMax = null;// (from m in details select m.fecha_consumo).Max();

                        var data = new ReportHospedajeSerge()
                        {
                            Id = reserva.Id,
                            fechaInicioReserva = reserva.fecha_desde.ToString("dd/MM/yyyy"),
                            fechaFinReserva = reserva.fecha_hasta.ToString("dd/MM/yyyy"),
                            identificacionProveedor = proveedor.identificacion,
                            ProveedorId = proveedor.Id,
                            razon_social = proveedor.razon_social,
                            Identificacion = reserva.Colaborador != null ? reserva.Colaborador.numero_identificacion : "",
                            NombresCopletos = reserva.Colaborador != null ? reserva.Colaborador.nombres_apellidos : "",
                            TipoHabitacionId = reserva.EspacioHabitacion.Habitacion.TipoHabitacion.Id,


                           // nombretipoHabitacion = reserva.EspacioHabitacion.Habitacion.TipoHabitacion.nombre,
                          //  NumeroHabitacion = reserva.EspacioHabitacion.Habitacion.numero_habitacion,

                            nombretipoHabitacion = reserva.NombreTipoHabitacion,
                            NumeroHabitacion = reserva.NumeroHabitacion+" - "+ reserva.CodigoEspacio,

                            fechaInicioConsumo = reserva.fecha_inicio_consumo.Value.ToString("dd/MM/yyyy   HH:mm:ss"), //fechaConsumoMin != null && fechaConsumoMin.HasValue ? fechaConsumoMin.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                            //fechaFinConsumo = fechaConsumoMax != null && fechaConsumoMax.HasValue ? fechaConsumoMax.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                            diasConsumidos = detailsreserva.Count()
                        };
                        datos.Add(data);
                    }


                }



                string cell = "";
                var count = 7;
                foreach (var c in datos)
                {
                    cell = "B" + count;
                    h.Cells[cell].Value = proveedor.razon_social;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = proveedor.identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "D" + count;
                    h.Cells[cell].Value = c.fechaInicioReserva;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "E" + count;
                    h.Cells[cell].Value = c.fechaFinReserva;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "F" + count;
                    h.Cells[cell].Value = c.nombretipoHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "G" + count;
                    h.Cells[cell].Value = c.NumeroHabitacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "H" + count;
                    h.Cells[cell].Value = c.Identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cell = "I" + count;
                    h.Cells[cell].Value = c.NombresCopletos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    cell = "J" + count;
                    h.Cells[cell].Value = c.fechaInicioConsumo;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "K" + count;
                    h.Cells[cell].Value = c.fechaFinConsumo;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    cell = "L" + count;
                    h.Cells[cell].Value = c.diasConsumidos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                    count++;



                }

            }


            /*  foreach (var ProveedorId in Ids)
              {
                  var proveedor = Repository.GetAll().Where(c => c.Id == ProveedorId).FirstOrDefault();
                  if (File.Exists((string)filename))
                  {
                      FileInfo newFile = new FileInfo(filename);
                      ExcelPackage pck = new ExcelPackage(newFile);
                      h = excel.Workbook.Worksheets.Add("RH_" + proveedor.razon_social, pck.Workbook.Worksheets[2]);

                  }
                  valor = 0;

                  var ContratoProveedorId = this.ObtenerContratoProveedor(ProveedorId);
                  var detallesreserva = _detalleReservarepository
                                          .GetAllIncluding(c => c.ReservaHotel.EspacioHabitacion.Habitacion.Proveedor,
                                                          c => c.ReservaHotel.Colaborador,
                                                          c => c.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion
                                                          )
                                          .Where(c => c.ReservaHotel.EspacioHabitacion.Habitacion.ProveedorId == ProveedorId)
                                          .Where(c => c.fecha_reserva.HasValue)
                                          .Where(c => c.fecha_consumo.HasValue)
                                          .Where(c => c.consumido)
                                          //  .Where(c => c.fecha_consumo == fecha)
                                          .ToList();

                  var list = (from d in detallesreserva
                              where d.fecha_consumo.Value.Date >= fechaInicio.Date
                              where d.fecha_consumo.Value.Date <= fechaFin.Date
                              select new ReportHospedaje()
                              {
                                  Id = d.Id,
                                  fechaConsumo = d.fecha_consumo.Value,
                                  formatfechaConsumo = d.fecha_consumo.HasValue ? d.fecha_consumo.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                  identificacionProveedor = proveedor.identificacion,
                                  ProveedorId = proveedor.Id,
                                  razon_social = proveedor.razon_social,
                                  Identificacion = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.numero_identificacion : "",
                                  NombresCopletos = d.ReservaHotel.Colaborador != null ? d.ReservaHotel.Colaborador.nombres_apellidos : "",
                                  TipoHabitacionId = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.Id,
                                  nombretipoHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.nombre,
                                  NumeroHabitacion = d.ReservaHotel.EspacioHabitacion.Habitacion.numero_habitacion,
                                  tarifa = Convert.ToDecimal(0)
                              }).ToList();

                  if (ContratoProveedorId > 0)
                  {
                      foreach (var l in list)
                      {
                          l.tarifa = this.ObtenerTarifaHotel(ContratoProveedorId, l.TipoHabitacionId);
                      }
                  }


                  string cell = "C3";
                  h.Cells[cell].Value = proveedor.identificacion;
                  cell = "C4";
                  h.Cells[cell].Value = proveedor.razon_social;
                  cell = "C6";
                  h.Cells[cell].Value = fechaInicio.ToShortDateString() + " - " + fechaFin.ToShortDateString();
                  decimal monto = Convert.ToDecimal(0);
                  var count = 7;

                  count++;
                  foreach (var c in list)
                  {


                      cell = "B" + count;
                      h.Cells[cell].Value = c.fechaConsumo.ToString("dd/MM/yyyy HH:mm:ss");
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      cell = "C" + count;
                      h.Cells[cell].Value = c.nombretipoHabitacion;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      cell = "D" + count;
                      h.Cells[cell].Value = c.NumeroHabitacion;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      cell = "E" + count;
                      h.Cells[cell].Value = c.Identificacion;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      cell = "F" + count;
                      h.Cells[cell].Value = c.NombresCopletos;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      h.Cells[cell].Style.WrapText = true;
                      h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                      cell = "G" + count;
                      h.Cells[cell].Value = 1;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      h.Cells[cell].Style.WrapText = true;
                      h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                      cell = "H" + count;
                      h.Cells[cell].Value = c.tarifa;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      h.Cells[cell].Style.WrapText = true;
                      h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                      h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                      cell = "I" + count;
                      h.Cells[cell].Value = 1 * c.tarifa;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      h.Cells[cell].Style.WrapText = true;
                      h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                      h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                      cell = "J" + count;
                      h.Cells[cell].Value = c.formatliquidado;
                      h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                      h.Cells[cell].Style.WrapText = true;
                      h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                      h.Column(6).Width = 63;
                      monto = monto + c.tarifa;

                      count++;



                  }
                  cell = "C5";
                  h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                  h.Cells[cell].Value = monto;

                  h.Cells[7, 2, h.Dimension.End.Row, 10].AutoFilter = true;
              }*/
            return excel;
        }
        public ExcelPackage ReporteDiarioConsumoDuplicado(List<int> Ids, DateTime fechaInicio, DateTime fechaFin)
        {

            ExcelPackage excel = new ExcelPackage();
            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaReporteCConsumo.xlsx");



            foreach (var proveedorIdM in Ids)
            {
                ExcelWorksheet h = null;


                var proveedor = Repository.GetAll().Where(c => c.Id == proveedorIdM).FirstOrDefault();
                var query_consumos = _consumo.GetAllIncluding(c => c.colaborador)
                    .Where(c => c.ProveedorId == proveedorIdM).ToList();

                var ContratoProveedorId = this.ObtenerContratoProveedor(proveedorIdM, fechaInicio);
                var TiposOpcionesComida = _tipoOpcionComida.GetAll().Where(c => c.ContratoId == ContratoProveedorId)
                                                                 .ToList();
                var result = new List<ReportConsumo>();
                var duplicados = new List<ReportConsumo>();
                var data = (from d in query_consumos
                            where d.fecha.Date >= fechaInicio.Date
                            where d.fecha.Date <= fechaFin.Date
                            select new ReportConsumo
                            {
                                //Id = d.Id,
                                fechaConsumo = d.fecha,
                                formatfechaConsumo = d.fecha.ToString("dd/MM/yyyy HH:mm:ss"),
                                ProveedorId = d.ProveedorId,
                                Identificacion = d.colaborador.numero_identificacion,
                                NombresCopletos = d.colaborador.nombres_apellidos,
                                TipoComidaId = d.Tipo_Comida_Id,
                                OpcionComidaId = d.Opcion_Comida_Id,
                                IdentificadorMovil = d.identificador

                            }).OrderBy(c => c.fechaConsumo)
                             .OrderBy(c => c.nombretipoComida)
                              .OrderBy(c => c.NombresCopletos)
                           .ToList();



                foreach (var d in data)


                {
                    var val = (from v in TiposOpcionesComida
                               where v.tipo_comida_id == d.TipoComidaId
                               where v.opcion_comida_id == d.OpcionComidaId
                               where v.ContratoId == ContratoProveedorId
                               select v).FirstOrDefault();


                    d.precio = val != null && val.Id > 0 ? val.costo : 0;

                    var TipoComida = _catalogo.GetAll().Where(c => c.Id == d.TipoComidaId).FirstOrDefault();
                    var opcionComida = _catalogo.GetAll().Where(c => c.Id == d.OpcionComidaId).FirstOrDefault();

                    if (TipoComida != null)
                    {
                        d.nombretipoComida = TipoComida.nombre;
                    }
                    if (opcionComida != null)
                    {
                        d.nombreopcionComida = opcionComida.nombre;
                    }

                    var duplicado = (from r in result
                                         // where r.fechaConsumo == d.fechaConsumo
                                     where r.formatfechaConsumo == d.formatfechaConsumo
                                     where r.ProveedorId == d.ProveedorId
                                     where r.Identificacion == d.Identificacion
                                     where r.NombresCopletos == d.NombresCopletos
                                     where r.TipoComidaId == d.TipoComidaId
                                     where r.OpcionComidaId == d.OpcionComidaId
                                     select r).FirstOrDefault();
                    if (duplicado != null)
                    {
                        // duplicados.Add(duplicado);
                        duplicados.Add(d);
                    }
                    else
                    {

                        result.Add(d);
                    }
                }

                decimal valor = 0;

                if (File.Exists((string)filename))
                {


                    FileInfo newFile = new FileInfo(filename);

                    ExcelPackage pck = new ExcelPackage(newFile);
                    h = excel.Workbook.Worksheets.Add("RM_" + proveedor.razon_social, pck.Workbook.Worksheets[2]);

                }

                string cell = "C3";
                h.Cells[cell].Value = proveedor.identificacion;
                cell = "C4";
                h.Cells[cell].Value = proveedor.razon_social;
                cell = "C6";
                h.Cells[cell].Value = fechaInicio.ToShortDateString() + " - " + fechaFin.ToShortDateString();

                decimal monto = Convert.ToDecimal(0);




                var count = 7;

                cell = "B" + count;
                h.Cells[cell].Value = "Fecha";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "C" + count;
                h.Cells[cell].Value = "Tipo";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "D" + count;
                h.Cells[cell].Value = "Opción";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "E" + count;
                h.Cells[cell].Value = "Identificación";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                cell = "F" + count;
                h.Cells[cell].Value = "Nombres";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Font.Bold = true;
                count++;



                foreach (var c in duplicados)
                {


                    cell = "B" + count;
                    h.Cells[cell].Value = c.fechaConsumo.ToString("dd/MM/yyyy HH:mm:ss");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "C" + count;
                    h.Cells[cell].Value = c.nombretipoComida;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "D" + count;
                    h.Cells[cell].Value = c.nombreopcionComida;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "E" + count;
                    h.Cells[cell].Value = c.Identificacion;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    cell = "F" + count;
                    h.Cells[cell].Value = c.NombresCopletos;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    h.Cells[cell].Style.WrapText = true;
                    h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    /* cell = "G" + count;
                     h.Cells[cell].Value = c.precio;
                     h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);*/
                    monto = monto + c.precio;

                    count++;



                }
                cell = "C5";
                h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                h.Cells[cell].Value = monto;
                h.Cells[7, 2, h.Dimension.End.Row, 6].AutoFilter = true;
            }
            return excel;

        }


        public List<DobleConsumo> IntentosDoblesConsumos(List<int> Ids, DateTime fechaInicio, DateTime fechaFin)
        {

            var fInicio = fechaInicio.Date;
            var fFin = fechaFin.Date;
            var query_consumos = _dobleconsumo.GetAllIncluding(c => c.Proveedor, c => c.Colaborador)
                .Where(c => c.Fecha >= fInicio)
                .Where(c => c.Fecha <= fFin)
                .ToList();

            /* /// SOLO SI GENERA DESDE REPORTES DE PROVEEDORES ///*/
            if (Ids.Count > 0)
            {
                query_consumos = _dobleconsumo.GetAllIncluding(c => c.Proveedor, c => c.Colaborador)
                .Where(c => c.Fecha >= fInicio)
                .Where(c => c.Fecha <= fFin)
                .Where(c => Ids.Contains(c.ProveedorId))
                .ToList();
            }

            return query_consumos;
        }

        public ExcelPackage ReporteDuplicados(List<int> Ids, DateTime fechaInicio, DateTime fechaFin)
        {

            ExcelPackage excel = new ExcelPackage();
            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaDoblesConsumos.xlsx");

            ExcelWorksheet h = null; //ReporteDuplicados
            ExcelWorksheet hi = null; //Intentos de Doble Consumo

            /*Zonas Proveedores*/

            var zonas_proveedor = _zonaProveedor.GetAllIncluding(c => c.Zona).Where(c => c.vigente).ToList();


            var fInicio = fechaInicio.Date; //Fecha Sin Hora
            var fFin = fechaFin.Date;//Fecha Sin Hora
            var query_consumos = _consumo.GetAllIncluding(c => c.Proveedor, c => c.colaborador, c => c.TipoComida).Where(c => c.fecha >= fInicio).Where(c => c.fecha <= fFin).ToList();
            /* /// SOLO SI GENERA DESDE REPORTES DE PROVEEDORES ///*/
            if (Ids.Count > 0)
            {
                query_consumos = _consumo.GetAllIncluding(c => c.Proveedor, c => c.colaborador, c => c.TipoComida).Where(c => c.fecha >= fInicio).Where(c => c.fecha <= fFin).Where(c => Ids.Contains(c.ProveedorId)).ToList();
                zonas_proveedor = zonas_proveedor.Where(c => Ids.Contains(c.ProveedorId)).ToList();

            }
            var results = from consumo in query_consumos
                          where consumo.fecha.Date >= fechaInicio.Date
                          where consumo.fecha.Date <= fechaFin.Date
                          group consumo by new
                          {
                              consumo.fecha.Date,
                              consumo.colaborador_id,
                              consumo.Tipo_Comida_Id,
                              //consumo.ProveedorId, //Mostrar en que Proveedor Comio
                          }
                           into grupo
                          select new
                          {
                              Grupos = grupo.Key,
                              ConsumosRepetidos =
                              (from d in grupo.ToList()
                               select new ReportConsumo
                               {
                                   fechaConsumo = d.fecha,
                                   formatfechaConsumo = d.fecha.ToString("dd/MM/yyyy HH:mm:ss"),
                                   ProveedorId = d.ProveedorId,
                                   Identificacion = d.colaborador.numero_identificacion,
                                   NombresCopletos = d.colaborador.nombres_apellidos,
                                   nombretipoComida = d.TipoComida.nombre,
                                   TipoComidaId = d.Tipo_Comida_Id,
                                   IdentificadorMovil = d.identificador,
                                   identificador = d.identificador,
                                   nombreOrigen=this.ObtenerNombreOrigenConsumo(d.origen_consumo)
                               }).OrderBy(c => c.fechaConsumo).OrderBy(c => c.nombretipoComida).OrderBy(c => c.NombresCopletos).Distinct().ToList()
                          };

            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                h = excel.Workbook.Worksheets.Add("DOBLES CONSUMOS", pck.Workbook.Worksheets[2]);
            }
            string cell = "";
            var count = 7;

            Color gris = Color.FromArgb(166, 166, 166);
            cell = "B" + count;
            h.Cells[cell].Value = "Fecha";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            h.Cells[cell].Style.Font.Bold = true;
            cell = "C" + count;
            h.Cells[cell].Value = "Tipo Alimentación";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            h.Cells[cell].Style.Font.Bold = true;
            cell = "D" + count;
            h.Cells[cell].Value = "CI";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            h.Cells[cell].Style.Font.Bold = true;
            cell = "E" + count;
            h.Cells[cell].Value = "Nombres";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            h.Cells[cell].Style.Font.Bold = true;
            cell = "F" + count;
            h.Cells[cell].Value = "Proveedor";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            h.Cells[cell].Style.Font.Bold = true;
            cell = "G" + count;
            h.Cells[cell].Value = "Dispositivo";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            h.Cells[cell].Style.Font.Bold = true;

            cell = "H" + count;
            h.Cells[cell].Value = "Sitio";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            h.Cells[cell].Style.Font.Bold = true;

            cell = "I" + count;
            h.Cells[cell].Value = "Consumido por";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            h.Cells[cell].Style.Font.Bold = true;
            count++;

            int color = 1;
            foreach (var grupo in results)
            {

                if (grupo.ConsumosRepetidos.Count > 1 || Ids.Count > 1)
                {
                    var inicial = count;

                    foreach (var c in grupo.ConsumosRepetidos)
                    {
                        var proveedor = Repository.GetAll().Where(x => x.Id == c.ProveedorId).FirstOrDefault();
                        var zonaNombre = "";
                        if (zonas_proveedor != null && zonas_proveedor.Count > 0)
                        {
                            zonaNombre = String.Join(",", (from z in zonas_proveedor where z.ProveedorId == proveedor.Id select z.Zona.nombre).ToList());
                        }

                        cell = "B" + count;
                        h.Cells[cell].Value = c.fechaConsumo.ToString("dd/MM/yyyy HH:mm:ss");
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        cell = "C" + count;
                        h.Cells[cell].Value = c.nombretipoComida;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        cell = "D" + count;
                        h.Cells[cell].Value = c.Identificacion;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        cell = "E" + count;
                        h.Cells[cell].Value = c.NombresCopletos;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        h.Cells[cell].Style.WrapText = true;
                        h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell = "F" + count;
                        h.Cells[cell].Value = proveedor.razon_social;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        cell = "G" + count;
                        h.Cells[cell].Value = c.identificador;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        cell = "H" + count;
                        h.Cells[cell].Value = zonaNombre;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);

                        cell = "I" + count;
                        h.Cells[cell].Value = c.nombreOrigen;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);


                        count++;
                    }
                    var final = count - 1;

                    if (color % 2 == 0)
                    {
                        h.Cells[inicial, 2, final, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        h.Cells[inicial, 2, final, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                    }

                    color++;
                }

            }

            cell = "C6";
            h.Cells[cell].Value = fechaInicio.Date.ToString("dd/MM/yyyy") + " al " + fechaFin.Date.ToString("dd/MM/yyyy");


            h.Cells[7, 2, h.Dimension.End.Row, 9].AutoFilter = true;

            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                hi = excel.Workbook.Worksheets.Add("DOBLES INTENTOS DE CONSUMO", pck.Workbook.Worksheets[3]);

            }
            hi.Cells[cell].Value = fechaInicio.Date.ToString("dd/MM/yyyy") + " al " + fechaFin.Date.ToString("dd/MM/yyyy");
            var intentosDoblesConsumos = this.IntentosDoblesConsumos(Ids, fechaInicio, fechaFin);


            var rowInicial = 7;

            cell = "B" + rowInicial;
            hi.Cells[cell].Value = "Fecha";
            hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            hi.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hi.Cells[cell].Style.Font.Bold = true;

            cell = "C" + rowInicial;
            hi.Cells[cell].Value = "Tipo Alimentacion";
            hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            hi.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hi.Cells[cell].Style.Font.Bold = true;

            cell = "D" + rowInicial;
            hi.Cells[cell].Value = "CI";
            hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            hi.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hi.Cells[cell].Style.Font.Bold = true;

            cell = "E" + rowInicial;
            hi.Cells[cell].Value = "Nombres";
            hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            hi.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hi.Cells[cell].Style.Font.Bold = true;
            cell = "F" + rowInicial;
            hi.Cells[cell].Value = "Proveedor";
            hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            hi.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hi.Cells[cell].Style.Font.Bold = true;
            cell = "G" + rowInicial;
            hi.Cells[cell].Value = "Dispositivo";
            hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            hi.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hi.Cells[cell].Style.Font.Bold = true;
            cell = "H" + rowInicial;
            hi.Cells[cell].Value = "Sitio";
            hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            hi.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hi.Cells[cell].Style.Font.Bold = true;

            cell = "I" + rowInicial;
            hi.Cells[cell].Value = "Consumido Por";
            hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
            hi.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hi.Cells[cell].Style.Font.Bold = true;


            rowInicial++;
            count = rowInicial;
            foreach (var i in intentosDoblesConsumos)
            {
                var proveedor = Repository.GetAll().Where(c => c.Id == i.ProveedorId).FirstOrDefault();

                var zonaNombre = "";
                if (zonas_proveedor != null && zonas_proveedor.Count > 0)
                {
                    zonaNombre = String.Join(",", (from z in zonas_proveedor where z.ProveedorId == proveedor.Id select z.Zona.nombre).ToList());
                }
                var ContratoProveedorId = this.ObtenerContratoProveedor(i.ProveedorId, fechaInicio);
                var TiposOpcionesComida = _tipoOpcionComida.GetAll().Where(c => c.ContratoId == ContratoProveedorId).ToList();
                var TipoComida = _catalogo.GetAll().Where(c => c.Id == i.TipoComidaId).FirstOrDefault();

                cell = "B" + count;
                hi.Cells[cell].Value = i.Fecha.ToString("dd/MM/yyyy HH:mm:ss");
                hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                cell = "C" + count;
                hi.Cells[cell].Value = TipoComida != null ? TipoComida.nombre : "";
                hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                cell = "D" + count;
                hi.Cells[cell].Value = i.Colaborador.numero_identificacion;
                hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                cell = "E" + count;
                hi.Cells[cell].Value = i.Colaborador.nombres;
                hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                cell = "F" + count;
                hi.Cells[cell].Value = proveedor.razon_social;
                hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                hi.Cells[cell].Style.WrapText = true;
                hi.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                cell = "G" + count;
                hi.Cells[cell].Value = i.Identificador;
                hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);

                cell = "H" + count;
                hi.Cells[cell].Value = zonaNombre;
                hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);


                cell = "I" + count;
                hi.Cells[cell].Value = i.OrigenConsumoId.HasValue? this.ObtenerNombreOrigenConsumo(Convert.ToInt32(i.OrigenConsumoId)) :"";
                hi.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);

                count++;

            }
            hi.Cells[7, 2, h.Dimension.End.Row, 9].AutoFilter = true;



            /* workSheet.PrinterSettings.PrintArea = workSheet.Cells[2, 3, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];

             workSheet.PrinterSettings.Orientation = eOrientation.Landscape;
             */
            //h.View.PageBreakView = true;


            h.PrinterSettings.FitToPage = true;
            h.PrinterSettings.PrintArea = h.Cells[2, 1, h.Dimension.End.Row, h.Dimension.End.Column];

            hi.PrinterSettings.FitToPage = true;
            hi.PrinterSettings.PrintArea = h.Cells[2, 1, h.Dimension.End.Row, h.Dimension.End.Column];
            return excel;

        }

        public int DiasVencimientos(DateTime fechaFin)
        {

            var dias = (DateTime.Now.Date - fechaFin.Date).Days;

            return dias < 0 ? 0 : dias; ;
        }
        public ExcelPackage ReporteVencimientoContratosProveedor()

        {

            ExcelPackage excel = new ExcelPackage();
            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/Proveedor/VencimientosContratos.xlsx");

            ExcelWorksheet h = null; //Vencimientos

            /*Contratos Proveedores*/

            var contratos_proveedor = _contratoproveedorrepository.GetAllIncluding(c => c.Proveedor).OrderByDescending(c => c.fecha_fin).ToList();



            var results = from contrato in contratos_proveedor
                              ///where consumo.fecha.Date >= fechaInicio.Date
                              //where consumo.fecha.Date <= fechaFin.Date
                          group contrato by new
                          {
                              contrato.ProveedorId
                          }
                           into grupo
                          select new
                          {
                              ProveedorId = grupo.Key,
                              Contratos =
                              (from d in grupo.ToList()
                               select new ReportVencimientosContrato
                               {
                                   razon_social = d.Proveedor.razon_social,
                                   codigo = d.codigo,
                                   ordenCompra = d.orden_compra,
                                   fechaInicio = d.fecha_inicio.ToString("dd/MM/yyyy"),
                                   fechaFin = d.fecha_fin.ToString("dd/MM/yyyy"),
                                   diasVencimiento = this.DiasVencimientos(d.fecha_fin),
                                   estado = this.DiasVencimientos(d.fecha_fin) <= 0 ? "VENCIDO" : this.DiasVencimientos(d.fecha_fin) > 0 && this.DiasVencimientos(d.fecha_fin) <= 30 ? "POR VENCER" : "CORRECTO",
                               }).ToList()

                          };

            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                h = excel.Workbook.Worksheets.Add("CONTRATOS PROVEEDOR", pck.Workbook.Worksheets[1]);
            }
            string cell = "";
            var count = 8;


            Color gris = Color.FromArgb(166, 166, 166);
            int color = 1;
            int indice = 1;
            foreach (var grupo in results)
            {
                if (grupo.Contratos.Count > 1)
                {
                    var inicial = count;

                    foreach (var c in grupo.Contratos)
                    {

                        cell = "B" + count;
                        h.Cells[cell].Value = indice;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);

                        cell = "C" + count;
                        h.Cells[cell].Value = c.razon_social;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        cell = "D" + count;
                        h.Cells[cell].Value = c.codigo;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        cell = "E" + count;
                        h.Cells[cell].Value = c.ordenCompra;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        h.Cells[cell].Style.WrapText = true;
                        h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell = "F" + count;
                        h.Cells[cell].Value = c.fechaInicio;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        cell = "G" + count;
                        h.Cells[cell].Value = c.fechaFin;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        cell = "H" + count;
                        h.Cells[cell].Value = c.diasVencimiento;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);
                        cell = "I" + count;
                        h.Cells[cell].Value = c.estado;
                        h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, gris);

                        h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        if (c.estado == "VENCIDO")
                        {
                            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 75, 75));

                        }
                        else if (c.estado == "POR VENCER")
                        {
                            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(244, 248, 217));
                        }
                        else
                        {
                            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.Green);
                        }



                        count++;
                        indice++;
                    }
                    var final = count - 1;

                    if (color % 2 == 0)
                    {
                        h.Cells[inicial, 2, final, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        h.Cells[inicial, 2, final, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                    }

                    color++;
                }

            }


            h.Cells[7, 2, h.Dimension.End.Row, 9].AutoFilter = true;



            /* workSheet.PrinterSettings.PrintArea = workSheet.Cells[2, 3, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];

             workSheet.PrinterSettings.Orientation = eOrientation.Landscape;
             */
            //h.View.PageBreakView = true;


            h.PrinterSettings.FitToPage = true;
            h.PrinterSettings.PrintArea = h.Cells[2, 1, h.Dimension.End.Row, h.Dimension.End.Column];

            return excel;

        }

        public List<int> ProveedoresConsolidadosporZona(List<int> ZonaId)
        {

            var query = _servicioProveedorRespRepository.GetAll()
             .Include(o => o.Proveedor)
             .Where(o => o.estado == ServicioEstado.Activo)
             .Where(o => o.Proveedor.estado == dominio.Proveedor.ProveedorEstado.Activo || o.Proveedor.estado == dominio.Proveedor.ProveedorEstado.Inactivo)
             .Where(o => o.Servicio.codigo == CatalogosCodigos.SERVICIO_ALMUERZO).ToList();

            var proveedores = (from sp in query
                               select sp.ProveedorId).ToList();
            if (ZonaId.Count == 0) {
                return proveedores;
            }

            var ZonasProveedor = _zonaProveedor.GetAll().Where(c => ZonaId.Contains(c.ZonaId)).Select(c => c.ProveedorId).ToList();

            var SoloProveedoresZona = proveedores.Where(p => ZonasProveedor.Contains(p)).ToList();

            return SoloProveedoresZona;

            //return proveedores;
        }

        public List<int> ProveedoresConsolidadosporZonaHospedaje(List<int> ZonaId)
        {

            var query = _servicioProveedorRespRepository.GetAll()
             .Include(o => o.Proveedor)
             .Where(o => o.estado == ServicioEstado.Activo)
             .Where(o => o.Proveedor.estado == dominio.Proveedor.ProveedorEstado.Activo || o.Proveedor.estado == dominio.Proveedor.ProveedorEstado.Inactivo)
             .Where(o => o.Servicio.codigo == CatalogosCodigos.SERVICIO_HOSPEDAJE).ToList();

            var proveedores = (from sp in query
                               select sp.ProveedorId).ToList();
            if (ZonaId.Count == 0)
            {
                return proveedores;
            }

            var ZonasProveedor = _zonaProveedor.GetAll().Where(c => ZonaId.Contains(c.ZonaId)).Select(c => c.ProveedorId).ToList();

            var SoloProveedoresZona = proveedores.Where(p => ZonasProveedor.Contains(p)).ToList();

            return SoloProveedoresZona;

            //return proveedores;
        }

        public List<Zona> Zonas()
        {
            var query = _zona.GetAll().Where(c=>c.vigente).ToList();
            return query;
        }




        public ExcelPackage ReporteUsuariosSincronizacionTiempo(DateTime fechaInicio, DateTime fechaFin, List<int> proveedorIds)
        {
            ExcelPackage package = new ExcelPackage();
            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.Add("ReporteSincronizacion");
            ExcelWorksheet h = package.Workbook.Worksheets[1];

            h.Column(1).Width = 20;
            h.Column(2).Width = 35;
            h.Column(3).Width = 30;
            h.Column(4).Width = 30;

            int count = 1;
            string cell = "A" + count;
            h.Cells[cell].Value = "ZONA";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "B" + count;
            h.Cells[cell].Value = "USUARIO";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "C" + count;
            h.Cells[cell].Value = "IDENTIFICADOR";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "D" + count;
            h.Cells[cell].Value = "FECHA SINCRONIZACION";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);


            count++;

            var queryAllSincronizacion = _registroSincronizaciones.GetAll()
                                                                 .Where(t=>t.FechaSincronizacion>=fechaInicio)
                                                                 .Where(t=>t.FechaSincronizacion<=fechaFin)
                                                                 .OrderByDescending(t=>t.FechaSincronizacion)
                                                                 .ToList();
            if (proveedorIds.Count > 0) {
                var cuentasUsuarioProveedor = Repository.GetAll().Where(c => proveedorIds.Contains(c.Id))
                                                               .Select(c => c.usuario).ToList();
                var usuarioId = _usuarioRepository.GetAll()
                                                  .Where(c => cuentasUsuarioProveedor.Contains(c.Cuenta))
                                                  .Select(t => t.Id)
                                                  .ToList();


                if (usuarioId.Count > 0) {
                    queryAllSincronizacion = queryAllSincronizacion.Where(t => usuarioId.Contains(t.UsuarioId)).ToList();
                }
               
            }


            foreach (var data in queryAllSincronizacion)
            {

                if (data != null)
                {

                    var user = _usuarioRepository.GetAll().Where(c => c.Id == data.UsuarioId).FirstOrDefault();
                 
                    var zonaString = "";

                    if (user != null)
                    {
                        var ZonasProveedor = _zonaProveedor.GetAll().Where(c => c.Proveedor.usuario == user.Cuenta).Select(c => c.Zona.nombre).ToList();


                        zonaString = String.Join(",", ZonasProveedor);
                    }

                    cell = "A" + count;
                    h.Cells[cell].Value = user != null ? zonaString : "";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                    cell = "B" + count;
                    h.Cells[cell].Value = user != null ? user.NombresCompletos : "";
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                    cell = "C" + count;
                    h.Cells[cell].Value = data.Identificador;
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                    cell = "D" + count;
                    h.Cells[cell].Value = data.FechaSincronizacion.ToString("dd/MM/yyyy HH:mm:ss");
                    h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                    count++;
                }



            }
            h.Cells[1, 1, h.Dimension.End.Row, h.Dimension.End.Column].AutoFilter = true;



            return package;
        }

    }
}

