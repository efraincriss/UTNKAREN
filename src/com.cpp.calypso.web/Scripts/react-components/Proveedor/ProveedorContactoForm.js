import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';
 
import moment from 'moment';
 
import http from '../Base/HttpService';
import ActionForm from '../Base/ActionForm';
import config from '../Base/Config';
import Field from '../Base/Field';
import wrapForm from '../Base/WrapForm';

import ProveedorContactoTipoOpcionComidaContainer from './ProveedorContactoTipoOpcionComidaContainer';
import ProveedorInfo from './ProveedorInfo';

class ProveedorContactoForm extends Component {

    constructor(props) {
        super(props);

        this.state = {
            params: {
                proveedorId: 0,
                contratoId: 0,
                entityAction:'create'
            },

            data: this.initData(),
            uploadFile: '',
            dataInfo: {},
            blocking: true,
            loadData: false,
            loading: false,
            errors: {},
            deleteIds: [],
            urlApiBase: '/proveedor/Proveedor/',
            dataExtra: [],
            dataExtraChild: []
        };
        this.handleChange = this.handleChange.bind(this);
        this.setData = this.setData.bind(this);
        this.handleAddedChild = this.handleAddedChild.bind(this);
        this.handleUpdatedChild = this.handleUpdatedChild.bind(this);
        this.handleDeleteChild=this.handleDeleteChild.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleCancel = this.handleCancel.bind(this);
    }

    
    componentWillMount() {

        let params = {
            proveedorId: this.props.getParameterByName('proveedorId'),
            contratoId: this.props.getParameterByName('contratoId'),
            entityAction: this.props.getParameterByName('entityAction')
        };

        console.log(params);
        this.setState({ params: params });
    }

 
    componentDidMount() {
        console.log('ProveedorContactoForm.componentDidMount');
        if (!this.state.loadData && !this.state.loading) {
            //Init
            this.loadData();
        }
    }

