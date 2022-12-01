import React from 'react';
import Field from "../../Base/Field-v2";
import { Button } from "primereact-v2/button";
import {
    MODULO_RECURSOS_HUMANOS,
    CONTROLLER_CAPACITACIONES,
} from "../../Base/Strings";
import http from "../../Base/HttpService";
import axios from "axios";

export class InformacionCapacitaciones extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            catalogoTipoCapacitaciones: [],
            catalogoNombreCapacitaciones: [],
            data: {
                CatalogoNombreCapacitacionId: 0,
                CatalogoTipoCapacitacionId: 0,
                FechaDesde: '',
                FechaHasta: ''
            },
            errors: {},
        }
    }

    componentWillMount() {
        this.loadData();
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
                                options={this.state.catalogoNombreCapacitaciones}
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
                                options={this.state.catalogoTipoCapacitaciones}
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
                                name="FechaDesde"
                                value={this.state.data.FechaDesde}
                                label="Fecha Desde"
                                type="date"
                                edit={true}
                                readOnly={false}
                                onChange={this.handleChange}
                                error={this.state.errors.FechaDesde}
                            />
                        </div>
                        <div className="col">
                            <Field
                                name="FechaHasta"
                                value={this.state.data.FechaHasta}
                                label="Fecha Hasta"
                                type="date"
                                edit={true}
                                readOnly={false}
                                onChange={this.handleChange}
                                error={this.state.errors.FechaHasta}
                            />
                        </div>
                    </div>

                    <hr />
                    <div className="row" style={{ marginTop: '0.4em' }}>
                        <div className="col">
                            <Button label="Descargar" className="p-button-rounded" onClick={() => this.handleSubmit()} icon="pi pi-save" />
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    handleSubmit = () => {
        this.props.blockScreen();
        let url = '';
        url = `/${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_CAPACITACIONES}/DescargarReporteCapacitaciones`

        let data = Object.assign({}, this.state.data);
        const formData = new FormData();
        for (var key in data) {
            if (data[key] !== null)
                formData.append(key, data[key]);
            else
                formData.append(key, '');
        }

        console.log(this.state.data)

        axios.post(
            url,
            this.state.data,
            { responseType: "arraybuffer" }
        )
            .then((response) => {
                var nombre = response.headers["content-disposition"].split("=");

                const url = window.URL.createObjectURL(
                    new Blob([response.data], {
                        type:
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    })
                );
                const link = document.createElement("a");
                link.href = url;
                link.setAttribute("download", nombre[1]);
                document.body.appendChild(link);
                link.click();
                this.showSuccess("Certificados generados exitosamente")
                this.setState({ colaboradoresSeleccionados: [] })
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error);
                this.showWarn("Ocurrió un error al descargar el archivo, intentalo nuevamente");
                this.props.unlockScreen();
            });
    }

    loadData = () => {
        this.props.blockScreen();
        var self = this;
        Promise.all([this.obtenerDatosDeCapacitaciones()])
            .then(function ([response]) {

                let data = response.data;
                if (data.success === true) {
                    let catalogoNombreCapacitaciones = self.buildDropdown(data.result.CatalogoNombreCapacitacion, 'nombre');
                    let catalogoTipoCapacitaciones = self.buildDropdown(data.result.CatalogoTipoCapacitacion, 'nombre');
                    self.setState({
                        catalogoNombreCapacitaciones,
                        catalogoTipoCapacitaciones
                    })
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    console.log(message);
                    this.showWarn("Ocurrió un error, intentalo más tarde.");
                }

                self.props.unlockScreen();
            })
            .catch((error) => {
                self.props.unlockScreen();
                console.log(error);
            });
    }

    obtenerDatosDeCapacitaciones = () => {
        let url = '';
        url = `/${MODULO_RECURSOS_HUMANOS}/${CONTROLLER_CAPACITACIONES}/ObtenerCatalogosDeCapacitaciones`;
        console.log(url)
        return http.get(url);
    }



    handleChange = (event) => {
        const target = event.target;
        const value = target.value;
        const name = target.name;
        const updatedData = {
            ...this.state.data
        };
        updatedData[name] = value;
        this.setState({
            data: updatedData
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

    onChangeDropdown = (name, value) => {
        const updatedData = {
            ...this.state.data
        };
        updatedData[name] = value;
        this.setState({
            data: updatedData
        });
    };

    showWarn = (displayMessage, type = 'Error') => {
        this.setState({ displayMessage }, () => this.warn(type))
    }

    warn = (type) => {
        abp.notify.error(this.state.displayMessage, type);
    }

    showSuccess = (displayMessage, type = 'Correcto') => {
        this.success(displayMessage, type)

    }

    success = (displayMessage, type) => {
        abp.notify.success(displayMessage, type);
    }
}