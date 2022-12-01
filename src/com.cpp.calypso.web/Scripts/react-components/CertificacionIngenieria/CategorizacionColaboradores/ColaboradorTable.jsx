import { Button } from "primereact-v2/button"
import React from "react"
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table"
export default class ColaboradorTable extends React.Component {
  constructor(props) {
    super(props)
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
              dataFormat={this.index}
            >
              No.
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Nombres"
              width={"25%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "left" }}
              thStyle={{ whiteSpace: "normal", textAlign: "left" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Apellidos Nombres
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Identificacion"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              isKey
            >
              No. Identificacion
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Externo"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Externo?
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="FechaIngreso"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              dataFormat={this.formatoFechaCorta}
            >
              Fecha Ingreso
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Estado"
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
    )
  }

  generarBotones = (cell, row) => {
    return (
      <Button
        label=""
        className="p-button-outlined"
        onClick={() => this.props.verDetallesColaborador(row)}
        icon="pi pi-users"
      />
    )
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
