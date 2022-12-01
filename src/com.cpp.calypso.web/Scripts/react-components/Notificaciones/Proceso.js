import React, { Component } from 'react';

import {DataTable} from 'primereact/components/datatable/DataTable';
import {Column} from 'primereact/components/column/Column';

export default class Proceso extends Component{
    constructor(props) {
      super(props)
    
      this.state = {
         selected: [],
         procesoId: 0,
      }
      this.SelectProceso = this.SelectProceso.bind(this);
      this.NextStep = this.NextStep.bind(this)
    }

    render() {
      return (
        <div>
            <DataTable value={this.props.procesos} header="Procesos" footer="Envio de notificación"
                selection={this.state.selected} onSelectionChange={(e) => this.SelectProceso(e)}>
                <Column selectionMode="single" style={{width:'2em'}}/>
                <Column field="nombre" header="Lista de Distribución" />
                <Column field="formato" header="Correo" />
            </DataTable>

            <button onClick={this.NextStep} className="btn btn-outline-primary">Siguiente</button>  
        </div>
      )
    }

    SelectProceso(e){
        this.props.SetProceso(e.data.Id);
        this.setState({selected: e.data, procesoId: e.data.Id})
    }

    NextStep(){
        if(this.state.procesoId == 0){
            this.props.warnMessage("Selecciona un proceso");
        } else {
            this.props.nextStep();
        }
    }
    
}