import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';

import { Button } from 'primereact/components/button/Button';
import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';

export default class CloneWbs extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            visible: false,
            proyectos: [],
            requerimientos: [],
            ProyectoId: 0,
            RequerimientoId: 0,
            message: '',
            blocking: false,
            blockSubmit: false,
        }
        this.showForm = this.showForm.bind(this);
        this.onHide = this.onHide.bind(this);
        this.getProyectos = this.getProyectos.bind(this);
        this.getRequerimientos = this.getRequerimientos.bind(this);
        this.getFormSelect = this.getFormSelect.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChangeProyecto = this.handleChangeProyecto.bind(this);
        this.handleChangeRequerimientos = this.handleChangeRequerimientos.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
    }

    componentDidMount(){
        this.getProyectos();
    }


    render(){
        return(
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <input type="button" className="btn btn-sm btn-outline-indigo" onClick={this.showForm} value="Clonar Oferta"/>

                
                <Dialog header="Clonar ofertas" visible={this.state.visible} width="450px" modal={true} onHide={this.onHide}>
                <BlockUi tag="div" blocking={this.state.blocking}>
                <form onSubmit={this.handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="label">Proyecto</label>

                        <select onChange={this.handleChangeProyecto} className="form-control">
                            <option value="0">-- Selecciona un proyecto --</option>
                            {this.getFormSelect(this.state.proyectos)}
                        </select>
                        
                    </div>

                    <div className="form-group">
                        <label htmlFor="label">Trabajos</label>

                        <select onChange={this.handleChangeRequerimientos} className="form-control">
                            <option value="0">-- Selecciona un requerimiento --</option>
                            {this.getFormSelect(this.state.requerimientos)}
                        </select>
                        
                    </div>
                    <Button type="submit"  label="Clonar" icon="fa fa-fw fa-folder-open" disabled={this.state.blockSubmit}/>
                    <Button type="button"  label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.onHide}/>
                </form>
                </BlockUi>
                </Dialog>
                
                
            </div>
        )
    }


    showForm(){
        this.setState({visible: true})
    }

    onHide(event){
        this.setState({visible: false, blockSubmit: false});
    }

    handleSubmit(event){
        event.preventDefault();
        if(this.state.ProyectoId == 0){
            this.setState({message: 'Selecciona un proyecto'},
            this.showWarn)
        } else if(this.state.RequerimientoId == 0){
            this.setState({message: 'Selecciona un requerimiento'},
            this.showWarn)
        } else {
            this.setState({blocking: true})
            window.location.href = "/Proyecto/Oferta/Create/" + this.state.RequerimientoId + "?clonar=Si&ofertaId="+document.getElementById('content-clone-wbs').className;
        }
    }

    handleChangeProyecto(event) {
        this.setState(
            {ProyectoId: event.target.value, RequerimientoId: 0, requerimientos: []},
            this.getRequerimientos
        );
    }

    handleChangeRequerimientos(event) {
        this.setState({RequerimientoId: event.target.value});
    }


    getProyectos(){
        axios.post("/Proyecto/Oferta/GetProyectosApi/"+document.getElementById('content-clone-wbs').className,{})
        .then((response) => {
            this.setState({proyectos: response.data})
            
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    getRequerimientos(){
        axios.post("/Proyecto/Oferta/GetRequerimientosProyectoApi/" + this.state.ProyectoId,{})
        .then((response) => {
            this.setState({requerimientos: response.data})          
        })
        .catch((error) => {
            console.log(error);           
        });
    }

    getFormSelect(list){      
        return(
            list.map((item) => {
                return (
                    <option value={item.Id}>{item.codigo}</option>
                )         
            })
            
        );
    }

    showSuccess() {
        this.growl.show({life: 5000,  severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({life: 5000,  severity: 'error', summary: 'Error', detail: this.state.message });
    }
}

ReactDOM.render(
    <CloneWbs />,
    document.getElementById('content-clone-wbs')
  );