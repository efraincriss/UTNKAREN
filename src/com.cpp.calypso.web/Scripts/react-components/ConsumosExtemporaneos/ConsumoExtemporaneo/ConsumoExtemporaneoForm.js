import React from "react";
import axios from "axios";
import Field from "../../Base/Field-v2";
import http from "../../Base/HttpService";
import config from "../../Base/Config";

import { Button } from "shards-react";
import { CONTROLLER_CONSUMO_EXTEMPORANEO, MODULO_PROVEEDOR } from "../../Base/Strings";
import CargarArchivo from "./cargarArchivo";
export default class ConsumoExtemporaneoForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {

            errors: {},
            formData: {
                Id: props.consumo.Id ? props.consumo.Id : 0,
                TipoComidaId: props.consumo.TipoComidaId ? props.consumo.TipoComidaId : 0,
                Fecha: props.consumo.Fecha ? props.consumo.Fecha : '', //moment(new Date()).format("YYYY-MM-DD")
                ProveedorId: props.consumo.ProveedorId ? props.consumo.ProveedorId : 0,
                DocumentoRespaldoId: props.consumo.DocumentoRespaldoId ? props.consumo.DocumentoRespaldoId : null
            },
            uploadFile: '',
        }
    }

    initData() {
        let dataInit = {
            formData: {
                Id: 0,
                TipoComidaId: 0,
                Fecha: '', //moment(new Date()).format("YYYY-MM-DD")
                ProveedorId: 0,
                DocumentoRespaldoId: null
            }
        };
        return dataInit;
    }

    render() {
        return (
            <div>
                <form onSubmit={this.handleSubmit}>
                    <div className="row">
                        <div className="col">
                            <Field
                                name="Fecha"
                                label="Fecha Registro"
                                required
                                type="date"
                                edit={true}
                                readOnly={false}
                                value={this.state.formData.Fecha}
                                onChange={this.handleChange}
                                error={this.state.errors.Fecha}
                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="ProveedorId"
                                required
                                value={this.state.formData.ProveedorId}
                                label="Proveedor"
                                options={this.props.proveedores}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.ProveedorId}
                                readOnly={false}
                                placeholder="Seleccione.."
                                filterPlaceholder="Seleccione.."
                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="TipoComidaId"
                                required
                                value={this.state.formData.TipoComidaId}
                                label="Tipo Comida"
                                options={this.props.tiposComidas}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.TipoComidaId}
                                readOnly={false}
                                placeholder="Seleccione.."
                                filterPlaceholder="Seleccione.."
                            />
                        </div>
                    </div>

                    <div className="row" style={{marginBottom: '1em'}}>
                        <div className="col">
                            <CargarArchivo
                                handleChange={this.handleChangeUploadFile}
                                label="Documento Respaldo"
                                onClear={this.clearUploadFile}
                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Button type="submit" pill>Guardar</Button>
                        </div>

                        <div className="col" style={{marginRight: '0.2em'}}>
                            <Button onClick={() => this.props.toggle()} pill>Cerrar</Button>
                        </div>
                    </div>
                </form>
            </div>
        )
    }

    handleSubmit = (event) => {
        event.preventDefault();

        if (!this.isValid()) {
            return;
        }
        this.props.blockScreen();

        if (this.state.formData.Id === 0) {
            this.crearConsumoExtemporaneo();
        } else {
            this.actualizarConsumoExtemporaneo();
        }
    }

    isValid() {
        const errors = {};

        if (!this.state.formData.TipoComidaId || this.state.formData.TipoComidaId === 0) {
            errors.TipoComidaId = 'Tipo Comida es requerido';
        }

        if (!this.state.formData.Fecha || this.state.formData.Fecha.length === '') {
            errors.Fecha = 'Fecha es requerida';
        }


        if (this.state.formData.ProveedorId === 0) {
            errors.ProveedorId = 'Proveedor es requerido';
        }

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }


    crearConsumoExtemporaneo = () => {
        let url = `/${MODULO_PROVEEDOR}/${CONTROLLER_CONSUMO_EXTEMPORANEO}/CrearConsumoExtemporaneo`;

        const formData = new FormData();
        formData.append('uploadFile', this.state.uploadFile);
        formData.append('Id', this.state.formData.Id);
        formData.append('TipoComidaId', this.state.formData.TipoComidaId);
        formData.append('Fecha', this.state.formData.Fecha);
        formData.append('ProveedorId', this.state.formData.ProveedorId);

        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };

        http.post(url, formData, config)
            .then((response) => {

                let data = response.data;
                console.log(data);

                if (data.created === true) {
                    this.setState({
                        formData: this.initData()
                    });
                    this.props.loadConsumosExtemporaneos();
                    this.props.toggle()
                    this.props.showSuccess("Consumo extemporaneo guardado")
                } else {
                    this.props.showWarning(data.result);
                }
                this.props.unlockScreen()
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn('Ocurrió un error, guardando el consumo');
                this.props.unlockScreen()
            });
    }

    actualizarConsumoExtemporaneo = () => {
        let url = `/${MODULO_PROVEEDOR}/${CONTROLLER_CONSUMO_EXTEMPORANEO}/ActualizarConsumoExtemporaneo`;

        const formData = new FormData();
        formData.append('uploadFile', this.state.uploadFile);
        formData.append('Id', this.state.formData.Id);
        formData.append('TipoComidaId', this.state.formData.TipoComidaId);
        formData.append('Fecha', this.state.formData.Fecha);
        formData.append('ProveedorId', this.state.formData.ProveedorId);
        if(this.state.formData.DocumentoRespaldoId != null) {
            formData.append('DocumentoRespaldoId', this.state.formData.DocumentoRespaldoId);
        }

        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };

        http.post(url, formData, config)
            .then((response) => {

                let data = response.data;
                console.log(data);

                if (data.updated === true) {
                    this.setState({
                        formData: this.initData()
                    });
                    this.props.loadConsumosExtemporaneos();
                    this.props.toggle()
                    this.props.showSuccess("Consumo extemporaneo actualizado")
                } else {
                    this.props.showWarn('Ocurrió un error al validar los datos');
                }

                this.props.unlockScreen()
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn('Ocurrió un error, actualizando el consumo');
                this.props.unlockScreen()
            });
    }


    handleChange = (event) => {
        const target = event.target;
        if (event.target.files) {
            console.log("Archivo")
            console.log(event.tarjet)
            if (event.files.length > 0) {
                let uploadFile = event.files[0];
                this.setState({
                    uploadFile: uploadFile
                });
            }
        }
        else {
            const value = target.type === "checkbox" ? target.checked : target.value;
            const name = target.name;
            const updatedData = {
                ...this.state.formData
            };
            updatedData[name] = value;
            this.setState({
                formData: updatedData,
            });
        }

    }

    handleChangeUploadFile = event => {
        if (event.files) {
            if (event.files.length > 0) {
                let uploadFile = event.files[0];
                this.setState({
                    uploadFile: uploadFile
                });
            } else {
                this.props.showWarn(FRASE_ERROR_SELECCIONA_ARCHIVO);
            }
        } else {
            this.props.showWarn(FRASE_ERROR_SELECCIONA_ARCHIVO);
        }
    }

    handleSubmitUploadFile = () => {
        this.props.blockScreen();
        let url = '';
        url = `/Accesos/ConsultaPublica/SubirArchivoArchivo`;

        const formData = new FormData();
        formData.append('uploadFile', this.state.uploadFile);
        formData.append('Id', this.state.formData.Id);
        formData.append('TipoComidaId', this.state.formData.TipoComidaId);
        formData.append('Fecha', this.state.formData.Fecha);
        formData.append('ProveedorId', this.state.formData.ProveedorId);

        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };

        http.post(url, formData, config)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({
                        uploadFile: ''
                    });
                    this.props.showSuccess(FRASE_PDF_SUBIDO)
                    this.ocultarUploadPDF()
                    this.OcultarListaDistribucion()
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn(FRASE_ERROR_GLOBAL);
                this.props.unlockScreen();
            });

    }

    clearUploadFile = () => {
        this.setState({uploadFile: ''})
    }


    setData = (name, value) => {
        let updatedData = {
            ...this.state.formData
        };
        if (name === "ProveedorId") {
            updatedData["TipoComidaId"] = 0;
        }
        updatedData[name] = value;
        this.setState({
            formData: updatedData
        });
        if (name === "ProveedorId") {
            this.props.loadTiposComidas(value);
        }
    }
}