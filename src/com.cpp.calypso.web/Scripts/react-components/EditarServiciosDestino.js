import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import { MultiSelect } from 'primereact/components/multiselect/MultiSelect';
import BlockUi from 'react-block-ui';

export default class EditarServicioDestino extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            destinoId: '',
            destino: '',
            alojamiento: false,
            lavanderia: false,
            movilizacion: false,
            alimentacion: false,
            opcion_comida: [],
            errores: [],
            destinos: [],
            tiposComidas: [],
            id_comidas: [],
            comidas: [],
            servicio_comidas: [],
            formIsValid: '',
            loading: true,
        }

        this.Regresar = this.Regresar.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.handleValidation = this.handleValidation.bind(this);

        this.getFormSelectDestino = this.getFormSelectDestino.bind(this);
        this.getFormSelectTiposComidas = this.getFormSelectTiposComidas.bind(this);

        this.Select = this.Select.bind(this);
        this.ConsultaServicio = this.ConsultaServicio.bind(this);
        this.GetComidas = this.GetComidas.bind(this);
        this.Comidas = this.Comidas.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
    }

    componentDidMount() {
        this.GetCatalogos();
        this.GetComidas();
        this.ConsultaServicio(sessionStorage.getItem('id_servicio'));
    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    <form onSubmit={this.handleSubmit}>

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="destino">* Destino: </label>
                                    <select value={this.state.destino} onChange={this.handleChange} className="form-control" name="destino" disabled>
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectDestino()}
                                    </select>
                                    <span style={{ color: "red" }}>{this.state.errores["destino"]}</span>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '10%' }}>
                                    <label htmlFor="alojamiento" style={{ width: '294px' }}>Alojamiento: </label>
                                    <input type="checkbox" id="alojamiento" className="form-control" checked={this.state.alojamiento} onChange={this.handleInputChange} name="alojamiento" style={{ marginTop: '5px', marginLeft: '-100%' }} />
                                </div>
                            </div>

                        </div>

                        <div className="row">
                            <div className="col">
                                <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '15px' }}>
                                    <label htmlFor="lavanderia" style={{ width: '294px' }}>Lavandería: </label>
                                    <input type="checkbox" id="lavanderia" className="form-control" checked={this.state.lavanderia} onChange={this.handleInputChange} name="lavanderia" style={{ marginTop: '5px', marginLeft: '-100%' }} />
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '15px' }}>
                                    <label htmlFor="movilizacion" style={{ width: '294px' }}>Movilización: </label>
                                    <input type="checkbox" id="movilizacion" className="form-control" checked={this.state.movilizacion} onChange={this.handleInputChange} name="movilizacion" style={{ marginTop: '5px', marginLeft: '-100%' }} />
                                </div>
                            </div>

                        </div>

                        <div className="row">
                            <div className="col">
                                <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '12%' }}>
                                    <label htmlFor="alimentacion" style={{ width: '294px' }}>Alimentación: </label>
                                    <input type="checkbox" id="alimentacion" className="form-control" checked={this.state.alimentacion} onChange={this.handleInputChange} name="alimentacion" style={{ marginTop: '5px', marginLeft: '-100%' }} />
                                </div>
                            </div>
                            <div className="col" style={{ visibility: this.state.alimentacion == true ? 'visible' : 'hidden' }}>
                                <div className="form-group">
                                    <div className="content-section implementation">
                                        <label htmlFor="opcion_comida" style={{ width: '294px' }}>* Tipo de Alimentación: </label>
                                        <MultiSelect value={this.state.opcion_comida} optionLabel="nombre" options={this.state.tiposComidas} dataKey="Id" defaultLabel="Seleccione..." onChange={(e) => this.setState({ opcion_comida: e.value })}  /*onChange={(e) => this.Select(e.value)}*/
                                            disabled={!this.state.alimentacion} style={{ width: '100%' }} filter={true} />
                                    </div>
                                    <span style={{ color: "red"}}>{this.state.errores["opcion_comida"]}</span>
                                </div>
                            </div>
                            <span style={{ color: "red"}}>{this.state.errores["servicios"]}</span>
                        </div>
                        <br/>
                        <div className="form-group" >
                            <div className="col">
                                <button type="submit" className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                <button onClick={() => this.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                            </div>
                        </div>
                    </form>
                </div >
            </BlockUi>
        )
    }

    ConsultaServicio(id) {
        axios.post("/RRHH/ServicioDestino/GetServicioApi/" + id, {})
            .then((response) => {
                console.log(response)
                this.setState({
                    destino: response.data.destinoId,
                    alojamiento: response.data.alojamiento,
                    lavanderia: response.data.lavanderia,
                    movilizacion: response.data.movilizacion,
                    alimentacion: response.data.alimentacion,
                    id_comidas: response.data.comidas,
                    destinoId: response.data.destinoId,
                    key_form: Math.random()
                })
                
            })
            .catch((error) => {
                console.log(error);
            });

    }

    GetComidas() {
        axios.post("/RRHH/ServicioDestino/GetComidasApi/", {
            Id: sessionStorage.getItem('id_servicio')
        })
            .then((response) => {
                this.setState({ comidas: response.data });
                this.Comidas();
            })
            .catch((error) => {
                console.log(error);
            });
    }

    Comidas() {
        let f = this.state.tiposComidas.slice();
        console.log('f',f)
        let ids = [];
        console.log('comidas',this.state.comidas)
        this.state.comidas.forEach(element => {
            var id = Number.parseInt(element.tipo_comida, 10);
            var aux = f.find(c => c.Id === id);
            console.log('aux',aux)
            if (aux != undefined) {
                ids.push(aux);
            }
        });

        this.setState({ opcion_comida: ids });
    }

    handleValidation() {
        console.log('entro a validacion');
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.destino) {
            this.state.formIsValid = false;
            errors["destino"] = "El campo Destino es obligatorio.";
        }
        if (this.state.alimentacion == true) {
            if (this.state.opcion_comida == '') {
                this.state.formIsValid = false;
                errors["opcion_comida"] = "El campo Tipo de Alimentación es obligatorio.";
            }
        }

        if (this.state.alimentacion == false 
            && this.state.alojamiento == false 
            && this.state.movilizacion == false
            && this.state.lavanderia == false ) {
            this.state.formIsValid = false;
            errors["servicios"] = "Seleccione al menos un servicio.";
        }

        this.setState({ errores: errors });
    }

    handleSubmit(event) {
        event.preventDefault();
        this.Select();
        this.handleValidation();
        console.log(this.state.opcion_comida);
        console.log(this.state.id_comidas);
        if (this.state.formIsValid) {
            axios.post("/RRHH/ServicioDestino/EditApiAsync/", {
                Id: sessionStorage.getItem('id_servicio'),
                destinoId: this.state.destinoId,
                alojamiento: this.state.alojamiento,
                lavanderia: this.state.lavanderia,
                movilizacion: this.state.movilizacion,
                alimentacion: this.state.alimentacion,
                idComidas: this.state.alimentacion == true ? this.state.id_comidas : '',
            })
                .then((response) => {
                    console.log(response);
                    this.setState({ loading: true })
                    this.successMessage("Servicio actualizado!")
                    setTimeout(
                        function() {
                            this.Regresar()
                        }.bind(this),2000
                    );
                })
                .catch((error) => {
                    console.log(error);
                    this.warnMessage("Algo salió mal.");
                });

        }
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['DESTINOS', 'TIPOCOMIDA'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
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
            switch (codigoCatalogo) {
                case 'DESTINOS':
                    this.setState({ destinos: catalogo })
                    this.getFormSelectDestino()
                    return;
                case 'TIPOCOMIDA':
                    this.setState({ tiposComidas: catalogo })
                    this.getFormSelectTiposComidas()
                    this.GetComidas();
                    return;
            }
        });
        this.setState({ loading: false })
    }

    getFormSelectDestino() {
        return (
            this.state.destinos.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id} id={item.nombre}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectTiposComidas() {
        return (
            this.state.tiposComidas.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    Select() {
        let selectedFrentes = this.state.opcion_comida.slice();
        let ids = [];
        selectedFrentes.forEach(element => {
            ids.push(element.Id)
        });
        this.state.id_comidas = ids.slice();
    }

    Regresar() {
        return (
            window.location.href = "/RRHH/ServicioDestino/Index/"
        );
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
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

}


ReactDOM.render(
    <EditarServicioDestino />,
    document.getElementById('content-editar-servicios')
);