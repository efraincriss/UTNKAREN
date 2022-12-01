import { Button } from "primereact-v2/button"
import { Dialog } from "primereact-v3.3/dialog"
import React, { Fragment } from "react"
import ReactDOM from "react-dom"
import Wrapper from "../../Base/BaseWrapper"
import http from "../../Base/HttpService"
import {
  CONTROLLER_COLABORADOR_INGENIERIA, FRASE_ERROR_GLOBAL,
  FRASE_PARAMETRO_COLABORADOR_ELIMINADO,
  MODULO_CERTIFICACION_INGENIERIA
} from "../../Base/Strings"
import ColaboradorTable from "./ColaboradorTable.jsx"
import ParametrosTable from "./ParametrosTable.jsx"
import { ParametroForm } from "./ParamtroForm.jsx"

class ColaboradorIngenieriaContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      screen: "colaboradorList",
      colaboradores: [],
      colaborador: {},
      parametros: [],
      parametro: {},
      catalogoDisciplina: {},
      catalogoModalidad: {},
      catalogoUbicacion: {},
      mostrarConfirmacionEliminar: false,
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
    if (this.state.screen === "colaboradorList") {
      return this.colaboradorListComponent()
    } else if (this.state.screen === "colaboradorDetails") {
      return this.colaboradorDetail()
    } else {
      return this.colaboradorFormComponent()
    }
  }

  render() {
    return <Fragment>{this.renderBody()}</Fragment>
  }

  colaboradorListComponent = () => {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <ColaboradorTable
              verDetallesColaborador={this.verDetallesColaborador}
              data={this.state.colaboradores}
            />
          </div>
        </div>
      </Fragment>
    )
  }

  colaboradorDetail = () => {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col">
                <Button
                  label="Regresar"
                  className="p-button-outlined"
                  onClick={() => this.mostrarColaboradorList()}
                  icon="pi pi-chevron-left"
                />
              </div>
            </div>
            <hr />
            <div className="row">
              <div className="col-6">
                <p>
                  <b>Identificación</b>: {this.state.colaborador.Identificacion}
                </p>
              </div>
              <div className="col-6">
                <p>
                  <b>Código SAP</b>: {this.state.colaborador.CodigoSap}
                </p>
              </div>
            </div>
            <div className="row">
              <div className="col-6">
                <p>
                  <b>Apellidos Nombres</b>: {this.state.colaborador.Nombres}
                </p>
              </div>
              <div className="col-6">
                <p>
                  <b>Externo</b>: {this.state.colaborador.Externo}
                </p>
              </div>
            </div>

            <hr />

            <div className="row">
              <div className="col">
                <Button
                  label="Nuevo"
                  className="p-button-outlined"
                  onClick={() =>
                    this.mostrarFormulario({
                      ColaboradorId: this.state.colaborador.Id,
                    })
                  }
                  icon="pi pi-plus"
                />
              </div>
            </div>
            <ParametrosTable
              data={this.state.parametros}
              mostrarFormulario={this.mostrarFormulario}
              mostrarConfirmacionParaEliminar={this.mostrarConfirmacionParaEliminar}
            />

            <Dialog
              header="Confirmación"
              visible={this.state.mostrarConfirmacionEliminar}
              modal
              style={{ width: "500px" }}
              footer={this.construirBotonesDeConfirmacion()}
              onHide={this.ocultarConfirmacionEliminar}
            >
              <div className="confirmation-content">
                <div className="p-12">
                  <i
                    className="pi pi-exclamation-triangle p-mr-3"
                    style={{ fontSize: "2rem" }}
                  />
                  <p>
                    {" "}
                    ¿Está seguro de eliminar la parametrización del colaborador? Si desea continuar
                    presione ELIMINAR, caso contrario CANCELAR
                  </p>
                </div>
              </div>
            </Dialog>
          </div>
        </div>
      </Fragment>
    )
  }

  colaboradorFormComponent = () => {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <ParametroForm
              ocultarFormulario={this.ocultarFormulario}
              actualizarParametroSeleccionado={
                this.actualizarParametroSeleccionado
              }
              parametro={this.state.parametro}
              catalogoDisciplina={this.state.catalogoDisciplina}
              catalogoModalidad={this.state.catalogoModalidad}
              catalogoUbicacion={this.state.catalogoUbicacion}
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
    Promise.all([this.obtenerColaboradores(), this.obtenerCatalogos()])
      .then(function ([colaboradores, catalogos]) {
        let colaboradoresData = colaboradores.data
        if (colaboradoresData.success === true) {
          self.setState({
            colaboradores: colaboradoresData.result,
          })
        }

        let catalogosData = catalogos.data
        let catalogoDisciplina = self.props.buildDropdown(
          catalogosData.result.Disciplina,
          "nombre"
        )
        let catalogoModalidad = self.props.buildDropdown(
          catalogosData.result.Modalidad,
          "nombre"
        )
        let catalogoUbicacion = self.props.buildDropdown(
          catalogosData.result.Ubicacion,
          "nombre"
        )
        if (catalogosData.success === true) {
          self.setState({
            catalogoDisciplina,
            catalogoModalidad,
            catalogoUbicacion,
          })
        }

        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  obtenerParametrizacionPorColaborador = (colaborador) => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_INGENIERIA}/ObtenerParametrizacionPorColaboradorId/${colaborador.Id}`

    http
      .get(url)
      .then((response) => {
        let data = response.data
        if (data.success === true) {
          this.setState({
            colaborador,
            screen: "colaboradorDetails",
            parametros: data.result,
          })
          this.props.unlockScreen()
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

  eliminarParametro = () => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_INGENIERIA}/Eliminar/${this.state.parametro.Id}`

    http
      .delete(url, {})
      .then((response) => {
        console.log(response)
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_PARAMETRO_COLABORADOR_ELIMINADO)
          this.ocultarConfirmacionEliminar(true)
          this.obtenerParametrizacionPorColaborador(this.state.colaborador)
          this.props.unlockScreen()
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

  mostrarColaboradorList = () => {
    this.setState({ screen: "colaboradorList", colaborador: {} })
  }

  obtenerColaboradores = () => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_INGENIERIA}/ObtenerColaboradores`
    return http.get(url)
  }

  obtenerCatalogos = () => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_INGENIERIA}/GetCatalogos`
    return http.get(url)
  }

  verDetallesColaborador = (colaborador) => {
    this.obtenerParametrizacionPorColaborador(colaborador)
  }

  mostrarFormulario = (parametro) => {
    this.setState({
      screen: "parametroForm",
      parametro,
    })
  }

  ocultarFormulario = (reload = false) => {
    this.setState({
      screen: "colaboradorDetails",
      parametro: {},
    })
    if (reload) {
      this.obtenerParametrizacionPorColaborador(this.state.colaborador)
    }
  }

  mostrarConfirmacionParaEliminar = (parametro) => {
    this.setState({
      parametro,
      mostrarConfirmacionEliminar: true,
    })
  }

  ocultarConfirmacionEliminar = () => {
    this.setState({ mostrarConfirmacionEliminar: false, parametro: {} })
  }

  actualizarParametroSeleccionado = (parametro) => {
    this.setState({ parametro })
  }

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarParametro()}
          icon="pi pi-save"
        />
        <Button
          style={{ marginLeft: "0.4em" }}
          label="Cancelar"
          className="p-button-outlined"
          onClick={() => this.ocultarConfirmacionEliminar()}
          icon="pi pi-ban"
        />
      </Fragment>
    )
  }
}

const Container = Wrapper(ColaboradorIngenieriaContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("colaborador_certificacion_ingenieria_container")
)
