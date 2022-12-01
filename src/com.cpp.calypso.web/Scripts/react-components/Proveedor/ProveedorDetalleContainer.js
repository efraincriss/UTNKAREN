import React from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';
import { TabView, TabPanel } from 'primereact/components/tabview/TabView';

import config from '../Base/Config';
import http from '../Base/HttpService';
import Field from "../Base/Field-v2";
import ProveedorInfo from './ProveedorInfo';
import ProveedorContactoContainer from './ProveedorContactoContainer';
import ProveedorNovedadContainer from './ProveedorNovedadContainer';
import ProveedorServicioContainer from './ProveedorServicioContainer';
import ProveedorZonaContainer from './ProveedorZonaContainer';
import ProveedorRequerimientoContainer from './ProveedorRequerimientoContainer';
import { Dialog } from 'primereact/components/dialog/Dialog';
import { Button } from 'primereact/components/button/Button';
class ProveedorDetalleContainer extends React.Component {

    constructor() {
        super();
        this.state = {
            data: [],
            padreId: 0,
            blocking: true,
            urlApiBase: '/proveedor/Proveedor/',
          
            viewenable: false,
            pass: "",
            viewdisable: false
        };

        this.GetData = this.GetData.bind(this);
        this.onRefreshData = this.onRefreshData.bind(this);
        this.onReturn = this.onReturn.bind(this);
        this.onActivar = this.onActivar.bind(this);
        this.onDesactivar = this.onDesactivar.bind(this);
        this.onDownload = this.onDownload.bind(this);
    }


    componentWillMount() {
        //TODO: Recuparar el ID, desde el path
        //console.log(this.props.match.params);

        let url = window.location.href;
        let padreId = url.substr(url.lastIndexOf('/') + 1);
        this.setState({ padreId: padreId });
    }

    componentDidMount() {

        this.GetData();

    }

    onHideview = () => {

        this.setState({ viewenable: false, viewdisable: false,pass:""});
    }
    viewenable = () => {

        this.setState({ viewenable: true });
    }
    viewdisable = () => {

        this.setState({ viewdisable: true });
    }

