import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

import actionFormatter from '../Base/ActionFormatter';

class ProveedorZonaList extends Component {

    constructor() {
        super();
 
    }

    render() {

       
        return (

            <BootstrapTable data={this.props.data} striped hover>

                <TableHeaderColumn width={"5%"} dataField="Id" isKey hidden>ID</TableHeaderColumn>

                <TableHeaderColumn dataField="zona_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort >Zona</TableHeaderColumn>
 
                <TableHeaderColumn dataField="Operaciones" dataFormat={actionFormatter} formatExtraData={this.props} width={'20%'} >Opciones</TableHeaderColumn>

            </BootstrapTable>

        );
    }
}
 
export default ProveedorZonaList;