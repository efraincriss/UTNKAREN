const webpack = require("webpack");
const path = require("path");

module.exports = {
  entry: {
    //wbs: ["./Scripts/react-components/Wbs.js", "./Scripts/react-components/JerarquiaWbs.js", "./Scripts/react-components/ClonarWbs.js"],
    items: ["./Scripts/react-components/CreateTreeItems.js"],
    cloneWbs: [
      "./Scripts/react-components/CloneWbs.js",
      "./Scripts/react-components/Notificacion.js",
      "./Scripts/react-components/OfertaCambiarEstado.js",
      "./Scripts/react-components/OfertaIncluirObservacion.js",
    ],
    detalle_avance_obra: ["./Scripts/react-components/CreateDetalleAvance.js"],
    avance_obra: [
      "./Scripts/react-components/AvanceObra.js",
      "./Scripts/react-components/SubirExcelEAC.js",
      "./Scripts/react-components/CargaMasivaAvanceObra.js",
      "./Scripts/react-components/AvanceObra/ArchivosList.js",
    ],
    cargaIdsRDO: ["./Scripts/react-components/SubirExcelIDRUBRO.js"],
    //disruptivos:"./Scripts/react-components/ObraDisruptivo.js",
    //obra_adicional: ["./Scripts/react-components/ObraAdicional.js"],
    seleccion_oferta: [
      "./Scripts/react-components/selectOferta.js",
      "./Scripts/react-components/DetalleAvanceIngenieria/UploadAvance.js",
    ],
    rdo_cabecera: ["./Scripts/react-components/RdoCabecera.js"],
    rso_cabecera: ["./Scripts/react-components/RsoCabecera.js"],
    computo: ["./Scripts/react-components/CreateTreeComputos.js"],
    computo_: ["./Scripts/react-components/ComputoRDO.js"],
    lista_de_distribucion: [
      "./Scripts/react-components/ListaDeDistribucion.js",
    ],
    notificacion: ["./Scripts/react-components/Notificacion.js"],
    avance_ingenieria: ["./Scripts/react-components/AvanceIngenieria.js"],
    seguimiento_oferta: ["./Scripts/react-components/SeguimientoOferta.js"],
    arbolzonafrente: ["./Scripts/react-components/ArbolZonasFrentes.js"],
    create_avance_obra: ["./Scripts/react-components/AvanceDeObra.js"],
    DetalleAvanceProcura: [
      "./Scripts/react-components/DetalleAvanceProcura.js",
    ],
    oferta_chage_state: ["./Scripts/react-components/OfertaCambiarEstado.js"],
    oferta_include: ["./Scripts/react-components/OfertaIncluirObservacion.js"],
    actualizar_wbs: ["./Scripts/react-components/ActualizacionWbs.js"],
    subir_factura: ["./Scripts/react-components/subir_factura.js"],
    modificacion_presupuesto: [
      "./Scripts/react-components/ModificacionPresupuesto.js",
    ],
    crearPresupuesto: ["./Scripts/react-components/CreatePresupuesto.js"],
    Certificado: ["./Scripts/react-components/Certificado.js"],
    clonarpresupuesto: ["./Scripts/react-components/ClonarPresupuesto.js"],
    colaboradores: ["./Scripts/react-components/Colaboradores.js"],
    crear_colaborador: ["./Scripts/react-components/CrearColaborador.js"],
    editar_colaborador: ["./Scripts/react-components/EditarColaborador.js"],
    servicios_colaborador: [
      "./Scripts/react-components/Colaboradores/ServiciosColaborador.js",
    ],
    crear_servicios_colaborador: [
      "./Scripts/react-components/Colaboradores/CrearServiciosColaborador.js",
    ],

    reingreso_colaborador: ["./Scripts/react-components/ColaboradorReingreso.js"],
    reingreso_historico: ["./Scripts/react-components/ReingresosColaboradores/ColaboradoresHistoricos.js"],
    computos_presupuesto: ["./Scripts/react-components/ComputoPresupuesto.js"],
    //presupuestos: ["./Scripts/react-components/Presupuestos.js"],
    requerimientos: ["./Scripts/react-components/Requerimientos.js"],

    crear_requisito_colaborador: [
      "./Scripts/react-components/CrearRequisitoColaborador.js",
    ],
    editar_requisito_colaborador: [
      "./Scripts/react-components/EditarRequisitoColaborador.js",
    ],
    requisitos_colaborador: [
      "./Scripts/react-components/RequisitosColaborador.js",
    ],
    presupuestos: ["./Scripts/react-components/Presupuestos.js"],
    detalle_presupuesto: ["./Scripts/react-components/DetallePresupuesto.js"],
    wbs_presupuestos: [
      "./Scripts/react-components/WbsPresupuesto.js",
      "./Scripts/react-components/WbsPresupuestoDragDrop.js",
      "./Scripts/react-components/WbsPresupuesto/SubirExcelFechas.js",
    ],

    Wbs2: [
      "./Scripts/react-components/WbsDragDrop.js",
      "./Scripts/react-components/Wbs2.js",
      "./Scripts/react-components/JerarquiaWbs.js",
      "./Scripts/react-components/ClonarWbs.js",
    ],
    cargafactura: ["./Scripts/react-components/CargaFactura.js"],
    cargacobro: ["./Scripts/react-components/CargaCobroFactura.js"],
    vercobros: ["./Scripts/react-components/VerCobros.js"],
    verfacturas: ["./Scripts/react-components/VerFacturas.js"],
    vercobrofactura: ["./Scripts/react-components/VerCobrosFactura.js"],
    verfacturacobro: ["./Scripts/react-components/FacturasCobros.js"],
    horarios: ["./Scripts/react-components/Horarios.js"],
    crear_horario: ["./Scripts/react-components/CrearHorario.js"],
    editar_horario: ["./Scripts/react-components/EditarHorario.js"],
    requisitos_servicio: ["./Scripts/react-components/RequisitosServicio.js"],
    crear_requisito_servicio: [
      "./Scripts/react-components/CrearRequisitoServicio.js",
    ],
    editar_requisito_servicio: [
      "./Scripts/react-components/EditarRequisitoServicio.js",
    ],
    rotaciones: ["./Scripts/react-components/Rotaciones.js"],
    crear_rotacion: ["./Scripts/react-components/CrearRotacion.js"],
    editar_rotacion: ["./Scripts/react-components/EditarRotacion.js"],
    requisitos: ["./Scripts/react-components/Requisitos.js"],
    crear_requisitos: ["./Scripts/react-components/CrearRequisitos.js"],
    editar_requisitos: ["./Scripts/react-components/EditarRequisitos.js"],
    servicios_destino: ["./Scripts/react-components/ServiciosDestino.js"],
    crear_servicios_destino: [
      "./Scripts/react-components/CrearServiciosDestino.js",
    ],
    editar_servicios_destino: [
      "./Scripts/react-components/EditarServiciosDestino.js",
    ],

    servicios: ["./Scripts/react-components/Servicios.js"],
    cumplimiento_requisito: [
      "./Scripts/react-components/CumplimientoRequisitos/CrearCumplimientoRequisitos.js",
    ],
    cumplimiento_requisito_table: [
      "./Scripts/react-components/CumplimientoRequisitos/CumplimientoRequisitosTable.js",
    ],
    colaboradores_bajas: ["./Scripts/react-components/ColaboradorBajas.js"],
    crear_colaboradores_bajas: [
      "./Scripts/react-components/CrearColaboradorBajas.js",
    ],

    //Req. Campusoft
    proveedores: ["./Scripts/react-components/Proveedor/ProveedorContainer.js"],
    proveedorDetalle: [
      "./Scripts/react-components/Proveedor/ProveedorDetalleContainer.js",
    ],

    // Cargos - Sector y Categorias de Encargado de Personal
    cargosSector: ["./Scripts/react-components/CargosSector.js"],
    crear_cargos_sector: ["./Scripts/react-components/CrearCargosSector.js"],
    editar_cargos_sector: ["./Scripts/react-components/EditarCargosSector.js"],
    categoriasEncargado: ["./Scripts/react-components/CategoriasEncargado.js"],
    crear_categorias_encargado: [
      "./Scripts/react-components/CrearCategoriasEncargado.js",
    ],
    editar_categorias_encargado: [
      "./Scripts/react-components/EditarCategoriasEncargado.js",
    ],
    proveedorContrato: [
      "./Scripts/react-components/Proveedor/ProveedorContactoForm.js",
    ],
    proveedorMenu: [
      "./Scripts/react-components/Proveedor/ProveedorMenuContainer.js",
    ],
    proveedorMenuDetalle: [
      "./Scripts/react-components/Proveedor/ProveedorMenuDetalleContainer.js",
    ],
    solicitudVianda: [
      "./Scripts/react-components/SolicitudVianda/SolicitudViandaContainer.js",
    ],
    distribucionVianda: [
      "./Scripts/react-components/DistribucionVianda/DistribucionViandaContainer.js",
    ],
    distribucionViandaEdit: [
      "./Scripts/react-components/DistribucionVianda/DistribucionViandaEditContainer.js",
    ],
    distribucionViandaVer: [
      "./Scripts/react-components/DistribucionVianda/DistribucionViandaVerContainer.js",
    ],
    tipoAccion: [
      "./Scripts/react-components/TipoAccionEmpresa/TipoAccionEmpresaContainer.js",
    ],
    distribucionTransporte: [
      "./Scripts/react-components/DistribucionVianda/DistribucionTransporteContainer.js",
    ],
    conciliacion: [
      "./Scripts/react-components/ConsumoVianda/ConciliacionContainer.js",
    ],
    justificarViandas: [
      "./Scripts/react-components/ConsumoVianda/JustificacionViandasContainer.js",
    ],

    colaboradoresAusentismo: [
      "./Scripts/react-components/ColaboradoresAusentismo.js",
    ],
    Ausentismoreintegro: [
      "./Scripts/react-components/ColaboradoresAusentismo/RegistrarReintegro.js",
    ],
    crear_huellas: [
      "./Scripts/react-components/Colaboradores/HuellaDigital.js",
    ],
    crear_codigo_qr: [
      "./Scripts/react-components/Colaboradores/CrearCodigoQR.js",
    ],

    //Scripts para Pruebas
    pruebas: ["./Scripts/react-components/pruebas.js"],
    enviarrequ: ["./Scripts/react-components/EnviarRequerimiento.js"],
    //OfertasComerciales
    OfertasComerciales: ["./Scripts/react-components/OfertasComerciales.js"],
    detalle_oferta_comercial: [
      "./Scripts/react-components/DetallesOfertasComerciales.js",
    ],
    detalleOferta_: [
      "./Scripts/react-components/OfertaComercialDetalle/DetalleOferta.js",
    ],
    historicos_curva: [
      "./Scripts/react-components/CurvaRDO/HistoricosCurva.js",
    ],
    new_colaboradores_qr: [
      "./Scripts/react-components/NuevoQRColaboradores/ColaboradoresQr.js",
    ],


    //detall orden//
    detalleorden: ["./Scripts/react-components/DetalleOrdenServicio.js"],

    //reportes

    reporte: ["./Scripts/react-components/Reportes.js"],
    base_rdo_presupuesto: ["./Scripts/react-components/RdoPresupuesto.js"],

    //Consumo de WEB SERVICE REGISTRO CIVIL
    ws_registro_civil: ["./Scripts/react-components/ConsumoWSRegistroCivil.js"],
    //
    certificado_ingenieria: [
      "./Scripts/react-components/Certificados/Ingenieria.js",
    ],
    //Transmital
    migracion_contratos: [
      "./Scripts/react-components/Migracion/MigracionContratos.js",
    ],

    transmittal: ["./Scripts/react-components/Transmittal/TransmitalList.js"],
    transmittal_users: [
      "./Scripts/react-components/Transmittal/TransmitalUserList.js",
    ],
    cartas: ["./Scripts/react-components/Cartas/CartasPrincipal.js"],

    detalle_cartas: [
      "./Scripts/react-components/Transmittal/DetalleCartaList.js",
    ],
    details_transmittal: [
      "./Scripts/react-components/Transmittal/DetalleTransmital.js",
    ],
    //Requerimientos en Cola
    requerimientos_cola: ["./Scripts/react-components/RequerimientosCola.js"],
    presupuestos_liberados: [
      "./Scripts/react-components/PresupuestosLiberados.js",
    ],
    colaboradores_ingenieria: [
      "./Scripts/react-components/Colaboradores_Ingenieria.js",
    ],

    // Hospedaje
    proveedores__hospedaje_table: [
      "./Scripts/react-components/Hospedaje/ProveedoresHospedajeTable.js",
    ],
    detalle_habitacion: [
      "./Scripts/react-components/Hospedaje/Habitacion/HabitacionDetalleContainer.js",
    ],
    tarifas_hoteles: [
      "./Scripts/react-components/Hospedaje/TarifasHoteles/TarifaHotelContainer.js",
    ],
    tarifas_lavanderia: [
      "./Scripts/react-components/Hospedaje/TarifasLavanderia/TarifaLavanderiaContainer.jsx",
    ],
    // Reservas Hoteles
    crear_reservas: [
      "./Scripts/react-components/ReservasHoteles/CrearReservasContainer.js",
    ],
    crear_reservas_extemporaneas: [
      "./Scripts/react-components/ReservasHotelesExtemporaneas/CrearReservasExtemporaneasContainer.js",
    ],
    gestion_reservas: [
      "./Scripts/react-components/ReservasHoteles/VerReservas/ReservasContainer.js",
    ],
    gestion_reservas_extemporaneas: [
      "./Scripts/react-components/ReservasHotelesExtemporaneas/VerReservas/ReservasContainer.js",
    ],

    panel_control_reservas: [
      "./Scripts/react-components/ReservasHoteles/PanelControl/PanelControlContainer.js",
    ],
    // Consultar Publica
    create_consulta_publica: [
      "./Scripts/react-components/Accesos/ConsultaPublica/CreateConsultaPublicaContainer.js",
    ],
    //Tarjetas
    buscar_colaborador: [
      "./Scripts/react-components/Accesos/ValidacionRequisitos/BuscarColaborador/BuscarColaboradorContainer.js",
    ],
    gestion_tarjetas_requisitos: [
      "./Scripts/react-components/Accesos/GestionTarjetas/TarjetasRequisitos/TarjetasRequisitosContainer.js",
    ],
    //Vehiculos
    gestion_vehiculos: [
      "./Scripts/react-components/Transportes/Vehiculos/VehiculosContainer.js",
    ],
    //Lugares
    gestion_lugares: [
      "./Scripts/react-components/Transportes/Lugares/LugaresContainer.js",
    ],
    //Paradas
    gestion_paradas: [
      "./Scripts/react-components/Transportes/Paradas/ParadasContainer.js",
    ],
    //Requisitos
    validacion_requisitos_container: [
      "./Scripts/react-components/Accesos/ValidacionRequisitos/Requisitos/RequisitosContainer.js",
    ],

    requisitos_reportes: [
      "./Scripts/react-components/Accesos/ValidacionRequisitos/Requisitos/RequisitosReporte.js",
    ],

    // Usuarios Externos
    usuarios_externos: ["./Scripts/react-components/UsuarioExterno.js"],

    // Módulo de Transportes
    transporte_choferes: [
      "./Scripts/react-components/Transportes/Choferes/ChoferesList.js",
    ],
    transporte_rutas: [
      "./Scripts/react-components/Transportes/Rutas/RutasList.js",
    ],
    transporte_rutas_paradas: [
      "./Scripts/react-components/Transportes/Rutas/RutaContainer.js",
    ],
    transporte_vehiculos_rutas: [
      "./Scripts/react-components/Transportes/VehiculosRutas/VehiculosRutas.js",
    ],
    transporte_colaboradores_rutas: [
      "./Scripts/react-components/Transportes/ColaboradoresRutas/ColaboradorRutaList.js",
    ],
    transporte_busqueda_colaboradores_rutas: [
      "./Scripts/react-components/Transportes/ColaboradoresRutas/BuscarColaboradorContainer.js",
    ],
    transporte_reportes: [
      "./Scripts/react-components/Transportes/Reportes/PersonasTransportadas.js",
    ],

    consumo_reporte: [
      "./Scripts/react-components/ReporteConsumo/ConsumoReporte.js",
    ],
    consumo_reporteExcel: [
      "./Scripts/react-components/ReporteConsumo/ConsumoReporteExcel.js",
    ],
    reporteSincronizacion: [
      "./Scripts/react-components/ReporteConsumo/ConsumoSincronizacion.js",
    ],
    hospedaje_reporteExcel: [
      "./Scripts/react-components/ReporteConsumo/Hospedaje.js",
    ],
    vianda_reporte: [
      "./Scripts/react-components/ReporteConsumo/ConsumoReporteVianda.js",
    ],
    new_certificados: [
      "./Scripts/react-components/CertificadoConstruccion/CertificadoContruccion.js",
    ],
    new_cabecera: [
      "./Scripts/react-components/CertificadoConstruccion/CertificadoCabecera.js",
    ],
    reportes_contratos: [
      "./Scripts/react-components/OfertasComerciales/Reportes.js",
    ],
    //Liquidacion Servicios
    liquidacion_servicios: [
      "./Scripts/react-components/LiquidacionServicios/LiquidacionServicioContainer.js",
    ],
    liquidacion_servicios_main: [
      "./Scripts/react-components/LiquidacionServicios/LiquidacionServicioList.js",
    ],
    liquidacion_servicios_details: [
      "./Scripts/react-components/LiquidacionServicios/LiquidacionServicioDetails.js",
    ],

    //Reportes de Colaboradores
    gestion_reportes: [
      "./Scripts/react-components/Colaboradores/Reportes/ReportesContainer.js",
    ],

    //Comentarios Proyectos
    comentarios_proyecto: [
      "./Scripts/react-components/ComentariosProyecto/ComentariosList.js",
    ],
    buscar_colaborador_tarjeta: [
      "./Scripts/react-components/Accesos/GestionTarjetas/ColaboradoresList/BuscarColaboradorContainer.js",
    ],
    huella_accesos: [
      "./Scripts/react-components/Accesos/Huella/Colaboradores.js",
    ],
    captura_huella_accesos: [
      "./Scripts/react-components/Accesos/Huella/HuellaDigital.js",
    ],

    //Nuevo Formato Items
    items_new: ["./Scripts/react-components/NuevoFormatoItems/Items.js"],

    // ConsumosExtemporaneos
    consumos_extemporaneos_container: [
      "./Scripts/react-components/ConsumosExtemporaneos/ConsumosExtemporaneosContainer.js",
    ],

    /* New Colaboradores Servicios */
    services_colaboradores: [
      "./Scripts/react-components/Colaboradores/ColaboradoresServices.js",
    ],
    /* New PoS */
    pos: ["./Scripts/react-components/Pos/PosList.js"],

    asig_req_usuarios: ["./Scripts/react-components/Accesos/Asignacion/Asignacion.js"],

    /* Alerta de Vencimiento*/
    alerta_vencimiento: ["./Scripts/react-components/Accesos/ValidacionRequisitos/AlertaVencimientos/AlertaVencimientoContainer.js"],

    /** Capacitaciones */
    capacitaciones_colaboradores_listado: ["./Scripts/react-components/CapacitacionesColaboradores/ListadoColaboradores/ListadoColaboradoresContainer.jsx"],
    detalle_capacitaciones: ["./Scripts/react-components/CapacitacionesColaboradores/DetalleCapacitaciones/DetalleCapacitacionesContainer.jsx"],

    /** Actualizacion Sueldos */
    actualizacion_sueldos: ["./Scripts/react-components/ActualizacionesMasivasRRHH/ActualizacionSueldos/ActualizacionSueldosContainer.jsx"],
    /** Actualizacion Colaboradores */
    actualizacion_masiva_colaboradores: ["./Scripts/react-components/ActualizacionesMasivasRRHH/ActualizacionColaboradores/ActualizacionColaboradoresContainer.jsx"],

    /* Horarios Tipos de Comida */
    horarioTipoComida: ["./Scripts/react-components/HorariosTipoOpcionComida/ContainerHorario.js"],

    /* Problemas Sincronizacion*/
    problema_sincronizacion_container: ["./Scripts/react-components/Seguridades/ProblemasSincronizacion/ProblemaSincronizacionContainer.jsx"],

    /* Gestion de Carpetas */
    gestion_carpetas_container: ["./Scripts/react-components/GestionDocumentos/Carpetas/CarpetasContainer.jsx"],
    /* Gestion de Usuarios Autorizados */
    gestion_usuarios_autorizados_contratos: ["./Scripts/react-components/GestionDocumentos/UsuariosAutorizados/UsuariosAutorizadosContainer.jsx"],
    /* Gestion de documentos */
    gestion_documentos_contratos: ["./Scripts/react-components/GestionDocumentos/Documentos/DocumentosContainer.jsx"],
    /* Gestion de secciones */
    gestion_secciones_container: ["./Scripts/react-components/GestionDocumentos/Secciones/SeccionesContainer.jsx"],
    /* Consulta de Documentos Listado */
    listado_contratos_container: ["./Scripts/react-components/GestionDocumentos/Consultas/ListadoContratosContainer.jsx"],
    consultas_contrato_container: ["./Scripts/react-components/ConsultaContratoContainer.js"],
    reporteDocumentos: ["./Scripts/react-components/GestionDocumentos/Reportes.js"],
    /* Certificacion Ingenieria */
    colaborador_rubro_container: ["./Scripts/react-components/Certificacioningenieria/ColaboradorRubro/ColaboradorRubroContainer.jsx"],
    detalles_directos_ingenieria_container: ["./Scripts/react-components/Certificacioningenieria/DetallesDirectosIngenieria/DetallesDirectosIngenieraContainer.jsx"],
    /* Feriados */
    dias_feriado_container: ["./Scripts/react-components/Certificacioningenieria/AdministracionFeriados/FeriadosContainer.jsx"],
    /* Detalle Indirectos Ingenieria*/
    detalles_indirectos_ingenieria_container: ["./Scripts/react-components/Certificacioningenieria/DetallesIndirectosIngenieria/DetallesIndirectosIngenieriaContainer.jsx"],
    colaborador_certificacion_ingenieria_container: ["./Scripts/react-components/Certificacioningenieria/CategorizacionColaboradores/ColaboradoresIngenieriaContainer.jsx"],
    /* Nueva Certificacion Ingenieria */
    avance_porcentaje_proyecto_container: ["./Scripts/react-components/Certificacioningenieria/AvancePorcentajeProyecto/AvancePorcentajeProyectoContainer.jsx"],
    certificacion_ingenieria: ["./Scripts/react-components/Certificacioningenieria/Certificacion/CertificacionContainer.jsx"],
    porcentaje_avance_ingenieria: ["./Scripts/react-components/Certificacioningenieria/AvanceIngenieria/AvanceIngenieriaContainer.jsx"],
    planificacion_timesheet_container: ["./Scripts/react-components/Certificacioningenieria/PlanificacionTimesheet/PlanificacionTimesheetContainer.jsx"],
    parametros_sistema_container: ["./Scripts/react-components/Certificacioningenieria/ParametrosSistema/ParametrosSistemaContainer.jsx"],
    redistribucion_proyectos:["./Scripts/react-components/Certificacioningenieria/RedistribucionProyectos/RedistribucionProyectosContainer.jsx"],

    parametrizacion_proyectos_certificacion:  ["./Scripts/react-components/Certificacioningenieria/ParametrizacionProyectos/ParametrizacionProyectosContainer.jsx"],
    paciente: ["./Scripts/react-components/Pacientes/PacienteContainer.js"],
  },

  output: {
    path: path.resolve(__dirname, "./Scripts/build"),
    filename: "[name].js",
  },
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        exclude: /(node_modules|bower_components)/,
        use: {
          loader: "babel-loader",
          options: {
            cacheDirectory: true,
            presets: ["@babel/preset-env"],
            plugins: ["transform-class-properties"],
          },
        },
      },
      {
        test: /\.css$/,
        use: ["css-loader"],
      },
    ],
  },

  plugins: [
    //new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
    new webpack.ContextReplacementPlugin(/moment[/\\]locale$/, /es/),
  ],

  optimization: {
    splitChunks: {
      cacheGroups: {
        // Split vendor code to its own chunk(s)
        commons: {
          test: /[\\/]node_modules[\\/]/,
          name: "vendors",
          chunks: "all",
        },
      },
    },
  },

  /*
  optimization: {
       // Automatically split vendor and commons
      // https://twitter.com/wSokra/status/969633336732905474
      // https://medium.com/webpack/webpack-4-code-splitting-chunk-graph-and-the-splitchunks-optimization-be739a861366
      splitChunks: {
          chunks: 'all',
          name: false,
      },
      // Keep the runtime chunk seperated to enable long term caching
      // https://twitter.com/wSokra/status/969679223278505985
      runtimeChunk: true,
  },
  */

  /*
    optimization: {
        splitChunks: {
            cacheGroups: {
                // Split vendor code to its own chunk(s)
                vendors: {
                    test: /[\\/]node_modules[\\/]/i,
                    chunks: "all",
                    minSize: 30000
                },
                // Split code common to all chunks to its own chunk
                commons: {
                    name: "commons",    // The name of the chunk containing all common code
                    chunks: "initial",  // TODO: Document
                    minChunks: 2        // This is the number of modules
                }
            }
        },
        // The runtime should be in its own chunk
        runtimeChunk: {
            name: "runtime"
        }

    }*/

  /*
    // Excluir librerias de los empaquetados, para incluir dichas librerias con CDN
    //https://webpack.js.org/configuration/externals/
    externals: {
        react: {
            root: 'React',
            commonjs2: 'react',
            commonjs: 'react',
            amd: 'react',
            umd: 'react',
        },
        'react-dom': {
            root: 'ReactDOM',
            commonjs2: 'react-dom',
            commonjs: 'react-dom',
            amd: 'react-dom',
            umd: 'react-dom',
        },
    },*/
};
