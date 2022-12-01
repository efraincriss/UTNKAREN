import React from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapper";
import http from "../../Base/HttpService";
import axios from "axios";
import Field from "../../Base/Field-v2";
import ConsultaPublicaTable from "./ConsultaPublicaTable";
import CrearCandidatoForm from "./CrearCandidatoForm";
import { Dialog } from 'primereact-v2/dialog';
import UploadPdfForm from "./UploadPdfForm";
import { MultiSelect } from 'primereact-v2/multiselect';
import { Card } from 'primereact-v2/card';
import { FRASE_PDF_SUBIDO, FRASE_ERROR_GLOBAL, FRASE_ERROR_SELECCIONA_ARCHIVO } from "../../Base/Strings";


export default class CreateConsultaPublicaContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            data: [],
            identificacion: '',
            nombres: '',
            errors: {},
            createForm: false,
            uploadPDF: false,
            consultaPublicaId: 0,
            uploadFile: '',


            DialogDistribucion: false,
            ListaDistribucion: [],
            ListasSeleccionadas: [],

            //UserFoto
            Usuario: null,
            File: null,
            Upload: null,
            VerFoto: false,

            fechadesde: null,
            fechahasta: null,
            report: false,
            errorsR: {},
        }

        this.formRef = React.createRef();
        this.handleChangeFirma = this.handleChangeFirma.bind(this);
        this.ObtenerUserFirma = this.ObtenerUserFirma.bind(this);
        this.GuardarUserFirma = this.GuardarUserFirma.bind(this);
    }

    onHideReport = () => {
        this.setState({ report: false });

    }
    componentDidMount() {
        this.props.unlockScreen();
        this.ObtenerListasDistribucion()
        this.ObtenerUserFirma()

    }


    render() {

        const footer = <span>
            <input type="file" onChange={this.handleChangeFirma} accept="image/x-png,image/gif,image/jpeg" /> &nbsp;
            <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                onClick={() => this.GuardarUserFirma()}
            >
                Guardar
            </button>  &nbsp;
            <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                onClick={this.ocultarCambiarFoto}
            >
                Cancelar
            </button>
        </span>;


        return (
            <div className="row">
                <div style={{ width: '100%' }}>
                    <div className="card">
                        <div className="card-body">

                            <div className="row" align="right">
                                <div className="col">
                                    <button
                                        type="button"
                                        className="btn btn-outline-primary"
                                        icon="fa fa-fw fa-ban"
                                        onClick={this.openReporte}
                                    >
                                        Reporte
                                    </button>
                                    <button
                                        style={{ marginLeft: '0.3em' }}
                                        className="btn btn-outline-primary"
                                        onClick={() => this.mostrarCrearForm()}
                                    >Ingresar Candidato</button>

                                </div>
                            </div>
                            <hr />

                            <div className="row">
                                <div className="col">
                                    <form onSubmit={this.handleSubmit}>
                                        <div className="row">
                                            <div className="col">
                                                <Field
                                                    name="identificacion"
                                                    label="Identificación"
                                                    type="text"
                                                    edit={true}
                                                    readOnly={false}
                                                    value={this.state.identificacion}
                                                    onChange={this.handleChange}
                                                    error={this.state.errors.identificacion}
                                                />
                                            </div>
                                            <div className="col">
                                                <Field
                                                    name="nombres"
                                                    label="Nombres"
                                                    type="text"
                                                    edit={true}
                                                    readOnly={false}
                                                    value={this.state.nombres}
                                                    onChange={this.handleChange}
                                                    error={this.state.errors.nombres}
                                                />
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col">
                                                <button type="submit" className="btn btn-outline-primary">Buscar</button>&nbsp;
                                                <hr />
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <ConsultaPublicaTable
                                        data={this.state.data}
                                        mostrarCambiarFoto={this.mostrarCambiarFoto}
                                        ocultarCambiarFoto={this.ocultarCambiarFoto}
                                        Usuario={this.state.File}
                                        mostrarUploadPDF={this.mostrarUploadPDF}
                                        mostrarListaDistribucion={this.mostrarListaDistribucion}
                                        OcultarListaDistribucion={this.OcultarListaDistribucion}
                                        showSuccess={this.props.showSuccess}
                                        showWarn={this.props.showWarn}
                                    />

                                    <Dialog header="Registrar Candidato" visible={this.state.createForm} width="730px" modal={true} onHide={this.ocultarCreateForm}>
                                        <CrearCandidatoForm
                                            ref={this.formRef}
                                            showSuccess={this.props.showSuccess}
                                            showWarn={this.props.showWarn}
                                            blockScreen={this.props.blockScreen}
                                            unlockScreen={this.props.unlockScreen}
                                            buildDropdown={this.props.buildDropdown}
                                            ocultarCreateForm={this.ocultarCreateForm}

                                        />
                                    </Dialog>

                                    <Dialog header="Subir PDF" visible={this.state.uploadPDF} width="500px" modal={true} onHide={this.ocultarUploadPDF}>
                                        <UploadPdfForm
                                            handleChange={this.handleChangeUploadFile}
                                            label="Anexo 10"
                                        />
                                    </Dialog>
                                    <Dialog header="Seleccione ListaDistribución" visible={this.state.DialogDistribucion} width="600px" modal={true} onHide={this.OcultarListaDistribucion}>

                                        <MultiSelect value={this.state.ListasSeleccionadas} options={this.state.ListaDistribucion} onChange={(e) => this.setState({ ListasSeleccionadas: e.value })}
                                            style={{ minWidth: '100%' }} defaultLabel="Seleccione.." filter={true} placeholder="Seleccione" />

                                        <hr />
                                        <button
                                            type="button"
                                            className="btn btn-outline-primary"
                                            icon="fa fa-fw fa-ban"
                                            onClick={() => this.mostrarUploadPDF(this.state.consultaPublicaId)}
                                        >
                                            Siguiente
                                        </button>  &nbsp;
                                        <button
                                            type="button"
                                            className="btn btn-outline-primary"
                                            icon="fa fa-fw fa-ban"
                                            onClick={this.OcultarListaDistribucion}
                                        >
                                            Cancelar
                                        </button>  &nbsp;
                                    </Dialog>
                                    <Dialog header="Cambiar Firma" visible={this.state.VerFoto} width="580px" modal={true} onHide={this.ocultarCambiarFoto}>
                                        <Card footer={footer} >
                                            <div align="center" >
                                                <img src={this.state.File} height="260" width="280" />
                                            </div>
                                        </Card>


                                    </Dialog>

                                    <Dialog
                                        header="Nuevo"
                                        visible={this.state.report}
                                        onHide={this.onHideReport}
                                        modal={true}
                                        style={{ width: "400px", overflow: "auto" }}
                                    >
                                        <div>
                                            <form onSubmit={this.GetReporte}>
                                                <div className="row">
                                                    <div className="col">

                                                        <Field
                                                            name="fechadesde"
                                                            label="Desde"
                                                            //required
                                                            type="date"
                                                            edit={true}
                                                            readOnly={false}
                                                            value={this.state.fechadesde}
                                                            onChange={this.handleChange}
                                                            error={this.state.errorsR.fechadesde}
                                                        />

                                                        <Field
                                                            name="fechahasta"
                                                            label="Hasta"
                                                            // required
                                                            type="date"
                                                            edit={true}
                                                            readOnly={false}
                                                            value={this.state.fechahasta}
                                                            onChange={this.handleChange}
                                                            error={this.state.errorsR.fechahasta}
                                                        />


                                                    </div>
                                                </div>
                                                <button type="submit" className="btn btn-outline-primary">
                                                    Generar
                                                </button>
                                                &nbsp;
                                                <button
                                                    type="button"
                                                    className="btn btn-outline-primary"
                                                    icon="fa fa-fw fa-ban"
                                                    onClick={this.onHideReport}
                                                >
                                                    Cancelar
                                                </button>
                                            </form>

                                        </div>

                                    </Dialog>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
    isValid = () => {
        const errorsR = {};
        if (this.state.fechadesde != null || this.state.fechahasta != null) {
            if (this.state.fechadesde == null) {
                errorsR.fechadesde = "Campo Requerido";
            }
            if (this.state.fechahasta == null) {
                errorsR.fechahasta = "Campo Requerido";
            }
            if (this.state.fechahasta != null && this.state.fechadesde != null && this.state.fechahasta < this.state.fechadesde) {
                errorsR.fechahasta = "Fecha Hasta debe ser mayor a la fecha desde";

            }
        }
        this.setState({ errorsR });
        return Object.keys(errorsR).length === 0;
    };

    openReporte = () => {
        this.setState({ report: true })
    }

    GetReporte = (event) => {
        event.preventDefault();
        if (!this.isValid()) {
            abp.notify.error(
                "No ha ingresado los campos obligatorios  o existen datos inválidos.",
                "Validación"
            );
            return;
        } else {

            this.props.blockScreen();
            axios
                .get("/Accesos/ConsultaPublica/GetReporte", {
                    params: {
                        desde: this.state.fechadesde,
                        hasta: this.state.fechahasta,
                    },
                    responseType: "arraybuffer",
                })
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
                    this.props.showSuccess("Reporte POS generado correctamente");
                    this.props.unlockScreen();
                })
                .catch((error) => {
                    console.log(error);
                    this.props.showWarn(error);
                    this.props.unlockScreen();
                });
        }
    };


    ObtenerListasDistribucion() {
        axios
            .post("/Accesos/ConsultaPublica/ObtenerListasDistribucion", {})
            .then(response => {
                var items = response.data.result.map(item => {
                    return { label: item.nombre, dataKey: item.Id, value: item.Id };
                });
                this.setState({ ListaDistribucion: items });

            })
            .catch(error => {
                console.log(error);

            });
    }
    ObtenerUserFirma() {
        axios
            .post("/Accesos/ConsultaPublica/ObtenerFotoUsuario", {})
            .then(response => {
                if (response.data == "NOFOTO") {
                    this.setState({ File: null });
                } else {
                    this.setState({ File: "data:image/jpg;base64," + response.data });
                }


            })
            .catch(error => {
                console.log(error);

            });
    }
    GuardarUserFirma() {
        this.props.blockScreen();

        if (this.state.Upload != null) {
            let url = '';
            url = `/Accesos/ConsultaPublica/GuardarFirmaUsuario/`;

            const formData = new FormData();
            formData.append('uploadFile', this.state.Upload);
            const config = {
                headers: {
                    'content-type': 'multipart/form-data'
                }
            };

            http.post(url, formData, config)
                .then((response) => {
                    if (response.data != "ERROR") {

                        this.setState({
                            File: null,
                            Upload: null
                        });
                        this.props.showSuccess("Firma Actualizada")
                        this.ocultarCambiarFoto()
                        this.ObtenerUserFirma()
                    } else {

                        this.props.showWarn("Ocurrio un Error");
                    }
                    this.props.unlockScreen();
                })
                .catch((error) => {
                    console.log(error);
                    this.props.showWarn(FRASE_ERROR_GLOBAL);
                    this.props.unlockScreen();
                });
        } else {
            this.props.showWarn("Seleccione una imagen para actualizar");
            this.props.unlockScreen();
        }
    }

    handleSubmit = (event) => {
        event.preventDefault();
        this.props.blockScreen();
        var identificacion = this.state.identificacion;
        var nombres = this.state.nombres;
        let url = '';
        url = `/Accesos/ConsultaPublica/BuscarConsultaPublica?identificacion=${identificacion}&nombres=${nombres}`
        http.get(url)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({ data: data.result })
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
    }

    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }
    handleChangeFirma(event) {
        this.setState({
            File: URL.createObjectURL(event.target.files[0]),
            Upload: event.target.files[0]
        })
    }


    handleSubmitUploadFile = () => {
        this.props.blockScreen();
        let url = '';
        url = `/Accesos/ConsultaPublica/SubirArchivoArchivo`;

        const formData = new FormData();
        formData.append('uploadFile', this.state.uploadFile);
        formData.append('Id', this.state.consultaPublicaId);
        formData.append('listadistribucionids', this.state.ListasSeleccionadas);

        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };

        http.post(url, formData, config)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({
                        uploadFile: ''
                    });
                    this.props.showSuccess(FRASE_PDF_SUBIDO)
                    this.ocultarUploadPDF()
                    this.OcultarListaDistribucion()
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn(FRASE_ERROR_GLOBAL);
                this.props.unlockScreen();
            });

    }


    handleChangeUploadFile = event => {
        if (event.files) {
            if (event.files.length > 0) {
                let uploadFile = event.files[0];
                this.setState({
                    uploadFile: uploadFile
                }, this.handleSubmitUploadFile);
            } else {
                this.props.showWarn(FRASE_ERROR_SELECCIONA_ARCHIVO);
            }
        } else {
            this.props.showWarn(FRASE_ERROR_SELECCIONA_ARCHIVO);
        }
    }

    resetFormData = () => {
        this.formRef.current.resetDatos();
    }

    ocultarCreateForm = () => {
        this.setState({ createForm: false }, this.resetFormData)
    }

    mostrarCrearForm = () => {
        this.setState({ createForm: true })
    }

    ocultarUploadPDF = () => {
        this.setState({ uploadPDF: false, uploadFile: '' })
    }

    mostrarUploadPDF = consultaPublicaId => {
        this.setState({ uploadPDF: true, consultaPublicaId })
    }
    mostrarCambiarFoto = () => {
        this.setState({ VerFoto: true })
    }
    ocultarCambiarFoto = () => {
        this.setState({ VerFoto: false, File: null, Upload: null, })
        this.ObtenerUserFirma()

    }
    mostrarListaDistribucion = cell => {
        this.setState({ DialogDistribucion: true, consultaPublicaId: cell })
    }
    OcultarListaDistribucion = cell => {
        this.setState({ DialogDistribucion: false, consultaPublicaId: 0, ListasSeleccionadas: [] })
    }
}

const Container = Wrapper(CreateConsultaPublicaContainer);

ReactDOM.render(
    <Container />,
    document.getElementById('create_consulta_publica')
);