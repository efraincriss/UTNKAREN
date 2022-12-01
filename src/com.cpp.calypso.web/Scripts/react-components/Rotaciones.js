import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Growl } from 'primereact/components/growl/Growl';

export default class Rotaciones extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            rotaciones: [],
            key_form: 23423,
        }
        this.GetRotaciones= this.GetRotaciones.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.generateButton = this.generateButton.bind(this);
        this.LoadRotacion = this.LoadRotacion.bind(this);
        this.Delete = this.Delete.bind(this);
    }

    componentDidMount() {
        this.GetRotaciones();
    }

    render() {
        const options = {
            withoutNoDataText: true
        };
        return (
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <div className="row">
                    <div className="col">
                        <BootstrapTable
                            data={this.state.rotaciones}
                            hover={true} 
                            pagination={true}
                            striped={false}
                            condensed={true}
                            options={options}
                        >
                            <TableHeaderColumn width={'6%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn>
                            <TableHeaderColumn dataField="codigo" filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Código</TableHeaderColumn>
                            <TableHeaderColumn width={'20%'} dataField='nombre' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Nombre</TableHeaderColumn>
                            <TableHeaderColumn dataField='dias_laborablesC' thStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Días Laborables Campo</TableHeaderColumn>
                            <TableHeaderColumn dataField='dias_laborablesO' thStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Días Laborables Oficina</TableHeaderColumn>
                            <TableHeaderColumn dataField='dias_descanso' thStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Días Descanso</TableHeaderColumn>
                            <TableHeaderColumn dataField='nombre_estado' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Estado</TableHeaderColumn>
                            <TableHeaderColumn width={'17%'} dataField='Operaciones' dataFormat={this.generateButton.bind(this)} headerAlign="center">Opciones</TableHeaderColumn>
                        </BootstrapTable>
                    </div>
                </div>
            </div>
        )
    }

    GetRotaciones() {
        axios.post("/RRHH/AdminRotacion/GetListadoRotacionesApi/", {})
            .then((response) => {
                this.setState({ rotaciones: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    generateButton(cell, row){
        return(
            <div>
                {/* <button onClick={() => this.Detalles(row.Id)} className="btn btn-outline-success btn-sm">Ver</button> */}
                <button onClick={() => this.LoadRotacion(row.Id)} className="btn btn-outline-primary btn-sm" style={{marginLeft: '0.2em'}}>Editar</button>
                <button onClick={() => { if (window.confirm('Estás seguro?')) this.Delete(row.Id)}} className="btn btn-outline-danger btn-sm" style={{marginLeft: '0.2em'}}>Eliminar</button>
            </div>
        )
    }

    LoadRotacion(id){
        sessionStorage.setItem('id_rotacion', id);
        return (
            window.location.href = "/RRHH/AdminRotacion/Edit/"
        );
    }

    Delete(id){
        axios.post("/RRHH/AdminRotacion/DeleteApiAsync/", {id: id})
        .then((response) => {
            this.successMessage("Rotación Eliminada!")
            this.GetRotaciones()
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
    <Rotaciones />,
    document.getElementById('content-rotaciones')
);