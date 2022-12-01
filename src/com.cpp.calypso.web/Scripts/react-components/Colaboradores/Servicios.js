import React from 'react';
import axios from 'axios';
import { MultiSelect } from 'primereact/components/multiselect/MultiSelect';

export default class Servicios extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            id_colaborador: '',
            destino_estancia: 0,
            colaborador: [],
            comidas: [],
            visible: false,
            alimentacion: false,
            opcion_comida: [],
            nro_comidas: 0,
            alojamiento: false,
            nombre_hotel: 0,
            tipo_habitacion: 0,
            movilizacion: false,
            tipo_movilizacion: 0,
            parroquia: 0,
            comunidad: 0,
            lavanderia: false,
            errores: [],
            formIsValid: '',
            nombresHoteles: [],
            tiposHabitaciones: [],
            tiposMovilizacion: [],
            parroquias: [],
            comunidades: [],
            tiposComidas: [],
            comidas_col: [],
            tiposServicios: [],
            servicios_colaborador: [],
            serviciosConsulta: [],
            movilizacionConsulta: [],
        }

        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.Regresar = this.Regresar.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleValidation = this.handleValidation.bind(this);

        this.ConsultaServicios = this.ConsultaServicios.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
        this.getFormSelectTiposComidas = this.getFormSelectTiposComidas.bind(this);
        this.getFormSelectTipoMovilizacion = this.getFormSelectTipoMovilizacion.bind(this);

        this.arrayComidas = this.arrayComidas.bind(this);
        this.arrayServicios = this.arrayServicios.bind(this);

        this.VerificarServiciosPorDestino = this.VerificarServiciosPorDestino.bind(this);
        this.VerificarComidas = this.VerificarComidas.bind(this);
        this.ConsultaConfiguracion = this.ConsultaConfiguracion.bind(this);
        this.ConsultaDestinoAlimentacion = this.ConsultaDestinoAlimentacion.bind(this);
    }


    componentDidMount() {
        this.GetCatalogos();
        // this.ConsultaServicios();
    }

    render() {
        return (
            <div>
                <div className="row">
                    <div className="col-1"></div>
                    <div className="col-xs-12 col-md-8">
                        <form onSubmit={this.handleSubmit}>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '32px' }}>
                                        <label htmlFor="alimentacion" style={{ width: '285px' }}>Alimentación: ({this.state.nro_comidas})</label>
                                        <input type="checkbox" id="alimentacion" className="form-control" checked={this.state.alimentacion} onChange={this.handleInputChange} name="alimentacion" style={{ marginTop: '5px', marginLeft: '-30%' }} />
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <div className="content-section implementation">
                                            <label htmlFor="opcion_comida">* Tipo de Comida: </label>
                                            <MultiSelect value={this.state.opcion_comida} optionLabel="nombre" options={this.state.tiposComidas} dataKey="Id" defaultLabel="Seleccione..." onChange={(e) => this.setState({ opcion_comida: e.value })}
                                                disabled={!this.state.alimentacion} style={{ width: '100%' }} filter={true} />
                                        </div>
                                        <span style={{ color: "red", marginLeft: '5%' }}>{this.state.errores["opcion_comida"]}</span>
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group checkbox" style={{ display: 'inline-flex' }}>
                                        <label htmlFor="alojamiento" style={{ width: '285px' }}>Alojamiento: </label>
                                        <input type="checkbox" id="alojamiento" className="form-control" checked={this.state.alojamiento} onChange={this.handleInputChange} name="alojamiento" style={{ marginTop: '5px', marginLeft: '-30%' }} />
                                    </div>
                                </div>
                                <div className="col">
                                </div>
                            </div>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '5%' }}>
                                        <label htmlFor="movilizacion" style={{ width: '285px' }}>Movilización: </label>
                                        <input type="checkbox" id="movilizacion" className="form-control" checked={this.state.movilizacion} onChange={this.handleInputChange} name="movilizacion" style={{ marginTop: '5px', marginLeft: '-30%' }} />
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group" style={{ marginTop: '-5%' }}>
                                        <label htmlFor="tipo_movilizacion">* Tipo de Movilizacion: </label>
                                        <select value={this.state.tipo_movilizacion} onChange={this.handleChange} className="form-control" name="tipo_movilizacion" disabled={!this.state.movilizacion}>
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectTipoMovilizacion()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["tipo_movilizacion"]}</span>
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '5%' }}>
                                        <label htmlFor="lavanderia" style={{ width: '285px' }}>Lavandería: </label>
                                        <input type="checkbox" id="lavanderia" className="form-control" checked={this.state.lavanderia} onChange={this.handleInputChange} name="lavanderia" style={{ marginTop: '5px', marginLeft: '-30%' }} />
                                    </div>
                                </div>
                                <div className="col">
                                </div>
                            </div>

                            <br />
                            <div className="form-group">
                                <div className="col">
                                    <button type="submit" className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                    <button onClick={() => this.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                                </div>
                            </div>

                        </form>
                    </div>
                </div>
            </div>

        )
    }

    handleValidation() {
        let errors = {};
        this.state.formIsValid = true;

        if (this.state.alimentacion == true) {
            if (this.state.opcion_comida.length == 0) {
                this.state.formIsValid = false;
                errors["opcion_comida"] = "El campo Tipo de Comida es obligatorio.";
            }
        }
        if (this.state.movilizacion == true) {
            if (!this.state.tipo_movilizacion) {
                this.state.formIsValid = false;
                errors["tipo_movilizacion"] = "El campo Tipo de Movilización es obligatorio.";
            }
        }

        this.setState({ errores: errors });
        return this.state.formIsValid;
    }

    handleSubmit(event) {
        event.preventDefault();
        this.arrayServicios();
        // console.log('servicios_colaborador', this.state.servicios_colaborador)
        // console.log('comidas_col', this.state.comidas_col);
        this.handleValidation();
        if (this.state.formIsValid) {

            axios.post("/RRHH/Colaboradores/CreateServiciosApi/", {
                servicios: this.state.servicios_colaborador,
                idComidas: this.state.comidas_col,
                movilizacion: this.state.tipo_movilizacion
            })
                .then((response) => {
                    console.log(response);
                    this.props.successMessage("Servicios Guardados!");
                    this.props.Siguiente(4);
                })
                .catch((error) => {
                    console.log(error);
                    this.props.warnMessage("Algo salió mal.");
                });


        }else{
            this.props.warnMessage("Se ha encontrado errores, por favor revisar el formulario");
        }
    }

    arrayServicios() {
        let arrayServicios = [];
        // this.state.alimentacion ? servicios.push()

        if (this.state.alimentacion == true) {
            let s = {};
            var alimentacion = this.state.tiposServicios.filter(c => c.codigo == "SALMUERZO");

            s.ColaboradoresId = this.props.id_colaborador;
            s.ServicioId = alimentacion[0].Id;
            s.nombre = "alimentacion";

            // console.log('s',s)
            arrayServicios.push(s);
            this.arrayComidas();
        }
        if (this.state.alojamiento == true) {
            let s = {};
            var alojamiento = this.state.tiposServicios.filter(c => c.codigo == "SHOSPEDAJE");
            // console.log('alojamiento',alojamiento)
            s.ColaboradoresId = this.props.id_colaborador;
            s.ServicioId = alojamiento[0].Id;
            s.nombre = "alojamiento";

            // console.log('s',s)
            arrayServicios.push(s);
        }
        if (this.state.movilizacion == true) {
            let s = {};
            var movilizacion = this.state.tiposServicios.filter(c => c.codigo == "STRASPORTE");
            // console.log('movilizacion',movilizacion)
            s.ColaboradoresId = this.props.id_colaborador;
            s.ServicioId = movilizacion[0].Id;
            s.nombre = "movilizacion";

            // console.log('s',s)
            arrayServicios.push(s);
        }
        if (this.state.lavanderia == true) {
            let s = {};
            var lavanderia = this.state.tiposServicios.filter(c => c.codigo == "SLAVANDERIA");
            // console.log('lavanderia',lavanderia)
            s.ColaboradoresId = this.props.id_colaborador;
            s.ServicioId = lavanderia[0].Id;
            s.nombre = "lavanderia";

            // console.log('s',s)
            arrayServicios.push(s);
        }
        // console.log('arrayServicios',arrayServicios)
        this.state.servicios_colaborador = arrayServicios;

    }

    arrayComidas() {
        let comidas = this.state.opcion_comida.slice();
        let ids = [];
        comidas.forEach(element => {
            ids.push(element.Id)
        });
        this.state.comidas_col = ids.slice();
        // console.log('comidas_col', this.state.comidas_col);

    }

    ConsultaConfiguracion(id) {
        axios.post("/RRHH/ServicioDestino/GetServiciosApi/", {})
            .then((response) => {
                // console.log('conf',response.data);
                // console.log('destino',this.props.destino_estancia)
                if (id > 0) {
                    var config = response.data.filter(c => c.destinoId == id);
                    // console.log('config',config);
                    this.VerificarServiciosPorDestino(config[0])
                }
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false })
            });
    }

    VerificarServiciosPorDestino(servicio) {

        if (servicio != undefined) {
            // console.log(servicio)
            this.setState({
                alimentacion: servicio.alimentacion,
                alojamiento: servicio.alojamiento,
                lavanderia: servicio.lavanderia,
                movilizacion: servicio.movilizacion,
            })

            if (servicio.alimentacion == true) {
                this.ConsultaDestinoAlimentacion(servicio.Id)
            }
        }
        this.setState({ loading: false })
    }

    ConsultaDestinoAlimentacion(id) {
        this.setState({ loading: true })
        axios.post("/RRHH/ServicioDestino/GetComidasApi/", {
            Id: id
        })
            .then((response) => {
                // console.log('conf', response.data);
                this.VerificarComidas(response.data)
            })
            .catch((error) => {
                console.log(error);
            });
    }

    VerificarComidas(c) {
        let ids = [];
        // console.log('c', c)
        // console.log('comidas', comidas)

        c.forEach(e => {
            var find = this.state.tiposComidas.filter(c => c.Id == e.tipo_comida);
            ids.push(find[0]);
        });

        this.setState({
            opcion_comida: this.state.opcion_comida = ids.slice(),
            nro_comidas: c.length,
            loading: false
        })

    }


    Regresar() {
        return (
            window.location.href = "/RRHH/Colaboradores/Servicios/"
        );
    }

    GetCatalogos(d) {
        let codigos = [];
        codigos = ['DESTINOS', 'TIPOCOMIDA', 'TIPODEMOVILIZACION'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                //console.log(response.data);
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
            if (codigoCatalogo == 'DESTINOS') {
                this.setState({ tiposDestinos: catalogo })
                return;
            }
            else if (codigoCatalogo == 'TIPOCOMIDA') {
                this.setState({ tiposComidas: catalogo })
                this.getFormSelectTiposComidas()
                return;
            }
            else if (codigoCatalogo == 'TIPODEMOVILIZACION') {
                this.setState({ tiposMovilizacion: catalogo })
                this.getFormSelectTipoMovilizacion()
                return;
            }

        });
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

    getFormSelectTipoMovilizacion() {
        return (
            this.state.tiposMovilizacion.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    ConsultaServicios(id) {
        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", {
            codigo: 'SERVICIO'
        })
            .then((response) => {
                // console.log('tiposServicios', response.data)
                this.setState({
                    tiposServicios: response.data
                })
                this.ConsultaConfiguracion(id);
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false })
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

}