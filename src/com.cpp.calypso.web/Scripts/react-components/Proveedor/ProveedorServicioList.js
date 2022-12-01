import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

import actionFormatter from '../Base/ActionFormatter';

class ProveedorServicioList extends Component {

    constructor() {
        super();
 
    }

    render() {

       
        return (

            <BootstrapTable data={this.props.data} striped hover>

                <TableHeaderColumn width={"5%"} dataField="Id" isKey hidden>ID</TableHeaderColumn>

                <TableHeaderColumn dataField="servicio_nombre" dataSort filter={{ type: 'TextFilter', delay: 500 }} dataSort >Servicio</TableHeaderColumn>

                <TableHeaderColumn dataField="estado_nombre" dataSort filter={{ type: 'TextFilter', delay: 500 }} dataSort >Estado</TableHeaderColumn>

                <TableHeaderColumn dataField="Operaciones" dataFormat={actionFormatter} formatExtraData={this.props} >Opciones</TableHeaderColumn>

            </BootstrapTable>

        );
    }
}
 
export default ProveedorServicioList;