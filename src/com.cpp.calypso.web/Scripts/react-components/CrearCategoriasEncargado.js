import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { PickList } from 'primereact/components/picklist/PickList';
import { Growl } from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';

export default class CrearCategoriasEncargado extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            encargados: [],
            categoriasSource: [],
            categoriasTarget: [],
            encargado: '',
            errores: [],
            formIsValid: true,
            estado: false,
            loading: false,
        }

        this.getFormSelectEncargado = this.getFormSelectEncargado.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.Regresar = this.Regresar.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.handleChange = this.handleChange.bind(this);

        this.onChange = this.onChange.bind(this);
        this.categoriasTemplate = this.categoriasTemplate.bind(this);
        this.Guardar = this.Guardar.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
        this.GetTargetCategorias = this.GetTargetCategorias.bind(this);
        this.GetCategoriasDisponibles = this.GetCategoriasDisponibles.bind(this);
    }

    componentDidMount() {
        this.GetCatalogos();
    }

    onChange(event) {
        this.setState({
            categoriasSource: event.source,
            categoriasTarget: event.target
        });
    }

    categoriasTemplate(categoria) {

        return (
            <div className="p-clearfix">
                <div style={{ fontSize: '14px', float: 'right', margin: '15px 5px 0 0' }}>{categoria.nombre}</div><br /><br />
            </div>
        );
    }

    Guardar() {

        this.handleValidation();

        if (this.state.formIsValid == true) {

            this.setState({ loading: true });

            var idTargets = this.state.categoriasTarget.map((item) => {
                return (item.Id)
            });

            var idDisponibles = this.state.categoriasSource.map((item) => {
                return (item.Id)
            });

            axios.post("/RRHH/CategoriasEncargado/EditarCategoriaEncargadoApi/", {
                encargado: this.state.encargado,
                idCategorias: idTargets,
                idCategoriasDisponibles: idDisponibles
            })
                .then((response) => {
                    this.successMessage("Categorías Guardadas para el Encargado de Personal!");
                    this.setState({ loading: false });
                    this.Regresar();
                })
                .catch((error) => {
                    console.log(error);
                    this.warnMessage("Algo salió mal.");
                });

        }
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
                                        <label htmlFor="label">* Encargado: </label>
                                        <select value={this.state.encargado} onChange={this.handleChange} className="form-control" name="encargado">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectEncargado()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["encargado"]}</span>
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <br /><br />
                                        <div className="content-section implementation">
                                            <PickList source={this.state.categoriasSource} target={this.state.categoriasTarget} itemTemplate={this.categoriasTemplate}
                                                sourceHeader="Categorías Disponibles" targetHeader="Categorías Asignadas" responsive={true}
                                                sourceStyle={{ height: '200px' }} targetStyle={{ height: '200px' }}
                                                onChange={this.onChange}></PickList>
                                        </div>
                                        <span style={{ color: "red" }}>{this.state.errores["categoria"]}</span>
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

    GetCategoriasDisponibles(id) {
        axios.post("/RRHH/CategoriasEncargado/GetCategoriasDisponiblesApiAsync/" + id, {})
            .then((response) => {
                response.data.forEach(e => {
                    var catalogo = JSON.parse(e);
                    this.setState({ categoriasSource: catalogo })
                    return;
                });
            })
            .catch((error) => {
                console.log(error);
            });
    }

    GetTargetCategorias(id) {
        axios.post("/RRHH/CategoriasEncargado/GetCategoriasPorEncargadoApiAsync/" + id, {})
            .then((response) => {
                response.data.forEach(e => {
                    var catalogo = JSON.parse(e);
                    this.setState({ categoriasTarget: catalogo })
                    this.setState({ loading: false });
                    return;
                });
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false });
            });
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['ENCARGADO', 'CATEGORIA'];

        axios.post("/RRHH/CategoriasEncargado/GetListaCatalogosPorCodigoApi/", { codigo: codigos })
            .then((response) => {
                console.log(response.data);
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
            if (codigoCatalogo == 'ENCARGADO') {
                this.setState({ encargados: catalogo })
                this.getFormSelectEncargado();
                return;
            }
            else if (codigoCatalogo == 'CATEGORIA') {
                this.setState({ categoriasSource: catalogo })
                return;
            }
        });
    }

    getFormSelectEncargado() {
        return (
            this.state.encargados.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    Regresar() {
        return (
            window.location.href = "/RRHH/CategoriasEncargado/Index/"
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

        if (!this.state.encargado) {
            this.state.formIsValid = false;
            errors["encargado"] = "El campo Encargado es obligatorio.";
        }
        if (this.state.categoriasTarget.length == 0) {
            this.state.formIsValid = false;
            errors["categoria"] = "Debe seleccionar una Categoría.";
        }

        this.setState({ errores: errors });
    }

    handleChange(event) {

        if (event.target.value != null) {
            this.setState({ loading: true });
            this.GetCategoriasDisponibles(event.target.value);
            this.GetTargetCategorias(event.target.value);
        } else {
            this.GetCatalogos();
        }

        this.setState({ [event.target.name]: event.target.value });
    }

}


ReactDOM.render(
    <CrearCategoriasEncargado />,
    document.getElementById('content-crear-categorias-encargado')
);