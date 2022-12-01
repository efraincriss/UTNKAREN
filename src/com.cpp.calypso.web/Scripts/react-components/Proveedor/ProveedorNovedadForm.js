import React, { Component } from 'react';
import BlockUi from 'react-block-ui';

import moment from 'moment';

import http from '../Base/HttpService';
import ActionForm from '../Base/ActionForm';
import config from '../Base/Config';
import Field from '../Base/Field';
import wrapForm from '../Base/WrapForm';

class ProveedorNovedadForm extends Component {

    constructor(props) {
        super(props);

        this.state = {
            data: this.initData(),
            uploadFile: '',
            blocking: true,
            loadDataExtra: false,
            loading: false,
            errors: {},
            visibleForm: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.setData = this.setData.bind(this);

        this.handleSubmit = this.handleSubmit.bind(this);
        this.onDescargar = this.onDescargar.bind(this);
    }

    

    componentDidMount() {
        console.log('SolicitudViandaForm.componentDidMount');
    }

    componentDidUpdate(prevProps) {
        console.log('SolicitudViandaForm.componentDidUpdate');
        
        // Typical usage (don't forget to compare props):
        if (this.props.show && (this.props.entityId !== prevProps.entityId ||
            this.props.show !== prevProps.show)) {
            this.loadData();
        }
    }

    initData() {
        let dataInit = {
            Id: 0,
            ProveedorId: 0,
            resuelta: false,
            descripcion: "",
            fecha_registro: "",
            fecha_solucion: "",
        };
        return dataInit;
    }


    loadData() {
        console.log('this.props.entityId : ' + this.props.entityId);

        this.setState({ blocking: true, visibleForm:false });

        if (this.props.entityId > 0) {

            let url = '';
            url = `${this.props.urlApiBase}/GetProveedorNovedadApi/${this.props.entityId}`;


            http.get(url, {})
                .then((response) => {

                    let data = response.data;

                    if (data.success === true) {

                        console.log(response.data);

                        //Fix 
                        let dataEntity = data.result;
                        this.normalizingData(dataEntity);

                        this.setState({
                            data: dataEntity
                        });
                        if (data.result.fecha_solucion !== null && data.result.fecha_solucion !== "" && data.result.fecha_solucion) {
                            this.setState({
                                visibleForm: true
                            });
                        } 

                    } else {
                        //TODO: 
                        //Presentar errores... 
                        var message = $.fn.responseAjaxErrorToString(data);
                        abp.notify.error(message, 'Error');
                    }


                    this.setState({ blocking: false, errors: {} });

                })
                .catch((error) => {
                    console.log(error);
                });
        } else {

            this.setState({
                data: this.initData(),
                errors: {},
                blocking: false
            });

        }
    }

    normalizingData(data) {
        data['fecha_registro'] = moment(data['fecha_registro']).format(config.formatDate);

        if (data['fecha_solucion'] !== undefined && data['fecha_solucion'] !== null && moment(data['fecha_solucion']).isValid()) {
            data['fecha_solucion'] = moment(data['fecha_solucion']).format(config.formatDate);
        } else {
            data['fecha_solucion'] = '';
        }
    }

   
    isValid() {
        const errors = {};

        if (this.state.data.fecha_registro === undefined ||
            !moment(this.state.data.fecha_registro).isValid()) {
            errors.fecha_registro = 'Debe ingresar una Fecha';
        }

        if (moment(this.state.data.fecha_registro).diff(moment().format("YYYY-MM-DD")) > 0){
            errors.fecha_registro = 'La fecha de registro no puede ser mayor al dia actual';
        }

        if (!this.state.data.descripcion.length || this.state.data.descripcion.length <= 1) {
            errors.descripcion = 'Descripción es requerido';
        }

        if (this.state.data.resuelta) {
            if (this.state.data.fecha_solucion === undefined ||
                !moment(this.state.data.fecha_solucion).isValid()) {
                errors.fecha_solucion = 'Debe ingresar una Fecha Resolución';
            }

            if (this.state.data.fecha_solucion < this.state.data.fecha_registro) {
                errors.fecha_solucion = 'Debe ingresar una Fecha Resolución igual o mayor a la Fecha de Registro';
            }
        }


        if (this.state.uploadFile !== null && this.state.uploadFile !== undefined && this.state.uploadFile !== "") {
            if (this.state.uploadFile.size > `${config.maxFileSize}`) {
                errors.uploadFile = 'Debe ingresar un documento menor a 2MB';
            }

            if (!(/\.(gif|jpg|jpeg|png|pdf)$/i).test(this.state.uploadFile.name)) {
                errors.uploadFile = 'Debe ingresar documentos de tipo imagen';
            }
        }

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    handleSubmit(event) {
        event.preventDefault();

        if (!this.isValid()) {
            abp.notify.error("No ha ingresado los campos obligatorios  o existen datos inválidos.", 'Validación');
            return;
        }

        console.log(this.state);

        this.setState({ blocking: true });

        let url = '';
        if (this.props.entityAction === 'edit')
            url = `${this.props.urlApiBase}/EditProveedorNovedadApi`;
        else
            url = `${this.props.urlApiBase}/CreateProveedorNovedadApi`;


        //creating copy of object
        let data = Object.assign({}, this.state.data);
        data.Id = this.props.entityId;                        //updating value
        data.ProveedorId = this.props.padreId;

        this.normalizingDataSubmit(data);


        const formData = new FormData();
        for (var key in data) {
            if (data[key] !== null)
                formData.append(key, data[key]);
            else
                formData.append(key, '');
        }

        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };

        http.post(url, formData,config)
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    this.setState({
                        data: this.initData()
                    });


                    if (this.props.entityId <= 0) {
                        this.props.onAdded(response.data.result.id);
                    }
                    else {
                        this.props.onUpdated(response.data.result.id);
                    }

                } else {
                    //TODO: 
                    //Presentar errores... 
                    var message = $.fn.responseAjaxErrorToString(data);
                    abp.notify.error(message, 'Error');
                }


                this.setState({ blocking: false });

            })
            .catch((error) => {
                console.log(error);

                this.setState({ blocking: false });
            });

    }

    normalizingDataSubmit(dataEntity) {

        //Fix. Error boolean convert Enum Estado Contrato
        if (dataEntity.resuelta)
            dataEntity.resuelta = 1;
        else
            dataEntity.resuelta = 0;

        //Add File
        dataEntity.uploadFile = this.state.uploadFile;
    }

    handleChange(event) {
        const target = event.target;

        if (event.target.files) {

            let files = event.target.files || event.dataTransfer.files;
            if (files.length > 0) {
                let uploadFile = files[0];
                console.log(uploadFile);
                this.setState({
                    uploadFile: uploadFile,
                });                
            }
            
        } else {
            const value = target.type === "checkbox" ? target.checked : target.value;

            const name = target.name;

            const updatedData = {
                ...this.state.data
            };

            if (target.type === "checkbox") {
                this.setState({
                    visibleForm: target.checked
                });

                if (!target.checked) {
                    updatedData["fecha_solucion"] = null;
                }
            }

            updatedData[name] = value;


            this.setState({
                data: updatedData
            });
        }

        
    }


    setData(name, value) {

        const updatedData = {
            ...this.state.data
        };

        updatedData[name] = value;

        this.setState({
            data: updatedData
        });
    }

    onDescargar() {
        console.log(this.state.data.documentacion_id);
        return (
            window.location = `${config.appUrl}/proyecto/Archivo/Descargar/${this.state.data.documentacion_id}`
        );
    }


    render() {

        let disabled = false;
        if (this.props.entityAction === 'show') {
            disabled = true;
        }

        let blocking = this.state.blocking || this.state.loading || this.props.blocking;

        return (
            (!this.props.show) ? (<div>...</div>) : (
            <BlockUi tag="div" blocking={blocking}>
            
                <form onSubmit={this.handleSubmit}>
                    <div className="row">
                        <div className="col">
                            <Field
                                name="fecha_registro"
                                label="Fecha"
                                type="date"
                                required
                                value={this.state.data.fecha_registro}
                                edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                readOnly={(this.props.entityAction === 'show')}
                                error={this.state.errors.fecha_registro}
                                onChange={this.handleChange}
                            />

                        </div>
                        <div className="col">
                            <Field 
                                name="resuelta"
                                label="Resuelto"
                                labelOption="(Si/No)"
                                type="checkbox"
                                value={this.state.data.resuelta}
                                edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                readOnly={(this.props.entityAction === 'show')}
                                error={this.state.errors.resuelta}
                                onChange={this.handleChange}
                                />
                        </div>
                    </div>

                  
                    <div className="row">
                        <div className="col">
                            <Field
                                name="descripcion"
                                label="Descripción"
                                type="textarea"
                                required
                                value={this.state.data.descripcion}
                                edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                readOnly={(this.props.entityAction === 'show')}
                                error={this.state.errors.descripcion}
                                onChange={this.handleChange}
                            />
                        </div>
                    </div>

                    
                    <div className="row">
                        {
                        this.state.visibleForm ?
                            <div className="col">
                                <Field
                                    name="fecha_solucion"
                                    label="Fecha Resolución"
                                    type="date"
                                    value={this.state.data.fecha_solucion}
                                    edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                    readOnly={(this.props.entityAction === 'show')}
                                    error={this.state.errors.fecha_solucion}
                                    onChange={this.handleChange}
                                />
                            </div>
                        :null
                        }
                        {
                            (this.props.entityAction !== 'show')?
                            <div className="col">
                                <Field
                                    name="uploadFile"
                                    label="Cargar Documento REF"
                                    type={"file"}
                                    edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                    readOnly={(this.props.entityAction === 'show')}
                                    onChange={this.handleChange}
                                    error={this.state.errors.uploadFile}
                                    accept="image/*"
                                />
                            </div>
                            :
                            <div className="col">
                                <label htmlFor="descargarDoc"><b>Descargar Documento REF</b></label>
                                <br />
                                {
                                (this.state.data.documentacion_id === null || this.state.data.documentacion_id === undefined) ?
                                <p>No existe documento de referencia asociado</p>
                                :
                                <button type="button" name="descargarDoc" onClick={this.onDescargar}>Descargar</button>
                                }
                            </div>
                        }
                        
                    </div>
                    

                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                        <ActionForm onCancel={this.props.onHide} onSave={this.handleSubmit} />
                    }

                    {this.props.entityAction === 'show' &&
                        <ActionForm onAccept={this.props.onHide} />
                    }

                </form>
            </BlockUi>
            )
            );
    }
}

export default wrapForm(ProveedorNovedadForm);
 