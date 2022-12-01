import React from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';
 
import { Dialog } from 'primereact/components/dialog/Dialog';

import config from '../Base/Config';
import http from '../Base/HttpService';
import wrapContainer from '../Base/WrapContainer';
import moment from 'moment';

import JustificacionViandasList from './JustificacionViandasList';
import JustificacionViandasForm from './JustificacionViandasForm';
 
class JustificacionViandasContainer extends React.Component {

    constructor() {
        super();

        this.state = {
            params: {
                fecha: moment().format(config.formatDate)
            },
            dataEdit: {},
            errors: {},
            visibleForm: false,
            blocking: false,
            blockingForm: false,
            entityAction: 'create',
            entityId: 0,
            urlApiBase: '/proveedor/justificarvianda/'
        };

        this.onEditItem = this.onEditItem.bind(this);
        this.onHide = this.onHide.bind(this);
        this.handleChangeDate = this.handleChangeDate.bind(this);
        this.handleChangeEdit = this.handleChangeEdit.bind(this);
        this.handleSaveEdit = this.handleSaveEdit.bind(this);
    }

     

    onEditItem(entity) {

        this.setState({ blocking: true });

        let url = '';
        url = `${this.state.urlApiBase}/CreateNewApi/?solicitudId=${entity.Id}`;


        http.get(url, {})
            .then((response) => {

                let data = response.data;

                if (data.success === true) {
 
                    let dataEntity = data.result;
                    this.normalizingData(dataEntity);
 
                    this.setState({
                        dataEdit: dataEntity 
                    });


                    this.setState({ blocking: false, errors: {}, entityId: dataEntity.Id, visibleForm: true });

                } else {
                    //TODO: 
                    //Presentar errores... 
                    var message = $.fn.responseAjaxErrorToString(data);
                    abp.notify.error(message, 'Error');

                    this.setState({ blocking: false, errors: { message}, entityId: 0, visibleForm: false });

                }
 
            })
            .catch((error) => {
                 abp.notify.error(error, 'Error');

                this.setState({ blockingForm: false, errors: { error }, visibleForm: false, dataEdit: {} });
                console.log(error);
            });
    }
      

    normalizingData(dataEntity) {
        
    }

    handleSaveEdit() {
        console.log(this.state);

        this.setState({ blockingForm: true });

        let url = '';
        if (this.props.entityAction === 'edit')
            url = `${this.state.urlApiBase}/EditApi`;
        else
            url = `${this.state.urlApiBase}/CreateApi`;


        //creating copy of object
        let data = { ... this.state.dataEdit };
       

        http.post(url, data)
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    this.setState({ visibleForm: false,  entityId: 0 });

                    abp.notify.success("Proceso guardado exitosamente", "Aviso");

                    var newParams = {
                        fecha: `${moment(this.state.params.fecha).format(config.formatDate)}`
                    };
 
                    this.props.onRefreshData(newParams);
          

                } else {
                    //TODO: 
                    //Presentar errores... 
                    var message = $.fn.responseAjaxErrorToString(data);
                    abp.notify.error(message, 'Error');
                }


                this.setState({ blockingForm: false });

            })
            .catch((error) => {
                console.log(error);

                this.setState({ blockingForm: false });
            });
    }

    onHide() {
        console.log('onHide ');
        this.setState({ visibleForm: false, entityId: 0 });
    }

    handleChangeDate(event) {
        const target = event.target;
        const value = target.type === "checkbox" ? target.checked : target.value;
        const name = target.name;

          
        if (moment(value).isValid()) {

            const updatedData = {
                ...this.state.params
            };

            updatedData[name] = value;
            this.setState({
                params: updatedData
            });

            var newParams = {
                fecha: `${moment(value).format(config.formatDate)}`
            };

            this.props.onRefreshData(newParams);
            
        }           
    }

    
    handleChangeEdit(event) {
        const target = event.target;
        const value = target.type === "checkbox" ? target.checked : target.value;
        const name = target.name;

        const updatedData = {
            ...this.state.dataEdit
        };

        updatedData[name] = value;


        this.setState({
            dataEdit: updatedData
        });
    }

    render() {
        let usuario = (this.props.data && this.props.data.nombreUsuario) ? this.props.data.nombreUsuario : "";
        let blocking = this.props.blocking || this.state.blocking;
        return (
            <BlockUi tag="div" blocking={blocking}>

                <div className="row">
                    <div className="col-sm-4">
                        <div className="form-group row">
                            <label htmlFor="fecha" className="col-sm-3 col-form-label">Fecha</label>
                            <div className="col-sm-9">
                                <input type="date" id="fecha" className="form-control" value={this.state.params.fecha} onChange={this.handleChangeDate} name="fecha" />
                                {this.state.errors.fecha && <div className="alert alert-danger">{this.state.errors.fecha}</div>}
                            </div>
                        </div>
                    </div>
                    <div className="col-sm-6">
                        <div className="form-group row">
                            <label htmlFor="usuario" className="col-sm-3 col-form-label">Solicitante</label>
                            <div className="col-sm-9">
                                <input type="text" id="usuario" readOnly className="form-control-plaintext" value={usuario} name="usuario" />
                            </div>
                        </div>
                    </div>
                </div>
                 
                <hr />
            
                <div>
                    <JustificacionViandasList
                        data={this.props.data.lista}
                        onEditAction={this.onEditItem}
                        editActionName="Justificar" 
                    />
                </div>

                <Dialog header="Justificar pedidos faltantes" visible={this.state.visibleForm} width="680px" modal minY={70} onHide={this.onHide} >
 
                    <JustificacionViandasForm
                        entityId={this.state.entityId}
                        entityAction={this.state.entityAction}
                        handleChange={this.handleChangeEdit}
                        onSave={this.handleSaveEdit}
                        show={this.state.visibleForm}
                        onHide={this.onHide}
                        data={this.state.dataEdit}
                        blocking={this.state.blockingForm}
                    />

                </Dialog>


            </BlockUi>
        );
    }
}

 

// HOC
const Container = wrapContainer(JustificacionViandasContainer,
    `/proveedor/justificarvianda/GetSolicitudDiariaApi/`,
    {
        fecha: `${moment().format(config.formatDate)}`
    });

ReactDOM.render(
    <Container />,
    document.getElementById('nuc_tree_body_justificacion_viandas')
);

