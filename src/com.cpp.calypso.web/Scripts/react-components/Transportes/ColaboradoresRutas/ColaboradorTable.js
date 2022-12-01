import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class ColaboradorTable extends React.Component {
    constructor(props) {
        super(props)
    }


    render() {
        return (
            <div className="row" style={{ marginTop: '1em' }}>
                <div className="col">
                    <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                        <TableHeaderColumn
                            dataField='secuencial'
                            width={'8%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            isKey>N°</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='tipo_identificacion_nombre'
                            width={'25%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Tipo de Identificación</TableHeaderColumn>
                        <TableHeaderColumn
                            dataField='numero_identificacion'
                            width={'25%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>No. Identificación</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='nombres_apellidos'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Apellidos Nombres</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='estado'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Estado</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='numero_identificacion'
                            width={'10%'}
                            dataFormat={this.generarBotones}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}>Opciones
                        </TableHeaderColumn>
                    </BootstrapTable>
                </div>
            </div>
        )
    }

    generarBotones = (cell, row) => {
        return (
            <div>
                <button className="btn btn-outline-info" data-toggle="tooltip" data-placement="left" title="Ir a Asignación"
                    onClick={
                        () => { this.submit(cell) }
                    }>
                    <i className="fa fa-eye"></i>
                </button>

            </div>

        )
    }

    submit = (cell) => {
        window.location.href = "/Transporte/ColaboradorRuta/Index/" + cell;
    }
}