import React from 'react';
import axios from "axios/index";


export default class RdoForm extends React.Component{

    constructor(props){
        super(props);
        this.state = {
            fecha_rdo: '',
            block: false
        };
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }

    render(){
        return(
            <div>
                <form onSubmit={this.handleSubmit}>
                    <div className="form-group">
                        <label>Fecha Rdo</label>
                        <input
                            type="date"
                            id="no-filter"
                            name="fecha_rdo"
                            className="form-control"
                            onChange={this.handleChange}
                            value={this.state.fecha_rdo}
                            required
                        />

                    </div>
                    <button className="btn btn-outline-primary" type="submit" disabled={this.state.block}>Guardar</button>
                </form>
            </div>
        )
    }

    handleSubmit(event){
        event.preventDefault();
        this.props.handleBlocking();
        axios.post("/Proyecto/RdoCabecera/Create",{
            fecha_registro: this.state.fecha_rdo,
            ProyectoId: document.getElementById('content').className
        })
        .then((response) => {
            if(response.data == "Ok"){
                this.props.showSuccess();
                this.props.updateData();
                this.setState({block: true})
            } else {
                this.props.showWarn();
            }
            this.props.handleBlocking();
        })
        .catch((error) => {
            console.log(error)
            this.props.showWarn();
            this.props.handleBlocking();
        });
    }

    handleChange(event) {
        event.stopPropagation();
        this.setState({[event.target.name]: event.target.value});
    }
}