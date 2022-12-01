import React from 'react';
import BlockUi from 'react-block-ui';
import axios from "axios";
import { Dialog } from 'primereact/components/dialog/Dialog';

import http from '../Base/HttpService';
 
import ProveedorContactoTipoOpcionComidaList from './ProveedorContactoTipoOpcionComidaList';
import ProveedorContactoTipoOpcionComidaForm from './ProveedorContactoTipoOpcionComidaForm';

class ProveedorContactoTipoOpcionComidaContainer extends React.Component {


    constructor() {
        super();
        this.state = {

            visibleForm: false,
            entityId: 0,
            entityAction: 'create',
            errors: {},
            entitySelected: {}
             
        };

        this.onNewItem = this.onNewItem.bind(this);
        this.onEditItem = this.onEditItem.bind(this);
        this.onDeleteItem = this.onDeleteItem.bind(this);
        this.onDetailItem = this.onDetailItem.bind(this);

        this.handleUpdated = this.handleUpdated.bind(this);
        this.handleAdded = this.handleAdded.bind(this);
   

     
        this.onHide = this.onHide.bind(this);
    }

    componentDidMount() {

    }

    onNewItem(event) {
        event.preventDefault();
        this.setState({ entityId: 0, visibleForm: true, entityAction: 'create', entitySelected: {} });
    }

    onEditItem(entity, event) {
        event.preventDefault();
        this.setState({ entityId: entity.Id, visibleForm: true, entityAction: 'edit', entitySelected: entity });
    }

    onDetailItem(entity,event) {
        event.preventDefault();
        this.setState({ entityId: entity.Id, visibleForm: true, entityAction: 'show', entitySelected: entity });
    }


    onDeleteItem(entity) {
        console.log(entity);
        if(entity!=null && entity.Id>0){
        axios
        .post("/Proveedor/Proveedor/SearchCanDeleteTipoOpcionComida/"+entity.Id, {})
        .then(response => {
            console.log(response.data);
            if(response.data=="OK"){
                let result = this.props.handleDeleteChild(entity);

                if (result) {
                    this.setState({ visibleForm: false, entityId: 0 });
                } 
            }else{
                abp.notify.warn(
                    "No se puede eliminar opción comida existen datos relacionados",
                    "Error"
                  );

            }
        
        })
        .catch(error => {
          console.log(error);
          abp.notify.error(
            "Ocurrió un Error al realizar la transacción",
            "Error"
          );

        });

    }
      
    }



    handleAdded(entity) {
     
        let result = this.props.handleAddedChild(entity);

        if (result) {
            this.setState({ visibleForm: false, entityId: 0 });
        } 

    }

    handleUpdated(entity) {

        let result = this.props.handleUpdatedChild(entity);

        if (result) {
            this.setState({ visibleForm: false, entityId: 0 });
        } 
    }

    onHide() {
        console.log('onHide ');
        this.setState({ visibleForm: false, entityId: 0 });
    }

    
    render() {

        let blocking = this.props.blocking || this.state.blocking;
        let visibleForm = this.props.visibleForm || this.state.visibleForm;

        return (
            <BlockUi tag="div" blocking={blocking}>

                {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                    <div className="row">
                        <div className="col">
                            <div className="col nav justify-content-end">
                                <button className="btn btn-outline-primary" onClick={this.onNewItem} > Nuevo  </button>
                            </div>
                        </div>
                    </div>
                }

                <hr />

                <div>
                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                        <ProveedorContactoTipoOpcionComidaList
                            data={this.props.data}

                            onEditAction={this.onEditItem}
                            onDeleteAction={this.onDeleteItem}
                        
                            />
                    }

                    {(this.props.entityAction === 'show') &&
                        <ProveedorContactoTipoOpcionComidaList
                            data={this.props.data}
                            onDetailAction={this.onDetailItem}
                        />
                    }
                </div>


                <Dialog header="Tipo Comida / Opciones Comida" visible={this.state.visibleForm} width="480px" modal minY={70} onHide={this.onHide} maximizable>

                    <ProveedorContactoTipoOpcionComidaForm

                        data={this.state.entitySelected}

                        entityId={this.state.entityId}
                        padreId={this.props.padreId}
                        entityAction={this.state.entityAction}

                        onUpdated={this.handleUpdated}
                        onAdded={this.handleAdded}

                        onHide={this.onHide}
                        show={visibleForm}
                        urlApiBase={this.state.urlApiBase}

                        dataExtra={this.props.dataExtra}

                    />

                </Dialog>

            </BlockUi>
        );
    } 
}
 
export default ProveedorContactoTipoOpcionComidaContainer;
 