import React from 'react';
import { TIPO_VEHICULO } from "../../Base/Constantes";
import http from "../../Base/HttpService";
import Field from "../../Base/Field-v2";
import {
    FRASE_ERROR_INGRESA_CODIGO,
    FRASE_ERROR_INGRESA_NOMBRE,
    CONTROLLER_LUGAR,
    MODULO_TRANSPORTE,
    FRASE_LUGAR_CREADO,
    FRASE_LUGAR_ACTUALIZADO,
    FRASE_ERROR_VALIDACIONES

} from "../../Base/Strings";
import config from "../../Base/Config";
import validationRules from '../../Base/validationRules';

export default class LugaresForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            Id: props.lugar.Id ? props.lugar.Id : 0,
            Codigo: props.lugar.Codigo ? props.lugar.Codigo : '',
            Nombre: props.lugar.Nombre ? props.lugar.Nombre : '',
            Latitud: props.lugar.Latitud ? props.lugar.Latitud : '',
            Longitud: props.lugar.Longitud ? props.lugar.Longitud : '',
            Descripcion: props.lugar.Descripcion ? props.lugar.Descripcion : '',

            errors: props.errors,
        }
    }

    componentWillReceiveProps(prevProps) {
        this.setState({
            Id: prevProps.lugar.Id ? prevProps.lugar.Id : 0,
            Codigo: prevProps.lugar.Codigo ? prevProps.lugar.Codigo : '',
            Nombre: prevProps.lugar.Nombre ? prevProps.lugar.Nombre : '',
            Latitud: prevProps.lugar.Latitud ? prevProps.lugar.Latitud : '',
            Longitud: prevProps.lugar.Longitud ? prevProps.lugar.Longitud : '',
            Descripcion: prevProps.lugar.Descripcion ? prevProps.lugar.Descripcion : '',
            
        })
    }


    render() {
        return (
            <form onSubmit={this.handleSubmit}>

                <div className="row">
                    {this.props.editForm && <div className="col">
                        <Field
                            name="Codigo"
                            label="Código"
                            required
                            edit={true}
                            readOnly={this.props.editForm}
                            value={this.state.Codigo}
                            onChange={this.handleChangeFilter}
                            style={{textTransform: 'uppercase'}}
                            error={this.state.errors.Codigo}
                            
                        />
                    </div>
                    }
                    <div className="col">
                        <Field
                            name="Nombre"
                            label="Nombre"
                            required
                            edit={true}
                            readOnly={false}
                            value={this.state.Nombre}
                            onChange={this.handleChange}
                            style={{textTransform: 'uppercase'}}
                            error={this.state.errors.Nombre}
                        />
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <Field
                            name="Latitud"
                            label="Latitud (Desde -90 a 90) "
                            edit={true}
                            readOnly={false}
                            value={this.state.Latitud}
                            onChange={this.handleChange}
                            error={this.state.errors.Latitud}
                        />

                    </div>

                    <div className="col">
                        <Field
                            name="Longitud"
                            label="Longitud (Desde -180 a 180)"
                            edit={true}
                            readOnly={false}
                            value={this.state.Longitud}
                            onChange={this.handleChange}
                            error={this.state.errors.Longitud}
                        />
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Field
                            name="Descripcion"
                            label="Descripción"
                            edit={true}
                            readOnly={false}
                            value={this.state.Descripcion}
                            onChange={this.handleChangeLowerCase}
                            error={this.state.errors.Descripcion}
                        />
                    </div>
                </div>

                <button type="submit" className="btn btn-outline-primary">Guardar</button>&nbsp;

              <button
                    type="button"
                    className="btn btn-outline-primary"
                    icon="fa fa-fw fa-ban"
                    onClick={()=>this.setState({errors:{}},this.props.ocultarForm)}
                >
                    Cancelar
              </button>
            </form>
        )
    }

    handleSubmit = (event) => {
        event.preventDefault();

        if (!this.isValid()) {
            this.props.showValidation(FRASE_ERROR_VALIDACIONES);
            return;
        } else {

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
            Codigo: "0",
            Nombre: this.state.Nombre.toUpperCase(),
            Latitud: this.state.Latitud,
            Longitud: this.state.Longitud,
            Descripcion: this.state.Descripcion,
        }
        let url = '';
        url = `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_LUGAR}/CreateApi`
        http.post(url, entity)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    if (data.created === true) {
                        this.setState({
                            Id: 0,
                            Codigo: '',
                            Nombre: '',
                            Latitud: '',
                            Longitud: '',
                            Descripcion: '',
                        }, this.successCreate)
                    } else {
                        this.props.setLugar(entity);
                        this.showWarn(data.errors);
                        this.props.unlockScreen();
                    }
                } else {
                    this.props.setLugar(entity);
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
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
        url = `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_LUGAR}/UpdateApi`
        var entity = {
            Id: this.state.Id,
            Codigo: this.state.Codigo.toUpperCase(),
            Nombre: this.state.Nombre.toUpperCase(),
            Latitud: this.state.Latitud,
            Longitud: this.state.Longitud,
            Descripcion: this.state.Descripcion,
        }
        http.post(url, entity)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    if (data.updated === true) {
                        this.setState({
                            Id: 0,
                            Codigo: '',
                            Nombre: '',
                            Latitud: '',
                            Longitud: '',
                            Descripcion: '',
                        }, this.successUpdate)
                    } else {
                        this.props.setLugar(entity);
                        this.props.unlockScreen();
                        this.showWarn(data.errors);
                    }
                } else {
                    this.props.setLugar(entity);
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                    this.props.unlockScreen();
                }
            })
            .catch((error) => {
                this.props.unlockScreen();
                console.log(error)
            })
    }

    isValid() {
        const errors = {};

        if (this.state.Nombre.length == 0) {
            errors.Nombre = 'Campo requerido';
        }
        if (this.state.Nombre.length >100) {
            errors.Nombre = 'El campo debe tener máximo 100 caracteres alfanuméricos';
        }
        if (this.state.Latitud.length > 0) {
            const sevendecimals = validationRules["haveSevenDecimals"]([], this.state.Latitud);
            if (!sevendecimals) {
                errors.Latitud = 'Campo requerido (-90 a 90) máximo 7 decimales';
            }
        }

        if (this.state.Latitud.length > 0 && parseInt(this.state.Latitud) < -90 ||
            this.state.Latitud.length > 0 && parseInt(this.state.Latitud) > 90) {
            errors.Latitud = 'Campo requerido (-90 a 90)';
        }
        if (this.state.Longitud.length > 0) {
            const sevendecimals = validationRules["haveSevenDecimals"]([], this.state.Longitud);
            if (!sevendecimals) {
                errors.Longitud = 'Campo requerido (-180 a 180) máximo 7 decimales';
            }
        }


        if (this.state.Longitud.length > 0 && parseInt(this.state.Latitud) < -180 ||
            this.state.Longitud.length > 0 && parseInt(this.state.Latitud) > 180) {
            errors.Longitud = 'Campo requerido (-180 a 180)';
        }



        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    successCreate = () => {
        this.props.onRefreshData({});
        this.props.showSuccess(FRASE_LUGAR_CREADO);
        this.props.ocultarForm();
    }

    successUpdate = () => {
        this.props.onRefreshData({});
        this.props.showSuccess(FRASE_LUGAR_ACTUALIZADO);
        this.props.ocultarForm();
    }

    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value});
    }

    handleChangeLowerCase = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleChangeFilter = (event) => {
        this.setState({ [event.target.name]: event.target.value.replace(/[^A-Z0-9]+/ig, "")});
    }
    handleChangeFloat = (event) => {
        this.setState({ [event.target.name]: event.target.value.replace(/^\d+(\.\d{0,2})?$/, "") });
    }

    warn = () => {
        abp.notify.error(this.state.displayMessage, 'Error');
    }

    showWarn = displayMessage => {
        this.setState({ displayMessage }, this.warn)
    }
}