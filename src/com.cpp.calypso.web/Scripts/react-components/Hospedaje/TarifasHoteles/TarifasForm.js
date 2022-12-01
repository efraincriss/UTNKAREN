import React from 'react';
import { TIPO_HABITACION } from "../../Base/Constantes";
import http from "../../Base/HttpService";
import Field from "../../Base/Field-v2";
import { Message } from 'primereact-v2/message';

import {
    FRASE_ERROR_SELECCIONA_UN_TIPO_HABITACION,
    FRASE_TARIFA_CREADA,
    FRASE_TARIFA_ACTUALIZADA,
    FRASE_ERROR_VALIDACIONES
} from "../../Base/Strings";
import validationRules from '../../Base/validationRules';


export default class TarifasForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            Id: props.tarifa.Id ? props.tarifa.Id : 0,
            catalogo_tipo_habitacion: [],
            tipo_habitacion_id: props.tarifa.TipoHabitacionId ? props.tarifa.TipoHabitacionId : 0,
            capacidad: props.tarifa.capacidad ? props.tarifa.capacidad : 0,
            costo_persona: props.tarifa.costo_persona ? props.tarifa.costo_persona : 0.0,
            estado: props.tarifa.estado ? props.tarifa.estado : true,
            errors: {},
        }
    }

    componentWillMount() {
        this.consultarDatos();
    }

    componentWillReceiveProps(prevProps) {
        this.setState({
            Id: prevProps.tarifa.Id ? prevProps.tarifa.Id : 0,
            tipo_habitacion_id: prevProps.tarifa.TipoHabitacionId ? prevProps.tarifa.TipoHabitacionId : 0,
            capacidad: prevProps.tarifa.capacidad ? prevProps.tarifa.capacidad : 0,
            costo_persona: prevProps.tarifa.costo_persona ? prevProps.tarifa.costo_persona : 0.0,
            estado: prevProps.tarifa.estado === undefined ? true : prevProps.tarifa.estado,
        })
    }


    render() {
        return (
            <>
                {this.props.editable &&
                    <div className="p-col-12 p-md-3">
                        <Message severity="warn" text="La tarifa afectará a todos los consumos registrados para el proveedor en el periodo de vigencia del contrato" />
                    </div>
                }

                <form onSubmit={this.handleSubmit}>
                    <div className="row">
                        <div className="col">
                            <Field
                                name="tipo_habitacion_id"
                                required
                                value={this.state.tipo_habitacion_id}
                                label="Tipo de Habitación"
                                options={this.state.catalogo_tipo_habitacion}
                                type={"select"}
                                onChange={this.onChangeTipoHabitacion}
                                error={this.state.errors.tipo_habitacion_id}
                                readOnly={this.props.editable}
                                placeholder="Seleccione..."
                                filterPlaceholder="Seleccione"

                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="capacidad"
                                label="Capacidad"
                                required
                                edit={true}
                                readOnly={false}
                                value={this.state.capacidad}
                                onChange={this.handleChange}
                                error={this.state.errors.capacidad}
                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="costo_persona"
                                label="Costo Por Persona"
                                required
                                edit={true}
                                readOnly={false}
                                value={this.state.costo_persona}
                                onChange={this.handleChange}
                                error={this.state.errors.costo_persona}
                            />
                        </div>
                    </div>

                    <button type="submit" className="btn btn-outline-primary">Guardar</button>&nbsp;

                    <button type="button" className="btn btn-outline-primary" onClick={this.props.ocultarForm}>Cancelar</button>&nbsp;
                </form>
            </>
        )
    }

    handleSubmit = (event) => {
        event.preventDefault();
        if (this.state.tipo_habitacion_id === 0) {
            this.showWarn(FRASE_ERROR_SELECCIONA_UN_TIPO_HABITACION);
        } else {
            if (!this.isValid()) {
                this.showWarn(FRASE_ERROR_VALIDACIONES);
                return;
            }
            this.props.blockScreen();
            if (this.state.Id > 0) {
                // Editar
                this.actionUpdate();
            } else {
                // Nuevo
                this.actionCreate();
            }


        }
    }

    isValid() {
        const errors = {};

        if (!validationRules["isFloat"]([], this.state.costo_persona)) {
            errors.costo_persona = 'Costo por persona debe ser un número';
        }

        if (!validationRules["isFloat"]([], this.state.capacidad)) {
            errors.capacidad = 'Capacidad debe ser un número, usar  . “punto” como separador decimal';
        }

        if (this.state.costo_persona <= 0) {
            errors.costo_persona = 'El costo por persona debe ser mayor a cero';
        }

        if (!validationRules["haveTwoDecimals"]([], this.state.costo_persona)) {
            errors.costo_persona = 'El costo por persona debe tener máximo 3 enteros y 2 decimales';
        }

        if (this.state.costo_persona > 1000.00) {
            errors.costo_persona = 'El costo por persona debe tener máximo 3 enteros y 2 decimales';
        }



        if (this.state.capacidad <= 0) {
            errors.capacidad = 'La capacidad debe ser mayor a cero';
        }

        if (this.state.capacidad > 100) {
            errors.capacidad = 'La capacidad debe tener máximo 2 dígitos';
        }

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    actionCreate = () => {
        var tarifa = {
            TipoHabitacionId: this.state.tipo_habitacion_id,
            capacidad: this.state.capacidad,
            ContratoProveedorId: this.props.contratoProveedorId,
            costo_persona: this.state.costo_persona,
            estado: true
        }
        let url = '';
        url = "/Proveedor/TarifaHotel/CreateTarifasApi"
        http.post(url, tarifa)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({
                        Id: 0,
                        tipo_habitacion_id: 0,
                        capacidad: 0,
                        costo_persona: 0.0
                    }, this.successCreate)
                } else {
                    this.showWarn("La tarifa ya se encuentra registrada")
                    this.props.unlockScreen();
                    console.log(data.errors)
                }
            })
            .catch((error) => {
                this.props.unlockScreen();
                console.log(error)
            })
    }

    actionUpdate = () => {
        let url = '';
        url = "/Proveedor/TarifaHotel/UpdateApi"
        var entity = {
            Id: this.state.Id,
            TipoHabitacionId: this.state.tipo_habitacion_id,
            capacidad: this.state.capacidad,
            estado: this.state.estado,
            ContratoProveedorId: this.props.contratoProveedorId,
            costo_persona: this.state.costo_persona,

        }
        http.post(url, entity)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({
                        Id: 0,
                        tipo_habitacion_id: 0,
                        capacidad: 0,
                        costo_persona: 0.0,
                        estado: true,
                    }, this.successUpdate)
                } else {
                    this.props.unlockScreen();
                    console.log(data.errors)
                }
            })
            .catch((error) => {
                this.props.unlockScreen();
                console.log(error)
            })
    }

    successCreate = () => {
        this.props.getTarifasHoteles(this.props.contratoProveedorId);
        this.props.showSuccess(FRASE_TARIFA_CREADA);
        this.props.ocultarForm();
    }

    successUpdate = () => {
        this.props.getTarifasHoteles(this.props.contratoProveedorId);
        this.props.showSuccess(FRASE_TARIFA_ACTUALIZADA);
        this.props.ocultarForm();
    }

    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }


    catalogoTipoHabitacion = () => {
        let url = '';
        url = `/Proveedor/TarifaHotel/SearchByCodeApi/?code=${TIPO_HABITACION}`;
        return http.get(url);
    }

    consultarDatos = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.catalogoTipoHabitacion()])
            .then(function ([tipos]) {
                console.log(tipos)
                self.setState({
                    catalogo_tipo_habitacion: self.buildDropdown(tipos.data, 'nombre', 'Id')
                }, self.props.unlockScreen)
            })
            .catch((error) => {

                self.props.unlockScreen();
                console.log(error);
            });
    }

    onChangeTipoHabitacion = (name, value) => {
        this.setState({
            tipo_habitacion_id: value
        });
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

    warn = () => {
        abp.notify.error(this.state.displayMessage, 'Error');
    }

    showWarn = displayMessage => {
        this.setState({ displayMessage }, this.warn)
    }
}