import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import axios from 'axios';
import UploadPdfForm from "../Colaboradores/Bajas/UploadPdfForm";
import { Dialog } from 'primereact/components/dialog/Dialog';
import moment from 'moment';
import Field from "../Base/Field-v2";
import { FileUpload } from "primereact-v2/fileupload";
export default class Historicos extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: [],
            /* Colaboradores Ausentismos */
            uploadFile: '',
            uploadPDF: false,
            downloadPDF: false,
            ColaboradoresAusentismo: null,

            checkedausentismo: false,
            checkedreintegro: false,
            ArchivoAusentismoId: 0,
            ArchivoReintegroId: 0,

            //Edición Ausentismo
            vieweditar: false,
            ColaboradorAusentismoId: 0,
            observacion: '',
            fecha_inicio: null,
            fecha_fin: null,

            viewreintegro: false,

            errors: {},
            errorsr: {},

            //Reintegro

            fecha_reintegro: null,
            motivo_reintegro: '',
            uploadFileR: '',
            keyUpload: 98

        }
        this.EnviarFormulario = this.EnviarFormulario.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.isValid = this.isValid.bind(this);
        this.isValidR = this.isValidR.bind(this);
        this.loadReintegro = this.loadReintegro.bind(this);
        this.guardarReintegro = this.guardarReintegro.bind(this);
    }

    componentDidMount() {
        this.GetAusentismos();

    }
    isValid() {
        const errors = {};
        if (this.state.fecha_inicio === null) {
            errors.fecha_inicio = "Campo Requerido";
        }
        if (this.state.fecha_fin === null) {
            errors.fecha_fin = "Campo Requerido";
        }
        if (this.state.observacion === '') {
            errors.observacion = "Campo Requerido";
        }
        if (this.state.fecha_inicio != null && this.state.fecha_fin != null) {
            if (moment(this.state.fecha_fin) < moment(this.state.fecha_inicio)) {
                errors.fecha_fin = "Campo no puede ser menor a Fecha Inicio";
            }


        }

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }
    isValidR() {
        const errorsr = {};
        if (this.state.fecha_reintegro === null) {
            errorsr.fecha_reintegro = "Campo Requerido";
        }
        if (this.state.motivo_reintegro === '') {
            errorsr.motivo_reintegro = "Campo Requerido";
        }
        if (this.state.uploadFileR === '') {
            abp.notify.warn(
                "Documento de Respaldo Obligatorio",
                "Validación"
            );
        }


        this.setState({ errorsr });
        return Object.keys(errorsr).length === 0;
    }
    handleChange(event) {

        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
    }
    handleChangeFile = (event) => {
        if (event.files) {
            if (event.files.length > 0) {
                let uploadFile = event.files[0];

                this.setState({
                    uploadFileR: uploadFile
                });

            }
        } else {
            abp.notify.warn(
                "No se ha cargado ningun archivo.",
                "Validación"
            );
        }
    }
    DeleteAusentismos = (row) => {
        this.props.blockScreen();

        axios.post("/RRHH/ColaboradoresAusentismo/GetEliminarAusentismo/", {
            id: row.Id
        })
            .then((response) => {
                console.clear();
                console.log(response.data);
                if (response.data === "OK") {
                    this.GetAusentismos();
                    abp.notify.success(
                        "Eliminado Correctamente",
                        "Correcto"
                    );
                } else {
                    this.props.unlockScreen();
                    abp.notify.error(
                        "Existe un inconveniente inténtelo más tarde",
                        "Error"
                    );
                }


            })
            .catch((error) => {
                console.log(error);
                this.props.unlockScreen();
            });



    }
    GetAusentismos = () => {
        this.props.blockScreen();
        axios.post("/RRHH/Colaboradores/GetColaboradorAusentismos/", {
            id: this.props.colaborador.Id
        })
            .then((response) => {
                console.clear();
                console.log(response.data);
                console.log("Colaborador", this.props.colaborador)
                this.setState({ data: response.data })

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error);
                this.props.unlockScreen();
            });
    }
    Secuencial(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }
    mostrarEditar = row => {
        console.clear();
        console.log(row);
        this.setState({
            vieweditar: true,
            ColaboradoresAusentismo: row,
            fecha_inicio: row.fecha_inicio,
            fecha_fin: row.fecha_fin,
            observacion: row.observacion

        })
    }
    loadReintegro = row => {
        console.clear();
        console.log(row);
        this.setState({
            viewreintegro: true,
            ColaboradoresAusentismo: row
        })
    }


    EnviarFormulario(event) {
        this.setState({ loading: true })
        event.preventDefault();

        if (!this.isValid()) {
            abp.notify.error(
                "No ha ingresado los campos obligatorios  o existen datos inválidos.",
                "Validación"
            );
            return;
        } else {

            axios
                .post("/RRHH/ColaboradoresAusentismo/GetEditAusentimo", {
                    Id: this.state.ColaboradoresAusentismo.Id,
                    fecha_inicio: this.state.fecha_inicio,
                    fecha_fin: this.state.fecha_fin,
                    observacion: this.state.observacion,
                    catalogo_tipo_ausentismo_id: this.state.ColaboradoresAusentismo.catalogo_tipo_ausentismo_id,
                    colaborador_id: this.state.ColaboradoresAusentismo.colaborador_id,
                    estado: "ACTIVO"
                })
                .then(response => {

                    if (response.data == "EXISTE") {
                        abp.notify.error(
                            "Ya existe un ausentismo activo del mismo tipo en las misma fechas",
                            "ALERTA"
                        );
                        this.setState({ loading: false })

                    } else {

                        if (response.data == "OK") {
                            abp.notify.success("Ausetismo Editado", 'Aviso');
                            this.setState({ vieweditar: false, errors: {} });
                            this.GetAusentismos();

                        } else {
                            abp.notify.error('Existe un incoveniente intentelo más tarde', 'Error');
                            this.setState({ loading: false })
                        }
                    }

                })
                .catch(error => {
                    console.log(error);
                    abp.notify.error('Existe un incoveniente intentelo más tarde', 'Error');
                    this.setState({ loading: false })
                });


        }
    }
    guardarReintegro() {
        event.preventDefault();

        if (!this.isValidR()) {
            abp.notify.error(
                "No ha ingresado los campos obligatorios  o existen datos inválidos.",
                "Validación"
            );
            return;
        } else {
            this.props.blockScreen();
            console.log('file', this.state.uploadFileR);

            const formData = new FormData();
            formData.append('idAusentismo', this.state.ColaboradoresAusentismo.Id)
            formData.append('fecha_reintegro', this.state.fecha_reintegro)
            formData.append('motivo', this.state.motivo_reintegro)
            if (this.state.uploadFileR == '') {
                formData.append('UploadedFile', null)
            } else {
                formData.append('UploadedFile', this.state.uploadFileR)
            }
            const config = { headers: { 'content-type': 'multipart/form-data' } }
            axios.post("/RRHH/ColaboradoresAusentismo/CrearReintegro/", formData, config)
                .then((response) => {
                    this.props.unlockScreen();
                    this.props.showSuccess("Reintegro registrado Correctamente");
                    this.GetAusentismos();
                    this.setState({ viewreintegro: false })

                })
                .catch((error) => {
                    this.props.unlockScreen();
                    console.log(error);


                });
        }

    }



    mostrarUploadPDF = row => {

        this.setState({ uploadPDF: true, ColaboradoresAusentismo: row })
    }
    ocultarEditar = () => {
        this.setState({ vieweditar: false, ColaboradoresAusentismo: null, errors: {} })
    }
    ocultarReintegro = () => {
        this.setState({ viewreintegro: false, ColaboradoresAusentismo: null, errorsr: {} })
    }

    ocultarUploadPDF = () => {
        this.setState({ uploadPDF: false, uploadFile: '' })
    }

    onDownload = (row) => {

        if (row != null && row.requisitos != null && row.requisitos[0] != null && row.requisitos[0].archivo_id!=null && row.requisitos[0].archivo_id > 0) {

            return (
                window.location = `/RRHH/ColaboradoresAusentismo/GetArchivo/${row.requisitos[0].archivo_id}`
            );
        } else {

            this.props.showWarn("No posee Archivos Cargados");
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
        formData.append('Id', this.state.ColaboradoresAusentismo.requisitos[0].Id);
        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };

        console.log("Comienza archivo baja");

        axios.post("/RRHH/ColaboradoresAusentismo/GetChangeArchivo", formData, config)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({
                        uploadFile: ''
                    });
                    abp.notify.success("Documento Cargado Correctamente!", "Aviso");
                    this.ocultarUploadPDF()
                    this.GetAusentismos();
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


    generateButton(cell, row) {

        return (
            <div style={{ textAlign: 'center' }}>
                {row.estado === "ACTIVO" &&

                    <button title="Registrar Reintegro Anticipado" onClick={() => this.loadReintegro(row)} style={{ marginLeft: '0.3em' }} className="btn btn-outline-primary btn-sm fa fa-user"></button>

                }
                {//row.estado === "ACTIVO" &&
                    <button className="btn btn-outline-primary btn-sm" style={{ marginLeft: '0.3em' }}
                        onClick={() => this.mostrarEditar(row)}
                        data-toggle="tooltip"
                        data-placement="top"
                        title="Editar Documento">
                        <i className="fa fa-pencil"></i>
                    </button>
                }

                {//row.estado === "ACTIVO" &&
                    < button className="btn btn-outline-indigo btn-sm" style={{ marginLeft: '0.3em' }}
                        onClick={() => this.mostrarUploadPDF(row)}
                        data-toggle="tooltip"
                        data-placement="top"
                        title="Subir Documento">
                        <i className="fa fa-cloud-upload"></i>
                    </button>
                }
                {row != null && row.requisitos != null && row.requisitos[0] != null  && row.requisitos[0].archivo_id!=null && row.requisitos[0].archivo_id > 0 &&
                    <button className="btn btn-outline-indigo btn-sm" style={{ marginLeft: '0.3em' }}
                        onClick={() => this.onDownload(row)}
                        data-toggle="tooltip"
                        data-placement="top"
                        title="Descargar Documento">
                        <i className="fa fa-cloud-download"></i>
                    </button>
                }
                {//row.estado === "ACTIVO" &&
                    <button title="Eliminar" style={{ marginLeft: '0.3em' }}
                        onClick={() => { if (window.confirm('¿Está seguro de eliminar la información?')) this.DeleteAusentismos(row); }}
                        className="btn btn-outline-danger btn-sm fa fa-trash"></button>
                }



            </div >
        )
    }

    dateFormatter = (cell) => {
        console.log(cell);
        if (cell != null) {
            var d = (moment(new Date(cell).toLocaleDateString()).format("DD/MM/YYYY"));
            return d;
        }
        else {
            return "";
        }
    }

    onClear = () => {
        this.setState({ uploadFileR: '' })
    }
    render() {
        return (
            <div>
                <div className="row">
                    <div div className="col">

                        <div className="row" align="right">
                            <div className="col">

                                <button
                                    style={{ marginLeft: '0.3em' }}
                                    className="btn btn-outline-primary"
                                    onClick={() => this.props.RedireccionarLista()}
                                >Regresar</button>

                            </div>
                        </div>
                        <hr />

                        <div className="row">
                            <div className="col-xs-12 col-md-6">
                                <h6 className="text-gray-700"><b>Tipo Identificación:</b> {this.props.colaborador != null ? this.props.colaborador.nombre_identificacion : ""}</h6>
                                <h6 className="text-gray-700"><b>Nombres-Apellidos:</b>  {this.props.colaborador.nombres_apellidos ? this.props.colaborador.nombres_apellidos : ""}</h6>
                            </div>

                            <div className="col-xs-12 col-md-6">
                                <h6 className="text-gray-700"><b>Nro de Identificación:</b> {this.props.colaborador != null ? this.props.colaborador.numero_identificacion : ""}</h6>
                                <h6 className="text-gray-700"><b>Grupo Personal:</b> {this.props.colaborador != null ? this.props.colaborador.nombre_grupo_personal : ""}</h6>
                            </div>
                        </div>

                        <hr />
                        <div className="row">
                            <BootstrapTable
                                data={this.state.data}
                                hover={true}
                                pagination={true}
                                striped={false}
                                condensed={true}
                            >
                                <TableHeaderColumn
                                    dataField="any"
                                    dataFormat={this.Secuencial}
                                    width={"8%"}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                >
                                    Nº
                               </TableHeaderColumn>
                                <TableHeaderColumn

                                    dataField="nombre_ausentismo"
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Tipo Ausentismo</TableHeaderColumn>
                                <TableHeaderColumn dataField="formatFechaInicio"
                                    width={"10%"}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Fecha Desde</TableHeaderColumn>
                                <TableHeaderColumn
                                    width={"10%"}
                                    dataField="formatFechaFin"
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}

                                    headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Fecha Hasta</TableHeaderColumn>
                                <TableHeaderColumn
                                    width={"10%"}
                                    dataField="observacion"
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}

                                    headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Observación</TableHeaderColumn>

                                <TableHeaderColumn dataField="estado"
                                    width={"10%"}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Estado</TableHeaderColumn>
                                <TableHeaderColumn dataField='Id'
                                    width={"18%"}
                                    isKey
                                    headerAlign="center"
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    dataFormat={this.generateButton.bind(this)}>Opciones</TableHeaderColumn>
                            </BootstrapTable>

                        </div>
                        <Dialog header="Editar Archivo" visible={this.state.uploadPDF} width="500px" modal={true} onHide={this.ocultarUploadPDF}>

                            <UploadPdfForm
                                handleChange={this.handleChangeUploadFile}
                            />


                        </Dialog>
                        <Dialog header="Editar Ausentismo" visible={this.state.vieweditar} width="600px" modal={true} onHide={this.ocultarEditar}>

                            <div>
                                <form onSubmit={this.EnviarFormulario} height="730px">
                                    <div className="row">
                                        <div className="col">
                                            <h6 className="text-gray-700"><b>Tipo Identificación:</b> {this.props.colaborador != null ? this.props.colaborador.nombre_identificacion : ""}</h6>
                                            <h6 className="text-gray-700"><b>Nombres-Apellidos:</b>  {this.props.colaborador.nombres_apellidos ? this.props.colaborador.nombres_apellidos : ""}</h6>

                                        </div>
                                        <div className="col">
                                            <h6 className="text-gray-700"><b>Nro de Identificación:</b> {this.props.colaborador != null ? this.props.colaborador.numero_identificacion : ""}</h6>
                                            <h6 className="text-gray-700"><b>Tipo Ausentismo:</b> {this.props.ColaboradoresAusentismo != null ? this.props.ColaboradoresAusentismo.nombre_ausentismo : ""}</h6>

                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col">
                                            <Field
                                                name="fecha_inicio"
                                                label="Fecha Inicio"
                                                required
                                                type="date"
                                                edit={true}
                                                readOnly={false}
                                                value={this.state.fecha_inicio}
                                                onChange={this.handleChange}
                                                error={this.state.errors.fecha_inicio}
                                            />
                                        </div>
                                        <div className="col">
                                            <Field
                                                name="fecha_fin"
                                                label="Fecha Hasta"
                                                required
                                                type="date"
                                                edit={true}
                                                readOnly={false}
                                                value={this.state.fecha_fin}
                                                onChange={this.handleChange}
                                                error={this.state.errors.fecha_fin}
                                            />
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col">
                                            <Field
                                                name="observacion"
                                                label="Observación"
                                                required
                                                edit={true}
                                                readOnly={false}
                                                value={this.state.observacion}
                                                onChange={this.handleChange}
                                                error={this.state.errors.observacion}
                                            />
                                        </div>
                                    </div>

                                    <button type="submit" className="btn btn-outline-primary">
                                        Guardar
  </button>
                                    &nbsp;
        <button
                                        type="button"
                                        className="btn btn-outline-primary"
                                        icon="fa fa-fw fa-ban"
                                        onClick={this.ocultarEditar}
                                    >
                                        Cancelar
  </button>
                                </form>
                            </div>
                        </Dialog>
                        <Dialog header="Reintegro Anticipado" visible={this.state.viewreintegro} width="600px" modal={true} onHide={this.ocultarEditar}>

                            <div>
                                <form onSubmit={this.guardarReintegro} height="730px">
                                    <div className="row">
                                        <div className="col">
                                            <h6 className="text-gray-700"><b>Tipo Identificación:</b> {this.props.colaborador != null ? this.props.colaborador.nombre_identificacion : ""}</h6>
                                            <h6 className="text-gray-700"><b>Nombres-Apellidos:</b>  {this.props.colaborador.nombres_apellidos ? this.props.colaborador.nombres_apellidos : ""}</h6>

                                        </div>
                                        <div className="col">
                                            <h6 className="text-gray-700"><b>Nro de Identificación:</b> {this.props.colaborador != null ? this.props.colaborador.numero_identificacion : ""}</h6>
                                            <h6 className="text-gray-700"><b>Tipo Ausentismo:</b> {this.props.ColaboradoresAusentismo != null ? this.props.ColaboradoresAusentismo.nombre_ausentismo : ""}</h6>

                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col">
                                            <Field
                                                name="fecha_reintegro"
                                                label="Fecha Reintegro"
                                                required
                                                type="date"
                                                edit={true}
                                                readOnly={false}
                                                value={this.state.fecha_reintegro}
                                                onChange={this.handleChange}
                                                error={this.state.errorsr.fecha_reintegro}
                                            />
                                        </div>
                                        <div className="col">
                                            <div style={{ paddingTop: '10px' }}>
                                                <label>* Documento Respaldo</label>
                                            </div>
                                            <FileUpload
                                                chooseLabel="Seleccionar Archivo"
                                                cancelLabel="Cancelar"
                                                mode="basic"
                                                name="uploadFileR"
                                                accept=".xlsx,.xls,image/*,.doc, .docx,.ppt, .pptx,.txt,.pdf, .zip"
                                                onSelect={this.handleChangeFile}
                                            />
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col">
                                            <Field
                                                name="motivo_reintegro"
                                                label="Motivo Reintegro"
                                                required
                                                edit={true}
                                                readOnly={false}
                                                value={this.state.motivo_reintegro}
                                                onChange={this.handleChange}
                                                error={this.state.errorsr.motivo_reintegro}
                                            />
                                        </div>
                                    </div>

                                    <button type="button" onClick={() => { if (window.confirm('¿Está seguro que desea guardar la información?')) this.guardarReintegro(); }} className="btn btn-outline-primary"> Guardar</button>

                                    &nbsp;
<button
                                        type="button"
                                        className="btn btn-outline-primary"
                                        icon="fa fa-fw fa-ban"
                                        onClick={this.ocultarReintegro}
                                    >
                                        Cancelar
</button>
                                </form>
                            </div>
                        </Dialog>

                    </div>
                </div>
            </div>
        );
    }


}