import axios from "axios";
import { FileUpload } from 'primereact-v2/fileupload';
import React from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapper";
import http from "../../Base/HttpService";
import {
    CONTROLLER_ACTUALIZACION_SUELDO,
    MODULO_RECURSOS_HUMANOS
} from "../../Base/Strings";

class ActualizacionColaboradoresContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            
        }
    }

    componentDidMount() {
        this.props.unlockScreen();
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
                                        onClick={() => this.descargarFormatoActualizacionMasivaDeColaboradores()}
                                    >
                                        Descargar formato carga masiva
                                </button>
                                    <button
                                        className="btn btn-outline-primary mr-4"
                                        type="button" data-toggle="collapse"
                                        data-target="#collapseExample"
                                        aria-expanded="false" aria-controls="collapseExample"
                                    >
                                        Actualizar Colaboradores
                                </button>
                                </div>
                            </div>
                        </div>
                        <div className="row pt-4">
                            <div className="col">
                                <div className="collapse" id="collapseExample">
                                    <div className="card card-body">
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

                <div className="card">
                    <div className="card-body">
                        <div className="row">
                            <div className="col">
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    descargarFormatoActualizacionMasivaDeColaboradores = () => {
        this.props.blockScreen()
        var formData = new FormData();
        axios
            .post(
                `/${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_ACTUALIZACION_SUELDO}/DescargarPlantillaActualizacionMasivaDeColaboradores`, formData,
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

        if (file.type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
            this.setState({ file });

            this.props.blockScreen();
            var formData = new FormData();
            formData.append("file", event.files[0]);

            axios.post(`/${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_ACTUALIZACION_SUELDO}/CargaMasivaDeActualizacionColaboradores
            
            `, formData, {
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
                    this.props.showSuccess("Archivo procesado exitosamente");
                    this.props.unlockScreen();

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
    onBasicUploadReingresos = (event) => {
        var file = event.files[0];

        if (file.type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
            this.setState({ file });

            this.props.blockScreen();
            var formData = new FormData();
            formData.append("file", event.files[0]);

            axios.post(`/${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_ACTUALIZACION_SUELDO}/CargaMasivaDeReingresosColaboradores
            
            `, formData, {
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
                    this.props.showSuccess("Archivo procesado exitosamente");
                    this.props.unlockScreen();

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

    
}

const Container = Wrapper(ActualizacionColaboradoresContainer);

ReactDOM.render(
    <Container />,
    document.getElementById('actualizacion_masiva_colaboradores')
);