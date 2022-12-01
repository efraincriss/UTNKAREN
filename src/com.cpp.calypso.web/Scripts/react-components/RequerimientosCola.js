import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import moment from 'moment';
import { Dropdown } from 'primereact/components/dropdown/Dropdown'
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';


class RequerimientosCola extends React.Component {
    constructor(props){
        super(props);
        this.state = {
            blocking: true,
            data: [],
            message:''

        }

        this.handleChange = this.handleChange.bind(this);
        this.OcultarFormulario= this.OcultarFormulario.bind(this);
        this.MostrarFormulario = this.MostrarFormulario.bind(this);
        this.alertMessage = this.alertMessage.bind(this);
        this.infoMessage = this.infoMessage.bind(this);

        this.Loading = this.Loading.bind(this);
        this.StopLoading = this.StopLoading.bind(this);
        
        this.ConsultarSR=this.ConsultarSR.bind(this);

        
        
        
        
    }

    componentDidMount (){
        this.ConsultarSR()
    }
    GenerarBotones(cell, row){
        return(
            <div>
                <button onClick={() => this.Redireccionar(row)} className="btn btn-sm btn-outline-indigo">Ver</button>
            </div>
        )
    }
    render(){
        return(
            <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <BootstrapTable data={ this.state.data } hover={true} pagination={true}>
             
             <TableHeaderColumn 
             dataField='proyecto_codigo' 
             isKey
             tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             filter={ { type: 'TextFilter', delay: 500 } } 
             width={"10%"} 
             dataSort={true}>Proyecto</TableHeaderColumn>

             <TableHeaderColumn 
             dataField='codigo' 
             tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             filter={ { type: 'TextFilter', delay: 500 } } 
             width={"10%"} 
             dataSort={true}>Cod_Trab</TableHeaderColumn>

             <TableHeaderColumn 
             dataField='descripcion' 
             tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             filter={ { type: 'TextFilter', delay: 500 } } 
             width={"30%"} 
             dataSort={true}>Descripción</TableHeaderColumn>
                           <TableHeaderColumn 
                 dataField='monto_construccion' 
                 dataFormat={this.MontosFormato} 
                 tdStyle={{ whiteSpace: 'normal' }} 
                 thStyle={{ whiteSpace: 'normal' }} 
                 filter={ { type: 'TextFilter', delay: 500 } } 
                 tdStyle={{ whiteSpace: 'normal', fontSize: '11px' ,textAlign: 'right'}}
                 width={"10%"} 
                 thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                 dataSort={true}>
                 M. Construcción</TableHeaderColumn>
                 <TableHeaderColumn 
                 dataField='monto_ingenieria' 
                 dataFormat={this.MontosFormato} 
                 tdStyle={{ whiteSpace: 'normal' }} 
                 thStyle={{ whiteSpace: 'normal' }} 
                 filter={ { type: 'TextFilter', delay: 500 } } 
                 tdStyle={{ whiteSpace: 'normal', fontSize: '11px' ,textAlign: 'right'}}
                 width={"10%"} 
                 thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                 dataSort={true}>
                 M. Ingeniería</TableHeaderColumn>
                 <TableHeaderColumn 
                 dataField='monto_procura' 
                 dataFormat={this.MontosFormato} 
                 tdStyle={{ whiteSpace: 'normal' }} 
                 thStyle={{ whiteSpace: 'normal' }} 
                 filter={ { type: 'TextFilter', delay: 500 } }
                 tdStyle={{ whiteSpace: 'normal', fontSize: '11px' ,textAlign: 'right'}}
                 thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                 width={"10%"} 
                 dataSort={true}>
                 M. Suministros</TableHeaderColumn>
                {/* 
                 <TableHeaderColumn 
             dataField='Operaciones'
             width={'8%'}
             dataFormat={this.GenerarBotones.bind(this)}></TableHeaderColumn>
             */}
             <TableHeaderColumn 
             dataField='tiene_presupuesto' 
             tdStyle={{ whiteSpace: 'normal', fontSize: '11px' ,textAlign: 'center'}}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' ,textAlign: 'center'}}
             filter={ { type: 'TextFilter', delay: 500 } } 
             width={"30%"} 
             dataSort={true}>Estado</TableHeaderColumn>
         </BootstrapTable>  
              
            </BlockUi>
        );
    }

    
    

    ConsultarSR(){
        axios.post("/Proyecto/OfertaPresupuesto/ListarRequerimientoenCola", {})
        .then((response) => {
            console.log(response.data)
            this.setState({ data: response.data, blocking: false })
         
        })
        .catch((error) => {
            this.setState({blocking: false })
            this.alertMessage("Ocurrió un error al consultar los Requerimientos")
            console.log(error);
        });
    }

    

    handleChange(event){
        this.setState({[event.target.name]: event.target.value});
    }

    Redireccionar(id){
        window.location.href = "/Proyecto/OfertaPresupuesto/Details/" + id;
    }

    Loading(){
        this.setState({blocking: true})
    }

    StopLoading(){
        this.setState({blocking: false})
    }

    OcultarFormulario(){
        this.setState({visible: false})
    }

    MostrarFormulario(){
        this.setState({visible: true})
    }

    showInfo() {
        this.growl.show({severity: 'info', summary: 'Información', detail: this.state.message});
    }

    infoMessage(msg){
        this.setState({message: msg}, this.showInfo)
    }

    showAlert() {
        this.growl.show({severity: 'warn', summary: 'Alerta', detail: this.state.message});
    }

    alertMessage(msg){
        this.setState({message: msg}, this.showAlert)
    }
}

ReactDOM.render(
    <RequerimientosCola />,
    document.getElementById('content')
  );