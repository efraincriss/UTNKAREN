import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import CurrencyFormat from 'react-currency-format';
import { ConfirmPopup } from 'primereact_/confirmpopup';

export default class TarifasTable extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            visible: false
        };

        this.reject = this.reject.bind(this);

    }


    reject() {
        this.setState({ visible: false })
    }
    renderShowsTotal(start, to, total) {
        return (
            <p style={{ color: "blue" }}>
                De {start} hasta {to}, Total Registros {total}
            </p>
        );
    }
    render() {
        const optionsColaboradores = {
            sizePerPage: 10,
            noDataText: "No existen datos registrados",
            sizePerPageList: [
                {
                    text: "10",
                    value: 10,
                },
                {
                    text: "20",
                    value: 20,
                },
            ],
            paginationShowsTotal: this.renderShowsTotal, // Accept bool or function
        };
        return (
            <div>
                <BootstrapTable data={this.props.data} options={optionsColaboradores} hover={true} pagination={true}>
                    <TableHeaderColumn
                        dataField='secuencial'
                        width={'8%'}
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        isKey>N°</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='tipo_servicio_nombre'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Tipo Servicio</TableHeaderColumn>


                    <TableHeaderColumn
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                        dataField="valor_servicio"
                        dataFormat={this.valorFormat}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort>Valor Servicio</TableHeaderColumn>



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
                    onClick={() => this.props.detalleTarifa(row)}>
                    <i className="fa fa-pencil"></i>
                </button>
                {row.estado === true ?
                    <button className="btn btn-outline-warning" style={{ marginLeft: '0.3em' }} data-toggle="tooltip" data-placement="left" title="Inactivar Tarifa"
                        onClick={() => { if (window.confirm(`Esta acción inactivará la tarifa, ¿Desea continuar?`)) this.props.deleteTarifa(cell) }}>
                        <i className="fa fa-minus-square"></i>
                    </button>
                    :

                    <button className="btn btn-outline-success" style={{ marginLeft: '0.3em' }} data-toggle="tooltip" data-placement="left" title="Activar Tarifa"
                        onClick={() => { if (window.confirm(`Esta acción activará la tarifa, ¿Desea continuar?`)) this.props.activarTarifa(cell) }}>
                        <i className="fa fa-check-square"></i>
                    </button>
                }
               <button className="btn btn-outline-danger" style={{ marginLeft: '0.3em' }} data-toggle="tooltip" data-placement="left" title="Eliminar Tarifa"
                        onClick={() => { if (window.confirm(`Esta acción eliminará la tarifa, ¿Desea continuar?`)) this.props.eliminarTarifa(cell) }}>
                        <i className="fa fa-trash"></i>
                    </button>

            </div>
        )
    }


}