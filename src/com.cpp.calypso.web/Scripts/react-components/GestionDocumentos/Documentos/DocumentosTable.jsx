import React from "react"
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table"
import { CONTROLLER_SECCION, CONTROLLER_USUARIO_AUTORIZADO, MODULO_DOCUMENTOS } from "../../Base/Strings"

export class DocumentosTable extends React.Component {
  constructor(props) {
    super(props)
    this.state = {}
  }

  render() {
    return (
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
          dataField="Codigo"
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Código
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="Nombre"
          width={"25%"}
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Nombre Documento
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="TipoDocumentoNombre"
          tdStyle={{ whiteSpace: "normal", textAlign: "center+" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center+" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Tipo
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="DocumentoPadreCodigo"
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Pertenece A
        </TableHeaderColumn>

        <TableHeaderColumn
        width={"12%"}
          dataField="CantidadPaginas"
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Nro. Páginas
        </TableHeaderColumn>

        <TableHeaderColumn
        width={"12%"}
          dataField="EsImagen"
          dataFormat={this.esImagen}
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Es Imagen?
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
          onClick={() => this.onRedirectSecciones(cell)}
          data-toggle="tooltip"
          data-placement="top"
          title="Contenido"
        >
          <i className="fa fa-file"></i>
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

  onRedirect = contratoId => {
    window.location.href = `/${MODULO_DOCUMENTOS}/${CONTROLLER_USUARIO_AUTORIZADO}/Usuarios?contratoId=${contratoId}`
  }

  index(cell, row, enumObject, index) {
    return <div>{index + 1}</div>
  }

  esImagen(cell, row, enumObject, index) {
    return <div>{cell === true ? "SI" : "NO"}</div>
  }

  onRedirectSecciones = documentoId => {
    window.location.href = `/${MODULO_DOCUMENTOS}/${CONTROLLER_SECCION}/Secciones?documentoId=${documentoId}`
  }
}
