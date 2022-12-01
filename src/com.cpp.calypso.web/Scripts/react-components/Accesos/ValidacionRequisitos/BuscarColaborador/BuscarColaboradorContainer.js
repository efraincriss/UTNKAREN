import React from "react";
import ReactDOM from 'react-dom';
import Field from "../../../Base/Field-v2";
import http from "../../../Base/HttpService";
import config from "../../../Base/Config";
import {
    CONTROLLER_RESERVA_HOTEL
} from "../../../Base/Strings"
import Wrapper from "../../../Base/BaseWrapper";
import ColaboradorTable from "./ColaboradorTable";


class BuscarColaboradorContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            data: [],
            identificacion: '',
            nombres: '',
            errors: {},
            source: ''
        }
    }

    componentDidMount() {
        this.props.unlockScreen();
        this.setState({
            source: this.props.getParameterByName('source')
        })
    }



    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <form onSubmit={this.handleSubmit}>
                            <div className="row">
                                <div className="col">
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
                                <div className="col">
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

                                <div className="col-xs-12 col-md-2" style={{paddingTop: '33px'}}>
                                    <button type="submit" className="btn btn-outline-primary align-bottom">Buscar</button>&nbsp;
                                </div>

                                <hr />
                            </div>

                        </form>
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <ColaboradorTable
                            data={this.state.data}
                            source={this.state.source}
                        />
                    </div>
                </div>
            </div>
        )
    }


    handleSubmit = (event) => {
        event.preventDefault();
        this.props.blockScreen();
        var identificacion = this.state.identificacion;
        var nombres = this.state.nombres;
        let url = '';
        url = `/Accesos/ValidacionRequisito/Search?identificacion=${identificacion}&nombres=${nombres}`
        http.get(url)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
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

const Container = Wrapper(BuscarColaboradorContainer);

ReactDOM.render(
    <Container />,
    document.getElementById('buscar_colaborador_container')
);