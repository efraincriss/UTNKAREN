import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';

import { Growl } from 'primereact/components/growl/Growl';

import SeguimientoOfertaMenu from './SeguimientoOferta/SeguimientoOfertaMenu';

class SeguimientoOferta extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            visible: false,
            table_key: 8957,
            empresas: [],
            clientes: [],
            contratos: [],
        }
        this.GetEmpresas = this.GetEmpresas.bind(this);
        this.GetClientes = this.GetClientes.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
    }

    componentDidMount() {
        this.GetEmpresas();
        this.GetClientes();
    }

    render() {
        return (
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <div className="row">
                    <div className="col">
                        <h4>Actualización de EAC</h4>
                    </div>
                </div>
                <hr />
                <SeguimientoOfertaMenu
                    empresas={this.state.empresas}
                    clientes={this.state.clientes}
                    contratos={this.state.contratos}
                    getContratos={this.state.GetContratos}
                    key={this.state.key}
                    showSuccess={this.successMessage}
                    showWarn={this.warnMessage}
                />

            </div>
        )
    }

    GetEmpresas() {
        axios.get("/Proyecto/Empresa/GetEmpresa")
            .then((response) => {
                this.setState({ empresas: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    GetClientes() {
        axios.get("/Proyecto/Cliente/GetClientesApi")
            .then((response) => {
                this.setState({ clientes: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    updateTableKey() {
        this.setState({ key: Math.random() })
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
    <SeguimientoOferta />,
    document.getElementById('content_seguimiento_oferta')
);