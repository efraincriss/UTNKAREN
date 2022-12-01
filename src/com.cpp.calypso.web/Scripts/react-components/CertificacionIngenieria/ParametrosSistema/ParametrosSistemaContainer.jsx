import { Button } from "primereact-v2/button"
import React, { Fragment } from "react"
import ReactDOM from "react-dom"
import Wrapper from "../../Base/BaseWrapper"
import http from "../../Base/HttpService"
import {
  CONTROLLER_PARAMETROS,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"
import { ParametroSistemaForm } from "./ParametroSistemaForm.jsx"
import ParametroSistemaTable from "./ParametroSistemaTable.jsx"

class ParametrosSistemaContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      screen: "parametrosList",
      parametros: [],
      parametro: {},
    }
  }

  componentDidMount() {
    this.loadData()
  }

  handleChange = (name, value) => {
    this.setState({ [name]: value })
  }

  handleChangeFechas = (event) => {
    const target = event.target
    const value = target.value
    const name = target.name
    this.setState({ [name]: value })
  }

  renderBody = () => {
    if (this.state.screen === "parametrosList") {
      return this.parametrosList()
    } else {
      return this.paramtrosForm()
    }
  }

  render() {
    return <Fragment>{this.renderBody()}</Fragment>
  }

  parametrosList = () => {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <ParametroSistemaTable
              data={this.state.parametros}
              mostrarFormulario={this.mostrarFormulario}
            />
          </div>
        </div>
      </Fragment>
    )
  }

  paramtrosForm = () => {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <ParametroSistemaForm
              ocultarFormulario={this.ocultarFormulario}
              actualizarParametroSeleccionado={
                this.actualizarParametroSeleccionado
              }
              parametro={this.state.parametro}
              showSuccess={this.props.showSuccess}
              showWarn={this.props.showWarn}
              blockScreen={this.props.blockScreen}
              unlockScreen={this.props.unlockScreen}
              key={this.state.keyForm}
            />
          </div>
        </div>
      </Fragment>
    )
  }

  loadData = () => {
    this.props.blockScreen()
    var self = this
    Promise.all([this.obtenerParametros()])
      .then(function ([parametros]) {
        let parametrosData = parametros.data
        if (parametrosData.success === true) {
          self.setState({
            parametros: parametrosData.result,
          })
        }

        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  mostrarParametroList = () => {
    this.setState({ screen: "parametrosList", parametro: {} })
  }

  obtenerParametros = () => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PARAMETROS}/ObtenerParametrosPorModuloCertificacion`
    return http.get(url)
  }

  mostrarFormulario = (parametro) => {
    this.setState({
      screen: "parametroForm",
      parametro,
    })
  }

  ocultarFormulario = (reload = false) => {
    this.setState({
      screen: "parametrosList",
      parametro: {},
    })
    if (reload) {
      this.loadData()
    }
  }

  actualizarParametroSeleccionado = (parametro) => {
    this.setState({ parametro })
  }
}

const Container = Wrapper(ParametrosSistemaContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("parametros_sistema_container")
)
