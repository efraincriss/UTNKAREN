import React from 'react';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';

export default class Fotografia extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            Id: null,
            tipo_identificacion: '',
            apellidos: '',
            nombres: '',
            colaborador: [],
            errores: [],
            nro_identificacion: '',
            tiposIdentificacion: [],
            file: null,
            imagenBase64: '',
            formIsValid: true,
            display: 'none',
            registro: 'none',
            loading: false
        }
        this.getFormSelectTipoIdent = this.getFormSelectTipoIdent.bind(this);
        this.GetTiposItentificacion = this.GetTiposItentificacion.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.clearStates = this.clearStates.bind(this);
        this.onUpload = this.onUpload.bind(this);
        this.getFotografia = this.getFotografia.bind(this);
        this.deleteFotografia = this.deleteFotografia.bind(this);
        this.subirArchivo = this.subirArchivo.bind(this);
        this.handleValidation = this.handleValidation.bind(this);

        /* Mensajes al usuario */
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
    }

    componentWillReceiveProps(nextProps) {
        this.setState({
            nro_identificacion: nextProps.colaborador.numero_identificacion != null ? nextProps.colaborador.numero_identificacion : "",
            nombres: nextProps.colaborador.nombres_apellidos,
            tipo_identificacion: nextProps.nombre_identificacion,
            colaborador: nextProps.colaborador != [] ? nextProps.colaborador : [],
            Id: nextProps.colaborador.Id != null ? nextProps.colaborador.Id : "",
            display: 'none',
            registro: 'none',
            errores: []
        })
    }



    componentDidMount() {
        this.GetTiposItentificacion();
    }

    render() {
        return (
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <form>
                    <div className="col-sm-12" style={{ margin: 'auto' }}>


                        <div className="row">
                            <div className="col-1"></div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Tipo de Identificación:</b> {this.props.tipo_identificacion} </label>

                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>No. de Identificación:</b> {this.props.nro_identificacion} </label>
                                </div>
                            </div>
                            <div className="col-1"></div>
                        </div>
                        <div className="row">
                            <div className="col-1"></div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Apellidos Nombres:</b> {this.props.nombres_apellidos} </label>
                                </div>
                            </div>
                            <div className="col-1"></div>
                        </div>

                        <div className="row">
                            <div className="col-1"></div>
                            <div className="form-group">
                                <label htmlFor="label"><b>* Fotografía:</b> </label><br />
                                <input type="file" id="cargar_foto" accept=".png,.jpg" onChange={(e) => this.onUpload(e)} />
                                {/* <FileUpload chooseLabel="Seleccionar" mode="basic" name="demo[]" accept="image/*" maxFileSize={1000000} onUpload={this.onUpload} auto={true} /> */}
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-1"></div>
                            <br />
                            <div style={{ height: '150px' }}>
                                <div style={{ display: this.state.registro }}>
                                    No se ha registrado fotografía
                                            </div>
                                <BlockUi tag="div" blocking={this.state.loading}>
                                    <div style={{ width: '110px', display: this.state.display }} >
                                        <img id="FPImage1" height="150" width="120" />
                                    </div>
                                </BlockUi>
                                <span style={{ color: "red" }}>{this.state.errores["foto"]}</span>
                            </div>
                            <br /><br />
                        </div>
                        <br />
                        <div className="row">
                            <div className="col-1"></div>
                            <br /><br />
                            {/* <button type="button" onClick={() => this.getFotografia()} className="btn btn-outline-primary">Ver Fotografía
                                        </button> */}
                            <div className="col-1">
                                <button type="button" onClick={() => this.deleteFotografia()} className="btn btn-outline-danger btn-sm">Eliminar</button>
                            </div>

                        </div>
                        <br />
                        <hr />
                        <div className="row">
                            <div className="col-sm-1"></div>
                            <div className="col-sm-5">
                                <button type="button" onClick={() => this.subirArchivo()} className="btn btn-outline-primary">Guardar</button>
                                <button type="button" onClick={() => this.clearStates()} className="btn btn-outline-primary" style={{ marginLeft: '3px' }}>Cancelar</button>                            </div>
                            <div className="col-sm-5"></div>
                            <div className="col-sm-1"></div>
                        </div>
                    </div>
                </form>
            </div >
        )
    }

    deleteFotografia() {

        var origen = "CAR_ARC";
        document.getElementById("FPImage1").src = "";
        this.setState({ display: 'none' })

        axios.post("/Accesos/Huella/DeleteFotografiaApi/",
            {
                Idcolaborador: this.state.Id,
                origen: origen
            })
            .then((response) => {

                if (response.data == "OK") {
                    this.successMessage("Fotografía eliminada!");
                }
            })
            .catch((error) => {
                console.log(error);
                this.warnMessage("Algo salio mal!");
            });

    }

    getFotografia() {

        var origen = "CAR_ARC";
        document.getElementById("FPImage1").src = "";

        this.setState({ loading: true })

        axios.post("/Accesos/Huella/GetArchivoFotografiaApi/",
            {
                Idcolaborador: this.state.Id,
                origen: origen
            })
            .then((response) => {

                if (response.data != "NO") {
                    document.getElementById("FPImage1").src = "data:" + response.data.tipo_contenido + ";base64," + response.data.hash;
                    this.setState({ display: 'block', registro: 'none' })

                } else {
                    this.setState({ registro: 'block', display: 'none' })
                }
            })
            .catch((error) => {
                console.log(error);
                this.warnMessage("Algo salio mal!");
            });

        this.setState({ loading: false })
    }

    subirArchivo() {

        this.handleValidation();

        if (this.state.formIsValid == true) {
            if (this.state.file != null) {
                const formData = new FormData();
                formData.append('idColaborador', this.props.Id)
                formData.append('UploadedFile', this.state.file)
                const config = { headers: { 'content-type': 'multipart/form-data' } }

                axios.post("/Accesos/Huella/CreateArchivoFotografia/", formData, config)
                    .then((response) => {
                        this.successMessage("Fotografía registrada!");
                        setTimeout(
                            function () {
                                this.props.onHide();
                            }.bind(this), 2000
                        );
                    })
                    .catch((error) => {
                        this.warnMessage("Algo salio mal!");
                    });
            }
        }
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    onUpload(event) {

        console.log('e', event.target.files[0]);
        var file = event.target.files[0];
        if (file != null) {

            if (file >= 2 * 1024 * 1024) {
                this.warnMessage("El archivo solo puede ser de máximo 2MB");
                document.getElementById("cargar_foto").value = "";
                return;
            } else if (!file.type.match('image/png') && !file.type.match('image/jpeg')) {
                this.warnMessage("No puede subir archivos de ese formato");
                document.getElementById("cargar_foto").value = "";
                return;
            } else {
                this.setState({
                    registro: 'none',
                    file: file,
                    display: 'block'
                })
                this.successMessage("Fotografía Procesada!");
                /* Convertir a base 64 para mostrar la imagen */
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onloadend = function () {

                    document.getElementById("FPImage1").src = reader.result;
                }

            }

        } else {
            console.log("Error cargar archivo");
        }

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

    GetTiposItentificacion() {

        let codigos = [];

        codigos = ['TIPOINDENTIFICACION'];

        axios.post("/Accesos/Huella/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                this.cargarCatalogos(response.data);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    handleValidation() {
        let errors = {};
        this.setState({ formIsValid: true });

        if (this.state.display == "none") {
            this.state.formIsValid = false;
            errors["foto"] = "Debe cargar una fotografía!";
        }

        this.setState({ errores: errors });
    }

    clearStates() {
        document.getElementById("FPImage1").src = "";

        this.setState({
            nro_identificacion: '',
            tipo_identificacion: '',
            nombres: '',
            apellidos: '',
            display: 'none',
            registro: 'none',
            Id: null
        }, this.props.onHide())
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