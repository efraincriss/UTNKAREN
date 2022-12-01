import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class VehiculosTable extends React.Component {
    constructor(props) {
        super(props)
    }


    render() {
        return (
            <div>
                <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                    <TableHeaderColumn
                        dataField='Secuencial'
                        width={'8%'}
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        isKey>N°</TableHeaderColumn>
                    <TableHeaderColumn
                        dataField='Codigo'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Código</TableHeaderColumn>
                    <TableHeaderColumn
                        dataField='CodigoEquipoInventario'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Equipo Inventario</TableHeaderColumn>
                    <TableHeaderColumn
                        dataField='ProveedorRazonSocial'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Proveedor</TableHeaderColumn>



                    <TableHeaderColumn
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        dataField="NumeroPlaca"
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort>Placa</TableHeaderColumn>

                    <TableHeaderColumn
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        dataField="TipoVehiculo"
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort>Tipo Vehículo</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='Capacidad'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Capacidad</TableHeaderColumn>
                    <TableHeaderColumn
                        dataField='FechaVencimiento'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Fecha V. Matrícula</TableHeaderColumn>
                    <TableHeaderColumn
                        dataField='EstadoNombre'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Estado</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='Id'
                        width={'12%'}
                        dataFormat={this.generarBotones}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}>Opciones</TableHeaderColumn>
                </BootstrapTable>
            </div>
        )
    }

    generarBotones = (cell, row) => {
        return (
            <div>
                <button className="btn btn-outline-info"
                    onClick={() => this.props.mostrarForm(row, true)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Editar Vehículo">
                    <i className="fa fa-pencil"></i>
                </button>
                <button className="btn btn-outline-danger" style={{ marginLeft: '0.3em' }}
                    onClick={() => { if (window.confirm(`Esta acción eliminará el vehículo con código ${row.Codigo}, ¿Desea continuar?`)) this.props.onDelete(cell) }}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Eliminar Vehículo">
                    <i className="fa fa-trash"></i>
                </button>
            </div>
        )
    }
}