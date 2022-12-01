using System.Linq;
using Abp.AutoMapper;
using Abp.Modules;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.entityframework;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using System.Reflection;
using com.cpp.calypso.proyecto.aplicacion.Service;
using com.cpp.calypso.framework;
using Abp.Reflection.Extensions;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.framework.Extensions;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.dominio.Acceso;
using com.cpp.calypso.proyecto.dominio.Accesos;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using com.cpp.calypso.proyecto.dominio.Transporte;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;
using com.cpp.calypso.proyecto.dominio.Documentos;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [DependsOn(

          typeof(FrameworkModule),
          typeof(ComunEntityFrameworkModule),
          typeof(AbpAutoMapperModule),
          typeof(ComunAplicacionModule)
      )]
    public class ProyectoAplicacionModule : AbpModule
    {
        public override void PreInitialize()
        {


            IocManager.Register<IEmpresaAsyncBaseCrudAppService, EmpresaServiceAsyncBaseCrudAppService>();
            IocManager.Register<IContratoAsyncBaseCrudAppService, ContratoServiceAsyncBaseCrudAppService>();
            IocManager.Register<IContratoDocumentoBancarioAsyncBaseCrudAppService, ContratoDocumentoBancarioServiceAsyncBaseCrudAppService>();
            IocManager.Register<ICentrocostoContratoAsyncBaseCrudAppService, CentrocostosContratoServiceAsyncBaseCrudAppService>();
            IocManager.Register<IAdendaAsyncBaseCrudAppService, AdendaServiceAsyncBaseCrudAppService>();
            IocManager.Register<IItemAsyncBaseCrudAppService, ItemServiceAsyncBaseCrudAppService>();
            IocManager.Register<IWbsOfertaAsyncBaseCrudAppService, WbsOfertaServiceAsyncBaseCrudAppService>();
            IocManager.Register<IComputoAsyncBaseCrudAppService, ComputoServiceAsyncBaseCrudAppService>();
            IocManager.Register<IClienteAsyncBaseCrudAppService, ClienteServiceAsyncBaseCrudAppService>();
            IocManager.Register<IPreciarioAsyncBaseCrudAppService, PreciarioServiceAsyncBaseCrudAppService>();
            IocManager.Register<IDetallePreciarioAsyncBaseCrudAppService, DetallePreciarioServiceAsyncBaseCrudAppService>();
            IocManager.Register<IRdoCabeceraAsyncBaseCrudAppService, RdoCabeceraAsyncBaseCrudAppService>();
            IocManager.Register<IRdoDetalleAsyncBaseCrudAppService, RdoDetalleServiceAsyncBaseCrudAppService>();
            IocManager.Register<IGananciaAsyncBaseCrudAppService, GananciaServiceAsyncBaseCrudAppService>();
            IocManager.Register<ITransmitalCabeceraAsyncBaseCrudAppService, TransmitalCabeceraServiceAsyncBaseCrudAppService>();
            IocManager.Register<ITransmitalDetalleAsyncBaseCrudAppService, TransmitalDetalleServiceAsyncBaseCrudAppService>();
            IocManager.Register<IFacturaAsyncBaseCrudAppService, FacturaServiceAsyncBaseCrudAppService>();
            IocManager.Register<ICertificadoFacturaAsyncBaseCrudAppService, CertificadoFacturaServiceAsyncBaseCrudAppService>();
            IocManager.Register<IRetencionFacturaAsyncBaseCrudAppService, RetencionFacturaServiceAsyncBaseCrudAppService>();
            IocManager.Register<IDestinatarioAsyncBaseCrudAppService, DestinatarioServiceAsyncBaseCrudAppService>();
            IocManager.Register<IDestinatarioCartaAsyncBaseCrudAppService, DestinatarioCartaServiceAsyncBaseCrudAppService>();
            IocManager.Register<ICartaArchivoAsyncBaseCrudAppService, CartaArchivoServiceAsyncBaseCrudAppService>();
            IocManager.Register<ICartaAsyncBaseCrudAppService, CartaServiceAsyncBaseCrudAppService>();
            IocManager.Register<IArchivoAsyncBaseCrudAppService, ArchivoServiceAsyncBaseCrudAppService>();
            IocManager.Register<IAvanceProcuraAsyncBaseCrudAppService, AvanceProcuraServiceAsyncBaseCrudAppService>();
            IocManager.Register<IDetalleAvanceProcuraAsyncBaseCrudAppService, DetalleAvanceProcuraServiceAsyncBaseCrudAppService>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());


            // Configurar los mappers entre objetos
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                //Proveedor - DTO
                cfg.CreateMap<dominio.Proveedor.Proveedor, ProveedorDto>()
                    .ForMember(x => x.estado_nombre, opt => opt.MapFrom(src => src.estado.GetDescription()))
                    .ForMember(x => x.tipo_identificacion_nombre, opt => opt.MapFrom(src => src.tipo_identificacion.ToString()))
                    .ForMember(x => x.tipo_proveedor_nombre, opt => opt.MapFrom(src => src.tipo_proveedor.nombre))

                    //Contacto
                    .ForMember(x => x.contacto_id, opt => opt.MapFrom(src => src.contacto.Id))
                    .ForMember(x => x.calle_principal, opt => opt.MapFrom(src => src.contacto.calle_principal))
                    .ForMember(x => x.calle_secundaria, opt => opt.MapFrom(src => src.contacto.calle_secundaria))
                    .ForMember(x => x.celular, opt => opt.MapFrom(src => src.contacto.celular))
                    // .ForMember(x => x.CiudadId, opt => opt.MapFrom(src => src.contacto.CiudadId))
                    //  .ForMember(x => x.PaisId, opt => opt.MapFrom(src => src.contacto.PaisId))
                    .ForMember(x => x.ParroquiaId, opt => opt.MapFrom(src => src.contacto.ParroquiaId))
                    // .ForMember(x => x.ProvinciaId, opt => opt.MapFrom(src => src.contacto.ProvinciaId))
                    .ForMember(x => x.referencia, opt => opt.MapFrom(src => src.contacto.referencia))
                    .ForMember(x => x.telefono_convencional, opt => opt.MapFrom(src => src.contacto.telefono_convencional))
                    .ForMember(x => x.numero, opt => opt.MapFrom(src => src.contacto.numero))
                    .ForMember(x => x.correo_electronico, opt => opt.MapFrom(src => src.contacto.correo_electronico))
                    ;

                cfg.CreateMap<Contacto, ProveedorDto>()
                     .ForMember(x => x.contacto_id, opt => opt.MapFrom(src => src.Id))
                     ;


                cfg.CreateMap<ProveedorDto, ContactoDto>()
                  .ForMember(x => x.Id, opt => opt.MapFrom(src => src.contacto_id))
                  ;


                //Proveedor - Info
                cfg.CreateMap<dominio.Proveedor.Proveedor, ProveedorInfoDto>()
                     .ForMember(x => x.estado_nombre, opt => opt.MapFrom(src => src.estado.GetDescription()))
                     .ForMember(x => x.tipo_identificacion_nombre, opt => opt.MapFrom(src => src.tipo_identificacion.ToString()))
                     .ForMember(x => x.tipo_proveedor_nombre, opt => opt.MapFrom(src => src.tipo_proveedor.nombre))
                     ;

                //Proveedor - Detalle
                cfg.CreateMap<dominio.Proveedor.Proveedor, ProveedorDetalleDto>()
                     .ForMember(x => x.estado_nombre, opt => opt.MapFrom(src => src.estado.GetDescription()))
                     //Contacto
                     .ForMember(x => x.correo_electronico, opt => opt.MapFrom(src => src.contacto.correo_electronico))
                     .ForMember(x => x.telefono_convencional, opt => opt.MapFrom(src => src.contacto.telefono_convencional))
                     ;




                cfg.CreateMap<Contacto, ProveedorDetalleDto>();


                //Proveedor - Servicio
                cfg.CreateMap<ServicioProveedor, ServicioProveedorDto>()
               .ForMember(x => x.estado_nombre, opt => opt.MapFrom(src => src.estado.GetDescription()))
               .ForMember(x => x.servicio_nombre, opt => opt.MapFrom(src => src.Servicio.nombre))
               .ForMember(x => x.servicio_codigo, opt => opt.MapFrom(src => src.Servicio.codigo))
               ;

                //RequisitoServicio - RequisitoServicioDto
                cfg.CreateMap<RequisitoServicio, RequisitoServicioDto>()
              // .ForMember(x => x.servicio_nombre, opt => opt.MapFrom(src => src.Servicio.nombre))
              //  .ForMember(x => x.requisito_nombre, opt => opt.MapFrom(src => src.requisito.nombre))
              ;

                //Proveedor - Requisito
                cfg.CreateMap<RequisitoProveedor, RequisitoProveedorDto>()
              .ForMember(x => x.requisito_nombre, opt => opt.MapFrom(src => src.Requisitos.nombre))
              .ForMember(x => x.cumple_nombre, opt => opt.MapFrom(src => src.cumple.GetDescription()))
              ;

                //Proveedor - Zona
                cfg.CreateMap<ZonaProveedor, ZonaProveedorDto>()
                        .ForMember(x => x.zona_nombre, opt => opt.MapFrom(src => src.Zona.nombre));



                //cfg.CreateMap<ZonaProveedor, ZonaProveedorDto>()
                //.ForMember(x => x.zona_nombre, opt => opt.MapFrom(src => src.Zona.nombre));



                //Proveedor Contacto
                cfg.CreateMap<ContratoProveedor, ContratoProveedorDto>()
                        .ForMember(x => x.empresa_nombre,
                        opt => opt.MapFrom(src => src.Empresa.razon_social))
                        .ForMember(x => x.estado_nombre,
                        opt => opt.MapFrom(src => src.estado.ToString()))
                        ;

                cfg.CreateMap<ProveedorDto, ContactoDto>()
                     .ForMember(x => x.CiudadId,
                        opt => opt.MapFrom(src => src.CiudadId))
                        .ForMember(x => x.ProvinciaId,
                        opt => opt.MapFrom(src => src.ProvinciaId))

                       ;



                cfg.CreateMap<ContratoProveedor, ContratoProveedorTipoOpcionesDto>()
                      .ForMember(x => x.empresa_nombre,
                      opt => opt.MapFrom(src => src.Empresa.razon_social))
                      .ForMember(x => x.proveedor_nombre,
                      opt => opt.MapFrom(src => src.Proveedor.razon_social))
                      .ForMember(x => x.estado_nombre,
                      opt => opt.MapFrom(src => src.estado.ToString()))
                      ;

                //Proveedor Contacto TipoComidaOpciones
                cfg.CreateMap<TipoOpcionComida, TipoOpcionComidaDto>()
                       .ForMember(x => x.opcion_comida_nombre,
                       opt => opt.MapFrom(src => src.opcion_comida.nombre))
                       .ForMember(x => x.tipo_comida_nombre,
                       opt => opt.MapFrom(src => src.tipo_comida.nombre))
                       ;

                //Vianda
                cfg.CreateMap<SolicitudVianda, SolicitudViandaDto>()
                        .ForMember(x => x.solicitante_nombre,
                        opt => opt.MapFrom(src => src.solicitante.nombres))
                        .ForMember(x => x.disciplina_nombre,
                        opt => opt.MapFrom(src => src.disciplina.nombre))
                        .ForMember(x => x.locacion_nombre,
                        opt => opt.MapFrom(src => src.locacion.nombre))
                        .ForMember(x => x.tipo_comida_nombre,
                        opt => opt.MapFrom(src => src.tipo_comida.nombre))
                        ;

                //Tipo Accion Empresa
                cfg.CreateMap<TipoAccionEmpresa, TipoAccionEmpresaDto>()
                       .ForMember(x => x.accion_nombre,
                       opt => opt.MapFrom(src => src.Accion.nombre))
                       .ForMember(x => x.tipo_comida_nombre,
                       opt => opt.MapFrom(src => src.tipo_comida.nombre))
                       .ForMember(x => x.empresa_nombre,
                       opt => opt.MapFrom(src => src.Empresa.razon_social))
                       ;

                //Locacion
                cfg.CreateMap<Locacion, LocacionDto>()
                       .ForMember(x => x.zona_nombre,
                       opt => opt.MapFrom(src => src.Zona.nombre))
                       ;

                // Habitaciones
                cfg.CreateMap<Habitacion, HabitacionDto>()
                    .ForMember(x => x.tipo_habitacion_nombre, opt => opt.MapFrom(src => src.TipoHabitacion.nombre))
                    .ForMember(x => x.estado_nombre, opt => opt.MapFrom(src => src.GetEstadoNombre()))
                    ;

                // Espacios Habitaciones
                cfg.CreateMap<EspacioHabitacion, EspacioHabitacionDto>()
                    .ForMember(x => x.tipo_habitacion_nombre, opt => opt.MapFrom(src => src.Habitacion.TipoHabitacion.nombre))
                    .ForMember(x => x.numero_habitacion, opt => opt.MapFrom(src => src.Habitacion.numero_habitacion))
                    .ForMember(x => x.estado_nombre, opt => opt.MapFrom(src => src.GetEstadoNombre()))
                    .ForMember(x => x.activo_nombre, opt => opt.MapFrom(src => src.GetActivoNombre()))
                    .ForMember(x => x.proveedor_razon_social, opt => opt.MapFrom(src => src.Habitacion.Proveedor.razon_social))
                    .ForMember(x => x.capacidad_habitacion, opt => opt.MapFrom(src => src.Habitacion.capacidad))
                    .ForMember(x => x.capacidadHabitacionConfig, opt => opt.MapFrom(src => src.Habitacion.capacidad))
                    ;

                // Tarifas Hoteles
                cfg.CreateMap<TarifaHotel, TarifaHotelDto>()
                    .ForMember(x => x.tipo_habitacion_nombre, opt => opt.MapFrom(src => src.TipoHabitacion.nombre))
                    .ForMember(x => x.estado_nombre, opt => opt.MapFrom(src => src.GetEstadoNombre()))
                    .ForMember(x => x.total, opt => opt.MapFrom(src => src.GetTotal()))
                    ;


                cfg.CreateMap<TarifaLavanderia, TarifaLavanderiaDto>()
                  .ForMember(x => x.tipo_servicio_nombre, opt => opt.MapFrom(src => src.TipoServicio.nombre))
                  .ForMember(x => x.estado_nombre, opt => opt.MapFrom(src => src.GetEstadoNombre()))

                  ;


                cfg.CreateMap<TarifaHotel, TarifaTipoHabitacionDto>()
                    .ForMember(x => x.name, opt => opt.MapFrom(src => src.TipoHabitacion.nombre))
                    .ForMember(x => x.Id, opt => opt.MapFrom(src => src.TipoHabitacion.Id))
                    ;

                // Reservas Hoteles
                cfg.CreateMap<ReservaHotel, ReservaHotelDto>()
                    .ForMember(x => x.estado_nombre, opt => opt.MapFrom(src => src.estado.GetDescription()))
                    .ForMember(x => x.colaborador_nombres, opt => opt.MapFrom(src => src.Colaborador.nombres_apellidos))
                      .ForMember(x => x.colaborador_identificacion, opt => opt.MapFrom(src => src.Colaborador.numero_identificacion))
                    .ForMember(x => x.colaborador_id_sap, opt => opt.MapFrom(src => src.Colaborador.empleado_id_sap))
                    .ForMember(x => x.finalizado_manual, opt => opt.MapFrom(src => src.consumo_finalizado ? "SI" : "NO"))
                    .ForMember(x => x.iniciado_manual, opt => opt.MapFrom(src => src.inicio_consumo ? "SI" : "NO"))

                    //Obtenidos Nuevos Campos de Reserva Hotel
                     .ForMember(x => x.numero_habitacion, opt => opt.MapFrom(src =>string.Concat(src.NumeroHabitacion," - ",src.CodigoEspacio)))
                     .ForMember(x => x.tipo_habitacion_nombre, opt => opt.MapFrom(src => src.NombreTipoHabitacion))

                     //Campos Obtenidos de Espacio Habitacion
                    // .ForMember(x => x.numero_habitacion, opt => opt.MapFrom(src => src.EspacioHabitacion.GetHabitacionEspacioCodigo()))
                    //  .ForMember(x => x.tipo_habitacion_nombre, opt => opt.MapFrom(src => src.EspacioHabitacion.Habitacion.TipoHabitacion.nombre))

                    .ForMember(x => x.proveedor_razon_social, opt => opt.MapFrom(src => src.EspacioHabitacion.Habitacion.Proveedor.razon_social))
                    .ForMember(x => x.colaborador_grupo_personal, opt => opt.MapFrom(src => src.Colaborador.GrupoPersonal.nombre))
                    .ForMember(x => x.es_extemporaneo, opt => opt.MapFrom(src => src.extemporaneo ? "SI" : "NO"))
                    .ForMember(x => x.extemporaneo, opt => opt.MapFrom(src => src.extemporaneo))
                    ;

                //Detalles Reserva Hoteles
                cfg.CreateMap<DetalleReserva, DetalleReservaDto>()
                    .ForMember(x => x.facturado_nombre, opt => opt.MapFrom(src => src.GetFacturadoNombre()))
                    .ForMember(x => x.consumido_nombre, opt => opt.MapFrom(src => src.GetConsumidoNombre()))
                                        .ForMember(x => x.aplica_lavanderia_nombre, opt => opt.MapFrom(src => src.aplica_lavanderia ? "Si" : "No"))
                     .ForMember(x => x.fecha_reserva_format, opt => opt.MapFrom(src => src.fecha_reserva.HasValue ? src.fecha_reserva.Value.ToString("dd/MM/yyyy HH:mm:ss") : ""))
                     .ForMember(x => x.fecha_consumo_format, opt => opt.MapFrom(src => src.fecha_consumo.HasValue ? src.fecha_consumo.Value.ToString("dd/MM/yyyy HH:mm:ss") : ""))
                    ;

                // Colaboradores
                cfg.CreateMap<Colaboradores, ColaboradoresDetallesDto>()
                    .ForMember(x => x.PrimerApellido, opt => opt.MapFrom(src => src.primer_apellido))
                    .ForMember(x => x.SegundoApellido, opt => opt.MapFrom(src => src.segundo_apellido))
                    .ForMember(x => x.NombresApellidos, opt => opt.MapFrom(src => src.nombres_apellidos))
                    .ForMember(x => x.CargoNombre, opt => opt.MapFrom(src => src.Cargo.nombre))
                    .ForMember(x => x.PaisNombre, opt => opt.MapFrom(src => src.Pais.nombre))
                    .ForMember(x => x.PaisNombre, opt => opt.MapFrom(src => src.Pais.nombre))
                    .ForMember(x => x.ProvinciaNombre, opt => opt.MapFrom(src => src.Contacto.Parroquia.Ciudad.Provincia.nombre))
                    .ForMember(x => x.Calle, opt => opt.MapFrom(src => src.Contacto.calle_principal))
                    .ForMember(x => x.Interseccion, opt => opt.MapFrom(src => src.Contacto.calle_secundaria))
                    .ForMember(x => x.Ciudad, opt => opt.MapFrom(src => src.Contacto.Parroquia.Ciudad.nombre))
                    .ForMember(x => x.Parroquia, opt => opt.MapFrom(src => src.Contacto.Parroquia.nombre))
                    .ForMember(x => x.NumeroCasa, opt => opt.MapFrom(src => src.Contacto.numero))
                    .ForMember(x => x.TelefonoDomicilio, opt => opt.MapFrom(src => src.Contacto.telefono_convencional))
                    .ForMember(x => x.TelefonoCelular, opt => opt.MapFrom(src => src.Contacto.celular))
                    .ForMember(x => x.Correo, opt => opt.MapFrom(src => src.Contacto.correo_electronico))
                    .ForMember(x => x.TipoIdentificacionNombre, opt => opt.MapFrom(src => src.TipoIdentificacion.nombre))
                    .ForMember(x => x.Identificacion, opt => opt.MapFrom(src => src.numero_identificacion))
                    .ForMember(x => x.Departamento, opt => opt.MapFrom(src => src.Sector.nombre))
                    .ForMember(x => x.GrupoPersonalId, opt => opt.MapFrom(src => src.GrupoPersonal.Id))
                    .ForMember(x => x.PrimerNombre, opt => opt.MapFrom(src => src.nombres))
                    .ForMember(x => x.Genero, opt => opt.MapFrom(src => src.Genero.nombre))
                    .ForMember(x => x.GrupoPersonal, opt => opt.MapFrom(src => src.GrupoPersonal.nombre))
                       .ForMember(x => x.EncargadoPersonal, opt => opt.MapFrom(src => src.EncargadoPersonal != null ? src.EncargadoPersonal.nombre : ""))
                       .ForMember(x => x.IdSap, opt => opt.MapFrom(src => src.empleado_id_sap > 0 ? "" + src.empleado_id_sap : ""))
                    .ForMember(x => x.Telefonos, opt => opt.MapFrom(src => src.Contacto.GetTelefonos()))
                      .ForMember(x => x.FechaRegistro, opt => opt.MapFrom(src => src.fecha_ingreso.GetValueOrDefault().ToShortDateString()))
                    ;

                cfg.CreateMap<Colaboradores, ColaboradorNombresDto>()
                    .ForMember(x => x.grupo_personal, opt => opt.MapFrom(src => src.GrupoPersonal.nombre))
                    .ForMember(x => x.estado, opt => opt.MapFrom(src => src.estado))
                    .ForMember(x=>x.fechaIngreso,opt=>opt.MapFrom(src=>src.fecha_ingreso.HasValue?src.fecha_ingreso.Value.ToShortDateString():""))
                    ;

                // ConsultaPublica
                cfg.CreateMap<ConsultaPublica, ConsultaPublicaDto>()
                    .ForMember(x => x.tipo_identificacion_nombre, opt => opt.MapFrom(src => src.TipoIdentificacion.nombre))
                    //.ForMember(x => x.PaisTrabajoId, opt => opt.MapFrom(src => src.CiudadTrabajo.Provincia.PaisId))
                    //.ForMember(x => x.ProvinciaTrabajoId, opt => opt.MapFrom(src => src.CiudadTrabajo.ProvinciaId))
                    ;

                // Tarjetas
                cfg.CreateMap<TarjetaAcceso, TarjetaAccesoDto>()
                    .ForMember(x => x.entregada_nombre, opt => opt.MapFrom(src => src.Entragada()))
                    .ForMember(x => x.secuencial_format, opt => opt.MapFrom(src => StringHelper.FormtSixDigits(src.secuencial)))
                    .ForMember(x => x.estado_nombre, opt => opt.MapFrom(src => src.estado.GetDescription()))
                    ;

                // Vehiculos
                cfg.CreateMap<Vehiculo, VehiculoDto>()
                    .ForMember(x => x.ProveedorRazonSocial, opt => opt.MapFrom(src => src.Proveedor.razon_social))
                    .ForMember(x => x.TipoVehiculo, opt => opt.MapFrom(src => src.TipoVehiculo.nombre))
                     .ForMember(x => x.FechaVencimiento, opt => opt.MapFrom(src => src.FechaVencimientoMatricula.ToShortDateString()))
                    .ForMember(x => x.EstadoNombre, opt => opt.MapFrom(src => src.GetEstado()))
                    ;


                //Qr Reserva Hotel y Detalle Reserva

                // Reservas Hoteles
                cfg.CreateMap<ReservaHotel, ReservaQR>()

                    .ForMember(x => x.proveedorId, opt => opt.MapFrom(src => src.EspacioHabitacion.Habitacion.ProveedorId))

                        ;

                //Detalles Reserva Hoteles
                cfg.CreateMap<DetalleReserva, ReservaQRDetalle>()
                .ForMember(x => x.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.fre, opt => opt.MapFrom(src => src.fecha_reserva.GetValueOrDefault().ToShortDateString()));

                //Consumos Extemporaneo
                cfg.CreateMap<ConsumoExtemporaneo, ConsumoExtemporaneoDto>()
                    .ForMember(x => x.ProveedorNombre, opt => opt.MapFrom(src => src.Proveedor.razon_social))
                    .ForMember(x => x.TipoComidaNombre, opt => opt.MapFrom(src => src.TipoComida.nombre));

                //Detalle Consumos Extemporaneo
                cfg.CreateMap<DetalleConsumoExtemporaneo, DetalleConsumoExtemporaneoDto>()
                    .ForMember(x => x.ColaboradorNombre, opt => opt.MapFrom(src => src.NombresCompletos()))
                    .ForMember(x => x.LiquidadoString, opt => opt.MapFrom(src => src.GetLiquidadoString()))
                    .ForMember(x => x.ColaboradorIdentificacion, opt => opt.MapFrom(src => src.Colaborador.numero_identificacion));

                //Detalles Liquidacion
                cfg.CreateMap<LiquidacionServicio, LiquidacionServicioDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.NombreContratoProveedor, opt => opt.MapFrom(src => src.ContratoProveedor.Proveedor.razon_social))
                .ForMember(x => x.ProveedorId, opt => opt.MapFrom(src => src.ContratoProveedor.Proveedor.Id))
                .ForMember(x => x.FormatFechaDesde, opt => opt.MapFrom(src => src.FechaDesde.ToShortDateString()))
                .ForMember(x => x.FormatFechaHasta, opt => opt.MapFrom(src => src.FechaHasta.ToShortDateString()))
                .ForMember(x => x.FormatFechaPago, opt => opt.MapFrom(src => src.FechaPago.HasValue ? src.FechaPago.GetValueOrDefault().ToShortDateString() : ""))
                .ForMember(x => x.MontoConsumido, opt => opt.MapFrom(src => src.MontoConsumido))
                .ForMember(x => x.NombreTipoServicio, opt => opt.MapFrom(src => src.TipoServicio.nombre))
                .ForMember(x => x.TipoServicioId, opt => opt.MapFrom(src => src.TipoServicioId))
                .ForMember(x => x.FechaDesde, opt => opt.MapFrom(src => src.FechaDesde))
                .ForMember(x => x.FechaHasta, opt => opt.MapFrom(src => src.FechaHasta))
                .ForMember(x => x.NombreEstado, opt => opt.MapFrom(src => src.Estado.GetDescription()));

                //Liquidaciones Servicios
                cfg.CreateMap<DetalleReserva, FormatLiquidacionReserva>()
               .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(x => x.NombreProveedor, opt => opt.MapFrom(src => src.ReservaHotel.EspacioHabitacion.Habitacion.Proveedor.razon_social))
               .ForMember(x => x.FechaConsumo, opt => opt.MapFrom(src => src.fecha_consumo.HasValue ? src.fecha_consumo.GetValueOrDefault().ToShortDateString() : "dd/mm/aaaa"))
               .ForMember(x => x.Legajo, opt => opt.MapFrom(src => src.ReservaHotel.Colaborador.numero_legajo_definitivo))
               .ForMember(x => x.Identificacion, opt => opt.MapFrom(src => src.ReservaHotel.Colaborador.numero_identificacion))
               .ForMember(x => x.Nombres, opt => opt.MapFrom(src => src.ReservaHotel.Colaborador.nombres_apellidos))
               .ForMember(x => x.Cargo, opt => opt.MapFrom(src => src.ReservaHotel.Colaborador.Cargo.nombre))
               .ForMember(x => x.Habitacion, opt => opt.MapFrom(src => src.ReservaHotel.EspacioHabitacion.Habitacion.numero_habitacion))
               .ForMember(x => x.Espacio, opt => opt.MapFrom(src => src.ReservaHotel.EspacioHabitacion.codigo_espacio))
               .ForMember(x => x.Tipo, opt => opt.MapFrom(src => src.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacion.nombre))
               .ForMember(x => x.TipoHabitacionId, opt => opt.MapFrom(src => src.ReservaHotel.EspacioHabitacion.Habitacion.TipoHabitacionId))
               .ForMember(x => x.FechaSalida, opt => opt.MapFrom(src => src.ReservaHotel.fecha_hasta.ToShortDateString()))
               .ForMember(x => x.liquidado, opt => opt.MapFrom(src => src.liquidado));

                //Liquidaciones Servicios
                cfg.CreateMap<Consumo, FormatLiquidacionConsumo>()
               .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(x => x.NombreProveedor, opt => opt.MapFrom(src => src.Proveedor.razon_social))
               .ForMember(x => x.FechaConsumo, opt => opt.MapFrom(src => src.fecha != null ? src.fecha.ToShortDateString() : "dd/mm/aaaa"))
               .ForMember(x => x.Legajo, opt => opt.MapFrom(src => src.colaborador.numero_legajo_definitivo))
               .ForMember(x => x.Identificacion, opt => opt.MapFrom(src => src.colaborador.numero_identificacion))
               .ForMember(x => x.Nombres, opt => opt.MapFrom(src => src.colaborador.nombres_apellidos))
               .ForMember(x => x.Cargo, opt => opt.MapFrom(src => src.colaborador.Cargo.nombre))
               .ForMember(x => x.TipoComida, opt => opt.MapFrom(src => src.TipoComida.nombre))
               .ForMember(x => x.OpcionComida, opt => opt.MapFrom(src => src.OpcionComida.nombre))
                           .ForMember(x => x.TipoComidaId, opt => opt.MapFrom(src => src.Tipo_Comida_Id))
               .ForMember(x => x.OpcionComidaId, opt => opt.MapFrom(src => src.Opcion_Comida_Id))
               .ForMember(x => x.liquidado, opt => opt.MapFrom(src => src.liquidado));

                // Capacitaciones
                cfg.CreateMap<Colaboradores, com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto.ColaboradorDto>()
                    .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(x => x.area_nombre, opt => opt.MapFrom(src => src.Area.nombre))
                    .ForMember(x => x.fecha_baja, opt => opt.MapFrom(src => src.ColaboradorBajas.Count > 0 ? src.ColaboradorBajas.Last().fecha_baja : null));

                cfg.CreateMap<Capacitacion, CapacitacionDto>()
                    .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(x => x.ColaboradorNombre, opt => opt.MapFrom(src => src.Colaboradores.nombres_apellidos))
                    .ForMember(x => x.ColaboradorSap, opt => opt.MapFrom(src => src.Colaboradores.empleado_id_sap))
                    .ForMember(x => x.ColaboradorIdentificacion, opt => opt.MapFrom(src => src.Colaboradores.numero_identificacion))
                    .ForMember(x => x.TipoCapacitacionNombre,
                        opt => opt.MapFrom(src => src.CatalogoTipoCapacitacion.nombre))
                    .ForMember(x => x.NombreCapacitacion,
                        opt => opt.MapFrom(src => src.CatalogoNombreCapacitacion.nombre));

                // ActualizacionDeSaldos
                cfg.CreateMap<DetalleActualizacionSueldo, DetalleActualizacionSueldoDto>()
                    .ForMember(x => x.NombreCategoriaEncargado, opt => opt.MapFrom(src => src.CategoriaEncargado.Categoria.nombre));
                /* Carpeta */
                cfg.CreateMap<Carpeta, CarpetaDto>()
                    .ForMember(x => x.EstadoNombre, opt => opt.MapFrom(src => src.Estado.nombre))
                    .ForMember(x => x.NumeroDocumentos, opt => opt.MapFrom(src => src.Documentos.Count()));

                /* Documento */
                cfg.CreateMap<Documento, DocumentoDto>()
                    .ForMember(x => x.TipoDocumentoNombre, opt => opt.MapFrom(src => src.TipoDocumento.nombre))
                    .ForMember(x => x.DocumentoPadreCodigo, opt => opt.MapFrom(src => src.DocumentoPadre.Codigo));

                /* ColaboradorRubroIngenieria */
                cfg.CreateMap<ColaboradorRubroIngenieria, ColaboradorRubroIngenieriaDto>()
                    .ForMember(x => x.Identificacion, opt => opt.MapFrom(src => src.Colaborador.numero_identificacion))
                    .ForMember(x => x.Nombres, opt => opt.MapFrom(src => src.Colaborador.nombres_apellidos))
                     .ForMember(x => x.Estado, opt => opt.MapFrom(src => src.Colaborador.estado))
                    .ForMember(x => x.RubroNombre, opt => opt.MapFrom(src => src.Rubro.Item.nombre))
                    .ForMember(x => x.ItemId, opt => opt.MapFrom(src => src.Rubro.ItemId))
                    .ForMember(x => x.NombreContrato, opt => opt.MapFrom(src => src.Contrato.Codigo));
                /* Feriados*/
                cfg.CreateMap<Feriado, FeriadoDto>()
                    .ForMember(x => x.Anio, opt => opt.MapFrom(src => src.FechaInicio.Year));
                /* Colaborador Ingenieria*/
                cfg.CreateMap<dominio.CertificacionIngenieria.ColaboradorCertificacionIngenieria, CertificacionIngenieria.Dto.ColaboradorCertificacionIngenieriaDto>()
                    .ForMember(x => x.DisciplinaNombre, opt => opt.MapFrom(src => src.Disciplina.nombre))
                    .ForMember(x => x.ModalidadNombre, opt => opt.MapFrom(src => src.Modalidad.nombre))
                    .ForMember(x => x.UbicacionNombre, opt => opt.MapFrom(src => src.Ubicacion.nombre));
                cfg.CreateMap<Colaboradores, ColaboradorListadoIngenieriaDto>()
                    .ForMember(x => x.Nombres, opt => opt.MapFrom(src => src.nombres_apellidos))
                    .ForMember(x => x.Identificacion, opt => opt.MapFrom(src => src.numero_identificacion))
                    .ForMember(x => x.Externo, opt => opt.MapFrom(src => src.es_externo.Value ? "SI" : "NO"))
                    .ForMember(x => x.CodigoSap, opt => opt.MapFrom(src => src.empleado_id_sap))
                    .ForMember(x => x.FechaIngreso, opt => opt.MapFrom(src => src.fecha_ingreso))
                    .ForMember(x => x.Estado, opt => opt.MapFrom(src => src.estado));

                /* Detalles Indirectos*/
                cfg.CreateMap<DetalleIndirectosIngenieria, DetalleIndirectosIngenieriaDto>()
                    .ForMember(x => x.ColaboradorNombres, opt => opt.MapFrom(src => src.ColaboradorRubro.Colaborador.nombres_apellidos))

                    .ForMember(x => x.ColaboradorIdentificacion, opt => opt.MapFrom(src => src.ColaboradorRubro.Colaborador.numero_identificacion))
                    .ForMember(x => x.CertificadoNombre, opt => opt.MapFrom(src => src.Certificado ? "SI" : "NO"))
                          .ForMember(x => x.ItemNombre, opt => opt.MapFrom(src => src.ColaboradorRubro.Rubro.Item.nombre))
                    .ForMember(x => x.ColaboradorId, opt => opt.MapFrom(src => src.ColaboradorRubro.ColaboradorId))
                   .ForMember(x => x.FechaDesdeString, opt => opt.MapFrom(src => src.FechaDesde.ToShortDateString()))
                                .ForMember(x => x.contratoId, opt => opt.MapFrom(src => src.ColaboradorRubro.ContratoId))
                   .ForMember(x => x.FechaHastaString, opt => opt.MapFrom(src => src.FechaHasta.ToShortDateString()));
                cfg.CreateMap<PorcentajeIndirectoIngenieria, PorcentajeIndirectoIngenieriaDto>()
                    .ForMember(x => x.ContratoNombre, opt => opt.MapFrom(src => src.Contrato.Codigo));
                cfg.CreateMap<PorcentajeAvanceIngenieria, PorcentajeAvanceIngenieriaDto>()
                    .ForMember(x => x.PorcentajeNombre, opt => opt.MapFrom(src => src.CatalogoPorcentaje.nombre))
                    .ForMember(x => x.ProyectoCodigo, opt => opt.MapFrom(src => src.Proyecto.codigo))
                    .ForMember(x => x.ProyectoNombre, opt => opt.MapFrom(src => src.Proyecto.nombre_proyecto));
                cfg.CreateMap<Proyecto, ProyectoIngenieriaDto>()
                    .ForMember(x => x.Codigo, opt => opt.MapFrom(src => src.codigo))
                    .ForMember(x => x.Nombre, opt => opt.MapFrom(src => src.nombre_proyecto));
                cfg.CreateMap<PlanificacionTimesheet, PlanificacionTimesheetDto>()
                    .ForMember(x => x.Anio, opt => opt.MapFrom(src => src.Fecha.Year));
            });


        }

        public override void PostInitialize()
        {

        }
    }
}
