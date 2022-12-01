import React from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';
import axios from 'axios';



import { Dialog } from 'primereact/components/dialog/Dialog';

import config from '../Base/Config';
import http from '../Base/HttpService';
import wrapContainer from '../Base/WrapContainer';
import moment from 'moment';

import ConciliacionList from './ConciliacionList';
import ConciliacionDetalleSolicitudInfo from './ConciliacionDetalleSolicitudInfo';


class ConciliacionContainer extends React.Component {

    constructor() {
        super();

        this.state = {
            data: {
                fecha: moment().format(config.formatDate)
            }, 
            dataDetalle: {},
            errors: {},
            visibleForm: false,
            entityId: 0,
            urlApiBase: '/proveedor/consumo/'
        };

        this.onDetailItem = this.onDetailItem.bind(this);
        this.onHide = this.onHide.bind(this);
        this.handleChangeDate = this.handleChangeDate.bind(this);
    }

      

    onDetailItem(entity) {
        console.log('DetailItem Item: ' + entity);

        this.setState({ blocking: true });

        let url = '';
        url = `${this.state.urlApiBase}/GetDetalleConsumoSolicitudApi/?solicitudId=${entity.solicitud_id}`;


        http.get(url, {})
            .then((response) => {

                let data = response.data;

                if (data.success === true) {
 
                    let dataEntity = data.result;
                    this.normalizingData(dataEntity);
                    this.setState({
                        dataDetalle: dataEntity
                    });


                    this.setState({ blocking: false, errors: {}, entityId: entity.solicitud_id, visibleForm: true });

                } else {
                    //TODO: 
                    //Presentar errores... 
                    var message = $.fn.responseAjaxErrorToString(data);
                    abp.notify.error(message, 'Error');

                    this.setState({ blocking: false, errors: { message}, entityId: 0, visibleForm: false });

                }
 
            })
            .catch((error) => {
                 abp.notify.error(error, 'Error');

                this.setState({ blocking: false, errors: { error }, visibleForm: false, dataDetalle: {} });
                console.log(error);
            });
    }
      

    normalizingData(dataEntity) {
        /*
        dataEntity['consumo_viandas'].map

        dataEntity['consumo_viandas'] = dataEntity['consumo_viandas'].map(
                item => ({ conductor_asignado_id: item.conductor_asignado_id, Id: item.Id }
            )
        );


        dataEntity['fecha_solicitud'] = moment(dataEntity['fecha_solicitud']).format(config.formatDate);
        if (dataEntity['fecha_alcancce'] !== undefined && dataEntity['fecha_alcancce'] !== null && moment(dataEntity['fecha_alcancce']).isValid()) {
            dataEntity['fecha_alcancce'] = moment(dataEntity['fecha_alcancce']).format(config.formatDate);
        } else {
            dataEntity['fecha_alcancce'] = '';
        }

        if (!dataEntity.observaciones) {
            dataEntity.observaciones = "";
        }*/
    }

    onHide() {
        console.log('onHide ');
        this.setState({ visibleForm: false, entityId: 0 });
    }

    handleChangeDate(event) {
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

        //TODO: Validar si existe fecha ingresada....
 
        var newParams = {
            fecha: `${moment(value).format(config.formatDate)}`
        };

        this.props.onRefreshData(newParams);
    }

    render() {

        let blocking = this.props.blocking || this.state.blocking;
        return (
            <BlockUi tag="div" blocking={blocking}>

                <div className="row">
                    <div className="col-sm-4">
                        <div className="form-group row">
                            <label htmlFor="fecha" className="col-sm-3 col-form-label">Fecha</label>

                            <div className="col-sm-9">
                                <input type="date" id="fecha" className="form-control" value={this.state.data.fecha} onChange={this.handleChangeDate} name="fecha" />
                                {this.state.errors.fecha && <div className="alert alert-danger">{this.state.errors.fecha}</div>}
                            </div>

                        </div>

                    </div>
                </div>
                 
                <hr />
            
                <div>
                    <ConciliacionList
                        data={this.props.data}
                        onDetailAction={this.onDetailItem}
                    />
                </div>

                <Dialog header="Detalle de Solicitud" visible={this.state.visibleForm} width="680px" modal minY={70} onHide={this.onHide} >
 
                    <ConciliacionDetalleSolicitudInfo
                        show={this.state.visibleForm}
                        onHide={this.onHide}
                        data={this.state.dataDetalle}
                    />

                </Dialog>


            </BlockUi>
        );
    }
}

 

// HOC
const Container = wrapContainer(ConciliacionContainer,
    `/proveedor/consumo/GetConciliacionDiariaApi/`,
    {
        fecha: `${moment().format(config.formatDate)}`
    });

ReactDOM.render(
    <Container />,
    document.getElementById('nuc_tree_body_conciliacion_diaria')
);

