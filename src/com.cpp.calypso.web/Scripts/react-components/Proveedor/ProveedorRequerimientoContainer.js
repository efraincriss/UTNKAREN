import React from 'react';
import BlockUi from 'react-block-ui';

import { Dialog } from 'primereact/components/dialog/Dialog';
 
import http from '../Base/HttpService';
import ProveedorRequerimientoList from './ProveedorRequerimientoList';
//import ProveedorRequerimientoForm from './ProveedorRequerimientoForm';

class ProveedorRequerimientoContainer extends React.Component {

    constructor() {
        super();
        this.state = {
           
            visibleForm: false,
            entityId: 0,
            entityAction: 'create',
            errors: {},
            selectIds: [],
            urlApiBase: '/proveedor/Proveedor/'
        };

        this.onEnabled = this.onEnabled.bind(this);
        this.onDisabled = this.onDisabled.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);

        this.handleSelect = this.handleSelect.bind(this);

        this.onHide = this.onHide.bind(this);
    }

    componentDidMount() {
         
    }

    handleSelect(isSelected, selectIds) {

        var newSelectIds = [];
        this.setState({ selectIds: selectIds });

    }

    onHide() {
        console.log('onHide ');
        this.setState({ visibleForm: false, entityId: 0 });
    }

    isValid() {
        const errors = {};

        if (this.state.selectIds.length <= 0) {
            errors.selectIds = 'Debe seleccionar por lo menos un ítem';
            abp.notify.error(errors.selectIds, 'Error');
        }

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    onEnabled(event) {
        event.preventDefault();

        this.handleSubmit(true);

    }

    onDisabled(event) {
        event.preventDefault();

        this.handleSubmit(false);
    }

    handleSubmit(enabled) {

        event.preventDefault();

        if (!this.isValid()) {
            return;
        }


        this.setState({ blocking: true });

        let url = '';
        url = `${this.state.urlApiBase}/EditProveedorRequerimientoApi`;


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

    render() {

        let blocking = this.props.blocking || this.state.blocking;

        return (
            <BlockUi tag="div" blocking={blocking}>
                <div className="row">


                    <div className="col">
                        <div className="col nav justify-content-end">
                            <button className="btn btn-outline-primary" onClick={this.onEnabled} > Cumple </button>
                            <button className="btn btn-outline-primary" onClick={this.onDisabled} > No Cumple </button>
                        </div>
                    </div>
                </div>

                <hr />

                <div>
                    <ProveedorRequerimientoList
                        data={this.props.data}
                        onSelectAction={this.handleSelect}
                        selectIds={this.state.selectIds}
                    />
                </div>
                  
               
            </BlockUi>
        );
    }

}
 
export default ProveedorRequerimientoContainer;
 