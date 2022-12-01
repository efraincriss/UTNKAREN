import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';



export default class RegistrarReintegro extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            Id: '',
            colaboradoresAusentismo: [],
            tipo_identificacion_list: [],
            tipo_genero: [],
            tipo_grupo_personal: [],
            tipo_discapacidad: [],
            tipo_ausentismo: [],
            errores: [],
            tipo_identificacion: '',
            colaborador_id: '',
            apellidos: '',
            nombres: '',
            telefono: '',
            genero: '',
            grupo_personal: '',
            isDiscapacidad: false,
            discapacidad: '',
            ausentismo: '',
            fecha_desde: '',
            fecha_hasta: '',
            fecha_reintegro: '',
            inputKey: '',
            file: '',
            motivo: '',
            colaborador: [],
            errores: [],
            nro_identificacion: '',
            archivoNombre: '',
            formIsValid: true,
            vista_discapacidad: 'none',
            loading: true,
            nombre_grupo_personal: '',
            nombre_identificacion: '',
            nombre_ausentismo: '',
            nombre_genero: ''
        }

        this.dateFormat = this.dateFormat.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleCheck = this.handleCheck.bind(this);
        this.handleValidation = this.handleValidation.bind(this);

        /* Guardar Reintegro */
        this.guardarReintegro = this.guardarReintegro.bind(this);
        this.buscarColaborador = this.buscarColaborador.bind(this);
        this.buscarAusentismo = this.buscarAusentismo.bind(this);
        this.onUpload = this.onUpload.bind(this);
        this.descargarFile = this.descargarFile.bind(this);
        this.regresar = this.regresar.bind(this);

        /* Selects */
        this.getFormSelectTipoIdent = this.getFormSelectTipoIdent.bind(this);
        this.getFormSelectGenero = this.getFormSelectGenero.bind(this);
        this.getFormSelectGrupo = this.getFormSelectGrupo.bind(this);
        this.getFormSelectDiscapacidad = this.getFormSelectDiscapacidad.bind(this);
        this.getFormSelectAusentismo = this.getFormSelectAusentismo.bind(this);


        /* Mensajes al usuario */
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
    }


    componentDidMount() {
        this.GetCatalogos();
        this.setState({ Id: sessionStorage.getItem('ausentismo_id') })

        if (sessionStorage.getItem('ausentismo_id') != 0) {
            this.buscarAusentismo(sessionStorage.getItem('ausentismo_id'));
            this.buscarColaborador(sessionStorage.getItem('colaborador_id'));
        }
    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    <form onSubmit={this.handleSubmit} >
                        <div className="row">
                        <div className="col-1"></div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Tipo de Identificación:</b> {this.state.nombre_identificacion} </label>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>No. de Identificación:</b> {this.state.nro_identificacion} </label>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Apellidos Nombres:</b> {this.state.nombres} </label>
                                </div>
                            </div>
                            <div className="col-1"></div>
                        </div>
                        <div className="row">
                        <div className="col-1"></div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Género:</b> {this.state.nombre_genero} </label>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Teléfono:</b> {this.state.telefono} </label>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Agrupación para Requisitos:</b> {this.state.nombre_grupo_personal}</label>
                                </div>
                            </div>
                            <div className="col-1"></div>
                        </div>
                        <div className="row">
                        <div className="col-1"></div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Tipo de Ausentismo:</b> {this.state.nombre_ausentismo} </label>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Fecha Desde:</b> {this.state.fecha_desde} </label>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Fecha Hasta:</b> {this.state.fecha_hasta}</label>
                                </div>
                            </div>
                            <div className="col-1"></div>
                        </div>
                       
                        <div className="row">
                        <div className="col-1"></div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label"><b>* Fecha de Reintegro:</b> </label>
                                    <input type="date" id="fecha_reintegro" className="form-control" value={this.state.fecha_reintegro} style={{ width: '95%' }} onChange={this.handleChange} name="fecha_reintegro" />
                                    <span style={{ color: "red" }}>{this.state.errores["fecha_reintegro"]}</span>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label"><b>Archivo de Respaldo:</b> </label>
                                    <br />
                                    <input type="file" id="file_reintegro" accept="application/pdf" onChange={this.onUpload} key={this.state.inputKey || ''} />
                                    <br />
                                    <div style={{ marginTop: '10px' }}><a href="#" onClick={() => this.descargarFile()}> {this.state.archivoNombre}</a> </div>
                                    <br />
                                    <span style={{ color: "red" }}>{this.state.errores["file"]}</span>
                                </div>
                            </div>

                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label"><b>* Motivo de Reintegro:</b> </label>
                                    <textarea id="motivo" className="form-control" rows="2" cols="100" name="motivo" value={this.state.motivo} onChange={this.handleChange} style={{ width: '95%' }} > </textarea>
                                    <span style={{ color: "red" }}>{this.state.errores["motivo"]}</span>
                                </div>
                            </div>
                            <div className="col-1"></div>
                        </div>
                        <hr />
                        <div className="row">
                            <div className="col-sm-2"></div>
                            <div className="col-sm-4">
                                <button type="button" onClick={() => { if (window.confirm('¿Está seguro que desea guardar la información?')) this.guardarReintegro(); }} className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                <button type="button" onClick={() => this.regresar()} className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                            </div>
                            <div className="col-sm-4"></div>
                            <div className="col-sm-2"></div>
                        </div>
                        {/* </div> */}
                    </form>
                </div >
            </BlockUi>
        )
    }

    onUpload(event) {

        let randomString = Math.random().toString(36);
        var file = event.target.files[0];

        if (file != null) {

            if (file.size >= 2 * 1024 * 1024) {
                this.warnMessage("El archivo solo puede ser de máximo 2MB");
                this.setState({ inputKey: randomString, file: null, archivoNombre: '' });
                return;
            }

            if (!file.type.match('application/pdf')) {
                this.warnMessage("Solo se pueden subir archivos PDF");
                this.setState({ inputKey: randomString, file: null, archivoNombre: '' });
                return;
            }

            this.setState({ file: file, archivoNombre: file.name });
            this.successMessage("Archivo Procesado!");
        } else {
            this.setState({ inputKey: randomString, file: null, archivoNombre: '' });
        }
    }

    descargarFile() {

        var file = this.state.file;
        var filename = file.name;
        var blob = new Blob([file]);
        var url = URL.createObjectURL(blob);

        var element = document.createElement('a');
        element.setAttribute('href', url);
        element.setAttribute('download', filename);

        element.style.display = 'none';
        document.body.appendChild(element);

        element.click();

        document.body.removeChild(element);

    }

    buscarAusentismo(id) {
        /* Buscar el ausentismo */
        axios.post("/RRHH/ColaboradoresAusentismo/GetAusentismo/" + id, {})
            .then((response) => {
                if (response.data != "NO") {
                    this.setState({ colaboradoresAusentismo: response.data })
                    this.setState({
                        ausentismo: response.data.catalogo_tipo_ausentismo_id,
                        fecha_desde: this.dateFormat(response.data.fecha_inicio),
                        fecha_hasta: this.dateFormat(response.data.fecha_fin),
                        nombre_ausentismo: response.data.TipoAusentismo.nombre,
                        loading: false
                    })
                }
            })
            .catch((error) => {
                console.log(error);
            });
    }

    buscarColaborador(id) {

        /* Buscamos al colaborador */
        axios.post("/RRHH/Colaboradores/GetColaboradorApi/" + id, {})
            .then((response) => {
                var apellido = response.data.segundo_apellido == null ? '' : response.data.segundo_apellido;
                this.setState({
                    colaborador: response.data,
                    colaborador_id: response.data.Id,
                    nombres: response.data.nombres,
                    genero: response.data.catalogo_genero_id,
                    nombre_genero : response.data.nombre_genero,
                    nombre_grupo_personal: response.data.nombre_grupo_personal,
                    grupo_personal: response.data.catalogo_grupo_personal_id,
                    isDiscapacidad: response.data.discapacidad == null ? '' : response.data.discapacidad,
                    telefono: response.data.telefono == null ? '' : response.data.telefono,
                    tipo_identificacion: response.data.catalogo_tipo_identificacion_id,
                    nombre_identificacion: response.data.nombre_identificacion,
                    nro_identificacion: response.data.numero_identificacion,
                    discapacidad: response.data.tipo_discapacidad == null ? '' : response.data.tipo_discapacidad
                })

                if (response.data.tipo_discapacidad != null) {
                    this.setState({ vista_discapacidad: 'block' })
                }
            })
            .catch((error) => {
                console.log(error);
            });

    }

    clearStates() {

        this.setState({
            nro_identificacion: '',
            tipo_identificacion: '',
            nombres: '',
            apellidos: '',
            telefono: '',
            genero: '',
            grupo_personal: '',
            isDiscapacidad: false,
            discapacidad: '',
            colaborador_id: '',
            ausentismo: '',
            fecha_desde: '',
            fecha_hasta: '',
            colaboradoresAusentismo: [],
            tipo_identificacion_list: [],
            tipo_genero: [],
            tipo_grupo_personal: [],
            tipo_discapacidad: [],
            tipo_ausentismo: [],
            errores: [],
            fecha_reintegro: '',
            file: '',
            archivoNombre: '',
            motivo: '',
            vista_discapacidad: 'none',
        }, this.regresar())
    }

    cargarCatalogos(data) {
        data.forEach(e => {
            var catalogo = JSON.parse(e);
            var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
            if (codigoCatalogo == 'TIPOINDENTIFICACION') {
                this.setState({ tipo_identificacion_list: catalogo })
                this.getFormSelectTipoIdent();
                return;
            }
            if (codigoCatalogo == 'GENERO') {
                this.setState({ tipo_genero: catalogo })
                this.getFormSelectGenero();
                return;
            }
            if (codigoCatalogo == 'GRUPOPERSONAL') {
                this.setState({ tipo_grupo_personal: catalogo })
                this.getFormSelectGrupo();
                return;
            }
            if (codigoCatalogo == 'TIPODISCAPACIDAD') {
                this.setState({ tipo_discapacidad: catalogo })
                this.getFormSelectDiscapacidad();
                return;
            }
            if (codigoCatalogo == 'TIPOAUSENTISMO') {
                this.setState({ tipo_ausentismo: catalogo })
                this.getFormSelectGrupo();
                return;
            }
        });
    }

    handleValidation() {
        let errors = {};
        this.setState({ formIsValid: true });

        if (!this.state.fecha_reintegro) {
            this.state.formIsValid = false;
            errors["fecha_reintegro"] = "El campo Fecha Reintegro es obligatorio.";
        }

        if ((new Date(this.state.fecha_reintegro).getTime() > new Date(this.state.fecha_hasta).getTime())) {
            this.state.formIsValid = false;
            errors["fecha_reintegro"] = "Fecha de Reintegro no puede ser mayor que Fecha Hasta.";
        }

        if ((new Date(this.state.fecha_reintegro).getTime() < new Date(this.state.fecha_desde).getTime())) {
            this.state.formIsValid = false;
            errors["fecha_reintegro"] = "Fecha de Reintegro no puede ser menor que Fecha Desde.";
        }

        // if (!this.state.file) {
        //     this.state.formIsValid = false;
        //     errors["file"] = "El Archivo de Respaldo es obligatorio.";
        // }

        if (!this.state.motivo) {
            this.state.formIsValid = false;
            errors["motivo"] = "El campo Motivo es obligatorio.";
        }

        this.setState({ errores: errors });
    }

    getFormSelectAusentismo() {
        return (
            this.state.tipo_ausentismo.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectDiscapacidad() {
        return (
            this.state.tipo_discapacidad.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
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

    getFormSelectGenero() {
        return (
            this.state.tipo_genero.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectTipoIdent() {
        return (
            this.state.tipo_identificacion_list.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }


    GetCatalogos() {

        let codigos = [];

        codigos = ['TIPOINDENTIFICACION', 'GENERO', 'TIPODISCAPACIDAD', 'GRUPOPERSONAL', 'TIPOAUSENTISMO'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                this.cargarCatalogos(response.data);
            })
            .catch((error) => {
                console.log(error);
            });
    }


    guardarReintegro() {

        this.handleValidation();

        if (this.state.formIsValid == true) {
            this.setState({ loading: true });
            console.log('file', this.state.file);

            const formData = new FormData();
            formData.append('idAusentismo', this.state.Id)
            formData.append('fecha_reintegro', this.state.fecha_reintegro)
            formData.append('motivo', this.state.motivo)
            if (this.state.file == '') {
                formData.append('UploadedFile', null)
            } else {
                formData.append('UploadedFile', this.state.file)
            }
            const config = { headers: { 'content-type': 'multipart/form-data' } }
            axios.post("/RRHH/ColaboradoresAusentismo/CrearReintegro/", formData, config)
                .then((response) => {
                    this.setState({ loading: false });
                    this.successMessage("Reintegro registrado!");
                    setTimeout(
                        function () {
                            this.regresar();
                        }.bind(this), 2000
                    );

                })
                .catch((error) => {
                    this.setState({ loading: false });
                    this.warnMessage("Algo salio mal!");
                });
        } else {
            this.warnMessage("Se ha encontrado errores, por favor revisar el formulario");
        }
    }


    dateFormat(date) {

        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');

    }

    handleCheck() {
        this.setState({ isDiscapacidad: !this.state.isDiscapacidad });
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

    regresar() {
        return (
            window.location.href = "/RRHH/ColaboradoresAusentismo/Index/"
        );
    }
}

ReactDOM.render(
    <RegistrarReintegro />,
    document.getElementById('content-ColaboradoresReintegro')
);
