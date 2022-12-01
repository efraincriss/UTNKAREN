import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { ScrollPanel } from "primereact-v2/scrollpanel";
import React from 'react';
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapper";
import config from "../../Base/Config";
import http from "../../Base/HttpService";
import {
    CONTROLLER_CAPACITACIONES,
    FRASE_CAPACITACION_ELIMINADA,
    FRASE_ERROR_GLOBAL, MODULO_RECURSOS_HUMANOS
} from "../../Base/Strings";
import { CapacitacionForm } from './CapacitacionForm.jsx';
import { DetalleCapacitacionesTable } from './DetalleCapacitacionesTable.jsx';
import { ResumenCapacitacionTable } from './ResumenCapacitacionTable.jsx';
import { Button } from 'primereact-v2/button';

class DetalleCapacitacionesContainer extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            colaboradorId: 0,
            colaborador: {},
            detalleCapacitaciones: [],
            resumenCapacitaciones: [],
            mostrarFormulario: false,
            mostrarConfirmacion: false,
            capacitacionSeleccionada: {},
            capacitacionAEliminar: 0,
        }
    }

    componentWillMount() {
        this.setState({
            colaboradorId: this.props.getParameterByName('colaboradorId')
        })
    }

    componentDidMount() {
        this.loadData();
    }

    render() {
        return (
            <div>
                <div className="row">
                    <div style={{ width: '100%' }}>
                        <div className="card">
                            <div className="card-body">
                                <div className="row" align="right">
                                    <div className="col">

                                        <button
                                            style={{ marginLeft: '0.3em' }}
                                            className="btn btn-outline-primary"
                                            onClick={() => this.redireccionar("REGRESAR", 0)}
                                        >Regresar</button>
                                    </div>
                                </div>
                                <hr />

                                <div className="row">
                                    <div className="col-xs-12 col-md-6">
                                        <h6 className="text-gray-700"><b>Código SAp:</b> {this.state.colaborador.CodigoSap ? this.state.colaborador.CodigoSap : ""}</h6>
                                        <h6 className="text-gray-700"><b>Nombres-Apellidos:</b>  {this.state.colaborador.NombresColaborador ? this.state.colaborador.NombresColaborador : ""}</h6>
                                    </div>

                                    <div className="col-xs-12 col-md-6">
                                        <h6 className="text-gray-700"><b>Identificación:</b> {this.state.colaborador.NumeroIdentificacion ? this.state.colaborador.NumeroIdentificacion : ""}</h6>
                                        <h6 className="text-gray-700"><b>Estado:</b> {this.state.colaborador.Estado ? this.state.colaborador.Estado : ""}</h6>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="row">
                    <div style={{ width: '100%' }}>
                        <ul className="nav nav-tabs" id="capacitaciones-tab" role="tablist">
                            <li className="nav-item">
                                <a className="nav-link active" id="capacitaciones-tab" data-toggle="tab" href="#capacitaciones" role="tab" aria-controls="home" aria-expanded="true">Detalle Capacitaciones</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" id="resumen-tab" data-toggle="tab" href="#resumen" role="tab" aria-controls="resumen">Resumen Capacitaciones</a>
                            </li>
                        </ul>

                        <div className="tab-content" id="myTabContent">
                            <div className="tab-pane fade show active" id="capacitaciones" role="tabpanel" aria-labelledby="capacitaciones-tab">
                                <div className="row" align="right">
                                    <div className="col">
                                        <button
                                            style={{ marginLeft: '0.3em' }}
                                            className="btn btn-outline-primary"
                                            onClick={() => this.nuevaCapacitacion()}
                                        >
                                            Agregar
                                        </button>
                                    </div>
                                </div>
                                <DetalleCapacitacionesTable
                                    data={this.state.detalleCapacitaciones}
                                    mostrarEdicionDeCapacitacion={this.mostrarEdicionDeCapacitacion}
                                    eliminarCapacitacion={this.mostrarEliminacionCapacitacion}
                                />
                            </div>

                            <div className="tab-pane fade" id="resumen" role="tabpanel" aria-labelledby="resumen-tab">
                                <ResumenCapacitacionTable
                                    data={this.state.resumenCapacitaciones}
                                />
                            </div>

                        </div>
                    </div>
                </div>
                <Dialog
                    header="Gestión de Capacitaciones"
                    modal={true}
                    visible={this.state.mostrarFormulario}
                    style={{ width: "750px" }}
                    onHide={this.onHideFormulario}
                >
                    <CapacitacionForm
                        onHideFormulario={this.onHideFormulario}
                        capacitacion={this.state.capacitacionSeleccionada}
                        showSuccess={this.props.showSuccess}
                        showWarn={this.props.showWarn}
                        blockScreen={this.props.blockScreen}
                        unlockScreen={this.props.unlockScreen}
                        ocultarUploadFile={this.ocultarUploadFile}
                        catalogoNombreCapacitaciones={this.state.catalogoNombreCapacitaciones}
                        catalogoTipoCapacitaciones={this.state.catalogoTipoCapacitaciones}
                        actualizarCapacitacionSeleccionada={this.actualizarCapacitacionSeleccionada}

                    />

                </Dialog>

                <Dialog header="Confirmación"
                    visible={this.state.mostrarConfirmacion}
                    modal style={{ width: '400px' }}
                    footer={this.construirBotonesDeConfirmacion()}
                    onHide={this.onHideConfirmacion}
                >
                    <div className="confirmation-content">
                        <i className="pi pi-exclamation-triangle p-mr-3" style={{ fontSize: '2rem' }} />
                        <div className="p-12">
                            <h4>¿Estás seguro de eliminar el registro?</h4>
                        </div>
                    </div>
                </Dialog>
            </div>
        )
    }

    loadData = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.obtenerDatosDeCapacitaciones()])
            .then(function ([response]) {

                let data = response.data;
                if (data.success === true) {
                    let catalogoNombreCapacitaciones = self.props.buildDropdown(data.result.CatalogoNombreCapacitacion, 'nombre');
                    let catalogoTipoCapacitaciones = self.props.buildDropdown(data.result.CatalogoTipoCapacitacion, 'nombre');
                    self.setState({
                        colaborador: {
                            NombresColaborador: data.result.NombresColaborador,
                            NumeroIdentificacion: data.result.NumeroIdentificacion,
                            CodigoSap: data.result.CodigoSap,
                            Estado: data.result.Estado,
                        },
                        detalleCapacitaciones: data.result.DetalleCapacitaciones,
                        resumenCapacitaciones: data.result.ResumenCapacitaciones,
                        catalogoNombreCapacitaciones,
                        catalogoTipoCapacitaciones
                    })
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    console.log(message);
                    this.props.showWarn("Ocurrió un error, intentalo más tarde.");
                }

                self.props.unlockScreen();
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });

    }

    obtenerDatosDeCapacitaciones = () => {
        let url = '';
        url = `/RRHH/Capacitacion/ObtenerCapacitacionesPorColaborador/${this.state.colaboradorId}`;
        console.log(url)
        return http.get(url);
    }

    redireccionar = (accion, id) => {
        if (accion === "REGRESAR") {
            window.location.href = `/RRHH/Capacitacion`;
        }
    }

    onHideFormulario = (recargar = false) => {
        this.setState({ mostrarFormulario: false, capacitacionSeleccionada: {} });
        if (recargar) {
            this.loadData();
        }
    }

    construirBotonesDeConfirmacion = () => {
        return (
            <div>
                <Button label="Cancelar" icon="pi pi-times" onClick={() => this.onHideConfirmacion()} className="p-button-text" />
                <Button label="Confirmar" icon="pi pi-check" onClick={() => this.eliminarCapacitacion()} autoFocus />
            </div>
        );
    }

    onHideConfirmacion = () => {
        this.setState({ mostrarConfirmacion: false, capacitacionAEliminar: 0 });
    }

    nuevaCapacitacion = () => {
        this.setState({ capacitacionSeleccionada: { ColaboradoresId: this.state.colaboradorId }, mostrarFormulario: true })
    }

    mostrarEliminacionCapacitacion = (capacitacionId) => {
        this.setState({ mostrarConfirmacion: true, capacitacionAEliminar: capacitacionId });
    }

    mostrarEdicionDeCapacitacion = (capacitacion) => {
        this.setState({ capacitacionSeleccionada: capacitacion, mostrarFormulario: true });
    }

    eliminarCapacitacion = () => {
        this.props.blockScreen();
        this.onHideConfirmacion();
        let url = '';
        url = `${config.apiUrl}${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_CAPACITACIONES}/EliminarCapacitacion/${this.state.capacitacionAEliminar}`

        http.post(url, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.props.showSuccess(FRASE_CAPACITACION_ELIMINADA);
                    this.loadData();
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.showWarn(message);
                    this.props.unlockScreen();
                }

            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn(FRASE_ERROR_GLOBAL);
                this.props.unlockScreen();
            });
    }

    actualizarCapacitacionSeleccionada = (capacitacion) =>{
        this.setState({capacitacionSeleccionada: capacitacion})
    }
}


const Container = Wrapper(DetalleCapacitacionesContainer);

ReactDOM.render(
    <Container />,
    document.getElementById('detalle_capacitaciones_por_colaborador')
);

