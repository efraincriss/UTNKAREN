import React from 'react';
import axios from 'axios';

export default class EditarServicios extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            codigo: '',
            nombre: '',
            errores: [],
            formIsValid: '',
        }

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleValidation = this.handleValidation.bind(this);

        this.ConsultaServicio = this.ConsultaServicio.bind(this);
    }

    componentDidMount() {
        //this.ConsultaServicio();
    }

    render() {
        return (
            <div>
                <form onSubmit={this.handleSubmit}>
                    <div className="row">
                        <div className="col-md-2"></div>
                        <div className="col-xs-12 col-md-8">
                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="codigo">* Código: </label>
                                        <input type="text" id="codigo" className="form-control" value={this.state.codigo} onChange={this.handleChange} name="codigo" disabled/>
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

                            <div className="form-group">
                                <div className="col">
                                    <button type="submit" className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                    <button onClick={() => this.props.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
            </div >
        )
    }

    ConsultaServicio() {
        axios.post("/RRHH/Servicio/GetServicioApi/", {id: sessionStorage.getItem('id_servicio')})
            .then((response) => {
                console.log(response)
                this.setState({
                    codigo: response.data.codigo,
                    nombre: response.data.nombre,
                    key_form: Math.random()
                })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    handleValidation() {
        console.log('entro a validacion');
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.codigo) {
            this.state.formIsValid = false;
            errors["codigo"] = "El campo Código es obligatorio.";
        }
        if (!this.state.nombre) {
            this.state.formIsValid = false;
            errors["nombre"] = "El campo Nombre es obligatorio.";
        }

        this.setState({ errores: errors });
    }

    handleSubmit(event) {
        event.preventDefault();
        this.handleValidation();
        if (this.state.formIsValid) {
            axios.post("/RRHH/Servicio/CreateApiAsync/", {
                Id: sessionStorage.getItem('id_servicio'),
                codigo: this.state.codigo,
                nombre: this.state.nombre,
            })
                .then((response) => {
                    if (response.data == "SI") {
                        this.props.warnMessage("Código ya existe");
                    } else if (response.data == "NO") {
                        this.props.successMessage("Servicio Actualizado!")
                        this.props.Regresar()
                    }
                })
                .catch((error) => {
                    console.log(error);
                    this.warnMessage("Algo salió mal.");
                });

        }
    }
    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

}