import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";

export class ResumenCapacitacionTable extends React.Component {
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
                            width={"12%"}
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            isKey
                        >
                            N°
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="NombreCapacitacion"
                            width={"40%"}
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            Nombre Curso Certificado
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

                    </BootstrapTable>
                </div>
            </div>
        );
    }
    index(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }

    
}