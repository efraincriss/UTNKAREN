import React, { Component } from 'react';
import BlockUi from 'react-block-ui';
import moment from 'moment';

import http from '../Base/HttpService';
import config from '../Base/Config';
import ActionForm from '../Base/ActionForm';
import Field from '../Base/Field';

class SolicitudViandaCancelForm extends Component {

    constructor(props) {
        super(props);

      
        this.state = {
            data: this.initData(),
            blocking: true,
            loadDataExtra: false,
            loading:false,
            errors: {} 
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }


    componentDidMount() {
        console.log('SolicitudViandaCancelForm.componentDidMount');     
    }

    componentDidUpdate(prevProps) {
        console.log('SolicitudViandaCancelForm.componentDidUpdate');
   
        // Typical usage (don't forget to compare props):
        if (this.props.show  && (this.props.entityId !== prevProps.entityId || 
            this.props.show !== prevProps.show)) {
            this.loadData();
        }
    }

    initData() {
        let dataInit = {
            solicitante_id: 0,
            solicitante_nombre: '',
            LocacionId: 0,
            tipo_comida_id: 0,
            disciplina_id: 0,
            area_id:0,
            fecha_solicitud: moment().format(config.formatDate),
            fecha_alcancce: '',
            pedido_viandas: 0,
            alcance_viandas: 0,
            total_pedido: 0,
            consumido: 0,
            consumo_justificado: 0,
            total_consumido: 0,
            por_justificar: 0,
            estado: 1,
            referencia_ubicacion: '',
            observaciones:''
        };
        return dataInit; 
    }

    loadData() {
        console.log('this.props.entityId : ' + this.props.entityId);

        if (this.props.entityId > 0) {

            let url = '';
            url = `${config.apiUrl}/proveedor/SolicitudVianda/GetApi/${this.props.entityId}`;
             

            http.get(url, {})
                .then((response) => {

                    let data = response.data;

                    if (data.success === true) {
 
                        console.log(response.data);

                        let dataEntity = data.result;
                        this.normalizingData(dataEntity);

                        this.setState({
                            data: dataEntity
                        });

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


    normalizingData(dataEntity) {
        //Fix Date
        dataEntity['fecha_solicitud'] = moment(dataEntity['fecha_solicitud']).format(config.formatDate);
        if (dataEntity['fecha_alcancce'] !== undefined && dataEntity['fecha_alcancce'] !== null && moment(dataEntity['fecha_alcancce']).isValid()) {
            dataEntity['fecha_alcancce'] = moment(dataEntity['fecha_alcancce']).format(config.formatDate);
        } else {
            dataEntity['fecha_alcancce'] = '';
        }

        if (!dataEntity.observaciones) {
            dataEntity.observaciones = "";
        }
    }

    isValid() {
        const errors = {};

        if (!this.state.data.observaciones  || this.state.data.observaciones.length <= 1) {
            errors.observaciones = 'Debe ingresar una observación';
        }

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    handleSubmit(event) {
        event.preventDefault();

        if (!this.isValid()) {
            return;
        }

        console.log(this.state);

        this.setState({ blocking: true });

        let url = '';
        url = `${config.apiUrl}/proveedor/SolicitudVianda/EditCancelApi`;
 
         //creating copy of object
        let data = {
            id: this.props.entityId,                       //updating value
            observaciones: this.state.data.observaciones
        }; 

       
        http.post(url, data)
            .then((response) => {

                let data = response.data;

                if (data.success === true) {
 
                    this.setState({
                        data: this.initData()
                    });

                    this.props.onUpdated(response.data.result);
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

    handleChange(event) {
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
      
    render() {
 
 
        return (

            <BlockUi tag="div" blocking={this.state.blocking}>

                <form >
                    <div className="row">
                        <div className="col">

                            <Field
                                name="fecha_solicitud"
                                label="Fecha"
                                type="date"
                                readOnly
                                value={this.state.data.fecha_solicitud}
                            />
                        </div>
                         

                        <div className="col">
                            <Field
                                name="tipo_comida_nombre"
                                label="Tipo Comida"
                                readOnly
                                value={this.state.data.tipo_comida_nombre}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <Field
                                name="solicitante_nombre"
                                label="Solicitante"
                                readOnly
                                value={this.state.data.solicitante_nombre}
                            />
                  
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">

                            <Field
                                name="locacion_nombre"
                                label="Locación"
                                readOnly
                                value={this.state.data.locacion_nombre}
                            />

                        </div>
                        <div className="col">
                            <Field
                                name="area_nombre"
                                label="Área"
                                readOnly
                                value={this.state.data.area_nombre}
                            />
                        </div>
                        <div className="col">

                            <Field
                                name="disciplina_nombre"
                                label="Disciplina"
                                edit={false}
                                readOnly
                                value={this.state.data.disciplina_nombre}
                            />
                             
                        </div>
                    </div>
 
                    <div className="row">
                        <div className="col">

                            <Field
                                name="pedido_viandas"
                                label="Viandas Original"
                                type="number"
                                readOnly
                                value={this.state.data.pedido_viandas}
                            />
                        </div>
                        <div className="col">

                            <Field
                                name="alcance_viandas"
                                label="Viandas Adicionales"
                                type="number"
                                readOnly
                                value={this.state.data.alcance_viandas}
                            />
                           
                        </div>
                        <div className="col">
                            <Field
                                name="total_pedido"
                                label="Viandas Totales"
                                type="number"
                                readOnly
                                value={this.state.data.total_pedido}
                            />
                             
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">

                            <Field
                                name="observaciones"
                                required
                                value={this.state.data.observaciones}
                                label="Observaciones"
                                type={"textArea"}
                                onChange={this.handleChange}
                                edit={true}
                                error={this.state.errors.observaciones}
                            />
                        </div>
                    </div>

                    <ActionForm onCancel={this.props.onHide} onSave={this.handleSubmit} />
       
                </form>
            </BlockUi>
        );
    }
}

export default SolicitudViandaCancelForm;