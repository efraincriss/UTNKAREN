import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import moment from 'moment';

export class ActualizacionSueldoTable extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="row" style={{ marginTop: "1em" }}>
                <div className="col">
                    <BootstrapTable data={this.props.data} hover={true} pagination={true}>

                        <TableHeaderColumn
                            dataField="Id"
                            dataFormat={this.index}
                            width={"8%"}
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            isKey
                        >
                            N°
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="FechaCarga"
                            
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                            dataFormat={this.formatoFecha}
                        >
                            Fecha Carga
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="Observaciones"
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                            width={"25%"}
                        >
                            Observaciones
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="FechaInicio"
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                            dataFormat={this.formatoFechaCorta}
                        >
                            Fecha Inicio
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="FechaFin"
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                            dataFormat={this.formatoFechaCorta}
                        >
                            Fecha Fin
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="Id"
                            width={"18%"}
                            dataFormat={this.generarBotones}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                        ></TableHeaderColumn>
                    </BootstrapTable>
                </div>
            </div>
        );
    }

    generarBotones = (cell, row) => {
        return (
            <div>
                <button
                    className="btn btn-outline-info mr-2"
                    onClick={() => this.onWatch(row)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Ver Actualización"
                >
                    <i className="fa fa-eye"></i>
                </button>
            </div>
        )
    };

    onWatch = (row) => {
        console.log(row.DetalleActualizacionSueldos);
        this.props.mostrarDetalles(row.DetalleActualizacionSueldos)
    }

    index(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }

    formatoFecha = (cell, row) => {
        if (cell != null) {
            var informacion = cell.split("T")
            return informacion[0] + "  " + informacion[1].substring(0, 5);
        } else {
            return null;
        }

    }

    formatoFechaCorta = (cell, row) => {
        if (cell != null) {
            var informacion = cell.split("T")
            return informacion[0];
        } else {
            return null;
        }
    }
}