import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';
import timeFormatter from '../Base/TimeFormatter';


class ConciliacionConsumoList extends Component {

    constructor() {
        super();
        
    }
  
    render() {
        let total = this.props.data ? this.props.data.length : 0;
        
        return (
            <div>
                <h6 className="p-3 mb-2 bg-primary">Consumos ({total})</h6>

                <BootstrapTable data={this.props.data} pagination striped hover>

                <TableHeaderColumn width={"5%"}  dataField="Id" isKey  hidden>ID</TableHeaderColumn>

                <TableHeaderColumn dataField="colaborador_nombre"  >Colaborador</TableHeaderColumn>

                    <TableHeaderColumn dataField="fecha_consumo_vianda" dataFormat={timeFormatter} formatExtraData={this.props}  >Hora Consumo</TableHeaderColumn>

                </BootstrapTable>
            </div>
        );
    }
}



export default ConciliacionConsumoList;