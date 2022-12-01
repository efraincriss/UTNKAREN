import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

import actionFormatter from '../Base/ActionFormatter';
import dateFormatter from '../Base/DateFormatter';

class MenuList extends Component {

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
   
         this.props.onSelectAction(isSelected,newList);
    }

    aprobadoFormatter(cell, row) {
        if (cell === 0) {
            return "Reprobado";
        } else {
            return "Aprobado";
        }
    }

    render() {

        console.log("Selecion:");
        console.log(this.props.selectIds);
        const selectRowProp = {
            mode: 'checkbox',
            clickToSelect: true,
            onSelect: this.onRowSelect,
            onSelectAll: this.onSelectAll,
            selected: this.props.selectIds
        };
       
        return (

            <BootstrapTable data={this.props.data} selectRow={selectRowProp} pagination striped hover>

                <TableHeaderColumn width={"5%"} dataField="Id" isKey hidden>ID</TableHeaderColumn>

                <TableHeaderColumn dataField="fecha_inicial" filter={{ type: 'TextFilter', delay: 500 }} dataFormat={dateFormatter} formatExtraData={this.props}  dataSort >Fecha Inicial</TableHeaderColumn>

                <TableHeaderColumn dataField="fecha_final" filter={{ type: 'TextFilter', delay: 500 }} dataFormat={dateFormatter} formatExtraData={this.props}  >Fecha Final</TableHeaderColumn>

                <TableHeaderColumn dataField="aprobado" filter={{ type: 'TextFilter', delay: 500 }} dataFormat={this.aprobadoFormatter} dataSort >Aprobado</TableHeaderColumn>

                <TableHeaderColumn dataField="Operaciones" width={'20%'} dataFormat={actionFormatter} formatExtraData={this.props} >Opciones</TableHeaderColumn>

                 
            </BootstrapTable>

        );
    }
}
 
export default MenuList;