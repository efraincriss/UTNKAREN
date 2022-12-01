import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';

import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';


class CloneWbs extends React.Component{
    constructor(props) {
        super(props)
        
        this.state = {
            visible: false,
            blocking: false,
            requerimientos: [],
            proyectos: [],
            ProyectoId: 0,
            RequerimientoId: 0,
            message: '',
        }

        this.showForm = this.showForm.bind(this);
        this.onHide = this.onHide.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this)
        this.handleChangeProyecto = this.handleChangeProyecto.bind(this);
        this.handleChangeRequerimientos = this.handleChangeRequerimientos.bind(this);
        this.successMessage = this.successMessage.bind(this)
        this.warnMessage = this.warnMessage.bind(this)
        this.getProyectos = this.getProyectos.bind(this)
        this.getRequerimientos = this.getRequerimientos.bind(this)
        this.getFormSelect = this.getFormSelect.bind(this);
    }

    componentDidMount(){
        this.getProyectos();
    }

    render() {
        return (
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <input align="right" type="button" className="btn btn-outline-primary" onClick={this.showForm} value="Clonar WBS"/>

                <Dialog header="Clonar WBS" visible={this.state.visible} width="450px" modal={true} onHide={this.onHide}>
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
                            <label htmlFor="label">SR(Service Request)</label>

                            <select onChange={this.handleChangeRequerimientos} className="form-control">
                                <option value="0">-- Selecciona un requerimiento --</option>
                                {this.getFormSelect(this.state.requerimientos)}
                            </select>
                            
                        </div>
                        <button type="submit"  className="btn btn-outline-primary" icon="fa fa-fw fa-folder-open" style={{marginRight: '0.3em'}}>Clonar</button>
                        <button type="button"  className="btn btn-outline-primary" icon="fa fa-fw fa-ban" onClick={this.onHide}>Cancelar</button>
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
        this.setState({blocking: true})
        if(this.state.ProyectoId == 0){
            this.warnMessage("Selecciona un proyecto")
        } else if(this.state.RequerimientoId == 0){
            this.warnMessage("Selecciona un requerimiento")
        } else {
            axios.post("/Proyecto/Wbs/ClonarWBS",{
                RequerimientoId: this.state.RequerimientoId,
                OfertaId: document.getElementById('content-clone-wbs').className
            })
            .then((response) => {
                if(response.data == "ErrorComputos"){
                    this.warnMessage("El SR tiene una oferta definitiva con WBS");
                } else if(response.data == "ErrorDefinitiva") {
                    this.warnMessage("El SR no tiene una oferta definitiva");
                } else {
                    window.location.href = "/Proyecto/Wbs/Index/" + response.data ;
                }
                
            })
            .catch((error) => {
                console.log(error);    
            });


            this.setState({blocking: false})
            
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
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.message });
    }

    successMessage(msg) {
        this.setState({ message: msg }, this.showSuccess)
    }

    warnMessage(msg) {
        this.setState({ message: msg }, this.showWarn)
    }
    
}


ReactDOM.render(
    <CloneWbs />,
    document.getElementById('content-clone-wbs')
  );