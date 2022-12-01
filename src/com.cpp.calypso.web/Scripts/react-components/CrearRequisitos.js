import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import {
    FRECUENCIA_DIARIA, FRECUENCIA_SEMANAL, FRECUENCIA_MENSUAL
} from './Colaboradores/Codigos';

export default class CrearRequisitos extends React.Component {

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
            genera_notificacion: '',
            frecuencia_notificacion: '',
            tiempo_inicio_notificacion: '',
            dia_notificacion: '',
            tiposFrecuenciaNotificacion: [],
            diasNotificacion: [],
        }

        this.Regresar = this.Regresar.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);

        this.getFormSelectTipoRequisito = this.getFormSelectTipoRequisito.bind(this);
        this.getFormSelectResponsableRequisito = this.getFormSelectResponsableRequisito.bind(this);
        this.getFormSelectFrecuencia = this.getFormSelectFrecuencia.bind(this);
        this.getFormSelectDias = this.getFormSelectDias.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);

    }

    componentDidMount() {
        this.GetCatalogos();
    }

    render() {
        return (
            <div>
                <form onSubmit={this.handleSubmit}>

                    <div className="row">
                        <div className="col">
                            <div className="content-section implementation">
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
                                <label htmlFor="nombre">* Nombre: </label>
                                <input type="text" id="nombre" className="form-control" value={this.state.nombre} onChange={this.handleChangeUpperCase} name="nombre" />
                                {this.state.errores["nombre"]}
                            </div>
                        </div>
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
                            <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '15px' }} >
                                <label htmlFor="genera_notificacion" style={{ width: '240px' }} >Genera Notificación: </label>
                                <input type="checkbox" id="genera_notificacion" className="form-control" checked={this.state.genera_notificacion} onChange={this.handleInputChange} name="genera_notificacion" style={{ marginTop: '5px'/*, marginLeft: '-100%'*/ }} />
                            </div>
                        </div>
                    </div>
                    <div className="row" hidden={!this.state.caducidad}>
                        <div className="col">
                            <div className="form-group" >
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
                    <div className="row" hidden={!this.state.genera_notificacion}>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="frecuencia_notificacion">* Frecuencia Notificación: </label>
                                <select value={this.state.frecuencia_notificacion} onChange={this.handleChange} className="form-control" name="frecuencia_notificacion">
                                    <option value="">Seleccione...</option>
                                    {this.getFormSelectFrecuencia()}
                                </select>
                                {this.state.errores["frecuencia_notificacion"]}
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group"  >
                                <label htmlFor="tiempo_inicio_notificacion">* Tiempo Inicio Notificación: </label>
                                <input type="number" id="tiempo_inicio_notificacion" className="form-control" value={this.state.tiempo_inicio_notificacion} onChange={this.handleChange} name="tiempo_inicio_notificacion" />
                                {this.state.errores["tiempo_inicio_notificacion"]}
                            </div>
                        </div>
                    </div>
                    <div className="row" hidden={!this.state.genera_notificacion}>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="dia_notificacion">* Día Envío Notificación: </label>
                                <select value={this.state.dia_notificacion} onChange={this.handleChange} className="form-control" name="dia_notificacion">
                                    <option value="">Seleccione...</option>
                                    {this.getFormSelectDias()}
                                </select>
                                {this.state.errores["dia_notificacion"]}
                            </div>
                        </div>
                        <div className="col">
                        </div>
                    </div>

                    <div className="form-group" >
                        <div className="col">
                            <button type="submit" className="btn btn-outline-primary fa fa-save"> Guardar</button>
                            <button onClick={() => this.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                        </div>
                    </div>
                </form>
            </div >
        )
    }

    handleValidation() {
        console.log('entro a validacion');
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.tipo_requisito) {
            this.state.formIsValid = false;
            errors["tipo_requisito"] = <div className="alert alert-danger">El campo Tipo Requisito es obligatorio</div>;
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

        if (this.state.genera_notificacion == true) {
            if (!this.state.frecuencia_notificacion) {
                this.state.formIsValid = false;
                errors["frecuencia_notificacion"] = <div className="alert alert-danger">El campo Frecuencia Notificación es obligatorio</div>;
            }else{
                var frecuencia = this.state.tiposFrecuenciaNotificacion.filter(c => c.Id == Number.parseInt(this.state.frecuencia_notificacion));
                console.log('frecuencia', frecuencia)
                if(frecuencia[0].codigo == FRECUENCIA_SEMANAL || frecuencia[0].codigo == FRECUENCIA_MENSUAL){
                    if (!this.state.dia_notificacion) {
                        this.state.formIsValid = false;
                        errors["dia_notificacion"] = <div className="alert alert-danger">El campo Día Envío Notificación es obligatorio</div>;
                    }
                }
            }
            if (!this.state.tiempo_inicio_notificacion) {
                this.state.formIsValid = false;
                errors["tiempo_inicio_notificacion"] = <div className="alert alert-danger">El campo Tiempo Inicio Notificación es obligatorio</div>;
            } else {
                if (this.state.tiempo_inicio_notificacion < 0) {
                    this.state.formIsValid = false;
                    errors["tiempo_inicio_notificacion"] = <div className="alert alert-danger">El campo no puede ser negativo</div>;
                }
                if (this.state.tiempo_inicio_notificacion.length > 3) {
                    this.state.formIsValid = false;
                    errors["tiempo_inicio_notificacion"] = <div className="alert alert-danger">El campo no puede tener más de 3 dígitos</div>;
                }
                if (!isFinite(this.state.tiempo_inicio_notificacion)) {
                    this.state.formIsValid = false;
                    errors["tiempo_inicio_notificacion"] = <div className="alert alert-danger">El campo permite solo ingreso numérico</div>;
                }
            }
        }

        this.setState({ errores: errors });
    }

    handleSubmit(event) {
        event.preventDefault();
        this.handleValidation();
        // var tempDate = new Date();
        // var date = tempDate.getFullYear() + '-' + (tempDate.getMonth() + 1) + '-' + tempDate.getDate() + ' ' + tempDate.getHours() + ':' + tempDate.getMinutes() + ':' + tempDate.getSeconds();

        if (this.state.formIsValid) {
            axios.post("/RRHH/Requisitos/CreateApiAsync/", {
                nombre: this.state.nombre,
                codigo:".",
                requisitoId: this.state.tipo_requisito,
                descripcion: this.state.descripcion,
                responsableId: this.state.responsable,
                caducidad: this.state.caducidad,
                tiempo_vigencia: this.state.tiempo_vigencia,
                dias_inicio_alerta: this.state.alerta,
                genera_notificacion: this.state.genera_notificacion,
                CatalogoFrecuenciaNotificacionId: this.state.frecuencia_notificacion,
                tiempo_inicio_notificacion: this.state.tiempo_inicio_notificacion,
                dia_envio_notificacion: this.state.dia_notificacion,
            })
                .then((response) => {
                    console.log(response)
                    if (response.data == "SI") {
                        abp.notify.error('Código ya existe', 'Error');
                    } else if (response.data == "NO") {
                        abp.notify.success("Requisito Guardado!", "Aviso");
                        this.Regresar()
                    }
                })
                .catch((error) => {
                    console.log(error);
                    abp.notify.error('Algo salió mal.', 'Error');
                });

        } else {
            abp.notify.error('Se ha encontrado errores, por favor revisar el formulario', 'Error');
        }
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['TIPOREQUISITO', 'RESPONSABLEREQUISITO', 'FRECUENCIANOTIFICACION', 'DIAS'];
        // console.log('codigos', codigos)
        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                // console.log(response.data)
                this.cargarCatalogos(response.data);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    cargarCatalogos(data) {
        data.forEach(e => {
            var catalogo = JSON.parse(e);
            // console.log(catalogo)
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
                case 'FRECUENCIANOTIFICACION':
                    this.setState({ tiposFrecuenciaNotificacion: catalogo })
                    this.getFormSelectFrecuencia()
                    return;
                case 'DIAS':
                    this.setState({ diasNotificacion: catalogo })
                    this.getFormSelectDias()
                    return;
            }
        });
    }

    getFormSelectFrecuencia() {
        return (
            this.state.tiposFrecuenciaNotificacion.map((item) => {
                return (
                    <option key={item.Id} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectDias() {
        return (
            this.state.diasNotificacion.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectTipoRequisito() {
        this.state.tiposRequisito.sort(function (a, b) {
            if (a.nombre.toLowerCase() < b.nombre.toLowerCase()) return -1;
            if (a.nombre.toLowerCase() > b.nombre.toLowerCase()) return 1;
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
        this.state.responsables.sort(function (a, b) {
            if (a.nombre.toLowerCase() < b.nombre.toLowerCase()) return -1;
            if (a.nombre.toLowerCase() > b.nombre.toLowerCase()) return 1;
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
    <CrearRequisitos />,
    document.getElementById('content-crear-requisitos')
);