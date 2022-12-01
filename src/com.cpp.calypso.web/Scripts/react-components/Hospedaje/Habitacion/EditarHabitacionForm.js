import React from 'react';
import { TIPO_HABITACION } from "../../Base/Constantes";
import { FRASE_ERROR_SELECCIONA_UN_TIPO_HABITACION, FRASE_HABITACION_CREADA } from "../../Base/Strings";
import http from "../../Base/HttpService";
import Field from "../../Base/Field-v2";
import { Message } from 'primereact-v2/message';

export default class EditarHabitacionForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            id: 0,
            antigua_capacidad: props.proveedor.capacidad,
            nro_habitacion: props.proveedor.numero_habitacion,
            tipo_habitacion_id: props.proveedor.TipoHabitacionId,
            capacidad: props.proveedor.capacidad,
            catalogo_tipo_habitacion: [],
            errors: {},
            proveedorId: props.proveedor.ProveedorId
        }
    }

    componentWillMount() {
        this.consultarDatos();
    }



    render() {
        return (
            <div className="row">
                <div className="col">

                    <div className="p-col-12 p-md-3">
                        <Message severity="warn" text="Si actualiza el tipo y capacidad de la habitación debe dirigirse a la viñeta Stock Espacios y proceda a inactivar los espacios no necesarios." />
                    </div>
                    <form onSubmit={this.handleSubmit}>

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="habitacion">N° Habitación</label>
                                    <input required type="text" id="habitacion" className="form-control" value={this.state.nro_habitacion} onChange={this.handleChange} name="nro_habitacion" />
                                </div>
                            </div>
                        </div>

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

                        />

                        <div className="form-group">
                            <label htmlFor="capacidad">Capacidad</label>
                            <input type="number" min="1" max="15" required id="capacidad" className="form-control" value={this.state.capacidad} onChange={this.handleChange} name="capacidad" />
                        </div>


                        <button type="submit" className="btn btn-outline-primary">Guardar</button>&nbsp;



                    </form>
                </div>
            </div>
        );
    }

    handleSubmit = (event) => {
        event.preventDefault();
        if (this.state.tipo_habitacion_id === 0) {
            this.props.showWarn(FRASE_ERROR_SELECCIONA_UN_TIPO_HABITACION);
        } else {
            this.props.blockScreen();
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
            url = "/Proveedor/Habitacion/CreateHabitacioYEspaciosnApi"
            http.post(url, habitacion)
                .then((response) => {
                    let data = response.data;
                    if (data.success === true) {
                        this.setState({
                            nro_habitacion: '',
                            tipo_habitacion_id: 0,
                            capacidad: 0,
                        }, this.successSubmit)
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
    }

    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }

    catalogoTipoHabitacion = () => {
        let url = '';
        url = `/Proveedor/Habitacion/SearchByCodeApi/?code=${TIPO_HABITACION}`;
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

    successSubmit = () => {
        this.props.getHabitaciones();
        this.props.getEspacios();
        this.props.showSuccess(FRASE_HABITACION_CREADA);
        this.props.ocultarForm();
    }

}