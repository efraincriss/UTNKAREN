import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


 
import dateFormatter from '../Base/DateFormatter'; 
import actionFormatter from '../Base/ActionFormatter'; 

const indicadorFormatter = (cell, row) => ((row.total_consumido < row.total_pedido) ? (<span className="text-sm-center text-danger"><i className="fa fa-circle fa-lg"></i></span>) : (<span className="text-sm-center text-success"><i className="fa fa-circle fa-lg"></i></span>));

const customActionFormatter = (cell, row, props) => ((row.total_consumido < row.total_pedido) ? (actionFormatter(cell, row, props)) : (<span></span>));

 
class ConciliacionList extends Component {

    constructor() {
        super();
        
    }
  
    render() {

        

        return (
 
            <BootstrapTable data={this.props.data}   pagination striped hover>

                <TableHeaderColumn width={"5%"}  dataField="Id" isKey  hidden>ID</TableHeaderColumn>

                <TableHeaderColumn dataField="fecha_solicitud" dataFormat={dateFormatter} formatExtraData={this.props} filter={{ type: 'TextFilter', delay: 500 }} dataSort>Fecha </TableHeaderColumn>

                <TableHeaderColumn dataField="zona_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Zona</TableHeaderColumn>

                <TableHeaderColumn dataField="tipo_comida_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Tipo Comida</TableHeaderColumn>

                <TableHeaderColumn dataField="area_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Área</TableHeaderColumn>

                <TableHeaderColumn dataField="locacion_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Locación</TableHeaderColumn>

                <TableHeaderColumn dataField="total_pedido" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }} dataSort>Pedido</TableHeaderColumn>

                <TableHeaderColumn dataField="total_consumido" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }} dataSort>Consumido</TableHeaderColumn>

                <TableHeaderColumn dataField="total_consumido" dataFormat={indicadorFormatter} dataAlign='center' dataSort>Indicador</TableHeaderColumn>

                <TableHeaderColumn dataField="Operaciones" dataFormat={customActionFormatter} formatExtraData={this.props}>Opciones</TableHeaderColumn>

            </BootstrapTable>

        );
    }
}



export default ConciliacionList;