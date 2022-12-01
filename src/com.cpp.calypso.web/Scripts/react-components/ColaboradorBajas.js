import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { ProgressBar } from 'primereact/components/progressbar/ProgressBar';
import { Dialog } from 'primereact/components/dialog/Dialog';
import { Button } from 'primereact/components/button/Button';
import PagoLiquidacion from './Colaboradores/Bajas/PagoLiquidacion';
import ConfirmarBajas from './Colaboradores/Bajas/ConfirmarBajas';
import EnvioIESS from './Colaboradores/Bajas/EnvioIESS';
import EditarBaja from './Colaboradores/Bajas/EditarBaja';
import BlockUi from 'react-block-ui';
import moment from 'moment';
import UploadPdfForm from "./Colaboradores/Bajas/UploadPdfForm";
import { Checkbox } from 'primereact-v2/checkbox';
import { Messages } from 'primereact-v2/messages';

export default class ColaboradorBajas extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            colaboradores: [],
            baja_id: '',
            colaborador_id: '',
            tipo_identificacion: '',
            nro_identificacion: '',
            nombres: '',
            estado: '',
            nombres_apellidos: '',
            nro_legajo: '',
            id_sap: '',
            motivo_baja: '',
            fecha_baja: '',
            visible_certificado: false,
            visible_pago: false,
            key_form: 23423,
            visible_desestimacion: false,
            motivo_desestimacion: '',
            errorDesestimacion: '',
            loading: false,
            tiposEstados: [],
            nro_identificacionConsulta: '',
            nombresConsulta: '',
            visible_envio_Manual: false,
            bajasEnvio: [],
            filas: [],
            visible_confirmacion: false,
            visible_iess: false,
            visible_edicion: false,
            fecha_ingreso: '',
            loading_envioManual: false,

            //colaboradores form certificado

            fecha_ingreso_cert: moment(new Date()).format("YYYY-MM-DD"),
            fecha_baja_cert: moment(new Date()).format("YYYY-MM-DD"),
            colaboradoId: 0,
            colaborador: null,
            block: true,
            link: "#",
            value1: 0,
            archivo: {},

            /*ES: Detalle Baje*/
            detalle_baja: '',
            MotivoBajaId: 0,
            CatalogoBajas: [],
            motivo_edicion: '',

            errors: {},

            /*ES: Upload and Download FIle*/
            uploadFile: '',
            uploadPDF: false,
            BajaId: 0,

            checkedbaja: false,
            checkedliquidacion: false,
            ok: true,

            downloadPDF: false,
            ArchivoBajaId: 0,
            ArchivoLiquidacionId: 0,
            baja: null,

        }

        this.handleChange = this.handleChange.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);

        this.limpiarEstados = this.limpiarEstados.bind(this);
        this.ConsultaEstados = this.ConsultaEstados.bind(this);
        this.getFormSelectEstado = this.getFormSelectEstado.bind(this);

        this.childPago = React.createRef();
        this.childConfirmacion = React.createRef();
        this.childIESS = React.createRef();
        this.generateButton = this.generateButton.bind(this);

        this.onHideConfirmacionBaja = this.onHideConfirmacionBaja.bind(this);
        this.showFormConfirmacionBaja = this.showFormConfirmacionBaja.bind(this);
        this.onHideEnvioManual = this.onHideEnvioManual.bind(this);
        this.showFormEnvioManual = this.showFormEnvioManual.bind(this);
        this.onHideDesestimacion = this.onHideDesestimacion.bind(this);
        this.showFormDesestimacion = this.showFormDesestimacion.bind(this);
        this.onHidePago = this.onHidePago.bind(this);
        this.showFormPago = this.showFormPago.bind(this);
        this.onHideCertificado = this.onHideCertificado.bind(this);
        this.showFormCertificado = this.showFormCertificado.bind(this);
        this.onHideArchivoIESS = this.onHideArchivoIESS.bind(this);
        this.showFormArchivoIESS = this.showFormArchivoIESS.bind(this);
        this.onHideEdicion = this.onHideEdicion.bind(this);
        this.showFormEdicion = this.showFormEdicion.bind(this);

        this.LoadColaborador = this.LoadColaborador.bind(this);
        this.LoadPago = this.LoadPago.bind(this);
        this.LoadDesestimacion = this.LoadDesestimacion.bind(this);
        this.GuardarDesestimacion = this.GuardarDesestimacion.bind(this);

        this.GetColaboradores = this.GetColaboradores.bind(this);
        this.GetColaboradorInformation = this.GetColaboradorInformation.bind(this);
        this.Certificado = this.Certificado.bind(this);

        this.GenerarArchivoBajas = this.GenerarArchivoBajas.bind(this);
        this.EnvioManual = this.EnvioManual.bind(this);
        this.DatosEnvio = this.DatosEnvio.bind(this);
        this.onSelectAll = this.onSelectAll.bind(this);
        this.GetColaboradoresEnvioSap = this.GetColaboradoresEnvioSap.bind(this);
    }

    componentDidMount() {
        this.GetColaboradores();
        this.ConsultaEstados();
    }
    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }
    Certificado() {

        this.setState({ block: true })

        if (this.state.fecha_baja_cert < this.state.fecha_ingreso_cert) {
            abp.notify.error("La Fecha Hasta debe ser Mayor a la Fecha Desde", 'Error');
            this.setState({ link: "#" })
            this.setState({ block: false })
        } else if (this.state.colaborador.fecha_ingreso != null &&
            this.state.fecha_ingreso_cert < moment(this.state.colaborador.fecha_ingreso).format("YYYY-MM-DD")) {
            abp.notify.error("La Fecha Desde no puede ser menor a la Fecha de Ingreso del Colaborador", 'Error');
            this.setState({ link: "#" })
            this.setState({ block: false })
        }
        else if (this.state.colaborador.fecha_baja != null &&
            this.state.fecha_baja_cert > moment(this.state.colaborador.fecha_baja).format("YYYY-MM-DD")) {
            abp.notify.error("La Fecha Hasta no puede ser mayor a la fecha de Baja", 'Error');
            this.setState({ link: "#" })
            this.setState({ block: false })
        }
        else {
            this.interval = setInterval(() => {
                let val = this.state.value1;
                val += Math.floor(Math.random() * 130) + 1;

                if (val >= 100) {
                    val = 100;

                    clearInterval(this.interval);

                    this.setState({ value1: 0 })
                    this.onHideCertificado()
                }

                this.setState({
                    value1: val
                });
            }, 2000);
            this.setState({ link: "/RRHH/Colaboradores/GetCertificadoApi/" + this.state.colaboradoId + "?a=" + this.state.fecha_ingreso_cert + "&b=" + this.state.fecha_baja_cert, block: false })

        }

    }

    render() {
        const footer = (
            <div>
                <Button label="Enviar" icon="pi pi-check" onClick={() => { if (window.confirm('Está seguro de enviar los registros seleccionados?')) this.EnvioManual(); }} />
                <Button label="Cancelar" icon="pi pi-times" onClick={() => this.onHideEnvioManual()} className="p-button-secondary" />
            </div>
        );
        const selectRowProp = {
            mode: 'checkbox',
            clickToSelect: true,
            onSelect: this.DatosEnvio,
            onSelectAll: this.onSelectAll
        };
        const footerCertificado = (

            <div>
                <a href={this.state.link}
                    className="btn btn-primary"
                    role="button"
                    onClick={() => this.Certificado()}
                    style={{ marginRight: '0.3em', color: 'white' }}
                > Generar Certificado</a>
                <button icon="pi pi-times" onClick={() => this.onHideCertificado()} className="btn btn-primary" style={{ marginRight: '0.3em' }}>Cancelar</button>
            </div>
        );
        const footerArchivos = (

            <div>
                <a href={this.state.link}
                    className="btn btn-primary"
                    role="button"
                    onClick={() => this.onDownload()}
                    style={{ marginRight: '0.3em', color: 'white' }}
                > Descargar</a>
                <button icon="pi pi-times" onClick={() => this.ocultarDownload()} className="btn btn-primary" style={{ marginRight: '0.3em' }}>Cancelar</button>
            </div>
        );
        const footerPago = (
            <div>
                <Button label="Aceptar" icon="pi pi-check" onClick={() => this.childPago.current.Guardar()} />
                <Button label="Cancelar" icon="pi pi-times" onClick={() => this.onHidePago()} className="p-button-secondary" />
            </div>
        );
        const options = {
            withoutNoDataText: true
        };
        const footerDesestimacion = (
            <div>
                <Button label="Aceptar" icon="pi pi-check" onClick={() => this.GuardarDesestimacion()} />
                <Button label="Cancelar" icon="pi pi-times" onClick={() => this.onHideDesestimacion()} className="p-button-secondary" />
            </div>
        );
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div className="row" >
                    <div className="col" style={{ marginTop: '2.2%' }}>
                        <button type="button" onClick={() => this.showFormEnvioManual()} style={{ marginLeft: '0.2em' }} className="btn btn-primary pull-right">Envío Manual a SAP</button>
                        <button type="button" onClick={() => this.showFormConfirmacionBaja()} style={{ marginLeft: '0.2em' }} className="btn btn-primary pull-right">Validación Información SAP</button>
                        <button type="button" onClick={() => this.showFormArchivoIESS()} style={{ marginLeft: '0.2em' }} className="btn btn-primary pull-right">Generar Archivo IESS</button>
                        <button type="button" onClick={() => this.generarCertificadosDeCapacitacionesDeBajas()} style={{ marginLeft: '0.2em' }} className="btn btn-primary pull-right">Generar Certificado De Capacitaciones</button>
                    </div>
                </div>
                <br />
                <div className="row" >
                    <div className="col-3">
                        <div className="form-group">
                            <label htmlFor="text"><b>No. Identificación:</b></label>
                            <input type="text" id="nro_identificacionConsulta" name="nro_identificacionConsulta" value={this.state.nro_identificacionConsulta} className="form-control" onChange={this.handleChange} />
                        </div>
                    </div>
                    <div className="col-4">
                        <div className="form-group">
                            <label htmlFor="text"><b>Apellidos Nombres:</b></label>
                            <input type="text" minLength="3" id="nombresConsulta" className="form-control" value={this.state.nombresConsulta} name="nombresConsulta" onChange={this.handleChange} />
                        </div>
                    </div>
                    <div className="col-3">
                        <div className="form-group">
                            <label htmlFor="text"><b>Estado:</b></label>
                            <select value={this.state.estado} onChange={this.handleChange} className="form-control" name="estado">
                                <option value="">Seleccione...</option>
                                {this.getFormSelectEstado()}
                            </select>
                        </div>
                    </div>
                    <div className="col-2" style={{ marginTop: '2.2%' }}>
                        <button type="button" onClick={() => this.GetColaboradoreBuscar()} style={{ marginLeft: '0.2em' }} className="btn btn-outline-primary">Buscar</button>
                        <button type="button" onClick={() => this.limpiarEstados()} style={{ marginLeft: '0.3em' }} className="btn btn-outline-primary">Cancelar</button>
                    </div>
                </div>
                <div>
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    <BootstrapTable data={this.state.colaboradores} hover={true} pagination={true} options={options}>
                        <TableHeaderColumn width={'6%'} dataField="nro" isKey={true}
                            dataAlign="center"
                            headerAlign="center"
                            tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                            thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                            dataSort={true}>No.</TableHeaderColumn>
                        <TableHeaderColumn dataField="nombre_identificacion" hidden tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                            thStyle={{ whiteSpace: 'normal', fontSize: '10px' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Tipo Identificación</TableHeaderColumn>
                        <TableHeaderColumn width={'9%'} dataField="numero_identificacion" dataFormat={this.formatNumeroIdentificacion.bind(this)} thStyle={{ whiteSpace: 'normal', fontSize: '10px' }} tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>No. Identificación</TableHeaderColumn>
                        <TableHeaderColumn width={'10%'} dataField='apellidos_nombres' thStyle={{ whiteSpace: 'normal', fontSize: '10px' }} tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Apellidos</TableHeaderColumn>
                        <TableHeaderColumn width={'10%'} dataField='nombres' dataFormat={this.formatNombres.bind(this)} thStyle={{ whiteSpace: 'normal', fontSize: '10px' }} tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Nombres</TableHeaderColumn>
                        <TableHeaderColumn width={'10%'} dataField='nombre_grupo_personal' hidden thStyle={{ whiteSpace: 'normal', fontSize: '10px' }} tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Encargado de Personal</TableHeaderColumn>
                        <TableHeaderColumn width={'8%'} dataField='estado' dataFormat={this.formatEstado.bind(this)} thStyle={{ whiteSpace: 'normal', fontSize: '10px' }} tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Estado</TableHeaderColumn>
                        <TableHeaderColumn width={'9%'} dataField='fecha_baja' thStyle={{ whiteSpace: 'normal', fontSize: '10px' }} tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }} dataAlign="center" dataFormat={this.formatFecha.bind(this)} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Fecha Baja</TableHeaderColumn>
                        <TableHeaderColumn width={'10%'} dataField='motivo_baja' thStyle={{ whiteSpace: 'normal', fontSize: '10px' }} tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Motivo de Baja</TableHeaderColumn>
                        <TableHeaderColumn width={'9%'} dataField='liquidado' thStyle={{ whiteSpace: 'normal', fontSize: '10px' }} tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }} headerAlign="center" dataAlign="center" dataFormat={this.formatFechaLiquidacion.bind(this)} filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Fecha Liquidación</TableHeaderColumn>
                        <TableHeaderColumn width={'20%'} dataField='Id' thStyle={{ whiteSpace: 'normal', fontSize: '10px' }} tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }} headerAlign="center" dataFormat={this.generateButton.bind(this)}>Opciones</TableHeaderColumn>
                    </BootstrapTable>

                    {/* <button onClick={() => this.GenerarArchivoBajas()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Excel</button> */}

                    <Dialog header="Subir Archivo" visible={this.state.uploadPDF} width="500px" modal={true} onHide={this.ocultarUploadPDF}>
                     
                        <div className="content-section implementation">
                            <div className="p-grid" style={{ width: '250px', marginBottom: '10px' }}>
                                {this.state.baja != null &&
                                    <div className="p-col-12">

                                        <Checkbox checked={this.state.checkedbaja} onChange={e => this.setState({ checkedbaja: e.checked, checkedliquidacion: false, ok: false })} />  <label>Baja</label>
                                    </div>
                                }
                                {this.state.baja != null && this.state.baja.fecha_pago_liquidacion !=null &&
                                    <div className="p-col-12">

                                        <Checkbox checked={this.state.checkedliquidacion} onChange={e => this.setState({ checkedliquidacion: e.checked, checkedbaja: false, ok: !this.state.ok })} /> <label>Pago Liquidación</label></div>
                                }
                            </div>


                        </div>
                        {this.state.checkedbaja &&
                            <UploadPdfForm
                                handleChange={this.handleChangeUploadFile}
                                ok={this.state.ok}
                            />
                        }
                        {this.state.checkedliquidacion &&
                            <UploadPdfForm
                                handleChange={this.handleChangeUploadFile}
                                ok={this.state.ok}
                            />
                        }

                    </Dialog>

                    <Dialog header="Bajar Archivo" visible={this.state.downloadPDF}  width="500px" modal={true} onHide={this.ocultarDownload}>
                       
                        <div className="content-section implementation">
                            <div className="p-grid" style={{ width: '250px', marginBottom: '10px' }}>

                                {this.state.baja != null && this.state.baja.ArchivoId &&
                                    <div className="p-col-12">

                                        <Checkbox checked={this.state.checkedbaja} onChange={e => this.setState({ checkedbaja: e.checked, checkedliquidacion: false, ok: false })} />  <label>Baja</label>
                                    </div>
                                }
                                {this.state.baja != null && this.state.baja.archivo_liquidacion_id &&
                                    <div className="p-col-12">

                                        <Checkbox checked={this.state.checkedliquidacion} onChange={e => this.setState({ checkedliquidacion: e.checked, checkedbaja: false, ok: !this.state.ok })} /> <label>Pago Liquidación</label></div>
                                }
                            </div>

                        </div>
                        {this.state.checkedbaja &&
                            <div>
                                <a href={this.state.link}
                                    className="btn btn-primary"
                                    role="button"
                                    onClick={() => this.onDownload()}
                                    style={{ marginRight: '0.3em', color: 'white' }}
                                > Descargar</a>
                                <button icon="pi pi-times" onClick={() => this.ocultarDownload()} className="btn btn-primary" style={{ marginRight: '0.3em' }}>Cancelar</button>
                            </div>
                        }
                        {this.state.checkedliquidacion &&
                            <div>
                                <a href={this.state.link}
                                    className="btn btn-primary"
                                    role="button"
                                    onClick={() => this.onDownload()}
                                    style={{ marginRight: '0.3em', color: 'white' }}
                                > Descargar</a>
                                <button icon="pi pi-times" onClick={() => this.ocultarDownload()} className="btn btn-primary" style={{ marginRight: '0.3em' }}>Cancelar</button>
                            </div>
                        }

                    </Dialog>

                    <Dialog header="Generar Certificado de Trabajo"
                        visible={this.state.visible_certificado}
                        width="850px" modal={true}
                        footer={footerCertificado}
                        minY={70}
                        onHide={this.onHideCertificado}>
                        <BlockUi tag="div" blocking={this.state.block}>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <b><label>Tipo de Identificación: </label></b>
                                        <br />
                                        {this.state.colaborador != null ? this.state.colaborador.nombre_identificacion : ""}
                                    </div>
                                    <div className="form-group">
                                        <b><label>Nombres y Apellidos: </label></b>
                                        <br />
                                        {this.state.colaborador != null ? this.state.colaborador.nombres_apellidos : ""}
                                    </div>
                                    <div className="form-group">
                                        <b><label>ID SAP: </label></b>
                                        <br />
                                        {this.state.colaborador != null ? this.state.colaborador.candidato_id_sap : "-"}
                                    </div>
                                    <div className="form-group">
                                        <b><label>Fecha Desde</label></b>
                                        <input
                                            type="date"
                                            id="no-filter"
                                            name="fecha_ingreso_cert"
                                            className="form-control"
                                            onChange={this.handleChange}
                                            value={this.state.fecha_ingreso_cert}
                                        />
                                    </div>
                                </div>

                                <div className="col">
                                    <div className="form-group">
                                        <b><label>Número de Identificación: </label></b>
                                        <br />
                                        {this.state.colaborador != null ? this.state.colaborador.numero_identificacion : ""}
                                    </div>
                                    <div className="form-group">
                                        <b><label>Estado: </label></b>
                                        <br />
                                        {this.state.colaborador != null ? this.state.colaborador.estado : ""}
                                    </div>
                                    <div className="form-group">
                                        <b><label>ID Legajo: </label></b>
                                        <br />
                                        {this.state.colaborador != null && this.state.colaborador.numero_legajo_definitivo != null ? this.state.colaborador.numero_legajo_definitivo : "-"}
                                    </div>
                                    <div className="form-group">
                                        <b> <label>Fecha Hasta:</label></b>
                                        <input
                                            type="date"
                                            id="no-filter"
                                            name="fecha_baja_cert"
                                            className="form-control"
                                            onChange={this.handleChange}
                                            value={this.state.fecha_baja_cert}
                                        />
                                    </div>
                                    <ProgressBar value={this.state.value1}></ProgressBar>
                                </div>

                            </div>

                        </BlockUi>
                    </Dialog>

                    <Dialog header="Registrar Pago de Liquidación" visible={this.state.visible_pago} width="850px" modal={true} footer={footerPago} minY={70} onHide={this.onHidePago}>
                        <PagoLiquidacion
                            ref={this.childPago}
                            successMessage={this.successMessage}
                            warnMessage={this.warnMessage}
                            onHidePago={this.onHidePago}
                            baja_id={this.state.baja_id}
                            tipo_identificacion={this.state.tipo_identificacion}
                            nro_identificacion={this.state.nro_identificacion}
                            nombres_apellidos={this.state.nombres_apellidos}
                            nro_legajo={this.state.nro_legajo}
                            id_sap={this.state.id_sap}
                            motivo_baja={this.state.motivo_baja}
                            fecha_baja={this.state.fecha_baja}
                            GetColaboradores={this.GetColaboradores}
                        />
                    </Dialog>

                    <Dialog header="Desestimar Baja" visible={this.state.visible_desestimacion} width="850px" modal={true} footer={footerDesestimacion} minY={70} onHide={this.onHideDesestimacion}>
                        <BlockUi tag="div" blocking={this.state.loading}>
                            <div className="row" >
                                <div className="col-xs-12 col-md-12" >
                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="text"><b>Tipo de Identificación:</b> {this.state.tipo_identificacion} </label>
                                            </div>
                                        </div>
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="text"><b>No. de Identificación:</b> {this.state.nro_identificacion} </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="text"><b>Apellidos Nombres:</b> {this.state.nombres_apellidos}</label>
                                            </div>
                                        </div>
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="text"><b>ID Legajo:</b> {this.state.nro_legajo} </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="motivo_desestimacion">* Motivo de Desestimación: </label>
                                                <textarea type="text" id="motivo_desestimacion" /*rows="1"*/ maxLength="200" className="form-control" value={this.state.motivo_desestimacion} onChange={this.handleChange} name="motivo_desestimacion" />
                                                <span style={{ color: "red" }}>{this.state.errorDesestimacion}</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </BlockUi>
                    </Dialog>

                    <Dialog header="Envío Manual de Bajas a SAP" visible={this.state.visible_envio_Manual} width="1000px" modal={true} footer={footer} onHide={this.onHideEnvioManual}>
                        <BlockUi tag="div" blocking={this.state.loading_envioManual}>
                            <BootstrapTable data={this.state.bajasEnvio} hover={true} pagination={true} options={options} selectRow={selectRowProp} containerStyle={{ width: '100%', overflowX: 'scroll' }}>
                                <TableHeaderColumn width={'6%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn>
                                <TableHeaderColumn width={'8%'} dataField="id_sap" dataFormat={this.IDSAP.bind(this)} dataAlign="center" thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>ID SAP</TableHeaderColumn>
                                <TableHeaderColumn dataField="nombre_identificacion" dataFormat={this.formatTipoIdentificacion.bind(this)} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Tipo Identificación</TableHeaderColumn>
                                <TableHeaderColumn dataField="numero_identificacion" dataFormat={this.formatNumeroIdentificacion.bind(this)} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>No. Identificación</TableHeaderColumn>
                                <TableHeaderColumn dataField='apellidos_nombres' dataFormat={this.formatNombres.bind(this)} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Apellidos Nombres</TableHeaderColumn>
                                <TableHeaderColumn dataField='encargado_personal' dataFormat={this.formatEncargadoPersonal.bind(this)} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Encargado de Personal</TableHeaderColumn>
                                <TableHeaderColumn dataField='motivo_baja' headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} dataSort={true}>Motivo Baja</TableHeaderColumn>
                                <TableHeaderColumn width={'9%'} /*dataField='fecha_baja'*/ dataFormat={this.formatFechaBaja.bind(this)} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Fecha Baja</TableHeaderColumn>
                                <TableHeaderColumn dataField='estado_baja' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Estado Baja</TableHeaderColumn>
                            </BootstrapTable>
                        </BlockUi>
                    </Dialog>

                    <Dialog header="Validación de Información a SAP" visible={this.state.visible_confirmacion} width="1000px" modal={true} onHide={this.onHideConfirmacionBaja}>
                        <ConfirmarBajas
                            ref={this.childConfirmacion}
                            onHide={this.onHideConfirmacionBaja}
                            GetColaboradores={this.GetColaboradores}
                        />
                    </Dialog>

                    <Dialog header="Generación Archivo IESS" visible={this.state.visible_iess} width="1000px" modal={true} onHide={this.onHideArchivoIESS}>
                        <EnvioIESS
                            ref={this.childIESS}
                            onHide={this.onHideArchivoIESS}
                            GetColaboradores={this.GetColaboradores}
                        />
                    </Dialog>

                    <Dialog header="Editar Baja" visible={this.state.visible_edicion} width="800px" height="550px" modal={true} onHide={this.onHideEdicion}>
                        <EditarBaja
                            onHide={this.onHideEdicion}
                            baja_id={this.state.baja_id}
                            tipo_identificacion={this.state.tipo_identificacion}
                            nro_identificacion={this.state.nro_identificacion}
                            nombres_apellidos={this.state.nombres_apellidos}
                            nro_legajo={this.state.nro_legajo}
                            id_sap={this.state.id_sap}
                            motivo_baja={this.state.motivo_baja}
                            fecha_baja={this.state.fecha_baja}
                            fecha_ingreso={this.state.fecha_ingreso}
                            GetColaboradores={this.GetColaboradores}
                            MotivoBajaId={this.state.MotivoBajaId}
                            CatalogoBajas={this.state.CatalogoBajas}
                            detalle_baja={this.state.detalle_baja}
                            motivo_edicion={this.state.motivo_edicion}
                            errors={this.state.errors}
                        />
                    </Dialog>

                </div>
            </BlockUi>
        )
    }

    GenerarArchivoIESS() {
        this.setState({ loading: true });
        axios.post("/RRHH/ColaboradorBaja/GetArchivoIESSApi/", {})
            .then((response) => {
                console.log(response.data);
                this.setState({ loading: false });
                if (response.data == "OK") {
                    abp.notify.success("Arhivo generado", "Aviso");
                    this.onHideEnvioManual();
                } else {
                    abp.notify.error("Algo salió mal", 'Error');
                }

            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false });
            });
    }

    EnvioManual() {
        this.setState({ loading: true });
        console.log(this.state.filas)
        axios.post("/RRHH/ColaboradorBaja/GetBajaManualApi/", { ids: this.state.filas })
            .then((response) => {
                this.setState({ loading: false });
                if (response.data == "OK") {
                    abp.notify.success("Excel generado", "Aviso");
                    this.onHideEnvioManual();
                } else {
                    abp.notify.error("Algo salió mal", 'Error');
                }

            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false });
            });
    }

    GenerarArchivoBajas() {
        this.setState({ loading: true });
        console.log(this.state.filas)
        axios.post("/RRHH/ColaboradorBaja/GetExcelBajasApi/", {})
            .then((response) => {
                this.setState({ loading: false });
                if (response.data == "OK") {
                    abp.notify.success("Excel generado", "Aviso");
                    // this.onHideEnvioManual();
                } else {
                    abp.notify.error("Algo salió mal", 'Error');
                }

            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false });
            });
    }

    DatosEnvio(row, isSelected, e) {
        var select = this.state.filas.slice();
        console.log('row', row)
        if (isSelected == true) {
            select.push(row.Id);
        } else {
            var i = select.findIndex(c => c == row.Id)
            if (i > -1) {
                select.splice(i, 1)
            }
        }
        console.log(select)
        this.setState({
            filas: select
        });
    }

    onSelectAll(isSelected, rows) {
        var select = [];
        if (isSelected == true) {
            rows.forEach(e => {
                select.push(e.Id)
            });
            this.setState({
                filas: select
            });
        } else {
            this.setState({
                filas: select
            });
        }
        console.log(select)
    }

    formatFecha(cell, row) {
        if (row.fecha_baja != null) {
            return moment(row.fecha_baja).format('DD-MM-YYYY');
        } else {
            return null;
        }

    }

    GetColaboradoresEnvioSap() {
        this.setState({ loading_envioManual: true })
        axios.post("/RRHH/ColaboradorBaja/GetBajasEnviarSapApi/", {})
            .then((response) => {
                console.log(response.data)
                this.setState({ bajasEnvio: response.data, loading_envioManual: false })
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading_envioManual: false })
            });
    }

    formatFechaLiquidacion(cell, row) {
        if (row.fecha_pago_liquidacion != null) {
            return moment(row.fecha_pago_liquidacion).format('DD-MM-YYYY');
        } else {
            return null;
        }

    }

    formatNumeroIdentificacion(cell, row) {
        return row.Colaboradores.numero_identificacion;
    }

    formatNombres(cell, row) {
        return row.Colaboradores.nombres;
    }

    formatEstado(cell, row) {
        return row.Colaboradores.estado;
    }

    IDSAP(cell, row) {
        return row.Colaboradores.empleado_id_sap;
    }

    formatTipoIdentificacion(cell, row) {
        return row.Colaboradores.TipoIdentificacion.nombre;
    }

    formatNumeroIdentificacion(cell, row) {
        return row.Colaboradores.numero_identificacion;
    }

    // formatNombres(cell, row) {
    //     return row.Colaboradores.nombres_apellidos;
    // }

    formatEncargadoPersonal(cell, row) {
        return row.Colaboradores.EncargadoPersonal.nombre;
    }

    formatFechaBaja(cell, row) {
        return moment(row.fecha_baja).format("DD-MM-YYYY");
    }



    generateButton(cell, row) {
        var estado = row.Colaboradores.estado;
        if (estado == "INACTIVO") {
            return (
                <div>
                    <button onClick={() => this.LoadPago(row)} data-toggle="tooltip" data-placement="top" title="Pago Liquidación" type="button" style={{ marginLeft: '0.3em' }} className="btn btn-outline-primary btn-sm fa fa-credit-card"></button>
                    <button onClick={() => this.LoadDesestimacion(row)} data-toggle="tooltip" data-placement="top" title="Desestimar Baja" type="button" style={{ marginLeft: '0.3em' }} className="btn btn-outline-primary btn-sm fa fa-thumbs-up" ></button>
                    <button onClick={() => this.LoadBaja(row)} data-toggle="tooltip" data-placement="top" title="Editar" type="button" style={{ marginLeft: '0.3em' }} className="btn btn-outline-primary btn-sm fa fa-edit" ></button>
                    <button className="btn btn-outline-success btn-sm fa fa-file-word-o" onClick={() => this.showFormCertificado(row.Colaboradores.Id)} style={{ float: 'left', marginLeft: '0.3em' }} data-toggle="tooltip" data-placement="top" title="Certificado de Trabajo"></button>

                    {row!=null &&
                       <button className="btn btn-outline-indigo btn-sm" style={{ marginLeft: '0.3em' }}
                            onClick={() => this.mostrarUploadPDF(row)}
                            data-toggle="tooltip"
                            data-placement="top"
                            title="Subir Documento">
                            <i className="fa fa-cloud-upload"></i>
                        </button>}
                    {row!=null &&
                        <button className="btn btn-outline-indigo btn-sm" style={{ marginLeft: '0.3em' }}
                            onClick={() => this.mostrarDownload(row)}
                            data-toggle="tooltip"
                            data-placement="top"
                            title="Descargar Documento">
                            <i className="fa fa-cloud-download"></i>
                        </button>
                    }

                </div>
            )
        } else if (estado == "ACTIVO") {
            return (
                <div>
                    <button onClick={() => this.LoadColaborador(row.Colaboradores.Id, row.Colaboradores.catalogo_encargado_personal_id)} data-toggle="tooltip" data-placement="top" title="Dar de Baja" type="button" className="btn btn-outline-primary btn-sm fa fa-thumbs-down"></button>
                    <button className="btn btn-outline-success btn-sm fa fa-file-word-o" onClick={() => this.showFormCertificado(row.Colaboradores.Id)} style={{ float: 'left', marginRight: '0.3em' }} data-toggle="tooltip" data-placement="top" title="Certificado de Trabajo"></button>
                </div>
            )
        } else {
            return (
                <div>
                    <button className="btn btn-outline-success btn-sm fa fa-file-word-o" onClick={() => this.showFormCertificado(row.Colaboradores.Id)} style={{ float: 'left', marginRight: '0.3em' }} data-toggle="tooltip" data-placement="top" title="Certificado de Trabajo"></button>
                </div>
            )
        }



    }

    GetColaboradorInformation(Id) {
        this.setState({ block: true })
        console.log('Informacion Colaborador');

        if (Id > 0) {
            axios.post("/RRHH/Colaboradores/GetColaboradorApi/", {
                Id: Id
            })
                .then((response) => {
                    console.log(response.data);
                    this.setState({
                        colaboradoId: Id, colaborador: response.data,
                        fecha_ingreso_cert: response.data.fecha_ingreso != null ?
                            moment(response.data.fecha_ingreso).format("YYYY-MM-DD")
                            : moment(new Date()).format("YYYY-MM-DD"),

                        fecha_baja_cert: response.data.fecha_baja != null ?
                            moment(response.data.fecha_baja).format("YYYY-MM-DD")
                            : moment(new Date()).format("YYYY-MM-DD"),
                        block: false
                    })


                })
                .catch((error) => {
                    console.log(error);
                    this.setState({ block: false })
                });
        }
    }

    GetColaboradores() {
        this.setState({ loading: true })
        axios.post("/RRHH/Colaboradores/GetColaboradoresBajasApi/", {})
            .then((response) => {

                console.log(response.data)
                this.setState({ colaboradores: response.data, loading: false })
                // this.procesarDatos(response.data);
            })
            .catch((error) => {
                this.setState({ loading: false })
                console.log(error);
            });
    }

    GetColaboradoreBuscar() {
        if (!this.state.nro_identificacionConsulta && !this.state.nombresConsulta && !this.state.estado) {
            abp.notify.error("Debe seleccionar un criterio de busqueda!", 'Error');
        } else {
            if (this.state.nombresConsulta.length < 3 && this.state.nombresConsulta.length != 0) {
                abp.notify.error("Debe ingresar al menos tres caracteres para realizar la busqueda por apellidos nombres!", 'Error');
            } else {
                this.setState({ loading: true })

                var numeroIdentificacion = "";
                var nombres = "";
                var estado = "";

                if (this.state.nro_identificacionConsulta) {
                    numeroIdentificacion = this.state.nro_identificacionConsulta;
                }

                if (this.state.nombresConsulta) {
                    nombres = this.state.nombresConsulta;
                }

                if (this.state.estado) {
                    estado = this.state.estado;
                }

                axios.post("/RRHH/Colaboradores/GetFiltrosBajas/",
                    {
                        numeroIdentificacion: numeroIdentificacion,
                        nombres: nombres,
                        estado: estado
                    })
                    .then((response) => {
                        console.log('consulta', response.data)
                        if (response.data.length == 0) {
                            this.setState({ loading: false, colaboradores: [] })
                            abp.notify.error("No existe registros con la información ingresada", 'Error');
                        } else {
                            // this.setState({ loading: false, colaboradores: response.data })
                            this.procesaConsulta(response.data);
                        }
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            }
        }
    }

    procesaConsulta(data) {
        var array = [];
        data.forEach(e => {
            var c = {};
            c.Colaboradores = {};

            c.Colaboradores.Id = e.Id;
            c.Id = e.baja_id;
            c.nro = e.nro;
            c.nombre_identificacion = e.nombre_identificacion;
            c.Colaboradores.numero_identificacion = e.numero_identificacion;
            c.apellidos_nombres = e.apellidos_nombres;
            c.Colaboradores.nombres = e.nombres;
            c.nombre_grupo_personal = e.nombre_grupo_personal;
            c.Colaboradores.estado = e.estado;
            c.fecha_baja = e.fecha_baja;
            c.liquidado = e.estaLiquidado;
            c.fecha_pago_liquidacion = e.liquidado;
            c.motivo_baja = e.motivo_baja;
            c.Colaboradores.catalogo_encargado_personal_id = e.catalogo_encargado_personal_id;

            array.push(c);
        });
        this.setState({ loading: false, colaboradores: array })
    }

    limpiarEstados() {

        this.setState({
            nro_identificacionConsulta: '',
            estado: '',
            nombresConsulta: '',
        })
    }

    LoadColaborador(id, encargado) {
        sessionStorage.setItem('id_colaborador', id);
        sessionStorage.setItem('enargado_personal_id', encargado);
        return (
            window.location.href = "/RRHH/Colaboradores/Bajas/"
        );
    }

    LoadPago(data) {
        console.log(data);
        if (data.liquidado == "NO") {
            if (data.estado == 3) {
                this.setState({
                    baja_id: data.Id,
                    tipo_identificacion: data.nombre_identificacion,
                    nro_identificacion: data.Colaboradores.numero_identificacion,
                    nombres_apellidos: data.Colaboradores.nombres_apellidos,
                    nro_legajo: data.Colaboradores.numero_legajo_temporal,
                    id_sap: data.Colaboradores.empleado_id_sap == 0 ? '' : data.Colaboradores.empleado_id_sap,
                    motivo_baja: data.catalogo_motivo_baja_id,
                    fecha_baja: moment(data.fecha_baja).format("YYYY-MM-DD"),
                }, this.showFormPago)
            } else {
                abp.notify.error("La baja no ha sido confirmada en Sap", 'Error');
            }
        } else {
            abp.notify.error("El colaborador ya ha sido liquidado!", 'Error');
        }

    }

    LoadDesestimacion(data) {
        console.log(data);
        console.log(moment().format("YYYY-MM-DD"), moment().format("M"), moment(data.fecha_baja).format("M"));
        if (data.liquidado == "NO") {
            if (moment().format("M") == moment(data.fecha_baja).format("M")) {
                this.setState({
                    baja_id: data.Id,
                    colaborador_id: data.Colaboradores.Id,
                    tipo_identificacion: data.nombre_identificacion,
                    nro_identificacion: data.Colaboradores.numero_identificacion,
                    nombres_apellidos: data.Colaboradores.nombres_apellidos,
                    nro_legajo: data.Colaboradores.numero_legajo_temporal,
                    id_sap: data.Colaboradores.candidato_id_sap == 0 ? '' : data.Colaboradores.candidato_id_sap,
                    motivo_baja: data.catalogo_motivo_baja_id,
                    fecha_baja: moment(data.fecha_baja).format("YYYY-MM-DD"),
                }, this.showFormDesestimacion)
            } else {
                abp.notify.error("El plazo de desestimación ha finalizado!", 'Error');
            }

        } else {
            abp.notify.error("El colaborador ya ha sido liquidado!", 'Error');
        }

    }

    GuardarDesestimacion() {
        if (this.state.motivo_desestimacion != '') {
            this.setState({ loading: true });
            axios.post("/RRHH/ColaboradorBaja/CreateDesestimacionApi/", {
                Id: this.state.baja_id,
                catalogo_motivo_baja_id: this.state.motivo_baja,
                motivo_desestimacion: this.state.motivo_desestimacion,
                ColaboradoresId: this.state.colaborador_id
            })
                .then((response) => {
                    this.setState({ loading: false });
                    abp.notify.success("Desestimación Guardada!", "Aviso");
                    this.GetColaboradores();
                    this.onHideDesestimacion();
                })
                .catch((error) => {
                    this.setState({ loading: false });
                    console.log(error);
                    abp.notify.error("Algo salió mal.", 'Error');
                });
        } else {
            this.setState({ errorDesestimacion: 'El campo Motivo de Desestimación es obligatorio' })
        }

    }

    LoadBaja(data) {
        console.log(data);
        this.setState({
            baja_id: data.Id,
            colaborador_id: data.Colaboradores.Id,
            tipo_identificacion: data.nombre_identificacion,
            nro_identificacion: data.Colaboradores.numero_identificacion,
            nombres_apellidos: data.Colaboradores.nombres_apellidos,
            nro_legajo: data.Colaboradores.numero_legajo_temporal,
            id_sap: data.Colaboradores.candidato_id_sap == 0 ? '' : data.Colaboradores.candidato_id_sap,
            motivo_baja: data.motivo_baja,
            fecha_ingreso: moment(data.Colaboradores.fecha_ingreso).format("YYYY-MM-DD"),
            fecha_baja: moment(data.fecha_baja).format("YYYY-MM-DD"),
            detalle_baja: data.detalle_baja,
            MotivoBajaId: data.catalogo_motivo_baja_id,
            motivo_edicion: data.motivo_edicion,
            errors: {},

        }, this.showFormEdicion)
    }

    onHideCertificado() {
        this.setState({ visible_certificado: false })
    }

    showFormCertificado(Id) {
        console.log(Id);
        this.setState({ visible_certificado: true, value1: 0 });
        this.GetColaboradorInformation(Id);

    }

    ConsultaEstados() {
        this.setState({ loading: true })
        axios.post("/RRHH/Colaboradores/GetCatalogosPorCodigoApiSinAltaAnulada/", {
            codigo: 'ESTADOSCOL'
        })
            .then((response) => {
                // console.log('tiposDestinos', response.data)
                this.setState({
                    tiposEstados: response.data,
                    // loading: false
                })
                this.getFormSelectEstado();
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false })
            });

        axios
            .post("/RRHH/ColaboradorBaja/GetByCodeApi/?code=MOTIVOBAJA", {})
            .then(response => {

                var items = response.data.result.map(item => {
                    return { label: item.nombre, dataKey: item.Id, value: item.Id };
                });
                this.setState({ CatalogoBajas: items });

            })
            .catch(error => {
                console.log(error);

            });


    }

    getFormSelectEstado() {
        return (
            this.state.tiposEstados.map((item) => {
                return (
                    <option key={Math.random()} value={item.nombre}>{item.nombre}</option>
                )
            })
        );
    }



    onHidePago() {
        this.setState({ visible_pago: false })
    }

    showFormPago() {
        this.setState({ visible_pago: true })
    }

    onHideDesestimacion() {
        this.setState({ visible_desestimacion: false })
    }

    showFormDesestimacion() {
        this.setState({ visible_desestimacion: true })
    }

    onHideEdicion() {
        this.setState({ visible_edicion: false })
    }

    showFormEdicion() {
        this.setState({ visible_edicion: true })
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

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
    }

    onHideEnvioManual() {
        this.setState({ visible_envio_Manual: false })
    }

    showFormEnvioManual() {
        // console.log("showFormEnvioManual")
        this.GetColaboradoresEnvioSap();
        this.setState({ visible_envio_Manual: true })
    }

    onHideConfirmacionBaja() {
        this.setState({ visible_confirmacion: false })
    }

    showFormConfirmacionBaja() {
        this.childConfirmacion.current.GetColaboradoresEnvioSap();
        this.setState({ visible_confirmacion: true });
    }

    onHideArchivoIESS() {
        this.setState({ visible_iess: false })
    }

    showFormArchivoIESS() {
        this.childIESS.current.GetColaboradoresEnvioSap();
        this.setState({ visible_iess: true })
    }

    mostrarUploadPDF = row => {
       
        this.setState({ uploadPDF: true, baja: row, BajaId: row.Id })
    }
    mostrarDownload = row => {
        if (row != null && row.ArchivoId == null && row.archivo_liquidacion_id == null) {
            console.log("no tienes archivos cargados");
            abp.notify.warn("No posee ningun archivo cargado", 'Aviso');

        } else {
        this.setState({ downloadPDF: true, baja: row })
        }
    }

    ocultarUploadPDF = () => {
        this.setState({ uploadPDF: false, uploadFile: '', ok: false, checkedbaja: false, checkedliquidacion: false })
    }

    ocultarDownload = () => {
        this.setState({ downloadPDF: false, uploadFile: '', ok: false, checkedbaja: false, checkedliquidacion: false })
    }
    onDownload = () => {
        if (this.state.checkedbaja) {
            if (this.state.baja != null && this.state.baja.ArchivoId > 0) {
                return (
                    window.location = `/RRHH/ColaboradorBaja/GetArchivo/${this.state.baja.ArchivoId}`
                );
            }
        } else if (this.state.checkedliquidacion) {
            if (this.state.baja != null && this.state.baja.archivo_liquidacion_id > 0) {
                return (
                    window.location = `/RRHH/ColaboradorBaja/GetArchivo/${this.state.baja.archivo_liquidacion_id}`
                );
            }
        }
    }
    handleChangeUploadFile = event => {
        if (event.files) {
            if (event.files.length > 0) {
                let uploadFile = event.files[0];
                this.setState({
                    uploadFile: uploadFile
                }, this.handleSubmitUploadFile);
            } else {
                abp.notify.error("Selecciona un Archivo", 'Error');
            }
        } else {
            abp.notify.error("Selecciona un Archivo", 'Error');
        }
    }

    handleSubmitUploadFile = () => {
        this.setState({ loading: true })
        const formData = new FormData();
        formData.append('uploadFile', this.state.uploadFile);
        formData.append('Id', this.state.BajaId);
        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };

        if (this.state.checkedbaja) {
            console.log("Comienza archivo baja");

            axios.post("/RRHH/ColaboradorBaja/GetSubirArchivo", formData, config)
                .then((response) => {
                    let data = response.data;
                    if (data.success === true) {
                        this.setState({
                            uploadFile: ''
                        });
                        abp.notify.success("Documento Cargado Correctamente!", "Aviso");
                        this.ocultarUploadPDF()
                        this.GetColaboradores();
                    } else {
                        var message = $.fn.responseAjaxErrorToString(data);
                        abp.notify.error(message, 'Error');
                    }
                  
                })
                .catch((error) => {
                    console.log(error);
                    abp.notify.error('Existe un incoveniente intentelo más tarde', 'Error');
                    this.setState({ loading: false })
                });
        } else {
            console.log("Comienza archivo pago");
            axios.post("/RRHH/ColaboradorBaja/GetSubirArchivoPago", formData, config)
                .then((response) => {
                    let data = response.data;
                    if (data.success === true) {
                        this.setState({
                            uploadFile: ''
                        });
                        abp.notify.success("Documento  Cargado Correctamente!", "Aviso");
                        this.ocultarUploadPDF()
                        this.GetColaboradores();
                    } else {
                        var message = $.fn.responseAjaxErrorToString(data);
                        abp.notify.error(message, 'Error');
                    }
                   
                })
                .catch((error) => {
                    console.log(error);
                    abp.notify.error('Existe un incoveniente intentelo más tarde', 'Error');
                    this.setState({ loading: false })
                });
        }
    }




    generarCertificadosDeCapacitacionesDeBajas = () => {
        if (this.state.colaboradores.length === 0) {
            abp.notify.error("No existen colaboradores en el listado de bajas");
        } else {
            let parametros = '';
            this.setState({ loading: true })
           
            this.state.colaboradores.forEach(e => {
                if(e.ColaboradoresId !== undefined){
                    parametros += `colaboradores=${e.ColaboradoresId}&`
                } else {
                    parametros += `colaboradores=${e.Colaboradores.Id}&`
                }
                
            });

            var arrayColaboradores = this.state.colaboradores.map(e => {
                if(e.ColaboradoresId !== undefined){
                   return e.ColaboradoresId;
                } else {
                    return e.Colaboradores.Id;
                }
            })

            let url = `/RRHH/Capacitacion/GenerarCertificados`

            axios.post(
                url, 
                {colaboradores: arrayColaboradores},
                { responseType: "arraybuffer" }
            )
            .then((response) => {
                var nombre = response.headers["content-disposition"].split("=");

                const url = window.URL.createObjectURL(
                    new Blob([response.data], {
                        type:
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    })
                );
                const link = document.createElement("a");
                link.href = url;
                link.setAttribute("download", nombre[1]);
                document.body.appendChild(link);
                link.click();
                abp.notify.success("Certificados generados exitosamente", "Aviso");
                this.setState({ loading: false })
            })
            .catch((error) => {
                console.log(error);
                abp.notify.error("Ocurrió un error al descargar el archivo, intentalo nuevamente");
                this.setState({ loading: false })
            });
        }
    }


}

ReactDOM.render(
    <ColaboradorBajas />,
    document.getElementById('content-bajas')
);