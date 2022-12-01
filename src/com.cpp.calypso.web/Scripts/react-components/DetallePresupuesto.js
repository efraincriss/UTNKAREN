import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import CabeceraDetallePrespuesto from './Presupuestos/CabeceraDetallePresupuesto';

import { Dialog } from 'primereact/components/dialog/Dialog';
import { Growl } from 'primereact/components/growl/Growl';
import ComputosPresupuestosTable from './Presupuestos/ComputosPresupuestosTable';
import EditarPresupuesto from './Presupuestos/EditarPresupuesto';
import EditarPresupuestoCorreo from './Presupuestos/EditarPresupuestoCorreo';
import { TabView, TabPanel } from "primereact-v2/tabview";
import ArchivosPresupuesto from "./Presupuestos/ArchivosList";
export default class DetallePresupuesto extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: [],
            blocking: true,
            OfertaId: 0,
            Oferta: {},
            visible: false,
            visibleemail: false,
            FormKey: 8498,
        }

        this.successMessage = this.successMessage.bind(this)
        this.warnMessage = this.warnMessage.bind(this);
        this.alertMessage = this.alertMessage.bind(this);
        this.ConsultarComputos = this.ConsultarComputos.bind(this);
        this.OcultarFormulario = this.OcultarFormulario.bind(this);
        this.MostrarFormularioEditar = this.MostrarFormularioEditar.bind(this);
        this.OcultarFormularioEmail = this.OcultarFormularioEmail.bind(this);
        this.MostrarFormularioEmail = this.MostrarFormularioEmail.bind(this);
        this.Loading = this.Loading.bind(this)
        this.CancelarLoading = this.CancelarLoading.bind(this)
        this.ConsultarOferta = this.ConsultarOferta.bind(this)
        this.AprobarPresupuesto = this.AprobarPresupuesto.bind(this)
        this.DesaprobarPresupuesto = this.DesaprobarPresupuesto.bind(this)
    }

    componentDidMount() {
        this.setState({ OfertaId: document.getElementById('content').className }, this.ConsultarComputos)
    }


    render() {
        return (
            <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>

                <CabeceraDetallePrespuesto

                    Oferta={this.state.Oferta}
                    OfertaId={this.state.OfertaId}
                    MostrarFormulario={this.MostrarFormularioEditar}
                    MostrarFormularioEmail={this.MostrarFormularioEmail}
                    DesaprobarPresupuesto={this.DesaprobarPresupuesto}
                    AprobarPresupuesto={this.AprobarPresupuesto}
                />
                <TabView className="tabview-custom">
                    <TabPanel header="Datos Presupuesto">
                        <ComputosPresupuestosTable
                            data={this.state.data}
                            Oferta={this.state.Oferta}
                            OfertaId={this.state.OfertaId}
                            ConsultarComputos={this.ConsultarComputos}
                            Loading={this.Loading}
                            ConsultarOferta={this.ConsultarOferta}
                            CancelarLoading={this.CancelarLoading}
                        />
                    </TabPanel>
                    <TabPanel header="Archivos Presupuesto">
                        {this.state.OfertaId != null && this.state.OfertaId > 0 && (
                            <div>
                                <ArchivosPresupuesto
                                     Oferta={this.state.Oferta}
                                     OfertaId={this.state.OfertaId}
                                    showSuccess={this.successMessage}
                                    showWarning={this.alertMessage}
                                    showWarn={this.warnMessage}
                                    blockScreen={this.Loading}
                                    unlockScreen={this.CancelarLoading}
                                />
                            </div>
                        )}
                    </TabPanel>
                </TabView>



                <Dialog header="Editar Presupuesto" visible={this.state.visible} width="800px" modal={true} onHide={this.OcultarFormulario}>
                    <EditarPresupuesto
                        Oferta={this.state.Oferta}
                        key={this.state.FormKey}
                        OcultarFormulario={this.OcultarFormulario}
                        successMessage={this.successMessage}
                        warnMessage={this.warnMessage}
                        alertMessage={this.alertMessage}
                        ConsultarOferta={this.ConsultarOferta}
                        Loading={this.Loading}
                        CancelarLoading={this.CancelarLoading}
                        OcultarFormulario={this.OcultarFormulario} />
                </Dialog>
                <Dialog header="Confirmación Correo" visible={this.state.visibleemail} width="800px" modal={true} onHide={this.OcultarFormularioEmail}>
                    <EditarPresupuestoCorreo
                        Oferta={this.state.Oferta}
                        key={this.state.FormKey}
                        OcultarFormulario={this.OcultarFormularioEmail}
                        successMessage={this.successMessage}
                        warnMessage={this.warnMessage}
                        alertMessage={this.alertMessage}
                        ConsultarOferta={this.ConsultarOferta}
                        Loading={this.Loading}
                        CancelarLoading={this.CancelarLoading}
                        OcultarFormulario={this.OcultarFormularioEmail} />
                </Dialog>
            </BlockUi>
        )
    }

    ConsultarOferta() {
        axios.post("/proyecto/OfertaPresupuesto/DetailsPresupuestoApi/" + this.state.OfertaId, {})
            .then((response) => {
                console.log(response.data);
                this.setState({ Oferta: response.data, blocking: false, FormKey: Math.random() })
            })
            .catch((error) => {
                console.log(error);
                this.setState({ blocking: false })
                this.warnMessage("Error al consultar el Presupuesto")
            });
    }

    ConsultarComputos() {
        axios.get("/proyecto/ComputoPresupuesto/ComputosPresupuesto/" + this.state.OfertaId, {})
            .then((response) => {
                console.log(response.data);
                this.setState({ data: response.data })
                this.ConsultarOferta();
            })
            .catch((error) => {
                console.log(error);
                this.warnMessage("Error al consultar los computos")
            });


    }




    AprobarPresupuesto() {
        this.setState({ blocking: true })
        axios.post("/proyecto/OfertaPresupuesto/AprobarPresupuesto/" + this.state.OfertaId, {})
            .then((response) => {

                if (response.data == "NO_GENERADO") {

                    this.setState({ blocking: false })
                    this.warnMessage("Debe Generar el Presupuesto")
                } else {

                    this.setState({ blocking: false })
                    this.successMessage("Presupuesto Aprobado")
                    this.ConsultarOferta();

                }


            })
            .catch((error) => {
                console.log(error);
                this.setState({ blocking: false })
                this.warnMessage("Error Aprobar el Presupuesto")
            });
    }

    DesaprobarPresupuesto() {
        this.setState({ blocking: true })
        axios.post("/proyecto/OfertaPresupuesto/DesaprobarPresupuesto/" + this.state.OfertaId, {})
            .then((response) => {
                this.setState({ blocking: false })
                this.successMessage("Presupuesto Desaprobado")
                this.ConsultarOferta();
            })
            .catch((error) => {
                console.log(error);
                this.setState({ blocking: false })
                this.warnMessage("Error Desaprobar el Presupuesto")
            });
    }





    Redireccionar(accion, id) {
        if (accion === "Presupuesto") {
            window.location.href = "/Proyecto/Presupuesto/Details/" + id;
        }
    }

    OcultarFormulario() {
        this.setState({ visible: false })
    }

    MostrarFormularioEditar() {
        this.setState({ visible: true })
    }
    MostrarFormularioEmail() {
        this.setState({ visibleemail: true })
    }
    OcultarFormularioEmail() {
        this.setState({ visibleemail: false })
    }

    Loading() {
        this.setState({ blocking: true })
    }

    CancelarLoading() {
        this.setState({ blocking: false })
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: '', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: '', detail: this.state.message });
    }

    showAlert() {
        this.growl.show({ severity: 'warn', summary: '', detail: this.state.message });
    }

    successMessage(msg) {
        this.setState({ message: msg }, this.showSuccess)
    }

    warnMessage(msg) {
        this.setState({ message: msg }, this.showWarn)
    }

    alertMessage(msg) {
        this.setState({ message: msg }, this.showAlert)
    }

}

ReactDOM.render(
    <DetallePresupuesto />,
    document.getElementById('content')
);