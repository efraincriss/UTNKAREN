import React, { Component } from 'react';

import {DataTable} from 'primereact/components/datatable/DataTable';
import {Column} from 'primereact/components/column/Column';

export default class Lista extends Component{
    constructor(props) {
      super(props)
    
      this.state = {
         selected: []
      }
      this.SelectCorreo = this.SelectCorreo.bind(this);
    }

    render() {
      return (
        <div>
            <button onClick={this.props.previousStep} className="btn btn-outline-primary">Anterior</button>

            <button className="btn btn-outline-primary" onClick={this.props.HandleSubmit}>
                <i className="fa fa-envelope"></i>
                &nbsp; Enviar
            </button>

            <DataTable value={this.props.destinatarios} header="Listas de distribución" footer="Envio de notificación"
                selection={this.state.selected} onSelectionChange={(e) => this.SelectCorreo(e)}>
                <Column selectionMode="multiple" style={{width:'2em'}}/>
                <Column field="nombre_lista" header="Lista de Distribución" />
                <Column field="correo" header="Correo" />
                <Column field="nombres" header="Nombres" />
            </DataTable>
        </div>
      )
    }


    SelectCorreo(e){
        this.setState({selected: e.data})
        this.props.SetCorreos(e.data)
    }
}