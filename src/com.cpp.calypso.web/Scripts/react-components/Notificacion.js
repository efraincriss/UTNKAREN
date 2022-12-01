import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import StepWizard from 'react-step-wizard';

import {Growl} from 'primereact/components/growl/Growl';
import {Dialog} from 'primereact/components/dialog/Dialog';

import Proceso from './Notificaciones/Proceso';
import Lista from './Notificaciones/Lista';


class Notificacion extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            destinatarios: [], // catalogo de destinatarios
            procesos: [{nombre: "", formato: ""}], // catalogo de procesos
            procesoId: 0, // proceso seleccionado
            visible: false,
            correos: [], // correos electrónicos seleccionados,
            message: '',
            key: 42434,
        }
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.getDestinatarios = this.getDestinatarios.bind(this);
        this.SetProceso = this.SetProceso.bind(this)
        this.onHide = this.onHide.bind(this)
        this.GetProcesos = this.GetProcesos.bind(this);
        this.SetCorreos = this.SetCorreos.bind(this);
        this.HandleSubmit = this.HandleSubmit.bind(this);
    }

    componentWillMount(){
        this.GetProcesos();
    }

    render(){
        return(
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>

                <div className="row">
                    
                         <div className="col" align="right">
                            <button 
                             style={{marginLeft: '0.3em'}}
                            className="btn btn-sm btn-outline-primary" onClick={() => this.setState({visible: true})}>
                                <i className="fa fa-envelope"></i>
                                &nbsp; Enviar
                            </button>
                        </div>
                    

                    <Dialog header="Envío de Notificación" visible={this.state.visible} width="700px" modal={true} onHide={this.onHide}>
                        <StepWizard key={this.state.key}>
                            <Proceso 
                            SetProceso={this.SetProceso} 
                            procesos={this.state.procesos}
                            successMessage={this.successMessage}
                            warnMessage={this.warnMessage}/>

                            <Lista 
                            destinatarios={this.state.destinatarios}
                            SetCorreos={this.SetCorreos}
                            HandleSubmit={this.HandleSubmit}
                            />
                        </StepWizard>
                        
                    </Dialog>
                    

                </div>
            </div>
        )
    }

    HandleSubmit(){
        if(this.state.correos.length == 0){
            this.warnMessage("Selecciona los correos")
        } else {
            axios.post("/Proyecto/Oferta/GetNotificacion",{
                ProcesoId: this.state.procesoId,
                OfertaId: document.getElementById('OfertaId').className,
                correos: this.state.correos
            })
            .then((response) => {
                if(response.data == "Ok"){
                    this.successMessage("Notificación procesada")
                    this.setState({visible: false, destinatarios: [], procesoId: 0, key: Math.random(), correos: []})
                }
                
                
            })
            .catch((error) => {
                this.warnMessage("Intentalo más tarde")
                console.log(error);
                
            });
        }
    }

    getDestinatarios(){
        axios.post("/Proyecto/ProcesoListaDistribucion/GetCorreosProceso/" + this.state.procesoId,{})
        .then((response) => {
            this.setState({destinatarios: response.data})
            
            
        })
        .catch((error) => {
            console.log(error);
            
        });
    }

    SetProceso(id){
        this.setState({procesoId: id}, this.getDestinatarios)
    }

    SetCorreos(e){
        console.log(e)
        if(e.length > 0){
            var mails = e.map(mail => {
                return mail.correo
            })
            this.setState({correos: mails})
        } else {
            this.setState({correos: []})
        }
        
    }

    onHide(){
        this.setState({visible: false})
    }

    GetProcesos(){
        axios.post("/proyecto/ProcesoNotificacion/GetProcesos",{
            tipo: document.getElementById('TipoProceso').className
        })
        .then((response) => {
            console.log(response.data)
            if(response.data == null){

            } else {
                this.setState({procesos: response.data, blocking: false})
            }
            
            
        })
        .catch((error) => {
            console.log(error);    
        });
    }


    showSuccess() {
        this.growl.show({life: 5000,  severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({life: 5000,  severity: 'error', summary: 'Error', detail: this.state.message });
    }

    successMessage(msg){
        this.setState({message: msg}, this.showSuccess)
    }

    warnMessage(msg){
        this.setState({message: msg}, this.showWarn)
    }

}

ReactDOM.render(
    <Notificacion />,
    document.getElementById('content-notificacion')
  );