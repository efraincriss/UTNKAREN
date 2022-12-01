import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

import actionFormatter from '../Base/ActionFormatter';

class ProveedorContactoTipoOpcionComidaList extends Component {

    constructor() {
        super();
 
    }

    
    render() {
 
        return (

            <BootstrapTable data={this.props.data}  striped hover >

                <TableHeaderColumn width={"5%"}  dataField="Id" isKey hidden>ID</TableHeaderColumn>

                <TableHeaderColumn dataField="tipo_comida_nombre"   dataSort >Tipo Comida</TableHeaderColumn>

                <TableHeaderColumn dataField="opcion_comida_nombre" dataSort >Opciones de Comida</TableHeaderColumn>

                <TableHeaderColumn dataField="costo" dataAlign='right'   dataSort>Costo</TableHeaderColumn>

                <TableHeaderColumn dataField="Operaciones" dataFormat={actionFormatter} formatExtraData={this.props} >Opciones</TableHeaderColumn>

            </BootstrapTable>

        );
    }
}
 
export default ProveedorContactoTipoOpcionComidaList;