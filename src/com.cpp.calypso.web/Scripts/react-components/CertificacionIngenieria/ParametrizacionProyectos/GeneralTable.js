import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import CurrencyFormat from 'react-currency-format';
export default class TarifasTable extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            visible: false
        };

        this.reject = this.reject.bind(this);

    }

    Secuencial(cell, row, enumObject, index) {
        return <div>{index + 1}</div>
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
        const options = {
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
                <BootstrapTable data={this.props.data} options={options} hover={true} pagination={true}>
                    <TableHeaderColumn dataField="any"

                        dataFormat={this.Secuencial}
                        width={"8%"}
                        tdStyle={{ whiteSpace: "normal", textAlign: "center", fontSize: "11px" }}
                        thStyle={{ whiteSpace: "normal", textAlign: "center", fontSize: "11px" }}
                    >
                        Nº
                    </TableHeaderColumn>

                    <TableHeaderColumn
                        width={'16%'}
                        dataField='codigo_contrato'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Contrato</TableHeaderColumn>


                    <TableHeaderColumn
                        width={'16%'}
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        dataField="codigo_proyecto"
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort>Código</TableHeaderColumn>



                    <TableHeaderColumn
                        dataField='nombre_proyecto'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Nombre Proyecto</TableHeaderColumn>


                    <TableHeaderColumn
                        isKey
                        dataField='Id'
                        width={'16%'}
                        dataFormat={this.generarBotones}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}>Opciones</TableHeaderColumn>
                </BootstrapTable>


            </div >
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
                <button className="btn btn-outline-success" data-toggle="tooltip" data-placement="left" title="Ver Parametrización"
                    onClick={() => this.props.mostrarForm(row)}>
                    <i className="fa fa-eye"></i>
                </button>


            </div>
        )
    }


}