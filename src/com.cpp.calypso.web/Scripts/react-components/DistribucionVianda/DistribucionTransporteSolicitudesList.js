import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';

class DistribucionTransporteSolicitudesList extends Component {

    constructor() {
        super();

        
    }
 
    render() {

      
        return (
            <div>
                <h6 className="p-3 mb-2 bg-primary">Detalle de Solicitudes</h6>

                <BootstrapTable data={this.props.data}  striped hover>

                    <TableHeaderColumn width={"5%"} dataField="Id" isKey hidden>ID</TableHeaderColumn>

                    <TableHeaderColumn dataField="solicitante_nombre"
                    tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    >Solicitante</TableHeaderColumn>
                    <TableHeaderColumn dataField="area_nombre" 
                    tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    >Área  </TableHeaderColumn>
                    <TableHeaderColumn dataField="locacion_nombre" 
                    tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    >Locación </TableHeaderColumn>
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
                    >Total Vianda</TableHeaderColumn>
 
                </BootstrapTable>
            </div>
        );
    }
}



export default DistribucionTransporteSolicitudesList;