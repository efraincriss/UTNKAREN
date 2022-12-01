import React, { Component } from 'react';
import BlockUi from 'react-block-ui';


import ActionForm from '../Base/ActionForm';
import Field from '../Base/Field';
import wrapForm from '../Base/WrapForm';

class ProveedorContactoTipoOpcionComidaForm extends Component {

    constructor(props) {
        super(props);

        this.state = {
            data: this.initData(),
            blocking: true,
            loadDataExtra: false,
            loading: false,
            errors: {},

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
 
        // Typical usage (don't forget to compare props):
        if (this.props.show && (this.props.entityId !== prevProps.entityId ||
            this.props.show !== prevProps.show)) {
            this.loadData();
        }
    }

    initData() {
        let dataInit = {
            Id: Math.floor((Math.random() * 300) + 1) * -1, 
            ContratoId: this.props.padreId,
            opcion_comida_id: 0,
            opcion_comida_nombre: '',
            tipo_comida_id: 0,
            tipo_comida_nombre: '', 
            costo:0 
        };
        return dataInit;
    }
    loadData() {
        console.log('this.props.entityId : ' + this.props.entityId);

        if (this.props.entityId > 0) {

            let dataEntity = { ...this.props.data };
            this.normalizingData(dataEntity);
            this.setState({
                data: dataEntity,
                blocking: false
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

        if (this.props.entityAction === 'create') {
            dataEntity = { ...initData() };
        }
    }

    
    isValid() {
        const errors = {};

        if (this.state.data.tipo_comida_id === undefined || this.state.data.tipo_comida_id <= 0) {
            errors.tipo_comida_id = 'Debe seleccionar un tipo de Comida';
        }

        if (this.state.data.opcion_comida_id === undefined || this.state.data.opcion_comida_id <= 0) {
            errors.opcion_comida_id = 'Debe seleccionar una Opción de Comida';
        }
     

        if (this.state.data.costo === undefined || this.state.data.costo <= 0) {
            errors.costo = 'Debe ingresar un valor mayor a Cero';
        }
 

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    handleSubmit(event) {
        event.preventDefault();

        if (!this.isValid()) {
            return;
        }

       
        this.setState({ blocking: true });
 
        //creating copy of object
        let data = Object.assign({}, this.state.data);
        console.log('dataadded',data)
        this.normalizingDataSubmit(data);

        if (this.props.entityId <= 0) {
            this.props.onAdded(data);
        }
        else {
            this.props.onUpdated(data);
        }

        this.setState({ blocking: false });
    }

    normalizingDataSubmit(dataEntity) {

        //dataEntity.Id = this.props.entityId;                        //updating value
        dataEntity.ContratoId = this.props.padreId;

        //Add Label..
        let tipo_comida = this.props.dataExtra.tiposComidas.filter(item => item.value === dataEntity.tipo_comida_id);
        if (tipo_comida.length === 1) {
            dataEntity.tipo_comida_nombre = tipo_comida[0].label;
        }
        let opcion_comida = this.props.dataExtra.opcionescomida.filter(item => item.value === dataEntity.opcion_comida_id);
        if (tipo_comida.length === 1) {
            dataEntity.opcion_comida_nombre = opcion_comida[0].label;
        }
        dataEntity.hora_inicio= new Date();
        dataEntity.hora_fin=new Date();
        console.log('dataEntity',dataEntity);

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

            <BlockUi tag="div" blocking={blocking}>

                <div >

                    <div className="row">

                        <div className="col">

                            <Field
                                name="tipo_comida_id"
                                required
                                value={this.state.data.tipo_comida_id}
                                label="Tipo Comida"
                                options={this.props.dataExtra.tiposComidas}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.tipo_comida_id}
                                readOnly={(this.props.entityAction === 'show')}
                            />
                             
                        </div>
                        <div className="col">
                            <Field
                                name="opcion_comida_id"
                                required
                                value={this.state.data.opcion_comida_id}
                                label="Opciones de Comida"
                                options={this.props.dataExtra.opcionescomida}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.opcion_comida_id}
                                readOnly={(this.props.entityAction === 'show')}
                            />
                        </div>
                    </div>
                     
                    <div className="row">
                        <div className="col-sm-6 col-md-6">

                            <Field
                                name="costo"
                                label="Costo"
                                required
                                type="number"
                                min="0.0"
                                step=".01"
                                edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                readOnly={(this.props.entityAction === 'show')}
                                value={this.state.data.costo}
                                onChange={this.handleChange}
                                error={this.state.errors.costo}
                            />
                        </div>
 
                    </div>
                     
 
                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                        <ActionForm onCancel={this.props.onHide} onSave={this.handleSubmit} />
                    }

                    {this.props.entityAction === 'show' &&
                        <ActionForm onAccept={this.props.onHide} />
                    }
       
                </div>
            </BlockUi>
        );
    }
}

export default wrapForm(ProveedorContactoTipoOpcionComidaForm);