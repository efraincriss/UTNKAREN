import React from 'react';
import axios from 'axios';


import {Button} from 'primereact/components/button/Button';
import {Dropdown} from 'primereact/components/dropdown/Dropdown';


export default class DetalleIngenieriaForm extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            tipo_registros_catalogo: props.tipo_registros_catalogo, 
            cantidad_horas: props.cantidad_horas,
            computo: props.computo,
            Id: props.Id,
            registro: props.registro,
            valor_real: props.valor_real,
        }

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);

    }



    render(){
        return(
            <div>
            
                <form onSubmit={this.handleSubmit}>
                    

                    <div className="form-group">
                        <label htmlFor="label">Computo</label>

                        <div>
                        <Dropdown 
                            value={this.state.computo} 
                            options={this.props.computos_list} 
                            onChange={(e) => {this.setState({computo: e.value})}} 
                            filter={true} filterPlaceholder="Selecciona un computo" 
                            filterBy="label,value" placeholder="Selecciona un computo"
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
                        <label htmlFor="label">Valor Real</label>
                        <input 
                            type="number"
                            min="0" 
                            value={this.state.valor_real}
                            step="any"
                            className="form-control"
                            name="valor_real"
                            disabled
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
        if(this.state.computo == 0){
            this.props.showWarn("Selecciona un computo");
        } else {
            axios.post("/Proyecto/DetalleAvanceIngenieria/Create",{
                Id: this.state.Id,
                AvanceIngenieriaId: document.getElementById('AvanceIngenieriaId').className,
                tipo_registro: this.state.registro,
                ComputoId: this.state.computo,
                cantidad_horas: this.state.cantidad_horas,
                vigente: true,
                valor_real: 0,
                fecha_real: new Date()
            })
            .then((response) => {
                this.props.updateData();
                this.props.showSuccess("Ingresado correctamente")
                this.props.onHide();
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn("Intentalo mas tarde")
            });
        }
        
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