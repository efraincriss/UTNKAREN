import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import dateFormatter from "../../Base/DateFormatter";
export default class ReservasTable extends React.Component {
    constructor(props) {
        super(props)
    }

    Secuencial(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }
    render() {
        return (
            <div className="row" style={{ marginTop: '1em' }}>
                <div className="col">
                    <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                       {/*  <TableHeaderColumn
                            dataField='Id'
                            width={'8%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            isKey>N°</TableHeaderColumn>
                            */}
                        <TableHeaderColumn
                            dataField="any"
                            dataFormat={this.Secuencial}
                            width={"8%"}
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                        >
                            Nº
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='colaborador_nombres'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Colaborador</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='colaborador_grupo_personal'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Colaborador Grupo</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='proveedor_razon_social'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Proveedor</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='numero_habitacion'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>N° Habitación</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='tipo_habitacion_nombre'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Tipo</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='fecha_desde'
                            dataFormat={this.dateFormat}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Fecha Desde</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='fecha_hasta'
                            dataFormat={this.dateFormat}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Fecha Hasta</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Id'
                            isKey
                            width={'15%'}
                            dataFormat={this.generarBotones}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center', fontSize: '10px' }}></TableHeaderColumn>
                    </BootstrapTable>
                </div>
            </div>
        )
    }

    generarBotones = (cell, row) => {
        return (
            <div>
                <button className="btn btn-outline-success" data-toggle="tooltip" data-placement="left" title="Ver Detalles"
                    style={{ marginRight: '0.3em' }}
                    onClick={() => this.props.consultarDetalles(cell)}>
                    <i className="fa fa-eye"></i>
                </button>
                <button className="btn btn-outline-info" data-toggle="tooltip" data-placement="left" title="Editar Reserva"
                    style={{ marginRight: '0.3em' }}
                    onClick={() => this.props.seleccionarEditarReserva(row)}>
                    <i className="fa fa-pencil"></i>
                </button>
                <button className="btn btn-outline-danger" data-toggle="tooltip" data-placement="left" title="Eliminar Reserva"
                    onClick={() => { if (window.confirm(`Esta seguro de eliminar la reserva?`)) this.props.eliminarReserva(cell) }}>
                    <i className="fa fa-trash"></i>
                </button>
            </div>
        )
    }

    dateFormat = (cell, row) => {
        return dateFormatter(cell);
    }
}