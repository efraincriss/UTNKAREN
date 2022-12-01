import React from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class ListadoDetallesIngenieriaTable extends React.Component{
    constructor(props){
        super(props);
        this.state = {

        }

    }



    render(){
        return(
            <div>
                <BootstrapTable data={this.props.data} hover={true} pagination={ true } >
                    <TableHeaderColumn dataField="codigo_item"  filter={ { type: 'TextFilter', delay: 500 } } isKey={true} dataAlign="center" dataSort={true}>Código Item</TableHeaderColumn>
                    <TableHeaderColumn dataField="descripcion_item" filter={ { type: 'TextFilter', delay: 500 } } dataSort={true} tdStyle={ { whiteSpace: 'normal' } } thStyle={ { whiteSpace: 'normal' } }>Descripción</TableHeaderColumn>
                    <TableHeaderColumn dataField="cantidad_horas"  filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}># Horas</TableHeaderColumn>
                    <TableHeaderColumn dataField="cantidad_acumulada_anterior"  filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}># Horas Anterior</TableHeaderColumn>
                    <TableHeaderColumn dataField="cantidad_acumulada"  filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}># Horas Acumuladas</TableHeaderColumn>
                    <TableHeaderColumn dataField="valor_real"  filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}>Valor Real</TableHeaderColumn>
                    
                    
                
                </BootstrapTable>
            </div>
        )
    }
}