import React from 'react';
import moment from 'moment';
import axios from 'axios';

export default class EditarPresupuesto extends React.Component {
    constructor(props){
        super(props)
        this.state ={
            Oferta: props.Oferta,
            fecha_registro: moment(props.Oferta.fecha_registro).format('YYYY-MM-DD'),
            RequerimientoId: props.Oferta.RequerimientoId,
            ProyectoId: props.Oferta.ProyectoId,
            Clase: props.Oferta.Clase,
            descripcion: props.Oferta.descripcion,
            version: props.Oferta.version,
            codigo: props.Oferta.codigo,
            alcance: props.Oferta.alcance,
            descuento:props.Oferta.descuento,
            justificacion_descuento:props.Oferta.justificacion_descuento,
        }

        this.handleChange = this.handleChange.bind(this);
        this.EnviarFormulario = this.EnviarFormulario.bind(this);
    }


    EnviarFormulario(event){
        event.preventDefault();
        this.props.Loading();
        axios.post("/proyecto/OfertaPresupuesto/EditPresupuesto",{
            Id: this.props.Oferta.Id,
            ProyectoId: this.state.ProyectoId,
            fecha_registro: this.state.fecha_registro,
            RequerimientoId: this.state.RequerimientoId,
            Clase: this.state.Clase,
            descripcion: this.state.descripcion,
            version: this.state.version,
            codigo: this.state.codigo,
            alcance: this.state.alcance,
            descuento:this.state.descuento,
            justificacion_descuento:this.state.justificacion_descuento
        })
        .then((response) => {
            this.props.successMessage("Presupuesto Actualizado")
            this.props.ConsultarOferta();
            this.props.OcultarFormulario();
        })
        .catch((error) => {
            console.log(error);
            this.props.warnMessage("Ocurrió un Error")
            this.props.CancelarLoading();
        });
    }


    render() {
        return(
            <form onSubmit={this.EnviarFormulario}>
                <div className="row">
                    <div className="col">
                        <div className="form-group">
                            <label>Fecha Registro</label>
                            <input
                                type="date"
                                id="no-filter"
                                name="fecha_registro"
                                className="form-control"
                                onChange={this.handleChange}
                                value={this.state.fecha_registro}
                            />

                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="label">Clase</label>
                            <select onChange={this.handleChange} value={this.state.Clase} className="form-control" name="Clase">
                                <option value="0">-- Selecciona una Clase --</option>
                                <option value="1">Budgetario</option>
                                <option value="2">Clase 1</option>
                                <option value="3">Clase 2</option>
                                <option value="4">Clase 3</option>
                                <option value="5">Clase 4</option>
                                <option value="6">Clase 5</option>
                            </select>
                            
                        </div>
                    </div>
                </div>
                        

                <div className="row">
                    <div className="col">
                        <div className="form-group">
                            <label>Descripción</label>
                            <input
                                type="text"
                                name="descripcion"
                                className="form-control"
                                onChange={this.handleChange}
                                value={this.state.descripcion}
                            />
                        </div>
                    </div>
                </div>
                <div className="row">
                             <div className="col">
                                      <div className="form-group">
                                            <label htmlFor="label">Valor Descuento</label>
                                            <input
                                            type="number"
                                            name="descuento"
                                            className="form-control"
                                            onChange={this.handleChange}
                                            min="0" value="0" step="any"
                                            value={this.state.descuento}
                                    />
                                         </div>
                                </div>
                            <div className="col">
                                <div className="form-group">
                                    <label>Justificacion Descuento</label>
                                    <input
                                        type="text"
                                        name="justificacion_descuento"
                                        className="form-control"
                                        onChange={this.handleChange}
                                        value={this.state.justificacion_descuento}
                                    />
                                </div>
                            </div>

                        </div>
                <div className="row">
                    <div className="col">
                        <div className="form-group">
                            <label>Alcance</label>
                            <input
                                type="text"
                                name="alcance"
                                className="form-control"
                                onChange={this.handleChange}
                                value={this.state.alcance}
                            />
                        </div>
                    </div>
                </div>

                <button type="submit"  className="btn btn-outline-primary" style={{marginRight: '0.3em'}}>Guardar</button>
                <button type="button" className="btn btn-outline-primary" style={{marginRight: '0.3em'}} onClick={() => this.props.OcultarFormulario()}>Cancelar</button>
            </form>
        )
    }

    handleChange(event){
        this.setState({[event.target.name]: event.target.value});
    }
}