import React from 'react';
import {TIPO_HABITACION} from "../../Base/Constantes";
import { Message } from 'primereact-v2/message';
import {
    FRASE_ERROR_SELECCIONA_UN_TIPO_HABITACION, 
    FRASE_HABITACION_CREADA, 
    FRASE_HABITACION_ACTUALIZADA,
    MODULO_PROVEEDOR,
    CONTROLLER_RESERVA_HOTEL,
    CONTROLLER_HABITACION,
    FRASE_ERROR_VALIDACIONES
} from "../../Base/Strings";
import http from "../../Base/HttpService";
import Field from "../../Base/Field-v2";
import config from "../../Base/Config";
import validationRules from "../../Base/validationRules";

export default class CrearHabitacionForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Id: props.habitacion.Id ? props.habitacion.Id : 0,
            nro_habitacion: props.habitacion.numero_habitacion ? props.habitacion.numero_habitacion : '',
            tipo_habitacion_id: props.habitacion.TipoHabitacionId ? props.habitacion.TipoHabitacionId : 0,
            capacidad: props.habitacion.capacidad ? props.habitacion.capacidad : 0,
            catalogo_tipo_habitacion: [],
            estado: props.habitacion.estado ? props.habitacion.estado : true,
            ProveedorId: props.habitacion.ProveedorId ? props.habitacion.ProveedorId : 0,
            aprobado: props.habitacion.aprobado ? props.habitacion.aprobado : true,
            fecha_aprobacion: props.habitacion.fecha_aprobacion ? props.habitacion.fecha_aprobacion : new Date(),
            capacidad_anterior: props.habitacion.capacidad ? props.habitacion.capacidad : 0,
            errors: {},

        }
    }

    componentWillMount() {
        this.consultarDatos();
    }

    componentWillReceiveProps(prevProps){
        this.setState({
            Id: prevProps.habitacion.Id ? prevProps.habitacion.Id : 0,
            nro_habitacion: prevProps.habitacion.numero_habitacion ? prevProps.habitacion.numero_habitacion : '',
            tipo_habitacion_id: prevProps.habitacion.TipoHabitacionId ? prevProps.habitacion.TipoHabitacionId : 0,
            capacidad: prevProps.habitacion.capacidad ? prevProps.habitacion.capacidad : 0,
            capacidad_anterior: prevProps.habitacion.capacidad ? prevProps.habitacion.capacidad : 0,
            estado: prevProps.habitacion ? prevProps.habitacion.estado : true,
            ProveedorId: prevProps.habitacion.ProveedorId ? prevProps.habitacion.ProveedorId : 0,
            aprobado: prevProps.habitacion.aprobado ? prevProps.habitacion.aprobado : true,
            fecha_aprobacion: prevProps.habitacion.fecha_aprobacion ? prevProps.habitacion.fecha_aprobacion : new Date(),
        })
    }

    render() {
        return (
            <div className="row">
                <div className="col">
                {this.props.editable &&
                <div className="p-col-12 p-md-3">
                        <Message severity="warn" text="Si se actualiza el tipo y reduce la capacidad de la habitación debe dirigirse a la viñeta 'Stock de Espacios', y proceda a inactivar los espacios no necesarios." />
                    </div>}
                    <form onSubmit={this.handleSubmit}>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="nro_habitacion"
                                label="N° Habitación"
                                required
                                edit={true}
                                readOnly={this.props.editable}
                                value={this.state.nro_habitacion}
                                onChange={this.handleChange}
                                error={this.state.errors.nro_habitacion}
                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="tipo_habitacion_id"
                                required
                                value={this.state.tipo_habitacion_id}
                                label="Tipo Habitación"
                                options={this.state.catalogo_tipo_habitacion}
                                type={"select"}
                                onChange={this.onChangeTipoHabitacion}
                                error={this.state.errors.tipo_habitacion_id}
                                readOnly={false}
                                placeholder="Seleccione.."
                                filterPlaceholder="Seleccione.."

                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="capacidad"
                                label="Capacidad"
                                required
                                type="number"
                                edit={true}
                                readOnly={false}
                                value={this.state.capacidad}
                                onChange={this.handleChange}
                                error={this.state.errors.capacidad}
                            />
                        </div>
                    </div>


                    <button type="submit" className="btn btn-outline-primary">Guardar</button>&nbsp;
                    <button type="button" onClick={this.props.ocultarForm} className="btn btn-outline-primary">Cancelar</button>&nbsp;
                    </form>
                </div>
            </div>
        );
    }

    resetErrors = () => {
        this.setState({errors: {}})
    }
            
    handleSubmit = (event) => {
        event.preventDefault();
        if (!this.isValid()) {
            this.showWarn(FRASE_ERROR_VALIDACIONES, 'Validaciones');
            return;
        } else {
            this.props.blockScreen();
            if(this.state.Id > 0){
                // Editar
                this.actionUpdate();
            } else {
                // Nuevo
                this.actionCreate();
            }
        }
    }

    actionCreate = () => {
        var habitacion = {
            ProveedorId: this.props.proveedorId,
            numero_habitacion: this.state.nro_habitacion,
            TipoHabitacionId: this.state.tipo_habitacion_id,
            capacidad: this.state.capacidad,
            estado: true,
            aprobado: false,
            fecha_aprobacion: new Date()
        }
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/${CONTROLLER_HABITACION}/CreateHabitacioYEspaciosnApi`
        http.post(url, habitacion)
        .then((response) => {
            let data = response.data;
            if(data.success === true){
                this.setState({
                    Id: 0,
                    nro_habitacion: '',
                    tipo_habitacion_id: 0,
                    capacidad: 0,
                    estado: 1,
                    aprobado:  1,
                    fecha_aprobacion: new Date(),
                }, this.successCreate)
            } else {
                this.props.setHabitacion(habitacion)
                this.props.showWarn(data.errors);
                this.props.unlockScreen();
            }
        })
        .catch((error) => {
            this.props.unlockScreen();
            console.log(error)
        })
    }

    actionUpdate = () => {
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/${CONTROLLER_HABITACION}/UpdateApi`
        var entity = {
            Id: this.state.Id,
            ProveedorId: this.state.ProveedorId,
            numero_habitacion: this.state.nro_habitacion,
            TipoHabitacionId: this.state.tipo_habitacion_id,
            capacidad: this.state.capacidad,
            capacidad_anterior: this.state.capacidad_anterior,
            estado: this.state.estado,
            aprobado: this.state.aprobado,
            fecha_aprobacion: this.state.fecha_aprobacion
        }
        http.post(url, entity)
        .then((response) => {
            let data = response.data;
            if(data.success === true){
                this.setState({
                    Id: 0,
                    nro_habitacion: '',
                    tipo_habitacion_id: 0,
                    capacidad: 0,
                    estado: 1,
                    aprobado:  1,
                    fecha_aprobacion: new Date(),
                }, this.successUpdate)
            } else {
                this.props.setHabitacion(entity)
                this.props.unlockScreen();
                console.log(data.errors)
            }
        })
        .catch((error) => {
            this.props.unlockScreen();
            console.log(error)
        })
    }

    handleChange = (event) => {
        this.setState({[event.target.name]: event.target.value.toUpperCase()});
    }

    catalogoTipoHabitacion = () => {
        let url = '';
        url = `/Proveedor/Habitacion/ListarTarifasTipoHabitacion/${this.props.proveedorId}`;
        return http.get(url);
    }

    isValid() {
        const errors = {};

        if (this.state.capacidad <= 0) {
            errors.capacidad = 'Capacidad debe ser un número entero de 1 a 15';
        }

        if (this.state.capacidad > 15) {
            errors.capacidad = 'Capacidad debe ser un número entero de 1 a 15';
        }

        if (!validationRules["isIntegerNumber"]([], this.state.capacidad)) {
            errors.capacidad = 'Capacidad debe ser un número entero';
        }

        if(this.state.tipo_habitacion_id === 0){
            errors.tipo_habitacion_id = 'Seleccionar un tipo de habitación';
        }

        if (validationRules["isEmptyString"]([], this.state.nro_habitacion)) {
            errors.nro_habitacion = 'Ingresa el número de habitación';
        }

        

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    consultarDatos = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.catalogoTipoHabitacion()])
        .then(function ([tipos]){
            self.setState({
                catalogo_tipo_habitacion: self.buildDropdown(tipos.data, 'name', 'Id')
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
        console.log(data.result)
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

    successCreate = () => {
        this.props.consultarEspaciosYHabitaciones();
        this.props.showSuccess(FRASE_HABITACION_CREADA);
        this.props.ocultarForm();
    }

    successUpdate = () => {
        this.props.consultarEspaciosYHabitaciones();
        this.props.showSuccess(FRASE_HABITACION_ACTUALIZADA);
        this.props.ocultarForm();
    }

    warn = (title = 'Error') => {
        abp.notify.error(this.state.displayMessage, title);
    }

    showWarn = (displayMessage, title) => {
        this.setState({ displayMessage }, () => this.warn(title))
    }
    
}