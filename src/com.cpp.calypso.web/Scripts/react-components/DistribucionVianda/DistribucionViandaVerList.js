import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';
import dateFormatter from '../Base/DateFormatter';

class DistribucionViandaVerList extends Component {

    constructor() {
        super();
    }
     
    render() {

       
        return (

    
            <BootstrapTable data={this.props.data} pagination striped hover >
                <TableHeaderColumn width={"5%"} dataField="Id" isKey hidden >ID</TableHeaderColumn>

                <TableHeaderColumn dataField="proveedor_zona" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }}>ZONA</TableHeaderColumn>

                <TableHeaderColumn dataField="proveedor_identificacion" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }}>RUC</TableHeaderColumn>

                <TableHeaderColumn dataField="proveedor_nombre" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }}>RESTAURANTE</TableHeaderColumn>

                <TableHeaderColumn dataField="total_pedido" dataAlign='right'filter={{ type: 'TextFilter', delay: 500 }} >TOTAL</TableHeaderColumn>

                <TableHeaderColumn dataField="Operaciones" dataFormat={actionFormatter} formatExtraData={this.props}  filter={{ type: 'TextFilter', delay: 500 }}>Opciones</TableHeaderColumn>

            </BootstrapTable>

        );
    }
}



export default DistribucionViandaVerList;