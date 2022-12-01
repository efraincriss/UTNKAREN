import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import moment, { now } from 'moment';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import BlockUi from 'react-block-ui';

export default class CumplimientoRequisitosTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            colaboradores: [],
            tiposEstados: [],
            nro_identificacionConsulta: '',
            estado: '',
            nombresConsulta: '',
            loading: false,
        }

        this.generateButton = this.generateButton.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.limpiarEstados = this.limpiarEstados.bind(this);

        this.ConsultaEstados = this.ConsultaEstados.bind(this);
        this.getFormSelectEstado = this.getFormSelectEstado.bind(this);
        this.GetColaboradoreBuscar = this.GetColaboradoreBuscar.bind(this);
    }

    componentDidMount() {
        this.ConsultaEstados();
    }

    render() {
        const options = {
            withoutNoDataText: true
        };
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <div className="row" >
                        <div className="col-3">
                            <div className="form-group">
                                <label htmlFor="text"><b>No. Identificación:</b></label>
                                <input type="text" id="nro_identificacionConsulta" name="nro_identificacionConsulta" value={this.state.nro_identificacionConsulta} className="form-control" onChange={this.handleChange} />
                            </div>
                        </div>
                        <div className="col-4">
                            <div className="form-group">
                                <label htmlFor="text"><b>Apellidos Nombres:</b></label>
                                <input type="text" minLength="3" id="nombresConsulta" className="form-control" value={this.state.nombresConsulta} name="nombresConsulta" onChange={this.handleChange} />
                            </div>
                        </div>
                        <div className="col-3">
                            <div className="form-group">
                                <label htmlFor="text"><b>Estado:</b></label>
                                <select value={this.state.estado} onChange={this.handleChange} className="form-control" name="estado">
                                    <option value="">Seleccione...</option>
                                    {this.getFormSelectEstado()}
                                </select>
                            </div>
                        </div>
                        <div className="col-2" style={{ marginTop: '2.2%' }}>
                            <button type="button" onClick={() => this.GetColaboradoreBuscar()} style={{ marginLeft: '0.2em' }} className="btn btn-outline-primary">Buscar</button>
                            <button type="button" onClick={() => this.limpiarEstados()} style={{ marginLeft: '0.3em' }} className="btn btn-outline-primary">Cancelar</button>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <BootstrapTable data={this.state.colaboradores} hover={true} pagination={true} options={options}>
                                <TableHeaderColumn width={'5%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn>
                                <TableHeaderColumn width={'6%'} dataField="empleado_id_sap" dataAlign="center" thStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>ID SAP</TableHeaderColumn>
                                <TableHeaderColumn dataField="nombre_identificacion" thStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Tipo Identificación</TableHeaderColumn>
                                <TableHeaderColumn dataField="numero_identificacion" thStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>No. Identificación</TableHeaderColumn>
                                <TableHeaderColumn /*width={'20%'}*/ dataField='apellidos_nombres' tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Apellidos</TableHeaderColumn>
                                <TableHeaderColumn /*width={'20%'}*/ dataField='nombres' tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Nombres</TableHeaderColumn>
                                <TableHeaderColumn /*width={'20%'}*/ dataField='fecha_ingreso' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} dataFormat={this.formatFechaIngreso.bind(this)} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Fecha Ingreso</TableHeaderColumn>
                                <TableHeaderColumn dataField='nombre_grupo_personal' tdStyle={{ whiteSpace: 'normal' }} thStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Agrupación para Requisitos</TableHeaderColumn>
                                <TableHeaderColumn dataField='estado' headerAlign="center" tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Estado</TableHeaderColumn>
                                <TableHeaderColumn dataField='Operaciones' headerAlign="center" width={'10%'} dataFormat={this.generateButton.bind(this)}>Opciones</TableHeaderColumn>
                            </BootstrapTable>
                        </div>
                    </div>
                </div >
            </BlockUi>
        )
    }

    generateButton(cell, row) {
        return (
            <div>
                <button onClick={() => this.LoadColaborador(row.Id)} data-toggle="tooltip" data-placement="top" title="Verificar Cumplimiento de Requisitos"className="btn btn-outline-primary btn-sm fa fa-check-square-o" style={{ marginLeft: '0.2em' }}></button>
            </div>
        )
    }

    LoadColaborador(id) {
        sessionStorage.setItem('id_colaborador', id);
        return (
            window.location.href = "/RRHH/ColaboradorRequisito/Create/"
        );
    }

    GetColaboradoreBuscar() {
        if (!this.state.nro_identificacionConsulta && !this.state.nombresConsulta && !this.state.estado) {
            abp.notify.error("Debe seleccionar un criterio de busqueda!", 'Error');
        } else {
            if (this.state.nombresConsulta.length < 3 && this.state.nombresConsulta.length != 0) {
                abp.notify.error("Debe ingresar al menos tres caracteres para realizar la busqueda por apellidos nombres!", 'Error');
            } else {
                this.setState({ loading: true })

                var numeroIdentificacion = "";
                var nombres = "";
                var estado = "";

                if (this.state.nro_identificacionConsulta) {
                    numeroIdentificacion = this.state.nro_identificacionConsulta;
                }

                if (this.state.nombresConsulta) {
                    nombres = this.state.nombresConsulta;
                }

                if (this.state.estado) {
                    estado = this.state.estado;
                }

                axios.post("/RRHH/Colaboradores/GetFiltrosColaboradoresTableApi/",
                    {
                        numeroIdentificacion: numeroIdentificacion,
                        nombres: nombres,
                        estado: estado
                    })
                    .then((response) => {
                        console.log('consulta', response.data)
                        if (response.data.length == 0) {
                            this.setState({ loading: false, colaboradores: [] })
                            abp.notify.error("No existe registros con la información ingresada", 'Error');
                        } else {
                            this.setState({ loading: false, colaboradores: response.data })
                            // this.procesaConsulta(response.data);
                        }
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            }
        }
    }

    ConsultaEstados() {
        this.setState({ loading: true })
        console.log("COLABORADOREQUISITO")
        axios.post("/RRHH/Colaboradores/GetByCodeApi/?code=ESTADOSCOL", {})
            .then((response) => {
                console.log(response.data)
                this.setState({
                    tiposEstados: response.data.result,
                    loading: false
                })
                this.getFormSelectEstado();
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false })
            });
    }

    getFormSelectEstado() {
        return (
            this.state.tiposEstados.map((item) => {
                return (
                    <option key={Math.random()} value={item.nombre}>{item.nombre}</option>
                )
            })
        );
    }

    limpiarEstados() {
        this.setState({
            nro_identificacionConsulta: '',
            estado: '',
            nombresConsulta: '',
        })
    }

    formatFechaIngreso(cell, row) {
        return moment(row.fecha_ingreso).format("DD-MM-YYYY");
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
    }

}
ReactDOM.render(
    <CumplimientoRequisitosTable />,
    document.getElementById('content-cumple-requisitos')
);