import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';
import dateFormatter from '../Base/DateFormatter';

class DistribucionViandaVerDetalleList extends Component {

    constructor() {
        super();
    }

     

    render() {

       
        return (

    
            <BootstrapTable data={this.props.data} pagination striped hover >
                <TableHeaderColumn width={"5%"} dataField="ProveedorId" isKey hidden>ID</TableHeaderColumn>

                <TableHeaderColumn dataField="solicitante_nombre" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }} >Solicitantes</TableHeaderColumn>

                <TableHeaderColumn dataField="area_nombre" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }} >Area</TableHeaderColumn>

                <TableHeaderColumn dataField="locacion_nombre" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }} >Locaci�n</TableHeaderColumn>

                <TableHeaderColumn dataField="total_pedido" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }} >TOTAL</TableHeaderColumn>
            </BootstrapTable>

        );
    }
}



export default DistribucionViandaVerDetalleList;