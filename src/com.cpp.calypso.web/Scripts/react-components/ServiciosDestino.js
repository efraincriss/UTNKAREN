import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Growl } from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';

export default class ServiciosDestino extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            servicios: [],
            key_form: 23423,
            loading: true,
        }
        this.GetServicios = this.GetServicios.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.generateButton = this.generateButton.bind(this);
        this.LoadRotacion = this.LoadRotacion.bind(this);
        this.Delete = this.Delete.bind(this);
    }

    componentDidMount() {
        this.GetServicios();
    }

    render() {
        const options = {
            withoutNoDataText: true
        };
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    <div className="row">
                        <div className="col">
                            <BootstrapTable
                                data={this.state.servicios}
                                hover={true}
                                pagination={true}
                                striped={false}
                                condensed={true}
                                options={options}
                            >
                                <TableHeaderColumn width={'6%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn>
                                <TableHeaderColumn dataField="destino" filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Destino</TableHeaderColumn>
                                <TableHeaderColumn dataField='nombre_alimentacion' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Alimentación</TableHeaderColumn>
                                <TableHeaderColumn dataField='nombre_movilizacion' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Movilización</TableHeaderColumn>
                                <TableHeaderColumn dataField='nombre_alojamiento' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Alojamiento</TableHeaderColumn>
                                <TableHeaderColumn dataField='nombre_lavanderia' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Lavandería</TableHeaderColumn>
                                <TableHeaderColumn width={'17%'} dataField='Operaciones' dataFormat={this.generateButton.bind(this)} headerAlign="center">Opciones</TableHeaderColumn>
                            </BootstrapTable>
                        </div>
                    </div>
                </div>
            </BlockUi>
        )
    }

    GetServicios() {
        axios.post("/RRHH/ServicioDestino/GetServiciosApi/", {})
            .then((response) => {
                this.setState({ servicios: response.data })
                this.setState({ loading: false })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    generateButton(cell, row) {
        return (
            <div>
                {/* <button onClick={() => this.Detalles(row.Id)} className="btn btn-outline-success btn-sm">Ver</button> */}
                <button onClick={() => this.LoadRotacion(row.Id)} className="btn btn-outline-primary btn-sm" style={{ marginLeft: '0.2em' }}>Editar</button>
                <button onClick={() => { if (window.confirm('Estás seguro?')) this.Delete(row.Id) }} className="btn btn-outline-danger btn-sm" style={{ marginLeft: '0.2em' }}>Eliminar</button>
            </div>
        )
    }

    LoadRotacion(id) {
        sessionStorage.setItem('id_servicio', id);
        return (
            window.location.href = "/RRHH/ServicioDestino/Edit/"
        );
    }

    Delete(id) {
        axios.post("/RRHH/ServicioDestino/DeleteApiAsync/", { id: id })
            .then((response) => {
                this.successMessage("Servicio eliminado!")
                this.GetServicios()
            })
            .catch((error) => {
                console.log(error);
            });
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
    <ServiciosDestino />,
    document.getElementById('content-servicios')
);