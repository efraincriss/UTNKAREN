import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import BlockUi from 'react-block-ui';

export default class Requisitos extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            requisitos: [],
            loading: true,
            key_form: 23423,
        }
        this.GetRequisitos = this.GetRequisitos.bind(this);
        this.generateButton = this.generateButton.bind(this);
        this.LoadRotacion = this.LoadRotacion.bind(this);
        this.Delete = this.Delete.bind(this);
    }

    componentDidMount() {
        this.GetRequisitos();
    }

    render() {
        const options = {
            withoutNoDataText: true
        };
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>

                    <div className="row">
                        <div className="col">
                            <BootstrapTable
                                data={this.state.requisitos}
                                hover={true}
                                pagination={true}
                                striped={false}
                                options={options}
                                condensed={true}
                            >
                                <TableHeaderColumn dataField="any" dataFormat={this.Secuencial} width={"8%"} tdStyle={{ whiteSpace: "normal", textAlign: "center" }} thStyle={{ whiteSpace: "normal", textAlign: "center" }}>Nº</TableHeaderColumn>
                                {/* <TableHeaderColumn width={'6%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn> */}
                                <TableHeaderColumn dataField="codigo" isKey={true} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Código</TableHeaderColumn>
                                <TableHeaderColumn width={'20%'} dataField='nombre' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Nombre</TableHeaderColumn>
                                <TableHeaderColumn dataField='nombre_caducidad' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Tipo Usuario</TableHeaderColumn>
                                <TableHeaderColumn dataField='nombre_estado' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Responsable de Requisito</TableHeaderColumn>
                                <TableHeaderColumn width={'8%'} dataField='nombre_activo' dataAlign="center" thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Activo</TableHeaderColumn>
                                <TableHeaderColumn width={'17%'} dataField='Operaciones' dataFormat={this.generateButton.bind(this)} headerAlign="center">Opciones</TableHeaderColumn>
                            </BootstrapTable>
                        </div>
                    </div>
                </div>
            </BlockUi>
        )
    }

    GetRequisitos() {
        axios.post("/RRHH/Requisitos/GetRequisitosApi/", {})
            .then((response) => {
                this.setState({ requisitos: response.data, loading: false })
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
                <button onClick={() => { if (window.confirm('Estás seguro?')) this.Delete(row.Id); }} className="btn btn-outline-danger btn-sm" style={{ marginLeft: '0.2em' }}>Eliminar</button>
            </div>
        )
    }

    LoadRotacion(id) {
        sessionStorage.setItem('id_requisitos', id);
        return (
            window.location.href = "/RRHH/Requisitos/Edit/"
        );
    }

    Delete(id) {
        axios.post("/RRHH/Requisitos/DeleteApiAsync/", { id: id })
            .then((response) => {
                abp.notify.success("Requisito Eliminado!", "Aviso");
                this.GetRequisitos()
            })
            .catch((error) => {
                console.log(error);
                abp.notify.error('Algo salió mal.', 'Error');
            });
    }

    Secuencial(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }

}

ReactDOM.render(
    <Requisitos />,
    document.getElementById('content-requisitos')
);