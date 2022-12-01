import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';

class SolicitudViandaList extends Component {

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

             

            <BootstrapTable data={this.props.data} selectRow={selectRowProp}  pagination striped hover>

                <TableHeaderColumn width={"5%"}  dataField="Id"  tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}  isKey  hidden>ID</TableHeaderColumn>

                <TableHeaderColumn dataField="solicitante_nombre"    tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} filter={{ type: 'TextFilter', delay: 500 }} dataSort>Solicitante</TableHeaderColumn>

                <TableHeaderColumn dataField="estado_nombre"    tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}   filter={{ type: 'TextFilter', delay: 500 }} dataSort>Estado</TableHeaderColumn>
                <TableHeaderColumn dataField="disciplina_nombre"     tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} filter={{ type: 'TextFilter', delay: 500 }} dataSort>Disciplina</TableHeaderColumn>

                <TableHeaderColumn dataField="tipo_comida_nombre"     tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} filter={{ type: 'TextFilter', delay: 500 }} dataSort>Tipo Comida</TableHeaderColumn>

                <TableHeaderColumn dataField="locacion_nombre"     tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} filter={{ type: 'TextFilter', delay: 500 }} dataSort>Locación</TableHeaderColumn>

                <TableHeaderColumn dataField="total_pedido"    tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }} dataSort>Viandas Totales</TableHeaderColumn>

                <TableHeaderColumn dataField="Operaciones"   tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
             thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}  dataFormat={actionFormatter} formatExtraData={this.props} width={'20%'}  >Opciones</TableHeaderColumn>

            </BootstrapTable>

        );
    }
}



export default SolicitudViandaList;