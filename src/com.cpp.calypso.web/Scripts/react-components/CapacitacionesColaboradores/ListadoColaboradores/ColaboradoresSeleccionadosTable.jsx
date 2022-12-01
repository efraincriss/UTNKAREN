import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";

export class ColaboradoresSeleccionadosTable extends React.Component {
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
                            width={"8%"}
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
                            width={"25%"}
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
                            dataField="Id"
                            width={"8%"}
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
                    className="btn btn-outline-danger mr-2"
                    onClick={() => this.onDeleteRow(row)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Quitar de la lista"
                >
                    <i className="fa fa-close"></i>
                </button>
            </div>
        )
    };

    onDeleteRow = (cell, row) => {
        this.props.eliminarColaborador(cell);
    }
}