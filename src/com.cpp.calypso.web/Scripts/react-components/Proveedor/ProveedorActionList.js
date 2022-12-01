import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';

class ProveedorActionList extends Component {

    constructor() {
        super();
    }

    

    render() {

       
        return (

            <BootstrapTable data={this.props.data} pagination striped hover>

                <TableHeaderColumn width={"5%"} isKey  dataField="Id"  hidden >ID</TableHeaderColumn>

                <TableHeaderColumn dataField="identificacion" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Identificación</TableHeaderColumn>

                <TableHeaderColumn dataField="razon_social" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Proveedor</TableHeaderColumn>
        
                <TableHeaderColumn dataField="Operaciones" dataFormat={actionFormatter} formatExtraData={this.props}  >Opciones</TableHeaderColumn>

            </BootstrapTable>

        );
    }
}



export default ProveedorActionList;