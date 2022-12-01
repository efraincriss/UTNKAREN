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
} from "../../Base/Strings"
import { Button } from "primereact-v2/button"
import { Dialog } from "primereact-v2/components/dialog/Dialog"
import { Fragment } from "react"
import { Tree } from "primereact-v2/components/tree/Tree"
import { ContextMenu } from "primereact-v2/contextmenu"
import axios from "axios"
import { CabeceraConsulta } from "./CabeceraConsultas.jsx"

class ConsultaContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      contratoId: 0,
      contrato: {},
    }
  }

 /*componentWillMount() {
    this.setState(
      {
        documentoId: this.props.getParameterByName("contratoId"),
      },
      () => console.log(this.state.contratoId)
    )
  }

  componentDidMount() {
    this.consultarDatos()
  }
*/
  render() {
    return (
      <div>
      {/**  <CabeceraConsulta carpeta={this.state.carpeta} />*/} 
        
        <h3>Filtros</h3>
        <div className="row">
          <div style={{ width: "100%" }}>
            <div className="card">
              <div className="card-body">
                <h3>Resultados</h3>
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }

  obtenerContrato = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_CARPETA}/ObtenerCarpeta/${this.state.contratoId}`
    return http.get(url)
  }

  consultarDatos = () => {
    this.props.blockScreen()
    var self = this
    Promise.all([this.obtenerContrato()])
      .then(function ([carpeta]) {
        self.setState(
          {
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
}

const Container = Wrapper(ConsultaContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("elementContenedorId")
)
