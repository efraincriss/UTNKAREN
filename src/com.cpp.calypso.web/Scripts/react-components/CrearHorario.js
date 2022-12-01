import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';

export default class CrearHorario extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            codigo: '',
            nombre: '',
            h_inicio: '',
            h_fin: '',
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
                                <span style={{color: "red"}}>{this.state.errores["codigo"]}</span>
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="nombre">* Nombre: </label>
                                <input type="text" id="nombre" className="form-control" value={this.state.nombre} onChange={this.handleChange} name="nombre" />
                                <span style={{color: "red"}}>{this.state.errores["nombre"]}</span>
                            </div>
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="h_inicio">* Hora Inicio: </label>
                                <input type="time" id="h_inicio" className="form-control" value={this.state.h_inicio} onChange={this.handleChange} name="h_inicio" />
                                <span style={{color: "red"}}>{this.state.errores["h_inicio"]}</span>
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="h_fin">* Hora Fin: </label>
                                <input type="time" id="h_fin" className="form-control" value={this.state.h_fin} onChange={this.handleChange} name="h_fin" />
                                <span style={{color: "red"}}>{this.state.errores["h_fin"]}</span>
                            </div>
                        </div>
                    </div>

                    <div className="form-group">
                        <div className="col">
                            <button type="submit" className="btn btn-outline-primary fa fa-save"> Guardar</button>
                            <button onClick={() =>this.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                        </div>
                    </div>
                </form>
            </div >
        )
    }

    handleValidation(){
        console.log('entro a validacion');
        let errors = {};
        this.state.formIsValid = true;

        if(!this.state.codigo){
            this.state.formIsValid = false;
           errors["codigo"] = "El campo Código es obligatorio.";
        }else{
            if (this.state.codigo.length > 5) {
                this.state.formIsValid = false;
                errors["codigo"] = "El campo no puede tener más de cinco dígitos.";
            }
        }
        if(!this.state.nombre){
            this.state.formIsValid = false;
            errors["nombre"] = "El campo Nombre es obligatorio.";
         }
        if(!this.state.h_inicio){
            this.state.formIsValid = false;
            errors["h_inicio"] = "El campo Hora Inicio es obligatorio.";
         }
         if(!this.state.h_fin){
            this.state.formIsValid = false;
            errors["h_fin"] = "El campo Hora Fin es obligatorio.";
         }

        // if(this.state.h_inicio > this.state.h_fin) {
        //     this.state.formIsValid = false;
        //     errors["h_inicio"] = "El campo debe ser menor a Hora Fin.";
        //     errors["h_fin"] = "El campo debe ser mayor a Hora Inicio.";
        //  }

       this.setState({errores: errors});
   }

    handleSubmit(event) {
        event.preventDefault();
        this.handleValidation();
        if(this.state.formIsValid){  
            axios.post("/RRHH/Horario/CreateApiAsync/", {
                codigo: this.state.codigo,
                nombre: this.state.nombre,
                h_inicio: this.state.h_inicio,
                h_fin: this.state.h_fin,
                
                
            })
                .then((response) => {
                    if(response.data=="SI"){
                        this.warnMessage("Código ya existe");
                    }else if(response.data=="NO"){
                        this.successMessage("Horario Guardado!")
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
           window.location.href = "/RRHH/Horario/Index/"
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
    <CrearHorario />,
    document.getElementById('content-crear-horario')
);