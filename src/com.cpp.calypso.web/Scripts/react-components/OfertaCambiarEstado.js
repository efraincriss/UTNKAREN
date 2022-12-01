import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';

import { Button } from 'primereact/components/button/Button';
import { Dialog } from 'primereact/components/dialog/Dialog';
import { Growl } from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';

export default class OfertaCambiarEstado extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            visible: false,
            message: '',
            blocking: false,
            observacion: '',
            Idestado: '',
            IdestadoTemp: '',
            cod_oferta: '',
            cod_proyecto: '',
            estados: [],
            usuario: '',
        }
        this.handleChange = this.handleChange.bind(this);
        this.showForm = this.showForm.bind(this);
        this.onHide = this.onHide.bind(this);
        this.getEstadoSelect = this.getEstadoSelect.bind(this);
        this.getEstados = this.getEstados.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
    }

    componentDidMount() {
        this.getEstados();
    }


    render() {
        return (
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <input type="button" className="btn btn-sm btn-outline-indigo" onClick={this.showForm} value="Cambiar Estado" />


                <Dialog header="Cambiar Estado" visible={this.state.visible} width="450px" modal={true} onHide={this.onHide}>
                    <BlockUi tag="div" blocking={this.state.blocking}>
                        <form onSubmit={this.handleSubmit}>
                            <div className="form-group">
                                <div className="row">
                                    <div className="col-sm-6">
                                        <label htmlFor="cod_proyecto">Proyecto:</label>
                                        <input type="text" name='cod_proyecto' disabled="disabled" value={this.state.cod_proyecto} className="form-control" />
                                    </div>
                                    <div className="col-sm-6">
                                        <label htmlFor="cod_oferta">Oferta:</label>
                                        <input type="text" name='cod_oferta' disabled="disabled" value={this.state.cod_oferta} className="form-control" />
                                    </div>
                                </div>
                            </div>

                            <div className="form-group">
                                <label htmlFor="label">Estado:</label>
                                <select value={this.state.Idestado} required onChange={this.handleChange} className="form-control" name="Idestado">
                                    <option value="0">-- Selecciona un Estado --</option>
                                    {this.getEstadoSelect()}
                                </select>
                            </div>

                            <div className="form-group">
                                <label htmlFor="nombre">Observación</label>
                                <input type="text" name='observacion' value={this.state.observacion} id="observacion" onChange={this.handleChange} className="form-control" />
                            </div>

                            <Button type="submit" label="Guardar" icon="fa fa-fw fa-folder-open" />
                            <Button type="button" label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.onHide} />
                        </form>
                    </BlockUi>
                </Dialog>


            </div>
        )
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    showForm() {
        this.setState({ visible: true })
    }

    onHide(event) {
        this.setState({ visible: false, blockSubmit: false });
    }

    handleSubmit(event) {
        event.preventDefault();
        if (this.state.Idestado == 0) {
            this.setState({ message: 'Selecciona un Estado.' },
                this.showWarn)
        } else {
            this.setState({ blocking: true })
            axios.post("/Proyecto/HistoricosOferta/CreateHistorico/", {
                OfertaId: document.getElementById('content-cambiar-estado').className,
                observaciones: this.state.observacion,
                estado: this.state.Idestado,
                usuario: document.getElementById('username').className,
                IdestadoTemp: this.state.IdestadoTemp
            })
                .then((response) => {
                    this.setState({ blocking: false })
                    this.successMessage("Cambio de Estado Exitoso")
                    this.onHide()
                    setTimeout(window.location.reload(), 10);
                })
                .catch((error) => {
                    console.log(error);
                });
        }
    }

    getEstados() {
        axios.post("/Proyecto/Catalogo/GetCatalogo/" + '1006', {})
            .then((response) => {
                this.setState({
                    estados: response.data,
                    cod_oferta: document.getElementById('cod_oferta').className,
                    cod_proyecto: document.getElementById('cod_proyecto').className,
                    Idestado: document.getElementById('estado_oferta').className,
                    IdestadoTemp: document.getElementById('estado_oferta').className,
                })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    getEstadoSelect() {
        return (
            this.state.estados.map((item) => {
                return (
                    <option value={item.Id}>{item.nombre}</option>
                )
            })

        );
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.message });
    }

    successMessage(msg) {
        this.setState({ message: msg }, this.showSuccess)
    }

    warnMessage(msg) {
        this.setState({ message: msg }, this.showWarn)
    }
}

ReactDOM.render(
    <OfertaCambiarEstado />,
    document.getElementById('content-cambiar-estado')
);