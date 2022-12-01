import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

import actionFormatter from '../Base/ActionFormatter';
import dateFormatter from '../Base/DateFormatter';


class ProveedorNovedadList extends Component {

    constructor() {
        super();
 
    }

    resultadoFormatter(cell, row) {
        if (cell == 0) {
            return "Pendiente";
        } else {
            return "Resuelto";
        }
    }

    render() {

       
        return (

            <BootstrapTable data={this.props.data} striped hover>

                <TableHeaderColumn width={"5%"} dataField="Id" isKey hidden>ID</TableHeaderColumn>


                <TableHeaderColumn dataField="fecha_registro" dataFormat={dateFormatter} formatExtraData={this.props} dataSort filter={{ type: 'TextFilter', delay: 500 }}>Fecha Registro</TableHeaderColumn>

                <TableHeaderColumn dataField="descripcion" dataSort filter={{ type: 'TextFilter', delay: 500 }}>Descripción</TableHeaderColumn>

                <TableHeaderColumn dataField="resuelta" dataSort dataFormat={this.resultadoFormatter} formatExtraData={this.props} filter={{ type: 'TextFilter', delay: 500 }}>Resuelta</TableHeaderColumn>

                <TableHeaderColumn dataField="Operaciones" dataFormat={actionFormatter} formatExtraData={this.props} >Opciones</TableHeaderColumn>

            </BootstrapTable>

        );
    }
}
 
export default ProveedorNovedadList;