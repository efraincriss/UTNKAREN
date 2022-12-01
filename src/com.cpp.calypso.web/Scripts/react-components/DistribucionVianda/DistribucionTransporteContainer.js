import React from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';
import axios from 'axios';


import { Dialog } from 'primereact/components/dialog/Dialog';

import config from '../Base/Config';
import http from '../Base/HttpService';

import wrapContainer from '../Base/WrapContainer';
import moment from 'moment';

import DistribucionTransporteSolicitudesList from './DistribucionTransporteSolicitudesList';
import DistribucionTransporteList from './DistribucionTransporteList';
 


class DistribucionTransporteContainer extends React.Component {

    constructor() {
        super();

        this.state = {
            visibleForm: false,
            entityId: 0,
            params: {
                fecha: moment().format(config.formatDate),
                tipoComidaId: 0
            },
            data: [],
            urlApiBase: '/proveedor/distribucionvianda/',
            dataExtra: {
                distribucionSolicitudes: [],
                transportistas: [],
                tipoComida: {
                    Id: 0,
                    nombre: ''
                }
            },
            distribucion_id: 0,
            isDirty: false,
            loadData: true,
            selectIds: [],
            selectedTreeIds: [],
            deleteIds: []

        };



         
        this.onDeleteItem = this.onDeleteItem.bind(this);
        this.onDetailItem = this.onDetailItem.bind(this);

        
        this.setDataTransportista = this.setDataTransportista.bind(this);
      
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleAprobar = this.handleAprobar.bind(this);
        this.onReturn = this.onReturn.bind(this);
        this.onRefreshData = this.onRefreshData.bind(this);
        

    }

    componentWillMount() {
        let params = {
            fecha: moment(this.getParameterByName('fecha')).format(config.formatDate),
            tipoComidaId: this.getParameterByName('tipoComidaId')
        };

        console.log(params);
        this.setState({ params: params });
    }


    componentDidMount(prevProps) {

        if (this.state.loadData) {
            this.GetData();
        }

    }


    getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

   
    onDeleteItem(entity) {
        console.log('onDeleteItem Item: ' + entity);
 
        let dataUpdate = [... this.state.data];
        let index = dataUpdate.findIndex(item => item.Id === entity.Id);
        dataUpdate[index]["conductor_asignado_id"] = null;
        dataUpdate[index]["conductor_asignado_nombre"] = "";
        this.setState({ data: dataUpdate, isDirty: true });   
 
    }

    onDetailItem(entity) {
        this.setState({ distribucion_id: entity.Id });
    }
 
    onReturn() {

        event.preventDefault();
        if (this.state.isDirty) {
            var self = this;

            abp.message.confirm(
                "Existen cambios sin guardar. ¿Desea continuar?",
                "Confirmación",
                function (isConfirmed) {
                    if (isConfirmed) {
                        return (
                            window.location.href = `${config.appUrl}/proveedor/DistribucionVianda`
                        );
                    }
                }
            );
        } else {

            return (
                window.location.href = `${config.appUrl}/proveedor/DistribucionVianda`
            );
        }
    }

