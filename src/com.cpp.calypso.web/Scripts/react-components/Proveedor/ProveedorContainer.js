import React from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';

import { Dialog } from 'primereact/components/dialog/Dialog';

import http from '../Base/HttpService';
import config from '../Base/Config';
import wrapContainer from '../Base/WrapContainer';

import ProveedorList from './ProveedorList';
import ProveedorForm from './ProveedorForm';


class ProveedorContainer extends React.Component {

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
   

    onNewItem(e) {
       this.setState({ entityId: 0, visibleForm: true, entityAction: 'create' });
    }

    onEditItem(entity) {
        this.setState({ entityId: entity.Id, visibleForm: true, entityAction: 'edit' });
    }

    onDeleteItem(entity) {
     
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
        
         return (
            window.location.href = `${config.appUrl}/proveedor/Proveedor/Details/${entity.Id}`
        );
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
        this.setState({ entityId: 0, visibleForm: false });
    }

    render() {

        let blocking = this.props.blocking || this.state.blocking;

        return (
            <BlockUi tag="div" blocking={blocking}>

                <div className="row">
                   <div className="col">
                        <div className="col nav justify-content-end">
                            <button className="btn btn-outline-primary" onClick={this.onNewItem} > Nuevo  </button>
                        </div>
                    </div>
                </div>

                <hr />
                <div>
                    <ProveedorList
                        data={this.props.data}
                        onEditAction={this.onEditItem}
                        onDetailAction={this.onDetailItem}
                        showActionName={"Detalle"}
                     />
                </div>

                <Dialog header="Proveedor" visible={this.state.visibleForm} width="680px" modal minY={70} onHide={this.onHide} maximizable>

                    <ProveedorForm
                        entityId={this.state.entityId}
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

 

// HOC
const Container = wrapContainer(ProveedorContainer,
    `/proveedor/Proveedor/GetAllApi/`,
    { });

ReactDOM.render(
    <Container />,
    document.getElementById('nuc_tree_body_proveedores')
);