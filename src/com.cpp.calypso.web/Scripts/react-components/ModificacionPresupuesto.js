import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';

import { Growl } from 'primereact/components/growl/Growl';

import ModificacionPresupuestoMenu from './ModificacionPresupuesto/ModificacionPresupuestoMenu';

class ModificacionPresupuesto extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            visible: false,
            table_key: 8957,
            contratos: [],
        }
        this.GetContratos = this.GetContratos.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
    }

    componentDidMount() {
        this.GetContratos();
    }

    render() {
        return (
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>

                <ModificacionPresupuestoMenu
                    contratos={this.state.contratos}
                    key={this.state.key}
                    showSuccess={this.successMessage}
                    showWarn={this.warnMessage}
                />
            </div>
        )
    }

    GetContratos() {
        axios.post("/Proyecto/Contrato/GetContratos/")
            .then((response) => {
                this.setState({ contratos: response.data, blocking: false })
            })
            .catch((error) => {
                this.setState({ blocking: false })
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
    <ModificacionPresupuesto />,
    document.getElementById('content_presupuesto')
);