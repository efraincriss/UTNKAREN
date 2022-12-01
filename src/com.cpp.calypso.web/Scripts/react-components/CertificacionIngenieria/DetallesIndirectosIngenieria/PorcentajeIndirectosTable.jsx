import React, { Fragment } from "react"
import { Button } from "primereact-v2/button"
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table"

export class PorcentajeIndirectosTable extends React.Component {
  constructor(props) {
    super(props)
    this.state = {}
  }

  render() {
    return (
      <BootstrapTable data={this.props.data} hover={true} pagination={true}>
        <TableHeaderColumn
          dataField="ContratoNombre"
          width={"40%"}
          tdStyle={{ whiteSpace: "normal", textAlign: "left" }}
          thStyle={{ whiteSpace: "normal", textAlign: "left" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
          dataFormat={this.formatoFechaCorta}
          isKey
        >
          Contrato
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="PorcentajeIndirecto"
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
         
        >
          Porcentaje
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="Horas"
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Horas
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="Id"
          dataFormat={this.generarBotones}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
        ></TableHeaderColumn>
      </BootstrapTable>
    )
  }

  generarBotones = (cell, row) => {
    return (
      <Fragment>
        <Button
          label=""
          className="p-button-outlined"
          onClick={() => this.onEdit(row)}
          icon="pi pi-pencil"
        />
        <Button
          style={{ marginLeft: "0.2em" }}
          label=""
          className="p-button-danger p-button-outlined"
          onClick={() => this.props.mostrarConfirmacionParaEliminar(row)}
          icon="pi pi-ban"
        />
      </Fragment>
    )
  }

  onEdit = (row) => {
    this.props.mostrarFormularioPocentajes(row)
  }
}
