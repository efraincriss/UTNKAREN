import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class ComputosTable extends React.Component{
    constructor(props){
        super(props);
        this.state = {

        }
        this.generateButton = this.generateButton.bind(this);
    }


    generateButton(cell, row){
        return(
            <div>
                <button className="btn btn-outline-primary" onClick={() => this.props.selectComputo(row.Id, row.item_nombre, row.precio_unitario,row.cantidad_acumulada_anterior, row.cantidad_eac)} style={{float:'left', marginRight:'0.3em'}}>Seleccionar</button>
            </div>
        )
    }


    render(){
        return(
            <div>
                <BootstrapTable data={this.props.data} hover={true} pagination={ true } >
                    <TableHeaderColumn dataField="item_codigo"  width={'15%'}filter={ { type: 'TextFilter', delay: 500 } } isKey={true} dataAlign="center" dataSort={true}>Código Item</TableHeaderColumn>
                    <TableHeaderColumn dataField="item_nombre" tdStyle={ { whiteSpace: 'normal' }} thStyle={ { whiteSpace: 'normal' }} filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}>Nombre</TableHeaderColumn>
                    <TableHeaderColumn dataField="item_padre_nombre" tdStyle={ { whiteSpace: 'normal' } } thStyle={ { whiteSpace: 'normal' } } width={'35%'} filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}>Código Padre</TableHeaderColumn>
                    <TableHeaderColumn dataField='Operaciones'  width={'20%'} dataFormat={this.generateButton.bind(this)}>Operaciones</TableHeaderColumn>
                </BootstrapTable>
            </div>
        );
    }
}