    isValid() {
        const errors = {};
        //TODO: Pendiente... 

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
        url = `${this.state.urlApiBase}/EditTransporteApi`;


        let dataLocal = [...this.state.data];
        dataLocal = dataLocal.map(item => ({ conductor_asignado_id: item.conductor_asignado_id, Id:item.Id }));
       
        //creating copy of object
        let data = {
            model: dataLocal,
            fecha: this.state.params.fecha,
            tipoComidaId: this.state.params.tipoComidaId
        };


        http.post(url, data)
            .then((response) => {

                let data = response.data;

                if (data.success === true) {


                    //TODO: Campusoft: Mejorar
                    abp.notify.success("Proceso guardado exitosamente", "Aviso");

                    //TODO: Mantener en la funcionalidad... o regresar al listado... 
                    //Refresh
                    this.onRefreshData();

                    this.setState({ isDirty: false });

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
 

    handleAprobar(event) {
        event.preventDefault();

        if (this.state.isDirty) {
            let message = "La acción Aprobar e Imprimir, no se puede ejecutar. Existe cambios pendientes de guardar";
            abp.notify.error(message, 'Error');
            return;
        }

        //Valid
        let dataLocal = [...this.state.data];
        let pendientes = dataLocal.filter(dis => dis.conductor_asignado_id === null || dis.conductor_asignado_id === undefined || dis.conductor_asignado_id === 0 );


        if (pendientes.length > 0) {
            let message = "La acción Aprobar e Imprimir, no se puede ejecutar. Existe asignaciones de Transportistas pendientes";
            abp.notify.error(message, 'Error');
            return;
        }

        console.log(this.state);

        this.setState({ blocking: true });

        let url = '';
        url = `${this.state.urlApiBase}/EditApproveTransporteApi`;

 
        //creating copy of object
        let data = {
             fecha: this.state.params.fecha,
            tipoComidaId: this.state.params.tipoComidaId
        };


        http.post(url, data)
            .then((response) => {

                let data = response.data;

                if (data.success === true) {


                    //TODO: Campusoft: Mejorar
                    abp.notify.success("Proceso guardado exitosamente", "Aviso");

                    //TODO: Mantener en la funcionalidad... o regresar al listado... 
                    //Refresh
                    this.onRefreshData();

                    this.setState({ isDirty: false });

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


    GetData() {

        this.setState({ blocking: true });

        let url = '';
        url = `${this.state.urlApiBase}/GetTransporteApi/?fecha=${this.state.params.fecha}&tipoComidaId=${this.state.params.tipoComidaId}`;


        http.get(url, {})
            .then((response) => {
                console.log("data transportista")
                console.log(response.data)
                let data = response.data;

                if (data.success === true) {

                     
                    let dataFix = data.result.distribucionTrasporte;
                   
                    var dataExtra = {
                        distribucionSolicitudes: data.result.distribucionSolicitudes,
                        transportistas: data.result.transportistas,
                        tipoComida: data.result.tipoComida
                    };

                    this.setState({
                        data: dataFix,
                        dataExtra: dataExtra
                    });


                } else {
                    //TODO: 
                    //Presentar errores... 
                    var message = $.fn.responseAjaxErrorToString(data);
                    abp.notify.error(message, 'Error');
                }


                this.setState({ blocking: false, loadData: false });
            })
            .catch((error) => {
                console.log(error);
            });
    }

    onRefreshData() {
        this.GetData();
    }

    setDataTransportista(row,itemSelected) {
        console.log(row);
        
        let dataUpdate = [... this.state.data];
        let index = dataUpdate.findIndex(item => item.Id === row.Id);
        dataUpdate[index]["conductor_asignado_id"] = itemSelected["Id"];
        this.setState({ data: dataUpdate, isDirty: true });        
    }

 
    render() {

        let blocking =   this.state.blocking;
        return (
            <BlockUi tag="div" blocking={blocking}>


                <div className="nav justify-content-end">
                    <button className="btn btn-outline-primary" onClick={this.onReturn} > Regresar </button>
                    <button className="btn btn-outline-primary" onClick={this.handleSubmit} disabled={!this.state.isDirty} > Guardar </button>
                    <button className="btn btn-outline-primary" onClick={this.handleAprobar} disabled={this.state.isDirty} > Aprobar </button>
                </div>

                <hr />


                <div className="row">
                    <div className="col">
                        <div className="form-group row">
                            <label htmlFor="fecha" className="col-sm-3 col-form-label"><strong>Fecha</strong></label>
                            <div className="col-sm-9">
                                <input name="fecha_solicitud" type="date" value={this.state.params.fecha} readOnly className="form-control-plaintext" />
                            </div>
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group row">
                            <label htmlFor="tipo_comida_nombre" className="col-sm-4 col-form-label"><strong>Tipo Comida</strong></label>
                            <div className="col-sm-8">
                                <input name="tipo_comida_nombre" type="text" value={this.state.dataExtra.tipoComida.nombre} readOnly className="form-control-plaintext" />
                            </div>
                        </div>
                    </div>
                    <div className="col">
                        {this.state.isDirty &&
                            <div className="form-group row">
                                <div className="col-sm-12">
                                    <span className="m-2 p-1 badge badge-warning"><i className="fa fa-dot-circle-o"></i> Cambios Pendientes de Guardar</span>
                                </div>
                            </div>
                        }
                    </div>

                </div>


                <hr />

                <div className="row">
                    <div className="col">
                        <DistribucionTransporteList
                            data={this.state.data}
                            isDirty={this.state.isDirty}
                            dataExtra={this.state.dataExtra.transportistas}
                            onUpdateData={this.setDataTransportista}
                            onDeleteAction={this.onDeleteItem}
                            onDetailAction={this.onDetailItem}
                            showActionName={"Ver Solicitudes"}
                            actionDeleteMessage={"Estás seguro de eliminar el transportista. ¿Desea continuar?"}
                        />
                    </div>
                </div>

                <hr />

                <div className="row">
                    <div className="col">
                        <DistribucionTransporteSolicitudesList
                            data={this.state.dataExtra.distribucionSolicitudes.filter(sol => sol.Id === this.state.distribucion_id)}
                        />
                    </div>
                </div>

                

               
            </BlockUi>
        );
    }
}

// HOC

ReactDOM.render(
    <DistribucionTransporteContainer />,
    document.getElementById('nuc_edit_body_distribucion_transporte')
);


