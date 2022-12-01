import React from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapper";
import HotelesPanel from "./HotelesPanel";
import HabitacionesPanel from "./HabitacionesPanel";
import http from "../../Base/HttpService";
import { 
    FRASE_ERROR_SELECCIONAR_FECHA,
    FRASE_RESERVA_CREADA,
    FRASE_ERROR_GLOBAL,
    MODULO_PROVEEDOR,
    CONTROLLER_RESERVA_HOTEL
} from "../../Base/Strings";
import BuscarColaboradorContainer from "../BuscarColaborador/BuscarColaboradorContainer";
import { Dialog } from 'primereact-v2/dialog';
import config from "../../Base/Config";

export default class PanelControlContainer extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            switchView: true,
            hoteles: [],
            fecha: '',
            fechaBuscada: '',
            proveedorId: 0,
            nodes: [],
            razonSocial: '',
            espacios: [],
            visible: false,
            espacioHabitacionId: 0,
            habitacionId: 0,
        }
    }

    componentDidMount() {
        this.props.unlockScreen();
    }


    render() {
        return (
            <div>
                {this.renderApp()}
            </div>
        )
    }

    renderApp = () => {
        if (this.state.switchView) {
            return (
                <HotelesPanel
                    hoteles={this.state.hoteles}
                    consultarHoteles={this.consultarHoteles}
                    handleChange={this.handleChange}
                    showSuccess={this.props.showSuccess}
                    showWarn={this.props.showWarn}
                    blockScreen={this.props.blockScreen}
                    unlockScreen={this.props.unlockScreen}
                    consultarHabitaciones={this.consultarHabitaciones}
                />
            )
        } else {
            return (
                <div>

                    <HabitacionesPanel
                        switchView={this.switchViewWithValue}
                        nodes={this.state.nodes}
                        razonSocial={this.state.razonSocial}
                        consultarEspacios={this.consultarEspacios}
                        espacios={this.state.espacios}
                        mostrarForm={this.mostrarForm}
                    />

                    <Dialog header="Colaborador" visible={this.state.visible} width="730px" modal={true} onHide={this.ocultarForm}>
                        <BuscarColaboradorContainer
                            seleccionarColaborador={this.seleccionarColaborador}
                            unlockScreen={this.props.unlockScreen}
                            blockScreen={this.props.blockScreen}
                            showSuccess={this.props.showSuccess}
                            showWarn={this.props.showWarn}
                        />
                    </Dialog>
                </div>
            )
        }
    }


    consultarHoteles = (fecha) => {
        event.preventDefault();
        if (fecha === "") {
            this.props.showWarn(FRASE_ERROR_SELECCIONAR_FECHA)
            this.props.unlockScreen();
        } else {
            this.props.blockScreen();
            let url = '';
            url = `/Proveedor/ReservaHotel/ListarHoteles?fecha=${fecha}`
            http.get(url)
                .then((response) => {
                    let data = response.data;
                    if (data.success === true) {
                        this.setState({ hoteles: data.result, fechaBuscada: fecha });
                    } else {
                        var message = $.fn.responseAjaxErrorToString(data);
                        this.props.showWarn(message);
                    }

                    this.props.unlockScreen();
                })
                .catch((error) => {
                    console.log(error)
                    this.props.unlockScreen();
                })
        }
    }



    consultarHabitaciones = (proveedorId, razonSocial) => {
        this.props.blockScreen();
        let url = '';
        url = `/Proveedor/Habitacion/HabitacionArbol/${proveedorId}`
        http.get(url)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({ nodes: data.result, switchView: false, razonSocial });
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
    }

    consultarEspacios = habitacionId => {
        this.props.blockScreen();
        let url = '';
        url = `/Proveedor/EspacioHabitacion/EspaciosLibresConDatos/${habitacionId}?fecha=${this.state.fechaBuscada}`
        http.get(url)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({ espacios: data.result, habitacionId });
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
    }



    crearEspacio = () => {
        this.props.blockScreen();
        if (this.state.fechaBuscada === "") {
            this.props.showWarn(FRASE_ERROR_SELECCIONAR_FECHA)
            this.props.unlockScreen();
        } else {
            this.crearEspacioApi();
        }
    }


    crearEspacioApi = () => {
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/${CONTROLLER_RESERVA_HOTEL}/CrearReservaApi`
        var entity = {
            EspacioHabitacionId: this.state.espacioHabitacionId,
            ColaboradorId: this.state.colaboradorId,
            fecha_desde: this.state.fechaBuscada,
            fecha_hasta: this.state.fechaBuscada,
            fecha_registro: null,
        }
        http.post(url, entity)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    if (data.created == true) {
                        this.props.showSuccess(FRASE_RESERVA_CREADA)
                        this.setState({
                            colaboradorId: 0,
                            visible: false,
                        });
                        this.consultarEspacios(this.state.habitacionId)
                    } else {
                        this.props.showWarn(data.errors);
                    }
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.showWarn(FRASE_ERROR_GLOBAL);
                this.props.unlockScreen();
            })
    }



    seleccionarColaborador = colaboradorId => {
        this.setState({ colaboradorId }, this.crearEspacio)
    }


    switchViewWithValue = value => {
        this.setState({ switchView: value, nodes: [] })
    }

    handleChange = event => {
        this.setState({ [event.target.name]: event.target.value });
    }

    switchView = (proveedorId) => {
        this.setState({ proveedorId, switchView: false })
    }

    ocultarForm = () => {
        this.setState({ visible: false })
    }

    mostrarForm = (espacioHabitacionId) => {
        console.log(espacioHabitacionId)
        this.setState({ visible: true, espacioHabitacionId })
    }

}

const Container = Wrapper(PanelControlContainer);

ReactDOM.render(
    <Container />,
    document.getElementById('panel_control_container')
);