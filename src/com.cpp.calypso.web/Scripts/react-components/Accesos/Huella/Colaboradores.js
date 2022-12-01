import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import moment, { now } from 'moment';
import wrapForm from "../../Base/BaseWrapper";
import Field from "../../Base/Field-v2";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import Fotografia from './Fotografia';
import { Dialog } from 'primereact/components/dialog/Dialog';

//Generación de Qr


import { Checkbox } from 'primereact-v2/checkbox';
import { Card } from 'primereact-v2/card';
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


            //FILTROS BUSQUEDA
            Identificacion: '',
            ApellidosNombres: '',
            Estado: '',
            errors: {},

            //Fotografia
            colaborador: [],
            visible_foto: false,
            Id: '',
            tipo_identificacion: '',
            nro_identificacion: '',
            apellidos: '',
            nombres: '',



            /*Generación QR */
            loadingqr: false,

            QrDialog: false,
            iseleccionado: null,

            //VALIDACION CÉDULA
            checked: false,
        }

        this.childFoto = React.createRef();
        this.GetColaboradores = this.GetColaboradores.bind(this);
        this.GetColaboradoreBuscar = this.GetColaboradoreBuscar.bind(this);
        this.ConsultaEstados = this.ConsultaEstados.bind(this);


        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.handleChange = this.handleChange.bind(this);


        //Nuevo
        this.onChangeValue = this.onChangeValue.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.GetColaboradoreBuscar = this.GetColaboradoreBuscar.bind(this);


        this.onHideFoto = this.onHideFoto.bind(this);
        this.showFormFoto = this.showFormFoto.bind(this);
        this.Fotografia = this.Fotografia.bind(this);


        /* Generación de QR */
        this.validacionCedula = this.validacionCedula.bind(this);
        this.OcultarDialogQr = this.OcultarDialogQr.bind(this);
        this.MostrarDialogQr = this.MostrarDialogQr.bind(this);

        this.DescargarQR = this.DescargarQR.bind(this);
        this.permitirvalidacioncedula = this.permitirvalidacioncedula.bind(this);

        this.onChangeValue = this.onChangeValue.bind(this);
        this.handleChange = this.handleChange.bind(this);



    }

    onHideFoto() {
        this.setState({ visible_foto: false })
    }

    componentDidMount() {
        this.ConsultaEstados();
        this.props.unlockScreen();

    }
    validacionCedula(cell, row) {
        return (
            <div>
                <label>{row.validacion_cedula == true ? "SI" : "NO"}</label>
            </div>
        );
    }
    generateButton = (cell, row) => {
        // console.log(row)
        return (
            <div>
                <button
                    className="btn btn-outline-info btn-sm fa fa-qrcode"
                    onClick={() => this.MostrarDialogQr(row)}
                    style={{ marginLeft: '0.2em' }}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Validación por Cédula"
                />
                <button onClick={() => this.HuellaDigital(row.Id)} data-toggle="tooltip" data-placement="top" title="Huella Digital" className="btn btn-outline-primary btn-sm fa fa-paw" style={{ marginLeft: '0.2em' }}></button>
                <button onClick={() => this.Fotografia(row.Id)} data-toggle="tooltip" data-placement="top" title="Fotografía" className="btn btn-outline-primary btn-sm fa fa-photo" style={{ marginLeft: '0.2em' }}></button>
            </div>
        )
    }

    formatFechaIngreso(cell, row) {
        return moment(row.fecha_ingreso).format("DD-MM-YYYY");
    }

    HuellaDigital = (id) => {
        sessionStorage.setItem('id_colaborador', id);
        return (
            window.location.href = "/Accesos/Huella/Captura/"
        );
    }

    Fotografia = (id) => {
        axios.post("/Accesos/Huella/GetColaboradorApi/" + id, {})
            .then((response) => {
                console.log(response)
                this.setState({
                    Id: id,
                    tipo_identificacion: response.data.nombre_identificacion,
                    nro_identificacion: response.data.numero_identificacion,
                    nombres: response.data.nombres_apellidos,
                    key_form: Math.random(),
                    colaborador: response.data
                }, this.showFormFoto)
            })
            .catch((error) => {
                this.props.warnMessage("");
            });
    }

    showFormFoto() {
        this.setState({ visible_foto: true }, this.childFoto.current.getFotografia);
    }

    render() {
        return (
            <div>

                <div className="row">
                    <div className="col-3">
                        <Field
                            name="Identificacion"
                            label="Identificación"
                            edit={true}
                            readOnly={false}
                            value={this.state.Identificacion}
                            onChange={this.handleChange}
                            error={this.state.errors.Identificacion}
                        />
                    </div>
                    <div className="col-3">
                        <Field
                            name="ApellidosNombres"
                            label="Apellidos Nombres"
                            edit={true}
                            readOnly={false}
                            value={this.state.ApellidosNombres}
                            onChange={this.handleChange}
                            error={this.state.errors.ApellidosNombres}
                        />
                    </div>
                    <div className="col-3">
                        <Field
                            name="Estado"
                            value={this.state.Estado}
                            label="Estado"
                            options={this.state.tiposEstados}
                            type={"select"}
                            onChange={this.onChangeValue}
                            error={this.state.errors.Estado}
                            readOnly={false}
                            filter={true}
                            placeholder="Seleccione"
                            filterPlaceholder="Seleccione"
                        />
                    </div>
                    <div className="col-3" style={{ paddingTop: '35px' }}>
                        <button
                            type="button"
                            onClick={() => this.GetColaboradoreBuscar()}
                            style={{ marginLeft: "0.2em" }}
                            className="btn btn-outline-primary"
                        >
                            Buscar
            </button>
                        <button
                            type="button"
                            onClick={() => this.limpiarEstados()}
                            style={{ marginLeft: "0.3em" }}
                            className="btn btn-outline-primary"
                        >
                            Cancelar
            </button>
                    </div>
                </div>




                <div>

                    <BootstrapTable data={this.state.colaboradores} hover={true} pagination={true} >
                        <TableHeaderColumn width={'5%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn>
                        <TableHeaderColumn dataField="nombre_identificacion" thStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Tipo Identificación</TableHeaderColumn>
                        <TableHeaderColumn dataField="numero_identificacion" thStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>No. Identificación</TableHeaderColumn>
                        <TableHeaderColumn /*width={'20%'}*/ dataField='MergeApellidos' tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Apellidos</TableHeaderColumn>
                        <TableHeaderColumn /*width={'20%'}*/ dataField='nombres' tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Nombres</TableHeaderColumn>
                        <TableHeaderColumn /*width={'20%'}*/ dataField='fecha_ingreso' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} dataFormat={this.formatFechaIngreso.bind(this)} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Fecha Ingreso</TableHeaderColumn>
                        <TableHeaderColumn dataField='nombre_destino' tdStyle={{ whiteSpace: 'normal' }} thStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Destino</TableHeaderColumn>
                        <TableHeaderColumn dataField='estado' headerAlign="center" tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Estado</TableHeaderColumn>
                        <TableHeaderColumn dataField='numeroHuellas' headerAlign="center" tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Nº Huellas</TableHeaderColumn>

                        <TableHeaderColumn dataField='Operaciones' headerAlign="center" width={'15%'} dataFormat={this.generateButton.bind(this)}>Opciones</TableHeaderColumn>


                    </BootstrapTable>
                    <Dialog header="Registrar Fotografía" visible={this.state.visible_foto} width="850px" modal={true} onHide={this.onHideFoto}>
                        <Fotografia ref={this.childFoto}
                            onHide={this.onHideFoto}
                            Id={this.state.Id}
                            tipo_identificacion={this.state.tipo_identificacion}
                            nro_identificacion={this.state.nro_identificacion}
                            nombres_apellidos={this.state.nombres}
                            colaborador={this.state.colaborador}
                        />
                    </Dialog>

                    <Dialog
                        header="Validación por Cédula"
                        visible={this.state.QrDialog}
                        style={{ width: "800PX" }}
                        modal={true}
                        onHide={this.OcultarDialogQr}

                    >
                        <BlockUi tag="div" blocking={this.state.loadingqr}>
                            <div>

                                <Card className="ui-card-shadow">
                                    <b>Información Colaborador</b><br /><br />
                                    <h6 className="text-gray-700">
                                        <b>No. de Identificación: </b>{" "}
                                        {this.state.iseleccionado != null
                                            ? this.state.iseleccionado.numero_identificacion
                                            : ""}
                                    </h6>
                                    <h6 className="text-gray-700">
                                        <b> Apellidos Nombres: </b>{" "}
                                        {this.state.iseleccionado != null
                                            ? this.state.iseleccionado.apellidos_nombres
                                            : ""}
                                    </h6>
                                    <h6 className="text-gray-700">
                                        <b>Destino: </b>{" "}
                                        {this.state.iseleccionado != null
                                            ? this.state.iseleccionado.nombreestancia
                                            : ""}
                                    </h6>
                                    <h6 className="text-gray-700">
                                        <b>Servicios: </b>{" "}
                                        {this.state.iseleccionado != null
                                            ? this.state.iseleccionado.serviciosvigentes
                                            : ""}
                                    </h6>
                                   
                                </Card><br />
                                <Card className="ui-card-shadow">
                                    <b>Permite Validación por Cédula:</b> <Checkbox checked={this.state.checked} onChange={this.permitirvalidacioncedula} /><br />
                                    <br />
                                </Card><br />

                            </div>
                        </BlockUi>
                    </Dialog>

                </div>
            </div>
        )
    }


    /* QR MEtodos */
    onChangeValue = (name, value) => {
        this.setState({
            [name]: value
        });
    };

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    permitirvalidacioncedula(e) {
        console.log(e)
        this.setState({ loadingqr: true })
        if (this.state.iseleccionado.Id > 0) {


            axios
                .post("/Accesos/Huella/ChangeValidacionCedula/", {
                    id: this.state.iseleccionado.Id
                })
                .then(response => {
                    if (response.data == "OK") {
                        this.setState({ checked: e.checked, loadingqr: false })
                        this.props.showSuccess("Validación por cédula actualizado");
                    } else {
                        this.setState({ loadingqr: false })
                        this.props.showWarning("Debe seleccionar un colaborador");
                    }

                })
                .catch(error => {
                    console.log(error);
                    this.setState({ loadingqr: false })
                    this.warnMessage("Algo salio mal!");
                });
        } else {
            this.props.showWarn("Ocurrió un error al actualizar campo validación");

        }
    }

    Guardar() {
        if (this.state.codigo_qr) {
            axios
                .post("/Accesos/Huella/UpdateColaboradorQR/", {
                    id: sessionStorage.getItem("id_colaborador"),
                    validacion: this.state.permiteValidacion
                })
                .then(response => {
                    this.showSuccess("Colaborador guardado con exito!");
                    this.Regresar();
                })
                .catch(error => {
                    console.log(error);
                    this.warnMessage("Algo salio mal!");
                });
        } else {
            this.warnMessage("Se debe generar el QR antes de guardar!");
        }
    }


    DescargarQR() {
        if (this.state.codigo_qr) {
            var element = document.createElement("a");
            element.setAttribute("href", document.getElementById("FPImage1").src);
            element.setAttribute(
                "download",
                this.state.iseleccionado != null
                    ? this.state.iseleccionado.nombres_apellidos + ".jpg"
                    : "QR.jpg"
            );

            element.style.display = "none";
            document.body.appendChild(element);

            element.click();

            document.body.removeChild(element);
        } else {
            this.warnMessage("Se debe generar el QR!");
        }
    }



    MostrarDialogQr(row) {
        console.log(row);

        this.setState({ iseleccionado: row, loadingqr: false, checked: row.validacion_cedula, QrDialog: true });

    }
    OcultarDialogQr() {
        this.setState({ QrDialog: false, iseleccionado: null });
        this.GetColaboradoreBuscar();
    }
    /********* */

    limpiarEstados() {
        this.setState({
            Identificacion: '',
            ApellidosNombres: '',
            Estado: '',
        })
    }
    formatLegajo(cell, row) {
        // console.log(row.Id, row.numero_legajo_temporal, row)
        if (row.numero_legajo_temporal != null) {
            var length = row.numero_legajo_temporal.length;
            switch (length) {
                case 1:
                    var numero = "0000" + row.numero_legajo_temporal;
                    return numero;
                case 2:
                    var numero = "000" + row.numero_legajo_temporal;
                    return numero;
                case 3:
                    var numero = "00" + row.numero_legajo_temporal;
                    return numero;
                case 4:
                    var numero = "0" + row.numero_legajo_temporal;
                    return numero;
                case 5:
                    var numero = row.numero_legajo_temporal;
                    return numero;
                default:
                    var numero = row.numero_legajo_temporal;
                    return numero;
            }
        }


    }

    GetColaboradores() {
        
    }

    GetColaboradoreBuscar() {
        if (this.state.Identificacion=== '' && this.state.ApellidosNombres=== '' && this.state.Estado === '') {
            this.props.showWarn(
                "Seleccione un Criterio de Búsqueda"
            );
        } else {
            this.props.blockScreen();

            axios
                .post("/Accesos/Huella/FilterSearch/", {
                    numeroIdentificacion: this.state.Identificacion,
                    nombres: this.state.ApellidosNombres,
                    estado: this.state.Estado
                })
                .then(response => {
                    console.log(response.data)
                    if (response.data.length == 0) {
                        this.setState({ loading: false, colaboradores: [] });
                        this.props.showWarn(
                            "No existe registros con la información ingresada"
                        );
                        this.props.unlockScreen();
                    } else {
                        this.setState({ loading: false, colaboradores: response.data });
                        this.props.unlockScreen();
                    }
                })
                .catch(error => {
                    console.log(error);
                    this.props.unlockScreen();
                });

        }
    }

    ConsultaEstados() {
        this.setState({ loading: true })
        axios.post("/Accesos/Huella/GetByCodeApi/?code=ESTADOSCOL", {})
            .then((response) => {
                var items = response.data.result.map(item => {
                    return { label: item.nombre, dataKey: item.Id, value: item.nombre };
                });
                this.setState({ tiposEstados: items })
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false })
            });
    }


    onChangeValue = (name, value) => {
        this.setState({
            [name]: value
        });
    };

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
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
    document.getElementById('content')
);