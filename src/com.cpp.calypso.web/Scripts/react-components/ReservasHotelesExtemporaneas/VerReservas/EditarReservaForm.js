import React from "react";
import Field from "../../Base/Field-v2";
import moment from "moment";
import config from "../../Base/Config";
import http from "../../Base/HttpService";
import { FRASE_ERROR_EDITAR_RESERVA, FRASE_RESERVA_EDITADA } from "../../Base/Strings";

export default class EditarReservaForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            fecha: props.reserva.fecha_hasta ? moment(props.reserva.fecha_hasta).format(config.formatDate) : '',
            errors: {}
        }
    }

    componentWillReceiveProps(prevProps) {
        this.setState({
            fecha: prevProps.reserva.fecha_hasta ? moment(prevProps.reserva.fecha_hasta).format(config.formatDate) : ''
        })
    }


    render() {
        return (
            <div className="row">
                <div className="col">
                    <form onSubmit={this.handleSubmit}>
                        <div className="row">
                            <div className="col">
                                <Field
                                    name="fecha"
                                    label="Fecha Hasta"
                                    required
                                    type="date"
                                    edit={true}
                                    readOnly={false}
                                    value={this.state.fecha}
                                    onChange={this.handleChange}
                                    error={this.state.errors.fecha}
                                />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col">
                                <button type="submit" className="btn btn-outline-primary">Guardar</button>&nbsp;
                                <hr />
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        )
    }

    handleSubmit = event => {
        event.preventDefault();
        console.log(this.props.reserva.fecha_desde)
        console.log("Fecha Edición",moment(this.state.fecha));
        console.log("FechaLimite",moment(this.props.reserva.fecha_desde));

        let fechaForm = moment(this.state.fecha).format("YYYY-MM-DD");
        let fechaReservaDesde =moment(this.props.reserva.fecha_desde).format("YYYY-MM-DD");
        console.log(fechaForm);
        console.log(fechaReservaDesde);
        //if (moment(this.state.fecha) >= moment(this.props.reserva.fecha_desde)) { 
            if (moment(this.state.fecha) >= moment(this.props.reserva.fecha_desde)) { 
        
        this.props.blockScreen();
        let url = '';
        url = `/Proveedor/ReservaHotel/EditarReserva`
        http.post(url, {
            id: this.props.reserva.Id,
            fecha: this.state.fecha
        })
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    //this.props.consultarReservas();
                    this.props.resetData();
                    this.props.ocultarDialogEditar();
                    this.props.showSuccess(FRASE_RESERVA_EDITADA);
                } else {
                    this.props.showWarn(FRASE_ERROR_EDITAR_RESERVA);
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
        /*if (moment(this.props.fechaAnterior) > moment(this.state.fecha)) {
            this.props.showWarn("La fecha no puede ser menor a la anterior")
        } else {

        }*/
    }else{
        this.props.showWarn("La Fecha Hasta no puede ser menor a la Fecha Desde de la Reserva");

    }
    }

    handleChange = event => {
        this.setState({ [event.target.name]: event.target.value });
    }
}