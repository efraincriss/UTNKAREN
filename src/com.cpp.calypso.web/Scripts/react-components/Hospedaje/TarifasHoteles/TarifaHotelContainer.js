import React, { Component } from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapperApi";
import config from '../../Base/Config'
import http from "../../Base/HttpService";
import TarifasTable from "./TarifasTable";
import { Dialog } from 'primereact-v2/dialog';
import CabeceraTarifas from "./CabeceraTarifas";
import TarifasForm from "./TarifasForm";
import {FRASE_ERROR_GLOBAL, FRASE_TARIFA_INHABILITADA, MODULO_PROVEEDOR, FRASE_TARIFA_HABILITADA} from "../../Base/Strings";

class TarifaHotelContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            tarifas: [],
            visible: false,
            contratoProveedorId: 0,
            tarifa: {},
            editable: false
        }
    }

    componentWillMount(){
        let url = window.location.href;
        let contratoProveedorId = url.substr(url.lastIndexOf('/') + 1);
        this.setState({contratoProveedorId});
        this.getTarifasHoteles(contratoProveedorId);
    }


    render() {
        return (
            <div>
                <CabeceraTarifas
                    data={this.props.data}
                    redireccionar={this.redireccionar}
                />


                <div className="row">
                    <div style={{ width: '100%' }}>
                        <div className="card">
                            <div className="card-body">
                                <div className="row" align="right">
                                    <div className="col">
                                        <button
                                            style={{ marginLeft: '0.3em' }}
                                            className="btn btn-outline-primary"
                                            onClick={() => this.mostrarForm(false)}
                                        >Nuevo</button>
                                    </div>
                                </div>

                                <div className="row" style={{marginTop: '1em'}}>
                                    <div className="col">
                                        <TarifasTable
                                            data={this.state.tarifas}
                                            detalleTarifa={this.detalleTarifa}
                                            deleteTarifa={this.deleteTarifa}
                                            activarTarifa={this.activarTarifa}
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <Dialog header="Ingresar Tarifa" visible={this.state.visible} width="500px" modal={true} onHide={this.ocultarForm}>
                    <TarifasForm
                        unlockScreen={this.props.unlockScreen}
                        blockScreen={this.props.blockScreen}
                        showSuccess={this.props.showSuccess}
                        showWarn={this.props.showWarn}
                        tarifa={this.state.tarifa}
                        contratoProveedorId={this.state.contratoProveedorId}
                        getTarifasHoteles={this.getTarifasHoteles}
                        ocultarForm={this.ocultarForm}
                        editable={this.state.editable}
                    />
                </Dialog>
            </div>

        )
    }

    getTarifasHoteles = contratoProveedorId => {
        this.props.blockScreen();
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/TarifaHotel/ListarPorContrato/${contratoProveedorId}`
        http.get(url, {})
            .then((response) => {
                let data = response.data;

                if (data.success === true) {
                    this.setState({ tarifas: data.result });
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    console.log(data)
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                this.props.unlockScreen();
                console.log(error)
            });
    }

    detalleTarifa = id => {
        // 1. Consultar Detalles
        // 2. Mostrar Form
        this.props.blockScreen();
        let url;
        url = `${config.appUrl}${MODULO_PROVEEDOR}/TarifaHotel/DetailsApi/${id}`
        http.get(url,{})
        .then((response) => {
            let data = response.data
            this.setState({tarifa: data.result}, this.mostrarForm(true))
            this.props.unlockScreen();
        })
        .catch((error) => {
            console.log(error)
            this.props.unlockScreen();
        })   
    }


    deleteTarifa = id => {
        this.props.blockScreen();
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/TarifaHotel/DeleteApi/`+id
        http.post(url, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.deleteSuccess();
                } else {
                    this.props.unlockScreen();
                    this.props.showWarn(FRASE_ERROR_GLOBAL)
                    console.log(data.errors)
                }
            })
            .catch((error) => {
                this.props.unlockScreen();
                this.props.showWarn(FRASE_ERROR_GLOBAL)
                console.log(error)
            })
    }

    activarTarifa = id => {
        this.props.blockScreen();
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/TarifaHotel/ActivarTarifaApi/${id}`
        http.post(url, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.activateSuccess();
                } else {
                    this.props.unlockScreen();
                    this.props.showWarn(FRASE_ERROR_GLOBAL)
                    console.log(data.errors)
                }
            })
            .catch((error) => {
                this.props.unlockScreen();
                this.props.showWarn(FRASE_ERROR_GLOBAL)
                console.log(error)
            })
    }

    deleteSuccess = () => {
        this.props.showSuccess(FRASE_TARIFA_INHABILITADA)
        this.getTarifasHoteles(this.state.contratoProveedorId);
    }

    activateSuccess = () => {
        this.props.showSuccess(FRASE_TARIFA_HABILITADA)
        this.getTarifasHoteles(this.state.contratoProveedorId);
    }


    mostrarForm = (editable) => {
        this.setState({visible: true, editable})
    }

    ocultarForm = () => {
        this.setState({visible: false, tarifa: {}, editable: false})
    }

    redireccionar = (accion, id) => {
        if (accion === "REGRESAR") {
            window.location.href = `/proveedor/Proveedor/Details/${id}`;
        }
    }
}

const url = window.location.href;
const contratoProveedorId = url.substr(url.lastIndexOf('/') + 1);

const Container = Wrapper(TarifaHotelContainer,
    `/proveedor/TarifaHotel/GetContratoInfo/${contratoProveedorId}`,
    {},
    false);

ReactDOM.render(
    <Container />,
    document.getElementById('tarifas_hoteles')
);