import React from 'react';
import Field from "../../../Base/Field-v2";
import { Button } from "primereact-v2/button";
import { FileUpload } from "primereact-v2/fileupload";
import config from "../../../Base/Config";
import http from "../../../Base/HttpService";
import {
    MODULO_ACCESO,
    CONTROLLER_VALIDACION_REQUISITO,
    FRASE_REQUISITO_ACTUALIZADO,
    FRASE_ERROR_GLOBAL,
    FRASE_ERROR_VALIDACIONES
} from "../../../Base/Strings";

export default class RequisitosForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            data: {
                Id: props.requisito.ColaboradorRequisitoId ? props.requisito.ColaboradorRequisitoId : 0,
                ColaboradoresId: props.ColaboradorId ? props.ColaboradorId : 0,
                RequisitosId: props.requisito.RequisitoId ? props.requisito.RequisitoId : 0,
                ArchivoId: props.requisito.ArchivoId ? props.requisito.ArchivoId : null,
                cumple: props.requisito.CumpleBool ? props.requisito.CumpleBool : false,
                fecha_emision: props.requisito.FechaEmision ? props.requisito.FechaEmision : '',
                fecha_caducidad: props.requisito.FechaCaducidad ? props.requisito.FechaCaducidad : '',
                observacion: props.requisito.Observacion ? props.requisito.Observacion : '',
                vigente: props.requisito.Vigente ? props.requisito.Vigente : true,
                AplicaCaducidad: props.requisito.AplicaCaducidad ? props.requisito.AplicaCaducidad : '',
                TiempoVigenciaMaximo: props.requisito.TiempoVigenciaMaximo ? props.requisito.TiempoVigenciaMaximo : ''

            },
            errors: {},
            uploadFile: '',
            displayMessage: '',
            keyUpload: 98
        }
    }

    componentWillReceiveProps(prevProps) {
        let updatedValues = {
            Id: prevProps.requisito.ColaboradorRequisitoId ? prevProps.requisito.ColaboradorRequisitoId : 0,
            ColaboradoresId: prevProps.ColaboradorId ? prevProps.ColaboradorId : 0,
            RequisitosId: prevProps.requisito.RequisitoId ? prevProps.requisito.RequisitoId : 0,
            ArchivoId: prevProps.requisito.ArchivoId ? prevProps.requisito.ArchivoId : null,
            cumple: prevProps.requisito.CumpleBool ? prevProps.requisito.CumpleBool : false,
            fecha_emision: prevProps.requisito.FechaEmision ? prevProps.requisito.FechaEmision : '',
            fecha_caducidad: prevProps.requisito.FechaCaducidad ? prevProps.requisito.FechaCaducidad : '',
            observacion: prevProps.requisito.Observacion ? prevProps.requisito.Observacion : '',
            vigente: prevProps.requisito.Vigente ? prevProps.requisito.Vigente : true,
            AplicaCaducidad: prevProps.requisito.AplicaCaducidad ? prevProps.requisito.AplicaCaducidad : '',
            TiempoVigenciaMaximo: prevProps.requisito.TiempoVigenciaMaximo ? prevProps.requisito.TiempoVigenciaMaximo : ''

        }
        this.setState({
            data: updatedValues
        })
    }


    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <b>Requisito: </b>{this.props.requisito.Nombre ? this.props.requisito.Nombre : "" }
                    </div>
                    <div className="col">
                        <b>Obligatorio: </b>{this.props.requisito.Obligatorio ? this.props.requisito.Obligatorio : "" }
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <Field
                            name="AplicaCaducidad"
                            value={this.state.data.AplicaCaducidad}
                            label="Aplica Caducidad?"
                            edit={true}
                            onChange={this.handleChange}
                            error={this.state.errors.AplicaCaducidad}
                            readOnly={true}
                        />
                    </div>

                    <div className="col">
                        <Field
                            name="TiempoVigenciaMaximo"
                            value={this.state.data.TiempoVigenciaMaximo}
                            label="Tiempo máximo de Vigencia"
                            edit={true}
                            onChange={this.handleChange}
                            error={this.state.errors.TiempoVigenciaMaximo}
                            readOnly={true}
                        />
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <Field
                            name="fecha_emision"
                            value={this.state.data.fecha_emision}
                            label="Fecha Emisión"
                            type="date"
                            edit={true}
                            onChange={this.handleChange}
                            error={this.state.errors.fecha_emision}
                            readOnly={false}
                        />
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Field
                            name="fecha_caducidad"
                            value={this.state.data.fecha_caducidad}
                            label="Fecha Caducidad"
                            type="date"
                            edit={true}
                            onChange={this.handleChange}
                            error={this.state.errors.fecha_caducidad}
                            readOnly={false}
                        />
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Field
                            name="cumple"
                            label="Cumple"
                            labelOption="(Si/No)"
                            type="checkbox"
                            value={this.state.data.cumple}
                            edit={true}
                            readOnly={false}
                            error={this.state.errors.cumple}
                            onChange={this.handleChange}
                        />
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <FileUpload
                            chooseLabel="Seleccionar Archivo"
                            cancelLabel="Cancelar"
                            mode="basic"
                            name="uploadFile"
                            accept=".xlsx,.xls,image/*,.doc, .docx,.ppt, .pptx,.txt,.pdf, .zip"
                            onSelect={this.handleChange}
                            onClear={this.onClear}
                            onError={this.onError}
                        />

                    </div>
                </div>
                <hr />
                <div className="row" style={{ marginTop: '0.4em' }}>
                    <div className="col">
                        <Button label="Guardar" className="p-button-rounded" onClick={() => this.handleSubmit()} icon="pi pi-save" />
                    </div>
                </div>

            </div>
        )
    }

    handleSubmit = () => {
        if (!this.isValid()) {
            this.showWarn(FRASE_ERROR_VALIDACIONES, 'Validaciones');
            return;
        }

        this.props.blockScreen();
        let url = '';
        url = `${config.apiUrl}${MODULO_ACCESO}/${CONTROLLER_VALIDACION_REQUISITO}/UpdateApi`
        console.log(url)
        
        let data = Object.assign({}, this.state.data);
        const formData = new FormData();
        for (var key in data) {
            if (data[key] !== null)
                formData.append(key, data[key]);
            else
                formData.append(key, '');
        }
        formData.append('uploadFile', this.state.uploadFile);

        const configuration = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };

        http.post(url, formData, configuration)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    if (data.aproved === true) {
                      
                        this.props.showSuccess(FRASE_REQUISITO_ACTUALIZADO)
                        this.props.submitBusqueda();
                        this.stt
                        this.props.ocultarUploadFile()
                        this.setState({
                            uploadFile: ''
                        });
                    } else {
                        this.showWarn(data.errors);
                        this.props.unlockScreen();
                        this.setState({
                            uploadFile: ''
                        });
                    }
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.showWarn(message);
                    this.setState({
                        uploadFile: ''
                    });
                    this.props.unlockScreen();
                }

            })
            .catch((error) => {
                console.log(error);
                this.showWarn(FRASE_ERROR_GLOBAL);
                this.props.unlockScreen();
            });
    }

    onClear = () => {
        this.setState({ uploadFile: '' })
    }

    isValid = () => {
        const errors = {};
        let fechaEmision = this.state.data.fecha_emision
        let fechaCaducidad = this.state.data.fecha_caducidad
        let aplicaCaducidad = this.state.data.AplicaCaducidad

        console.log(aplicaCaducidad)

        if (fechaEmision === '') {
            errors.fecha_emision = 'Ingresa una fecha de emisión';
        }

        if(aplicaCaducidad === 'SI'){
            if (fechaCaducidad === '') {
                errors.fecha_caducidad = 'Ingresa una fecha de caducidad';
            }
        }
        

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }



    handleChange = (event) => {
        if (event.files) {
            if (event.files.length > 0) {
                let uploadFile = event.files[0];
                if(uploadFile.size > config.maxFileSize){
                    this.setState({keyUpload: Math.random()})
                    this.showWarn(`Tamaño del archivo inválido, el tamaño máximo es ${this.formatSize(config.maxFileSize)}`, 'Validaciones');
                } else {
                    this.setState({
                        uploadFile: uploadFile
                    });
                }
                
            }
        } else {
            const target = event.target;
            const value = target.type === "checkbox" ? target.checked : target.value;
            const name = target.name;
            const updatedData = {
                ...this.state.data
            };
            updatedData[name] = value;
            this.setState({
                data: updatedData
            });
        }
    }

    formatSize = (bytes) => {
        if (bytes === 0) {
            return '0 B';
        }
        var k = 1000,
            dm = 3,
            sizes = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
            i = Math.floor(Math.log(bytes) / Math.log(k));

        return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
    }

    warn = (type) => {
        abp.notify.error(this.state.displayMessage, type);
    }

    showWarn = (displayMessage, type= 'Error') => {
        this.setState({ displayMessage }, () => this.warn(type))
    }

}