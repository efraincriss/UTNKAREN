import React, { Component } from 'react';
import BlockUi from 'react-block-ui';
import axios from 'axios';
import moment from 'moment';

import http from '../Base/HttpService';
import { Dropdown } from 'primereact/components/dropdown/Dropdown';

import config from '../Base/Config';
import Field from '../Base/Field';
import ActionForm from '../Base/ActionForm';
import wrapForm from '../Base/WrapForm';

 
class JustificacionViandasSolicitudInfo extends Component {
     
    render() {
        
        return (
            <div>
                 
                <div className="row">
                    <div className="col">
                        <Field 
                            name="fecha_solicitud"
                            readOnly={true}
                            label="Fecha"
                            type="Date"
                            value={this.props.data.fecha_solicitud}
                        />

                        <Field
                            name="conductor_asignado_nombre"
                            readOnly={true}
                            label="Transportista"
                            value={this.props.data.conductor_asignado_nombre}
                        />
 
                       

                    </div>

                    <div className="col">

                        <Field
                            name="solicitante_nombre"
                            readOnly={true}
                            label="Solicitante"
                            value={this.props.data.solicitante_nombre}
                        />
                         

                        <Field
                            name="anotador_nombre"
                            readOnly={true}
                            label="Anotador" 
                            value={this.props.data.anotador_nombre}
                        />

                    </div>
                </div>

                <div className="row">
                    <div className="col">

                        <Field
                            name="total_pedido"
                            readOnly={true}
                            label="Total Solicitado"
                            type="Number"
                            value={this.props.data.total_pedido}
                            classLabel="col-sm-5 col-form-label"
                            classControl="col-sm-7"
                        />

                    </div>
                    <div className="col">
                        <Field
                            name="total_consumido"
                            readOnly={true}
                            label="Total Justificado"
                            type="Number"
                            value={this.props.data.total_consumido}
                            classLabel="col-sm-5 col-form-label"
                            classControl="col-sm-7"
                        />
                    </div>
                </div>
            </div>
        );
    }
}

export default JustificacionViandasSolicitudInfo;