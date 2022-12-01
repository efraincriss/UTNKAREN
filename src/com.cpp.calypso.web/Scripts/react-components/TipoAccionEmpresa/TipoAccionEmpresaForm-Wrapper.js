import React, { Component } from 'react';
import BlockUi from 'react-block-ui';
import axios from 'axios';
import moment from 'moment';

import http from '../Base/HttpService';
import { Dropdown } from 'primereact/components/dropdown/Dropdown';

import config from '../Base/Config';
import wrapForm from '../Base/WrapForm';
import ActionForm from '../Base/ActionForm';

class TipoAccionEmpresaForm extends Component {

    constructor(props) {
        super(props);
      
        this.state = {
             
           
            dataExtra: {
                empresas: [],
                acciones: [],
                tiposComidas: []
            },
            
            errors: {} 
        };

        props.registerLoadDataExtraHandler(this.loadDataExtra);
        props.registerInitDataExtraHandler(this.initData);
    }

 

    initData() {
        let dataInit = {
            EmpresaId: 0,
            empresa_nombre: '',
            TipoOpcionComidaId: 0,
            tipo_comida_nombre: '',
            AccionId:0,
            accion_nombre:  '',
            hora_desde: '',
            hora_hasta:''
        };
        return dataInit; 
    }

      

    getTipoComida() {
        let url = '';
        url = `/Proveedor/TipoAccion/SearchByCodeApi/?code=TIPOCOMIDA`;
        
        return http.get(url);
    }

    getAccion() {
        let url = '';
        url = `/Proveedor/TipoAccion/SearchByCodeApi/?code=ACCION`;
        
        return http.get(url);
    }


    getEmpresa() {
        let url = '';
        url = `/Proveedor/TipoAccion/SearchEmpresasApi`;

        return http.get(url);
    }
 

    loadDataExtra() {

        var self = this;
      

        Promise.all([this.getAccion(), this.getTipoComida(), this.getEmpresa()])
            .then(function ([acciones, tiposComidas,empresas]) {

      
                self.setState({
                    dataExtra: {
                        acciones: self.mapDropdown(acciones.data, 'nombres', 'Id'),
                        tiposComidas: self.mapDropdown(tiposComidas.data, 'nombre', 'Id'),
                        empresas: self.mapDropdown(empresas.data, 'razon_social', 'Id')
                    }
                });

                this.props.completedDataExtraHandler();

                //self.setState({ blocking: false, loadDataExtra: true });
                //self.setState({ blocking: false, loading: false});

            })
            .catch((error) => {
                //TODO: Mejorar gestion errores
                //self.setState({ blocking: false, loading: false });
                this.props.completedDataExtraHandler();
                console.log(error);
            });
    }

   
    isValid() {
        const errors = {};

        if (this.props.data.EmpresaId === undefined || this.props.data.EmpresaId <= 0) {
            errors.EmpresaId = 'Debe seleccionar una Empresa';
        }

        if (this.props.data.tipo_comida_id === undefined || this.props.data.tipo_comida_id <= 0) {
            errors.tipo_comida_id = 'Debe seleccionar un tipo de Comida';
        }
  
        if (this.props.data.AccionId === undefined || this.props.data.AccionId <= 0) {
            errors.AccionId = 'Debe seleccionar una Acción';
        }

        if (this.state.props.hora_desde === undefined || this.state.props.hora_desde.length === 0
            || !moment(this.state.props.hora_desde).isValid()) {
            errors.hora_desde = 'Debe ingresar una hora desde';
        }

        if (this.state.props.hora_hasta === undefined || this.state.props.hora_hasta.length === 0
            || !moment(this.state.props.hora_hasta).isValid()) {
            errors.hora_hasta = 'Debe ingresar una hora hasta';
        }
        

        if (this.props.hora_desde > this.props.hora_hasta) {
            this.props.formIsValid = false;
            errors.hora_desde = 'Debe ingresar una hora desde menor a hora hasta';
            errors.hora_hasta = 'Debe ingresar una hora hasta mayor a hora desde';
        }


        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

  
    render() {

        let disabled = false;
        if (this.props.entityAction === 'edit' || this.props.entityAction === 'show') {
            disabled = true;
        }
 
        return (

            <BlockUi tag="div" blocking={this.props.blocking}>

                <form onSubmit={this.props.handleSubmit}>
                    <div className="row">
                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="EmpresaId" className="col-sm-12 col-form-label" >Empresa</label>
                                <div className="col-sm-12">

                                    <Dropdown
                                        id="EmpresaId"
                                        value={this.props.data.EmpresaId}
                                        options={this.state.dataExtra.empresas}
                                        onChange={(e) => {
                                            this.props.setData("EmpresaId", e.value);
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
                                <label htmlFor="tipo_comida_id" className="col-sm-12 col-form-label" >Tipo Comida</label>
                                <div className="col-sm-12">

                                    <Dropdown
                                        id="tipo_comida_id"
                                        value={this.props.data.tipo_comida_id}
                                        options={this.state.dataExtra.tiposComidas}
                                        onChange={(e) => {
                                            this.props.setData("tipo_comida_id", e.value);
                                        }}
                                        style={{ width: '100%' }}
                                        required
                                        disabled={disabled}
                                          
                                    />

                                    {this.state.errors.tipo_comida_id && <div className="alert alert-danger">{this.state.errors.tipo_comida_id}</div>}
                                </div>
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="AccionId" className="col-sm-12 col-form-label" >Acción</label>
                                <div className="col-sm-12">

                                    <Dropdown
                                        id="AccionId"
                                        value={this.props.data.AccionId}
                                        options={this.state.dataExtra.acciones}
                                        onChange={(e) => {
                                            this.props.setData("AccionId", e.value);
                                        }}
                                        style={{ width: '100%' }}
                                        required
                                        disabled={disabled}

                                    />

                                    {this.state.errors.AccionId && <div className="alert alert-danger">{this.state.errors.AccionId}</div>}
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="hora_desde" className="col-sm-12 col-form-label">Hora Desde</label>
                                <div className="col-sm-12">
                                    <input name="hora_desde" type="time" value={this.props.data.hora_desde} onChange={this.handleChange} className="form-control" />
                                    {this.state.errors.hora_desde && <div className="alert alert-danger">{this.state.errors.hora_desde}</div>}
                                </div>
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group row">
                                <label htmlFor="hora_hasta" className="col-sm-12 col-form-label">Hora Hasta</label>
                                <div className="col-sm-12">
                                    <input name="hora_hasta" type="time" value={this.props.data.hora_hasta} onChange={this.handleChange} className="form-control" />
                                    {this.state.errors.hora_hasta && <div className="alert alert-danger">{this.state.errors.hora_hasta}</div>}
                                </div>
                            </div>
                        </div>

                    </div>
                     

                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                        <ActionForm onCancel={this.props.onHide} />
                    }

                    {this.props.entityAction === 'show' &&
                        <ActionForm onAccept={this.props.onHide} />
                    }

                    <button type="submit" className="btn btn-outline-primary"><i className="fa fa-save"></i> OK </button>

       
                </form>
            </BlockUi>
        );
    }
}

export default wrapForm(TipoAccionEmpresaForm);