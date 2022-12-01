import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Button, Tooltip } from "shards-react";
import dateFormatter from "../../Base/DateFormatter";

export default class DetalleConsumosExtemporaneosTable extends React.Component {
    constructor(props) {
        super(props)
        this.state = {

        }
    }


    render() {
        return (
            <div className="row" style={{ marginTop: '1em' }}>
                <div className="col">
                    <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                        <TableHeaderColumn
                            dataField='Secuencia'
                            width={'8%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            isKey>N°</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='ColaboradorNombre'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Apellidos Nombres</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='ColaboradorIdentificacion'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>N. Identificación</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Observaciones'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Observaciones</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='LiquidadoString'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Liquidado</TableHeaderColumn>


                        <TableHeaderColumn
                            dataField='Id'
                            dataFormat={this.generarBotones}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}></TableHeaderColumn>
                    </BootstrapTable>
                </div>
            </div>
        )
    }

    generarBotones = (cell, row) => {
        return (
            <div>
                <Button
                    theme="danger"
                    onClick={() => { if (window.confirm('Estás segur@ de querer eliminar este registro?')) this.props.eliminarDetalleConsumo(cell) }}
                >
                    Eliminar
                </Button>
            </div>
        )
    }

    dateFormat = (cell, row) => {
        if (cell === null) {
            return ''
        }
        return dateFormatter(cell);
    }

    toggle = () => {
        this.setState({
            open: !this.state.open
        });
    }
}