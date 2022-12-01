import React from 'react';
import axios from 'axios';


import {Button} from 'primereact/components/button/Button';
import {Dropdown} from 'primereact/components/dropdown/Dropdown';


export default class ItemIngenieriaForm extends React.Component{
    constructor(props){
        super(props);
        console.log(props)
        this.state = {      
            tipo_registros_catalogo: props.tipo_registros_catalogo, 
            ColaboradorId: props.ColaboradorId,
            cantidad_horas: props.cantidad_horas_item,
            etapa: props.etapa,
            fecha_registro: props.fecha_registro,
            especialidad: props.especialidad,
            ItemId: props.itemId,
            registro: props.registros,
        }

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);

    }



    render(){
        return(
            <div>
            
                <form onSubmit={this.handleSubmit}>

                        <div className="form-group">
                        <label htmlFor="label">Tipo de Registros</label>

                        <select onChange={this.handleChange} name="registro" required className="form-control" value={this.state.registro}>
                            <option value="">-- Selecciona un tipo de registro --</option>
                            {this.getFormSelect(this.props.tipo_registros_catalogo)}
                        </select>
                        
                    </div>
                    <div className="form-group">
                        <label htmlFor="label">Especialidad</label>

                        <select onChange={this.handleChange} name="especialidad" required className="form-control" value={this.state.especialidad}>
                            <option value="">-- Selecciona una especialidad --</option>
                            {this.getFormSelect(this.props.especialidad_catalogo)}
                        </select>
                        
                    </div>

                    <div className="form-group">
                        <label htmlFor="label">Etapa</label>

                        <select onChange={this.handleChange} name="etapa" required className="form-control" value={this.state.etapa}>
                            <option value="">-- Selecciona una etapa --</option>
                            <option value="1">Ingeniería Básica</option>
                            <option value="2">Ingeniería Detalle</option>
                            <option value="3">Ingeniería Conceptual</option>
                            <option value="4">As Built</option>
                        </select>
                        
                    </div>

                    <div className="form-group">
                        <label htmlFor="label">Colaborador</label>

                        <div>
                        <Dropdown 
                            value={this.state.ColaboradorId} 
                            options={this.props.colaboradores_catalogo} 
                            onChange={(e) => {this.setState({ColaboradorId: e.value})}} 
                            filter={true} filterPlaceholder="Selecciona un colaborador" 
                            filterBy="label,value" placeholder="Selecciona un colaborador"
                            style={{width: '100%', heigh: '18px'}}
                            required
                        />
                        </div>
                        
                    </div>
                    
                    <div className="form-group">
                        <label htmlFor="label">Cantidad de Horas</label>
                        <input 
                        type="number"
                        min="0" 
                        value={this.state.cantidad_horas}
                        step="any"
                        className="form-control"
                        onChange={this.handleChange}
                        name="cantidad_horas"
                        />
                        
                    </div>

                    <div className="form-group">
                        <label htmlFor="label">Fecha Registro</label>

                        <input 
                        type="date"
                        min="0" 
                        value={this.state.fecha_registro}
                        step="any"
                        className="form-control"
                        onChange={this.handleChange}
                        name="fecha_registro"
                        required
                        />
                        
                    </div>


                    <Button type="submit"  label="Guardar" icon="fa fa-fw fa-folder-open"/>
                    <Button type="button"  label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.props.onHide}/>
                </form>
            </div>
        );
    }

    handleSubmit(event){
        event.preventDefault();
        axios.post("/Proyecto/DetalleItemIngenieria/Create",{
            Id: this.props.ItemId,
            DetalleAvanceIngenieriaId: this.props.DetalleAvanceIngenieriaId,
            ColaboradorId: this.state.ColaboradorId,
            cantidad_horas: this.state.cantidad_horas,
            fecha_registro: this.state.fecha_registro,
            etapa: this.state.etapa,
            especialidad: this.state.especialidad,
            vigente: true,
            tipo_registro: this.state.registro,
        })
        .then((response) => {
            this.props.updateData(this.props.DetalleAvanceIngenieriaId);
            this.props.showSuccess("Ingresado correctamente")
            this.props.onHide();
        })
        .catch((error) => {
            console.log(error);
            this.props.showWarn("Intentalo mas tarde")
        });
        
    }

    handleChange(event) {
        this.setState({[event.target.name]: event.target.value});
    }

    getFormSelect(list){      
        return(
            
            list.map((item) => {
                return (
                    <option key={item.Id} value={item.Id}>{item.nombre}</option>
                )         
            })
            
        );
    }

}