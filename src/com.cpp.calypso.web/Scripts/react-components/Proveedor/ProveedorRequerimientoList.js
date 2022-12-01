import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

import actionFormatter from '../Base/ActionFormatter';
import dateFormatter from '../Base/DateFormatter';


class ProveedorRequerimientoList extends Component {

    constructor() {
        super();

        this.onRowSelect = this.onRowSelect.bind(this);
        this.onSelectAll = this.onSelectAll.bind(this);
    }



    onRowSelect(row, isSelected, e) {

        console.log(`is selected: ${isSelected}, ${row}`);

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

    onSelectAll(isSelected, rows) {

        console.log(`is selected all: ${isSelected}, ${rows}`);

        let newList = [];

        if (isSelected) {
            //Add All
            for (var i = 0; i < rows.length; i++) {
                newList.push(rows[i].Id);
            }
        }

        this.props.onSelectAction(isSelected, newList);
    }
 
    render() {

        const selectRowProp = {
            mode: 'checkbox',
            clickToSelect: true,
            onSelect: this.onRowSelect,
            onSelectAll: this.onSelectAll,
            selected: this.props.selectIds
        };
       
        return (

            <BootstrapTable data={this.props.data} selectRow={selectRowProp} striped hover>

                <TableHeaderColumn width={"5%"} dataField="Id" isKey  hidden>ID</TableHeaderColumn>

                <TableHeaderColumn dataField="requisito_nombre" dataSort filter={{ type: 'TextFilter', delay: 500 }}>Requerimiento</TableHeaderColumn>

                <TableHeaderColumn dataField="cumple_nombre" dataSort  filter={{ type: 'TextFilter', delay: 500 }}>Cumple</TableHeaderColumn>

                <TableHeaderColumn dataField="observaciones" tdStyle={{ whiteSpace: 'normal' }}   >Observaciones</TableHeaderColumn>
                
            </BootstrapTable>

        );
    }
}
 
export default ProveedorRequerimientoList;