import React from 'react';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import moment from 'moment';
export default class Alta extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            errores: [],
            id_empleado: '',
            meta4: '',
        }

        this.handleChange = this.handleChange.bind(this);
        this.Guardar = this.Guardar.bind(this);
    }

    componentDidMount() {
    }


    render() {
        return (
            <div>
                <BlockUi tag="div" blocking={this.state.loading}>
                    <div className="row">
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="text"><b>Tipo de Identificación:</b> {this.props.tipo_identificacion} </label>

                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="text"><b>No. de Identificación:</b> {this.props.nro_identificacion} </label>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="text"><b>Apellidos Nombres:</b> {this.props.nombres_apellidos} </label>
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="text"><b>Motivo de Medida:</b> {this.props.empleado_id_sap == null ? "Alta" : "Reingreso"} </label>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="id_empleado">* ID Empleado: </label>
                                <input type="text" id="id_empleado" className="form-control" value={this.state.id_empleado} onChange={this.handleChange} name="id_empleado" />
                                {this.state.errores["id_empleado"]}
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="meta4">Meta 4: </label>
                                <input type="text" id="meta4" className="form-control" value={this.state.meta4} onChange={this.handleChange} name="meta4" />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="fecha_ingreso"><b>Fecha de Ingreso:</b></label>
                                <input type="date" id="fecha_ingreso" className="form-control" value={this.props.fecha_ingreso} onChange={this.handleChange} name="fecha_ingreso" disabled/>
                            </div>
                        </div>
                        <div className="col">
                        </div>
                    </div>


                    <div className="row">
                        <div className="form-group col">
                            <button type="button" onClick={() => this.Guardar()} className="btn btn-outline-primary"> Guardar</button>
                            <button onClick={() => this.props.onHide()} type="button" className="btn btn-outline-primary" style={{ marginLeft: '3px' }}> Cancelar</button>

                        </div>
                    </div>
                </BlockUi>
            </div>
        )
    }

    Guardar() {
        if (!this.state.id_empleado) {
            var errors = [];
            errors["id_empleado"] = <div className="alert alert-danger">El campo ID Empleado es obligatorio</div>;
            abp.notify.error('Se ha encontrado errores, por favor revisar el formulario', 'Error');
            this.setState({ errores: errors });
        } else {
            this.setState({ loading: true });

            axios.post("/RRHH/Colaboradores/CreateAltaColaboradorador/", {
                id: this.props.Id,
                id_empleado: this.state.id_empleado,
                meta4: this.state.meta4
            })
                .then((response) => {
                    console.log("DATA ", response.data);
                    this.setState({ loading: false });
                    abp.notify.success("Alta Guardada!", "Aviso");
                    this.props.GetColaboradores();
                    this.props.onHide();
                })
                .catch((error) => {
                    this.setState({ loading: false });
                    abp.notify.error('Algo salió mal', 'Error');
                    console.log("ERROR", error);
                });
        }


    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }
}