import React, { Component } from 'react';
import BlockUi from 'react-block-ui';
import axios from 'axios';
import moment from 'moment';

import { Dropdown } from 'primereact/components/dropdown/Dropdown';
import http from '../Base/HttpService';
import ActionForm from '../Base/ActionForm';
import config from '../Base/Config';
import TextInput from '../Base/TextForm';
import wrapForm from '../Base/WrapForm';

import ProveedorContactoTipoOpcionComidaContainer from './ProveedorContactoTipoOpcionComidaContainer';


class ProveedorContactoForm extends Component {

    constructor(props) {
        super(props);

        this.state = {
            data: this.initData(),
            blocking: true,
            loadDataExtra: false,
            loading: false,
            errors: {},

            dataExtra: [],
            dataExtraChild: []
        };
        this.handleChange = this.handleChange.bind(this);
        this.setData = this.setData.bind(this);
        this.handleAddedChild = this.handleAddedChild.bind(this);
        this.handleUpdatedChild = this.handleUpdatedChild.bind(this);
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
            Id: Math.floor((Math.random() * 300) + 1) * -1, 
            ProveedorId: 0,
            proveedor_nombre: '',
            codigo: '',
            EmpresaId: 0,
            empresa_nombre: '',
            objeto: '',
            fecha_inicio: moment().format(config.formatDate),
            fecha_fin: '',
            plazo_pago: 0,
            monto:0,
            orden_compra: '',
            estado: 1,
            estado_nombre: '',
            tipo_opciones_comida: []
        };
        return dataInit;
    }
    loadData() {
        console.log('this.props.entityId : ' + this.props.entityId);

        if (this.props.entityId > 0) {

            let url = '';
            url = `${this.props.urlApiBase}/GetProveedorContactoApi/${this.props.entityId}`;


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

    normalizingData(dataEntity) {
        dataEntity['fecha_inicio'] = moment(dataEntity['fecha_inicio']).format(config.formatDate);
        if (dataEntity['fecha_fin'] !== undefined && dataEntity['fecha_fin'] !== null && moment(dataEntity['fecha_alcancce']).isValid()) {
            dataEntity['fecha_fin'] = moment(dataEntity['fecha_fin']).format(config.formatDate);
        } else {
            dataEntity['fecha_fin'] = '';
        }
    }

    getEmpresa() {
        let url = '';
        url = `/Proveedor/Proveedor/SearchEmpresasApi`;

        return http.get(url);
    }

    getTipoComida() {
        let url = '';
        url = `/Proveedor/Proveedor/SearchByCodeApi/?code=TIPOCOMIDA`;

        return http.get(url);
    }

    getOpcionComida() {
        let url = '';
        url = `/Proveedor/Proveedor/SearchByCodeApi/?code=OPCIONCOMIDA`;

        return http.get(url);
    }
    



    loadDataExtra() {

        var self = this;

        self.setState({ blocking: true, loading: true });

        Promise.all([this.getEmpresa(), this.getTipoComida(), this.getOpcionComida()])
            .then(function ([empresas, tiposComidas, opcionescomida]) {
                self.setState({ blocking: false, loadDataExtra: true });


                self.setState({
                    dataExtra: {
                        empresas: self.props.mapDropdown(empresas.data, 'razon_social', 'Id')
                    },
                    dataExtraChild: {
                        opcionescomida: self.props.mapDropdown(opcionescomida.data, 'nombre', 'Id'),
                        tiposComidas: self.props.mapDropdown(tiposComidas.data, 'nombre', 'Id'),
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

        if (this.state.data.EmpresaId === undefined || this.state.data.EmpresaId <= 0) {
            errors.EmpresaId = 'Debe seleccionar una Empresa';
        }

        if (this.state.data.codigo.length <= 1) {
            errors.codigo = 'Código es requerido';
        }

        if (this.state.data.fecha_inicio === undefined ||
            !moment(this.state.data.fecha_inicio).isValid()) {
            errors.fecha_inicio = 'Debe ingresar una Fecha';
        }

        if (this.state.data.fecha_fin === undefined ||
            !moment(this.state.data.fecha_fin).isValid()) {
            errors.fecha_fin = 'Debe ingresar una Fecha';
        }

        if (this.state.data.plazo_pago === undefined || this.state.data.plazo_pago < 0) {
            errors.plazo_pago = 'Debe ingresar un valor mayor o igual a Cero';
        }

        if (this.state.data.monto === undefined || this.state.data.monto <= 0) {
            errors.monto = 'Debe ingresar un valor mayor   a Cero';
        }

        if (this.state.data.objeto.length <= 1) {
            errors.objeto = 'Descripción es requerido';
        }

        /*
        if (this.state.data.tipo_opciones_comida.length == 0) {
            errors.tipo_opciones_comida = 'Debe ingresar por lo menos un item de tipo de comida / opciones de Comida';
        }
        */

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
            url = `${this.props.urlApiBase}/EditProveedorContactoApi`;
        else
            url = `${this.props.urlApiBase}/CreateProveedorContactoApi`;


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

 
    handleAddedChild(entity) {

        console.log('add Item: ');
        console.log(entity);
       ///TODO: Pendiente...

    }

    handleUpdatedChild(entity) {

        console.log('update Item: ');
        console.log(entity);
        ///TODO: Pendiente...
    }

    render() {

        let disabled = false;
        if (this.props.entityAction === 'show') {
            disabled = true;
        }

        let blocking = this.props.blocking || this.state.blocking;
 
        return (

            <BlockUi tag="div" blocking={blocking}>

                <form onSubmit={this.handleSubmit}>

                    <div className="row">
                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="descripcion" className="col-sm-12 col-form-label">Código</label>
                                <div className="col-sm-12">
                                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                                        <input name="codigo" type="text" value={this.state.data.codigo} onChange={this.handleChange} className="form-control" />
                                    }

                                    {this.props.entityAction === 'show' &&
                                        <input readOnly name="codigo" type="text" value={this.state.data.codigo} className="form-control-plaintext" />
                                    }

                                    {this.state.errors.codigo && <div className="alert alert-danger">{this.state.errors.codigo}</div>}
                                </div>
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group row">

                                <label htmlFor="estado" className="col-sm-12 col-form-label">Activo</label>
                                <div className="col-sm-12">
                                    <div className="">
                                        <input
                                            name="estado"
                                            type="checkbox"
                                            checked={this.state.data.estado}
                                            onChange={this.handleChange}
                                        />
                                        <label className="form-check-label" htmlFor="estado">
                                            (Si/No)
                                       </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="EmpresaId" className="col-sm-12 col-form-label" >Empresa</label>
                                <div className="col-sm-12">

                                    <Dropdown
                                        id="EmpresaId"
                                        value={this.state.data.EmpresaId}
                                        options={this.state.dataExtra.empresas}
                                        onChange={(e) => {
                                            this.setData("EmpresaId", e.value);
                                        }}
                                        filter={true} filterPlaceholder="Selecciona una Empresa"
                                        filterBy="label,value" placeholder="Selecciona una Empresa"

                                        style={{ width: '100%' }}
                                        required
                                        disabled={disabled}

                                    />

                                    {this.state.errors.EmpresaId && <div className="alert alert-danger">{this.state.errors.EmpresaId}</div>}
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="objeto" className="col-sm-12 col-form-label">Descripción</label>
                                <div className="col-sm-12">
                                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                                        <input name="objeto" type="text" value={this.state.data.objeto} onChange={this.handleChange} className="form-control" />
                                    }

                                    {this.props.entityAction === 'show' &&
                                        <input readOnly name="objeto" type="text" value={this.state.data.objeto} className="form-control-plaintext" />
                                    }

                                    {this.state.errors.objeto && <div className="alert alert-danger">{this.state.errors.objeto}</div>}
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="fecha_inicio" className="col-sm-12 col-form-label">Fecha inicio</label>
                                <div className="col-sm-12">
                                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                                        <input name="fecha_inicio" type="date" value={this.state.data.fecha_inicio} onChange={this.handleChange} className="form-control" />
                                    }
                                    {(this.props.entityAction === 'show') &&
                                        <input readOnly name="fecha_inicio" type="date" value={this.state.data.fecha_inicio} className="form-control-plaintext" />
                                    }
                                    {this.state.errors.fecha_inicio && <div className="alert alert-danger">{this.state.errors.fecha_inicio}</div>}
                                </div>
                            </div>
                        </div>
                         
                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="fecha_fin" className="col-sm-12 col-form-label">Fecha fin</label>
                                <div className="col-sm-12">
                                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                                        <input name="fecha_fin" type="date" value={this.state.data.fecha_fin} onChange={this.handleChange} className="form-control" />
                                    }
                                    {(this.props.entityAction === 'show') &&
                                        <input readOnly name="fecha_fin" type="date" value={this.state.data.fecha_fin} className="form-control-plaintext" />
                                    }
                                    {this.state.errors.fecha_fin && <div className="alert alert-danger">{this.state.errors.fecha_fin}</div>}
                                </div>
                            </div>
                        </div>
                         
                    </div>

                    <div className="row">
                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="plazo_pago" className="col-sm-12 col-form-label">Plazo de Pago</label>
                                <div className="col-sm-12">
                                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                                        <input name="plazo_pago" type="number" value={this.state.data.plazo_pago} onChange={this.handleChange} className="form-control" />
                                    }
                                    {(this.props.entityAction === 'show') &&
                                        <input readOnly name="plazo_pago" type="date" value={this.state.data.plazo_pago} className="form-control-plaintext" />
                                    }
                                    {this.state.errors.plazo_pago && <div className="alert alert-danger">{this.state.errors.plazo_pago}</div>}
                                </div>
                            </div>
                        </div>

                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="monto" className="col-sm-12 col-form-label">Monto</label>
                                <div className="col-sm-12">
                                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                                        <input name="monto" type="number" value={this.state.data.monto} onChange={this.handleChange} className="form-control" />
                                    }
                                    {(this.props.entityAction === 'show') &&
                                        <input readOnly name="monto" type="date" value={this.state.data.monto} className="form-control-plaintext" />
                                    }
                                    {this.state.errors.monto && <div className="alert alert-danger">{this.state.errors.monto}</div>}
                                </div>
                            </div>
                        </div>

                    </div>

                    <div className="row">

                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="orden_compra" className="col-sm-12 col-form-label">Orden de Compra</label>
                                <div className="col-sm-12">
                                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                                        <input name="orden_compra" type="date" value={this.state.data.orden_compra} onChange={this.handleChange} className="form-control" />
                                    }
                                    {(this.props.entityAction === 'show') &&
                                        <input readOnly name="orden_compra" type="date" value={this.state.data.orden_compra} className="form-control-plaintext" />
                                    }
                                    {this.state.errors.orden_compra && <div className="alert alert-danger">{this.state.errors.orden_compra}</div>}
                                </div>
                            </div>

                        </div>

                        <div className="col">
                            ARCHIVOS...PENDIENTES>..
                            </div>
                    </div>

                    <div className="row">
                        <div className="col"> 
 
                            <ProveedorContactoTipoOpcionComidaContainer

                                data={this.state.data.tipo_opciones_comida}

                                blocking={this.state.blocking}

                                padreId={this.state.data.Id}

                                handleUpdatedChild={this.handleUpdatedChild}
                                handleAddedChild={this.handleAddedChild}


                                onRefreshData={this.onRefreshData}

                                dataExtra={this.state.dataExtraChild}
                            />

                            {this.state.errors.tipo_opciones_comida && <div className="alert alert-danger">{this.state.errors.tipo_opciones_comida}</div>}
 
                            <hr /> 
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
        );
    }
}

export default wrapForm(ProveedorContactoForm);