import React from 'react';
import axios from 'axios';
import {Button} from 'primereact/components/button/Button';
import {MultiSelect} from 'primereact/components/multiselect/MultiSelect';


export default class ActividadFormPresupuesto extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            ActividadIds: [],          
        }
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.showRegistroActividadButton=this.showRegistroActividadButton.bind(this);
       
    }


    
    showRegistroActividadButton(){
       // if(this.props.tiporequerimiento==0){
          
            return(
              
               
                   <Button type="button"  label="Nueva Actividad" icon="fa fa-fw fa-folder-open" onClick={this.props.MostrarNuevoCatalogo}/>
              
                 
            )
       // }
    }
    render(){
        return(
            <div>
                <form onSubmit={this.handleSubmit} style={{height: '400px'}}>
               
                    <div className="form-group">
                        <label htmlFor="label">Actividades</label>
                        <br/>
                   
                 
                    <div className="row">
                       
                         
                            <Button type="submit"  label="Guardar" icon="fa fa-fw fa-folder-open"/>
                            <Button type="button"  label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.props.onHide}/>
                            {this.showRegistroActividadButton()}
                     
                       
                        
                    </div>
                    <br/>
                    <br/>
                        <MultiSelect 
                            value={this.state.ActividadIds} 
                            options={this.props.ListActividades} 
                            onChange={(e) => this.setState({ActividadIds: e.value})}
                            style={{width:'42em'}} filter={true}
                            defaultLabel="Selecciona las actividades"
                            className="form-control"
                         />
                        
                    </div>
                  
                </form>
            </div>
        );
    }

    handleSubmit(event){
        event.preventDefault();
        axios.post("/proyecto/WbsPresupuesto/CreateActividades",{
            ActividadesIds: this.state.ActividadIds,
            PresupuestoId: this.props.presupuestoId,
            id_nivel_padre_codigo: this.props.codigo_padre,
            estado: true,
            observaciones: 'pendiente',
            nivel_nombre: "pendiente",
            es_actividad: true,
            vigente: true
        })
        .then((response) => {
            this.props.updateData();
            this.setState({ActividadIds: []})
            this.props.showSuccess("Wbs creado");
            this.props.onHide();
        })
        .catch((error) => {
            console.log(error);
            this.props.showSuccess("Ocurrio un error")
        });
    }

    
    
    handleChange(event) {
        this.setState({[event.target.name]: event.target.value});
    }
}