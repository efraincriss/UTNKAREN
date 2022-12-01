import React from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';

import { Dialog } from 'primereact/components/dialog/Dialog';

import config from '../Base/Config';
import http from '../Base/HttpService';

import wrapContainer from '../Base/WrapContainer';
import moment from 'moment';

import DistribucionViandaVerList from './DistribucionViandaVerList'; 
import DistribucionViandaVerDetalleList from './DistribucionViandaVerDetalleList'; 


class DistribucionViandaVerContainer extends React.Component {

    constructor() {
        super();

        this.state = {
            visibleForm: false,
            entityId: 0,
            params: {
                fecha: moment().format(config.formatDate),
                tipoComidaId:0
            },
            data: [],
            dataExtra: {
                total_pedidos:0,
                total_distribucciones: 0,
                total_consumidos: 0,
                total_por_consumir: 0,
                tipoComida: {
                    Id: 0,
                    nombre:''
                }
            },
            dataGroup: [],
            distribucion_id: 0,
            isDirty: false,
            loadData: true,
            total_distribucion:0
        };

        this.onDetailItem = this.onDetailItem.bind(this);
        this.onReturn = this.onReturn.bind(this);
        this.onHide = this.onHide.bind(this);
    }

    componentWillMount() {
        let params = {
            fecha: moment(this.getParameterByName('fecha')).format(config.formatDate),
            tipoComidaId: this.getParameterByName('tipoComidaId')
        };

        console.log(params);

        this.setState({  params: params });

    }
     

    componentDidMount(prevProps) {
        this.setState({ visibleForm: false });
        if (this.state.loadData) { 
            this.GetData();
        }     
    }


    getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

    onReturn() {

        event.preventDefault();
        
        return (
            window.location.href = `${config.appUrl}/proveedor/DistribucionVianda/Edit?fecha=${this.state.params.fecha}&tipoComidaId=${this.state.params.tipoComidaId}`
        );
        
    }

    onDetailItem(entity) {
        console.log(entity);
        //this.GetDetalle(entity);
        console.log(this.state.dataExtra);
        this.setState({ visibleForm: true, distribucion_id: entity.Id});
    }

    isValid() {
        const errors = {};
       
        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    isValidAprobar() {
        const errors = {};
      

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

   
    GetData() {

        this.setState({ blocking: true });

        let url = '';
        url = `/proveedor/distribucionvianda/VerDistribucionApi/?fecha=${this.state.params.fecha}&tipoComidaId=${this.state.params.tipoComidaId}`;
        

        http.get(url, {})
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                   //Fix Date
                    let dataDistribucionesFix = data.result.distribuciones;

                    let total_dist = 0;
                    dataDistribucionesFix.forEach(function (dataEntity) {
                        dataEntity['fecha'] = moment(dataEntity['fecha']).format(config.formatDate);
                        total_dist = total_dist + dataEntity['total_pedido'];
                    });

                    var dataExtra = {
                        total_pedidos: data.result.total_pedidos,
                        total_distribucciones: data.result.total_distribucciones,
                        total_consumidos: data.result.total_consumidos,

                        //TODO: CALCULO... 
                        total_por_consumir: data.result.total_por_consumir,
                        //solicitudes: data.result.solicitudes,
                        //proveedores: data.result.proveedores,
                        tipoComida: data.result.tipoComida
                        //distribuidos: data.result.distribuidos,
                        //distribuciones: data.result.distribuciones
                    };

                    var dataGroup = data.result.distribuidos;

                    this.setState({
                        data: dataDistribucionesFix,
                        dataExtra: dataExtra,
                        dataGroup: dataGroup
                    });
                } else {
                    //TODO: 
                    //Presentar errores... 
                    var message = $.fn.responseAjaxErrorToString(data);
                    abp.notify.error(message, 'Error');
                }

                this.setState({ blocking: false, loadData: false });
            })
            .catch((error) => {
                console.log(error);
            });
    }

    onRefreshData() {
        this.GetData();
    }

    onHide() {
        console.log('onHide ');
        this.setState({ visibleForm: false, entityId: 0 });
    }

    render() {
        let buttons = [{
            label: "Detalle",
            onClick: this.onDetailItem
        }];


        let blocking = this.props.blocking || this.state.blocking;
        let estado_distribuccion = "Registrado";
        if (this.state.data.length > 0) {
            estado_distribuccion = this.state.data[0].estado_nombre;
        }
    
        return (
            <BlockUi tag="div" blocking={blocking}>

               
                <div className="nav justify-content-end">
                    
                    <button className="btn btn-outline-primary" onClick={this.onReturn} > Regresar </button>
                    
                </div>

                <hr />

                <div className="row">
                    <div className="col">
                        <div className="form-group row">
                            <label htmlFor="fecha" className="col-sm-4 col-form-label"><strong>Fecha</strong></label>
                            <div className="col-sm-8">
                                <input name="fecha_solicitud" type="date" value={this.state.params.fecha} readOnly className="form-control-plaintext" />
                            </div>
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group row">
                            <label htmlFor="tipo_comida_nombre" className="col-sm-4 col-form-label"><strong>Tipo Comida</strong></label>
                            <div className="col-sm-8">
                                <input name="tipo_comida_nombre" type="text" value={this.state.dataExtra.tipoComida.nombre} readOnly className="form-control-plaintext" />
                            </div>
                        </div>
                    </div>

                </div>

                <div className="row">
                    <div className="col">
                        <div className="form-group row">
                            <label htmlFor="total_solicitado" className="col-sm-6 col-form-label"><strong>Total Solicitado</strong></label>
                            <div className="col-sm-6">
                                <input name="total_solicitado" type="number" readOnly value="100" className="form-control-plaintext" value={this.state.dataExtra.total_pedidos} />
                            </div>
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group row">
                            <label htmlFor="total_distribuido" className="col-sm-6 col-form-label"><strong>Total Distribuido</strong></label>
                            <div className="col-sm-6">
                                <input name="total_distribuido" type="number" readOnly className="form-control-plaintext" value={this.state.dataExtra.total_distribucciones} />
                            </div>
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group row">
                            <label htmlFor="total_consumido" className="col-sm-6 col-form-label"><strong>Total Consumido</strong></label>
                            <div className="col-sm-6">
                                <input name="total_consumido" type="number" readOnly value="0" className="form-control-plaintext" value={this.state.dataExtra.total_consumidos} />
                            </div>
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group row">
                            <label htmlFor="por_consumir" className="col-sm-6 col-form-label"><strong>Por Consumir</strong></label>
                            <div className="col-sm-6">
                                <input name="por_consumir" type="number" readOnly value="50" className="form-control-plaintext" value={this.state.dataExtra.total_por_consumir} />
                            </div>
                        </div>
                    </div>
                </div>

                <hr />
               
                <div className="row">
                    <div className="col">
                        <DistribucionViandaVerList
                            data={this.state.dataGroup}
                            buttons={buttons}
                        />
                    </div>
                </div>


                <hr />
                {
                    this.state.visibleForm ?
                    <DistribucionViandaVerDetalleList
                    data={this.state.data.filter(sol => sol.ProveedorId === this.state.distribucion_id)}
                    isDirty={this.state.isDirty}
                    visible={this.state.visibleForm.toString()}
                    />
                    :null

                }
       
                
            </BlockUi>
        );
    }
}

// HOC
 
ReactDOM.render(
    <DistribucionViandaVerContainer />,
    document.getElementById('nuc_ver_body_distribucion_transporte')
);


