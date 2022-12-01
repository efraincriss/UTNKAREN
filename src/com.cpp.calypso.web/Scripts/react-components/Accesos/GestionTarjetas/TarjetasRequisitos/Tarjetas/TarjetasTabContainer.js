import React from "react";

import moment from 'moment';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import config from "../../../../Base/Config";
import http from "../../../../Base/HttpService";

import {
    CONTROLLER_TARJETA_ACCESO,
    MODULO_ACCESO,
    FRASE_TARJETA_CREADA,
    FRASE_TARJETA_ANULADA,
    FRASE_TARJETA_ENTREGADA
} from "../../../../Base/Strings";
import dateFormatter from "../../../../Base/DateFormatter";
import { Dialog } from 'primereact-v2/dialog';
import CreateTarjetaForm from "./CreateTarjetaForm";
import AnularTarjetaForm from "./AnularTarjetaForm";
import UploadPdfForm from "../../../ConsultaPublica/UploadPdfForm";
import UploadFileForm from "./UploadFileForm";

export default class TarjetasTabContainer extends React.Component {
    constructor(props) {
        super(props)
        console.log(props)
        this.state = {
            crearDialog: false,
            anularDialog: false,
            uploadDialog: false,
            tarjeta: {},
            formkey: 5,
            tarjetaId: 0,
            viewDialog: false,
            infotarjeta: null,


        }

        this.formRef = React.createRef();
        this.AnularRef = React.createRef();

        this.MostrarViewTarjeta = this.MostrarViewTarjeta.bind(this);
        this.OcultarViewTarjeta = this.OcultarViewTarjeta.bind(this);
      

    }


