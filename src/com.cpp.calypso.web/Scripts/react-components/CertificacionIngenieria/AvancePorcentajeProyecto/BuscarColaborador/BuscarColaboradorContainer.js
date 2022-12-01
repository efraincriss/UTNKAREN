import React from "react";
import Field from "../../../Base/Field-v2";
import http from "../../../Base/HttpService";
import ColaboradorTable from "./ColaboradorTable";
import { ScrollPanel } from 'primereact-v3.3/scrollpanel';
import axios from "axios";
export default class BuscarColaboradorContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            data: [],
            search: '',
            errors: {},
            source: ''
        }
    }

    componentDidMount() {
        this.props.unlockScreen();
    }

    resetValues = () => {
        this.setState({
            data: [],
            identificacion: '',
            nombres: '',
        })
    }

    render() {
        return (
            <ScrollPanel style={{ width: '100%', height: '400px' }}>
                <div className="row">
                    <div className="col">
                        <form onSubmit={this.handleSubmit}>
                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="search"
                                        label="No. Identificación / Nombres"
                                        type="text"
                                        placeholder="Digite identificación o Nombres y Apellidos"
                                        edit={true}
                                        readOnly={false}
                                        value={this.state.search}
                                        onChange={this.handleChange}
                                        error={this.state.errors.search}
                                    />
                                </div>

                                <div className="col-xs-12 col-md-2" style={{ paddingTop: '33px' }}>
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
                            seleccionarColaborador={this.props.seleccionarColaborador}
                        />
                    </div>
                </div>
            </ScrollPanel>
        )
    }


    handleSubmit = (event) => {
        event.preventDefault();
        this.props.blockScreen();
        axios
            .post("/CertificacionIngenieria/DetalleDirectoIngenieria/ObtenerColaborador", {
                search: this.state.search,
            })
            .then((response) => {
                console.log("Response", response);
                this.setState({ data: response.data });
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error);
                this.props.unlockScreen();
            });
    }

    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }
}

