import React from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class DetalleAvanceTable extends React.Component{
    constructor(props){
        super(props);
        this.state = {

        }
        this.generateButton = this.generateButton.bind(this);
        this.onRowSelect = this.onRowSelect.bind(this);
    }

    onRowSelect(row, isSelected, e){;
        this.props.update_items(row.Id);
    }


    generateButton(cell, row){
        return(
            <div>
                
                <button style={{marginRight: '0.3em'}}onClick={() => this.props.SetDataDetalleIngenieria(row.Id)} className="btn btn-outline-primary btn-sm">
                <i className="fa fa-edit"></i>
                </button>
                <button onClick={() => {if (window.confirm('Estás seguro?')) this.props.DeleteItem(row.Id,"DETALLE",0)}} className="btn btn-outline-danger btn-sm">
                <i className="fa fa-trash-o"></i>
                </button>
            </div>
        )
    }


    render(){

        
        const selectRow = {
            mode: 'radio', //radio or checkbox
            className: 'clicked-row', // red color
           
            onSelect: this.onRowSelect,
            bgColor: '#6610f2'
            // selected: this.props.selectedClubs
        };
        return(
            <div>
                <BootstrapTable data={this.props.data} hover={true} pagination={ true } selectRow={ selectRow } >
                   <TableHeaderColumn dataField="codigo_item"  width={'15%'}filter={ { type: 'TextFilter', delay: 500 } } isKey={true} dataAlign="center" dataSort={true}>Código Item</TableHeaderColumn>
                   <TableHeaderColumn dataField="descripcion_item" width={'25%'}filter={ { type: 'TextFilter', delay: 500 } } dataSort={true} tdStyle={ { whiteSpace: 'normal' } } thStyle={ { whiteSpace: 'normal' } }>Descripción</TableHeaderColumn>
                   <TableHeaderColumn dataField="horas_presupuestadas"  width={'10%'} filter={ { type: 'TextFilter', delay: 500 } } dataSort={true} tdStyle={ { whiteSpace: 'normal' } } thStyle={ { whiteSpace: 'normal' } }>#H Pres</TableHeaderColumn>
                   <TableHeaderColumn dataField="cantidad_horas" width={'12%'} filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}># Horas</TableHeaderColumn>
                    <TableHeaderColumn dataField='Operaciones'  width={'12%'} dataFormat={this.generateButton.bind(this)}>Operaciones</TableHeaderColumn>
               </BootstrapTable>
            </div>
        )
    }
}