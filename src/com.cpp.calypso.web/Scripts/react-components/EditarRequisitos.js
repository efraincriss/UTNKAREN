import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';

export default class EditarRequisitos extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            codigo: '',
            nombre: '',
            tipo_requisito: '',
            descripcion: '',
            responsable: '',
            caducidad: false,
            tiempo_vigencia: '',
            errores: [],
            tiposRequisito: [],
            responsables: [],
            formIsValid: '',
            alerta: '',
            activo: '',
            loading: true,
        }

        this.Regresar = this.Regresar.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);

        this.getFormSelectTipoRequisito = this.getFormSelectTipoRequisito.bind(this);
        this.getFormSelectResponsableRequisito = this.getFormSelectResponsableRequisito.bind(this);

        this.ConsultaRequisito = this.ConsultaRequisito.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);

    }

    componentDidMount() {
        this.GetCatalogos();
        this.ConsultaRequisito(sessionStorage.getItem('id_requisitos'));
    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <form onSubmit={this.handleSubmit}>

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="tipo_requisito">* Tipo de Usuario: </label>
                                    <select value={this.state.tipo_requisito} onChange={this.handleChange} className="form-control" name="tipo_requisito">
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectTipoRequisito()}
                                    </select>
                                    {this.state.errores["tipo_requisito"]}
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="codigo">* Código: </label>
                                    <input type="text" id="codigo" className="form-control" value={this.state.codigo} onChange={this.handleChangeUpperCase} name="codigo" disabled />
                                    {this.state.errores["codigo"]}
                                </div>
                            </div>

                        </div>

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="nombre">* Nombre: </label>
                                    <input type="text" id="nombre" className="form-control" value={this.state.nombre} onChange={this.handleChangeUpperCase} name="nombre" />
                                    {this.state.errores["nombre"]}
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="responsable">* Responsable de Requisito: </label>
                                    <select value={this.state.responsable} onChange={this.handleChange} className="form-control" name="responsable">
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectResponsableRequisito()}
                                    </select>
                                    {this.state.errores["responsable"]}
                                </div>
                            </div>


                        </div>

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="descripcion">* Descripción: </label>
                                    <textarea type="text" id="descripcion" /*rows="1"*/ maxLength="200" className="form-control" value={this.state.descripcion} onChange={this.handleChange} name="descripcion" />
                                    {this.state.errores["descripcion"]}
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col">
                                <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '15px' }}>
                                    <label htmlFor="caducidad" style={{ width: '294px' }}>Aplica caducidad: </label>
                                    <input type="checkbox" id="caducidad" className="form-control" checked={this.state.caducidad} onChange={this.handleInputChange} name="caducidad" style={{ marginTop: '5px', marginLeft: '-100%' }} />
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '15px' }}>
                                    <label htmlFor="activo" style={{ width: '294px' }}>Activo: </label>
                                    <input type="checkbox" id="activo" className="form-control" checked={this.state.activo} onChange={this.handleInputChange} name="activo" style={{ marginTop: '5px', marginLeft: '-100%' }} />
                                </div>
                            </div>
                        </div>
                        <div className="row" hidden={!this.state.caducidad}>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="tiempo_vigencia">* Tiempo de Vigencia (meses): </label>
                                    <input type="number" id="tiempo_vigencia" className="form-control" value={this.state.tiempo_vigencia} onChange={this.handleChange} name="tiempo_vigencia" />
                                    {this.state.errores["tiempo_vigencia"]}
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group"  >
                                    <label htmlFor="alerta">* Inicio Alerta (días): </label>
                                    <input type="number" id="alerta" className="form-control" value={this.state.alerta} onChange={this.handleChange} name="alerta" />
                                    {this.state.errores["alerta"]}
                                </div>
                            </div>
                        </div>
                        <div className="row">

                            <div className="col">
                            </div>
                        </div>
                        <div className="form-group">
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

    ConsultaRequisito(id) {
        axios.post("/RRHH/Requisitos/GetRequisitoApi/" + id, {})
            .then((response) => {
                console.log(response)
                this.setState({
                    codigo: response.data.codigo,
                    nombre: response.data.nombre,
                    tipo_requisito: response.data.requisitoId,
                    descripcion: response.data.descripcion,
                    responsable: response.data.responsableId,
                    caducidad: response.data.caducidad,
                    tiempo_vigencia: response.data.tiempo_vigencia == null ? '' : response.data.tiempo_vigencia,
                    alerta: response.data.dias_inicio_alerta == null ? '' : response.data.dias_inicio_alerta,
                    activo: response.data.activo == 0 ? '' : response.data.activo,
                    loading: false
                })
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false });
            });

    }

    handleValidation() {
        console.log('entro a validacion');
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.tipo_requisito) {
            this.state.formIsValid = false;
            errors["tipo_requisito"] = <div className="alert alert-danger">El campo Tipo Requisito es obligatorio</div>;
        }
        if (!this.state.codigo) {
            this.state.formIsValid = false;
            errors["codigo"] = <div className="alert alert-danger">El campo Código es obligatorio</div>;
        }
        if (!this.state.nombre) {
            this.state.formIsValid = false;
            errors["nombre"] = <div className="alert alert-danger">El campo Nombre es obligatorio</div>;
        }
        if (!this.state.descripcion) {
            this.state.formIsValid = false;
            errors["descripcion"] = <div className="alert alert-danger">El campo Descripcion es obligatorio</div>;
        }
        if (!this.state.responsable) {
            this.state.formIsValid = false;
            errors["responsable"] = <div className="alert alert-danger">El campo Responsable de Requisito es obligatorio</div>;
        }
        if (this.state.caducidad == true) {
            if (!this.state.tiempo_vigencia) {
                this.state.formIsValid = false;
                errors["tiempo_vigencia"] = <div className="alert alert-danger">El campo Tiempo de Vigencia es obligatorio</div>;
            } else {
                if (this.state.tiempo_vigencia < 0) {
                    this.state.formIsValid = false;
                    errors["tiempo_vigencia"] = <div className="alert alert-danger">El campo no puede ser negativo</div>;
                }
                if (this.state.tiempo_vigencia.length > 2) {
                    this.state.formIsValid = false;
                    errors["tiempo_vigencia"] = <div className="alert alert-danger">El campo no puede tener más de dos dígitos</div>;
                }
                if (!isFinite(this.state.tiempo_vigencia)) {
                    this.state.formIsValid = false;
                    errors["tiempo_vigencia"] = <div className="alert alert-danger">El campo permite solo ingreso numérico</div>;
                }
            }

            if (!this.state.alerta) {
                this.state.formIsValid = false;
                errors["alerta"] = <div className="alert alert-danger">El campo Inicio Alerta es obligatorio</div>;
            } else {
                if (this.state.alerta < 0) {
                    this.state.formIsValid = false;
                    errors["alerta"] = <div className="alert alert-danger">El campo no puede ser negativo</div>;
                }
                if (this.state.alerta > 400) {
                    this.state.formIsValid = false;
                    errors["alerta"] = <div className="alert alert-danger">El campo no puede ser mayor a 400</div>;
                }
                if (!isFinite(this.state.alerta)) {
                    this.state.formIsValid = false;
                    errors["alerta"] = <div className="alert alert-danger">El campo permite solo ingreso numérico</div>;
                }
            }
        }

        this.setState({ errores: errors });
    }

    handleSubmit(event) {
        event.preventDefault();
        this.handleValidation();
        if (this.state.formIsValid) {
            axios.post("/RRHH/Requisitos/EditApiAsync/", {
                Id: sessionStorage.getItem('id_requisitos'),
                codigo: this.state.codigo,
                nombre: this.state.nombre,
                requisitoId: this.state.tipo_requisito,
                descripcion: this.state.descripcion,
                responsableId: this.state.responsable,
                caducidad: this.state.caducidad,
                tiempo_vigencia: this.state.tiempo_vigencia,
                dias_inicio_alerta: this.state.alerta,
                activo: this.state.activo
            })
                .then((response) => {
                    abp.notify.success("Requisito Guardado!", "Aviso");
                    this.Regresar()
                })
                .catch((error) => {
                    console.log(error);
                    abp.notify.error('Algo salió mal.', 'Error');
                });

        }else {
            abp.notify.error('Se ha encontrado errores, por favor revisar el formulario', 'Error');
        }
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['TIPOREQUISITO', 'RESPONSABLEREQUISITO'];

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
                case 'TIPOREQUISITO':
                    this.setState({ tiposRequisito: catalogo })
                    this.getFormSelectTipoRequisito()
                    return;
                case 'RESPONSABLEREQUISITO':
                    this.setState({ responsables: catalogo })
                    this.getFormSelectResponsableRequisito()
                    return;
            }
        });
    }

    getFormSelectTipoRequisito() {
        this.state.tiposRequisito.sort(function(a, b) {
            if(a.nombre.toLowerCase() < b.nombre.toLowerCase()) return -1;
            if(a.nombre.toLowerCase() > b.nombre.toLowerCase()) return 1;
            return 0;
           })
        return (
            this.state.tiposRequisito.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectResponsableRequisito() {
        this.state.responsables.sort(function(a, b) {
            if(a.nombre.toLowerCase() < b.nombre.toLowerCase()) return -1;
            if(a.nombre.toLowerCase() > b.nombre.toLowerCase()) return 1;
            return 0;
           })
        return (
            this.state.responsables.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    Regresar() {
        return (
            window.location.href = "/RRHH/Requisitos/Index/"
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

    handleChangeUpperCase(event) {
        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
    }

}


ReactDOM.render(
    <EditarRequisitos />,
    document.getElementById('content-editar-requisitos')
);