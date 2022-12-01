import React, { Component } from 'react';
import BlockUi from 'react-block-ui';
import moment from 'moment';

import http from '../Base/HttpService';

import config from '../Base/Config';
import Field from '../Base/Field';
import ActionForm from '../Base/ActionForm';
import wrapForm from '../Base/WrapForm';

import JustificacionViandasSolicitudInfo from './JustificacionViandasSolicitudInfo'

class JustificacionViandasForm extends Component {

    constructor(props) {
        super(props);

        this.state = {
            blocking: true,           
            loading: false,
            errors: {}
        };
   
        this.handleSubmit = this.handleSubmit.bind(this);
    }


    componentDidMount() {
        console.log('JustificacionViandasForm.componentDidMount');
    }

    componentDidUpdate(prevProps) {
        console.log('JustificacionViandasForm.componentDidUpdate');
        // Typical usage (don't forget to compare props):
        if (this.props.show && (this.props.entityId !== prevProps.entityId ||
            this.props.show !== prevProps.show)) {
            this.setState({ errors: {} });
        }     
    }

      

    isValid() {
        const errors = {};

        if (this.props.data.numero_viandas === undefined || this.props.data.numero_viandas <= 0) {
            errors.numero_viandas = 'Debe ingresar Viandas a justificar, debe ser mayor a 0';
        }
 
        if (this.props.data.justificacion === undefined
            || this.props.data.justificacion === null
            || this.props.data.justificacion.length <= 1) {
            errors.justificacion = 'Justificación es requerido';
        }
    
        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    handleSubmit(event) {
        event.preventDefault();

        if (!this.isValid()) {
            return;
        }

        this.props.onSave();
    }

    render() {

        let disabled = false;
        if (this.props.entityAction === 'show') {
            disabled = true;
        }


        let blocking =  this.props.blocking;
     
        return (
             (!this.props.show) ? (<div>...</div>):
                (
                    <BlockUi tag="div" blocking={blocking}>

                        <form onSubmit={this.handleSubmit}>

                            <JustificacionViandasSolicitudInfo
                                data={this.props.data}
                            />
 

                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="numero_viandas"
                                        label="Viandas a justificar"
                                        type="Number"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        required={true}
                                        value={this.props.data.numero_viandas}
                                        onChange={this.props.handleChange}
                                        error={this.state.errors.numero_viandas}
                                        classLabel="col-sm-4 col-form-label"
                                        classControl="col-sm-8"
                                        min="0"

                               />
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="justificacion"
                                        label="Justificación"
                                        type="TextArea"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        required={true}
                                        value={this.props.data.justificacion}
                                        onChange={this.props.handleChange}
                                        error={this.state.errors.justificacion}
                                    />
                                </div>
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

export default wrapForm(JustificacionViandasForm);