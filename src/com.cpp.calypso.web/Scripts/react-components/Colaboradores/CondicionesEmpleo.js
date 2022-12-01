import React from 'react';
import axios from 'axios';
import { Dialog } from 'primereact/components/dialog/Dialog';
import { Button } from 'primereact/components/button/Button';

export default class CondicionesEmpleo extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            id_candidato: '',
            meta4: '',
            sitio_trabajo: '0',
            destino: '',
            rotacion: '',
            area: '',
            cargo: '',
            vinculo_lab: '',
            clase: '',
            encuadre: '',
            beneficios: '',
            salud: '',
            dependiente: '',
            beneficios_dep: '',
            asociacion: '',
            medico: '',
            errores: [],
            formIsValid: '',
            gruposPersonal: [],
            tiposDestino: [],
            tiposRotaciones: [],
            areas: [],
            tiposCargos: [],
            vinculosLaborales: [],
            clases: [],
            encuadres: [],
            horarios: [],
            planBeneficios: [],
            planSalud: [],
            coberturaDependientes: [],
            planesBeneficios: [],
            asociaciones: [],
            aptosMedico: [],
            estado: false,
            tiposSitios: [],
            encargado_personal: '',
            tiposEncargadoPersonal: [],
            sector: '',
            tiposSectores: [],
        }

        this.handleChange = this.handleChange.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.Guardar = this.Guardar.bind(this);
        this.handleChangeSector = this.handleChangeSector.bind(this);

        this.getFormSelectGrupoPersonal = this.getFormSelectGrupoPersonal.bind(this);
        this.getFormSelectDestinos = this.getFormSelectDestinos.bind(this);
        this.GetRotaciones = this.GetRotaciones.bind(this);
        this.getFormSelectRotacion = this.getFormSelectRotacion.bind(this);
        this.getFormSelectAreas = this.getFormSelectAreas.bind(this);
        this.getFormSelectCargo = this.getFormSelectCargo.bind(this);
        this.getFormSelectVinculoLaboral = this.getFormSelectVinculoLaboral.bind(this);
        this.getFormSelectClase = this.getFormSelectClase.bind(this);
        this.getFormSelectEncuadre = this.getFormSelectEncuadre.bind(this);
        this.getFormSelectPlanBeneficios = this.getFormSelectPlanBeneficios.bind(this);
        this.getFormSelectPlanSalud = this.getFormSelectPlanSalud.bind(this);
        this.getFormSelectCoberturaDependiente = this.getFormSelectCoberturaDependiente.bind(this);
        this.getFormSelectPlanesBeneficios = this.getFormSelectPlanesBeneficios.bind(this);
        this.getFormSelectAsociaciones = this.getFormSelectAsociaciones.bind(this);
        this.getFormSelectAptoMedico = this.getFormSelectAptoMedico.bind(this);
        this.getFormSelectSitioTrabajo = this.getFormSelectSitioTrabajo.bind(this);
        this.getFormSelectEncargadoPersonal = this.getFormSelectEncargadoPersonal.bind(this);
        this.getFormSelectSector = this.getFormSelectSector.bind(this);
        this.GetHorarios = this.GetHorarios.bind(this);

        this.abrirConfirmacion = this.abrirConfirmacion.bind(this);
        this.cerrarConfirmacion = this.cerrarConfirmacion.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);

        this.GetCargosSector = this.GetCargosSector.bind(this);
    }

    componentDidMount() {
        this.GetCatalogos();
        this.GetRotaciones();
        // this.GetHorarios();
    }

    render() {
        const footer = (
            <div>
                <Button label="Yes" icon="pi pi-check" onClick={() => this.Guardar()} />
                <Button label="No" icon="pi pi-times" onClick={() => this.cerrarConfirmacion()} className="p-button-secondary" />
            </div>
        );
        return (
            <div>
                <div className="row">
                    <div className="col-xs-12 col-md-12">
                        <form onSubmit={this.handleSubmit}>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="meta4">Meta 4:</label>
                                        <input type="text" id="meta4" className="form-control" value={this.state.meta4} onChange={this.handleChange} name="meta4" disabled />

                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="encargado_personal">* Encargado de Personal: </label>
                                        <select value={this.props.colaborador.catalogo_encargado_personal_id} onChange={this.handleChange} className="form-control" name="encargado_personal" disabled>
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectEncargadoPersonal()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["encargado_personal"]}</span>
                                    </div>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="grupo_personal">* Agrupación para Requisitos: </label>
                                        <select value={this.state.grupo_personal} onChange={this.handleChange} className="form-control" name="grupo_personal">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectGrupoPersonal()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["grupo_personal"]}</span>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="destino">* Destino (Estancia): </label>
                                        <select value={this.props.colaborador.catalogo_destino_estancia_id} onChange={this.handleChange} className="form-control" name="destino" disabled>
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectDestinos()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["destino"]}</span>
                                    </div>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="area">* Área: </label>
                                        <select value={this.state.area} onChange={this.handleChange} className="form-control" name="area">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectAreas()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["area"]}</span>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="rotacion">* Régimen (Rotación): </label>
                                        <select value={this.state.rotacion} onChange={this.handleChange} className="form-control" name="rotacion">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectRotacion()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["rotacion"]}</span>
                                    </div>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="sector">* Sector: </label>
                                        <select value={this.state.sector} onChange={this.handleChangeSector} className="form-control" name="sector">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectSector()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["sector"]}</span>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="cargo">* Cargo: </label>
                                        <select value={this.state.cargo} onChange={this.handleChange} className="form-control" name="cargo">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectCargo()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["cargo"]}</span>
                                    </div>
                                </div>

                            </div>

                            <div className="row">

                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="clase">* Clase: </label>
                                        <select value={this.state.clase} onChange={this.handleChange} className="form-control" name="clase">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectClase()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["clase"]}</span>
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="vinculo_lab">* Vinculo Laboral: </label>
                                        <select value={this.state.vinculo_lab} onChange={this.handleChange} className="form-control" name="vinculo_lab">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectVinculoLaboral()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["vinculo_lab"]}</span>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="asociacion">* Asociación: </label>
                                        <select value={this.state.asociacion} onChange={this.handleChange} className="form-control" name="asociacion">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectAsociaciones()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["asociacion"]}</span>
                                    </div>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="encuadre">* Encuadre: </label>
                                        <select value={this.state.encuadre} onChange={this.handleChange} className="form-control" name="encuadre">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectEncuadre()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["encuadre"]}</span>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="beneficios">* Plan de Beneficios: </label>
                                        <select value={this.state.beneficios} onChange={this.handleChange} className="form-control" name="beneficios">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectPlanBeneficios()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["beneficios"]}</span>
                                    </div>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="salud">* Opción Plan de Salud: </label>
                                        <select value={this.state.salud} onChange={this.handleChange} className="form-control" name="salud">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectPlanSalud()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["salud"]}</span>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="dependiente">* Cobertura Dependiente: </label>
                                        <select value={this.state.dependiente} onChange={this.handleChange} className="form-control" name="dependiente">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectCoberturaDependiente()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["dependiente"]}</span>
                                    </div>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="beneficios_dep">* Planes de Beneficios: </label>
                                        <select value={this.state.beneficios_dep} onChange={this.handleChange} className="form-control" name="beneficios_dep">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectPlanesBeneficios()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["beneficios_dep"]}</span>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="medico">* Tipo Apto Médico: </label>
                                        <select value={this.state.medico} onChange={this.handleChange} className="form-control" name="medico">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectAptoMedico()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["medico"]}</span>
                                    </div>
                                </div>
                            </div>

                            <br />
                            <div className="form-group">
                                <div className="col">
                                    <button type="button" onClick={this.Guardar} className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                    <button onClick={() => this.props.regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                                </div>
                            </div>

                        </form>
                    </div>
                </div>
                <Dialog header="Mensaje de Confirmación" visible={this.state.estado} footer={footer} width="350px" minY={70} onHide={this.cerrarConfirmacion} maximizable={true}>
                    Está seguro de guardar la información registrada?
                    </Dialog>
            </div >
        )
    }

    abrirConfirmacion() {
        this.handleValidation();
        if (this.state.formIsValid == true) {
            this.setState({ estado: true });
        }
    }

    cerrarConfirmacion() {
        this.setState({ estado: false });
    }

    handleValidation() {
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.grupo_personal) {
            this.state.formIsValid = false;
            errors["grupo_personal"] = "El campo Agrupación para Requisitos es obligatorio.";
        }
        if (!this.state.rotacion) {
            this.state.formIsValid = false;
            errors["rotacion"] = "El campo Régimen (Rotación) es obligatorio.";
        }
        if (!this.state.area) {
            this.state.formIsValid = false;
            errors["area"] = "El campo Área es obligatorio.";
        }
        if (!this.state.cargo) {
            this.state.formIsValid = false;
            errors["cargo"] = "El campo Cargo es obligatorio.";
        }
        if (!this.state.vinculo_lab) {
            this.state.formIsValid = false;
            errors["vinculo_lab"] = "El campo Vinculo Laboral es obligatorio.";
        }
        if (!this.state.clase) {
            this.state.formIsValid = false;
            errors["clase"] = "El campo Clase es obligatorio.";
        }
        if (!this.state.encuadre) {
            this.state.formIsValid = false;
            errors["encuadre"] = "El campo Encuadre es obligatorio.";
        }
        if (!this.state.beneficios) {
            this.state.formIsValid = false;
            errors["beneficios"] = "El campo Plan de Beneficios es obligatorio.";
        }
        if (!this.state.salud) {
            this.state.formIsValid = false;
            errors["salud"] = "El campo Opción Plan de Salud es obligatorio.";
        }
        if (!this.state.dependiente) {
            this.state.formIsValid = false;
            errors["dependiente"] = "El campo Cobertura Dependiente es obligatorio.";
        }
        if (!this.state.beneficios_dep) {
            this.state.formIsValid = false;
            errors["beneficios_dep"] = "El campo Planes de Beneficios es obligatorio.";
        }
        if (!this.state.asociacion) {
            this.state.formIsValid = false;
            errors["asociacion"] = "El campo Asociación es obligatorio.";
        }
        if (!this.state.medico) {
            this.state.formIsValid = false;
            errors["medico"] = "El campo Tipo Apto Médico es obligatorio.";
        }
        if (!this.state.sector) {
            this.state.formIsValid = false;
            errors["sector"] = "El campo Sector es obligatorio.";
        }

        this.setState({ errores: errors });
        return this.state.formIsValid;
    }

    Guardar() {

        this.handleValidation();
        console.log('idc', this.props.id_colaborador);

        if (this.state.formIsValid) {
            this.props.colaborador.Id = this.props.id_colaborador;
            this.props.colaborador.catalogo_grupo_personal_id = this.state.grupo_personal;
            // this.props.colaborador.catalogo_destino_estancia_id = this.state.destino;
            this.props.colaborador.catalogo_sitio_trabajo_id = this.state.sitio_trabajo;
            this.props.colaborador.AdminRotacionId = this.state.rotacion;
            this.props.colaborador.catalogo_area_id = this.state.area;
            this.props.colaborador.catalogo_cargo_id = this.state.cargo;
            this.props.colaborador.catalogo_vinculo_laboral_id = this.state.vinculo_lab;
            this.props.colaborador.catalogo_clase_id = this.state.clase;
            this.props.colaborador.catalogo_encuadre_id = this.state.encuadre;
            this.props.colaborador.catalogo_plan_beneficios_id = this.state.beneficios;
            this.props.colaborador.catalogo_plan_salud_id = this.state.salud;
            this.props.colaborador.catalogo_cobertura_dependiente_id = this.state.dependiente;
            this.props.colaborador.catalogo_planes_beneficios_id = this.state.beneficios_dep;
            this.props.colaborador.catalogo_asociacion_id = this.state.asociacion;
            this.props.colaborador.catalogo_apto_medico_id = this.state.medico;
            // this.props.colaborador.catalogo_encargado_personal_id = this.state.encargado_personal;
            this.props.colaborador.catalogo_sector_id = this.state.sector;

            axios.post("/RRHH/Colaboradores/CreateEmpleoApi/", {
                colaborador: this.props.colaborador
            })
                .then((response) => {
                    console.log(response);
                    this.props.successMessage("Condiciones Empleo Guardadas!");
                    this.props.Siguiente(3);
                    this.cerrarConfirmacion();
                })
                .catch((error) => {
                    console.log(error);
                    this.props.warnMessage("Algo salió mal.");
                });
        } else {
            this.props.warnMessage("Se ha encontrado errores, por favor revisar el formulario");
        }

    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['GRUPOPERSONAL', 'DESTINOS', 'AREA',
            'VINCULOLABORAL', 'CLASE', 'ENCUADRE', 'PLANDEBENEFICIOS', 'OPCIONPLANSALUD',
            'COBERTURADEPENDIENTE', 'PLANESDEBENEFICIOS', 'ASOCIACION', 'TIPOAPTOMEDICO',
            'SITIODETRABAJO', 'ENCARGADO', 'SECTOR'];

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

            switch (codigoCatalogo) {
                case 'GRUPOPERSONAL':
                    this.setState({ gruposPersonal: catalogo })
                    this.getFormSelectGrupoPersonal();
                    return;
                case 'DESTINOS':
                    this.setState({ tiposDestino: catalogo })
                    this.getFormSelectDestinos();
                    return;
                case 'AREA':
                    this.setState({ areas: catalogo })
                    this.getFormSelectAreas();
                    return;
                case 'VINCULOLABORAL':
                    this.setState({ vinculosLaborales: catalogo })
                    this.getFormSelectVinculoLaboral();
                    return;
                case 'CLASE':
                    this.setState({ clases: catalogo })
                    this.getFormSelectClase();
                    return;
                case 'ENCUADRE':
                    this.setState({ encuadres: catalogo })
                    this.getFormSelectEncuadre();
                    return;
                case 'PLANDEBENEFICIOS':
                    this.setState({ planBeneficios: catalogo })
                    this.getFormSelectPlanBeneficios();
                    return;
                case 'OPCIONPLANSALUD':
                    this.setState({ planSalud: catalogo })
                    this.getFormSelectPlanSalud();
                    return;
                case 'COBERTURADEPENDIENTE':
                    this.setState({ coberturaDependientes: catalogo })
                    this.getFormSelectCoberturaDependiente();
                    return;
                case 'PLANESDEBENEFICIOS':
                    this.setState({ planesBeneficios: catalogo })
                    this.getFormSelectPlanesBeneficios();
                    return;
                case 'ASOCIACION':
                    this.setState({ asociaciones: catalogo })
                    this.getFormSelectAsociaciones();
                    return;
                case 'TIPOAPTOMEDICO':
                    this.setState({ aptosMedico: catalogo })
                    this.getFormSelectAptoMedico();
                    return;
                case 'ENCARGADO':
                    this.setState({ tiposEncargadoPersonal: catalogo })
                    this.getFormSelectEncargadoPersonal();
                    return;
                case 'SECTOR':
                    this.setState({ tiposSectores: catalogo })
                    this.getFormSelectSector();
                    return;
                default:
                    console.log(codigoCatalogo)
                    return;
            }


        });
    }

    getFormSelectSector() {
        return (
            this.state.tiposSectores.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectEncargadoPersonal() {
        return (
            this.state.tiposEncargadoPersonal.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectSitioTrabajo() {
        return (
            this.state.tiposSitios.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectGrupoPersonal() {
        return (
            this.state.gruposPersonal.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectDestinos() {
        return (
            this.state.tiposDestino.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectAreas() {
        return (
            this.state.areas.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectCargo() {
        console.log(this.state.tiposCargos);
        this.state.tiposCargos.sort((a, b) => a.Cargo.nombre.localeCompare(b.Cargo.nombre))
        console.log(this.state.tiposCargos);
        return (
     
            this.state.tiposCargos.map((item) => {
                return (
                    <option key={Math.random()} value={item.Cargo.Id}>{item.Cargo.nombre}</option>
                )
            })
        );
    }

    getFormSelectVinculoLaboral() {
        return (
            this.state.vinculosLaborales.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectClase() {
        return (
            this.state.clases.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectEncuadre() {
        return (
            this.state.encuadres.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    GetHorarios() {
        axios.post("/RRHH/Horario/GetHorariosApi/", {})
            .then((response) => {
                this.setState({ horarios: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    getFormSelectPlanBeneficios() {
        return (
            this.state.planBeneficios.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectPlanSalud() {
        return (
            this.state.planSalud.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectCoberturaDependiente() {
        return (
            this.state.coberturaDependientes.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectPlanesBeneficios() {
        return (
            this.state.planesBeneficios.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectAsociaciones() {
        return (
            this.state.asociaciones.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectAptoMedico() {
        return (
            this.state.aptosMedico.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    GetRotaciones() {
        axios.post("/RRHH/AdminRotacion/GetListadoRotacionesApi/", {})
            .then((response) => {
                this.setState({ tiposRotaciones: response.data })
                //envio
                this.getFormSelectRotacion()
            })
            .catch((error) => {
                console.log(error);
            });
    }

    getFormSelectRotacion() {
        return (
            this.state.tiposRotaciones.map((item) => {
                return (
                    <option key={item.Id} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleChangeSector(event) {
        this.setState({ [event.target.name]: event.target.value });
        this.GetCargosSector(event.target.value);
    }

    GetCargosSector(id) {
        axios.post("/RRHH/CargosSector/GetCargosPorSectorApi/", { id: id })
            .then((response) => {
                this.setState({ tiposCargos: response.data })
                this.getFormSelectCargo();
            })
            .catch((error) => {
                console.log(error);
            });
    }

}