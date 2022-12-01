import React, { Component } from 'react';
import Wrapper from "../Base/BaseWrapper";
import ReactDOM from 'react-dom';
import { Card, CardBody, CardTitle, CardSubtitle, Button, Modal, ModalBody, ModalHeader } from "shards-react";

import http from "../Base/HttpService";
import Field from "../Base/Field-v2";
import dateFormatter from "../Base/DateFormatter";

import { CONTROLLER_CONSUMO_EXTEMPORANEO, MODULO_PROVEEDOR, CONTROLLER_DETALLE_CONSUMO_EXTEMPORANEO } from "../Base/Strings";
import ConsumosExtemporaneosTable from './ConsumoExtemporaneo/ConsumosTable';
import ConsumoExtemporaneoForm from './ConsumoExtemporaneo/ConsumoExtemporaneoForm';
import DetalleConsumosExtemporaneosTable from './DetallesConsumoExtemporaneo/DetalleConsumoTable';
import BuscarColaboradorConsumo from './DetallesConsumoExtemporaneo/BuscarColaborador';

const PANTALLA_CABECERA = 'Cabecera';
const PANTALLA_DETALLES = 'Detalles';

export default class ConsumosExtemporaneosContainer extends Component {
    constructor(props) {
        super(props);
        this.state = {
            // Pantalla mostrada
            pantalla: 'Cabecera',
            // Consumos extemporaneos mostrados en cabecera
            consumosExtemporaneos: [],
            // Abrir el modal del formulario
            openModal: false,
            // Consumo cargado en el form
            consumo: {},
            // Tipos de Comida del Proveedor
            tiposComidas: [],
            // Key del formulario
            keyForm: 234,
            // Detalles de consumo extemporaneo por cabecera
            detallesConsumos: {},
            //Almacena el consumo extemporaneo seleccionado
            consumoExtemporaneo: {},
            // Abrir el modal de buscar colaborador
            openModalColaborador: false,
        }
    }

    componentDidMount() {
        this.props.unlockScreen();
        this.loadData();
    }

    renderApp = () => {

        if (this.state.pantalla === 'Cabecera') {
            return this.renderConsumosExtemporaneosScreen();
        } else {
            return this.renderDetallesConsumos();
        }
    }


    render() {


        return (
            <div className="row">
                {this.renderApp()}
            </div>
        )
    }

    renderConsumosExtemporaneosScreen = () => {
        const { openModal, consumo } = this.state;
        return (
            <div className="col">
                <Card>
                    <CardBody>
                        <CardTitle>
                            <div className="row">
                                <div className="col">
                                    Consumos Extemporaneos
                                    </div>
                                <div className="col" align="right">
                                    <Button outline
                                        onClick={this.toggle}
                                    >Nuevo</Button>
                                </div>
                            </div>
                        </CardTitle>
                        <CardSubtitle>Listado de consumos</CardSubtitle>


                        <ConsumosExtemporaneosTable
                            data={this.state.consumosExtemporaneos}
                            setConsumo={this.setConsumo}
                            loadDetalles={this.loadDetallesConsumosExtemporaneos}
                        />
                    </CardBody>
                </Card>

                <div>
                    <Modal open={openModal} toggle={this.toggle} centered={true}>
                        <ModalHeader>Nuevo Consumo Extemporaneo</ModalHeader>
                        <ModalBody>
                            <ConsumoExtemporaneoForm
                                key={this.state.keyForm}
                                consumo={consumo}
                                proveedores={this.state.proveedores}
                                tiposComidas={this.state.tiposComidas}
                                loadTiposComidas={this.loadTiposComidas}
                                loadConsumosExtemporaneos={this.loadConsumosExtemporaneos}
                                toggle={this.toggle}

                                showSuccess={this.props.showSuccess}
                                showWarn={this.props.showWarn}
                                blockScreen={this.props.blockScreen}
                                unlockScreen={this.props.unlockScreen}
                            />
                        </ModalBody>
                    </Modal>
                </div>
            </div>
        )
    }

