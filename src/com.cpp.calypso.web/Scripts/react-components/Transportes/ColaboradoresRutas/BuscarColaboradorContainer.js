import React from "react";
import ColaboradorTable from "./ColaboradorTable";
import Field from "../../Base/Field-v2";
import http from "../../Base/HttpService";
import wrapForm from "../../Base/BaseWrapper";
import ReactDOM from 'react-dom';
import {
    MODULO_PROVEEDOR,
    CONTROLLER_RESERVA_HOTEL,
} from "../../Base/Strings";
import config from "../../Base/Config";
 class BuscarColaboradorContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            data: [],
            identificacion: '',
            nombres: '',
            errors: {},
           
            
        }
    }

    componentDidMount() {
        this.props.blockScreen();
        this.props.unlockScreen();
      }

    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <form onSubmit={this.handleSubmit}>
                            <div className="row">
                                <div className="col-xs-6 col-md-5">
                                    <Field
                                        name="identificacion"
                                        label="No. Identificación"
                                        type="text"
                                        edit={true}
                                        readOnly={false}
                                        value={this.state.identificacion}
                                        onChange={this.handleChange}
                                        error={this.state.errors.identificacion}
                                    />
                                </div>
                                <div className="col-xs-6 col-md-5">
                                    <Field
                                        name="nombres"
                                        label="Apellidos Nombres"
                                        type="text"
                                        edit={true}
                                        readOnly={false}
                                        value={this.state.nombres}
                                        onChange={this.handleChange}
                                        error={this.state.errors.nombres}
                                    />
                                </div>

                                <div className="col" style={{paddingTop: '35px'}}>
                                    <button type="submit" className="btn btn-outline-primary">Buscar</button>&nbsp;
                                </div>
                            </div>
                        </form>
                        <hr />
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <ColaboradorTable
                            data={this.state.data}
                            seleccionarColaborador={this.props.seleccionarColaborador}
                        />
                    </div>
                </div>
            </div>
        )
    }

    resetValues = () => {
        this.setState({
            data: [],
            identificacion: '',
            nombres: '',
        })
    }


    handleSubmit = (event) => {
        event.preventDefault();
        this.props.blockScreen();
        var identificacion = this.state.identificacion;
        var nombres = this.state.nombres;
        let url = '';
        url = `/Transporte/ColaboradorRuta/BuscarColaboradorIdentificacion?identificacion=${identificacion}&nombres=${nombres}`
        http.get(url)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    console.log(data.result)
                    this.setState({ data: data.result })
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

    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }
}
const Container = wrapForm(BuscarColaboradorContainer);
ReactDOM.render(<Container />, document.getElementById("content"));