    render() {
        return (
            <div>
                <div className="row" style={{ marginTop: '1em' }}>
                    <div className="col" align="right">
                        <button className="btn btn-outline-primary" onClick={() => this.mostrarCrearDialog()}>
                            Generar Tarjeta
                        </button>
                    </div>
                </div>

                <hr />

                <div className="row">
                    <div className="col">
                        <BootstrapTable data={this.props.tarjetas} hover={true} pagination={true}>
                            <TableHeaderColumn
                                isKey
                                width={'8%'}
                                dataField='secuencial_format'
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Código
                        </TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='usuario_nombres'
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Responsable Emisión</TableHeaderColumn>
                            <TableHeaderColumn
                                dataField='solicitud_pam'
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Solicitud Pam</TableHeaderColumn>
                            <TableHeaderColumn
                                dataField='fecha_emision'
                                dataFormat={this.dateFormat}
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Fecha Emisión
                        </TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='fecha_vencimiento'
                                dataFormat={this.dateFormat}
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Fecha Vencimiento
                            </TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='observaciones'
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Observaciones
                            </TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='estado_nombre'
                                width={'8%'}
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Estado
                            </TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='entregada_nombre'
                                width={'8%'}
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Entregada?
                            </TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='Id'
                                width={'16%'}
                                dataFormat={this.generarBotones}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}></TableHeaderColumn>
                        </BootstrapTable>

                        <Dialog header="Generación de Tarjeta" visible={this.state.crearDialog} style={{ width: '500px' }} modal={true} onHide={this.ocultarCrearDialog} maximizable>
                            <CreateTarjetaForm
                                colaborador={this.props.colaborador}
                                ref={this.formRef}
                                crearTarjeta={this.crearTarjeta}
                                showSuccess={this.props.showSuccess}
                                showWarn={this.props.showWarn}
                                blockScreen={this.props.blockScreen}
                                unlockScreen={this.props.unlockScreen}

                            />
                        </Dialog>

                        <Dialog header="Anulación de Tarjeta" visible={this.state.anularDialog} style={{ width: '500px' }} modal={true} onHide={this.ocultarAnularDialog} maximizable>
                            <AnularTarjetaForm
                                ref={this.AnularRef}
                                showSuccess={this.props.showSuccess}
                                showWarn={this.props.showWarn}
                                blockScreen={this.props.blockScreen}
                                unlockScreen={this.props.unlockScreen}
                                tarjeta={this.state.tarjeta}
                                anularTarjeta={this.anularTarjeta}
                                key={this.state.formkey}
                            />
                        </Dialog>

                        <Dialog header="Subir Documento" visible={this.state.uploadDialog} style={{ width: '500px' }} modal={true} onHide={this.ocultarUploadDialog} maximizable>
                            <UploadFileForm
                                label="Archivo de Respaldo"
                                tarjetaId={this.state.tarjetaId}
                                showSuccess={this.props.showSuccess}
                                showWarn={this.props.showWarn}
                                blockScreen={this.props.blockScreen}
                                unlockScreen={this.props.unlockScreen}
                                ocultarUploadDialog={this.ocultarUploadDialog}
                                reloadTarjetas={this.props.reloadTarjetas}
                            />
                        </Dialog>
                        <Dialog header="Datos de Tarjeta" visible={this.state.viewDialog} style={{ width: '800px' }} modal={true} onHide={this.OcultarViewTarjeta} maximizable>
                            <div className="card">
                                <div className="card-header border border-info">
                                    <div className="row">
                                        <div className="col-xs-12 col-md-6">
                                            <h6 className="text-gray-700"><b>Tipo Identificación:</b> {this.props.colaborador.TipoIdentificacionNombre ? this.props.colaborador.TipoIdentificacionNombre : ""}</h6>
                                            <h6 className="text-gray-700"><b>Nombres-Apellidos:</b>  {this.props.colaborador.NombresApellidos ? this.props.colaborador.NombresApellidos : ""}</h6>
                                        </div>

                                        <div className="col-xs-12 col-md-6">
                                            <h6 className="text-gray-700"><b>Identificación:</b> {this.props.colaborador.Identificacion ? this.props.colaborador.Identificacion : ""}</h6>

                                        </div>
                                    </div>
                                </div>
                                <div className="card-body border border-info">
                                    <div className="row">
                                        <div className="col-xs-12 col-md-6">
                                            <h6 className="text-gray-700"><b>Código:</b> {this.state.infotarjeta != null ? this.state.infotarjeta.secuencial_format : ""}</h6>
                                            <h6 className="text-gray-700"><b>#Solicitud PAM:</b>  {this.state.infotarjeta != null ? this.state.infotarjeta.solicitud_pam : ""}</h6>
                                            <h6 className="text-gray-700"><b>Fecha Emisión:</b>  {this.state.infotarjeta != null ? moment(this.state.infotarjeta.fecha_emision).format("DD-MM-YYYY") : ""}</h6>
                                            <h6 className="text-gray-700"><b>Entregado:</b> {this.state.infotarjeta != null ? this.state.infotarjeta.entregada_nombre : ""}</h6>
                                        </div>

                                        <div className="col-xs-12 col-md-6">
                                            <h6 className="text-gray-700"><b>Estado:</b> {this.state.infotarjeta != null ? this.state.infotarjeta.estado_nombre : ""}</h6>
                                            <h6 className="text-gray-700"><b>Usuario Emisor:</b> {this.state.infotarjeta != null ? this.state.infotarjeta.usuario_nombres : ""}</h6>
                                            <h6 className="text-gray-700"><b>Fecha Vencimiento:</b>  {this.state.infotarjeta != null ? moment(this.state.infotarjeta.fecha_vencimiento).format("DD-MM-YYYY") : ""}</h6>

                                        </div>
                                    </div>
                                </div>
                                <div className="card-footer border border-info">

                                    <div className="row">

                                        <div className="col-xs-12 col-md-6">
                                            <h6 className="text-gray-700"><b>Observaciones:</b> {this.state.infotarjeta != null ? this.state.infotarjeta.observaciones : ""}</h6>

                                        </div>

                                        <div className="col-xs-12 col-md-6">


                                        </div>
                                    </div>
                                </div>
                            </div>
                        </Dialog>
                    </div>
                </div>
            </div>
        )
    }

    crearTarjeta = entity => {
        this.props.blockScreen();
        let url;
        entity.ColaboradorId = this.props.colaboradorId;
        entity.secuencial = 0;
        url = `/Accesos/TarjetaAcceso/CreateApi`
        http.post(url, entity)
            .then((response) => {
                let data = response.data
                if (data.success === true) {
                    this.props.reloadTarjetas();
                    this.ocultarCrearDialog();
                    this.props.showSuccess(FRASE_TARJETA_CREADA);
                } else if (data.validation !== null && data.validation === false) {
                    this.props.showWarn(data.result.message);
                    this.props.unlockScreen();
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                    this.props.unlockScreen();
                }

            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
    }


    anularTarjeta = formData => {
        this.props.blockScreen();

        let url = '';
        url = `/Accesos/TarjetaAcceso/UpdateApi`

        const configuration = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };

        http.post(url, formData, configuration)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.props.reloadTarjetas();
                    this.ocultarAnularDialog();
                    this.props.showSuccess(FRASE_TARJETA_ANULADA)
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                    this.props.unlockScreen();
                }

            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn(message);
                this.props.unlockScreen(FRASE_ERROR_GLOBAL);
            });
    }

    resetFormData = () => {
        this.formRef.current.resetValues();

    }


    resetAnularFormData = () => {
        this.AnularRef.current.resetValues();
    }


    dateFormat = (cell, row) => {
        return dateFormatter(cell);
    }

    MostrarViewTarjeta(row) {
        this.setState({ viewDialog: true, infotarjeta: row })
        console.log(row);


    }
    OcultarViewTarjeta() {
        this.setState({ viewDialog: false })

    }

    generarBotones = (cell, row) => {
        return (
            <div>
                <button
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Ver Info Tarjeta"
                    className="btn btn-outline-primary"
                    style={{ marginLeft: "0.3em" }}
                    onClick={() => this.MostrarViewTarjeta(row)}>  <i className="fa fa-eye"></i></button>
                {row.estado !== 2 &&
                    <button
                        data-toggle="tooltip"
                        data-placement="top"
                        title="Anular Tarjeta"
                        data-original-title="Anular Tarjeta"
                        className="btn btn-outline-danger"
                        style={{ marginLeft: '0.3em' }}
                        onClick={
                            () => { if (window.confirm('Vas a anular la tarjeta, Estás segur@?')) this.mostrarAnularDialog(row); }

                        }>
                        <i className="fa fa-times-circle"></i>
                    </button>
                }

                {row.DocumentoRespaldoId !== null &&
                    <button
                        data-toggle="tooltip"
                        data-placement="top"
                        title="Descargar Documento"
                        data-original-title="Descargar Documento"
                        className="btn btn-outline-info"
                        style={{ marginLeft: '0.3em' }}
                        onClick={
                            () => this.onDownloadPdf(row.DocumentoRespaldoId)
                        }>
                        <i className="fa fa-cloud-download"></i>
                    </button>
                }

                {row.DocumentoRespaldoId === null &&
                    <button
                        data-toggle="tooltip"
                        data-placement="top"
                        title="Subir"
                        data-original-title="Subir"
                        className="btn btn-outline-indigo"
                        style={{ marginLeft: '0.3em' }}
                        onClick={
                            () => this.mostrarUploadDialog(cell)
                        }>
                        <i className="fa fa-cloud-upload"></i>
                    </button>
                }

                {row.entregada_nombre !== "SI" &&
                    <button
                        data-toggle="tooltip"
                        data-placement="top"
                        title="Entregar Tarjeta"
                        data-original-title="Entregar Tarjeta"
                        className="btn btn-outline-success"
                        style={{ marginLeft: '0.3em' }}
                        onClick={
                            () => { if (window.confirm('Vas a entregar la tarjeta, Estás segur@?')) this.entregarTarjeta(cell); }
                        }>
                        <i className="fa fa-credit-card-alt"></i>
                    </button>
                }
                {row.entregada_nombre == "SI" &&
                    <button

                        data-toggle="tooltip"
                        data-placement="top"
                        title="Quitar Entrega Tarjeta"
                        data-original-title="Quitar Entrega Tarjeta"
                        className="btn btn-outline-danger"
                        style={{ marginLeft: '0.3em' }}
                        onClick={
                            () => { if (window.confirm('Seguro Deseas Quitar la Entrega de la tarjeta, Estás segur@?')) this.entregarTarjeta(cell); }
                        }>
                        <i className="fa fa-hand-scissors-o"></i>
                    </button>
                }
            </div>
        );
    }

    onDownloadPdf = ArchivoId => {
        return (
            window.location = `/Accesos/TarjetaAcceso/Descargar/${ArchivoId}`
        );
    }

    entregarTarjeta = tarjetaId => {
        this.props.blockScreen();

        let url = '';
        url = `/Accesos/TarjetaAcceso/SwitchEntregada/${tarjetaId}`


        http.post(url)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.props.reloadTarjetas();
                    this.props.showSuccess(FRASE_TARJETA_ENTREGADA)
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                    this.props.unlockScreen();
                }
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn(message);
                this.props.unlockScreen(FRASE_ERROR_GLOBAL);
            });
    }

    ocultarCrearDialog = () => {
        this.setState({ crearDialog: false }, this.resetFormData)
    }

    mostrarCrearDialog = () => {
        this.setState({ crearDialog: true })


    }

    ocultarAnularDialog = () => {
        this.setState({ anularDialog: false }, this.resetAnularFormData)
    }

    mostrarAnularDialog = tarjeta => {
        this.setState({ anularDialog: true, tarjeta, formkey: Math.random() })
    }

    ocultarUploadDialog = () => {
        this.setState({ uploadDialog: false, tarjetaId: 0 })
    }

    mostrarUploadDialog = tarjetaId => {
        this.setState({ uploadDialog: true, tarjetaId })
    }
}