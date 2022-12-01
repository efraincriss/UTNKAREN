import axios from "axios";
import { Button } from 'primereact-v2/button';
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import React from "react";
import Field from "../../Base/Field-v2";
import http from "../../Base/HttpService";
import {
    FRASE_CAPACITACION_ELIMINADA,
    FRASE_ERROR_GLOBAL
} from "../../Base/Strings";
import { CapacitacionesTable } from './CapacitacionesTable.jsx';

export class CapacitacionesList extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            filtroColaborador: "",
            nombreCapacitacion: "",
            tipoCapacitacion: "",
            fechaDesde: "",
            fechaHasta: "",
            capacitaciones: [],
            errors: {},
        }
    }
    render() {
        return (
            <div className="card mb-8 p-4">
                <div className="card-body">
                    <h5 className="card-title">Búsqueda de Capacitaciones</h5>
                    <hr />
                    <div className="row">
                        <div className="col">
                            <div>
                                <div className="row">
                                    <div className="col">
                                        <Field
                                            name="filtroColaborador"
                                            label="Filtros de búsqueda"
                                            type="text"
                                            edit={true}
                                            readOnly={false}
                                            value={this.state.filtroColaborador}
                                            onChange={this.handleChange}
                                            error={this.state.errors.filtroColaborador}
                                            data-toggle="tooltip"
                                            data-placement="top"
                                            title={"Buscar por Nombres, Número Identificación o Número Empleado"}
                                            placeholder="Buscar..."
                                        />
                                    </div>
                                    <div className="col">
                                        <Field
                                            name="tipoCapacitacion"
                                            label="Tipo Capacitacion"
                                            type="select"
                                            options={this.props.catalogoTipoCapacitaciones}
                                            edit={true}
                                            readOnly={false}
                                            value={this.state.tipoCapacitacion}
                                            onChange={this.onChangeDropdown}
                                            error={this.state.errors.tipoCapacitacion}
                                            placeholder="Seleccionar..."
                                        />
                                    </div>
                                    <div className="col">
                                        <Field
                                            name="nombreCapacitacion"
                                            label="Nombre Capacitacion"
                                            type="select"
                                            options={this.props.catalogoNombreCapacitaciones}
                                            edit={true}
                                            readOnly={false}
                                            value={this.state.nombreCapacitacion}
                                            onChange={this.onChangeDropdown}
                                            error={this.state.errors.nombreCapacitacion}
                                            placeholder="Seleccionar..."
                                        />
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <Field
                                            name="fechaDesde"
                                            value={this.state.fechaDesde}
                                            label="Fecha Desde"
                                            type="date"
                                            edit={true}
                                            readOnly={false}
                                            onChange={this.handleChange}
                                            error={this.state.errors.fechaDesde}
                                        />
                                    </div>
                                    <div className="col">
                                        <Field
                                            name="fechaHasta"
                                            value={this.state.fechaHasta}
                                            label="Fecha Hasta"
                                            type="date"
                                            edit={true}
                                            readOnly={false}
                                            onChange={this.handleChange}
                                            error={this.state.errors.fechaHasta}
                                        />
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col">
                                    <button className="btn btn-outline-primary mr-4" onClick={this.handleSubmit}>
                                        Buscar
                                    </button>
                                    <button className="btn btn-outline-primary" onClick={this.limpiarFiltros}>
                                        Limpiar Filtros
                                    </button>
                                </div>
                            </div>
                            <hr />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <CapacitacionesTable
                                data={this.state.capacitaciones}
                                eliminarCapacitacion={this.mostrarEliminacionCapacitacion}
                            />
                        </div>

                    </div>
                    <Dialog header="Confirmación"
                        visible={this.state.mostrarConfirmacion}
                        modal style={{ width: '400px' }}
                        footer={this.construirBotonesDeConfirmacion()}
                        onHide={this.onHideConfirmacion}
                    >
                        <div className="confirmation-content">
                            <i className="pi pi-exclamation-triangle p-mr-3" style={{ fontSize: '2rem' }} />
                            <div className="p-12">
                                <h4>¿Estás seguro de eliminar el registro?</h4>
                            </div>
                        </div>
                    </Dialog>

                </div>
            </div>
        )
    }

    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    };

    onChangeDropdown = (name, value) => {
        this.setState({
            [name]: value,
        });
    };

    handleSubmit = () => {
        if (this.state.filtroColaborador === "" && this.state.nombreCapacitacion === "" && this.state.tipoCapacitacion === ""
        && this.state.fechaDesde === "" && this.state.fechaHasta === ""
        ) {
            this.props.showWarn("Debes ingresar almenos un filtro de búsqueda");
            return;
        }

        if(this.state.fechaDesde !== "" && this.state.fechaHasta !== "") {
            if (moment(this.state.fechaDesde) > moment(this.state.fechaHasta)) { 
                this.props.showWarn("La Fecha desde no puede ser mayor a la fecha hasta");
                return;
            }
        }

        
        this.props.blockScreen();

        axios
            .get("/RRHH/Capacitacion/ObtenerTodasLasCapacitaciones", {
                params: {
                    filtroColaborador: this.state.filtroColaborador,
                    tipoCapacitacion: this.state.tipoCapacitacion,
                    nombreCapacitacion: this.state.nombreCapacitacion,
                    fechaDesde: this.state.fechaDesde,
                    fechaHasta: this.state.fechaHasta
                }
            })
            .then(response => {
                console.log(response);
                let data = response.data;
                if (data.success === true) {
                    this.setState({ capacitaciones: data.result.DetalleCapacitaciones })
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

    limpiarFiltros = () => {
        this.setState({
            filtroColaborador: "",
            nombreCapacitacion: "",
            tipoCapacitacion: "",
            fechaDesde: "",
            fechaHasta: ""
        })
    }

    construirBotonesDeConfirmacion = () => {
        return (
            <div>
                <Button label="Cancelar" icon="pi pi-times" onClick={() => this.onHideConfirmacion()} className="p-button-text" />
                <Button label="Confirmar" icon="pi pi-check" onClick={() => this.eliminarCapacitacion()} autoFocus />
            </div>
        );
    }

    onHideConfirmacion = () => {
        this.setState({ mostrarConfirmacion: false, capacitacionAEliminar: 0 });
    }

    mostrarEliminacionCapacitacion = (capacitacionId) => {
        this.setState({ mostrarConfirmacion: true, capacitacionAEliminar: capacitacionId });
    }

    eliminarCapacitacion = () => {
        this.props.blockScreen();
        var capacitacionEliminadaId = this.state.capacitacionAEliminar;
        this.onHideConfirmacion();
        let url = '';
        url = `/RRHH/Capacitacion/EliminarCapacitacion/${capacitacionEliminadaId}`

        http.post(url, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    console.log(this.state.capacitacionAEliminar)
                    var nuevasCapacitaciones = this.state.capacitaciones.filter(o => o.Id !== capacitacionEliminadaId);
                    this.setState({ capacitaciones: nuevasCapacitaciones })
                    this.props.showSuccess(FRASE_CAPACITACION_ELIMINADA);
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.showWarn(message);
                }
                this.props.unlockScreen();

            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn(FRASE_ERROR_GLOBAL);
                this.props.unlockScreen();
            });
    }
}