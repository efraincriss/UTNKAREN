import React from 'react';
import { TIPO_VEHICULO } from "../../Base/Constantes";
import http from "../../Base/HttpService";
import Field from "../../Base/Field-v2";
import {
    MODULO_PROYECTO,
    FRASE_ERROR_SELECCIONA_PROVEEDOR,
    FRASE_ERROR_INGRESA_CODIGO,
    FRASE_ERROR_SELECCIONA_TIPO_VEHICULO,
    FRASE_ERROR_INGRESA_PLACA,
    FRASE_ERROR_INGRESA_MARCA,
    FRASE_ERROR_INGRESA_COLOR,
    FRASE_ERROR_FECHA_VENCIMIENTO_MATRICULA,
    CONTROLLER_VEHICULOS,
    MODULO_TRANSPORTE,
    FRASE_VEHICULO_CREADO,
    FRASE_VEHICULO_ACTUALIZADO,
    MODULO_PROVEEDOR,
    CONTROLLER_PROVEEDOR

} from "../../Base/Strings";
import config from "../../Base/Config";
import validationRules from '../../Base/validationRules';

export default class VehiculosForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            Id: props.vehiculo.Id ? props.vehiculo.Id : 0,
            ProveedorId: props.vehiculo.ProveedorId ? props.vehiculo.ProveedorId : 0,
            Codigo: props.vehiculo.CodigoEquipoInventario ? props.vehiculo.CodigoEquipoInventario : '',
            CodigoEquipoInventario: props.vehiculo.Codigo ? props.vehiculo.Codigo : '',
            TipoVehiculoId: props.vehiculo.TipoVehiculoId ? props.vehiculo.TipoVehiculoId : 0,
            NumeroPlaca: props.vehiculo.NumeroPlaca ? props.vehiculo.NumeroPlaca : '',
            Marca: props.vehiculo.Marca ? props.vehiculo.Marca : '',
            AnioFabricacion: props.vehiculo.AnioFabricacion ? props.vehiculo.AnioFabricacion : null,
            Color: props.vehiculo.Color ? props.vehiculo.Color : '',
            FechaVencimientoMatricula: props.vehiculo.FechaVencimientoMatricula ? props.vehiculo.FechaVencimientoMatricula : '',
            Estado: props.vehiculo.Estado ? props.vehiculo.Estado : 'ACT',
            FechaEstado: props.vehiculo.FechaEstado ? props.vehiculo.FechaEstado : '',
            Capacidad: props.vehiculo.Capacidad ? props.vehiculo.Capacidad : null,
            estado_opciones: [
                { label: "ACTIVO", value: "ACT" },
                { label: "INACTIVO", value: "INA" },
                { label: "MANTENIMIENTO", value: "MAN" },
            ],
            catalogo_tipo_vehiculo: [],
            catalogo_proveedores: [],
            errors: props.errors,
        }
        this.isValid = this.isValid.bind(this);
    }

    componentWillMount() {
        this.consultarDatos();
    }

    componentWillReceiveProps(prevProps) {
        this.setState({
            Id: prevProps.vehiculo.Id ? prevProps.vehiculo.Id : 0,
            ProveedorId: prevProps.vehiculo.ProveedorId ? prevProps.vehiculo.ProveedorId : 0,
            Codigo: prevProps.vehiculo.CodigoEquipoInventario ? prevProps.vehiculo.CodigoEquipoInventario : '',
            CodigoEquipoInventario: prevProps.vehiculo.Codigo ? prevProps.vehiculo.Codigo : '',
            TipoVehiculoId: prevProps.vehiculo.TipoVehiculoId ? prevProps.vehiculo.TipoVehiculoId : 0,
            NumeroPlaca: prevProps.vehiculo.NumeroPlaca ? prevProps.vehiculo.NumeroPlaca : '',
            Marca: prevProps.vehiculo.Marca ? prevProps.vehiculo.Marca : '',
            AnioFabricacion: prevProps.vehiculo.AnioFabricacion ? prevProps.vehiculo.AnioFabricacion : null,
            Color: prevProps.vehiculo.Color ? prevProps.vehiculo.Color : '',
            FechaVencimientoMatricula: prevProps.vehiculo.FechaVencimientoMatricula ? prevProps.vehiculo.FechaVencimientoMatricula : '',
            Estado: prevProps.vehiculo.Estado ? prevProps.vehiculo.Estado : 'ACT',
            FechaEstado: prevProps.vehiculo.FechaEstado ? prevProps.vehiculo.FechaEstado : '',
            Capacidad: prevProps.vehiculo.Capacidad ? prevProps.vehiculo.Capacidad : null,
        })
    }

    isValid() {
        const errors = {};
        if (this.state.ProveedorId === 0) {
            errors.ProveedorId = 'Campo requerido';
        }
        if (this.state.Capacidad ==null ||this.state.Capacidad <= 0 || this.state.Capacidad > 99) {
            errors.Capacidad = 'Campo requerido (1-99)';
        }
        if (this.state.Capacidad !=null  && this.state.Capacidad > 0) {
            const integer = validationRules["isInt"]([], this.state.Capacidad);
            if (!integer) {
                errors.Capacidad = 'Ingresar un valor entero';
            }

        }
        if (this.state.Codigo === '') {
            errors.Codigo = 'Campo requerido';
        }
        if (this.state.Codigo != null && this.state.Codigo.length > 10) {
            errors.Codigo = 'Ingresar máximo 10 caracteres';
        }
        if (this.state.TipoVehiculoId === 0) {
            errors.TipoVehiculoId = 'Campo requerido';
        }
        if (this.state.NumeroPlaca === '') {
            errors.NumeroPlaca = 'Campo requerido';
        }
        if (this.state.NumeroPlaca != null && this.state.NumeroPlaca.length > 10) {
            errors.NumeroPlaca = 'Ingresar máximo 10 caracteres';
        }
        if (this.state.Marca === '') {
            errors.Marca = 'Campo requerido';
        }
        if (this.state.Marca != null && this.state.Marca.length > 60) {
            errors.Marca = 'Ingresar máximo 60 caracteres';
        }
        if (this.state.AnioFabricacion ==null ||this.state.AnioFabricacion !=null && this.state.AnioFabricacion < 1970 || this.state.AnioFabricacion !=null && this.state.AnioFabricacion > (new Date().getFullYear() + 1)) {
            var x = (new Date().getFullYear() + 1);
            console.log(x);
            errors.AnioFabricacion = 'Campo requerido (1970-' + x + ')';
        }
        if (this.state.Color === '') {
            errors.Color = 'Campo requerido';
        }
        if (this.state.Color != null && this.state.Color.length > 20) {
            errors.Color = 'Ingresar máximo 20 caracteres';
        }
        if (this.state.FechaVencimientoMatricula === '') {
            errors.FechaVencimientoMatricula = 'Campo requerido';

        }
        if (this.state.FechaVencimientoMatricula != '' && new Date(this.state.FechaVencimientoMatricula).getFullYear() > (new Date().getFullYear() + 10)) {
            errors.FechaVencimientoMatricula = 'Fecha Caducidad Matrícula no debe ser mayor a 10 años';

        }


        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }
    render() {
        return (
            <form onSubmit={this.handleSubmit}>

                <div className="row">
                    <div className="col">
                        <Field
                            name="Codigo"
                            label="Equipo Inventario"
                            required
                            edit={true}
                            readOnly={this.props.editForm}
                            value={this.state.Codigo}
                            onChange={this.handleChangeFilter}
                            style={{textTransform: 'uppercase'}}
                            error={this.state.errors.Codigo}
                        />
                    </div>

                    <div className="col">
                        <Field
                            name="NumeroPlaca"
                            label="No. de Placa"
                            required
                            edit={true}
                            readOnly={false}
                            value={this.state.NumeroPlaca}
                            onChange={this.handleChangeFilter}
                            style={{textTransform: 'uppercase'}}
                            error={this.state.errors.NumeroPlaca}
                        />
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <Field
                            name="TipoVehiculoId"
                            required
                            value={this.state.TipoVehiculoId}
                            label="Tipo de Vehículo"
                            options={this.state.catalogo_tipo_vehiculo}
                            type={"select"}
                            filter={true}
                            onChange={this.onChangeTipoVehiculo}
                            error={this.state.errors.TipoVehiculoId}
                            readOnly={false}
                            placeholder="Seleccione.."
                            filterPlaceholder="Seleccione.."

                        />
                    </div>

                    <div className="col">
                        <Field
                            name="ProveedorId"
                            required
                            value={this.state.ProveedorId}
                            label="Proveedor"
                            options={this.state.catalogo_proveedores}
                            type={"select"}
                            filter={true}
                            onChange={this.onChangeTipoVehiculo}
                            error={this.state.errors.ProveedorId}
                            readOnly={this.props.editForm}
                            placeholder="Seleccione.."
                            filterPlaceholder="Seleccione.."

                        />
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Field
                            name="Capacidad"
                            label="Capacidad"
                            required
                            type="number"
                            //min="1"
                            // max="99"
                            step="1"
                            edit={true}
                            readOnly={false}
                            value={this.state.Capacidad}
                            onChange={this.handleChange}
                            error={this.state.errors.Capacidad}
                        />
                    </div>

                    <div className="col">
                        <Field
                            name="Marca"
                            label="Marca"
                            required
                            edit={true}
                            readOnly={false}
                            value={this.state.Marca}
                            onChange={this.handleChange}
                            style={{textTransform: 'uppercase'}}
                            error={this.state.errors.Marca}
                        />
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Field
                            name="AnioFabricacion"
                            label="Año de Fabricación"
                            required
                            type="number"
                            //min="1"
                            step="1"
                            edit={true}
                            readOnly={false}
                            value={this.state.AnioFabricacion}
                            onChange={this.handleChange}
                            error={this.state.errors.AnioFabricacion}
                        />
                    </div>
                    <div className="col">
                        <Field
                            name="Color"
                            label="Color"
                            required
                            edit={true}
                            readOnly={false}
                            value={this.state.Color}
                            onChange={this.handleChange}
                            style={{textTransform: 'uppercase'}}
                            error={this.state.errors.Color}
                        />
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Field
                            name="FechaVencimientoMatricula"
                            label="Fecha Vencimiento de Matrícula"
                            required
                            type="date"
                            edit={true}
                            readOnly={false}
                            value={this.state.FechaVencimientoMatricula}
                            onChange={this.handleChange}
                            error={this.state.errors.FechaVencimientoMatricula}
                        />
                    </div>

                    {this.props.editForm ?
                        <div className="col">
                            <Field
                                name="Estado"
                                required
                                value={this.state.Estado}
                                label="Estado"
                                options={this.state.estado_opciones}
                                type={"select"}
                                filter={true}
                                onChange={this.onChangeTipoVehiculo}
                                error={this.state.errors.Estado}
                                readOnly={false}
                                placeholder="Estado"
                                filterPlaceholder="Estado"

                            />
                        </div>
                        :
                        <div className="col">
                            <h6><b>Estado:</b> {this.normalizeEstado(this.state.Estado)}</h6>
                        </div>
                    }
                </div>


                <button type="submit" className="btn btn-outline-primary">Guardar</button>&nbsp;
                <button
                    type="button"
                    className="btn btn-outline-primary"
                    icon="fa fa-fw fa-ban"
                    onClick={this.props.ocultarForm}
                >
                    Cancelar
              </button>
            </form>
        )
    }

    handleSubmit = (event) => {
        event.preventDefault();
        if (!this.isValid()) {
            abp.notify.error("No ha ingresado los campos obligatorios  o existen datos inválidos.", 'Validación');
            return;
        }

        else {
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

    actionCreate = () => {
        var entity = {
            Id: this.state.Id,
            ProveedorId: this.state.ProveedorId,
            Codigo: this.state.Codigo.toUpperCase(),
            CodigoEquipoInventario: this.state.Codigo.toUpperCase(),
            TipoVehiculoId: this.state.TipoVehiculoId,
            NumeroPlaca: this.state.NumeroPlaca.toUpperCase(),
            Marca: this.state.Marca.toUpperCase(),
            AnioFabricacion: this.state.AnioFabricacion,
            Color: this.state.Color.toUpperCase(),
            FechaVencimientoMatricula: this.state.FechaVencimientoMatricula,
            Estado: 'ACT',
            FechaEstado: new Date(),
            Capacidad: this.state.Capacidad
        }
        let url = '';
        url = `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_VEHICULOS}/CreateApi`
        http.post(url, entity)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({
                        Id: 0,
                        ProveedorId: 0,
                        Codigo: '',
                        TipoVehiculoId: 0,
                        NumeroPlaca: '',
                        Marca: '',
                        AnioFabricacion: null,
                        Color: '',
                        FechaVencimientoMatricula: '',
                        Estado: '',
                        FechaEstado: '',
                        Capacidad: null,
                    }, this.successCreate)
                } else {
                    this.props.setVehiculo(entity);
                    this.showWarn(data.errors);
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
        url = `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_VEHICULOS}/UpdateApi`
        var entity = {
            Id: this.state.Id,
            ProveedorId: this.state.ProveedorId,
            Codigo: this.state.CodigoEquipoInventario,
            CodigoEquipoInventario: this.state.Codigo.toUpperCase(),
            TipoVehiculoId: this.state.TipoVehiculoId,
            NumeroPlaca: this.state.NumeroPlaca.toUpperCase(),
            Marca: this.state.Marca.toUpperCase(),
            AnioFabricacion: this.state.AnioFabricacion,
            Color: this.state.Color.toUpperCase(),
            FechaVencimientoMatricula: this.state.FechaVencimientoMatricula,
            Estado: this.state.Estado,
            FechaEstado: this.state.FechaEstado,
            Capacidad: this.state.Capacidad,
        }
        http.post(url, entity)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({
                        Id: 0,
                        ProveedorId: 0,
                        Codigo: '',
                        TipoVehiculoId: 0,
                        NumeroPlaca: '',
                        Marca: '',
                        AnioFabricacion: null,
                        Color: '',
                        FechaVencimientoMatricula: '',
                        Estado: '',
                        FechaEstado: '',
                        Capacidad: null,
                    }, this.successUpdate)
                } else {
                    this.props.setVehiculo(entity);
                    this.props.unlockScreen();
                    this.showWarn(data.errors);
                    console.log(data.errors)
                }
            })
            .catch((error) => {
                this.props.unlockScreen();
                console.log(error)
            })
    }

    normalizeEstado = (estado) => {
        switch (estado) {
            case "ACT":
                return "ACTIVO"
            case "INA":
                return "INACTIVO"
            case "MAN":
                return "MANTENIMIENTO"
            default:
                ""
        }
    }

    successCreate = () => {
        this.props.onRefreshData({});
        this.props.showSuccess(FRASE_VEHICULO_CREADO);
        this.props.ocultarForm();
    }

    successUpdate = () => {
        this.props.onRefreshData({});
        this.props.showSuccess(FRASE_VEHICULO_ACTUALIZADO);
        this.props.ocultarForm();
    }

    handleChange = (event) => {
       // var start = event.target.selectionStart;
      
       // var end = event.target.selectionEnd;
       // this.setState({ [event.target.name]: event.target.value.toUpperCase() });
       this.setState({ [event.target.name]: event.target.value});
       // event.target.setSelectionRange(start, end);
    }
    handleChangeCapacidad = (event) => {
        this.setState({ [event.target.name]: event.target.value.match(/[0-9]?[0-9]/g) });
    }


    handleChangeFilter = (event) => {
        this.setState({ [event.target.name]: event.target.value.replace(/[^A-Z0-9]+/ig, "")});
    }


    catalogoTipoVehiculo = () => {
        let url = '';
        url = `${config.appUrl}${MODULO_PROYECTO}/Catalogo/GetByCodeApi/?code=${TIPO_VEHICULO}`;
        return http.get(url);
    }

    catalogoProveedores = () => {
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/${CONTROLLER_PROVEEDOR}/GetProveedoresTransporteApi`;
        return http.get(url);
    }

    consultarDatos = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.catalogoTipoVehiculo(), this.catalogoProveedores()])
            .then(function ([tipos, proveedores]) {
                console.log(tipos)
                self.setState({
                    catalogo_tipo_vehiculo: self.buildDropdown(tipos.data, 'nombre', 'Id'),
                    catalogo_proveedores: self.buildDropdown(proveedores.data, 'razon_social', 'Id')
                }, self.props.unlockScreen)
            })
            .catch((error) => {

                self.props.unlockScreen();
                console.log(error);
            });
    }

    onChangeTipoVehiculo = (name, value) => {
        this.setState({
            [name]: value
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