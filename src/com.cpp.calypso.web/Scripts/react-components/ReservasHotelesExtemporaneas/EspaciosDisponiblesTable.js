import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class EspaciosDisponiblesTable extends React.Component {
    constructor(props) {
        super(props)

        this.state = {

        }
    }



    render() {
        return (
            <div className="row">
                <div className="col">
                    <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                        <TableHeaderColumn
                            dataField='Id'
                            width={'8%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            isKey>N°</TableHeaderColumn>


                        <TableHeaderColumn
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            dataField="proveedor_razon_social"
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort>Proveedor</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='numero_habitacion'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>No. Habitación</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='tipo_habitacion_nombre'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Tipo Habitación</TableHeaderColumn>

                        <TableHeaderColumn
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            dataField="codigo_espacio"
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort>Espacio</TableHeaderColumn>


                        <TableHeaderColumn
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            dataField="capacidad_habitacion"
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort>Capacidad</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Id'
                            width={'8%'}
                            dataFormat={this.generarBotones}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}>
                            </TableHeaderColumn>
                    </BootstrapTable>
                </div>
            </div>
        )
    }


    generarBotones = (cell, row) => {
        return (
            <button className="btn btn-outline-info" data-toggle="tooltip" data-placement="left" title="Seleccionar colaborador"
                onClick={() => this.props.seleccionarEspacio(cell)}
                data-toggle="tooltip"
                data-placement="top"
                title="Seleccionar Colaborador">
                <i className="fa fa-user"></i>
            </button>
        )
    }
}