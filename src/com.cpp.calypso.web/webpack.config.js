const path = require('path');

module.exports = {
    mode: 'production',
    entry: {
        // // wbs: ["./Scripts/react-components/CreateWbs.js", "./Scripts/react-components/JerarquiaWbs.js"],
        // wbs: ["./Scripts/react-components/Wbs.js", "./Scripts/react-components/JerarquiaWbs.js", "./Scripts/react-components/ClonarWbs.js"],
        // items: ["./Scripts/react-components/CreateTreeItems.js"],
        // cloneWbs: ["./Scripts/react-components/CloneWbs.js",
        //     "./Scripts/react-components/Notificacion.js",
        //     "./Scripts/react-components/OfertaCambiarEstado.js",
        //     "./Scripts/react-components/OfertaIncluirObservacion.js"],
        // detalle_avance_obra: ["./Scripts/react-components/CreateDetalleAvance.js"],
        // avance_obra: ["./Scripts/react-components/ObraDisruptivo.js", "./Scripts/react-components/AvanceObra.js"],
        // //obra_adicional: ["./Scripts/react-components/ObraAdicional.js"],
        // seleccion_oferta: ["./Scripts/react-components/selectOferta.js"],
        // rdo_cabecera: ["./Scripts/react-components/RdoCabecera.js"],
        // computo: ["./Scripts/react-components/CreateTreeComputos.js"],
        // lista_de_distribucion: ["./Scripts/react-components/ListaDeDistribucion.js"],
        // //notificacion: ["./Scripts/react-components/Notificacion.js"],
        // avance_ingenieria: ["./Scripts/react-components/AvanceIngenieria.js"],
        // seguimiento_oferta: ["./Scripts/react-components/SeguimientoOferta.js"],
        //  arbolzonafrente: ["./Scripts/react-components/ArbolZonasFrentes.js"],
        // create_avance_obra: ["./Scripts/react-components/AvanceDeObra.js"],
        // DetalleAvanceProcura:["./Scripts/react-components/DetalleAvanceProcura.js"],
        // oferta_chage_state:["./Scripts/react-components/OfertaCambiarEstado.js"],
        // oferta_include:["./Scripts/react-components/OfertaIncluirObservacion.js"],
        // wbs_vista:["./Scripts/react-components/WbsVista.js"],
        // subir_factura:["./Scripts/react-components/subir_factura.js"],
        // modificacion_presupuesto: ["./Scripts/react-components/ModificacionPresupuesto.js"],
        // // crearPresupuesto: ["./Scripts/react-components/CreatePresupuesto.js"],
        // // Certificado: ["./Scripts/react-components/Certificado.js"],
        // colaboradores: ["./Scripts/react-components/Colaboradores.js"],
        // crear_colaborador: ["./Scripts/react-components/CrearColaborador.js"],
        //  editar_colaborador: ["./Scripts/react-components/EditarColaborador.js"],
        crear_requisito_colaborador: ["./Scripts/react-components/CrearRequisitoColaborador.js"],
        editar_requisito_colaborador: ["./Scripts/react-components/EditarRequisitoColaborador.js"],
        requisitos_colaborador: ["./Scripts/react-components/RequisitosColaborador.js"],
        // horarios: ["./Scripts/react-components/Horarios.js"],
        // crear_horario: ["./Scripts/react-components/CrearHorario.js"],
        // editar_horario: ["./Scripts/react-components/EditarHorario.js"],
        requisitos_servicio: ["./Scripts/react-components/RequisitosServicio.js"],
        crear_requisito_servicio: ["./Scripts/react-components/CrearRequisitoServicio.js"],
        editar_requisito_servicio: ["./Scripts/react-components/EditarRequisitoServicio.js"],
        // rotaciones: ["./Scripts/react-components/Rotaciones.js"],
        // crear_rotacion: ["./Scripts/react-components/CrearRotacion.js"],
        // editar_rotacion: ["./Scripts/react-components/EditarRotacion.js"],
        requisitos: ["./Scripts/react-components/Requisitos.js"],

    },
    output: {
        path: path.resolve(__dirname, './Scripts/build'),
        filename: '[name].js'
    },
        module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /(node_modules|bwer_components)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['babel-preset-react'],
                        plugins: ['transform-class-properties']
                    }
                }
            },
            {
                test: /\.css$/,
                use: [ 'css-loader' ]
            }
        ]
    },

}