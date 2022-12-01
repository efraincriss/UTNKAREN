import React from 'react';
import BlockUi from 'react-block-ui';

import { Dialog } from 'primereact/components/dialog/Dialog';

import http from '../Base/HttpService';
import config from '../Base/Config';

import ProveedorContactoList from './ProveedorContactoList';

class ProveedorContactoContainer extends React.Component {


    constructor() {
        super();
        this.state = {

            visibleForm: false,
            entityId: 0,
            entityAction: 'create',
            errors: {},
            urlApiBase: '/proveedor/Proveedor/'
        };

        this.onNewItem = this.onNewItem.bind(this);
        this.onEditItem = this.onEditItem.bind(this);
        this.onDeleteItem = this.onDeleteItem.bind(this);
        this.onDetailItem = this.onDetailItem.bind(this);


        this.handleAdded = this.handleAdded.bind(this);
        this.handleUpdated = this.handleUpdated.bind(this);

        this.onHide = this.onHide.bind(this);
        this.onDownload = this.onDownload.bind(this);
    }

    componentDidMount() {

    }

    onNewItem(e) {

        console.log('onNewItem Item: ' + e);
        let entityId = 0;
        let entityAction = 'create';
        return (
            window.location.href = `${config.appUrl}/proveedor/Proveedor/EditContrato/?proveedorId=${this.props.padreId}&contratoId=${entityId}&entityAction=${entityAction}`
        );
    }

    onEditItem(entity) {

        console.log('onEditItem Item: ' + entity);
        let entityId = entity.Id;
        let entityAction = 'edit';
        return (
            window.location.href = `${config.appUrl}/proveedor/Proveedor/EditContrato/?proveedorId=${this.props.padreId}&contratoId=${entityId}&entityAction=${entityAction}`
        );
    }

    onDetailItem(entity) {

        console.log('DetailItem Item: ' + entity);
        let entityId = entity.Id;
        let entityAction = 'show';
        return (
            window.location.href = `${config.appUrl}/proveedor/Proveedor/EditContrato/?proveedorId=${this.props.padreId}&contratoId=${entityId}&entityAction=${entityAction}`
        );
    }


    onDeleteItem(entity) {
        console.log('onDeleteItem Item: ' + entity);

        this.setState({ blocking: true });

        let url = '';
        url = `${this.state.urlApiBase}/DeleteProveedorContactoApi`;


        let data = {
            id: entity.Id
        };


        http.post(url, data)
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    abp.notify.success("Proceso guardado exitosamente", "Aviso");

                    var newParams = {
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

    handleAdded(entity) {

        console.log('add Item: ');
        console.log(entity);
        this.setState({ visibleForm: false, entityId: 0 });

        abp.notify.success("Proceso guardado exitosamente", "Aviso");

        var newParams = {

        };


        this.props.onRefreshData(newParams);

    }

    handleUpdated(entity) {

        console.log('update Item: ');
        console.log(entity);
        this.setState({ visibleForm: false, entityId: 0 });

        abp.notify.success("Proceso guardado exitosamente", "Aviso");

        var newParams = {
        };


        this.props.onRefreshData(newParams);
    }

    onHide() {
        console.log('onHide ');
        this.setState({ visibleForm: false, entityId: 0 });
    }

    onDownload(entity) {

        return (
            window.location = `/Proveedor/Proveedor/Descargar/${entity.documentacion_id}`
        );
    }

    onTarifasHoteles = entity => {
        return (
            window.location = `${config.appUrl}/proveedor/TarifaHotel/Details/${entity.Id}`
        );
    }
    onTarifasLavanderia = entity => {
        return (
            window.location = `${config.appUrl}/proveedor/TarifaLavanderia/Details/${entity.Id}`
        );
    }

    render() {

        let blocking = this.props.blocking || this.state.blocking;
        let tieneServicioHospedaje = this.props.tieneServicioHospedaje;
        let tieneServicioLavanderia = this.props.tieneServicioLavanderia;
        let buttons = [{
            label: "Doc.Orden Compra",
            onClick: this.onDownload,
            onCondition: function (row) { return row.documentacion_id && row.documentacion_id > 0; }
        },
        {
            label: "Hospedaje",
            onClick: this.onTarifasHoteles,
            onCondition: function (row) { return tieneServicioHospedaje; }
        },
        {
            label: "Lavandería",
            icon: "fa fa-fa-snowflake-o",
            onClick: this.onTarifasLavanderia,
            onCondition: function (row) { return tieneServicioLavanderia; }
        }];

        return (
            <BlockUi tag="div" blocking={blocking}>
                <div className="row">


                    <div className="col">
                        <div className="col nav justify-content-end">
                            <button className="btn btn-outline-primary" onClick={this.onNewItem} >Nuevo Contrato</button>
                        </div>
                    </div>
                </div>

                <hr />

                <div>
                    <ProveedorContactoList
                        data={this.props.data}
                        onEditAction={this.onEditItem}
                        onDetailAction={this.onDetailItem}
                        buttons={buttons}
                    />
                </div>




            </BlockUi>
        );
    }
}

export default ProveedorContactoContainer;
