import React, { Fragment } from "react"
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table"
import { ScrollPanel } from 'primereact-v3.3/scrollpanel';
import { Button } from "primereact-v2/button"

export class PlanificacionTimesheetTable extends React.Component {
  constructor(props) {
    super(props)
    this.state = {}
  }

  render() {
    return (
      <ScrollPanel style={{ width: "100%" }}>
        <BootstrapTable data={this.props.data} hover={true} pagination={true}>
          <TableHeaderColumn
            isKey={true}
            dataField="Id"
            width={"8%"}
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            dataSort={true}
            dataFormat={this.index}
          >
            Nro.
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="Descripcion"
            width={"35%"}
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Descripción
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="Fecha"
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
            dataFormat={this.formatoFechaCorta}
          >
            Fecha
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="Anio"
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Año
          </TableHeaderColumn>
          <TableHeaderColumn
              dataField="Id"
              dataFormat={this.generarBotones}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            ></TableHeaderColumn>
        </BootstrapTable>
      </ScrollPanel>
    )
  }

  generarBotones = (cell, row) => {
    return (
      <Fragment>
        <Button
          label=""
          className="p-button-outlined"
          onClick={() => this.props.editarPlanificacion(row)}
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
    this.props.mostrarFormulario(row)
  }

  index(cell, row, enumObject, index) {
    return <div>{index + 1}</div>
  }

  formatoFechaCorta = (cell, row) => {
    if (cell != null) {
      var informacion = cell.split("T")
      return informacion[0]
    } else {
      return null
    }
  }
}
