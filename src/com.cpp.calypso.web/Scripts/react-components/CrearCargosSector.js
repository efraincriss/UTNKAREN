import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { PickList } from 'primereact-v2/picklist';

import { Growl } from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';

export default class CrearCargosSector extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            sectores: [],
            cargosSource: [],
            cargosTarget: [],
            sector: '',
            errores: [],
            formIsValid: true,
            estado: false,
            cargoSector: [],
            targets: [],
            loading: false,
        }

        this.getFormSelectSector = this.getFormSelectSector.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.Regresar = this.Regresar.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.handleChange = this.handleChange.bind(this);

        this.onChange = this.onChange.bind(this);
        this.cargosTemplate = this.cargosTemplate.bind(this);
        this.Guardar = this.Guardar.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
        this.GetTargetCargos = this.GetTargetCargos.bind(this);
        this.GetCargosDisponibles = this.GetCargosDisponibles.bind(this);
    }

    componentDidMount() {
        this.GetCatalogos();
    }

    onChange(event) {
        this.setState({
            cargosSource: event.source,
            cargosTarget: event.target
        });
    }

    cargosTemplate(cargo) {

        return (
            <div className="p-clearfix">
                <div style={{ fontSize: '14px', float: 'right', margin: '15px 5px 0 0' }}>{cargo.nombre}</div><br /><br />
            </div>
        );
    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div className="row">
                    <div className="col-xs-12 col-md-12">
                        <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                        <form onSubmit={this.handleSubmit}>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="label">* Sector: </label>
                                        <select value={this.state.sector} onChange={this.handleChange} className="form-control" name="sector">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectSector()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["sector"]}</span>
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <br /><br />
                                        <div className="content-section implementation">
                                            <PickList source={this.state.cargosSource} target={this.state.cargosTarget} itemTemplate={this.cargosTemplate}
                                                sourceHeader="Cargos Disponibles" targetHeader="Cargos Asignados" responsive={true}
                                                sourceStyle={{ height: '400px' }} targetStyle={{ height: '400px' }}
                                                onChange={this.onChange}></PickList>
                                        </div>
                                        <span style={{ color: "red" }}>{this.state.errores["cargo"]}</span>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div className="form-group">
                                <div className="col">
                                    <button onClick={() => this.Guardar()} type="button" className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                    <button onClick={() => this.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </BlockUi>
        )
    }

    Guardar() {

        this.handleValidation();

        if (this.state.formIsValid == true) {

            this.setState({ loading: true });

            var idTargets = this.state.cargosTarget.map((item) => {
                return (item.Id)
            });

            var idDisponibles = this.state.cargosSource.map((item) => {
                return (item.Id)
            });

            axios.post("/RRHH/CargosSector/EditarCargoSectorApi/", {
                sector: this.state.sector,
                idCargos: idTargets,
                idCargosDisponibles: idDisponibles

            })
                .then((response) => {
                    this.setState({ loading: false });
                    this.successMessage("Cargos Guardados para el Sector!");
                    this.Regresar();
                })
                .catch((error) => {
                    console.log(error);
                    this.warnMessage("Algo salió mal.");
                });
        }
    }

    GetCargosDisponibles(id) {
        axios.post("/RRHH/CargosSector/GetCargosDisponiblesApiAsync/" + id, {})
            .then((response) => {
                response.data.forEach(e => {
                    var catalogo = JSON.parse(e);
                    this.setState({ cargosSource: catalogo })
                    return;
                });
            })
            .catch((error) => {
                console.log(error);
            });
    }

    GetTargetCargos(id) {
        axios.post("/RRHH/CargosSector/GetCargosPorSectorApiAsync/" + id, {})
            .then((response) => {
                response.data.forEach(e => {
                    var catalogo = JSON.parse(e);
                    this.setState({ cargosTarget: catalogo })
                    this.setState({ loading: false });
                    return;
                });
            })
            .catch((error) => {
                console.log(error);
            });
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['SECTOR', 'CARGO'];

        axios.post("/RRHH/CargosSector/GetListaCatalogosPorCodigoApi/", { codigo: codigos })
            .then((response) => {
                this.cargarCatalogos(response.data);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    cargarCatalogos(data) {
        data.forEach(e => {
            var catalogo = JSON.parse(e);
            var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
            if (codigoCatalogo == 'SECTOR') {
                this.setState({ sectores: catalogo })
                this.getFormSelectSector();
                return;
            }
            else if (codigoCatalogo == 'CARGO') {
                this.setState({ cargosSource: catalogo })
                return;
            }
        });
    }

    getFormSelectSector() {
        return (
            this.state.sectores.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    Regresar() {
        return (
            window.location.href = "/RRHH/CargosSector/Index/"
        );
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.message });
    }

    successMessage(msg) {
        this.setState({ message: msg }, this.showSuccess)
    }

    warnMessage(msg) {
        this.setState({ message: msg }, this.showWarn)
    }

    handleValidation() {
        let errors = {};
        this.setState({ formIsValid: true });

        if (!this.state.sector) {
            this.state.formIsValid = false;
            errors["sector"] = "El campo Sector es obligatorio.";
        }
        if (this.state.cargosTarget.length == 0) {
            this.state.formIsValid = false;
            errors["cargo"] = "Debe seleccionar un Cargo.";
        }

        this.setState({ errores: errors });
    }

    handleChange(event) {

        if (event.target.value != null) {
            this.setState({ loading: true });
            this.GetCargosDisponibles(event.target.value);
            this.GetTargetCargos(event.target.value);

        } else {
            this.GetCatalogos();
        }
        this.setState({ [event.target.name]: event.target.value });
    }

}


ReactDOM.render(
    <CrearCargosSector />,
    document.getElementById('content-crear-cargos-sector')
);