import React, { Component } from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapperApi";
import config from '../../Base/Config'
import http from "../../Base/HttpService";
import Field from "../../Base/Field_";
import CabeceraTarifas from "./CabeceraTarifas";
import TarifasTable from "./TarifasTable";
import { Dialog } from 'primereact_/dialog';
import { Button } from 'primereact_/button';
import { FRASE_ERROR_VALIDACIONES } from "../../Base/Strings";
import validationRules from '../../Base/validationRules';
import { TIPOSERVICIOLAVANDERIA } from "../../Base/Constantes";
import { FRASE_ERROR_GLOBAL, MODULO_PROVEEDOR } from "../../Base/Strings";
import { Message } from 'primereact_/message';

class TarifaLavanderiaContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            tarifas: [],
            visible: false,
            displayBasic: false,
            contratoProveedorId: 0,
            tarifa: {},
            editable: false,



            Id: 0,
            catalogo_tipo_servicio: [],
            TipoServicioId: 0,
            valor_servicio: 0.00,
            estado: true,
            errors: {},
            action: "create"
        }

        this.onHide = this.onHide.bind(this);

    }
    renderFooter(name) {
        return (
            <div>
                <Button label="No" icon="pi pi-times" onClick={() => this.onHide(name)} className="p-button-text" />
                <Button label="Yes" icon="pi pi-check" onClick={() => this.onHide(name)} autoFocus />
            </div>
        );
    }
    onHide(name) {
        this.setState({
            [`${name}`]: false
        });
    }

    mostrarForm = (detalle) => {
        console.log("DetalleActionEdit", detalle);

        if (Object.keys(detalle).length === 0) {
            this.setState({ action: "create", Id: 0, visible: true, editable: true, TipoServicioId: 0, valor_servicio: 0.00 })

        } else {
            console.log("EDIT");
            this.setState({
                action: "edit",
                Id: detalle.Id,
                visible: true,
                editable: false,
                TipoServicioId: detalle.TipoServicioId,
                valor_servicio: detalle.valor_servicio,
                estado: detalle.estado
            });



        }
    }
    accept = () => {
        this.setState({ visible: false })
    }

    reject = () => {
        this.setState({ visible: false })
    }
    componentWillMount() {
        let url = window.location.href;
        let contratoProveedorId = url.substr(url.lastIndexOf('/') + 1);
        this.setState({ contratoProveedorId });
        this.consultarDatos();
        this.getTarifasLavanderia(contratoProveedorId);
    }
    consultarDatos = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.catalogoTipoServicio()])
            .then(function ([tipos]) {
                console.log(tipos)
                self.setState({
                    catalogo_tipo_servicio: self.buildDropdown(tipos.data, 'nombre', 'Id')
                })
            })
            .catch((error) => {

                self.props.unlockScreen();
                console.log(error);
            });
    }

    catalogoTipoServicio = () => {
        let url = '';
        url = `/Proveedor/TarifaLavanderia/SearchByCodeApi/?code=${TIPOSERVICIOLAVANDERIA}`;
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

    getTarifasLavanderia = contratoProveedorId => {
        this.props.blockScreen();
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/TarifaLavanderia/ListarPorContrato/${contratoProveedorId}`
        http.get(url, {})
            .then((response) => {
                let data = response.data;

                if (data.success === true) {
                    this.setState({ tarifas: data.result });
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
    ocultarForm = () => {
        this.setState({ visible: false, tarifa: {}, editable: false })
    }
    onChangeTipoServicio = (name, value) => {
        this.setState({
            TipoServicioId: value
        });
    }
    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }
    isValid() {
        const errors = {};

        if (this.state.TipoServicioId == 0) {
            errors.TipoServicioId = 'Tipo servicio obligatorio';
        }
        if (!validationRules["isFloat"]([], this.state.valor_servicio)) {
            errors.valor_servicio = 'valor por servicio debe ser un número';
        }



        if (this.state.valor_servicio <= 0) {
            errors.valor_servicio = 'El valor por servicio debe ser mayor a cero';
        }

        if (!validationRules["haveTwoDecimals"]([], this.state.valor_servicio)) {
            errors.valor_servicio = 'El valor por servicio debe tener máximo 3 enteros y 2 decimales';
        }

        if (this.state.valor_servicio > 1000.00) {
            errors.valor_servicio = 'El valor por servicio debe tener máximo 3 enteros y 2 decimales';
        }


        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    render() {
        return (
            <div>

                <CabeceraTarifas
                    data={this.props.data}
                    redireccionar={this.redireccionar}
                />
                <div className="row">
                    <div style={{ width: '100%' }}>
                        <div className="card">
                            <div className="card-body">
                                <div className="row" align="right">
                                    <div className="col">
                                        {this.state.tarifas.length === 0 &&
                                            <button
                                                style={{ marginLeft: '0.3em' }}
                                                className="btn btn-outline-primary"
                                                onClick={() => this.mostrarForm({})}
                                            >Nuevo</button>
                                        }
                                    </div>
                                </div>

                                <div className="row" style={{ marginTop: '1em' }}>
                                    <div className="col">
                                        <TarifasTable
                                            data={this.state.tarifas}

                                            eliminarTarifa={this.eliminarTarifa}
                                            detalleTarifa={this.mostrarForm}
                                            deleteTarifa={this.deleteTarifa}
                                            activarTarifa={this.activarTarifa}
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <Dialog header="Servicio" visible={this.state.visible} style={{ width: '50vw' }} onHide={this.ocultarForm}>
                    {this.state.action === "edit" &&
                        <div className="p-col-12 p-md-3">
                            <Message severity="warn" text="La tarifa afectará a todos los consumos registrados para el proveedor en el periodo de vigencia del contrato" />
                        </div>
                    }
                    <form onSubmit={this.handleSubmit}>
                        <div className="row">
                            <div className="col">
                                <Field
                                    name="TipoServicioId"
                                    required
                                    value={this.state.TipoServicioId}
                                    label="Tipo de Servicio"
                                    options={this.state.catalogo_tipo_servicio}
                                    type={"select"}
                                    onChange={this.onChangeTipoServicio}
                                    error={this.state.errors.TipoServicioId}
                                    readOnly={!this.state.editable}
                                    placeholder="Seleccione..."
                                    filterPlaceholder="Seleccione"

                                />
                            </div>
                        </div>

                        <div className="row">
                            <div className="col">
                                <Field
                                    name="valor_servicio"
                                    label="Valor Servicio"
                                    required
                                    edit={true}
                                    readOnly={false}
                                    value={this.state.valor_servicio}
                                    onChange={this.handleChange}
                                    error={this.state.errors.valor_servicio}
                                />
                            </div>
                        </div>

                        <button type="submit" className="btn btn-outline-primary">Guardar</button>&nbsp;

                        <button type="button" className="btn btn-outline-primary" onClick={this.ocultarForm}>Cancelar</button>&nbsp;
                    </form>
                </Dialog>


            </div>

        )
    }

    handleSubmit = (event) => {
        event.preventDefault();
        if (this.state.TipoServicioId === 0) {
            this.props.showWarn("Debe seleccionar un tipo de servicio");
        } else {
            if (!this.isValid()) {
                this.props.showWarn(FRASE_ERROR_VALIDACIONES);
                return;
            }
            let url = '';
            url = "/Proveedor/TarifaLavanderia"
            this.props.blockScreen();
            if (this.state.Id > 0) {
                url += "/Editar";
            } else {
                url += "/Create";
            }

            console.log(url);

            http
                .post(url, {
                    Id: this.state.Id,
                    TipoServicioId: this.state.TipoServicioId,
                    valor_servicio: this.state.valor_servicio,
                    estado: this.state.estado,
                    ContratoProveedorId: this.state.contratoProveedorId
                })
                .then((response) => {
                    let data = response.data;
                    if (data.success === true) {
                        this.props.showSuccess("Registrado Correctamente");
                        this.ocultarForm();
                        this.getTarifasLavanderia(this.state.contratoProveedorId);
                    } else {
                        var message = $.fn.responseAjaxErrorToString(data);
                        this.props.showWarn(message);
                        this.props.unlockScreen();
                    }
                    this.props.unlockScreen();
                })
                .catch((error) => {
                    console.log(error);
                    this.props.showWarn("Existe un inconveniente inténtalo más tarde");
                    this.props.unlockScreen();
                });
            this.props.unlockScreen();

        }
    }

    redireccionar = (accion, id) => {
        if (accion === "REGRESAR") {
            window.location.href = `/proveedor/Proveedor/Details/${id}`;
        }
    }

    eliminarTarifa = id => {
        this.props.blockScreen();
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/TarifaLavanderia/EliminarApi/` + id
        http.post(url, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.props.showSuccess("Estado actualizado correctamente");
                    this.getTarifasLavanderia(this.state.contratoProveedorId);
                } else {
                    this.props.unlockScreen();
                    this.props.showWarn(data.errors)
                    console.log(data.errors)
                }
            })
            .catch((error) => {
                this.props.unlockScreen();
                this.props.showWarn(FRASE_ERROR_GLOBAL)
                console.log(error)
            })
    }

    deleteTarifa = id => {
        this.props.blockScreen();
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/TarifaLavanderia/DeleteApi/` + id
        http.post(url, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.props.showSuccess("Estado actualizado correctamente");
                    this.getTarifasLavanderia(this.state.contratoProveedorId);
                } else {
                    this.props.unlockScreen();
                    this.props.showWarn(data.errors)
                    console.log(data.errors)
                }
            })
            .catch((error) => {
                this.props.unlockScreen();
                this.props.showWarn(FRASE_ERROR_GLOBAL)
                console.log(error)
            })
    }

    activarTarifa = id => {
        this.props.blockScreen();
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/TarifaLavanderia/ActivarTarifaApi/${id}`
        http.post(url, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.props.showSuccess("Estado actualizado correctamente");
                    this.getTarifasLavanderia(this.state.contratoProveedorId);
                } else {
                    this.props.unlockScreen();
                    this.props.showWarn(FRASE_ERROR_GLOBAL)
                    console.log(data.errors)
                }
            })
            .catch((error) => {
                this.props.unlockScreen();
                this.props.showWarn(FRASE_ERROR_GLOBAL)
                console.log(error)
            })
    }
}

const url = window.location.href;
const contratoProveedorId = url.substr(url.lastIndexOf('/') + 1);

const Container = Wrapper(TarifaLavanderiaContainer,
    `/proveedor/TarifaHotel/GetContratoInfo/${contratoProveedorId}`,
    {},
    false);

ReactDOM.render(
    <Container />,
    document.getElementById('tarifas_lavanderia')
);