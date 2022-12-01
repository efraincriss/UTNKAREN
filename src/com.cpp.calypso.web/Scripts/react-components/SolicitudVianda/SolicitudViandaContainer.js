import React from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';
import axios from 'axios';

import { Dialog } from 'primereact/components/dialog/Dialog';

import config from '../Base/Config';
import http from '../Base/HttpService';
import wrapContainer from '../Base/WrapContainer';
import moment from 'moment';

import SolicitudViandaList from './SolicitudViandaList';
import SolicitudViandaForm from './SolicitudViandaForm';
import SolicitudViandaCancelForm from './SolicitudViandaCancelForm';
import { ScrollPanel } from 'primereact-v2/scrollpanel';

class SolicitudViandaContainer extends React.Component {

    constructor() {
        super();

        this.state = {
            data: {
                fecha: moment().format(config.formatDate)
            },
            selectIds: [],
            visibleForm: false,
            visibleCancelForm: false,
            entityId: 0,
            entityAction: 'create',
            errors: {},
            urlApiBase: '/proveedor/SolicitudVianda/'
        };

        this.onNewItem = this.onNewItem.bind(this);
        this.onEditItem = this.onEditItem.bind(this);
        this.onDeleteItem = this.onDeleteItem.bind(this);
        this.onDetailItem = this.onDetailItem.bind(this);
        this.onCancelItem = this.onCancelItem.bind(this);

        this.handleAdded = this.handleAdded.bind(this);
        this.handleUpdated = this.handleUpdated.bind(this);

        this.handleSelect = this.handleSelect.bind(this);
        this.onHide = this.onHide.bind(this);
        this.handleChangeDate = this.handleChangeDate.bind(this);
    }


    onNewItem(e) {
        console.log(e);
        this.setState({ entityId: 0, visibleForm: true, entityAction: 'create' });
    }

    onEditItem(entity) {

        console.log(entity);
        this.setState({ entityId: entity.Id, visibleForm: true, entityAction: 'edit' });

    }

    isValid() {
        const errors = {};

        if (this.state.selectIds.length <= 0) {
            errors.selectIds = 'Debe seleccionar por lo menos un ítem';
            abp.notify.error(errors.selectIds, 'Error');
        }

        if (this.state.selectIds.length > 1) {
            errors.selectIds = 'Debe seleccionar exactamente un ítem';
            abp.notify.error(errors.selectIds, 'Error');
        }

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    onCancelItem(e) {
        console.log(e);

        event.preventDefault();

        if (!this.isValid()) {
            return;
        }

        this.setState({ entityId: this.state.selectIds[0], visibleCancelForm: true });
    }


    onDeleteItem(entity) {
        console.log('onDeleteItem Item: ' + entity);

        this.setState({ blocking: true });

        let url = '';
        url = `${this.state.urlApiBase}/DeleteApi`;


        let data = {
            id: entity.Id
        };


        http.post(url, data)
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    abp.notify.success("Proceso guardado exitosamente", "Aviso");

                    var newParams = {
                        fecha: `${moment(this.state.data.fecha).format(config.formatDate)}`
                    };


                    this.props.onRefreshData(newParams);


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

    onDetailItem(entity) {
        console.log('DetailItem Item: ' + entity);
        this.setState({ entityId: entity.Id, visibleForm: true, entityAction: 'show' });
    }

    handleAdded(entity) {

        console.log('add Item: ');
        console.log(entity);
        this.setState({ visibleForm: false, entityId: 0 });

        abp.notify.success("Proceso guardado exitosamente", "Aviso");

        var newParams = {
            fecha: `${moment(this.state.data.fecha).format(config.formatDate)}`
        };


        this.props.onRefreshData(newParams);
    }

    handleUpdated(entity) {

        console.log('update Item: ');
        console.log(entity);
        this.setState({ visibleForm: false, visibleCancelForm: false, entityId: 0 });

        abp.notify.success("Proceso guardado exitosamente", "Aviso");

        var newParams = {
            fecha: `${moment(this.state.data.fecha).format(config.formatDate)}`
        };


        this.props.onRefreshData(newParams);
    }

    handleChangeDate(event) {
        const target = event.target;
        const value = target.type === "checkbox" ? target.checked : target.value;
        const name = target.name;

        if (moment(value).isValid()) {
            const updatedData = {
                ...this.state.data
            };

            updatedData[name] = value;

            this.setState({
                data: updatedData
            });

            var newParams = {
                fecha: `${moment(value).format(config.formatDate)}`
            };

            this.props.onRefreshData(newParams);
        }

    }

    onHide() {
        console.log('onHide ');
        this.setState({ visibleForm: false, visibleCancelForm: false, entityId: 0 });
    }


    handleSelect(isSelected, selectIds) {

        var newSelectIds = [];
        this.setState({ selectIds: selectIds });

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

                    <div className="col">
                        <div className="col nav justify-content-end">
                            <button className="btn btn-outline-primary" onClick={this.onNewItem} > Nuevo Pedido </button>
                            <button className="btn btn-outline-danger" onClick={this.onCancelItem} > Cancelar Pedido </button>
                        </div>
                    </div>
                </div>

                <hr />

                <div>
                    <SolicitudViandaList
                        data={this.props.data}
                        onSelectAction={this.handleSelect}
                        selectIds={this.state.selectIds}
                        onEditAction={this.onEditItem}
                        onDeleteAction={this.onDeleteItem}
                        onDetailAction={this.onDetailItem}
                        editCondition={(row) => { return row.estado !== 0; }}
                        deleteCondition={(row) => { return row.estado !== 0; }}
                    />


                </div>



                <Dialog header="Registro Especial de Pedidos" visible={this.state.visibleForm} width="720px" modal minY={70} onHide={this.onHide} maximizable>
                    <ScrollPanel style={{ width: '690px', height: '400px' }}>
                        <SolicitudViandaForm
                            entityId={this.state.entityId}
                            entityAction={this.state.entityAction}
                            onUpdated={this.handleUpdated}
                            onAdded={this.handleAdded}
                            onHide={this.onHide}
                            show={this.state.visibleForm}
                        />
                    </ScrollPanel>               </Dialog>

                <Dialog header="Cancelar Pedido" visible={this.state.visibleCancelForm} width="680px" modal minY={70} onHide={this.onHide} maximizable>

                    <SolicitudViandaCancelForm
                        entityId={this.state.entityId}
                        onUpdated={this.handleUpdated}
                        onHide={this.onHide}
                        show={this.state.visibleCancelForm}
                    />

                </Dialog>

            </BlockUi>
        );
    }
}

// HOC
const Container = wrapContainer(SolicitudViandaContainer,
    `/proveedor/solicitudvianda/GetSolicitudDiariaApi/`,
    {
        fecha: `${moment().format(config.formatDate)}`
    });

ReactDOM.render(
    <Container />,
    document.getElementById('nuc_tree_body_solicitud_vianda')
);

