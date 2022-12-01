import React, { Component } from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../Base/BaseWrapperApi";
import config from '../Base/Config';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

class ProveedorHospedajeTable extends Component {
    constructor(props) {
        super(props)
        this.state = {

        }
    }


    render() {
        return (
            <div className="row">
                <div className="col">
                    <BootstrapTable id="table-excel" data={this.props.data} hover={true} pagination={true}>
                        <TableHeaderColumn
                            dataField='secuencial'
                            width={'8%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center'  }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            isKey>N°</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='identificacion'
                            tdStyle={{ whiteSpace: 'normal'}}
                            thStyle={{ whiteSpace: 'normal'}}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>No. de Identificación</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='razon_social'
                            tdStyle={{ whiteSpace: 'normal'}}
                            thStyle={{ whiteSpace: 'normal'}}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Proveedor</TableHeaderColumn>

                        <TableHeaderColumn
                            tdStyle={{ whiteSpace: 'normal'}}
                            thStyle={{ whiteSpace: 'normal'}}
                            dataField="tipo_proveedor_nombre" 
                            filter={{ type: 'TextFilter', delay: 500 }} 
                            dataSort>Tipo</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='estado_nombre'
                            dataFormat={this.NumberFormat}
                            tdStyle={{ whiteSpace: 'normal'}}
                            thStyle={{ whiteSpace: 'normal'}}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Estado</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Id'
                            width={'10%'}
                            dataFormat={this.generarBotones}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}>Opciones</TableHeaderColumn>
                    </BootstrapTable>
                </div>
            </div>
        )
    }


    generarBotones = (cell, row) => {
        return (
            <button className="btn btn-outline-success" data-toggle="tooltip" data-placement="left" title="Ver"
                onClick={() => this.onDetailItem(row)}>
                <i className="fa fa-eye"></i>
            </button>
        )
    }

    onDetailItem = (entity) => {
        return (
           window.location.href = `${config.appUrl}/proveedor/Habitacion/Details/${entity.Id}`
       );
   }
}

const Container = Wrapper(ProveedorHospedajeTable,
    `/proveedor/Proveedor/GetProveedoresHospedajeApi`,
    {});

ReactDOM.render(
    <Container />,
    document.getElementById('proveedores_table_component')
);