    GetData() {

        let url = '';
        url = `${this.state.urlApiBase}/GetProveedorDetalleApi/${this.state.padreId}`;


        http.get(url, {})
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    this.setState({ data: data.result });

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
            });
    }

    onRefreshData() {
        this.GetData();
    }

    onReturn(event) {

        event.preventDefault();

        return (
            window.location.href = `${config.appUrl}/Proveedor/Proveedor/`
        );
    }

    onActivar() {

        if (this.state.pass === "") {
            abp.notify.error("Debe ingresar el código de Seguridad", 'Error');

        } else {
            console.log('onActivar ');

            var self = this;
            self.setState({ blocking: true });

            let url = '';
            url = `${self.state.urlApiBase}/EnableDisableApi`;


            let data = {
                id: self.state.padreId,
                opcion: true,
                pass: this.state.pass
            };


            http.post(url, data)
                .then((response) => {

                    let data = response.data;

                    if (data.result === true) {

                        abp.notify.success("Proceso guardado exitosamente", "Aviso");
                        this.onHideview();
                        var newParams = {
                        };

                        self.onRefreshData(newParams);

                    } else {
                        abp.notify.error("El código de seguridad es incorrecto", 'Error');
                        //TODO: 
                        //Presentar errores... 
                        //var message = $.fn.responseAjaxErrorToString(data);
                        // abp.notify.error(message, 'Error');
                    }


                    self.setState({ blocking: false });

                })
                .catch((error) => {
                    console.log(error);

                    self.setState({ blocking: false });
                });
        }

    }

    onDesactivar() {
        if (this.state.pass === "") {
            abp.notify.error("Debe ingresar el código de Seguridad", 'Error');

        } else {
            console.log('onDesactivar ');

            var self = this;
            self.setState({ blocking: true });

            let url = '';
            url = `${self.state.urlApiBase}/EnableDisableApi`;


            let data = {
                id: self.state.padreId,
                opcion: false,
                pass: this.state.pass
            };


            http.post(url, data)
                .then((response) => {

                    let data = response.data;

                    if (data.result === true) {

                        abp.notify.success("Proceso guardado exitosamente", "Aviso");
                        this.onHideview();
                        var newParams = {
                        };

                        self.onRefreshData(newParams);

                    } else {
                        abp.notify.error("El código de seguridad es incorrecto", 'Error');
                        //TODO: 
                        //Presentar errores... 
                        //var message = $.fn.responseAjaxErrorToString(data);
                       // abp.notify.error(message, 'Error');
                    }


                    self.setState({ blocking: false });

                })
                .catch((error) => {
                    console.log(error);

                    self.setState({ blocking: false });
                });
        }


    }
    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }
    onDownload() {

        return (
            window.location = `/Proveedor/Proveedor/Descargar/${this.state.data.documentacion_id}`
        );
    }

    render() {

        let blocking = this.props.blocking || this.state.blocking;

        return (
            <BlockUi tag="div" blocking={blocking}>
                <div className="row">
                    <div className="col">
                        <div className="col nav justify-content-end">
                            <button className="btn btn-outline-primary" onClick={this.onReturn} > Regresar </button>
                            <button className="btn btn-outline-primary" onClick={this.viewenable} > Activar  </button>
                            <button className="btn btn-outline-danger" onClick={this.viewdisable} > Dar Baja  </button>
                            {this.state.data && this.state.data.documentacion_id && this.state.data.documentacion_id > 0 && <button className="btn btn-outline-primary" onClick={this.onDownload} > Descargar  </button>}
                        </div>
                    </div>
                </div>
                <hr />
                <div style={{ marginTop: "-30px" }}>
                    <div className="card-body">
                        <div className="card">
                            <div className="card-body" style={{ paddingBottom: "5px" }}>
                                <ProveedorInfo data={this.state.data} />
                            </div>
                        </div>

                        <div >

                            <TabView>

                                <TabPanel header="Contratos">
                                    <ProveedorContactoContainer
                                        tieneServicioHospedaje={this.state.data.tiene_servicio_hospedaje}
                                        tieneServicioLavanderia={this.state.data.tiene_servicio_lavanderia}
                                        data={this.state.data.contratos}
                                        blocking={this.state.blocking}
                                        padreId={this.state.padreId}
                                        onRefreshData={this.onRefreshData}
                                    />
                                </TabPanel>

                                <TabPanel header="Servicios">

                                    <ProveedorServicioContainer
                                        data={this.state.data.servicios}
                                        blocking={this.state.blocking}
                                        padreId={this.state.padreId}
                                        padreData={this.state.data}
                                        onRefreshData={this.onRefreshData}
                                    />

                                </TabPanel>
                                <TabPanel header="Novedades">
                                    <ProveedorNovedadContainer
                                        data={this.state.data.novedades}
                                        blocking={this.state.blocking}
                                        padreId={this.state.padreId}
                                        onRefreshData={this.onRefreshData}
                                    />
                                </TabPanel>
                                <TabPanel header="Zonas Cobertura">

                                    <ProveedorZonaContainer
                                        data={this.state.data.zonas}
                                        blocking={this.state.blocking}
                                        padreId={this.state.padreId}
                                        onRefreshData={this.onRefreshData}
                                    />

                                </TabPanel>

                                <TabPanel header="Requerimientos">

                                    <ProveedorRequerimientoContainer
                                        data={this.state.data.requisitos}
                                        blocking={this.state.blocking}
                                        padreId={this.state.padreId}
                                        onRefreshData={this.onRefreshData}
                                    />


                                </TabPanel>
                            </TabView>
                            <Dialog header="Activar Proveedor" visible={this.state.viewenable} style={{ width: '50vw' }} modal onHide={this.onHideview} >

                                <div>Está seguro de activar al Proveedor. ¿Desea continuar?</div>
                                <br />
                                <Field
                                    name="pass"
                                    label="Código de Seguridad"
                                    required
                                    edit={true}
                                    value={this.state.pass}
                                    onChange={this.handleChange}


                                />
                                <br />
                                <div align="right">
                                    <Button label="SI" icon="pi pi-check" onClick={this.onActivar} />{" "}
                                    <Button label="NO" icon="pi pi-times" className="p-button-secondary" onClick={this.onHideview} />
                                </div>
                            </Dialog>
                            <Dialog header="Inactivar Proveedor" visible={this.state.viewdisable} style={{ width: '50vw' }} modal onHide={this.onHideview} >

                                <div>Está seguro de dar de baja al Proveedor. ¿Desea continuar?</div>
                                <br />
                                <Field
                                    name="pass"
                                    label="Código de Seguridad"
                                    required
                                    edit={true}
                                    value={this.state.pass}
                                    onChange={this.handleChange}


                                /> <br />
                                <div align="right">
                                    <Button label="SI" icon="pi pi-check" onClick={this.onDesactivar} />{" "}
                                    <Button label="NO" icon="pi pi-times" className="p-button-secondary" onClick={this.onHideview} />
                                </div>
                            </Dialog>
                        </div>

                    </div>
                </div>
            </BlockUi>
        );
    }

}

ReactDOM.render(
    <ProveedorDetalleContainer />,
    document.getElementById('nuc_detail_body_proveedores')
);

