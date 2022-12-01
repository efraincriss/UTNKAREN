import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import CategoriasEncargadoTable from './CategoriasEncargado/CategoriasEncargadoTable';

export default class CategoriasEncargado extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            categoriasEncargado: []
        }
        this.GetCategoriasEncargado = this.GetCategoriasEncargado.bind(this);
        this.Nuevo = this.Nuevo.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
    }

    componentDidMount() {
        this.GetCategoriasEncargado();
    }

    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                        <CategoriasEncargadoTable
                            data={this.state.categoriasEncargado}
                            showSuccess={this.successMessage}
                            showWarn={this.warnMessage}
                            GetCategoriasEncargado={this.GetCategoriasEncargado}
                        />
                    </div>
                </div>
            </div>
        )
    }

    GetCategoriasEncargado() {
        axios.post("/RRHH/CategoriasEncargado/GetCategoriasEncargadoApi/", {})
            .then((response) => {
                this.setState({ categoriasEncargado: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    Nuevo() {
        return (
            window.location.href = "/RRHH/CategoriasEncargado/Create/"
        );
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.message });
    }

    successMessage(msg) {
        this.setState({ message: msg }, this.showSuccess)
    }

    warnMessage(msg) {
        this.setState({ message: msg }, this.showWarn)
    }

}

ReactDOM.render(
    <CategoriasEncargado />,
    document.getElementById('content-CategoriasEncargado')
);