    componentDidUpdate(prevProps) {
        console.log('ProveedorContactoForm.componentDidUpdate');

        
    
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
            estado: true,
            estado_nombre: '',
            tipo_opciones_comida: []
        };
        return dataInit;
    }
    

    normalizingData(dataEntity) {
        
        if (dataEntity['fecha_inicio'] !== undefined && dataEntity['fecha_inicio'] !== null && moment(dataEntity['fecha_inicio']).isValid()) {
            dataEntity['fecha_inicio'] = moment(dataEntity['fecha_inicio']).format(config.formatDate);
        } else {
            dataEntity['fecha_inicio'] = '';
        }


        if (dataEntity['fecha_fin'] !== undefined && dataEntity['fecha_fin'] !== null && moment(dataEntity['fecha_alcancce']).isValid()) {
            dataEntity['fecha_fin'] = moment(dataEntity['fecha_fin']).format(config.formatDate);
        } else {
            dataEntity['fecha_fin'] = '';
        }
    }

    getData() {
        let url = '';
        url = `${this.state.urlApiBase}/GetProveedorContactoApi/?proveedorId=${this.state.params.proveedorId}&contratoId=${this.state.params.contratoId}`;
 
        return http.get(url);
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
    
      
    loadData() {

        var self = this;

        self.setState({ blocking: true, loading: true });

        Promise.all([this.getData(), this.getEmpresa(), this.getTipoComida(), this.getOpcionComida()])
            .then(function ([datos, empresas, tiposComidas, opcionescomida]) {
            
                //Fix 
                let dataEntity = datos.data.result.entity;
                self.normalizingData(dataEntity);
                let info = datos.data.result.info;
  
                self.setState({
                    data: dataEntity,
                    dataInfo: info,
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

        if (!this.state.data.codigo || this.state.data.codigo.length <= 1) {
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

        if (this.state.data.plazo_pago === undefined || this.state.data.plazo_pago <= 0) {
            errors.plazo_pago = 'Debe ingresar un valor mayor o igual a Cero';
        }

        if (this.state.data.monto === undefined || this.state.data.monto <= 0) {
            errors.monto = 'Debe ingresar un valor mayor   a Cero';
        }

        if (!this.state.data.objeto || this.state.data.objeto.length <= 1) {
            errors.objeto = 'Descripción es requerido';
        }

         
        if (!this.state.data.tipo_opciones_comida || this.state.data.tipo_opciones_comida.length == 0) {
            errors.tipo_opciones_comida = 'Debe ingresar por lo menos un item de tipo de comida / opciones de Comida';
        }
         

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    handleSubmit(event) {
        event.preventDefault();

        if (!this.isValid()) {
            abp.notify.error("No ha ingresado los campos obligatorios  o existen datos inválidos.", 'Validación');
            return;
        }

        console.log(this.state);

        this.setState({ blocking: true });

        let url = '';
        if (this.state.params.entityAction === 'edit')
            url = `${this.state.urlApiBase}/EditProveedorContactoApi`;
        else
            url = `${this.state.urlApiBase}/CreateProveedorContactoApi`;


        //creating copy of object
        let data = Object.assign({}, this.state.data);
        data.Id = this.state.params.contratoId;                        //updating value
        data.ProveedorId = this.state.params.proveedorId;

        this.normalizingDataSubmit(data);

        const formData = new FormData();
        for (var key in data) {
            if (key === "tipo_opciones_comida" || key === "deleteIds")
                continue;

            if (data[key] !== null)
                formData.append(key, data[key]);
            else
                formData.append(key, '');
        }

        //Add Array convert Json
        formData.append('tipo_opciones_comida_json', JSON.stringify(data.tipo_opciones_comida));
        formData.append('deleteIds_json', JSON.stringify(data.deleteIds));
        console.log(data.tipo_opciones_comida)

        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };

        //TODO: Como enviar archivos, y objetos Complejos.

        http.post(url, formData, config)
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    this.setState({
                        data: this.initData()
                    });

                    abp.notify.success("Proceso guardado exitosamente", "Aviso");

                    this.handleCancel();

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

    normalizingDataSubmit(dataEntity) {

        //Fix. Error boolean convert Enum Estado Contrato
        if (dataEntity.estado)
            dataEntity.estado = 1;
        else
            dataEntity.estado = 0;

        //Add File
        dataEntity.uploadFile = this.state.uploadFile;

        //Add DeleteID
        dataEntity.deleteIds = this.state.deleteIds;
    }

    handleChange(event) {
        const target = event.target;

        if (event.target.files) {

            let files = event.target.files || event.dataTransfer.files;
            if (files.length > 0) {
                let uploadFile = files[0];
                this.setState({
                    uploadFile: uploadFile
                });
            }

        } else {

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
        let dataUpdate = { ... this.state.data };

        if (!dataUpdate.tipo_opciones_comida)
            dataUpdate.tipo_opciones_comida = [];

        let exist = dataUpdate.tipo_opciones_comida.filter(item => item.tipo_comida_id === entity.tipo_comida_id &&
            item.opcion_comida_id === entity.opcion_comida_id).length > 0;

        if (exist) {
            abp.notify.warn("Ya existe un item con el tipo de Comida y Opción de Comida", "Aviso");
            return false;
        } else {
            console.log(entity);
            entity.Id=0;
            dataUpdate.tipo_opciones_comida.push(entity);
            this.setState({ data: dataUpdate });
            console.log(this.state.data);
            abp.notify.success("Ítem agregado exitosamente", "Aviso");
            return true;
        }
    }

    handleUpdatedChild(entity) {
 
        let dataUpdate = { ... this.state.data };
        let index = dataUpdate.tipo_opciones_comida.findIndex(item => item.Id === entity.Id);
        //dataUpdate.tipo_opciones_comida[index] = { ...dataUpdate.tipo_opciones_comida[index], entity };
        dataUpdate.tipo_opciones_comida[index] = { ...entity };

        this.setState({ data: dataUpdate });
        abp.notify.success("Ítem modificado exitosamente", "Aviso");

        return true;
    }

    handleDeleteChild(entity) {

        let dataUpdate = { ... this.state.data };
        dataUpdate.tipo_opciones_comida = dataUpdate.tipo_opciones_comida.filter(item => item.Id !== entity.Id);

        let deleteIdsLocal = [...this.state.deleteIds];
        if (entity.Id > 0)
            deleteIdsLocal.push(entity.Id);


        this.setState({ data: dataUpdate, deleteIds: deleteIdsLocal });
        abp.notify.success("Ítem eliminado exitosamente", "Aviso");

        return true;
    }

    handleCancel() {
        console.log('handleCancel Item: ');

        return (
            window.location.href = `${config.appUrl}/proveedor/Proveedor/Details/${this.state.params.proveedorId}`
        );
    }

    render() {

        let disabled = false;
        if (this.state.params.entityAction === 'show') {
            disabled = true;
        }

        let blocking = this.props.blocking || this.state.blocking;
 
        return (

            <BlockUi tag="div" blocking={blocking}>


                <div className="row justify-content-center">
                    <div className="col-md-8">

                        <div className="row card">
                            <div className="card-body">
                                <ProveedorInfo data={this.state.dataInfo} />
                            </div>
                        </div>

                        <div className="row card">
                            <div className="card-body">

                                <form >

                                    <div className="row">
                                        <div className="col">

                                            <Field
                                                name="codigo"
                                                label="Código"
                                                required
                                                edit={(this.state.params.entityAction === 'create' || this.state.params.entityAction === 'edit')}
                                                readOnly={(this.state.params.entityAction === 'show')}
                                                value={this.state.data.codigo}
                                                onChange={this.handleChange}
                                                error={this.state.errors.codigo}
                                            />
                                             
                                        </div>
                                        <div className="col">

                                            <Field
                                                name="estado"
                                                label="Activo"
                                                labelOption="(Si/No)"
                                                type={"checkbox"}
                                                edit={(this.state.params.entityAction === 'create' || this.state.params.entityAction === 'edit')}
                                                readOnly={(this.state.params.entityAction === 'show')}
                                                value={this.state.data.estado}
                                                onChange={this.handleChange}
                                                error={this.state.errors.estado}
                                            />
                                        </div>
                                    </div>

                                    <div className="row">
                                        <div className="col">


                                            <Field
                                                name="EmpresaId"
                                                required
                                                value={this.state.data.EmpresaId}
                                                label="Empresa"
                                                options={this.state.dataExtra.empresas}
                                                type={"select"}
                                                onChange={this.setData}
                                                error={this.state.errors.EmpresaId}
                                                readOnly={(this.state.params.entityAction === 'show')}

                                                filter={true} filterPlaceholder="Selecciona una Empresa"
                                                filterBy="label,value" placeholder="Selecciona una Empresa"
                                            />

                                             
                                        </div>
                                    </div>

                                    <div className="row">
                                        <div className="col">

                                            <Field
                                                name="objeto"
                                                label="Descripción"
                                                required
                                                edit={(this.state.params.entityAction === 'create' || this.state.params.entityAction === 'edit')}
                                                readOnly={(this.state.params.entityAction === 'show')}
                                                value={this.state.data.objeto}
                                                onChange={this.handleChange}
                                                error={this.state.errors.objeto}
                                            />
                                             
                                        </div>
                                    </div>

                                    <div className="row">
                                        <div className="col">

                                            <Field
                                                name="fecha_inicio"
                                                label="Fecha inicio"
                                                required
                                                type="date"
                                                edit={(this.state.params.entityAction === 'create' || this.state.params.entityAction === 'edit')}
                                                readOnly={(this.state.params.entityAction === 'show')}
                                                value={this.state.data.fecha_inicio}
                                                onChange={this.handleChange}
                                                error={this.state.errors.fecha_inicio}
                                            />

                                             
                                        </div>

                                        <div className="col">
 
                                            <Field
                                                name="fecha_fin"
                                                label="Fecha fin"
                                                required
                                                type="date"
                                                edit={(this.state.params.entityAction === 'create' || this.state.params.entityAction === 'edit')}
                                                readOnly={(this.state.params.entityAction === 'show')}
                                                value={this.state.data.fecha_fin}
                                                onChange={this.handleChange}
                                                error={this.state.errors.fecha_fin}
                                            />
 
                                        </div>

                                    </div>

                                    <div className="row">
                                        <div className="col">

                                            <Field
                                                name="plazo_pago"
                                                label="Plazo de Pago"
                                                required
                                                type="number"
                                                min="0" 
                                                edit={(this.state.params.entityAction === 'create' || this.state.params.entityAction === 'edit')}
                                                readOnly={(this.state.params.entityAction === 'show')}
                                                value={this.state.data.plazo_pago}
                                                onChange={this.handleChange}
                                                error={this.state.errors.plazo_pago}
                                            />
 
                                        </div>

                                        <div className="col">

                                            <Field
                                                name="monto"
                                                label="Monto"
                                                required
                                                type="number"
                                                min="0.0"
                                                step=".01"
                                                edit={(this.state.params.entityAction === 'create' || this.state.params.entityAction === 'edit')}
                                                readOnly={(this.state.params.entityAction === 'show')}
                                                value={this.state.data.monto}
                                                onChange={this.handleChange}
                                                error={this.state.errors.monto}
                                            />
 
                                         
                                        </div>

                                    </div>

                                    <div className="row">

                                        <div className="col">

                                            <Field
                                                name="orden_compra"
                                                label="Orden de Compra"
                                                edit={(this.state.params.entityAction === 'create' || this.state.params.entityAction === 'edit')}
                                                readOnly={(this.state.params.entityAction === 'show')}
                                                value={this.state.data.orden_compra}
                                                onChange={this.handleChange}
                                                error={this.state.errors.orden_compra}
                                                required
                                            />
 

                                        </div>

                                        <div className="col">
                                            <Field
                                                name="uploadFile"
                                                label="Cargar Orden Firmada"
                                                type={"file"}
                                                edit={(this.state.params.entityAction === 'create' || this.state.params.entityAction === 'edit')}
                                                readOnly={(this.state.params.entityAction === 'show')}
                                                onChange={this.handleChange}
                                                error={this.state.errors.uploadFile}
                                            />
                                         </div>
                                    </div>

                                    <div className="row">
                                        <div className="col">

                                            <ProveedorContactoTipoOpcionComidaContainer

                                                data={this.state.data.tipo_opciones_comida}

                                                blocking={this.state.blocking}

                                                padreId={this.state.data.Id}

                                                entityAction={this.state.params.entityAction}

                                                handleUpdatedChild={this.handleUpdatedChild}
                                                handleAddedChild={this.handleAddedChild}
                                                handleDeleteChild={this.handleDeleteChild}
                                                show={this.state.visibleForm}

                                                onRefreshData={this.onRefreshData}

                                                dataExtra={this.state.dataExtraChild}
                                            />

                                            {this.state.errors.tipo_opciones_comida && <div className="alert alert-danger">{this.state.errors.tipo_opciones_comida}</div>}

                                            <hr />
                                        </div>
                                    </div>



                                    {(this.state.params.entityAction === 'create' || this.state.params.entityAction === 'edit') &&
                                        <ActionForm onCancel={this.handleCancel} onSave={this.handleSubmit} />
                                    }

                                    {this.state.params.entityAction === 'show' &&
                                        <ActionForm onAccept={this.handleCancel} acceptActionName={'Regresar'} />
                                    }

                                </form>
                            </div>
                        </div>


                    </div>
                </div>
            </BlockUi>
        );
    }
}

 
// HOC
const Container = wrapForm(ProveedorContactoForm);

ReactDOM.render(
    <Container />,
    document.getElementById('nuc_detail_body_proveedores_contratos')
);