import React, { Component } from 'react';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


import actionFormatter from '../Base/ActionFormatter';
import SelectCellEditor from '../Base/SelectCellEditor';
import DropdownCellEditor from '../Base/DropdownCellEditor';

const cellEditProp = {
    mode: 'click'
};



class DistribucionTransporteList extends Component {

    constructor() {
        super();
    }

    format=(cell)=>{
    if(cell!=null){
        return cell;
    }else{
        return (<label><b>Seleccione un Transportista..</b></label>);
    }

    }

    render() {

        const createSelectEditor = (onUpdate, props) => (<SelectCellEditor onUpdate={onUpdate} {...props} />);
        //const createSelectEditor = (onUpdate, props) => (<DropdownCellEditor onUpdate={onUpdate} {...props} />);
        const optionsSelectEditor = { fieldValueName: "Id", fieldTextName: "nombres", onUpdateData: this.props.onUpdateData, data: this.props.dataExtra, textSelect: "Seleccione un Transportista" };

        return (
            <div>
                <h6 className="p-3 mb-2 bg-primary">Pedidos a Restaurantes</h6>

                <BootstrapTable data={this.props.data} cellEdit={cellEditProp} pagination striped hover>

                    <TableHeaderColumn editable={false} width={"5%"} dataField="Id" tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} isKey hidden >ID</TableHeaderColumn>

                    <TableHeaderColumn editable={false} dataField="ProveedorId" tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} hidden>ProveedorId</TableHeaderColumn>
                    <TableHeaderColumn editable={false} dataField="proveedor_nombre" tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} >Restaurante </TableHeaderColumn>
                    <TableHeaderColumn editable={false} dataField="total_pedido" tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} dataAlign='right'>Total</TableHeaderColumn>
                    <TableHeaderColumn editable={false} dataField="conductor_asignado_id" tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} hidden >conductor_asignado_id</TableHeaderColumn>
                    <TableHeaderColumn
                        
                        dataField='nombres'
                        width={"30%"}
                        editable={true}
                        dataFormat={this.format}
                        tdStyle={{ whiteSpace: 'normal', alignContent:'center' ,alignItems:'center' ,fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal',alignContent:'center' ,alignItems:'center' , fontSize: '11px' }}
                        customEditor={{ getElement: createSelectEditor, customEditorParameters: optionsSelectEditor}}
                    >
                        Asignar Transportista
                    </TableHeaderColumn>
                    <TableHeaderColumn editable={false} dataField="conductor_asignado_nombre" tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} >Asignado A</TableHeaderColumn>

                    <TableHeaderColumn editable={false} dataField="estado" hidden tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}>Estado_id</TableHeaderColumn>
                    <TableHeaderColumn editable={false} dataField="estado_nombre" tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} >Estado</TableHeaderColumn>

                    <TableHeaderColumn tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                        thStyle={{ whiteSpace: 'normal', fontSize: '11px' }} editable={false} dataField="Operaciones" dataFormat={actionFormatter} formatExtraData={this.props} width={'20%'}  >Opciones</TableHeaderColumn>

                </BootstrapTable>
            </div>
        );
    }
}



export default DistribucionTransporteList;