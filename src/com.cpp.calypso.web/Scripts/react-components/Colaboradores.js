import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import ColaboradoresTable from './Colaboradores/ColaboradoresTable';
import BlockUi from 'react-block-ui';
import moment from 'moment';
import wrapForm from "./Base/BaseWrapper";
import { ProgressBar } from 'primereact/components/progressbar/ProgressBar';
import { Dialog } from 'primereact/components/dialog/Dialog';
import Field from "./Base/Field-v2";
import { Button } from "primereact/components/button/Button";
export default class Colaboradores extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            colaboradores: [],
            loading: true,
            nro_identificacionConsulta: '',
            nombresConsulta: '',
            estado: '',
            tiposEstados: [],


            //CERTIFICADO DE TRABAJO

            fecha_ingreso_cert: moment(new Date()).format("YYYY-MM-DD"),
            fecha_baja_cert: moment(new Date()).format("YYYY-MM-DD"),
            colaboradoId: 0,
            colaborador: null,
            block: true,
            link: "#",
            value1: 0,
            archivo: {},
            visible_certificado: false,

            pass: "",
            viewdisable: false,
            urlApiBase: '/RRHH/Colaboradores/',


        }
        this.GetColaboradores = this.GetColaboradores.bind(this);
        this.GetColaboradoreBuscar = this.GetColaboradoreBuscar.bind(this);
        this.ConsultaEstados = this.ConsultaEstados.bind(this);
        this.getFormSelectEstado = this.getFormSelectEstado.bind(this);

        this.Nuevo = this.Nuevo.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.handleChange = this.handleChange.bind(this);

        this.onHideCertificado = this.onHideCertificado.bind(this);
        this.showFormCertificado = this.showFormCertificado.bind(this);
        this.GetColaboradorInformation = this.GetColaboradorInformation.bind(this);
        this.onDesactivar = this.onDesactivar.bind(this);
    }

    componentDidMount() {
        this.ConsultaEstados();
        this.GetColaboradores();
        this.props.unlockScreen();

    }
    onHideview = () => {

        this.setState({ viewdisable: false, pass: "" });
    }
    viewdisable = (Id) => {
        console.log(Id);
        this.setState({ viewdisable: true });
        this.GetColaboradorInformation(Id);
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
                id: self.state.colaboradoId,
                pass: this.state.pass
            };


            axios.post(url, data)
                .then((response) => {

                    let data = response.data;

                    if (data.result === true) {

                        abp.notify.success("Proceso guardado exitosamente", "Aviso");
                        this.onHideview();
                        var newParams = {
                        };

                        self.GetColaboradores();

                    } else {
                        abp.notify.error("El código de seguridad es incorrecto, no está autorizado a continuar con el proceso", 'Error');
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

    onHideCertificado() {
        this.setState({ visible_certificado: false })
    }

    showFormCertificado(Id) {
        console.log(Id);
        this.setState({ visible_certificado: true, value1: 0 });
        this.GetColaboradorInformation(Id);

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

    render() {

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
        return (
            <div>
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
                <div className="row">
                    <div className="col">
                        <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                        <ColaboradoresTable
                            data={this.state.colaboradores}
                            successMessage={this.successMessage}
                            warnMessage={this.warnMessage}
                            GetColaboradores={this.GetColaboradores}
                            blockScreen={this.props.blockScreen}
                            unlockScreen={this.props.unlockScreen}
                            loading={this.state.loading}
                            showSuccess={this.props.showSuccess}
                            showWarning={this.props.showWarning}
                            showWarn={this.props.showWarn}
                            showFormCertificado={this.showFormCertificado}
                            GetColaboradoreBuscar={this.GetColaboradoreBuscar}
                            viewdisable={this.viewdisable}

                        />
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
                        <Dialog header="Anular Alta" visible={this.state.viewdisable} style={{ width: '50vw' }} modal onHide={this.onHideview} >

                            <div>¿Está seguro de anular el alta al colaborador?. Ingrese la clave de seguridad para proceder</div>
                            <br />
                            <Field
                                name="pass"
                                label="Código de Seguridad"
                                required
                                edit={true}
                                value={this.state.pass}
                                onChange={this.handleChangeCodigo}


                            /> <br />
                            <div align="right">
                                <Button label="SI" icon="pi pi-check" onClick={this.onDesactivar} />{" "}
                                <Button label="NO" icon="pi pi-times" className="p-button-secondary" onClick={this.onHideview} />
                            </div>
                        </Dialog>

                    </div>
                </div>
            </div>
        )
    }
    showFormCertificado(Id) {
        console.log(Id);
        this.setState({ visible_certificado: true, value1: 0 });
        this.GetColaboradorInformation(Id);

    }

    limpiarEstados() {
        this.setState({
            nro_identificacionConsulta: '',
            estado: '',
            nombresConsulta: '',
        }, this.GetColaboradores)
    }

    GetColaboradores() {
        this.setState({ loading: true })

        console.log("FirstList")
        axios.post("/RRHH/Colaboradores/FirstList/", {})

            .then((response) => {
                console.log(response.data)
                this.setState({ colaboradores: response.data })
                this.setState({ loading: false })
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false })
            });
    }

    GetColaboradoreBuscar() {
        this.setState({ loading: true });

        /*if (this.state.nro_identificacionConsulta=='' && this.state.nombresConsulta==''&& this.state.estado == '') {
            this.setState({ loading: false });

            abp.notify.error("Seleccione al menos un campo de búsqueda", 'Error');
        } else {
        */
        console.log("FilterSearch")
        axios.post("/RRHH/Colaboradores/FilterSearch/",
            {
                numeroIdentificacion: this.state.nro_identificacionConsulta,
                nombres: this.state.nombresConsulta,
                estado: this.state.estado
            })
            .then((response) => {
                console.log('consulta', response.data)
                if (response.data.length == 0) {
                    this.setState({ loading: false, colaboradores: [] })
                    abp.notify.error("No existe registros con la información ingresada", 'Error');
                } else {
                    this.setState({ loading: false, colaboradores: response.data })
                    // this.procesaConsulta(response.data);
                }
            })
            .catch((error) => {
                console.log(error);
            });
        /*}*/
    }

    Nuevo() {
        return (
            window.location.href = "/RRHH/Colaboradores/Create/"
        );
    }

    ConsultaEstados() {
        this.setState({ loading: true })
        axios.post("/RRHH/Colaboradores/GetByCodeApi/?code=ESTADOSCOL", {})
            .then((response) => {
                // console.log('tiposDestinos', response.data)
                this.setState({
                    tiposEstados: response.data.result,
                    // loading: false
                })
                this.getFormSelectEstado();
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false })
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
    handleChangeCodigo = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }
    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
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
const Container = wrapForm(Colaboradores);
ReactDOM.render(
    <Container />,
    document.getElementById('content-colaboradores')
);