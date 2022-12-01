import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';
import dateFormatter from '../Base/DateFormatter';

class DistribucionViandaEditList extends Component {

    constructor() {
        super();
    }



    render() {


        return (
            <div>
                <h6 className="p-3 mb-2 bg-primary">Solicitudes Asignadas</h6>

                <BootstrapTable data={this.props.data} pagination striped hover>
                    <TableHeaderColumn width={"5%"} dataField="detalle_distribuccion_id" hidden isKey >ID</TableHeaderColumn>

                    <TableHeaderColumn dataField="solicitud_id" hidden
                     >solicitud_id</TableHeaderColumn>
                    <TableHeaderColumn dataField="solicitante_nombre"
                        tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        >Solicitante</TableHeaderColumn>
                    <TableHeaderColumn dataField="AreaId" hidden >AreaId</TableHeaderColumn>
                    <TableHeaderColumn dataField="area_nombre" hidden
                        tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} 
                        filter={{ type: 'TextFilter', delay: 500 }}>Area</TableHeaderColumn>
                              <TableHeaderColumn dataField="disciplina_nombre" 
                        tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} 
                        filter={{ type: 'TextFilter', delay: 500 }}>Disciplina</TableHeaderColumn>
                    <TableHeaderColumn dataField="LocacionId" hidden 
                    >AreaId</TableHeaderColumn>
                    <TableHeaderColumn dataField="locacion_nombre"
                        tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                         filter={{ type: 'TextFilter', delay: 500 }}>Locación</TableHeaderColumn>
                    <TableHeaderColumn dataField="ProveedorId" hidden
                     >ProveedorId</TableHeaderColumn>
                    <TableHeaderColumn dataField="proveedor_nombre"
                        tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} 
                        filter={{ type: 'TextFilter', delay: 500 }}>Proveedor</TableHeaderColumn>
                                     <TableHeaderColumn dataField="anotador_nombre"
                        tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} 
                        filter={{ type: 'TextFilter', delay: 500 }}>Anotador</TableHeaderColumn>
                    <TableHeaderColumn dataField="tipo_comida_nombre" hidden
                     >Tipo Comida</TableHeaderColumn>
                    <TableHeaderColumn dataField="fecha"
                        tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} 
                        dataFormat={dateFormatter} formatExtraData={this.props} 
                        filter={{ type: 'TextFilter', delay: 500 }}  
                        >Fecha</TableHeaderColumn>
                    <TableHeaderColumn dataField="total_pedido"
                        tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                         dataAlign='right' filter={{ type: 'TextFilter', delay: 500 }}>Total Pedido</TableHeaderColumn>
                    <TableHeaderColumn dataField="estado_solicitud" hidden 
                    >estado</TableHeaderColumn>
                    <TableHeaderColumn dataField="estado_solicitud_nombre"
                        tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        filter={{ type: 'TextFilter', delay: 500 }} 
                        >Estado</TableHeaderColumn>

                    <TableHeaderColumn dataField="Operaciones"
                      tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                      thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    dataFormat={actionFormatter} formatExtraData={this.props}  >Opciones</TableHeaderColumn>

                </BootstrapTable>
            </div>
        );
    }
}



export default DistribucionViandaEditList;