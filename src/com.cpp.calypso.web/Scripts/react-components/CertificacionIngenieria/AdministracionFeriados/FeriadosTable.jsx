import React from "react"
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table"
import { ScrollPanel } from 'primereact-v3.3/scrollpanel';

export class FeriadosTable extends React.Component {
  constructor(props) {
    super(props)
    this.state = {}
  }

  render() {
    return (
      <ScrollPanel style={{ width: "100%", height: "470px" }}>
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
            width={"20%"}
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Descripción
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="FechaInicio"
            tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
            thStyle={{ whiteSpace: "normal", textAlign: "right" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
            dataFormat={this.formatoFechaCorta}
          >
            Fecha Inicial
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="FechaFin"
            tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
            thStyle={{ whiteSpace: "normal", textAlign: "right" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
            dataFormat={this.formatoFechaCorta}
          >
            Fecha Final
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="Anio"
            tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
            thStyle={{ whiteSpace: "normal", textAlign: "right" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Año
          </TableHeaderColumn>
        </BootstrapTable>
      </ScrollPanel>
    )
  }

  generarBotones = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-info mr-1 btn-sm"
          onClick={() => this.onEdit(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
        >
          <i className="fa fa-pencil"></i>
        </button>
        <button
          className="btn btn-outline-danger mr-1 btn-sm"
          onClick={() => this.props.mostrarConfirmacionParaEliminar(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
        >
          <i className="fa fa-trash"></i>
        </button>
      </div>
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
