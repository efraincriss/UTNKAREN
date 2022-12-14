import React from 'react';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import { MultiSelect } from 'primereact/components/multiselect/MultiSelect';

export default class EditarServicios extends React.Component {

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
            loading: true,
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

        this.ConsultaComidas = this.ConsultaComidas.bind(this);
        this.ConsultaServicios = this.ConsultaServicios.bind(this);
        this.ConsultaServiciosColaborador = this.ConsultaServiciosColaborador.bind(this);
        this.ConsultaMovilizaciones = this.ConsultaMovilizaciones.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
        this.getFormSelectTiposComidas = this.getFormSelectTiposComidas.bind(this);
        this.getFormSelectTipoMovilizacion = this.getFormSelectTipoMovilizacion.bind(this);

        this.arrayComidas = this.arrayComidas.bind(this);
        this.arrayServicios = this.arrayServicios.bind(this);

        this.VerificarServiciosPorDestino = this.VerificarServiciosPorDestino.bind(this);
        this.VerificarComidas = this.VerificarComidas.bind(this);
        this.cargarDatosServicios = this.cargarDatosServicios.bind(this);
        this.ConsultaConfiguracion = this.ConsultaConfiguracion.bind(this);
        this.ConsultaDestinoAlimentacion = this.ConsultaDestinoAlimentacion.bind(this);
    }


    componentDidMount() {
        // this.GetCatalogos()
    }

    componentWillReceiveProps(nextProps) {
        this.setState({
            id_colaborador: nextProps.id_colaborador,
            destino_estancia: nextProps.destino_estancia
        })
    }


    ConsultaServiciosColaborador() {
        axios.post("/RRHH/Colaboradores/GetServiciosColaboradorApi/", { Id: this.state.id_colaborador })
            .then((response) => {
                this.setState({
                    serviciosConsulta: response.data
                })
                if (response.data.length > 0) {
                    console.log('serviciosConsulta',response.data)
                    response.data.forEach(e => {
                        if (e.Catalogo.codigo == "SALMUERZO") {
                            this.ConsultaComidas(e.Id);
                            this.setState({
                                alimentacion: true
                            })
                        } else if (e.Catalogo.codigo == "STRASPORTE") {
                            this.ConsultaMovilizaciones(e.Id);
                            this.setState({
                                movilizacion: true
                            })
                        }
                        else if (e.Catalogo.codigo == "SLAVANDERIA") {
                            this.setState({
                                lavanderia: true
                            })
                        }
                        else if (e.Catalogo.codigo == "SHOSPEDAJE") {
                            this.setState({
                                alojamiento: true
                            })
                        }
                    });
                    this.setState({ loading: false })
                } else {
                    // console.log('nuevooo')
                    this.ConsultaConfiguracion()
                }

            })
            .catch((error) => {
                console.log(error);
            });
    }


    ConsultaComidas(Id) {
        this.setState({ loading: true })
        axios.post("/RRHH/Colaboradores/GetComidasApi/", { id: Id })
            .then((response) => {
                // console.log('comidas', response.data)
                this.setState({
                    comidas: response.data,
                    nro_comidas: response.data.length
                })
                this.cargarDatosServicios()
            })
            .catch((error) => {
                console.log(error);
            });
    }

    ConsultaMovilizaciones(Id) {
        axios.post("/RRHH/Colaboradores/GetMovilizacionApi/", { id: Id })
            .then((response) => {
                this.setState({
                    movilizacionConsulta: response.data
                })

                if (this.state.movilizacionConsulta != null) {
                    if (this.state.movilizacionConsulta.catalogo_tipo_movilizacion_id > 0) {
                        this.setState({
                            tipo_movilizacion: this.state.movilizacionConsulta.catalogo_tipo_movilizacion_id
                        })
                    }
                }

            })
            .catch((error) => {
                console.log(error);
            });
    }

    cargarDatosServicios() {

        let comidas = this.state.comidas.slice();
        let ids = [];
        comidas.forEach(e => {
            var c = this.state.tiposComidas.filter(c => c.Id === e.tipo_alimentacion_id);
            ids.push(c[0]);
        });

        this.setState({
            opcion_comida: this.state.opcion_comida = ids.slice(),
            loading: false
        })

    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <div className="row">
                        <div className="col-1"></div>
                        <div className="col-xs-12 col-md-8">
                            <form onSubmit={this.handleSubmit}>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '32px' }}>
                                            <label htmlFor="alimentacion" style={{ width: '285px' }}>Alimentaci??n: ({this.state.nro_comidas})</label>
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
                                            <label htmlFor="movilizacion" style={{ width: '285px' }}>Movilizaci??n: </label>
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
                                            <label htmlFor="lavanderia" style={{ width: '285px' }}>Lavander??a: </label>
                                            <input type="checkbox" id="lavanderia" className="form-control" checked={this.state.lavanderia} onChange={this.handleInputChange} name="lavanderia" style={{ marginTop: '5px', marginLeft: '-30%' }} />
                                        </div>
                                    </div>
                                    <div className="col">
                                    </div>
                                </div>

                                <br />
                                <div className="form-group">
                                    <div className="col">
                                        <button type="submit" className="btn btn-outline-primary fa fa-save" disabled={this.props.disable_button}> Guardar</button>
                                        <button onClick={() => this.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                                    </div>
                                </div>

                            </form>
                        </div>
                    </div>
                </div>
            </BlockUi >
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
                errors["tipo_movilizacion"] = "El campo Tipo de Movilizaci??n es obligatorio.";
            }
        }

        this.setState({ errores: errors });
        return this.state.formIsValid;
    }

    handleSubmit(event) {
        event.preventDefault();
        this.arrayServicios();
        this.handleValidation();
        if (this.state.formIsValid) {
            this.setState({ loading: true });
            axios.post("/RRHH/Colaboradores/CreateServiciosApi/", {
                servicios: this.state.servicios_colaborador,
                idComidas: this.state.comidas_col,
                movilizacion: this.state.tipo_movilizacion
            })
                .then((response) => {
                    console.log(response);
                    this.setState({ loading: false });
                    this.props.successMessage("Servicios Guardados!");
                    this.props.Siguiente(4);
                })
                .catch((error) => {
                    this.setState({ loading: false });
                    console.log(error);
                    this.props.warnMessage("Algo sali?? mal.");
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

            s.ColaboradoresId = this.state.id_colaborador;
            s.ServicioId = alimentacion[0].Id;
            s.nombre = "alimentacion";
            console.log('serviciosConsulta', this.state.serviciosConsulta)
            var a = this.state.serviciosConsulta.filter(c => c.Catalogo.codigo == "SALMUERZO");
            // console.log('alimentacion',a)
            if (a.length > 0) {
                s.Id = a[0].Id;
                s.Servicio = a[0].Servicio;
            }
            // console.log('s',s)
            arrayServicios.push(s);
            this.arrayComidas();
        }
        if (this.state.alojamiento == true) {
            let s = {};
            var alojamiento = this.state.tiposServicios.filter(c => c.codigo == "SHOSPEDAJE");
            // console.log('alojamiento',alojamiento)
            s.ColaboradoresId = this.state.id_colaborador;
            s.ServicioId = alojamiento[0].Id;
            s.nombre = "alojamiento";

            var a = this.state.serviciosConsulta.filter(c => c.Catalogo.codigo == "SHOSPEDAJE");
            if (a.length > 0) {
                s.Id = a[0].Id;
                s.Servicio = a[0].Servicio;
            }
            // console.log('s',s)
            arrayServicios.push(s);
        }
        if (this.state.movilizacion == true) {
            let s = {};
            var movilizacion = this.state.tiposServicios.filter(c => c.codigo == "STRASPORTE");
            // console.log('movilizacion',movilizacion)
            s.ColaboradoresId = this.state.id_colaborador;
            s.ServicioId = movilizacion[0].Id;
            s.nombre = "movilizacion";

            var a = this.state.serviciosConsulta.filter(c => c.Catalogo.codigo == "STRASPORTE");
            if (a.length > 0) {
                s.Id = a[0].Id;
                s.Servicio = a[0].Servicio;
            }
            // console.log('s',s)
            arrayServicios.push(s);
        }
        if (this.state.lavanderia == true) {
            let s = {};
            var lavanderia = this.state.tiposServicios.filter(c => c.codigo == "SLAVANDERIA");
            // console.log('lavanderia',lavanderia)
            s.ColaboradoresId = this.state.id_colaborador;
            s.ServicioId = lavanderia[0].Id;
            s.nombre = "lavanderia";

            var a = this.state.serviciosConsulta.filter(c => c.Catalogo.codigo == "SLAVANDERIA");
            if (a.length > 0) {
                s.Id = a[0].Id;
                s.Servicio = a[0].Servicio;
            }
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

    ConsultaConfiguracion() {
        axios.post("/RRHH/ServicioDestino/GetServiciosApi/", {})
            .then((response) => {
                // console.log('ConsultaConfiguracion', response.data)
                var config = response.data.filter(c => c.destinoId == this.props.destino_estancia);
                // console.log('config', this.props.destino_estancia, config)
                this.VerificarServiciosPorDestino(config[0])
            })
            .catch((error) => {
                console.log(error);
            });
    }

    VerificarServiciosPorDestino(servicio) {

        if (servicio != undefined) {
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
        let comidas = this.state.tiposComidas.slice();
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
            window.location.href = "/RRHH/Colaboradores/Vista/"
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
                this.ConsultaServiciosColaborador();
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

    ConsultaServicios() {
        axios.post("/RRHH/Colaboradores/GetCatalogosPorCodigoApi/", {
            codigo: 'SERVICIO'
        })
            .then((response) => {
                // console.log(response.data)
                this.setState({
                    tiposServicios: response.data
                })
                this.GetCatalogos();
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