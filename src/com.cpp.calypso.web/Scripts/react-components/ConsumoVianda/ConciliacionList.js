import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';

const estadoFormatter = (cell, row) => ((row.total_consumido < row.total_pedido) ? (<span className="text-sm-center text-danger">Pendiente</span>) : (<span className="text-sm-center text-success">Ok</span>) );

const indicadorFormatter = (cell, row) => ((row.total_consumido < row.total_pedido) ? (<span className="text-sm-center text-danger"><i className="fa fa-circle fa-lg"></i></span>) : (<span className="text-sm-center text-success"><i className="fa fa-circle fa-lg"></i></span>));

 

class ConciliacionList extends Component {

    constructor() {
        super();
        
    }
  
    render() {
  
        return (
 
            <BootstrapTable data={this.props.data}   pagination striped hover>

                <TableHeaderColumn width={"5%"}  dataField="Id" isKey  hidden>ID</TableHeaderColumn>

                <TableHeaderColumn dataField="solicitante_nombre" thStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} dataSort>Solicitante</TableHeaderColumn>

                <TableHeaderColumn dataField="proveedor_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Restaurante</TableHeaderColumn>

                <TableHeaderColumn dataField="tipo_comida_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Tipo Comida</TableHeaderColumn>

                <TableHeaderColumn dataField="area_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Área</TableHeaderColumn>

                <TableHeaderColumn dataField="locacion_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Locación</TableHeaderColumn>

                <TableHeaderColumn dataField="total_pedido" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }} dataSort>Pedido</TableHeaderColumn>

                <TableHeaderColumn dataField="total_consumido" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }} dataSort>Consumido</TableHeaderColumn>

                <TableHeaderColumn dataField="total_consumido" dataFormat={indicadorFormatter} dataAlign='center' dataSort>Indicador</TableHeaderColumn>

                <TableHeaderColumn dataField="Operaciones" dataFormat={actionFormatter} formatExtraData={this.props}   >Opciones</TableHeaderColumn>

            </BootstrapTable>

        );
    }
}



export default ConciliacionList;