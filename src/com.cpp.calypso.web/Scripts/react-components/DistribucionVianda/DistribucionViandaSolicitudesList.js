import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';

class DistribucionViandaSolicitudesList extends Component {

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
            <div>
                <h6 className="p-3 mb-2 bg-primary">Solicitudes No Asignadas</h6>

                <BootstrapTable data={this.props.data} selectRow={selectRowProp} pagination striped hover>

                    <TableHeaderColumn width={"5%"} dataField="Id" isKey hidden >ID</TableHeaderColumn>

                    <TableHeaderColumn dataField="solicitante_nombre"
                        tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    >Solicitante
                    </TableHeaderColumn>
                    <TableHeaderColumn dataField="locacion_nombre" 
                     tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                     thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    >Locación </TableHeaderColumn>
 <TableHeaderColumn dataField="disciplina_nombre" 
                     tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                     thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    >Disciplina </TableHeaderColumn>

                    <TableHeaderColumn dataField="pedido_viandas" dataAlign='right'
                     tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                     thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    >Viandas</TableHeaderColumn>
                    <TableHeaderColumn dataField="alcance_viandas" dataAlign='right'
                     tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                     thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    >Alcance</TableHeaderColumn>
                    <TableHeaderColumn dataField="total_pedido" dataAlign='right'
                     tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                     thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    >Total Pedido</TableHeaderColumn>

                </BootstrapTable>
            </div>
        );
    }
}



export default DistribucionViandaSolicitudesList;