
import { Checkbox } from "primereact-v2/checkbox"
import React, { Fragment } from "react";
import ReactDOM from 'react-dom';
import wrapForm from "../../Base/BaseWrapper";
import config from '../../Base/Config'
import http from "../../Base/HttpService";
import Field from "../../Base/Field_";
import { Dialog } from 'primereact_/dialog';
import { Card } from 'primereact_/card';
import { FRASE_ERROR_VALIDACIONES } from "../../Base/Strings";
import GeneralTable from "./GeneralTable";
import { PORTAFOLIOCERTIFICACION, UBICACIONCERTIFICACION } from "../../Base/Constantes";
import { FRASE_ERROR_GLOBAL, MODULO_CERTIFICACION_INGENIERIA } from "../../Base/Strings";

class ParametrizacionProyectosContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            proyectos: [],
            catalogo_portafolio: [],
            catalogo_ubicacion: [],

            proyecto: {},
            Id: 0,
            PortafolioId: 0,
            UbicacionId: 0,
            ProyectoCerrado: false,
            ProyectoCertificable: false,

            action: "crear",
            errors: {},

            vista: "lista"

        }
        this.onChange = this.onChange.bind(this)

    }



    mostrarForm = (o) => {
        console.log("ObjectoSeleccionado", o);

        if (Object.keys(o).length === 0) {
            console.log('ACTION CREATE');
            this.setState({
                proyecto: {},
                Id: 0,
                PortafolioId: 0,
                UbicacionId: 0,
                ProyectoCerrado: false,
                ProyectoCertificable: false,
                action: "crear",
                vista: "detalle",
                errors: {}
            })

        } else {
            console.log('ACTION EDIT');
            this.setState({
                action: "edit",
                vista: "detalle",
                proyecto: o,
                Id: o.Id,
                PortafolioId: o.PortafolioId,
                UbicacionId: o.UbicacionId,
                ProyectoCerrado: o.ProyectoCerrado,
                ProyectoCertificable:o.ProyectoCertificable,
            });
        }
    }
    regresar = () => {
        this.setState({ vista: "lista" });
    }


    componentWillMount() {

        this.consultarDatos();
        this.getProyectos();
    }
    consultarDatos = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.catalogoPortafolio(), this.catalogoUbicacion()])
            .then(function ([portafolio, ubicaciones]) {
                console.log(portafolio);
                console.log(ubicaciones)
                self.setState({
                    catalogo_portafolio: self.buildDropdown(portafolio.data, 'nombre', 'Id'),
                    catalogo_ubicacion: self.buildDropdown(ubicaciones.data, 'nombre', 'Id')
                })
            })
            .catch((error) => {

                self.props.unlockScreen();
                console.log(error);
            });
    }

    catalogoPortafolio = () => {
        let url = '';
        url = `/CertificacionIngenieria/Certificado/SearchByCodeApi/?code=${PORTAFOLIOCERTIFICACION}`;
        return http.get(url);
    }
    catalogoUbicacion = () => {
        let url = '';
        url = `/CertificacionIngenieria/Certificado/SearchByCodeApi/?code=${UBICACIONCERTIFICACION}`;
        return http.get(url);
    }
    buildDropdown = (data, nameField = 'name', valueField = 'Id') => {
        if (data.success === true) {

            return data.result.map(i => {
                return { label: i[nameField], value: i[valueField] }
            });
        } else if (data !== undefined) {

            return data.map(i => {
                return { label: i[nameField], value: i[valueField] }
            });
        }

        return {};
    }

    getProyectos = () => {
        this.props.blockScreen();
        let url = '';
        url = `${config.appUrl}${MODULO_CERTIFICACION_INGENIERIA}/Certificado/ListaProyectos`
        http.get(url, {})
            .then((response) => {
                let data = response.data;

                if (data.success === true) {
                    this.setState({ proyectos: data.result });
                    this.props.unlockScreen();
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    console.log(data)
                    this.props.unlockScreen();
                }


            })
            .catch((error) => {
                this.props.unlockScreen();
                console.log(error)
            });


    }


    onChange = (name, value) => {
        this.setState({
            [name]: value,
        })
    }
    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }
    handleChangeChecL = (event) => {
        console.log(event.target);
        const target = event.target;
        const value = target.type === "checkbox" ? target.checked : target.value;

        this.setState({
            ProyectoCerrado: value,
        });
    };
    handleChangeChecC = (event) => {
        console.log(event.target);
        const target = event.target;
        const value = target.type === "checkbox" ? target.checked : target.value;

        this.setState({
            ProyectoCertificable: value,
        });
    };
    isValid() {
        const errors = {};

        if (this.state.PortafolioId == 0) {
            errors.PortafolioId = 'Portafolio Obligatrio';
        }
        if (this.state.UbicacionId == 0) {
            errors.UbicacionId = 'Ubicacion Obligatrio';
        }
        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    render() {
        return (
            <div>
                <Card title="" subTitle="Personalización Proyecto">
                    {this.state.vista === "detalle" &&
                        <><div className="row">
                            <div className="col">
                                <div className="card border-default">
                                    <div className="card-header">Proyecto</div>
                                    <div className="card-body">
                                        <div className="row" >

                                            <div className="col" align="right">

                                                <button
                                                    style={{ marginLeft: '0.3em' }}
                                                    className="btn btn-outline-primary"
                                                    onClick={this.regresar}
                                                >Regresar</button>

                                            </div>
                                        </div>
                                        <hr></hr>
                                        <div className="row">
                                            <div className="col-6">
                                                <h6>
                                                    <b>Código: </b>{" "}
                                                    {this.state.proyecto != null
                                                        ? this.state.proyecto.codigo_proyecto
                                                        : ""}
                                                </h6>

                                            </div>
                                            <div className="col-6">
                                                <h6>
                                                    <b>Contrato: </b>{" "}
                                                    {this.state.proyecto != null
                                                        ? this.state.proyecto.codigo_contrato
                                                        : ""}
                                                </h6>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col">
                                                <h6>
                                                    <b>Nombre: </b>{" "}
                                                    {this.state.proyecto != null
                                                        ? this.state.proyecto.nombre_proyecto
                                                        : ""}
                                                </h6>

                                            </div>

                                        </div>
                                        <form onSubmit={this.handleSubmit}>
                                            <div className="row">
                                                <div className="col">
                                                    <Field
                                                        name="UbicacionId"

                                                        value={this.state.UbicacionId}
                                                        label="Ubicación"
                                                        options={this.state.catalogo_ubicacion}
                                                        type={"select"}
                                                        onChange={this.onChange}
                                                        error={this.state.errors.UbicacionId}

                                                        placeholder="Seleccione..."
                                                        filterPlaceholder="Seleccione"

                                                    />
                                                </div>
                                                <div className="col">
                                                    <Field
                                                        name="PortafolioId"

                                                        value={this.state.PortafolioId}
                                                        label="Portafolio"
                                                        options={this.state.catalogo_portafolio}
                                                        type={"select"}
                                                        onChange={this.onChange}
                                                        error={this.state.errors.UbicacionId}

                                                        placeholder="Seleccione..."
                                                        filterPlaceholder="Seleccione"

                                                    />
                                                </div>
                                                <div className="col">
                                                    <Fragment>
                                                        <label htmlFor="ab1" className="p-checkbox-label">
                                                            ¿Proyecto Cerrado?
                                                        </label>
                                                        <br></br>
                                                        <Checkbox
                                                            inputId="ab1"
                                                            checked={this.state.ProyectoCerrado}
                                                            onChange={(e) => this.handleChangeChecL(e)}
                                                            name="ProyectoCerrado"
                                                        />

                                                    </Fragment>
                                                </div>
                                                <div className="col">
                                                    <Fragment>
                                                        <label htmlFor="ab2" className="p-checkbox-label">
                                                            ¿Proyecto Certificable?
                                                        </label>
                                                        <br></br>
                                                        <Checkbox
                                                            inputId="ab2"
                                                            checked={this.state.ProyectoCertificable}
                                                            onChange={(e) => this.handleChangeChecC(e)}
                                                            name="ProyectoCertificable"
                                                        />

                                                    </Fragment>
                                                </div>
                                            </div>
                                            



                                            <button type="submit" className="btn btn-outline-primary">Guardar</button>&nbsp;

                                            <button type="button" className="btn btn-outline-primary" onClick={this.regresar}>Cancelar</button>&nbsp;
                                        </form>


                                    </div>
                                </div>
                            </div>
                        </div>
                            <hr></hr>
                        </>
                    }
                    {this.state.vista === "lista" &&
                        <>
                            <div className="row">
                                <div style={{ width: '100%' }}>
                                    <div className="card">
                                        <div className="card-body">


                                            <div className="row" style={{ marginTop: '1em' }}>
                                                <div className="col">
                                                    <GeneralTable
                                                        data={this.state.proyectos}
                                                        mostrarForm={this.mostrarForm}
                                                    />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </>
                    }
                </Card>



            </div>

        )
    }

    handleSubmit = (event) => {
        event.preventDefault();

        let url = '';
        url = `${config.appUrl}${MODULO_CERTIFICACION_INGENIERIA}/Certificado/ActualizarProyecto`
        this.props.blockScreen();

        console.log(url);

        http
            .post(url, {
                Id: this.state.proyecto.Id,
                PortafolioId: this.state.PortafolioId,
                UbicacionId: this.state.UbicacionId,
                ProyectoCerrado: this.state.ProyectoCerrado,
                ProyectoCertificable: this.state.ProyectoCertificable

            })
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.props.showSuccess("Actualizado Correctamente");

                    this.getProyectos();
                   // this.setState({ vista: "lista" });
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                    this.props.unlockScreen();
                }

            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn("Existe un inconveniente inténtalo más tarde");
                this.props.unlockScreen();
            });
        this.props.unlockScreen();


    }

    redireccionar = (accion, id) => {
        if (accion === "REGRESAR") {
            window.location.href = `/proveedor/Proveedor/Details/${id}`;
        }
    }





}

const Container = wrapForm(ParametrizacionProyectosContainer);
ReactDOM.render(
    <Container />,
    document.getElementById('content')
);