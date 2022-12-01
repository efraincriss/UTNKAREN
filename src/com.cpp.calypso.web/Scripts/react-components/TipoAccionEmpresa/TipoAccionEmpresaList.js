import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';

class TipoAccionEmpresaList extends Component {

    constructor() {
        super();

        this.onRowSelect = this.onRowSelect.bind(this);
        this.onSelectAll = this.onSelectAll.bind(this);
    }
     

    onRowSelect(row, isSelected, e) {

        console.log(`is selected: ${isSelected}, ${row}`);
        if (this.props.onSelectAction !== undefined) {
            let newList = [];

            if (isSelected) {
                //Add
                newList = [...this.props.selectIds, row.Id];
            } else {
                //Delete
                let propsLocal = { ...this.props };
                newList = propsLocal.selectIds.filter(item => item !== row.Id);
            }

        
            this.props.onSelectAction(isSelected, newList);
        }
    }

    onSelectAll(isSelected, rows) {

        console.log(`is selected all: ${isSelected}, ${rows}`);
        if (this.props.onSelectAction !== undefined) {

            let newList = [];

            if (isSelected) {
                //Add All
                for (var i = 0; i < rows.length; i++) {
                    newList.push(rows[i].Id);
                }
            }
   
            this.props.onSelectAction(isSelected, newList);
        }
    }

  
    render() {

     
        return (

            <BootstrapTable data={this.props.data} pagination striped hover>
                <TableHeaderColumn width={"5%"} dataField="Id" isKey  hidden>ID</TableHeaderColumn>
                <TableHeaderColumn dataField="empresa_nombre" thStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} dataSort>Empresa</TableHeaderColumn>
                <TableHeaderColumn dataField="tipo_comida_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Tipo Comida</TableHeaderColumn>
                <TableHeaderColumn dataField="accion_nombre" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Acción</TableHeaderColumn>
                <TableHeaderColumn dataField="hora_desde" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Hora Desde</TableHeaderColumn>
                <TableHeaderColumn dataField="hora_hasta" filter={{ type: 'TextFilter', delay: 500 }} dataSort>Hora Hasta</TableHeaderColumn>
                <TableHeaderColumn dataField="Operaciones" dataFormat={actionFormatter} formatExtraData={this.props} width={'20%'}  >Opciones</TableHeaderColumn>
            </BootstrapTable>

        );
    }
}



export default TipoAccionEmpresaList;