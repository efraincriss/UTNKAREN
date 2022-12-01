import React from 'react';
import Field from "../../Base/Field-v2";
import { Button } from "primereact-v2/button";
import {
    MODULO_RECURSOS_HUMANOS,
    CONTROLLER_CAPACITACIONES,
    FRASE_ERROR_VALIDACIONES,
    FRASE_CAPACITACION_CREADA,
    FRASE_CAPACITACION_ACTUALIZADA,
    FRASE_ERROR_GLOBAL
} from "../../Base/Strings";
import config from "../../Base/Config";
import http from "../../Base/HttpService";

export class CapacitacionForm extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            data: {
                Id: props.capacitacion.Id ? props.capacitacion.Id : 0,
                CatalogoNombreCapacitacionId: props.capacitacion.CatalogoNombreCapacitacionId ? props.capacitacion.CatalogoNombreCapacitacionId : 0,
                CatalogoTipoCapacitacionId: props.capacitacion.CatalogoTipoCapacitacionId ? props.capacitacion.CatalogoTipoCapacitacionId : 0,
                ColaboradoresId: props.capacitacion.ColaboradoresId ? props.capacitacion.ColaboradoresId : 0,
                Fuente: props.capacitacion.Fuente ? props.capacitacion.Fuente : '',
                Horas: props.capacitacion.Horas ? props.capacitacion.Horas : 0,
                Observaciones: props.capacitacion.Observaciones ? props.capacitacion.Observaciones : '',
                Fecha: props.capacitacion.Fecha ? props.capacitacion.Fecha : ''
            },
            errors: {},
            displayMessage: '',
            keyUpload: 98,
        }
    }

    componentWillReceiveProps(prevProps) {
        let updatedValues = {
            Id: prevProps.capacitacion.Id ? prevProps.capacitacion.Id : 0,
            CatalogoNombreCapacitacionId: prevProps.capacitacion.CatalogoNombreCapacitacionId ? prevProps.capacitacion.CatalogoNombreCapacitacionId : 0,
            CatalogoTipoCapacitacionId: prevProps.capacitacion.CatalogoTipoCapacitacionId ? prevProps.capacitacion.CatalogoTipoCapacitacionId : 0,
            ColaboradoresId: prevProps.capacitacion.ColaboradoresId ? prevProps.capacitacion.ColaboradoresId : 0,
            Fuente: prevProps.capacitacion.Fuente ? prevProps.capacitacion.Fuente : '',
            Horas: prevProps.capacitacion.Horas ? prevProps.capacitacion.Horas : "",
            Observaciones: prevProps.capacitacion.Observaciones ? prevProps.capacitacion.Observaciones : '',
            Fecha: prevProps.capacitacion.Fecha ? prevProps.capacitacion.Fecha : '',
        }
        this.setState({
            data: updatedValues
        })
    }

    render() {
        return (
            <div>
                <div>
                    <div className="row">
                        <div className="col">
                            <Field
                                name="CatalogoNombreCapacitacionId"
                                label="Nombre Capacitación"
                                type="select"
                                options={this.props.catalogoNombreCapacitaciones}
                                edit={true}
                                readOnly={false}
                                value={this.state.data.CatalogoNombreCapacitacionId}
                                onChange={this.onChangeDropdown}
                                error={this.state.errors.CatalogoNombreCapacitacionId}
                                placeholder="Seleccionar..."
                            />
                        </div>

                        <div className="col">
                            <Field
                                name="CatalogoTipoCapacitacionId"
                                label="Tipo de Capacitación"
                                type="select"
                                options={this.props.catalogoTipoCapacitaciones}
                                edit={true}
                                readOnly={false}
                                value={this.state.data.CatalogoTipoCapacitacionId}
                                onChange={this.onChangeDropdown}
                                error={this.state.errors.CatalogoTipoCapacitacionId}
                                placeholder="Seleccionar..."
                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="Fuente"
                                value={this.state.data.Fuente}
                                label="Fuente"
                                edit={true}
                                readOnly={false}
                                type="text"
                                onChange={this.handleChange}
                                error={this.state.errors.Fuente}
                            />
                        </div>

                        <div className="col">
                            <Field
                                name="Horas"
                                value={this.state.data.Horas}
                                label="Horas"
                                type="text"
                                edit={true}
                                readOnly={false}
                                onChange={this.handleChange}
                                error={this.state.errors.Horas}
                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="Fecha"
                                value={this.state.data.Fecha}
                                label="Fecha Capacitación"
                                type="date"
                                edit={true}
                                readOnly={false}
                                onChange={this.handleChange}
                                error={this.state.errors.Fecha}
                            />
                        </div>

                        <div className="col">
                            <Field
                                name="Observaciones"
                                value={this.state.data.Observaciones}
                                label="Observaciones"
                                type="textarea" 
                                onChange={this.handleChange}
                                error={this.state.errors.Observaciones}
                                edit={true}
                                readOnly={false}
                            />
                        </div>
                    </div>

                    <hr />
                    <div className="row" style={{ marginTop: '0.4em' }}>
                        <div className="col">
                            <Button label="Guardar" className="p-button-rounded" onClick={() => this.handleSubmit()} icon="pi pi-save" />
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    handleSubmit = () => {
        if (!this.isValid()) {
            this.showWarn(FRASE_ERROR_VALIDACIONES, 'Validaciones');
            return;
        }

        this.props.blockScreen();
        let url = '';
        url = `${config.apiUrl}${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_CAPACITACIONES}`
        if(this.state.data.Id > 0){
            url += '/ActualizarCapacitacion';
        } else {
            url += '/CrearCapacitacion';
        }

        console.log(url)
        
        let data = Object.assign({}, this.state.data);
        const formData = new FormData();
        for (var key in data) {
            if (data[key] !== null)
                formData.append(key, data[key]);
            else
                formData.append(key, '');
        }

        console.log("Enviando")

        http.post(url, formData)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    if(this.state.data.Id > 0){
                        this.props.showSuccess(FRASE_CAPACITACION_ACTUALIZADA);
                    } else {
                        this.props.showSuccess(FRASE_CAPACITACION_CREADA);
                    }
                    this.props.onHideFormulario(true);
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.showWarn(message);
                    this.props.unlockScreen();
                }

            })
            .catch((error) => {
                console.log(error);
                this.showWarn(FRASE_ERROR_GLOBAL);
                this.props.unlockScreen();
            });
    }

    isValid = () => {
        const errors = {};
        let CatalogoNombreCapacitacionId = this.state.data.CatalogoNombreCapacitacionId
        let CatalogoTipoCapacitacionId = this.state.data.CatalogoTipoCapacitacionId
        let Horas = this.state.data.Horas
        let Fecha = this.state.data.Fecha

        if (CatalogoNombreCapacitacionId === 0) {
            errors.CatalogoNombreCapacitacionId = 'Seleccione una capacitación';
        }

        if (CatalogoTipoCapacitacionId === 0) {
            errors.CatalogoTipoCapacitacionId = 'Seleccione un tipo capacitación';
        }
         let horas = parseFloat(Horas);
         console.log(horas)
        if (!(horas && horas > 0)) {
            errors.Horas = 'Ingresa un número de horas válido';
        } else if(horas === 0) {
            errors.Horas = 'Las horas deben ser mayor a cero';
        } else if (Horas !==  "" && Horas.toString().split(",").length > 1){
            errors.Horas = `Ingrese un valor separado por "."`
        }

        if(Fecha === ""){
            errors.Fecha = "Ingrese una fecha"
        }

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    handleChange = (event) => {
        const target = event.target;
        const value = target.value;
        const name = target.name;
        const updatedData = {
            ...this.state.data
        };
        updatedData[name] = value;
        this.props.actualizarCapacitacionSeleccionada(updatedData);
        this.setState({
            data: updatedData
        });
    }

    onChangeDropdown = (name, value) => {
        const updatedData = {
            ...this.state.data
        };
        updatedData[name] = value;
        this.props.actualizarCapacitacionSeleccionada(updatedData);
        this.setState({
            data: updatedData
        });
    };

    showWarn = (displayMessage, type= 'Error') => {
        this.setState({ displayMessage }, () => this.warn(type))
    }

    warn = (type) => {
        abp.notify.error(this.state.displayMessage, type);
    }
}