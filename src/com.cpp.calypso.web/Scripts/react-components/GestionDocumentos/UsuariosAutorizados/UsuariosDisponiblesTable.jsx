import React from "react"
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table"
import config from "../../Base/Config"

export default class UsuariosDisponiblesTable extends React.Component {
  constructor(props) {
    super(props)
  }

  render() {
    return (
      <div className="row" style={{ marginTop: "1em" }}>
        <div className="col">
          <BootstrapTable
            data={this.props.usuarios}
            hover={true}
            pagination={true}
          >
            <TableHeaderColumn
              dataField="Id"
              width={"7%"}
              dataFormat={this.index}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              isKey
            >
              N°
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Id"
              dataFormat={this.apellidosNombres}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Usuario
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Cuenta"
              width={"25%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Cuenta
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Id"
              width={"15%"}
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
      <div>
        <button
          className="btn btn-outline-danger mr-1 btn-sm"
          onClick={() => this.props.mostrarConfirmacionParaEliminar(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
        >
          <i className="fa fa-trash"></i>
          {" Eliminar"} 
        </button>
      </div>
    )
  }

  index(cell, row, enumObject, index) {
    return <div>{index + 1}</div>
  }

  apellidosNombres(cell, row) {
    return (
      <div>
        {row.Apellidos} {row.Nombres}
      </div>
    )
  }
}
