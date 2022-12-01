import { Button } from "primereact-v2/button"
import React, { Fragment } from "react"
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table"

export default class ParametroSistemaTable extends React.Component {
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
              isKey
            >
              No.
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Codigo"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              dataFormat={this.formatoFechaCorta}
            >
              Código
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Nombre"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              dataFormat={this.formatoFechaCorta}
            >
              Nombre
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Descripcion"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Descripción
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Valor"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Valor
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Id"
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
      <Fragment>
        <Button
          label=""
          className="p-button-outlined"
          onClick={() => this.props.mostrarFormulario(row)}
          icon="pi pi-pencil"
        />
      </Fragment>
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
