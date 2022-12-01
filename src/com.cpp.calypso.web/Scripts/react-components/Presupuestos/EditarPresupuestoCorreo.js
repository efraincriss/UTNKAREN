import React from 'react';
import moment from 'moment';
import axios from 'axios';

export default class EditarPresupuestoCorreo extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            Oferta: props.Oferta,
            fecha_registro: moment(props.Oferta.fecha_registro).format('YYYY-MM-DD'),
            RequerimientoId: props.Oferta.RequerimientoId,
            ProyectoId: props.Oferta.ProyectoId,
            Clase: props.Oferta.Clase,
            descripcion: props.Oferta.descripcion,
            version: props.Oferta.version,
            codigo: props.Oferta.codigo,
            alcance: props.Oferta.alcance,
            descuento: props.Oferta.descuento,
            justificacion_descuento: props.Oferta.justificacion_descuento,
            asuntoCorreo: props.Oferta.asuntoCorreo,
            descripcionCorreo: props.Oferta.descripcionCorreo
        }

        this.handleChange = this.handleChange.bind(this);
        this.EnviarFormulario = this.EnviarFormulario.bind(this);
    }


    EnviarFormulario(event) {
        event.preventDefault();
        this.props.Loading();
        axios.post("/proyecto/OfertaPresupuesto/EditPresupuestoEmail", {
            Id: this.props.Oferta.Id,
            ProyectoId: this.state.ProyectoId,
            fecha_registro: this.state.fecha_registro,
            RequerimientoId: this.state.RequerimientoId,
            Clase: this.state.Clase,
            descripcion: this.state.descripcion,
            version: this.state.version,
            codigo: this.state.codigo,
            alcance: this.state.alcance,
            descuento: this.state.descuento,
            justificacion_descuento: this.state.justificacion_descuento,
            asuntoCorreo: this.state.asuntoCorreo,
            descripcionCorreo: this.state.descripcionCorreo

        })
            .then((response) => {
                this.props.successMessage("Informacion Actualizada")
                this.props.ConsultarOferta();
                this.props.OcultarFormulario();
            })
            .catch((error) => {
                console.log(error);
                this.props.warnMessage("Ocurrió un Error")
                this.props.CancelarLoading();
            });
    }

    EnvioManual = () => {
        axios
            .post(
                "/Proyecto/OfertaPresupuesto/GetMailto/" +
                this.props.Oferta.Id,
                {}
            )
            .then((response) => {
                //this.setState({ mailto: response.data });
                window.location.href = response.data;
            })
            .catch((error) => {
                console.log(error);
            });
    };
    render() {
        return (
            <form onSubmit={this.EnviarFormulario}>

                <div className="row">
                    <div className="col">
                        <div className="form-group">
                            <label>Asunto</label>
                            <input
                                type="text"
                                name="asuntoCorreo"
                                className="form-control"
                                onChange={this.handleChange}
                                value={this.state.asuntoCorreo}
                            />
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <div className="form-group">
                            <label>Descripción</label>
                            <textarea 
                                type="text"
                                name="descripcionCorreo"
                                className="form-control"
                                onChange={this.handleChange}
                                value={this.state.descripcionCorreo}
                            />
                        </div>
                    </div>
                </div>
                <button type="submit" className="btn btn-outline-primary" style={{ marginRight: '0.3em' }}>Guardar</button>
                <button type="button" className="btn btn-outline-primary" style={{ marginRight: '0.3em' }} onClick={() => this.props.OcultarFormulario()}>Cancelar</button>
                <button

                    type="button"
                    style={{ marginRight: '0.3em' }}
                    className="btn btn-outline-indigo"
                    onClick={() => {
                        if (
                            window.confirm(
                                `Esta acción procesará su servidor de correos y mostrará la información guardada, ¿Desea continuar?`
                            )
                        )
                            this.EnvioManual();
                    }}
                >
                    Procesar Envío
                </button>

            </form>
        )
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }
}