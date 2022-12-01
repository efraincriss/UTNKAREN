import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import dateFormatter from "../../Base/DateFormatter";

export default class DetallesReservaTable extends React.Component {
    constructor(props) {
        super(props)
    }


    render() {
        return (
            <div className="row" style={{ marginTop: '1em' }}>
                <div className="col" style={{maxHeight: '570px', overflowX: 'overlay'}}>
                    <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                        <TableHeaderColumn
                            dataField='Id'
                            width={'8%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center'}}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center'}}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            isKey>N°</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='fecha_reserva'
                            dataFormat={this.dateFormat}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center'}}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center'}}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Fecha Reserva</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='fecha_consumo'
                            dataFormat={this.dateFormat}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center'}}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center'}}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Fecha Consumo</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='consumido_nombre'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center'}}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center'}}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Consumido</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='facturado_nombre'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center'}}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center'}}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Facturado</TableHeaderColumn>
  
                    </BootstrapTable>
                </div>
            </div>
        )
    }

    dateFormat = (cell, row) => {
        if(cell === null){
            return "";
        }
        return dateFormatter(cell);
    }
}