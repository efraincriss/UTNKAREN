import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import moment from 'moment';

export class ColaboradorTable extends React.Component {
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
                            width={"7%"}
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            isKey
                        >
                            N°
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="empleado_id_sap"
                            tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "right" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            No. Empleado
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="numero_identificacion"
                            tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "right" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            No. Identificación
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="nombres_apellidos"
                            width={"23%"}
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            Nombres
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="estado"
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            Estado
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="fecha_baja"
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                            dataFormat={this.formatFecha}
                        >
                            Fecha Baja
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
                    onClick={() => this.onRedirect(cell)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Ver Capacitaciones"
                >
                    <i className="fa fa-mortar-board"></i>
                </button>
                <button
                    className="btn btn-outline-primary mr-2"
                    onClick={() => this.props.agregarColaborador(row)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Agregar a impresion"
                >
                    <i className="fa fa-certificate"></i>
                </button>
                <button
                    className="btn btn-outline-primary mr-2"
                    onClick={() => this.imprimirCertificado(row)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Imprimir Certificado"
                >
                    <i className="fa fa-download"></i>
                </button>
            </div>
        )
    };

    onRedirect = (cell) => {
        window.location.href = `/RRHH/Capacitacion/Detalle?colaboradorId=${cell}`;
    };

    imprimirCertificado = (row) => {
        this.props.imprimirCertificadoDeUnColaborador(row)
    };

    index(cell, row, enumObject, index) {
        return (<div>{index+1}</div>) 
    }

    formatFecha(cell, row) {
        if (row.fecha_baja != null) {
            return moment(row.fecha_baja).format('DD-MM-YYYY');
        } else {
            return null;
        }

    }
}