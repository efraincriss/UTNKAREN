import axios from "axios";
import { FileUpload } from 'primereact-v2/fileupload';
import React from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapper";
import Field from "../../Base/Field-v2";
import http from "../../Base/HttpService";
import {
    CONTROLLER_CAPACITACIONES,
    MODULO_RECURSOS_HUMANOS
} from "../../Base/Strings";
import { CapacitacionesList } from './CapacitacionesList.jsx';
import { ColaboradoresSeleccionadosTable } from './ColaboradoresSeleccionadosTable.jsx';
import { ColaboradorTable } from './ColaboradorTable.jsx';

class ListadoColaboradoresContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            filtro: "",
            estado: "",
            colaboradores: [],
            colaboradoresSeleccionados: [],
            errors: {},
            estado: "",
            file: "",
            colaboradorEstado: [
                {
                    label: "ACTIVO ",
                    dataKey: 1,
                    value: "ACTIVO ",
                    selected: true
                },
                {
                    label: "INACTIVO",
                    dataKey: 2,
                    value: "INACTIVO",
                },
                {
                    label: "TEMPORAL",
                    dataKey: 3,
                    value: "TEMPORAL",
                },
                {
                    label: "ENVIADO A SAP",
                    dataKey: 4,
                    value: "ENVIADO A SAP",
                },
            ],
            catalogoNombreCapacitaciones: [],
            catalogoTipoCapacitaciones: []

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
                                        onClick={() => this.descargarFormatoCargaMasivaCapacitaciones()}
                                    >
                                        Descargar formato carga masiva
                                    </button>
                                    <button
                                        className="btn btn-outline-primary mr-4"
                                        type="button" data-toggle="collapse"
                                        data-target="#collapseExample"
                                        aria-expanded="false" aria-controls="collapseExample"
                                    >
                                        Carga masiva de capacitaciones
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
                                            maxFileSize={99999999999999}
                                        />

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="row">
                    <div style={{ 'marginRight': '8px', 'marginLeft': '14px' }}>
                        <ul className="nav nav-tabs" id="colaboradores-tab" role="tablist">
                            <li className="nav-item">
                                <a className="nav-link active" id="colaboradores-tab" data-toggle="tab" href="#colaboradores" role="tab" aria-controls="home" aria-expanded="true">Colaboradores</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" id="capacitaciones-tab" data-toggle="tab" href="#capacitaciones" role="tab" aria-controls="capacitaciones">Capacitaciones</a>
                            </li>
                        </ul>


                        <div className="tab-content" id="myTabContent">

                            <div className="tab-pane fade show active" id="colaboradores" role="tabpanel" aria-labelledby="colaboradores-tab">
                                <div className="card mb-8 p-4">
                                    <div className="card-body">
                                        <h5 className="card-title">Búsqueda de Colaboradores</h5>
                                        <hr />
                                        <div className="row">
                                            <div className="col">
                                                <form onSubmit={this.handleSubmit}>
                                                    <div className="row">
                                                        <div className="col">
                                                            <Field
                                                                name="filtro"
                                                                label="Filtros de búsqueda"
                                                                type="text"
                                                                edit={true}
                                                                readOnly={false}
                                                                value={this.state.filtro}
                                                                onChange={this.handleChange}
                                                                error={this.state.errors.filtro}
                                                                data-toggle="tooltip"
                                                                data-placement="top"
                                                                title={"Buscar por Nombres, Número Identificación o Número Empleado"}
                                                                placeholder="Buscar..."
                                                            />
                                                        </div>
                                                        <div className="col">
                                                            <Field
                                                                name="estado"
                                                                label="Estado"
                                                                type="select"
                                                                options={this.state.colaboradorEstado}
                                                                edit={true}
                                                                readOnly={false}
                                                                value={this.state.estado}
                                                                onChange={this.onChangeDropdown}
                                                                error={this.state.errors.estado}
                                                                placeholder="Seleccionar..."
                                                            />
                                                        </div>
                                                    </div>
                                                    <div className="row">
                                                        <div className="col">
                                                            <button type="submit" className="btn btn-outline-primary">
                                                                Buscar
                                                            </button>
                                                        </div>
                                                    </div>
                                                </form>
                                                <hr />
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col">
                                                <ColaboradorTable
                                                    data={this.state.colaboradores}
                                                    agregarColaborador={this.agregarColaborador}
                                                    imprimirCertificadoDeUnColaborador={this.imprimirCertificadoDeUnColaborador}
                                                />
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <br />
                                <br />
                                <div className="card mt-8">
                                    <div className="card-body">
                                        <h5 className="card-title">Colaboradores agregados para impresión de certificados</h5>
                                        <hr />
                                        <div className="row">
                                            <div className="col">
                                                <div>
                                                    <button className="btn btn-outline-primary mr-4"
                                                        onClick={() => this.imprimirCertificadosMasivos()}
                                                    >
                                                        Imprimir Capacitaciones
                                                     </button>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col">
                                                <ColaboradoresSeleccionadosTable
                                                    data={this.state.colaboradoresSeleccionados}
                                                    eliminarColaborador={this.eliminarColaborador}
                                                />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div className="tab-pane fade" id="capacitaciones" role="tabpanel" aria-labelledby="capacitaciones-tab">
                                <CapacitacionesList
                                    catalogoNombreCapacitaciones={this.state.catalogoNombreCapacitaciones}
                                    catalogoTipoCapacitaciones={this.state.catalogoTipoCapacitaciones}
                                    unlockScreen={this.props.unlockScreen}
                                    blockScreen={this.props.blockScreen}
                                    showSuccess={this.props.showSuccess}
                                    showWarn={this.props.showWarn}
                                    eliminarCapacitacion={this.mostrarEliminacionCapacitacion}
                                />
                            </div>

                            

                        </div>
                    </div>
                </div>


            </div>
        )
    }

    agregarColaborador = (colaborador) => {

        let colaboradoresState = this.state.colaboradoresSeleccionados;
        let yaAgregado = colaboradoresState.filter(e => e.Id == colaborador.Id);
        if (yaAgregado.length > 0) {
            this.props.showWarn(`El colaborador ${colaborador.nombres_apellidos} ya se encuentra agregado`)
        } else {
            colaboradoresState.push(colaborador)
            this.setState({ colaboradoresSeleccionados: colaboradoresState });
            this.props.showSuccess(`Colaborador ${colaborador.nombres_apellidos} agregado`)
        }

    }

    eliminarColaborador = (colaborador) => {
        let colaboradoresState = this.state.colaboradoresSeleccionados;
        colaboradoresState = colaboradoresState.filter(e => e.Id !== colaborador.Id)
        this.setState({ colaboradoresSeleccionados: colaboradoresState });
        this.props.showSuccess(`Colaborador ${colaborador.nombres_apellidos} removido`)
    }


    handleSubmit = (event) => {
        event.preventDefault();
        /*if (this.state.filtro === "" ) {
            this.props.showWarn("Debes ingresar almenos un filtro de búsqueda");
            return;
        }*/
        this.props.blockScreen();

        axios
            .get("/RRHH/Capacitacion/Search", {
                params: {
                    estado: this.state.estado,
                    filtro: this.state.filtro,
                }
            })
            .then(response => {
                console.log(response);
                let data = response.data;
                if (data.success === true) {
                    this.setState({ colaboradores: data.result })
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

    imprimirCertificadoDeUnColaborador = (colaborador) => {
        this.props.blockScreen();
        var colaboradores = [];
        colaboradores.push(colaborador.Id);
        this.imprimir(colaboradores);
    }

    imprimirCertificadosMasivos = () => {
        if (this.state.colaboradoresSeleccionados.length === 0) {
            this.props.showWarn("Debes agregar al menos un colaborador en la lista de impresión");
        } else {
            let parametros = '';

            this.props.blockScreen();

            this.state.colaboradoresSeleccionados.forEach(e => {
                parametros += `colaboradores=${e.Id}&`
            });

            var arrayColaboradores = this.state.colaboradoresSeleccionados.map(c => c.Id);
            this.imprimir(arrayColaboradores);  
        }
    }

    imprimir = (arrayColaboradores) => {
        let url = `/RRHH/Capacitacion/GenerarCertificados`

        axios.post(url, 
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
                this.props.showSuccess("Certificados generados exitosamente")
                this.setState({ colaboradoresSeleccionados: [] })
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn("Ocurrió un error al descargar el archivo, intentalo nuevamente");
                this.props.unlockScreen();
            });
    }

    descargarPlantilla = (consultaPublicaId) => {
        if (this.props.Usuario != null) {
            window.location.href = `${config.appUrl}/Accesos/ConsultaPublica/DescargarPlantilla/${consultaPublicaId}`
        } else {

            this.props.showWarn("Debe actualizar la firma del Usuario para poder descargar el documento")
        }
    }

    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    };

    onChangeDropdown = (name, value) => {
        this.setState({
            [name]: value,
        });
    };

    descargarFormatoCargaMasivaCapacitaciones() {
        this.props.blockScreen()
        var formData = new FormData();
        axios
            .post(
                `/${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_CAPACITACIONES}/DescargarPlantillaCargaMasivaDeCapacitaciones/`, formData,
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

        var a = {};

        if (file.type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
            this.setState({ file });

            this.props.blockScreen();
            var formData = new FormData();
            formData.append("file", event.files[0]);

            axios.post(`/${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_CAPACITACIONES}/CargaMasivaDeCapacitaciones`, formData, {
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

    loadData = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.obtenerCatalogoDeCapacitaciones()])
            .then(function ([response]) {

                let data = response.data;
                if (data.success === true) {

                    let catalogoNombreCapacitaciones = self.props.buildDropdown(data.result.CatalogoNombreCapacitacion, 'nombre');
                    let catalogoTipoCapacitaciones = self.props.buildDropdown(data.result.CatalogoTipoCapacitacion, 'nombre');
                    catalogoNombreCapacitaciones.unshift({label: "Todos", value: ""});
                    catalogoTipoCapacitaciones.unshift({label: "Todos", value: ""});
                    self.setState({
                        catalogoNombreCapacitaciones,
                        catalogoTipoCapacitaciones,
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

    obtenerCatalogoDeCapacitaciones = () => {
        let url = '';
        url = `/${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_CAPACITACIONES}/ObtenerCatalogosDeCapacitaciones`;
        return http.get(url);
    }

    



}

const Container = Wrapper(ListadoColaboradoresContainer);

ReactDOM.render(
    <Container />,
    document.getElementById('capacitaciones_colaboradores_listado')
);