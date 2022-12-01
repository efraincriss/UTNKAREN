import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import wrapForm from "./Base/BaseWrapper";
import moment from 'moment';
import RegistrarAusentismo from './ColaboradoresAusentismo/RegistrarAusentismo';
import Vista from './ColaboradoresAusentismo/Historicos';

import Field from "./Base/Field-v2";


export default class ColaboradoresAusentismo extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            colaboradoresAusentismo: [],
            visible: false,
            visible_rientegro: false,
            ausentismo_id: '',
            colaborador_id: '',
            loading: false,
            vista: 'lista',
            tipo_grupo_personal: [],
            nro_identificacion: '',
            nombres: '',
            grupo_personal: '',
            id: '',
            ColaboradorId: 0,



            /* Busquedas */
            Colaborador: null,
            data: [],

            /* Colaboradores Ausentismos */
            uploadFile: '',
            uploadPDF: false,
            downloadPDF: false,
            ColaboradoresAusentismo: null,

            checkedausentismo: false,
            checkedreintegro: false,
            ArchivoAusentismoId: 0,
            ArchivoReintegroId: 0,

            //Edición Ausentismo
            vieweditar: false,
            ColaboradorAusentismoId: 0,
            observacion: '',
            fecha_inicio: null,
            fecha_fin: null,

            errors: {},

        }

        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.handleChange = this.handleChange.bind(this);

        this.generateButton = this.generateButton.bind(this);
        this.loadReintegro = this.loadReintegro.bind(this);
        this.dateFormat = this.dateFormat.bind(this);
        this.loadAusentismo = this.loadAusentismo.bind(this);

        this.ConsultaGruposPersonal = this.ConsultaGruposPersonal.bind(this);
        this.getFormSelectGrupo = this.getFormSelectGrupo.bind(this);

        /* dialogs */
        this.onHide = this.onHide.bind(this);
        this.getListado = this.getListado.bind(this);

        this.limpiarEstados = this.limpiarEstados.bind(this);
        this.GetColaboradoreBuscar = this.GetColaboradoreBuscar.bind(this);

        this.EnviarFormulario = this.EnviarFormulario.bind(this);
        this.isValid = this.isValid.bind(this);
    }

    componentDidMount() {
        //this.getListado();
        //this.ConsultaGruposPersonal();
        this.props.unlockScreen();
    }

    blockScreen = () => {
        this.setState({ loading: true });

    }
    unblockScreen = () => {
        this.setState({ loading: false });

    }
    RedireccionarListado = (row) => {
        console.log(row)
        this.setState({ vista: "historicos", Colaborador: row });
    }


    isValid() {
        const errors = {};
        if (this.state.fecha_inicio === null) {
            errors.fecha_inicio = "Campo Requerido";
        }
        if (this.state.fecha_fin === null) {
            errors.fecha_fin = "Campo Requerido";
        }
        if (this.state.observacion === '') {
            errors.observacion = "Campo Requerido";
        }
        if (this.state.fecha_inicio != null && this.state.fecha_fin != null) {
            if (moment(this.state.fecha_fin) < moment(this.state.fecha_inicio)) {
                errors.fecha_fin = "Campo no puede ser menor a Fecha Inicio";
            }


        }

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }
    render() {
        if (this.state.vista === "lista") {
            const options = {
                withoutNoDataText: true
            };
            return (
                <div>
                    <div>
                        <div className="row">
                            <div className="col-4">
                                <Field
                                    name="nro_identificacion"
                                    label="No. de Identificación"
                                    edit={true}
                                    readOnly={false}
                                    value={this.state.nro_identificacion}
                                    onChange={this.handleChange}
                                    error={this.state.errors.nro_identificacion}
                                />
                            </div>
                            <div className="col-4">
                                <Field
                                    name="nombres"
                                    label="Apellidos Nombres"
                                    edit={true}
                                    readOnly={false}
                                    value={this.state.nombres}
                                    onChange={this.handleChange}
                                    error={this.state.errors.nombres}
                                />
                            </div>

                            <div className="col-4" style={{ paddingTop: '35px' }}>
                                <button type="button" onClick={() => this.GetColaboradoreBuscar()} style={{ marginLeft: '0.2em' }} className="btn btn-outline-primary">Buscar</button>
                                <button type="button" onClick={() => this.limpiarEstados()} style={{ marginLeft: '0.3em' }} className="btn btn-outline-primary">Cancelar</button>
                                <button type="button" onClick={() => this.ActivarVencimientos()} style={{ marginLeft: '0.2em' }} className="btn btn-outline-indigo">Actualizar Vencimientos</button>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col">
                                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                                <div>
                                    <BootstrapTable
                                        data={this.state.data}
                                        hover={true}
                                        pagination={true}
                                        striped={false}
                                        condensed={true}
                                        options={options}>
                                        <TableHeaderColumn isKey={true} width={'5%'}
                                            dataField="nro"
                                            headerAlign="center"
                                            filter={{ type: 'TextFilter', delay: 500 }}
                                            dataSort={true}
                                            thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                            tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        >No.
                                                           </TableHeaderColumn>
                                        <TableHeaderColumn dataField="numero_legajo_definitivo" width={'8%'}
                                            thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                            tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                            dataFormat={this.formatLegajo.bind(this)}
                                            dataAlign="center" headerAlign="center"
                                            filter={{ type: 'TextFilter', delay: 500 }}
                                            dataSort={true}
                                        >No. de Legajo</TableHeaderColumn>
                                        <TableHeaderColumn dataField="nombre_identificacion"
                                            width={'10%'}
                                            thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                            tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                            headerAlign="center"
                                            filter={{ type: 'TextFilter', delay: 500 }}
                                            dataSort={true}
                                        >Tipo de Identificación</TableHeaderColumn>
                                        <TableHeaderColumn dataField="numero_identificacion"
                                            width={'8%'} 
                                            thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                            tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                            filter={{ type: 'TextFilter', delay: 500 }}
                                            dataSort={true}>No. de Identificación</TableHeaderColumn>
                                        <TableHeaderColumn dataField="nombres_apellidos" width={'20%'}
                                            thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                            tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}

                                            filter={{ type: 'TextFilter', delay: 500 }}
                                            dataSort={true}>Apellidos Nombres</TableHeaderColumn>

                                        <TableHeaderColumn dataField="estado"
                                            width={'6%'}
                                            thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                            tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}

                                            filter={{ type: 'TextFilter', delay: 500 }}
                                            dataSort={true}>Estado</TableHeaderColumn>
                                        <TableHeaderColumn dataField='Operaciones'
                                            width={'20%'}
                                            dataAlign="left"
                                            thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                            tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                            dataFormat={this.generateButton.bind(this)}>Opciones</TableHeaderColumn>
                                    </BootstrapTable>
                                </div>
                            </div>
                        </div>
                    </div>
                </div >
            )
        } else if (this.state.vista === "ausentismo") {
            return (
                <RegistrarAusentismo
                    listado={this.getListado}
                    colaborador={this.state.Colaborador}
                     onHide={this.onHide}
                    successMessage={this.successMessage}
                    ColaboradorId={this.state.ColaboradorId}
                    showSuccess={this.showSuccess}
                    showWarning={this.showWarning}
                    showWarn={this.showWarn}
                    blockScreen={this.props.blockScreen}
                    unlockScreen={this.props.unlockScreen}
                />
            )
        }
        else {
            return (
                <Vista
                    colaborador={this.state.Colaborador}
                    showSuccess={this.props.showSuccess}
                    showWarning={this.props.showWarning}
                    showWarn={this.props.showWarn}
                    blockScreen={this.props.blockScreen}
                    unlockScreen={this.props.unlockScreen}
                    RedireccionarLista={this.RedireccionarLista}
                />
            )
        }


    }




    EnviarFormulario(event) {
        this.setState({ loading: true })
        event.preventDefault();

        if (!this.isValid()) {
            abp.notify.error(
                "No ha ingresado los campos obligatorios  o existen datos inválidos.",
                "Validación"
            );
            return;
        } else {

            axios
                .post("/RRHH/ColaboradoresAusentismo/GetEditAusentimo", {
                    Id: this.state.ColaboradoresAusentismo.Id,
                    fecha_inicio: this.state.fecha_inicio,
                    fecha_fin: this.state.fecha_fin,
                    observacion: this.state.observacion,
                    estado: "ACTIVO"
                })
                .then(response => {
                    if (response.data == "OK") {
                        abp.notify.success("Ausetismo Editado", 'Aviso');
                        this.setState({ vieweditar: false, errors: {} });
                        this.getListado();

                    } else {
                        abp.notify.error('Existe un incoveniente intentelo más tarde', 'Error');
                        this.setState({ loading: false })
                    }

                })
                .catch(error => {
                    console.log(error);
                    abp.notify.error('Existe un incoveniente intentelo más tarde', 'Error');
                    this.setState({ loading: false })
                });


        }
    }
 
    ActivarVencimientos = () => {
        this.setState({ loading: true })
        axios.defaults.headers.post['Content-Type'] ='application/x-www-form-urlencoded';
        axios.defaults.headers.post['Access-Control-Allow-Origin'] = '*';
        axios.post("http://10.26.102.43:8181/api/FinalizarAusentismo/", {})
            .then((response) => {
                if (response.data != null && response.data.ejecutadoCorrectamente == true) {
                    abp.notify.success("Vencimientos Ausentimos ejecutados correctamente");

                } else {
                    abp.notify.warn('Existe un incoveniente intentelo más tarde', 'Error');
                }
                this.setState({ loading: false })
            })
            .catch((error) => {
                console.log(error);
                abp.notify.error('Existe un inconveniente intentelo más tarde ' + error, 'Error');
                this.setState({ loading: false })
            });

    }




    ocultarEditar = () => {
        this.setState({ vieweditar: false, ColaboradoresAusentismo: null, errors: {} })
    }
    generateButton(cell, row) {

        return (
            <div style={{ textAlign: 'center' }}>

                <button title="Registrar un Ausentismo" onClick={() => this.loadAusentismo(row)} style={{ marginLeft: '0.3em' }} className="btn btn-outline-primary btn-sm fa fa-gear"></button>
                {row.posee_ausentismos &&
                    <button title="Ver Ausentismos" onClick={() => this.RedireccionarListado(row)} style={{ marginLeft: '0.3em' }} className="btn btn-outline-success btn-sm fa fa-eye"></button>
                }
            </div>
        )
    }



    formatIdentificacion(cell, row) {
        return row.Colaborador.numero_identificacion;
    }

    formatLegajo(cell, row) {

        if (row.numero_legajo_temporal != null) {
            var length = row.numero_legajo_temporal.length;

            switch (length) {
                case 1:
                    var numero = "0000" + row.numero_legajo_temporal;
                    return numero;
                case 2:
                    var numero = "000" + row.numero_legajo_temporal;
                    return numero;
                case 3:
                    var numero = "00" + row.numero_legajo_temporal;
                    return numero;
                case 4:
                    var numero = "0" + row.numero_legajo_temporal;
                    return numero;
                case 5:
                    var numero = row.numero_legajo_temporal;
                    return numero;
                default:
                    var numero = row.numero_legajo_temporal;
                    return numero;
            }
        } else {
            return " ";
        }
    }

    limpiarEstados() {

        this.setState({
            nro_identificacion: '',
            grupo_personal: '',
            nombres: '',
        })
    }

    GetColaboradoreBuscar() {
        if (!this.state.nro_identificacion && !this.state.nombres && !this.state.grupo_personal) {
            abp.notify.error('Ingresa un campo de búsqueda!', 'Error');
        } else {
            if (this.state.nombres.length < 3 && this.state.nombres.length != 0) {
                abp.notify.error('Debe ingresar al menos tres caracteres para realizar la búsqueda por apellidos nombres!', 'Error');
            } else {
                this.setState({ loading: true })

                var numeroIdentificacion = "";
                var nombres = "";

                if (this.state.nro_identificacion) {
                    numeroIdentificacion = this.state.nro_identificacion;
                }

                if (this.state.nombres) {
                    nombres = this.state.nombres;
                }

                if (this.state.grupo_personal) {
                    grupo_personal = this.state.grupo_personal;
                }
                this.props.blockScreen();
                axios.post("/RRHH/Colaboradores/GetFiltrosAusentismo/",
                    {
                        numeroIdentificacion: numeroIdentificacion,
                        nombres: nombres,
                    })
                    .then((response) => {

                        if (response.data.length == 0) {
                            this.setState({ loading: false, colaboradores: [] })
                            abp.notify.error('No existe registros con la información ingresada', 'Error');
                            this.props.unlockScreen();
                        } else {
                            console.log(response.data);
                            this.setState({
                                loading: false,
                                data: response.data,
                                Colaborador: response.data
                            }
                            )
                            this.props.unlockScreen();
                            //this.procesaConsulta(response.data);
                        }
                    })
                    .catch((error) => {
                        console.log(error);
                        this.props.unlockScreen();
                    });
            }
        }
    }

    procesaConsulta(data) {
        var colaboradores = [];
        data.forEach(e => {
            var c = {};
            c.Colaborador = {}

            c.estado = null;
            c.estado_colaborador = e.estado;
            c.colaborador_id = e.Id;
            c.nro = e.nro;
            c.nro_legajo = e.numero_legajo_temporal;
            c.tipo_identificacion = e.nombre_identificacion;
            c.Colaborador.numero_identificacion = e.numero_identificacion;
            c.nombres = e.nombres_apellidos;
            c.grupo_personal = e.nombre_grupo_personal;
            c.tipo_identificacion = e.catalogo_tipo_identificacion_id;
            if (e.ausentismos != null && e.ausentismos.length > 0) {
                c.fecha_inicio = moment(e.ausentismos[0].fecha_inicio).format("YYYY-MM-DD");
                c.fecha_fin = moment(e.ausentismos[0].fecha_fin).format("YYYY-MM-DD");
                c.nombre_ausentismo = e.ausentismos[0].nombre_ausentismo;
                c.estado = e.ausentismos[0].estado;
                c.Id = e.ausentismos[0].Id;
            } else {
                c.fecha_inicio = null;
                c.fecha_fin = null;
                c.nombre_ausentismo = null;
            }


            colaboradores.push(c);
        });

        this.setState({ colaboradoresAusentismo: colaboradores });
    }


    getListado() {
        axios.post("/RRHH/ColaboradoresAusentismo/GetListado/", {})
            .then((response) => {
                console.log(response.data);
                this.setState({ colaboradoresAusentismo: response.data, loading: false })
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error);
                this.props.unlockScreen();
            });
    }

    loadAusentismo(row) {
        console.clear();
        console.log(row);
        this.setState({
            id: row.colaborador_id,
            vista: "ausentismo",
            ColaboradorId: row.colaborador_id,
            Colaborador:row
        })
    }

    loadReintegro(id, colaborador_id) {
        sessionStorage.setItem('colaborador_id', colaborador_id);
        sessionStorage.setItem('ausentismo_id', id);
        return (
            window.location.href = "/RRHH/ColaboradoresAusentismo/Create/");
    }


    dateFormat(cell, row) {
        if (cell === null) {
            return (
                ""
            )
        }
        return (
            moment(cell).format('YYYY-MM-DD')
        )
    }

    onHide() {
        this.getListado();
        this.setState({ vista: 'lista' })
    }

    RedireccionarLista = () => {
        this.setState({ vista: "lista" });

    }

    ConsultaGruposPersonal() {
        this.setState({ loading: true })
        axios.post("/RRHH/Colaboradores/GetCatalogosPorCodigoApi/", {
            codigo: 'GRUPOPERSONAL'
        })
            .then((response) => {

                this.setState({
                    tipo_grupo_personal: response.data,
                    loading: false
                })
                this.getFormSelectGrupo();
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false })
            });
    }

    getFormSelectGrupo() {
        return (
            this.state.tipo_grupo_personal.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    handleChange(event) {

        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
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
const Container = wrapForm(ColaboradoresAusentismo);
ReactDOM.render(
    <Container />,
    document.getElementById('content-ColaboradoresAusentismo')
);