import React from 'react';
import { TIPO_VEHICULO } from "../../Base/Constantes";
import http from "../../Base/HttpService";
import Field from "../../Base/Field-v2";
import {
    FRASE_ERROR_INGRESA_CODIGO,
    FRASE_ERROR_INGRESA_NOMBRE,
    CONTROLLER_PARADA,
    FRASE_PARADA_ACTUALIZADA,
    FRASE_PARADA_CREADA,
    MODULO_TRANSPORTE,
    FRASE_ERROR_VALIDACIONES
} from "../../Base/Strings";
import config from "../../Base/Config";
import validationRules from '../../Base/validationRules';

export default class ParadasForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            Id: props.parada.Id ? props.parada.Id : 0,
            Codigo: props.parada.Codigo ? props.parada.Codigo : '',
            Nombre: props.parada.Nombre ? props.parada.Nombre : '',
            Latitud: props.parada.Latitud ? props.parada.Latitud : '',
            Longitud: props.parada.Longitud ? props.parada.Longitud : '',
            Referencia: props.parada.Referencia ? props.parada.Referencia : '',

            errors: {},
        }
    }

    componentWillReceiveProps(prevProps) {
        this.setState({
            Id: prevProps.parada.Id ? prevProps.parada.Id : 0,
            Codigo: prevProps.parada.Codigo ? prevProps.parada.Codigo : '',
            Nombre: prevProps.parada.Nombre ? prevProps.parada.Nombre : '',
            Latitud: prevProps.parada.Latitud ? prevProps.parada.Latitud : '',
            Longitud: prevProps.parada.Longitud ? prevProps.parada.Longitud : '',
            Referencia: prevProps.parada.Referencia ? prevProps.parada.Referencia : '',
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
                            name="Referencia"
                            label="Referencia"
                            edit={true}
                            readOnly={false}
                            value={this.state.Referencia}
                            onChange={this.handleChangeLowerCase}
                            error={this.state.errors.Referencia}
                        />
                    </div>
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
            Referencia: this.state.Referencia,
        }
        let url = '';
        url = `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_PARADA}/CreateApi`
        http.post(url, entity)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    if (data.created == true) {
                        this.setState({
                            Id: 0,
                            Codigo: '',
                            Nombre: '',
                            Latitud: '',
                            Longitud: '',
                            Referencia: '',
                        }, this.successCreate)
                    } else {
                        this.props.setParada(entity);
                        this.showWarn(data.errors);
                        this.props.unlockScreen();
                        console.log(data.errors)
                    }

                } else {
                    this.props.setParada(entity);
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
        url = `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_PARADA}/UpdateApi`
        var entity = {
            Id: this.state.Id,
            Codigo: this.state.Codigo.toUpperCase(),
            Nombre: this.state.Nombre.toUpperCase(),
            Latitud: this.state.Latitud,
            Longitud: this.state.Longitud,
            Referencia: this.state.Referencia,
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
                            Referencia: '',
                        }, this.successUpdate)
                    } else {
                        this.props.setParada(entity);
                        this.showWarn(data.errors);
                        this.props.unlockScreen();
                    }
                } else {
                    this.props.setParada(entity);
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                    this.props.unlockScreen();
                }
            })
            .catch((error) => {
                this.props.setParada(entity);
                var message = $.fn.responseAjaxErrorToString(data);
                this.props.showWarn(message);
                this.props.unlockScreen();
            })
    }

    successCreate = () => {
        this.props.onRefreshData({});
        this.props.showSuccess(FRASE_PARADA_CREADA);
        this.props.ocultarForm();
    }

    successUpdate = () => {
        this.props.onRefreshData({});
        this.props.showSuccess(FRASE_PARADA_ACTUALIZADA);
        this.props.ocultarForm();
    }

    isValid() {
        const errors = {};

        if (this.state.Nombre.length == 0) {
            errors.Nombre = 'Campo requerido';
        }
        if (this.state.Nombre.length >100) {
            errors.Nombre = 'El campo debe tener máximo 100 caracteres alfanuméricos';
        }
        if (this.state.Latitud.length > 0 && parseInt(this.state.Latitud) < -90 ||
            this.state.Latitud.length > 0 && parseInt(this.state.Latitud) > 90) {
            errors.Latitud = 'Campo requerido (-90 a 90)';
        }

        if (this.state.Latitud.length > 0) {
            const sevendecimals = validationRules["haveSevenDecimals"]([], this.state.Latitud);
            if (!sevendecimals) {
                errors.Latitud = 'Campo requerido (-90 a 90) máximo 7 decimales';
            }
        }

        if (this.state.Longitud.length > 0 && parseInt(this.state.Latitud) < -180 ||
            this.state.Longitud.length > 0 && parseInt(this.state.Latitud) > 180) {
            errors.Longitud = 'Campo requerido (-180 a 180)';
        }
        if (this.state.Longitud.length > 0) {
            const sevendecimals = validationRules["haveSevenDecimals"]([], this.state.Longitud);
            if (!sevendecimals) {
                errors.Longitud = 'Campo requerido (-180 a 180) máximo 7 decimales';
            }
        }



        this.setState({ errors });
        return Object.keys(errors).length === 0;
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

    warn = () => {
        abp.notify.error(this.state.displayMessage, 'Error');
    }

    showWarn = displayMessage => {
        this.setState({ displayMessage }, this.warn)
    }
}