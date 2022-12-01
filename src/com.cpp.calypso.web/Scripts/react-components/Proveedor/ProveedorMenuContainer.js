import React from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';
import axios from 'axios';

import { Dialog } from 'primereact/components/dialog/Dialog';

import config from '../Base/Config';
import wrapContainer from '../Base/WrapContainer';

import ProveedorActionList from './ProveedorActionList';

class ProveedorMenuContainer extends React.Component {

    constructor() {
        super();

        this.state = {
            visibleForm: false,
            entityId: 0
        };

        this.onEditItem = this.onEditItem.bind(this);
        this.onHide = this.onHide.bind(this);
     
    }

    
    onEditItem(entity) {
        console.log('Editar Item: ' + entity);
  
        return (
            window.location.href = `${config.appUrl}/proveedor/menu/Details/${entity.Id}`
        );
    }
   
    onHide() {
        console.log('onHide ');
        this.setState({ visibleForm: false, entityId: 0 });
    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.props.blocking}>
  
                <ProveedorActionList
                    data={this.props.data}
                    onEditAction={this.onEditItem}
                    editActionName="Gestión de Menús"
                />
 
            </BlockUi>
        );
    }
}

// HOC
const Container = wrapContainer(ProveedorMenuContainer,
    `${config.apiUrl}/proveedor/Proveedor/GetAllApi/`,
    {});

ReactDOM.render(
    <Container />,
    document.getElementById('nuc_tree_body_proveedores_menus')
); 

