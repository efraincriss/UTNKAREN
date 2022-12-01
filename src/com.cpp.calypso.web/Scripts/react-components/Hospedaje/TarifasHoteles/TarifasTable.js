import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import CurrencyFormat from 'react-currency-format';


export default class TarifasTable extends React.Component {
    constructor(props) {
        super(props)
    }


    render() {
        return (
            <div>
                <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                    <TableHeaderColumn
                        dataField='secuencial'
                        width={'8%'}
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        isKey>N°</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='tipo_habitacion_nombre'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Tipo Habitación</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='capacidad'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Capacidad</TableHeaderColumn>

                    <TableHeaderColumn
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                        dataField="costo_persona"
                        dataFormat={this.valorFormat}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort>Valor por Persona</TableHeaderColumn>

                    <TableHeaderColumn
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                        dataFormat={this.costoTotal}
                        dataField='total'
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort>Total</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='estado_nombre'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Estado</TableHeaderColumn>


                    <TableHeaderColumn
                        dataField='Id'
                        width={'16%'}
                        dataFormat={this.generarBotones}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}>Opciones</TableHeaderColumn>
                </BootstrapTable>
            </div>
        )
    }


    costoTotal = (cell, row) => {
        return <CurrencyFormat value={cell} displayType={'text'} thousandSeparator={true} prefix={'$'} />
    }

    valorFormat = (cell, row) => {
        return <CurrencyFormat value={cell} displayType={'text'} thousandSeparator={true} prefix={'$'} />
    }

    generarBotones = (cell, row) => {
        return (
            <div>
                <button className="btn btn-outline-info" data-toggle="tooltip" data-placement="left" title="Editar"
                    onClick={() => this.props.detalleTarifa(cell)}>
                    <i className="fa fa-pencil"></i>
                </button>
                {row.estado === true ?
                    <button className="btn btn-outline-danger" style={{ marginLeft: '0.3em' }} data-toggle="tooltip" data-placement="left" title="Inactivar Tarifa"
                        onClick={() => { if (window.confirm(`Esta acción inactivará la tarifa ${row.Id}, ¿Desea continuar?`)) this.props.deleteTarifa(cell) }}>
                        <i className="fa fa-minus-square"></i>
                    </button>
                    :

                    <button className="btn btn-outline-success" style={{ marginLeft: '0.3em' }} data-toggle="tooltip" data-placement="left" title="Activar Tarifa"
                        onClick={() => { if (window.confirm(`Esta acción activará la tarifa ${row.Id}, ¿Desea continuar?`)) this.props.activarTarifa(cell) }}>
                        <i className="fa fa-check-square"></i>
                    </button>
                }

            </div>
        )
    }

    
}