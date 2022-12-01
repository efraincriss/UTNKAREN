import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Button } from "primereact-v2/button"
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
                            dataField='Id'
                            width={'8%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            isKey>No.</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="Identificacion"
                            width={"12%"}
                            tdStyle={{ whiteSpace: "normal" }}
                            thStyle={{ whiteSpace: "normal" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                            thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                            dataSort={true}
                        >
                            Identificación
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField="NombreCompleto"
                            tdStyle={{ whiteSpace: "normal" }}
                            thStyle={{ whiteSpace: "normal" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                            thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                            dataSort={true}
                        >
                            Apellidos y Nombres
                        </TableHeaderColumn>
                        <TableHeaderColumn
                            width={"10%"}
                            dataField="TipoUsuario"
                            tdStyle={{ whiteSpace: "normal" }}
                            thStyle={{ whiteSpace: "normal" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                            thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                            dataSort={true}
                        >
                            Tipo Usuario
                        </TableHeaderColumn>
                        <TableHeaderColumn
                            dataField="Area"
                            tdStyle={{ whiteSpace: "normal" }}
                            thStyle={{ whiteSpace: "normal" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                            thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                            dataSort={true}
                        >
                            Área
                        </TableHeaderColumn>
                        <TableHeaderColumn
                            dataField="Cargo"
                            tdStyle={{ whiteSpace: "normal" }}
                            thStyle={{ whiteSpace: "normal" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                            thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                            dataSort={true}
                        >
                            Cargo
                        </TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Id'
                            width={'8%'}
                            dataFormat={this.generarBotones}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}></TableHeaderColumn>
                    </BootstrapTable>
                </div>
            </div>
        )
    }

    generarBotones = (cell, row) => {
        return (
            <Button
                label=""
                className="p-button-outlined"
                onClick={() => this.props.seleccionarColaborador(row)}
                icon="pi pi-user"
            />
        )
    }


}