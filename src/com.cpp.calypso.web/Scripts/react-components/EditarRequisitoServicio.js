import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';

export default class EditarRequisitoServicio extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            tipo_usuario: '',
            tipo_estancia: '',
            horario: '',
            tipo_servicio: '',
            requisito: '',
            descripcion: '',
            obligatorio: '',
            tiposUsuario: [],
            tiposEstancia: [],
            horarios: [],
            tiposServicio: [],
            tiposRequisito: [],
            errores: [],
            formIsValid: '',
            visible: false,
            loading: true,
        }

        this.Regresar = this.Regresar.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleValidation = this.handleValidation.bind(this);

        this.GetRequisitos = this.GetRequisitos.bind(this);
        this.getFormSelectRequisito = this.getFormSelectRequisito.bind(this);
        this.GetTiposServicio = this.GetTiposServicio.bind(this);
        this.getFormSelectTipoServicio = this.getFormSelectTipoServicio.bind(this);

        this.ConsultaRequisito = this.ConsultaRequisito.bind(this);

    }

    componentDidMount() {
        this.GetTiposServicio();
        this.GetRequisitos();
        this.ConsultaRequisito(sessionStorage.getItem('id_requisito'));
    }

    render() {

        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <form onSubmit={this.handleSubmit}>

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label">* Servicio: </label>
                                    <select value={this.state.tipo_servicio} onChange={this.handleChange} className="form-control" name="tipo_servicio">
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectTipoServicio()}
                                    </select>
                                    {this.state.errores["tipo_servicio"]}
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label">* Requisito: </label>
                                    <select value={this.state.requisito} onChange={this.handleChange} className="form-control" name="requisito">
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectRequisito()}
                                    </select>
                                    {this.state.errores["requisito"]}
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col">
                                <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '15px' }}>
                                    <label htmlFor="obligatorio" style={{ width: '294px' }}>Obligatorio: </label>
                                    <input type="checkbox" id="obligatorio" className="form-control" checked={this.state.obligatorio} onChange={this.handleInputChange} name="obligatorio" style={{ marginTop: '5px', marginLeft: '-160px' }} />
                                </div>
                            </div>
                            <div className="col">
                            </div>
                        </div>



                        <div className="form-group">
                            <div className="col">
                                <button onClick={this.handleSubmit} type="button" className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                <button onClick={() => this.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                            </div>
                        </div>

                    </form>
                </div >
            </BlockUi>
        )
    }

    handleValidation() {
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.tipo_servicio) {
            this.state.formIsValid = false;
            errors["tipo_servicio"] = <div className="alert alert-danger">El campo Tipo Servicio es obligatorio</div>;
        }
        if (!this.state.requisito) {
            this.state.formIsValid = false;
            errors["requisito"] = <div className="alert alert-danger">El campo Requisito es obligatorio</div>;
        }

        this.setState({ errores: errors });
    }

    handleSubmit(event) {
        event.preventDefault();
        this.handleValidation();
        if (this.state.formIsValid) {
            axios.post("/RRHH/RequisitoServicio/CreateApiAsync/", {
                Id: sessionStorage.getItem('id_requisito'),
                RequisitosId: this.state.requisito,
                catalogo_servicio_id: this.state.tipo_servicio,
                obligatorio: this.state.obligatorio,

            })
                .then((response) => {
                    console.log(response);
                    if (response.data == "OK") {
                        this.setState({ loading: true })
                        abp.notify.success("Requisito actualizado!", "Aviso");
                        setTimeout(
                            function () {
                                this.Regresar()
                            }.bind(this), 2000
                        );
                    } else if (response.data == "SI") {
                        abp.notify.error('Requisito ya existe!', 'Error');
                    }
                })
                .catch((error) => {
                    console.log(error);
                    abp.notify.error('Algo salió mal.', 'Error');
                });

        }else {
            abp.notify.error('Se ha encontrado errores, por favor revisar el formulario', 'Error');
        }
    }

    ConsultaRequisito(id) {
        axios.post("/RRHH/RequisitoServicio/GetRequisitoApi/" + id, {})
            .then((response) => {
                console.log(response)
                this.setState({
                    requisito: response.data.RequisitosId,
                    tipo_servicio: response.data.catalogo_servicio_id,
                    obligatorio: response.data.obligatorio,
                    key_form: Math.random()
                })
            })
            .catch((error) => {
                console.log(error);
            });

    }

    GetRequisitos() {
        axios.post("/RRHH/Requisitos/GetRequisitosApi", {})
            .then((response) => {
                this.setState({ tiposRequisito: response.data })
                this.getFormSelectRequisito()
            })
            .catch((error) => {
                console.log(error);
            });
    }

    getFormSelectRequisito() {
        return (
            this.state.tiposRequisito.map((item) => {
                if (item.nombre_requisito == "PROVEEDORESREQ") {
                    return (
                        <option key={item.Id} value={item.Id}>{item.nombre}</option>
                    )
                }
            })
        );
    }

    GetTiposServicio() {
        axios.post("/RRHH/Colaboradores/GetCatalogosPorCodigoApi", { codigo: 'SERVICIO' })
            .then((response) => {
                console.log(response.data);
                this.setState({ tiposServicio: response.data })
                this.getFormSelectTipoServicio()
                this.setState({ loading: false })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    getFormSelectTipoServicio() {
        return (
            this.state.tiposServicio.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    Regresar() {
        return (
            window.location.href = "/RRHH/RequisitoServicio/Index/"
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

}


ReactDOM.render(
    <EditarRequisitoServicio />,
    document.getElementById('content-editar-requisito')
);