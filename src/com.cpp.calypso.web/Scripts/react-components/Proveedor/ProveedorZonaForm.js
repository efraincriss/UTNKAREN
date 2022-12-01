import React, { Component } from 'react';
import BlockUi from 'react-block-ui';
 
import moment from 'moment';

import http from '../Base/HttpService';
import ActionForm from '../Base/ActionForm';
import config from '../Base/Config';
import Field from '../Base/Field';
import wrapForm from '../Base/WrapForm';

class ProveedorZonaForm extends Component {

    constructor(props) {
        super(props);

        this.state = {
            data: this.initData(),
            blocking: true,
            loadDataExtra: false,
            loading: false,
            errors: {} ,

            dataExtra: [] 
        };
        this.handleChange = this.handleChange.bind(this);
        this.setData = this.setData.bind(this);

        this.handleSubmit = this.handleSubmit.bind(this);
    }


    componentDidMount() {
        console.log('SolicitudViandaForm.componentDidMount');
    }

    componentDidUpdate(prevProps) {
        console.log('SolicitudViandaForm.componentDidUpdate');

        if (!this.state.loadDataExtra && this.props.show && !this.state.loading) {
            //Init
            this.loadDataExtra();
        }

        // Typical usage (don't forget to compare props):
        if (this.props.show && (this.props.entityId !== prevProps.entityId ||
            this.props.show !== prevProps.show)) {
            this.loadData();
        }
    }

    initData() {
        let dataInit = {
            Id: 0,
            ProveedorId: 0,
            ZonaId: 0,
            zona_nombre: '',
        };
        return dataInit;
    }

    loadData() {
        console.log('this.props.entityId : ' + this.props.entityId);

        if (this.props.entityId > 0) {

            let url = '';
            url = `${this.props.urlApiBase}/GetProveedorZonaApi/${this.props.entityId}`;


            http.get(url, {})
                .then((response) => {

                    let data = response.data;

                    if (data.success === true) {

                        console.log(response.data);

                        //Fix 
                        let dataEntity = data.result;
                        this.normalizingData(dataEntity);

                        this.setState({
                            data: dataEntity
                        });

                    } else {
                        //TODO: 
                        //Presentar errores... 
                        var message = $.fn.responseAjaxErrorToString(data);
                        abp.notify.error(message, 'Error');
                    }


                    this.setState({ blocking: false, errors: {} });

                })
                .catch((error) => {
                    console.log(error);
                });
        } else {

            this.setState({
                data: this.initData(),
                errors: {},
                blocking: false
            });

        }
    }

    normalizingData(data) {
         
    }

    getZona() {
        let url = '';
        url = `/Proveedor/Proveedor/SearchZonasApi`;

        return http.get(url);
    }


    loadDataExtra() {

        var self = this;

        self.setState({ blocking: true, loading: true });

        Promise.all([this.getZona()])
            .then(function ([zonas]) {
                self.setState({ blocking: false, loadDataExtra: true });


                self.setState({
                    dataExtra: {
                        zonas: self.props.mapDropdown(zonas.data, 'nombre', 'Id') 
                    }
                });

                self.setState({ blocking: false, loading: false });

            })
            .catch((error) => {
                //TODO: Mejorar gestion errores
                self.setState({ blocking: false, loading: false });
                console.log(error);
            });
    }

    

    isValid() {
        const errors = {};

        if (this.state.data.ZonaId  <= 0) {
            errors.ZonaId = 'Zona es requerido';
        }
  

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
        if (this.props.entityAction === 'edit')
            url = `${this.props.urlApiBase}/EditProveedorZonaApi`;
        else
            url = `${this.props.urlApiBase}/CreateProveedorZonaApi`;


        //creating copy of object
        let data = Object.assign({}, this.state.data);
        data.Id = this.props.entityId;                        //updating value
        data.ProveedorId = this.props.padreId;

        http.post(url, data)
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    this.setState({
                        data: this.initData()
                    });


                    if (this.props.entityId <= 0) {
                        this.props.onAdded(response.data.result.id);
                    }
                    else {
                        this.props.onUpdated(response.data.result.id);
                    }

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



    handleChange(event) {
        const target = event.target;
        const value = target.type === "checkbox" ? target.checked : target.value;
        const name = target.name;

        const updatedData = {
            ...this.state.data
        };

        updatedData[name] = value;


        this.setState({
            data: updatedData
        });
    }


    setData(name, value) {

        const updatedData = {
            ...this.state.data
        };

        updatedData[name] = value;

        this.setState({
            data: updatedData
        });
    }


    render() {

        let disabled = false;
        if (this.props.entityAction === 'show') {
            disabled = true;
        }

        let blocking = this.state.blocking || this.state.loading || this.props.blocking;
       
        return (
            (!this.props.show) ? (<div>...</div>) :
                (
            <BlockUi tag="div" blocking={blocking}>
            
                <form onSubmit={this.handleSubmit}>

                    <div className="row">
                        <div className="col">

                            <Field
                                name="ZonaId"
                                required
                                value={this.state.data.ZonaId}
                                label="Zona"
                                options={this.state.dataExtra.zonas}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.ZonaId}
                                readOnly={(this.props.entityAction === 'show')}
                            />
                        </div>
                    </div>
 
                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                        <ActionForm onCancel={this.props.onHide} onSave={this.handleSubmit} />
                    }

                    {this.props.entityAction === 'show' &&
                        <ActionForm onAccept={this.props.onHide} />
                    }

                </form>
                    </BlockUi>
                )
        );
    }
}

export default wrapForm(ProveedorZonaForm);
 