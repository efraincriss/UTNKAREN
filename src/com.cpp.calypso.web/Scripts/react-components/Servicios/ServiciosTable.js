import React from 'react';
import axios from 'axios';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class ServiciosTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            servicios: [],
            key_form: 23423,
        }
        this.GetServicios = this.GetServicios.bind(this);

        this.generateButton = this.generateButton.bind(this);
        this.LoadServicio = this.LoadServicio.bind(this);
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
            <div>
                {/* "float: right; margin-right: 2em;" */}
                <div className="row">
                    <div className="col">
                        <div className="btn-toolbar" role="toolbar" style={{ float: 'right', marginRight: '2em' }} >
                            <a className="btn btn-primary pull-right fa fa-plus" onClick={() =>this.props.Siguiente(1)}>  Nuevo Servicio</a>
                        </div>
                    </div>
                </div>
                <br/>
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
                            {/* <TableHeaderColumn width={'6%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn> */}
                            <TableHeaderColumn dataField="codigo" isKey={true} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Código</TableHeaderColumn>
                            <TableHeaderColumn dataField='nombre' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Nombre</TableHeaderColumn>
                            <TableHeaderColumn dataField='Operaciones' dataFormat={this.generateButton.bind(this)} headerAlign="center">Opciones</TableHeaderColumn>
                        </BootstrapTable>
                    </div>
                </div>

            </div >
        )
    }

    GetServicios() {
        axios.post("/RRHH/Servicio/GetListaServiciosApi/", {})
            .then((response) => {
                this.setState({ servicios: response.data })
                console.log(this.state.servicios);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    generateButton(cell, row) {
        return (
            <div>
                {/* <button onClick={() => this.Detalles(row.Id)} className="btn btn-outline-success btn-sm">Ver</button> */}
                <button onClick={() => this.LoadServicio(row.Id)} className="btn btn-outline-primary btn-sm" style={{ marginLeft: '0.2em' }}>Editar</button>
                <button onClick={() => { if (window.confirm('Estás seguro?')) this.Delete(row.Id) }} className="btn btn-outline-danger btn-sm" style={{ marginLeft: '0.2em' }}>Eliminar</button>
            </div>
        )
    }

    LoadServicio(id) {
        sessionStorage.setItem('id_servicio', id);
        this.props.Siguiente(2);
    }

    Delete(id) {
        axios.post("/RRHH/Servicio/DeleteApiAsync/", { id: id })
            .then((response) => {
                this.props.successMessage("Servicio Eliminado!")
                this.GetServicios()
            })
            .catch((error) => {
                console.log(error);
            });
    }

}