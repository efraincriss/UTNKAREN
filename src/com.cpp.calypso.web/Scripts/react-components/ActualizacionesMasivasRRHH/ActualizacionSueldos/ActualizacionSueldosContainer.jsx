import axios from "axios";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { FileUpload } from 'primereact-v2/fileupload';
import React from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapper";
import http from "../../Base/HttpService";
import {
    CONTROLLER_ACTUALIZACION_SUELDO,
    MODULO_RECURSOS_HUMANOS
} from "../../Base/Strings";
import { ActualizacionSueldoTable } from './ActualizacionSueldosTable.jsx';
import { DetalleActualizacionSueldoTable } from "./DetalleActualizacionSueldosTable.jsx";
import Field from "../../Base/Field-v2";

class ActualizacionSueldosContainer extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            actualizacionSueldosListado: [],
            mostrarTablaDetalles: false,
            detalleActualizacionSueldos: [],
            errors: {},
            Observaciones: '',
            FechaInicio: new Date().toISOString().split('T')[0]
        }
    }

    componentDidMount() {
        this.loadData();
    }

    render() {
        return (
            <div>
                <div className="card">
                    <div className="card-body">

                        <div className="row">
                            <div className="col">
                                <div>
                                    <button className="btn btn-outline-primary mr-4"
                                        onClick={() => this.descargarFormatoCargaMasivaDeJornales()}
                                    >
                                        Descargar formato carga masiva
                                </button>
                                    <button
                                        className="btn btn-outline-primary mr-4"
                                        type="button" data-toggle="collapse"
                                        data-target="#collapseExample"
                                        aria-expanded="false" aria-controls="collapseExample"
                                    >
                                        Cargar Sueldos Jornales
                                </button>
                                </div>
                            </div>
                        </div>
                        <div className="row pt-4">
                            <div className="col">
                                <div className="collapse" id="collapseExample">
                                    <div className="card card-body">
                                    <div className="row">
                                            <div className="col">
                                                <Field
                                                    name="FechaInicio"
                                                    value={this.state.FechaInicio}
                                                    label="Fecha Inicio"
                                                    type="date"
                                                    onChange={this.handleChange}
                                                    error={this.state.errors.FechaInicio}
                                                    edit={true}
                                                    readOnly={false}
                                                    max={new Date().toISOString().split('T')[0]}
                                                />
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col">
                                                <Field
                                                    name="Observaciones"
                                                    value={this.state.Observaciones}
                                                    label="Observaciones"
                                                    type="textarea"
                                                    onChange={this.handleChange}
                                                    error={this.state.errors.Observaciones}
                                                    edit={true}
                                                    readOnly={false}
                                                />
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col">
                                                <FileUpload name="uploadedFile"
                                                    chooseLabel="Seleccionar"
                                                    cancelLabel="Cancelar"
                                                    uploadLabel="Cargar"
                                                    onUpload={this.onBasicUpload}
                                                    accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                                                    maxFileSize={1000000}
                                                />
                                            </div>
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div className="card">
                    <div className="card-body">
                        <div className="row">
                            <div className="col">
                                <ActualizacionSueldoTable
                                    data={this.state.actualizacionSueldosListado}
                                    mostrarDetalles={this.mostrarDetalles}
                                />
                            </div>
                        </div>
                    </div>
                </div>

                <Dialog
                    header="Detalles de Actualización de Sueldo"
                    modal={true}
                    visible={this.state.mostrarTablaDetalles}
                    style={{ width: "750px" }}
                    onHide={this.oncultarTablaDetalles}
                >
                    <DetalleActualizacionSueldoTable
                        data={this.state.detalleActualizacionSueldos}
                    />
                </Dialog>
            </div>
        )
    }

    descargarFormatoCargaMasivaDeJornales = () => {
        this.props.blockScreen()
        var formData = new FormData();
        axios
            .post(
                `/${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_ACTUALIZACION_SUELDO}/DescargarPlantillaCargaMasivaDeJornales`, formData,
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
                this.props.showSuccess("Formato descargado exitosamente")
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn("Ocurrió un error al descargar el archivo, intentalo nuevamente");
                this.props.unlockScreen();
            });
    }

    onBasicUpload = (event) => {
        var file = event.files[0];

        if (this.state.Observaciones === "") {
            this.props.showWarn("El campo observaciones es requerido");
            return;
        } else if(this.state.FechaInicio === ""){
            this.props.showWarn("El campo fecha inicio es requerido");
            return;
        }

        if (file.type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
            this.setState({ file });

            this.props.blockScreen();
            var formData = new FormData();
            formData.append("file", event.files[0]);
            formData.append("observaciones", this.state.Observaciones);
            formData.append("fecha", this.state.FechaInicio);

            axios.post(`/${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_ACTUALIZACION_SUELDO}/CargaMasivaDeSueldosJornales`, formData, {
                headers:
                {
                    'Content-Disposition': "attachment; filename=template.xlsx",
                    'Content-Type': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                },
                responseType: 'arraybuffer',
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
                    this.loadData();
                    this.props.showSuccess("Archivo procesado exitosamente");
                    this.setState({Observaciones: "", FechaInicio: new Date().toISOString().split('T')[0]});
                    
                })
                .catch((error) => {
                    console.log(error);
                    this.props.showWarn("Ocurrió un error al subir el archivo, intentalo nuevamente")
                    this.props.unlockScreen();
                });
        } else {
            this.props.showWarn("El formato de archivo es incorrecto")
        }
    }

    loadData = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.obtenerTodasLasActualizacionesDeSueldo()])
            .then(function ([response]) {

                let data = response.data;
                if (data.success === true) {
                    self.setState({
                        actualizacionSueldosListado: data.result,
                    })
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    console.log(message);
                    this.props.showWarn("Ocurrió un error, intentalo más tarde.");
                }

                self.props.unlockScreen();
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });

    }

    mostrarDetalles = (detalles) => {
        this.setState({
            detalleActualizacionSueldos: detalles,
            mostrarTablaDetalles: true
        })
    }

    oncultarTablaDetalles = () => {
        this.setState({ mostrarTablaDetalles: false, detalleActualizacionSueldos: [] });
    }

    obtenerTodasLasActualizacionesDeSueldo = () => {
        let url = '';
        url = `/RRHH/ActualizacionSueldo/ObtenerTodasLasActualizacionesDeSaldos`;
        return http.get(url);
    }

    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }

}

const Container = Wrapper(ActualizacionSueldosContainer);

ReactDOM.render(
    <Container />,
    document.getElementById('carga_masiva_de_sueldos')
);