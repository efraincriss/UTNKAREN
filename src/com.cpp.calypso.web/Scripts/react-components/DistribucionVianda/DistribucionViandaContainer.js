import React from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';
import axios from 'axios';



import { Dialog } from 'primereact/components/dialog/Dialog';
import { ScrollPanel } from 'primereact/components/scrollpanel/ScrollPanel';

import config from '../Base/Config';
import http from '../Base/HttpService';
import wrapContainer from '../Base/WrapContainer';
import moment from 'moment';

import DistribucionViandaList from './DistribucionViandaList';
import DistribucionViandaForm from './DistribucionViandaForm';

 

class DistribucionViandaContainer extends React.Component {

    constructor() {
        super();

        this.state = {
            visibleForm: false,
            entityId: 0
        };

        this.onNewItem = this.onNewItem.bind(this);
        this.onEditItem = this.onEditItem.bind(this);
        this.onDetailItem = this.onDetailItem.bind(this);
        this.onVerItem = this.onVerItem.bind(this);
        this.handleAdded = this.handleAdded.bind(this);
        this.handleUpdated = this.handleUpdated.bind(this);

        this.onHide = this.onHide.bind(this);

    }


    onNewItem(e) {
        this.setState({ entityId: 0, visibleForm: true });
    }

    onEditItem(entity) {

        console.log(entity);
   
        var fecha = moment(entity.fecha).format(config.formatDate);
        var tipo_comida_id = entity.tipo_comida_id;
  
        return (
            window.location.href = `${config.appUrl}/proveedor/DistribucionVianda/Edit?fecha=${fecha}&tipoComidaId=${tipo_comida_id}`
        );

    }

    

    onDetailItem(entity) {
        console.log('DetailItem Item: ' + entity);
    }

    onVerItem(entity) {
        console.log('VerItem Item: ');
        console.log(entity)
        return (
            window.location.href = `${config.appUrl}/proveedor/DistribucionVianda/Ver?fecha=${entity.fecha}&tipoComidaId=${entity.tipo_comida_id}`
        );
    }

    onTransporte(entity) {
        //TODO: Aplicar restricciones, segun el estado
        console.log(entity);

        var fecha = moment(entity.fecha).format(config.formatDate);
        var tipo_comida_id = entity.tipo_comida_id;

        return (
            window.location.href = `${config.appUrl}/proveedor/DistribucionVianda/EditTransportista?fecha=${fecha}&tipoComidaId=${tipo_comida_id}`
        );
    }

    handleAdded(entity) {
         
        this.setState({ visibleForm: false, entityId: 0 });

        var fecha = moment(entity.fecha).format(config.formatDate);
        var tipoComidaId = entity.tipoComidaId;

        return (
            window.location.href = `${config.appUrl}/proveedor/DistribucionVianda/Edit?fecha=${fecha}&tipoComidaId=${tipoComidaId}`
        );
    }

    handleUpdated(entity) {
         
    }

    onHide() {
        console.log('onHide ');
        this.setState({ visibleForm: false, entityId: 0 });
    }

    render() {

        let buttons = [{
            label: "Asignar Transporte",
            onClick: this.onTransporte
        }, {
                label: "Ver",
                onClick: this.onVerItem
            }];
 

        return (
            <BlockUi tag="div" blocking={this.props.blocking}>

               
                <div className="nav justify-content-end">
                    <button className="btn btn-outline-primary" onClick={this.onNewItem} > Nuevo </button>

                </div>
                <hr />
                <div>
                    <DistribucionViandaList
                        data={this.props.data}
                        onEditAction={this.onEditItem}
                        editActionName="Distribuir"
                        onVerAction={this.onVerItem}
                        buttons={buttons}
                    />
                </div>

                <Dialog header="Crear Distribución de Pedidos" visible={this.state.visibleForm} width="360px" modal minY={70} onHide={this.onHide} resizable={true} responsive={true} blockScroll={true} >
                    <ScrollPanel style={{ width: '100%', height: '450px' }}>
                        <DistribucionViandaForm
                            onAdded={this.handleAdded}
                            show={this.state.visibleForm}
                        />
                    </ScrollPanel>
                </Dialog>
                

            </BlockUi>
        );
    }
}

// HOC
const Container = wrapContainer(DistribucionViandaContainer,
    `/proveedor/distribucionvianda/GetPagedApi/`,
    {
        SkipCount: 0,
        MaxResultCount: 100,
        Sorting:"fecha"
    });

ReactDOM.render(
    <Container />,
    document.getElementById('nuc_tree_body_distribucion_vianda')
);

