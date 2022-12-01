import React from "react"
import ReactDOM from "react-dom"

import Wrapper from "../../Base/BaseWrapper"
import http from "../../Base/HttpService"
import {
  MODULO_DOCUMENTOS,
  CONTROLLER_USUARIO_AUTORIZADO,
  FRASE_USUARIO_DESASIGNADO,
  FRASE_ERROR_GLOBAL,
  CONTROLLER_SECCION,
  FRASE_SECCION_ELIMINADA,
  CONTROLLER_DOCUMENTO,
  CONTROLLER_CARPETA,
  CONTROLLER_CONSULTA_CONTRATO,
} from "../../Base/Strings"
import { Button } from "primereact-v2/button"
import { Dialog } from "primereact-v2/components/dialog/Dialog"
import { Fragment } from "react"
import { Tree } from "primereact-v2/components/tree/Tree"

import axios from "axios"

class ListadoContratosContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      estructura: [],
      expandedKeys: {},
    }
  }

  componentDidMount() {
    this.consultarDatos()
  }

  render() {
    return (
      <div>
        <div className="row">
          <div style={{ width: "100%" }}>
            <div className="card">
              <div className="card-body">
                <div className="row">
                  <div className="col">
                    <h3>Contratos disponibles para consulta</h3>
                  </div>
                </div>
                <hr />
                <Tree
                  style={{ with: "100%" }}
                  selectionMode="single"
                  selectionKeys={this.state.selectedKey}
                  onSelectionChange={(e) =>
                    this.setState({ selectedKey: e.value })
                  }
                  value={this.state.estructura}
                  expandedKeys={this.state.expandedKeys}
                  dragdropScope="demo"
                  onToggle={(e) => this.setState({ expandedKeys: e.value })}
                  onSelect={this.onNodeSelect}
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }

  obtenerEstructuraArbol = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_CARPETA}/ObtenerCarpetasautorizadasAUsuario`
    return http.get(url)
  }

  consultarDatos = () => {
    console.log('Ingreso')
    this.props.blockScreen()
    var self = this
    Promise.all([this.obtenerEstructuraArbol()])
      .then(function ([estructura]) {
        self.setState(
          {
            estructura: estructura.data.result,
          },
          self.props.unlockScreen
        )
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  consultarDatosSeccion = (id) => {
    this.props.blockScreen()
    var self = this
    Promise.all([this.obtenerSeccionPorId(id)])
      .then(function ([seccion]) {
        self.setState(
          {
            seccionSeleccionada: seccion.data.result,
          },
          self.props.unlockScreen
        )
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  onNodeSelect(node) {
    window.location.href = `/${MODULO_DOCUMENTOS}/${CONTROLLER_CONSULTA_CONTRATO}/Consultar?contratoId=${node.node.key}`
  }

  actualizarSeccionSeleccionada = (seccion) => {
    this.setState({ seccionSeleccionada: seccion })
  }
}

const Container = Wrapper(ListadoContratosContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("listado_contratos_container")
)
