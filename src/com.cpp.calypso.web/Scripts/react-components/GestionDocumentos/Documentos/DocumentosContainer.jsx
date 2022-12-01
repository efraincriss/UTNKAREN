import React from "react"
import ReactDOM from "react-dom"

import Wrapper from "../../Base/BaseWrapper"
import http from "../../Base/HttpService"
import {
  CONTROLLER_CARPETA,
  MODULO_DOCUMENTOS,
  FRASE_DOCUMENTO_ELIMINADO,
  FRASE_ERROR_GLOBAL,
  CONTROLLER_DOCUMENTO,
} from "../../Base/Strings"
import { Dialog } from "primereact-v2/components/dialog/Dialog"
import { Fragment } from "react"
import { Button } from "primereact-v2/button"
import { DocumentosTable } from "./DocumentosTable.jsx"
import { DocumentoForm } from "./DocumentoForm.jsx"
import { CabeceraDocumento } from "./CabeceraDocumento.jsx"

class DocumentosContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      contratoId: 0,
      carpeta: [],
      catalogoTipoDocumento: [],
      documentoSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: 12
    }
  }

  componentWillMount() {
    this.setState(
      {
        contratoId: this.props.getParameterByName("contratoId"),
      },
      () => console.log(this.state.contratoId)
    )
  }

  componentDidMount() {
    this.loadData()
  }

  render() {
    return (
      <div>
        <CabeceraDocumento carpeta={this.state.carpeta} />

        <div className="row">
          <div style={{ width: "100%" }}>
            <div className="card">
              <div className="card-body">
                <div className="row" style={{ marginTop: "1em" }}>
                  <div className="col" align="right">
                    <button
                      className="btn btn-outline-primary"
                      onClick={() => this.mostrarFormulario({})}
                    >
                      Nuevo Documento
                    </button>
                  </div>
                </div>

                <hr />

                <div className="row">
                  <div className="col">
                    <DocumentosTable
                      data={this.state.documentos}
                      mostrarFormulario={this.mostrarFormulario}
                      mostrarConfirmacionParaEliminar={
                        this.mostrarConfirmacionParaEliminar
                      }
                    />
                  </div>
                </div>

                <Dialog
                  header="Gestión de Contratos"
                  modal={true}
                  visible={this.state.mostrarFormulario}
                  style={{ width: "750px" }}
                  onHide={this.onHideFormulario}
                >
                  <DocumentoForm
                    documento={this.state.documentoSeleccionado}
                    actualizarDocumentoSeleccionado={
                      this.actualizarDocumentoSeleccionado
                    }
                    onHideFormulario={this.onHideFormulario}
                    showSuccess={this.props.showSuccess}
                    showWarn={this.props.showWarn}
                    blockScreen={this.props.blockScreen}
                    unlockScreen={this.props.unlockScreen}
                    catalogoTipoDocumento={this.state.catalogoTipoDocumento}
                    contratoId={this.state.contratoId}
                    documentosAnexos = {this.state.documentosAnexos}
                    key={this.state.keyForm}
                  />
                </Dialog>

                <Dialog
                  header="Confirmación"
                  visible={this.state.mostrarConfirmacion}
                  modal
                  style={{ width: "500px" }}
                  footer={this.construirBotonesDeConfirmacion()}
                  onHide={this.onHideFormulario}
                >
                  <div className="confirmation-content">
                    <div className="p-12">
                      <i
                        className="pi pi-exclamation-triangle p-mr-3"
                        style={{ fontSize: "2rem" }}
                      />
                      <p>
                        {" "}
                        ¿Está seguro de eliminar el documento{" "}
                        {this.state.documentoSeleccionado.Nombre},
                        <b>
                          recuerde que eliminará todas las secciones ingresadas
                          en el documento
                        </b>
                        , si desea continuar presione ELIMINAR, caso contrario{" "}
                        <b>Cancelar</b>
                      </p>
                    </div>
                  </div>
                </Dialog>
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }

  loadData = () => {
    this.props.blockScreen()
    var self = this
    Promise.all([
      this.obtenerCarpeta(),
      this.obtenerTiposDocumento(),
      this.obtenerDocumentosCarpeta(),
      this.obtenerAnexos()
    ])
      .then(function ([carpeta, tiposDocumento, documentos, documentosAnexos]) {
        self.setState({
          carpeta: carpeta.data.result,
          catalogoTipoDocumento: self.props.buildDropdown(
            tiposDocumento.data,
            "nombre",
            "Id"
          ),
          documentosAnexos: self.props.buildDropdown(
            documentosAnexos.data,
            "Nombre",
            "Id"
          ),
          documentos: documentos.data.result,
        })
        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  eliminarDocumento = () => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_DOCUMENTO}/EliminarDocumento/${this.state.documentoSeleccionado.Id}`

    http
      .delete(url, {})
      .then((response) => {
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_DOCUMENTO_ELIMINADO)
          this.onHideFormulario(true)
        } else {
          var message = data.result
          this.props.showWarn(message)
          this.props.unlockScreen()
        }
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(FRASE_ERROR_GLOBAL)
        this.props.unlockScreen()
      })
  }

  mostrarFormulario = (documento) => {
    this.setState({
      documentoSeleccionado: documento,
      mostrarFormulario: true,
    })
  }

  mostrarConfirmacionParaEliminar = (documento) => {
    this.setState({
      documentoSeleccionado: documento,
      mostrarConfirmacion: true,
    })
  }

  onHideFormulario = (recargar = false) => {
    this.setState({
      mostrarFormulario: false,
      documentoSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: Math.random()
    })
    if (recargar) {
      this.loadData()
    }
  }

  obtenerCarpeta = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_CARPETA}/ObtenerCarpeta/${this.state.contratoId}`
    return http.get(url)
  }

  obtenerAnexos = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_DOCUMENTO}/ObtenerDocumentosTipoAnexo/${this.state.contratoId}`
    return http.get(url)
  }

  obtenerDocumentosCarpeta = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_DOCUMENTO}/ObtenerDocumentosDeCarpeta/${this.state.contratoId}`
    return http.get(url)
  }

  obtenerTiposDocumento = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_DOCUMENTO}/ObtenerCatalogoTipoDocumento`
    return http.get(url)
  }

  actualizarDocumentoSeleccionado = (documento) => {
    this.setState({ documentoSeleccionado: documento })
  }

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarDocumento()}
          icon="pi pi-save"
        />
        <Button
          style={{ marginLeft: "0.4em" }}
          label="Cancelar"
          className="p-button-outlined"
          onClick={() => this.onHideFormulario()}
          icon="pi pi-ban"
        />
      </Fragment>
    )
  }
}

const Container = Wrapper(DocumentosContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("gestion_documentos_contratos")
)
