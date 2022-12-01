import React from 'react';
import {Button} from 'primereact/components/button/Button';
import axios from 'axios';

export default class NivelFormPresupuestos extends React.Component{
    constructor(props){
        super(props);

        this.state ={
            nombre: '',
           
            nombre_nuevo: this.props.NombreNivel
        }
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.DeleteNivel = this.DeleteNivel.bind(this);
        this.handleSubmitNivel = this.handleSubmitNivel.bind(this);
       
    }

    componentWillReceiveProps(prevProps){
        this.setState({nombre_nuevo: prevProps.NombreNivel})
    }

    render(){
        return(
            <div className="row">
                <div className="col">
                    <ul className="nav nav-tabs" id="ingreso_tabs" role="tablist">
                        <li className="nav-item">
                            <a className="nav-link active" id="ingreso-tab" data-toggle="tab" href="#ingreso" role="tab" aria-controls="profile">Ingresar</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" id="editar-tab" data-toggle="tab" href="#editar" role="tab" aria-controls="home" aria-expanded="true">Editar</a>
                        </li>
                    </ul>

                    <div className="tab-content" id="myTabContent">
                        <div className="tab-pane fade show active" id="ingreso" role="tabpanel" aria-labelledby="ingreso-tab">
                            <form onSubmit={this.handleSubmit}>
                                <div className="form-group">
                                    <label htmlFor="nombre">Nombre Nivel</label>
                                    <input type="text" name='nombre' value={this.state.nombre} id="nombre" onChange={this.handleChange} 
                                    ref={input => input && input.focus()}
                                    className="form-control"/>
                                </div>
                                <div className="row">
                                <div className="col">
                                <Button type="submit"  label="Guardar" icon="fa fa-fw fa-folder-open"/>
                                {this.showActividadButton()}
                                <Button style={{marginTop: '0.3em'}} type="button"  label="Eliminar" icon="fa fa-fw fa-ban" onClick={this.DeleteNivel}/>
                                </div>
                                <div className="col" align="right">
                          
                                </div>
                                </div>
                               
                              
                                
                            </form>
                        </div>
                        <div className="tab-pane fade" id="editar" role="tabpanel" aria-labelledby="editar-tab">
                            <form onSubmit={this.handleSubmitNivel}>
                                <div className="form-group">
                                    <label htmlFor="nombre_nuevo">Editar Nombre Nivel</label>
                                    <input type="text" name='nombre_nuevo' value={this.state.nombre_nuevo} id="nombre_nuevo" onChange={this.handleChange} className="form-control"/>
                                </div>

                                <Button type="submit"  label="Guardar" icon="fa fa-fw fa-folder-open"/>
                                {this.showActividadButton()}
                                <Button style={{marginTop: '0.3em'}} type="button"  label="Eliminar" icon="fa fa-fw fa-ban" onClick={() => { if (window.confirm('Estás segur@?')) this.DeleteNivel }}/>
                                
                                
                               
                            </form>
                        </div>
                    </div>
                </div>

            </div>
        );
    }

    showActividadButton(){
        if(this.props.codigo_padre != "."){
            return(
                <Button type="button"  label="Actividades" icon="fa fa-fw fa-calendar" onClick={this.props.showActividad}/>
            )
        }
    }


    handleSubmit(event){
        event.preventDefault();
        if(this.state.nombre === ""){
            this.props.showWarn("Escribe el nombre del nivel");
        } else {
            axios.post("/proyecto/WbsPresupuesto/Create",{
                PresupuestoId: this.props.presupuestoId,
                id_nivel_padre_codigo: this.props.codigo_padre,
                estado: true,
                observaciones: '',
                nivel_nombre: this.state.nombre,
                es_actividad: false,
                vigente: true
            })
            .then((response) => {
                this.props.updateData();
                this.setState({nombre: ''});
                this.props.onHide();
                this.props.showSuccess("Nivel creado");
                
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn("Ocurrio un error");
            });
        }
        
    }

    handleSubmitNivel(event){
        event.preventDefault();
        if(this.props.WbsIdSeleccionado == 0){
            this.props.showWarn("No hay nada que editar");
        } else if(this.state.nombre_nuevo === ""){
            this.props.showWarn("Ingresa el nombre del nivel");
        } else {
            axios.post("/proyecto/WbsPresupuesto/EditarNivel/"+ this.props.WbsIdSeleccionado,{
                nombre: this.state.nombre_nuevo
            })
            .then((response) => {
                if(response.data == "Ok"){
                    this.props.updateData();
                    this.setState({nombre: ''});
                    this.props.onHide();
                    this.props.showSuccess("Nivel actualizado");
                } else {
                    this.props.showWarn("No se pudo editar el nivel");
                } 
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn("Ocurrio un error");
            });
        }
        
    }



    DeleteNivel(event){
        event.preventDefault();
        axios.post("/proyecto/WbsPresupuesto/DeleteNivel/"+ this.props.WbsIdSeleccionado,{})
        .then((response) => {
            if(response.data == "Ok"){
                this.props.updateData();
                this.props.showSuccess("Nivel eliminado");
                this.setState({nombre: ''});
                this.props.onHide();
            } else {
                this.props.showWarn("Las actividades tienen items registrados");
            }
            
            
        })
        .catch((error) => {
            console.log(error);
            this.props.showWarn("Ocurrio un error");
        });
    }


    handleChange(event) {
        this.setState({[event.target.name]: event.target.value});
    }
}