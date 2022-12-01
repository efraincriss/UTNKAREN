import React, { Component } from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapper";
import http from '../../Base/HttpService';
import CabeceraDetalleHabitacion from "./CabeceraDetalleHabitacion";
import BodyDetalleHabitacion from "./BodyDetalleHabitacion";
import config from "../../Base/Config";
import {
    MODULO_PROVEEDOR,
    CONTROLLER_ESPACIO_HABITACION,
    CONTROLLER_HABITACION,
    CONTROLLER_PROVEEDOR
} from "../../Base/Strings";

class HabitacionDetalleContainer extends Component {

    constructor(props){
        super(props)
        this.state = { 
            proveedorId: 0,
            proveedor: {},
            habitaciones: [],
            espacios: [],
        }
    }

    componentWillMount(){
        let url = window.location.href;
        let proveedorId = url.substr(url.lastIndexOf('/') + 1);
        this.setState({ proveedorId});
    }

    componentDidMount(){
        this.consultarDatos();
    }


    render() {
        return (
            <div>
                <CabeceraDetalleHabitacion 
                    proveedor = {this.state.proveedor}
                />
                <BodyDetalleHabitacion
                    unlockScreen={this.props.unlockScreen}
                    blockScreen={this.props.blockScreen}
                    showSuccess={this.props.showSuccess}
                    showWarn={this.props.showWarn}
                    habitaciones = {this.state.habitaciones}
                    espacios = {this.state.espacios}
                    consultarEspaciosYHabitaciones={this.consultarEspaciosYHabitaciones}
                    proveedorId = {this.state.proveedorId}
                    getEspacios={this.getHabitaciones}
                />
            </div>
        )
      
    }


    getProveedor = () => {
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/${CONTROLLER_PROVEEDOR}/GetProveedorDetalleApi/${this.state.proveedorId}`;
        return http.get(url);
    }

    getHabitaciones = () => {
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/${CONTROLLER_HABITACION}/GetHabitacionesPorProveedor/${this.state.proveedorId}`;
        return http.get(url);
    }

    getEspacios = () => {
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/${CONTROLLER_ESPACIO_HABITACION}/GetEspaciosHabitacionesPorProveedor/${this.state.proveedorId}`;
        return http.get(url);
    }

    consultarDatos = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.getProveedor(), this.getHabitaciones(), this.getEspacios()])
            .then(function ([proveedor, habitaciones, espacios]) {
                self.setState({
                    proveedor: proveedor.data.result,
                    habitaciones: habitaciones.data.result,
                    espacios: espacios.data.result
                }, self.props.unlockScreen)
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });
    }

    consultarEspaciosYHabitaciones = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.getHabitaciones(), this.getEspacios()])
            .then(function ([habitaciones, espacios]) {
                self.setState({
                    habitaciones: habitaciones.data.result,
                    espacios: espacios.data.result
                }, self.props.unlockScreen)
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });
    }
}


const Container = Wrapper(HabitacionDetalleContainer);

ReactDOM.render(
    <Container />,
    document.getElementById('detalle_habitacion_component')
);
