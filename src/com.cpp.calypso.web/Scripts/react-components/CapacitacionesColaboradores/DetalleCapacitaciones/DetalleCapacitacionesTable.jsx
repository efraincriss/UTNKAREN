import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import moment from 'moment';

export class DetalleCapacitacionesTable extends React.Component {
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
                            dataField="NombreCapacitacion"
                            width={"25%"}
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            Nombre Curso Certificado
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="Fecha"
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                            dataFormat={this.formatFecha}
                        >
                            Fecha
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="Horas"
                            tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "right" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            Horas
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="TipoCapacitacionNombre"
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            Tipo
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="Fuente"
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            Base Info
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="Id"
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
                    onClick={() => this.onEdit(row)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Editar"
                >
                    <i className="fa fa-pencil"></i>
                </button>
                <button
                    className="btn btn-outline-danger mr-2"
                    onClick={() => this.onDelete(cell)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Eliminar"
                >
                    <i className="fa fa-trash"></i>
                </button>
            </div>
        )
    };

    onEdit = (row) => {
        this.props.mostrarEdicionDeCapacitacion(row);
    }

    onDelete = (cell) => {
        this.props.eliminarCapacitacion(cell)
    }

    onRedirect = (cell) => {
        window.location.href = `/RRHH/Capacitacion/Detalle?colaboradorId=${cell}`;
    };

    index(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }

    formatFecha(cell, row) {
        if (row.Fecha != null) {
            return moment(cell).format('DD-MM-YYYY');
        } else {
            return null;
        }

    }
}