import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';

export default class CrearRotacion extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            codigo: '',
            nombre: '',
            dias_campo: '',
            dias_oficina: '',
            dias_descanso: '',
            errores: [],
            formIsValid: '',
        }

        this.Regresar = this.Regresar.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.handleValidation = this.handleValidation.bind(this);


    }

    componentDidMount() {

    }

    render() {
        return (
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <form onSubmit={this.handleSubmit}>

                    <div className="row">
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="codigo">* Código: </label>
                                <input type="text" id="codigo" className="form-control" value={this.state.codigo} onChange={this.handleChange} name="codigo" />
                                <span style={{ color: "red" }}>{this.state.errores["codigo"]}</span>
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="nombre">* Nombre: </label>
                                <input type="text" id="nombre" className="form-control" value={this.state.nombre} onChange={this.handleChange} name="nombre" />
                                <span style={{ color: "red" }}>{this.state.errores["nombre"]}</span>
                            </div>
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="dias_campo">* Días laborables en campo: </label>
                                <input type="number" id="dias_campo" className="form-control" value={this.state.dias_campo} onChange={this.handleChange} name="dias_campo" />
                                <span style={{ color: "red" }}>{this.state.errores["dias_campo"]}</span>
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="dias_oficina">*  Días laborables en oficina: </label>
                                <input type="number" id="dias_oficina" className="form-control" value={this.state.dias_oficina} onChange={this.handleChange} name="dias_oficina" />
                                <span style={{ color: "red" }}>{this.state.errores["dias_oficina"]}</span>
                            </div>
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="dias_descanso">*  Días de descanso: </label>
                                <input type="number" id="dias_descanso" className="form-control" value={this.state.dias_descanso} onChange={this.handleChange} name="dias_descanso" />
                                <span style={{ color: "red" }}>{this.state.errores["dias_descanso"]}</span>
                            </div>
                        </div>
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
        )
    }

    handleValidation() {
        console.log('entro a validacion');
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.codigo) {
            this.state.formIsValid = false;
            errors["codigo"] = "El campo Código es obligatorio.";
        }else{
            if (this.state.codigo.length > 10) {
                this.state.formIsValid = false;
                errors["codigo"] = "El campo no puede tener más de 10 caracteres.";
            }
        }
        if (!this.state.nombre) {
            this.state.formIsValid = false;
            errors["nombre"] = "El campo Nombre es obligatorio.";
        }
        if (!this.state.dias_campo) {
            this.state.formIsValid = false;
            errors["dias_campo"] = "El campo Días laborables en campo es obligatorio.";
        }else{
            if (this.state.dias_campo.length > 2) {
                this.state.formIsValid = false;
                errors["dias_campo"] = "El campo no puede tener más de dos dígitos.";
            }
            if(!isFinite(this.state.dias_campo)){ 
                this.state.formIsValid = false; 
                errors["dias_campo"] = "El campo permite solo ingreso numérico"; 
               }  
        }
        if (!this.state.dias_oficina) {
            this.state.formIsValid = false;
            errors["dias_oficina"] = "El campo Días laborables en oficina es obligatorio.";
        }else{
            if (this.state.dias_oficina.length > 2) {
                this.state.formIsValid = false;
                errors["dias_oficina"] = "El campo no puede tener más de dos dígitos.";
            }
            if(!isFinite(this.state.dias_oficina)){ 
                this.state.formIsValid = false; 
                errors["dias_oficina"] = "El campo permite solo ingreso numérico"; 
               }  
        }
        if (!this.state.dias_descanso) {
            this.state.formIsValid = false;
            errors["dias_descanso"] = "El campo Días de descanso es obligatorio.";
        }else{
            if (this.state.dias_descanso.length > 2) {
                this.state.formIsValid = false;
                errors["dias_descanso"] = "El campo no puede tener más de dos dígitos.";
            }
            if(!isFinite(this.state.dias_descanso)){ 
                this.state.formIsValid = false; 
                errors["dias_descanso"] = "El campo permite solo ingreso numérico"; 
               }  
        }

        this.setState({ errores: errors });
    }

    handleSubmit(event) {
        event.preventDefault();
        this.handleValidation();
        if (this.state.formIsValid) {
            axios.post("/RRHH/AdminRotacion/CreateApiAsync/", {
                codigo: this.state.codigo,
                nombre: this.state.nombre,
                dias_laborablesC: this.state.dias_campo,
                dias_laborablesO: this.state.dias_oficina,
                dias_descanso: this.state.dias_descanso,
            })
                .then((response) => {
                    //console.log(response.data);
                    if (response.data == "SI") {
                        this.warnMessage("Código ya existe");
                    } else if (response.data == "NO") {
                        this.successMessage("Rotación Guardada!")
                        this.Regresar()
                    }
                })
                .catch((error) => {
                    console.log(error);
                    this.warnMessage("Algo salió mal.");
                });

        }
    }



    Regresar() {
        return (
            window.location.href = "/RRHH/AdminRotacion/Index/"
        );
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
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
    <CrearRotacion />,
    document.getElementById('content-crear-rotacion')
);