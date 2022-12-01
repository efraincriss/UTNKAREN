import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import moment from 'moment';

export class DetalleActualizacionSueldoTable extends React.Component {
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
                            dataField="NombreCategoriaEncargado"
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            Nombre Categoría
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="ValorSueldo"
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            Valor Sueldo
                        </TableHeaderColumn>

                    </BootstrapTable>
                </div>
            </div>
        );
    }

    
    

    index(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }

    formatoFecha(cell, row) {
        if (row.fecha_baja != null) {
            return moment(row.fecha_baja).format('DD-MM-YYYY');
        } else {
            return null;
        }

    }
}