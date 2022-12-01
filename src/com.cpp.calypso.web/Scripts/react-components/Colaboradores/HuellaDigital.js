import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import RegistrarHuella from './RegistrarHuella';

export default class HuellaDigital extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            Id: '',
            tipo_identificacion: '',
            apellidos: '',
            nombres: '',
            nro_identificacion: '',
            tiposIdentificacion: [],
            colaborador: [],
            errores: [],
            lsHuellas: [],
            visible: false,
            id_huella: '',
            mode: 'lista',
            colaboradoresHuellaDigital: []
        }
        this.getFormSelectTipoIdent = this.getFormSelectTipoIdent.bind(this);
        this.GetTiposItentificacion = this.GetTiposItentificacion.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.clearStates = this.clearStates.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
        this.GetHuellas = this.GetHuellas.bind(this);
        this.ActualizarListaHuellas = this.ActualizarListaHuellas.bind(this);


        /* Mensajes al usuario */
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);

        /* Opciones */
        this.generateButton = this.generateButton.bind(this);
        this.Delete = this.Delete.bind(this);
        this.onHide = this.onHide.bind(this);
        this.showForm = this.showForm.bind(this);
        this.hideForm = this.hideForm.bind(this);
        this.Regresar = this.Regresar.bind(this);
        this.buscarColaborador = this.buscarColaborador.bind(this);
    }


    componentDidMount() {
        this.GetTiposItentificacion();
        this.setState({ mode: 'lista' })
        this.GetHuellas(sessionStorage.getItem('id_colaborador'));
        this.buscarColaborador();
    }

    render() {
        const options = {
            withoutNoDataText: true
        };
        if (this.state.mode == 'lista') {
            return (
                <div>
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    <form>
                        <div className="col-sm-12" style={{ margin: 'auto' }}>
                            <div className="row">
                                <div className="col-1"></div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="text"><b>Tipo de Identificación:</b> {this.state.tipo_identificacion} </label>

                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="text"><b>No. de Identificación:</b> {this.state.nro_identificacion} </label>
                                    </div>
                                </div>
                                <div className="col-1"></div>
                            </div>
                            <div className="row">
                                <div className="col-1"></div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="text"><b>Apellidos Nombres:</b> {this.state.nombres_apellidos} </label>
                                    </div>
                                </div>
                                <div className="col">
                                </div>
                                <div className="col-1"></div>
                            </div>

                            <div className="row">
                                <div className="col-1"></div>
                                <div className="col">
                                    <div className="form-group">
                                        <h5 htmlFor="label"> <b>Huellas registradas:</b> </h5>
                                    </div>
                                </div>
                                <div className="col">
                                    <a onClick={() => this.showForm(0)} style={{ color: '#fff' }} className="btn btn-primary pull-right fa fa-plus" href="#"> Nuevo</a>
                                </div>
                                <div className="col-1"></div>
                            </div>
                            <br />
                            <div className="row">
                                <div className="col-sm-1"></div>
                                <div className="col-sm-10">
                                    <div>
                                        <BootstrapTable data={this.state.lsHuellas} hover={true} pagination={true} options={options}>
                                            <TableHeaderColumn dataField="nro_huella" width="10%" isKey={true} headerAlign="center" dataAlign="center" dataSort={true}>No.</TableHeaderColumn>
                                            <TableHeaderColumn dataField="dedo" headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Dedo Huella</TableHeaderColumn>
                                            <TableHeaderColumn dataField="esPrincipal" headerAlign="center" dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Principal</TableHeaderColumn>
                                            <TableHeaderColumn dataField='Operaciones' headerAlign="center" width={'25%'} dataFormat={this.generateButton.bind(this)}>Opciones</TableHeaderColumn>
                                        </BootstrapTable>
                                    </div>
                                </div>
                                <div className="col-sm-1"></div>
                            </div>
                            <br />
                            <div className="row">
                                <div className="col-sm-1"></div>
                                <div className="col-sm-5">
                                    <button type="button" onClick={() => this.clearStates()} className="btn btn-outline-primary fa fa-arrow-left"> Cancelar</button>
                                </div>
                                <div className="col-sm-5"></div>
                                <div className="col-sm-1"></div>
                            </div>
                        </div>
                    </form>
                </div >
            )
        }

        if (this.state.mode == 'huella') {
            return (
                <RegistrarHuella
                    id_colaborador={this.state.Id}
                    id_huella={this.state.id_huella}
                    hideForm={this.hideForm}
                    actualizarListaHuellas={this.ActualizarListaHuellas}
                />
            )
        }
    }

    GetHuellas(id) {
        axios.post("/RRHH/Colaboradores/GetHuellasPorColaboradorApi/" + id, {})
            .then((response) => {
                this.setState({ lsHuellas: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    ActualizarListaHuellas() {
        this.GetHuellas(this.state.Id);
    }


    generateButton(cell, row) {
        return (
            <div style={{ textAlign: 'center' }}>
                <button onClick={() => this.showForm(row.Id)} className="btn btn-outline-primary btn-sm fa ">Editar</button>
                <button onClick={() => { if (window.confirm('Estás seguro?')) this.Delete(row.Id) }} className="btn btn-outline-danger btn-sm fa" style={{ marginLeft: '0.2em' }}>Eliminar</button>
            </div>
        )
    }

    Delete(id) {
        axios.post("/RRHH/Colaboradores/DeleteHuellaApi/" + id, {})
            .then((response) => {
                this.GetHuellas(this.state.Id);
                this.successMessage("Operación Exitosa!");
            })
            .catch((error) => {
                console.log(error);
                this.warnMessage("Algo salio mal!");
            });
    }

    buscarColaborador() {

        var id = sessionStorage.getItem('id_colaborador');

        axios.post("/RRHH/Colaboradores/GetNamesHuella/" + id, {})
            .then((response) => {
                console.log(response.data)
                this.setState({
                    Id: response.data.Id,
                    tipo_identificacion: response.data.TipoIdentificacionNombre,
                    nro_identificacion: response.data.Identificacion,
                    nombres_apellidos: response.data.NombresApellidos,
                    key_form: Math.random(),
                    colaborador: response.data
                })
            })
            .catch((error) => {
                this.warnMessage("Existe un inconveniente inténtelo más tarde");
            });

    }

    clearStates() {
        this.setState({
            nro_identificacion: '',
            tipo_identificacion: '',
            nombres: '',
            apellidos: '',
            lsHuellas: []
        })

        this.Regresar();
    }


    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
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

    GetTiposItentificacion() {

        let codigos = [];

        codigos = ['TIPOINDENTIFICACION'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                this.cargarCatalogos(response.data);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    cargarCatalogos(data) {
        data.forEach(e => {
            var catalogo = JSON.parse(e);
            var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
            if (codigoCatalogo == 'TIPOINDENTIFICACION') {
                this.setState({ tiposIdentificacion: catalogo })
                this.getFormSelectTipoIdent();
                return;
            }
        });
    }

    getFormSelectTipoIdent() {
        return (
            this.state.tiposIdentificacion.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    onHide() {
        this.setState({ visible: false })
    }

    showForm(id) {
        this.setState({ mode: 'huella', id_huella: id })
    }

    hideForm() {
        this.ActualizarListaHuellas();
        this.setState({ mode: 'lista' })
    }

    Regresar() {
        /* Plocalizacion del sitio */
        var currentLocation = window.location;

        var lugar = sessionStorage.getItem('regresar_huella');
        if(lugar == 'visita'){
            return (
                window.location.href = "/RRHH/Colaboradores/CrearUsuarioExterno"
            );
        }else{
            return (
                window.location.href = currentLocation.origin + "/RRHH/Colaboradores/Vista/"
            );
        }
    }
}

ReactDOM.render(
    <HuellaDigital />,
    document.getElementById('content-crear-huella')
);