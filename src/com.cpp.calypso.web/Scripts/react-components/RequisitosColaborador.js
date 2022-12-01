import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class RequisitoColaborador extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            requisitos: [],
            tipo_usuario: '',
            accion: '',
            requisito: '',
            tiposUsuario: [],
            roles: [],
            loading: true,
        }
        this.GetRequisitos = this.GetRequisitos.bind(this);
        this.generateButton = this.generateButton.bind(this);
        this.LoadRequisito = this.LoadRequisito.bind(this);
        this.Delete = this.Delete.bind(this);
        this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);

        this.GetRequisitosBuscar = this.GetRequisitosBuscar.bind(this);
        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
        this.getFormSelectTipoUsuario = this.getFormSelectTipoUsuario.bind(this);
        this.getFormSelectRol = this.getFormSelectRol.bind(this);
    }

    componentDidMount() {
        // this.GetRequisitos();
        this.GetCatalogos();
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
                            <div className="form-group">
                                <label htmlFor="label">Agrupación para Requisitos: </label>
                                <select value={this.state.tipo_usuario} onChange={this.handleChangeUpperCase} className="form-control" name="tipo_usuario">
                                    <option value="">Seleccione...</option>
                                    {this.getFormSelectTipoUsuario()}
                                </select>
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="accion">Acción: </label>
                                <select value={this.state.accion} onChange={this.handleChangeUpperCase} className="form-control" name="accion">
                                    <option value="">Seleccione...</option>
                                    {this.getFormSelectRol()}
                                </select>
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="label">Requisito: </label>
                                <input type="text" id="requisito" name="requisito" value={this.state.requisito} className="form-control" onChange={this.handleChangeUpperCase} />
                            </div>
                        </div>
                        <div className="col" style={{ marginTop: '2.2%' }}>
                            <button type="button" onClick={() => this.GetRequisitosBuscar()} style={{ marginLeft: '0.2em' }} className="btn btn-outline-primary">Buscar</button>
                            <button type="button" onClick={() => this.limpiarEstados()} style={{ marginLeft: '0.3em' }} className="btn btn-outline-primary">Cancelar</button>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <BootstrapTable
                                data={this.state.requisitos}
                                hover={true}
                                pagination={true}
                                striped={false}
                                condensed={true}
                                options={options}
                            >
                                <TableHeaderColumn
                                    dataField="any"
                                    isKey={true}
                                    dataFormat={this.Secuencial}
                                    width={"8%"}
                                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                >
                                    Nº
            </TableHeaderColumn>
                                {/* <TableHeaderColumn width={'6%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn> */}
                                <TableHeaderColumn dataField="usuario" thStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Agrupación para Requisitos</TableHeaderColumn>
                                <TableHeaderColumn width={'12%'} dataField='nombre_rol' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Acción</TableHeaderColumn>
                                <TableHeaderColumn dataField='requisito' thStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Requisito</TableHeaderColumn>
                                <TableHeaderColumn width={'10%'} thStyle={{ whiteSpace: 'normal' }} dataField='nombre_obligatorio' filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Obligatorio</TableHeaderColumn>
                                <TableHeaderColumn width={'10%'} dataField='nombre_requiere' thStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Requiere Archivo</TableHeaderColumn>
                                <TableHeaderColumn width={'13%'} dataField='Operaciones' dataFormat={this.generateButton.bind(this)} headerAlign="center">Opciones</TableHeaderColumn>
                            </BootstrapTable>
                        </div>
                    </div>
                </div>
            </BlockUi>
        )
    }

    GetRequisitosBuscar() {
        if (!this.state.tipo_usuario && !this.state.accion && !this.state.requisito) {
            abp.notify.error('Debe seleccionar un criterio de busqueda!', 'Error');
        } else {
            this.setState({ loading: true })

            axios.post("/RRHH/RequisitoColaborador/GetFiltrosRequisitosApi/",
                {
                    tipo_usuario: this.state.tipo_usuario,
                    accion: this.state.accion,
                    requisitos: this.state.requisito
                })
                .then((response) => {
                    console.log('consulta', response.data)
                    if (response.data.length == 0) {
                        this.setState({ loading: false, requisitos: [] })
                        abp.notify.error('No existe registros con la información ingresada', 'Error');
                    } else {
                        this.setState({ loading: false, requisitos: response.data })
                        // this.procesaConsulta(response.data);
                    }
                })
                .catch((error) => {
                    this.setState({ loading: false })
                    console.log(error);
                });
        }
    }

    limpiarEstados() {

        this.setState({
            tipo_usuario: '',
            accion: '',
            requisito: '',
        })
    }

    GetRequisitos() {
        axios.post("/RRHH/RequisitoColaborador/GetRequisitosApi/", {})
            .then((response) => {
                this.setState({ requisitos: response.data })
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
                <button onClick={() => this.LoadRequisito(row.Id)} className="btn btn-outline-primary btn-sm" style={{ marginLeft: '0.2em' }}>Editar</button>
                <button onClick={() => { if (window.confirm('Estás seguro?')) this.Delete(row.Id); }} className="btn btn-outline-danger btn-sm" style={{ marginLeft: '0.2em' }}>Eliminar</button>
            </div>
        )
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['GRUPOPERSONAL', 'ACCIONCOL'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                //console.log('catalogos',response.data);
                this.cargarCatalogos(response.data);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    cargarCatalogos(data) {
        //console.log('data',data);
        data.forEach(e => {
            var catalogo = JSON.parse(e);
            var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
            switch (codigoCatalogo) {
                case 'GRUPOPERSONAL':
                    this.setState({ tiposUsuario: catalogo })
                    this.getFormSelectTipoUsuario()
                    return;
                case 'ACCIONCOL':
                    this.setState({ roles: catalogo })
                    this.getFormSelectRol()
                    return;
                default:
                    console.log(codigoCatalogo)
                    return;
            }


        });
        this.setState({ loading: false })

    }

    getFormSelectTipoUsuario() {
        return (
            this.state.tiposUsuario.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectRol() {
        return (
            this.state.roles.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    LoadRequisito(id) {
        sessionStorage.setItem('id_requisito', id);
        return (
            window.location.href = "/RRHH/RequisitoColaborador/Edit/"
        );
    }

    Delete(id) {
        axios.post("/RRHH/RequisitoColaborador/DeleteApiAsync/", { id: id })
            .then((response) => {
                abp.notify.success("Requisito eliminado!", "Aviso");
                this.GetRequisitos()
            })
            .catch((error) => {
                console.log(error);
            });
    }

    Secuencial(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }

    handleChangeUpperCase(event) {
        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
    }

}

ReactDOM.render(
    <RequisitoColaborador />,
    document.getElementById('content-requisitos')
);