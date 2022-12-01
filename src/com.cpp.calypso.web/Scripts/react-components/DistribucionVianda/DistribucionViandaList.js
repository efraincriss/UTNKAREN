import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';
import dateFormatter from '../Base/DateFormatter';

class DistribucionViandaList extends Component {

    constructor() {
        super();
    }

     

    render() {

       
        return (

    
            <BootstrapTable data={this.props.data} pagination striped hover >
                <TableHeaderColumn width={"5%"} dataField="Id" isKey hidden>ID</TableHeaderColumn>

                <TableHeaderColumn width={"18%"} dataField="fecha" dataFormat={dateFormatter} formatExtraData={this.props} filter={{ type: 'TextFilter', delay: 500 }} >Fecha</TableHeaderColumn>

                <TableHeaderColumn  width={"10%"} dataField="tipo_comida_nombre" filter={{ type: 'TextFilter', delay: 500 }} >Tipo Comida</TableHeaderColumn>

                <TableHeaderColumn width={"10%"} dataField="total_solicitudes" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }} >Total Solicitudes</TableHeaderColumn>

                <TableHeaderColumn width={"10%"} dataField="total_pedido" dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }} >Total Pedidos</TableHeaderColumn>

                <TableHeaderColumn width={"20%"} dataField="Operaciones" dataFormat={actionFormatter} formatExtraData={this.props}  >Opciones</TableHeaderColumn>

            </BootstrapTable>

        );
    }
}



export default DistribucionViandaList;