    renderDetallesConsumos = () => {
        const { openModalColaborador } = this.state;
        return (
            <div className="col">
                <Card>
                    <CardBody>
                        <CardTitle>
                            <div className="row">
                                <div className="col">
                                    Detalles Consumos Extemporaneos
                                    </div>
                                <div className="col" align="right">
                                    <Button outline
                                        onClick={() => this.toggleColaborador()}
                                    >Nuevo</Button>

                                    <Button
                                        outline
                                        onClick={() => this.redirect(PANTALLA_CABECERA)}
                                        style={{ marginLeft: '0.3em' }}
                                    >Regresar</Button>
                                </div>
                            </div>
                        </CardTitle>
                        <CardSubtitle>
                            <hr />
                            <div className="row" style={{ marginTop: '0.5em' }}>
                                <div className="col">
                                    <b>Proveedor: </b>{this.state.consumoExtemporaneo.ProveedorNombre}
                                </div>
                                <div className="col">
                                    <b>Fecha: </b>{dateFormatter(this.state.consumoExtemporaneo.Fecha)}
                                </div>
                                <div className="col">
                                    <b>Tipo Comida: </b>{this.state.consumoExtemporaneo.TipoComidaNombre}
                                </div>
                            </div>
                        </CardSubtitle>

                        <DetalleConsumosExtemporaneosTable
                            data={this.state.detallesConsumos}
                            eliminarDetalleConsumo={this.eliminarDetalleConsumo}

                        />

                    </CardBody>
                </Card>
                <div>
                    <Modal size="lg" open={openModalColaborador} toggle={this.toggleColaborador} centered={true}>
                        <ModalHeader>Buscar Colaborador</ModalHeader>
                        <ModalBody>
                            <BuscarColaboradorConsumo
                                blocking={this.props.blocking}
                                showSuccess={this.props.showSuccess}
                                showWarn={this.props.showWarn}
                                blockScreen={this.props.blockScreen}
                                unlockScreen={this.props.unlockScreen}
                                crearDetalleConsumo={this.crearDetalleConsumo}
                            />
                        </ModalBody>
                    </Modal>
                </div>
            </div>
        )
    }

