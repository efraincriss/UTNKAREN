import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


export default class ColaboradorTable extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            //Subida de Archivo


        }

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
                            dataField='numero_identificacion'
                            width={'22%'}
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
                            dataField='tiene_derecho'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Tiene Derecho</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Id'
                            width={'13%'}
                            dataFormat={this.generarBotones}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}>
                        </TableHeaderColumn>
                    </BootstrapTable>

                </div>
            </div>
        )
    }
    activarservicio = (row) => {
        this.props.UpdateServicio(row.Id)
    }
    generarBotones = (cell, row) => {
        if (row.tiene_derecho === 'SI') {

            return (
                <div>

                    <button className="btn btn-outline-info" data-toggle="tooltip" data-placement="left" title="Crear Reserva Extemporánea"
                        onClick={() => this.props.mostrarUpload(cell)}>
                        <i className="fa fa-calendar-check-o"></i>
                    </button>{" "}
                    <button className="btn btn-outline-danger" data-toggle="tooltip" data-placement="left" title="Quitar Servicio Hospedaje"
                        onClick={
                            () => { if (window.confirm(`Se actualizará la asignación del Servicio de Hospedaje para el colaborador seleccionado, Desea continuar?`)) this.activarservicio(row); }
                        }>
                        <i className="fa fa-hand-scissors-o"></i>
                    </button>
                </div>

            )
        } else {
            return (
                <div>

                    <button className="btn btn-outline-info" data-toggle="tooltip" data-placement="left" title="Crear Reserva Extemporánea"
                        onClick={() => this.props.mostrarUpload(cell)}>
                        <i className="fa fa-calendar-check-o"></i>
                    </button>{" "}
                    <button className="btn btn-outline-success" data-toggle="tooltip" data-placement="left" title="Asignar Servicio Hospedaje"
                        onClick={
                            () => { if (window.confirm(`Se actualizará la asignación del Servicio de Hospedaje para el colaborador seleccionado, Desea continuar?`)) this.activarservicio(row); }
                        }>
                        <i className="fa fa-bed"></i>
                    </button>
                </div>

            )
        }
    }




}