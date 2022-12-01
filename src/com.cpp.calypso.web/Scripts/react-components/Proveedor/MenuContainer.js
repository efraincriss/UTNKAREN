import React from 'react';
import BlockUi from 'react-block-ui';
import http from '../Base/HttpService'; 
import MenuList from './MenuList';
import FileDownload from '../Base/FileDownload'; 
import config from '../Base/Config';


class MenuContainer extends React.Component {

    constructor() {
        super();
        this.state = {
            visibleForm: false,
            entityId: 0,
            selectIds: []
        };

        this.onEnabled = this.onEnabled.bind(this);
        this.onDisabled = this.onDisabled.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);

        this.handleSelect = this.handleSelect.bind(this);
        this.onDownload = this.onDownload.bind(this);
    }
 

   
    onEnabled(event) {
        event.preventDefault();

        this.handleSubmit(true);
 
    }

    onDisabled(event) {
        event.preventDefault();
         
        this.handleSubmit(false);
    }

    isValid() {
        const errors = {};

        if (this.state.selectIds.length <= 0) {
            errors.selectIds = 'Debe seleccionar por lo menos un �tem';
            abp.notify.error(errors.selectIds, 'Error');
        }
        
        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    handleSubmit(enabled) {

        event.preventDefault();

        if (!this.isValid()) {
            return;
        }


        this.setState({ blocking: true });

        let url = '';
        url = `/proveedor/menu/EditApi`;
        

        http.post(url, {
            activar: enabled,
            selectedIds: this.state.selectIds
        })
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    this.setState({ selectIds: [] });

                    abp.notify.success("Proceso guardado exitosamente", "Aviso");

                    this.props.onRefreshData();

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

    
    handleSelect(isSelected,selectIds) {

        var newSelectIds = [];
        this.setState({ selectIds: selectIds });

    }
 
    onDownload(entity) {
         
        console.log(entity);
   
        return (
            window.location = `/Proveedor/Menu/Descargar/${entity.documentacion_id}`
        );     
    }

    render() {

        let buttons = [{
            label: "Descargar",
            onClick: this.onDownload,
            onCondition: function (row) { return row.documentacion_id && row.documentacion_id>0; }
        }];



        return (
            <BlockUi tag="div" blocking={this.props.blocking}>
                <div className="row">
                    <div className="col">
                        <div className="col nav justify-content-end">
                            <button className="btn btn-outline-primary" onClick={this.onEnabled} > Aprobar </button>
                            <button className="btn btn-outline-primary" onClick={this.onDisabled} > Negar </button>
                        </div>
                    </div>
                </div>

                <hr />
               
                   <div>
                        <MenuList
                            data={this.props.data}
                            onSelectAction={this.handleSelect}
                            selectIds={this.state.selectIds}
                            buttons={buttons}
                        />
                    </div>
                 
 
            </BlockUi>
        );
    }

}

export default MenuContainer;
