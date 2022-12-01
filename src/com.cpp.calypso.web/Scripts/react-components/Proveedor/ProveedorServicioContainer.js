import React from 'react';
import BlockUi from 'react-block-ui';

import { Dialog } from 'primereact/components/dialog/Dialog';

import http from '../Base/HttpService';
import ProveedorServicioList from './ProveedorServicioList';
import ProveedorServicioForm from './ProveedorServicioForm';

class ProveedorServicioContainer extends React.Component {


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
    }

    componentDidMount() {

    }

    onNewItem(e) {
        console.log(e);
        this.setState({ entityId: 0, visibleForm: true, entityAction: 'create' });
    }

    onEditItem(entity) {

        console.log(entity);
        this.setState({ entityId: entity.Id, visibleForm: true, entityAction: 'edit' });
    }

    onDetailItem(entity) {
        console.log('DetailItem Item: ' + entity);
        this.setState({ entityId: entity.Id, visibleForm: true, entityAction: 'show' });
    }


    onDeleteItem(entity) {
        console.log('onDeleteItem Item: ' + entity);

        this.setState({ blocking: true });

        let url = '';
        url = `${this.state.urlApiBase}/DeleteProveedorServiceApi`;


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



    render() {

        let dialogWith = "680px";

        let blocking = this.props.blocking || this.state.blocking;

        return (
            <BlockUi tag="div" blocking={blocking}>
                <div className="row">


                    <div className="col">
                        <div className="col nav justify-content-end">
                            <button className="btn btn-outline-primary" onClick={this.onNewItem} > Nuevo Servicio  </button>
                        </div>
                    </div>
                </div>

                <hr />

                <div>
                    <ProveedorServicioList
                        data={this.props.data}
                        onEditAction={this.onEditItem}
                        //onDeleteAction={this.onDeleteItem}
                        onDetailAction={this.onDetailItem}
                    />
                </div>


                <Dialog header="Servicios" visible={this.state.visibleForm} width="480px" modal minY={70} onHide={this.onHide} maximizable>

                    <ProveedorServicioForm

                        entityId={this.state.entityId}
                        padreId={this.props.padreId}
                        entityAction={this.state.entityAction}
                        onUpdated={this.handleUpdated}
                        onAdded={this.handleAdded}
                        onHide={this.onHide}
                        show={this.state.visibleForm}
                        urlApiBase={this.state.urlApiBase}
                    />

                </Dialog>

            </BlockUi>
        );
    }

}
 
export default ProveedorServicioContainer;
 