import React from "react"
import ReactDOM from "react-dom"

import Wrapper from "../../Base/BaseWrapper"
import http from "../../Base/HttpService"
import {
  MODULO_DOCUMENTOS,
  CONTROLLER_USUARIO_AUTORIZADO,
  CONTROLLER_CARPETA,
  FRASE_USUARIO_DESASIGNADO,
  FRASE_ERROR_GLOBAL
} from "../../Base/Strings"
import CabeceraUsuariosAutorizados from "./CabeceraUsuariosAutorizados.jsx"
import { Button } from "primereact-v2/button"
import { UsuariosAutorizadoMultiSelect } from "./UsuarioAutorizadoMultiSelect.jsx"
import UsuariosDisponiblesTable from "./UsuariosDisponiblesTable.jsx"
import { Dialog } from "primereact-v2/components/dialog/Dialog"
import { Fragment } from "react"

class UsuariosAutorizadosContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      contratoId: 0,
      usuariosAsignados: [],
      usuariosDisponibles: [],
      carpeta: {},
      usuarioSeleccionado: {},
      mostrarConfirmacion: false,
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
    this.consultarDatos()
  }

  render() {
    return (
      <div>
        <CabeceraUsuariosAutorizados carpeta={this.state.carpeta} />

        <div className="row">
          <div style={{ width: "100%" }}>
            <div className="card">
              <div className="card-body">
                <UsuariosAutorizadoMultiSelect
                  usuariosDisponibles={this.state.usuariosDisponibles}
                  consultarDatos={this.consultarDatos}
                  showSuccess={this.props.showSuccess}
                  showWarn={this.props.showWarn}
                  blockScreen={this.props.blockScreen}
                  unlockScreen={this.props.unlockScreen}
                  contratoId = {this.state.contratoId}
                />
                <hr />

                <UsuariosDisponiblesTable
                  usuarios={this.state.usuariosAsignados}
                  mostrarConfirmacionParaEliminar={this.mostrarConfirmacionParaEliminar}
                />
              </div>
            </div>
          </div>
        </div>
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
                  ¿Está seguro de eliminar el usuario asignado
                  <b> {this.state.usuarioSeleccionado.Cuenta}</b>? Si desea
                  continuar presione ELIMINAR, caso contrario CANCELAR
                </p>
              </div>
            </div>
          </Dialog>
      </div>
    )
  }

  obtenerUsuarioAsignados = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_USUARIO_AUTORIZADO}/ObtenerUsuariosAsignados/${this.state.contratoId}`
    return http.get(url)
  }

  obtenerColaboradorDisponibles = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_USUARIO_AUTORIZADO}/ObtenerUsuariosDisponibles/${this.state.contratoId}`
    return http.get(url)
  }

  obtenerCarpeta = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_CARPETA}/ObtenerCarpeta/${this.state.contratoId}`
    return http.get(url)
  }

  consultarDatos = () => {
    this.props.blockScreen()
    var self = this
    Promise.all([
      this.obtenerUsuarioAsignados(),
      this.obtenerColaboradorDisponibles(),
      this.obtenerCarpeta(),
    ])
      .then(function ([usuariosAsignados, usuariosDisponibles, carpeta]) {
        var UsuariosAutorizados = usuariosDisponibles.data.result.map((item) => {
          return { label: item.Cuenta + " - "+item.Apellidos+ " "+item.Nombres, dataKey: item.Id, value: item.Id };
        });
        self.setState(
          {
            usuariosDisponibles: UsuariosAutorizados,
            usuariosAsignados: usuariosAsignados.data.result,
            carpeta: carpeta.data.result,
          },
          self.props.unlockScreen
        )
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  eliminarUsuarioAsignado = () => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_USUARIO_AUTORIZADO}/EliminarUsuarioAutorizado?usuarioId=${this.state.usuarioSeleccionado.Id}&carpetaId=${this.state.contratoId}`
    let body = {
      usuarioId: this.state.usuarioSeleccionado.Id,
      carpetaId: this.state.contratoId
    }

    http
      .delete(url, body)
      .then((response) => {
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_USUARIO_DESASIGNADO)
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

  mostrarConfirmacionParaEliminar = (usuario) => {
    this.setState({
      usuarioSeleccionado: usuario,
      mostrarConfirmacion: true,
    })
  }

  onHideFormulario = (recargar = false) => {
    this.setState({
      usuarioSeleccionado: {},
      mostrarConfirmacion: false,
    })
    if (recargar) {
      this.consultarDatos()
    }
  }

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarUsuarioAsignado()}
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

const Container = Wrapper(UsuariosAutorizadosContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("gestion_usuarios_autorizados_contratos")
)
