import React from 'react';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import moment from 'moment';
export default class Bajas extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            motivo_baja: '',
            fecha_baja: '',
            detalle: '',
            archivo: '',
            entrevista: false,
            errores: [],
            formIsValid: false,
            tiposMotivosBajas: [],
            file: null,
            loading: true,
            encargado_personal: '',
            tiposEncargadoPersonal: [],
            archivoNombre: '',
        }

        this.handleChange = this.handleChange.bind(this);
        this.handleChangeMotivo = this.handleChangeMotivo.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.Guardar = this.Guardar.bind(this);
        this.onUpload = this.onUpload.bind(this);
        this.descargarFile = this.descargarFile.bind(this);

        this.getFormMotivosBajas = this.getFormMotivosBajas.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
    }

    componentDidMount() {
        this.GetCatalogos();
    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div className="row">
                    <div className="col-xs-12 col-md-12">
                        <form onSubmit={this.handleSubmit}>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="motivo_baja">* Motivo de Baja: </label>
                                        <select value={this.state.motivo_baja} onChange={this.handleChangeMotivo} className="form-control" name="motivo_baja" required>
                                            <option value="">Seleccione...</option>
                                            {this.getFormMotivosBajas()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["motivo_baja"]}</span>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="fecha_baja">* Fecha de Baja: </label>
                                        <input type="date" id="fecha_baja" className="form-control" value={this.state.fecha_baja} onChange={this.handleChange} name="fecha_baja" />
                                        <span style={{ color: "red" }}>{this.state.errores["fecha_baja"]}</span>
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="detalle">* Detalle de Baja: </label>
                                        <input type="text" id="detalle" className="form-control" value={this.state.detalle} onChange={this.handleChange} name="detalle" required />
                                        <span style={{ color: "red" }}>{this.state.errores["detalle"]}</span>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="archivo">Archivo de Respaldo: </label><br />
                                        <input type="file" id="file_respaldo" accept=".xls,.png,.jpg,.doc,.xlsx,.docx,.pdf" onChange={(e) => this.onUpload(e)} />
                                        <br />
                                        <div style={{ marginTop: '10px' }}><a href="#" onClick={() => this.descargarFile()}> {this.state.archivoNombre}</a> </div>
                                    </div>
                                </div>
                            </div>
                            {this.state.entrevista && 
                            <div className="row">
                                <div className="col">
                                    <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '32px' }}>
                                        <label htmlFor="entrevista" style={{ width: '285px' }}>Requiere Entrevista: </label>
                                        <input type="checkbox" id="entrevista" className="form-control" checked={this.state.entrevista} onChange={this.handleInputChange} name="entrevista" style={{ marginTop: '5px', marginLeft: '-90%' }} disabled/*={!this.state.entrevista}*/ />
                                    </div>
                                </div>
                                <div className="col">
                                </div>
                            </div>
                            }

                            <br />
                            <div className="form-group">
                                <div className="col">
                                    <button type="button" onClick={this.Guardar} className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                    <button onClick={() => this.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>

                                </div>
                            </div>

                        </form>
                    </div>
                </div>
            </BlockUi>
        )
    }

    onUpload(event) {
        console.log('e', event.target.files[0]);
        var file = event.target.files[0];
        if (file != null) {

            if (file >= 2 * 1024 * 1024) {
                this.props.warnMessage("El archivo solo puede ser de máximo 2MB");
                document.getElementById("file_respaldo").value = "";
                this.setState({ file: [], archivoNombre: '' });
                return;
            } /*else if (!file.type.match('application/vnd') && !file.type.match('image/png') && !file.type.match('image/jpeg')) {
                this.props.warnMessage("No puede subir archivos de ese formato");
                document.getElementById("file_respaldo").value = "";
                this.setState({ file: [], archivoNombre: '' });
                return;
            } */else {
                this.props.successMessage("Archivo Cargado!");
                this.setState({ file: file, archivoNombre: file.name });

            }

        } else {
            console.log("error llamada");
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


    handleValidation() {
        let errors = {};
        this.state.formIsValid = true;
        var today = moment().format("YYYY-MM-DD");
        var check = moment(today, 'YYYY/MM/DD');
        var month = check.format('M');
        var last = moment().endOf('month').format('D');

        if (!this.state.motivo_baja) {
            this.state.formIsValid = false;
            errors["motivo_baja"] = "El campo Motivo de Baja es obligatorio.";
        }
        if (!this.state.fecha_baja) {
            this.state.formIsValid = false;
            errors["fecha_baja"] = "El campo Fecha de Baja es obligatorio.";
        } else {
            console.log('Nuevo')
            var fecha = moment(this.state.fecha_baja, 'YYYY/MM/DD');
            console.log('month', month, fecha.format('M'), last)
            if (this.state.fecha_baja < moment(this.props.colaborador.fecha_ingreso).format("YYYY-MM-DD")) {
                console.log('true');
                this.state.formIsValid = false;
                errors["fecha_baja"] = "Fecha no puede ser menor a Fecha Ingreso.";
            }
           /* if (month != fecha.format('M')) {
                if (check.format('D') != last) {
                    this.state.formIsValid = false;
                    errors["fecha_baja"] = "La Fecha de Baja debe estar dentro del mes actual";
                }else if(fecha.format('D') > 5){
                    this.state.formIsValid = false;
                    errors["fecha_baja"] = "Debe ingresar una fecha hasta el 5 del próximo mes";
                }

            }
            */
        }
        if (!this.state.detalle) {
            this.state.formIsValid = false;
            errors["detalle"] = "El campo Detalle de Baja es obligatorio.";
        }


        this.setState({ errores: errors });
    }

    Guardar() {

        this.handleValidation();
        console.log('Guardar', this.state.formIsValid)
        if (this.state.formIsValid == true) {
            this.setState({ loading: true })
            // if(this.state.file!=null){
            const formData = new FormData();
            formData.append('estado', 'ACTIVA')
            formData.append('requiere_entrevista', this.state.entrevista)
            formData.append('detalle_baja', this.state.detalle)
            formData.append('fecha_baja', this.state.fecha_baja)
            formData.append('catalogo_motivo_baja_id', this.state.motivo_baja)
            formData.append('ColaboradoresId', this.props.id_colaborador)
            formData.append('UploadedFile', this.state.file)
            const config = { headers: { 'content-type': 'multipart/form-data' } }

            axios.post("/RRHH/Colaboradores/CreateBajaApi/", formData, config)
                .then((response) => {
                    this.setState({ loading: false })
                    this.props.successMessage("Información de Baja Guardada!");
                    this.Regresar();
                })
                .catch((error) => {
                    this.setState({ loading: false })
                    this.props.warnMessage("Algo salio mal!");
                });
            // }

        }

    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['MOTIVOBAJA', 'ENCARGADO'];

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
            switch (codigoCatalogo) {
                case 'MOTIVOBAJA':
                    this.setState({ tiposMotivosBajas: catalogo })
                    this.getFormMotivosBajas();
                    return;
                case 'ENCARGADO':
                    this.setState({ tiposEncargadoPersonal: catalogo })
                    console.log('encargado personal', sessionStorage.getItem('enargado_personal_id'))
                    catalogo.forEach(e => {
                        if (e.codigo.replace(/ /g, "") == "M53" && e.Id == sessionStorage.getItem('enargado_personal_id')) {
                            this.state.encargado_personal = true;
                            console.log('encargado', this.state.encargado_personal)
                            return;
                        }
                    });
                    return;
            }

        });
        this.setState({ loading: false })
    }

    getFormMotivosBajas() {
        return (
            this.state.tiposMotivosBajas.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    Regresar() {
        return (
            window.location.href = "/RRHH/Colaboradores/IndexBajas/"
        );
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleChangeMotivo(event) {
        this.setState({ [event.target.name]: event.target.value });
        var motivo = this.state.tiposMotivosBajas.filter(c => c.Id == parseInt(event.target.value));
        if (motivo.length > 0) {
            if (motivo[0].codigo.replace(/ /g, "") == "REN" && this.state.encargado_personal == true) {
                this.state.entrevista = true;
            } else {
                this.state.entrevista = false;
            }
        } else {
            this.state.entrevista = false;
        }
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }
}