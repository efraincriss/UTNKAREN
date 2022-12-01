import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import CargosSectorTable from './CargosSector/CargosSectorTable';

export default class CargosSector extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            cargosSector: []
        }
        this.GetCargosSector = this.GetCargosSector.bind(this);
        this.Nuevo = this.Nuevo.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
    }

    componentDidMount() {
        this.GetCargosSector();
    }

    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                        <CargosSectorTable
                            data={this.state.cargosSector}
                            showSuccess={this.successMessage}
                            showWarn={this.warnMessage}
                            getCargosSector={this.GetCargosSector}
                        />
                    </div>
                </div>
            </div>
        )
    }

    GetCargosSector() {
        axios.post("/RRHH/CargosSector/GetCargosSectorApi/", {})
            .then((response) => {
                this.setState({ cargosSector: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    Nuevo() {
        return (
            window.location.href = "/RRHH/CargosSector/Create/"
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
    <CargosSector />,
    document.getElementById('content-CargosSector')
);