import React, { Component } from 'react';
import BlockUi from 'react-block-ui';
import moment from 'moment';
import http from '../Base/HttpService';
import config from '../Base/Config';
import Field from '../Base/Field';
import ActionForm from '../Base/ActionForm';
import wrapForm from '../Base/WrapForm';

class TipoAccionEmpresaForm extends Component {

    constructor(props) {
        super(props);
      
        this.state = {
            data: this.initData(),
            dataExtra: {
                empresas: [],
                acciones: [],
                tiposComidas: []
            },
            dataAux:{},
            blocking: true,
            loadDataExtra: false,
            loading:false,
            errors: {} 
        };

        this.handleChange = this.handleChange.bind(this);
        this.setData = this.setData.bind(this);
         
        this.handleSubmit = this.handleSubmit.bind(this);
    }


    componentDidMount() {
        console.log('SolicitudViandaForm.componentDidMount');
        console.log(this.props);
        this.loadData2();
    }

    componentDidUpdate(prevProps) {
        console.log('SolicitudViandaForm.componentDidUpdate');

        if (!this.state.loadDataExtra && this.props.show && !this.state.loading) {
            //Init
            this.loadDataExtra();
        }

        // Typical usage (don't forget to compare props):
        if (this.props.show  && (this.props.entityId !== prevProps.entityId || 
            this.props.show !== prevProps.show)) {
            this.loadData();
        }
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

    loadData() {
        console.log('this.props.entityId : ' + this.props.entityId);

        if (this.props.entityId > 0) {

            let url = '';
            url = `${this.props.urlApiBase}/GetApi/${this.props.entityId}`;
             

            http.get(url, {})
                .then((response) => {

                    let data = response.data;

                    if (data.success === true) {
 
                        console.log(response.data);

                        //Fix 
                        let dataEntity = data.result;
                        
                        this.setState({
                            data: dataEntity
                        });

                    } else {
                        //TODO: 
                        //Presentar errores... 
                        var message = $.fn.responseAjaxErrorToString(data);
                        abp.notify.error(message, 'Error');
                    }


                    this.setState({ entityId: 0, visibleForm: true });
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

    loadData2() {
        let url = '';
        url = `${this.props.urlApiBase}GetAllApi/`;

        http.get(url, {})
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    //console.log(response.data);

                    //Fix 
                    let dataEntity = data.result;

                    this.setState({
                        dataAux: dataEntity
                    });

                } else {
                    //TODO: 
                    //Presentar errores... 
                    var message = $.fn.responseAjaxErrorToString(data);
                    abp.notify.error(message, 'Error');
                }


                this.setState({ entityId: 0, visibleForm: true });
            })
            .catch((error) => {
                console.log(error);
            });
        
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

        self.setState({ blocking: true, loading:   true });

        Promise.all([this.getAccion(), this.getTipoComida(), this.getEmpresa()])
            .then(function ([acciones, tiposComidas,empresas]) {
                self.setState({ blocking: false, loadDataExtra: true });

     
                self.setState({
                    dataExtra: {
                        acciones: self.props.mapDropdown(acciones.data, 'nombre', 'Id'),
                        tiposComidas: self.props.mapDropdown(tiposComidas.data, 'nombre', 'Id'),
                        empresas: self.props.mapDropdown(empresas.data, 'razon_social', 'Id')
                    }
                });

                self.setState({ blocking: false, loading: false});

            })
            .catch((error) => {
                //TODO: Mejorar gestion errores
                self.setState({ blocking: false, loading: false });
                console.log(error);
            });
    }


    existeRegistro(data, IdEmpresa, idAccion, idTipoComida) {
        let flag = false;

        // iterate over each element in the array
        for (var i = 0; i < data.length; i++) {
            // look for the entry with a matching `code` value
            if (data[i].EmpresaId == IdEmpresa && data[i].AccionId == idAccion && data[i].tipo_comida_id == idTipoComida) {
                flag = true
                console.log("registro existe " + IdEmpresa + " " + idAccion + " " + idTipoComida);
            }
        }

        return flag;
    }

    isValid() {
        const errors = {};

        //console.log(this.state.dataAux);
        //console.log("Registro");
        //this.existeRegistro(this.state.dataAux, 1, 9191, 10962);

        if (this.existeRegistro(this.state.dataAux, this.state.data.EmpresaId, this.state.data.AccionId, this.state.data.tipo_comida_id)) {
            errors.EmpresaId = 'Empresa, accion y tipo de comida ya existen, seleccione otros datos';
        }

        if (this.state.data.EmpresaId === undefined || this.state.data.EmpresaId <= 0) {
            errors.EmpresaId = 'Debe seleccionar una Empresa';
        }

        if (this.state.data.tipo_comida_id === undefined || this.state.data.tipo_comida_id <= 0) {
            errors.tipo_comida_id = 'Debe seleccionar un tipo de Comida';
        }
  
        if (this.state.data.AccionId === undefined || this.state.data.AccionId <= 0) {
            errors.AccionId = 'Debe seleccionar una Acción';
        }

        if (this.state.data.hora_desde === undefined || this.state.data.hora_desde.length === 0
            || !moment(this.state.data.hora_desde, config.formatTime).isValid()) {
            errors.hora_desde = 'Debe ingresar una hora desde';
        }

        if (this.state.data.hora_hasta === undefined || this.state.data.hora_hasta.length === 0
            || !moment(this.state.data.hora_hasta, config.formatTime).isValid()) {
            errors.hora_hasta = 'Debe ingresar una hora hasta';
        }
        

        if (this.state.data.hora_desde > this.state.data.hora_hasta) {
           
            errors.hora_desde = 'Debe ingresar una hora desde menor a hora hasta';
            errors.hora_hasta = 'Debe ingresar una hora hasta mayor a hora desde';
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
            url = `${this.props.urlApiBase}/EditApi`;
        else
            url = `${this.props.urlApiBase}/CreateApi`;

       
         //creating copy of object
        let data = Object.assign({}, this.state.data);   
        data.Id = this.props.entityId;                        //updating value

     
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
                                name="EmpresaId"
                                value={this.state.data.EmpresaId}
                                        label="Empresa"
                                        required
                                options={this.state.dataExtra.empresas}
                                type={"select"}
                                filter={true} filterPlaceholder="Selecciona una Empresa"
                                filterBy="label,value" placeholder="Selecciona una Empresa"
                                onChange={this.setData}
                                error={this.state.errors.EmpresaId}
                                readOnly={(this.props.entityAction === 'show')}
                            />
                            </div>
                     </div>
                    <div className="row">
                        <div className="col">
                            <Field
                                name="tipo_comida_id"
                                        value={this.state.data.tipo_comida_id}
                                        required
                                label="Tipo Comida"
                                options={this.state.dataExtra.tiposComidas}
                                type={"select"}
                               
                                onChange={this.setData}
                                error={this.state.errors.tipo_comida_id}
                                readOnly={(this.props.entityAction === 'show')}
                            />
                        </div>
                         
                        <div className="col">
                            <Field
                                name="AccionId"
                                        value={this.state.data.AccionId}
                                        required
                                label="Acción"
                                options={this.state.dataExtra.acciones}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.AccionId}
                                readOnly={(this.props.entityAction === 'show')}
                            />
                        </div> 
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="hora_desde"
                                        value={this.state.data.hora_desde}
                                        required
                                label="Hora Desde"
                                type={"time"}
                                onChange={this.handleChange}
                                edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                readOnly={(this.props.entityAction === 'show')}
                                error={this.state.errors.hora_desde}
                            />
                        </div>
                        <div className="col">
                            <Field
                                name="hora_hasta"
                                        value={this.state.data.hora_hasta}
                                        required
                                label="Hora Hasta"
                                type={"time"}
                                onChange={this.handleChange}
                                edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                readOnly={(this.props.entityAction === 'show')}
                                error={this.state.errors.hora_hasta}
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

export default wrapForm(TipoAccionEmpresaForm);