import React from 'react';
import axios from 'axios';
import moment from 'moment';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import {Dialog} from 'primereact/components/dialog/Dialog';

export default class ItemTable extends React.Component{
    constructor(props){
        super(props);
        
    }

    generateButton(cell, row){
        return(
            <div>
                <button onClick={() => this.props.selectItem(row.Id, row.codigo,)} className="btn btn-outline-indigo">Seleccionar</button>
            </div>
        )
    }

    render(){
        return(
            <div>
                <BootstrapTable data={this.props.data} hover={true} pagination={ true } >
                    <TableHeaderColumn dataField="Id" filter={ { type: 'TextFilter', delay: 500 } } isKey={true} dataAlign="center" dataSort={true}>Improductividad</TableHeaderColumn>
                    <TableHeaderColumn dataField="codigo" filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}>Hora Inicio</TableHeaderColumn>
                    <TableHeaderColumn dataField="nombre" filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}>Hora Fin</TableHeaderColumn>
                    <TableHeaderColumn dataField='Operaciones' dataFormat={this.generateButton.bind(this)}>Operaciones</TableHeaderColumn>
                </BootstrapTable> 
            </div>
        )
    }
}