    // ============================= Cabecera ======================
    loadData = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.ObtenerConsumosExtemporaneos(), this.ObtenerProveedoresAlimentacion()])
            .then(function ([consumosExtemporaneos, proveedores]) {


                let proveedoresMapped = self.props.buildDropdown(proveedores.data.result, 'razon_social', 'Id');
                self.setState({
                    consumosExtemporaneos: consumosExtemporaneos.data.result,
                    proveedores: proveedoresMapped
                }, self.props.unlockScreen)
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });
    }

    ObtenerConsumosExtemporaneos = () => {
        let url = '';
        url = `/${MODULO_PROVEEDOR}/${CONTROLLER_CONSUMO_EXTEMPORANEO}/ObtenerConsumosExtemporaneos`;
        return http.get(url);
    }

    ObtenerProveedoresAlimentacion = () => {
        let url = '';
        url = `/${MODULO_PROVEEDOR}/${CONTROLLER_CONSUMO_EXTEMPORANEO}/ObtenerProveedoresAlimentacion`;
        return http.get(url);
    }

    ObtenerTiposComida = (proveedorId) => {
        let url = '';
        url = `/${MODULO_PROVEEDOR}/${CONTROLLER_CONSUMO_EXTEMPORANEO}/ObtenerTiposComida?proveedorId=${proveedorId}`;
        return http.get(url);
    }

    loadTiposComidas = (proveedorId) => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.ObtenerTiposComida(proveedorId)])
            .then(function ([tiposComidas]) {
                let tiposComidasMapped = self.props.buildDropdown(tiposComidas.data.result, 'tipo_comida_nombre', 'tipo_comida_id');
                self.setState({
                    tiposComidas: tiposComidasMapped,
                }, self.props.unlockScreen)
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });
    }

    loadTiposComidasAwait = (proveedorId, consumo) => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.ObtenerTiposComida(proveedorId)])
            .then(function ([tiposComidas]) {
                let tiposComidasMapped = self.props.buildDropdown(tiposComidas.data.result, 'tipo_comida_nombre', 'tipo_comida_id');
                self.setState({
                    tiposComidas: tiposComidasMapped,
                    consumo,
                    keyForm: Math.random()
                }, self.props.unlockScreen)
                self.toggle();
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });
    }

    loadConsumosExtemporaneos = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.ObtenerConsumosExtemporaneos()])
            .then(function ([consumosExtemporaneos]) {
                self.setState({
                    consumosExtemporaneos: consumosExtemporaneos.data.result,
                }, self.props.unlockScreen)
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });
    }

    setConsumo = (consumo) => {
        let proveedor = consumo.ProveedorId;
        this.loadTiposComidasAwait(proveedor, consumo);
    }

    toggle = () => {
        // El modal esta abierto
        if (this.state.openModal) {
            this.setState({
                openModal: !this.state.openModal,
                consumo: {},
                keyForm: Math.random()
            });
        } else {
            this.setState({
                openModal: !this.state.openModal
            });
        }
    }

    // ========================= Fin Cabecera =============================


    // ========================= Detalle Cabecera =========================

    ObtenerDetallesConsumosExtemporaneos = (consumoExtemporaneoId) => {
        let url = '';
        url = `/${MODULO_PROVEEDOR}/${CONTROLLER_DETALLE_CONSUMO_EXTEMPORANEO}/BuscarDetallesPorCabecera?consumoExtemporaneoId=${consumoExtemporaneoId}`;
        return http.get(url);
    }

    loadDetallesConsumosExtemporaneos = (consumoExtemporaneo) => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.ObtenerDetallesConsumosExtemporaneos(consumoExtemporaneo.Id)])
            .then(function ([consumos]) {

                self.setState({
                    detallesConsumos: consumos.data.result,
                    pantalla: PANTALLA_DETALLES,
                    consumoExtemporaneo
                }, self.props.unlockScreen)
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });
    }

    toggleColaborador = () => {
        this.setState({
            openModalColaborador: !this.state.openModalColaborador
        });
    }

    crearDetalleConsumo = (colaboradorId, observaciones) => {
        this.props.blockScreen();

        let url = `/${MODULO_PROVEEDOR}/${CONTROLLER_DETALLE_CONSUMO_EXTEMPORANEO}/CrearDetalleConsumo`;

        let detalleConsumoModel = {
            ConsumoExtemporaneoId: this.state.consumoExtemporaneo.Id,
            ColaboradorId: colaboradorId,
            Observaciones: observaciones,
            Liquidado: false,
        }

        http.post(url, detalleConsumoModel)
            .then(response => {
                let data = response.data;
                console.log(data)
                if (data.success === true) {
                    if (data.created === true) {
                        this.props.showSuccess("Consumo registrado")
                        this.loadDetallesConsumosExtemporaneos(this.state.consumoExtemporaneo);

                    } else {
                        this.props.showWarning(data.result)
                    }
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.showWarn('Ocurrió un error al registrar el consumo');
                this.props.unlockScreen();
            })
    }

    eliminarDetalleConsumo = (detalleConsumoId) => {
        this.props.blockScreen();

        let url = `/${MODULO_PROVEEDOR}/${CONTROLLER_DETALLE_CONSUMO_EXTEMPORANEO}/EliminarDetalleConsumoExtemporaneoAsync`;

        http.post(url,
            {
                detalleConsumo: detalleConsumoId
            }
        )
            .then(response => {
                let data = response.data;
                console.log(data)
                if (data.deleted === true) {

                    this.props.showSuccess("Consumo eliminado")
                    this.loadDetallesConsumosExtemporaneos(this.state.consumoExtemporaneo);
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.showWarn('Ocurrió un error al registrar el consumo');
                this.props.unlockScreen();
            })
    }
    // ========================= Fin Detalle ==============================


    redirect = (pantalla) => {
        this.setState({ pantalla })
    }
}

const Container = Wrapper(ConsumosExtemporaneosContainer);

ReactDOM.render(
    <Container />,
    document.getElementById('consumos_extemporaneos_container')
);