import React from "react";
import ReactDOM from 'react-dom';

import Wrapper from "../../../Base/BaseWrapper";
import http from "../../../Base/HttpService";
import config from "../../../Base/Config";
import {
    MODULO_RECURSOS_HUMANOS,
    CONTROLLER_COLABORADORES,
    MODULO_ACCESO,
    CONTROLLER_TARJETA_ACCESO,
    CONTROLLER_CONTACTOS_EMERGENCIA
} from "../../../Base/Strings"
import CabeceraTarjetasRequisitos from "./CabeceraTarjetasRequisitos";
import DetallesTab from "./DetallesTab";
import TarjetasTabContainer from "./Tarjetas/TarjetasTabContainer";
import ContactosContainer from "./ContactoEmergencia/ContactosContainer";


class TarjetasRequisitosContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            colaboradorId: 0,
            colaborador: {},
            tarjetas: [],
            contactos: [],
           

        }
       
    }

    componentWillMount() {
        //let url = window.location.href;
        //let proveedorId = url.substr(url.lastIndexOf('/') + 1);
        //.setState({ colaboradorId });
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
                <CabeceraTarjetasRequisitos
                    colaborador={this.state.colaborador}
                />

                <div className="row">
                    <div style={{ width: '100%' }}>
                        <ul className="nav nav-tabs" id="detalles-tab" role="tablist">
                            <li className="nav-item">
                                <a className="nav-link active" id="detalles-tab" data-toggle="tab" href="#detalles" role="tab" aria-controls="home" aria-expanded="true">Detalles</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" id="tarjetas-tab" data-toggle="tab" href="#tarjetas" role="tab" aria-controls="tarjetas">Tarjetas</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" id="contactos-tab" data-toggle="tab" href="#contactos" role="tab" aria-controls="tarjetas">Contactos Emergencia</a>
                            </li>
                        </ul>

                        <div className="tab-content" id="myTabContent">
                            <div className="tab-pane fade show active" id="detalles" role="tabpanel" aria-labelledby="detalles-tab">
                                <DetallesTab
                                    colaborador={this.state.colaborador}
                                />
                            </div>

                            <div className="tab-pane fade" id="tarjetas" role="tabpanel" aria-labelledby="tarjetas-tab">
                                <TarjetasTabContainer
                                    colaboradorId={this.state.colaboradorId}
                                    colaborador={this.state.colaborador}
                                    tarjetas={this.state.tarjetas}
                                    showSuccess={this.props.showSuccess}
                                    showWarn={this.props.showWarn}
                                    blockScreen={this.props.blockScreen}
                                    unlockScreen={this.props.unlockScreen}
                                    reloadTarjetas={this.reloadTarjetas}
                                    
                                
                                />
                            </div>

                            <div className="tab-pane fade" id="contactos" role="tabpanel" aria-labelledby="contactos-tab">
                                <ContactosContainer
                                    contactos={this.state.contactos}
                                />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }


    obtenerColaborador = () => {
        let url = '';
        url = `/Accesos/TarjetaAcceso/Detalles/${this.state.colaboradorId}`;
        return http.get(url);
    }

    obtenerTarjetasColaborador = () => {
        let url = '';
        url = `/Accesos/TarjetaAcceso/GetByColaborador/${this.state.colaboradorId}`;
        return http.get(url);
    }


    obtenerContactosEmergencia = () => {
        let url = '';
        url = `/Accesos/TarjetaAcceso/DetallesContactosE/${this.state.colaboradorId}`;
        return http.get(url);
    }
    

    loadData = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.obtenerColaborador(), this.obtenerTarjetasColaborador(), this.obtenerContactosEmergencia()])
            .then(function ([colaborador, tarjetas, contactos]) {
                self.setState({
                    colaborador: colaborador.data.result,
                    tarjetas: tarjetas.data.result,
                    contactos: contactos.data.result,
                }, self.props.unlockScreen)
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });
          
    }

    reloadTarjetas = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.obtenerTarjetasColaborador()])
            .then(function ([tarjetas]) {
                self.setState({
                    tarjetas: tarjetas.data.result
                }, self.props.unlockScreen)
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });
    }
}

const Container = Wrapper(TarjetasRequisitosContainer);
ReactDOM.render(
    <Container />,
    document.getElementById('tarjetas_requisitos_container')
);
