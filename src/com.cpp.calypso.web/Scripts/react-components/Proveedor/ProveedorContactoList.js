import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

import actionFormatter from '../Base/ActionFormatter';
import dateFormatter from '../Base/DateFormatter';

class ProveedorContactoList extends Component {

    constructor() {
        super();
 
    }

    render() {

       
        return (

            <BootstrapTable data={this.props.data} striped hover>

                <TableHeaderColumn width={'5%'} dataField="Id" isKey hidden>ID</TableHeaderColumn>

                <TableHeaderColumn dataField="codigo" filter={{ type: 'TextFilter', delay: 500 }} dataSort >Código</TableHeaderColumn>

                <TableHeaderColumn dataField="empresa_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort >Empresa</TableHeaderColumn>
           
                <TableHeaderColumn dataField="estado_nombre"  filter={{ type: 'TextFilter', delay: 500 }} dataSort>Estado</TableHeaderColumn>
 
                <TableHeaderColumn dataField="fecha_inicio" dataFormat={dateFormatter} formatExtraData={this.props}  filter={{ type: 'TextFilter', delay: 500 }} dataSort tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }} thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}>Fecha Inicio</TableHeaderColumn>

                <TableHeaderColumn dataField="fecha_fin" dataFormat={dateFormatter} formatExtraData={this.props}  filter={{ type: 'TextFilter', delay: 500 }} dataSort tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }} thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}>Fecha Fin</TableHeaderColumn>

                <TableHeaderColumn dataField="Operaciones" width={'30%'}  dataFormat={actionFormatter} formatExtraData={this.props} >Opciones</TableHeaderColumn>

            </BootstrapTable>

        );
    }
}
 
export default ProveedorContactoList;