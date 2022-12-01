import React from "react"

import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table"
import {
  CONTROLLER_DOCUMENTO,
  CONTROLLER_USUARIO_AUTORIZADO,
  MODULO_DOCUMENTOS,
} from "../../Base/Strings"

export class CarpetaTable extends React.Component {
  constructor(props) {
    super(props)
    this.state = {}
  }

  render() {
    return (
      <BootstrapTable data={this.props.data} hover={true} pagination={true}>
        <TableHeaderColumn
          isKey={true}
          dataField="Codigo"
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Código
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="NombreCorto"
          width={"25%"}
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
          dataFormat={this.formatFecha}
        >
          Nombre
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="EstadoNombre"
          tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
          thStyle={{ whiteSpace: "normal", textAlign: "right" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Estado
        </TableHeaderColumn>

        <TableHeaderColumn
          width={"12%"}
          dataField="NumeroDocumentos"
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Nro. Documentos
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
          className="btn btn-outline-primary mr-1 btn-sm"
          onClick={() => this.onRedirectDocumentos(cell)}
          data-toggle="tooltip"
          data-placement="top"
          title="Documentos"
        >
          <i className="fa fa-file"></i>
        </button>
        <button
          className="btn btn-outline-warning mr-1 btn-sm"
          onClick={() => this.onRedirect(cell)}
          data-toggle="tooltip"
          data-placement="top"
          title="Usuarios"
        >
          <i className="fa fa-users"></i>
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

  onRedirect = (contratoId) => {
    window.location.href = `/${MODULO_DOCUMENTOS}/${CONTROLLER_USUARIO_AUTORIZADO}/Usuarios?contratoId=${contratoId}`
  }

  onRedirectDocumentos = (contratoId) => {
    window.location.href = `/${MODULO_DOCUMENTOS}/${CONTROLLER_DOCUMENTO}/Detalles?contratoId=${contratoId}`
  }

  index(cell, row, enumObject, index) {
    return <div>{index + 1}</div>
  }
}
