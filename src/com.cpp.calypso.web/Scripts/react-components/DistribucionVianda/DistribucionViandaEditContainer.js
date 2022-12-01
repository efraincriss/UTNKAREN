import React from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';

import { Dialog } from 'primereact/components/dialog/Dialog';
import axios from "axios";
import config from '../Base/Config';
import http from '../Base/HttpService';

import wrapContainer from '../Base/WrapContainer';
import moment from 'moment';

import DistribucionViandaSolicitudesList from './DistribucionViandaSolicitudesList';
import DistribucionViandaEditList from './DistribucionViandaEditList';
import DistribucionViandaProveedorList from './DistribucionViandaProveedorList';



class DistribucionViandaEditContainer extends React.Component {

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
            dataExtra: {
                solicitudes: [],
                proveedores: [],
                proveedoresTree: [],
                tipoComida: {
                    Id: 0,
                    nombre: ''
                },
                estadoDistribucion: 1 //Registrado
            },
            isDirty: false,
            loadData: true,
            selectIds: [],
            selectedTreeIds: null,
            deleteIds: []

        };



        this.onNewItem = this.onNewItem.bind(this);
        this.onEditItem = this.onEditItem.bind(this);
        this.onDeleteItem = this.onDeleteItem.bind(this);
        this.onDetailItem = this.onDetailItem.bind(this);

        this.handleSelect = this.handleSelect.bind(this);

        this.handleAdded = this.handleAdded.bind(this);
        this.handleUpdated = this.handleUpdated.bind(this);

        this.onHide = this.onHide.bind(this);


        this.onSelectionTreeChange = this.onSelectionTreeChange.bind(this);
        this.onAsignar = this.onAsignar.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.onReturn = this.onReturn.bind(this);
        this.onRefreshData = this.onRefreshData.bind(this);
        this.onAprobar = this.onAprobar.bind(this);
        this.onVer = this.onVer.bind(this);

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

    onNewItem(e) {
        console.log(e);
        this.setState({ entityId: 0 });
        this.setState({ visibleForm: true });

    }

    onEditItem(id) {

        console.log(id);
        this.setState({ entityId: id, visibleForm: true });

    }

    onDeleteItem(entity) {
        console.log('onDeleteItem Item: ' + entity);


        //2. Regresar items a solicitudes Pendientes.
        this.setState({ blocking: true });

        let stateLocal = { ...this.state };


        var item = {
            Id: entity.solicitud_id,
            solicitante_nombre: entity.solicitante_nombre,
            estado_solicitud_nombre: entity.estado_solicitud_nombre,

            disciplina_nombre: entity.disciplina_nombre,
            locacion_nombre: entity.locacion_nombre,
            area_nombre: entity.area_nombre,

            pedido_viandas: entity.pedido_viandas,
            alcance_viandas: entity.alcance_viandas,
            total_pedido: entity.total_pedido
        };


        //Quitar item
        let dataNew = stateLocal.data.filter(dis => dis.detalle_distribuccion_id !== entity.detalle_distribuccion_id);

        //Regresar a solicitudes item
        let solicitudesNew = [...this.state.dataExtra.solicitudes];
        solicitudesNew.push(item);

        let dataExtraLocal = { ...this.state.dataExtra };
        dataExtraLocal.solicitudes = solicitudesNew;

        let deleteIdsLocal = [...this.state.deleteIds];
        deleteIdsLocal.push(entity.detalle_distribuccion_id);

        this.setState({ isDirty: true, blocking: false, deleteIds: deleteIdsLocal, data: dataNew, selectedTreeIds: null, selectIds: [], dataExtra: dataExtraLocal });

    }

    onDetailItem(id) {
        console.log('DetailItem Item: ' + id);
    }

    onAsignar() {


        console.log('onAsignar');

        //1. Verificar selecciones... 
        if (this.state.selectIds && this.state.selectIds.length === 0) {
            abp.notify.warn("Debe seleccionar por lo menos una solicitud, para asignar", "Aviso");
            return;
        }

        if (this.state.selectedTreeIds && this.state.selectedTreeIds.length === 0) {
            abp.notify.warn("Debe seleccionar un proveedor, para asignar", "Aviso");
            return;
        }

        if (this.state.selectedTreeIds <= 0) {
            abp.notify.warn("Debe seleccionar un proveedor, para asignar", "Aviso");
            return;
        }

        //2. Asignar.. 
        this.setState({ blocking: true });

        let stateLocal = { ...this.state };

        var solicitudesId = [...this.state.selectIds];
        var newList = [];
        for (var i = 0, len = solicitudesId.length; i < len; i++) {
            var solicitudId = solicitudesId[i];

            var solicitud = stateLocal.dataExtra.solicitudes.filter(item => item.Id === solicitudId)[0];
            var proveedor = stateLocal.dataExtra.proveedores.filter(item => item.Id === this.state.selectedTreeIds)[0];


            var item = {
                Id: Math.floor((Math.random() * 300) + 1) * -1,
                detalle_distribuccion_id: Math.floor((Math.random() * 300) + 1) * -1,

                solicitud_id: solicitud.Id,
                solicitante_nombre: solicitud.solicitante_nombre,
                LocacionId: solicitud.LocacionId,
                locacion_nombre: solicitud.locacion_nombre,

                AreaId: solicitud.area_id,
                area_nombre: solicitud.area_nombre,

                disciplina_id: solicitud.disciplina_id,
                disciplina_nombre: solicitud.disciplina_nombre,

                ProveedorId: proveedor.ProveedorId,
                proveedor_nombre: proveedor.razon_social,
                tipo_comida_id: stateLocal.params.tipoComidaId,
                tipo_comida_nombre: stateLocal.dataExtra.tipoComida.nombre,
                fecha: stateLocal.params.fecha,

                pedido_viandas: solicitud.pedido_viandas,
                alcance_viandas: solicitud.alcance_viandas,
                total_pedido: solicitud.total_pedido,

                estado_solicitud: solicitud.estado,
                estado_solicitud_nombre: solicitud.estado_nombre,

                estado: stateLocal.dataExtra.estadoDistribucion,
                estado_nombre: 'Registrado'
            };


            newList.push(item);
        }



        console.log(newList);

        //3. Actualizar datos
        //3.1 Si existe en  Eliminados, y se estan agregando quitar...
        //let deleteNew = [];
        //deleteNew = stateLocal.deleteIds.filter(sol => !solicitudesId.includes(sol))

        //3.2 Add Solicitudes Asignadas
        let newData = [];
        newData = [...this.state.data];

        newList.forEach(function (element) {
            newData.push(element);
        });

        //3.3 Quitar Solicitudes asignadas desde solicitudes pendientes
        let dataExtraLocal = { ...this.state.dataExtra };
        dataExtraLocal.solicitudes = stateLocal.dataExtra.solicitudes.filter(sol => solicitudesId.filter(item => item === sol.Id).length === 0);

        this.setState({ isDirty: true, blocking: false, data: newData, selectedTreeIds: null, selectIds: [], dataExtra: dataExtraLocal });
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

    enviarcorreo = () => {
        this.setState({ blocking: true });
        axios
            .post("/Proveedor/DistribucionVianda/EnviarCorreos", {
                fecha: moment(this.getParameterByName('fecha')).format(config.formatDate)
            })
            .then(response => {
                if (response.data != "Error") {
                    abp.notify.success("Notificaciones Enviadas", "Aviso");
                    this.setState({ blocking: false });
                } else {
                    abp.notify.warn("Error al Enviar Notificaciones", "Aviso");
                    this.setState({ blocking: false });
                }
            })
            .catch(error => {
                console.log(error);
                this.setState({ blocking: false });
            });
    }

    onVer() {
        event.preventDefault();
        return (
            window.location.href = `${config.appUrl}/proveedor/DistribucionVianda/Ver?fecha=${this.state.params.fecha}&tipoComidaId=${this.state.params.tipoComidaId}`
        );
    }

    isValid() {
        const errors = {};

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    isValidAprobar() {
        const errors = {};


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
        url = `/proveedor/distribucionvianda/EditDistribucionApi`;


        //creating copy of object
        let data = {
            model: [...this.state.data],
            deleteIds: [...this.state.deleteIds],
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

    onAprobar(event) {
        event.preventDefault();

        if (!this.isValidAprobar()) {
            return;
        }

        console.log(this.state);

        this.setState({ blocking: true });

        let url = '';
        url = `/proveedor/distribucionvianda/EditAprobarApi`;


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

    handleAdded(entity) {

        console.log('add Item: ');
        console.log(entity);
        this.setState({ visibleForm: false, entityId: 0 });

        abp.notify.success("Proceso guardado exitosamente", "Aviso");

        this.props.onRefreshData();

    }

    handleUpdated(entity) {
        console.log('update Item: ');
        console.log(entity);
        this.setState({ visibleForm: false, entityId: 0 });


        abp.notify.success("Proceso guardado exitosamente", "Aviso");

        this.props.onRefreshData();
    }

    onHide() {
        console.log('onHide ');
        this.setState({ visibleForm: false, entityId: 0 });
    }



    handleSelect(isSelected, selectIds) {

        var newSelectIds = [];
        this.setState({ selectIds: selectIds });

    }

    onSelectionTreeChange(e) {
        console.log("onSelectionTreeChange");
        console.log(e);

        if (e.selection.nivel > 1) {
            this.setState({ selectedTreeIds: e.selection.key });
        }

    }

    GetData() {

        this.setState({ blocking: true });

        let url = '';
        url = `/proveedor/distribucionvianda/EditDistribucionApi/?fecha=${this.state.params.fecha}&tipoComidaId=${this.state.params.tipoComidaId}`;


        http.get(url, {})
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    //Fix Date
                    let dataFix = data.result.distribuciones;
                    dataFix.forEach(function (dataEntity) {
                        dataEntity['fecha'] = moment(dataEntity['fecha']).format(config.formatDate);
                    });

                    var dataExtra = {
                        solicitudes: data.result.solicitudes,
                        proveedores: data.result.proveedores,
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

    render() {

        let blocking = this.props.blocking || this.state.blocking;
        let estado_distribuccion = "Registrado";
        if (this.state.data.length > 0) {
            estado_distribuccion = this.state.data[0].estado_nombre;
        }

        return (
            <BlockUi tag="div" blocking={blocking}>


                <div className="nav justify-content-end">
                    <button className="btn btn-outline-primary" onClick={this.enviarcorreo} > Enviar Notificaciones </button>{" "}
                    <button className="btn btn-outline-primary" onClick={this.onVer} disabled={this.state.isDirty}> Ver </button>
                    <button className="btn btn-outline-primary" onClick={this.onReturn} > Regresar </button>
                    <button className="btn btn-outline-primary" onClick={this.onAsignar} > Asignar </button>
                    <button className="btn btn-outline-primary" onClick={this.handleSubmit} disabled={!this.state.isDirty} > Guardar </button>
                    <button className="btn btn-outline-primary" onClick={this.onAprobar} disabled={this.state.isDirty} > Aprobar </button>
                </div>

                <hr />

                <div className="row">
                    <div className="col">
                        <div className="form-group row">
                            <label htmlFor="fecha" className="col-sm-4 col-form-label"><strong>Fecha</strong></label>
                            <div className="col-sm-8">
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
                        <div className="form-group row">
                            <label htmlFor="tipo_comida_nombre" className="col-sm-4 col-form-label"><strong>Estado</strong></label>
                            <div className="col-sm-8">
                                <input name="tipo_comida_nombre" type="text" value={estado_distribuccion} readOnly className="form-control-plaintext" />
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
                    <div className="col-sm-8">
                        <DistribucionViandaSolicitudesList
                            data={this.state.dataExtra.solicitudes}
                            onSelectAction={this.handleSelect}
                            selectIds={this.state.selectIds}
                        />
                    </div>
                    <div className="col">
                        <DistribucionViandaProveedorList
                            data={this.state.dataExtra.proveedores}
                            dataAsignaciones={this.state.data}
                            onSelectionChange={this.onSelectionTreeChange}
                            selectedNodeKey={this.state.selectedTreeIds}

                        />
                    </div>
                </div>

                <hr />

                <div className="row">
                    <div className="col">
                        <DistribucionViandaEditList
                            data={this.state.data}
                            isDirty={this.state.isDirty}
                            onDeleteAction={this.onDeleteItem}
                        />
                    </div>
                </div>
            </BlockUi>
        );
    }
}

// HOC

ReactDOM.render(
    <DistribucionViandaEditContainer />,
    document.getElementById('nuc_edit_body_distribucion_vianda')
);


