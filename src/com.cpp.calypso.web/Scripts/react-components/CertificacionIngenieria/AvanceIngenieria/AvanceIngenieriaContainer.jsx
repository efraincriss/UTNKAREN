import { Button } from "primereact-v2/button"
import { Dialog } from "primereact-v3.3/dialog"
import React, { Fragment } from "react"
import ReactDOM from "react-dom"
import Wrapper from "../../Base/BaseWrapper"
import Field from "../../Base/Field-v2"
import http from "../../Base/HttpService"
import {
  CONTROLLER_PORCENTAJE_AVANCE_INGENIERIA,
  FRASE_AVANCE_INGENIERIA_ELIMINADO,
  FRASE_ERROR_GLOBAL,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"
import { AvanceIngenieriaForm } from "./AvanceIngenieriaForm.jsx"
import AvanceIngenieriaTable from "./AvanceIngenieriaTable.jsx"

class AvanceIngenieriaContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      avances: [],
      avanceSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: 12,
      screen: "list",
      catalogoProyectos: [],
      catalogoPorcentajes: [],
      errors: {},
      FechaDesde: "",
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
    if (this.state.screen === "list") {
      return this.listComponent()
    } else {
      return this.formComponent()
    }
  }

  render() {
    return <Fragment>{this.renderBody()}</Fragment>
  }

  listComponent = () => {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col">
                <Field
                  name="FechaDesde"
                  value={this.state.FechaDesde}
                  label="Fecha Desde"
                  type="date"
                  onChange={this.handleChangeFechas}
                  error={this.state.errors.FechaDesde}
                  edit={true}
                  readOnly={false}
                />
              </div>
              <div className="col"></div>
            </div>

            <div className="row" style={{ marginLeft: "0.1em" }}>
              <Button
                label="Buscar"
                className="p-button-outlined"
                onClick={() => this.obtenerAvances()}
                icon="pi pi-search"
              />
              <Button
                style={{ marginLeft: "0.4em" }}
                label="Borrar Filtros"
                className="p-button-outlined"
                onClick={() => this.borrarFiltros()}
                icon="pi pi-ban"
              />
            </div>
          </div>
        </div>

        <div className="card">
          <div className="card-body">
            <div className="row" style={{ marginTop: "1em" }}>
              <div className="col" align="right">
                <Button
                  label="Nuevo"
                  className="p-button-outlined"
                  onClick={() => this.mostrarFormulario({})}
                  icon="pi pi-plus"
                />
              </div>
            </div>
            <hr />

            <div className="row">
              <div className="col">
                <AvanceIngenieriaTable
                  data={this.state.avances}
                  mostrarFormulario={this.mostrarFormulario}
                  mostrarConfirmacionParaEliminar={
                    this.mostrarConfirmacionParaEliminar
                  }
                />
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
                    {" "}
                    ¿Está seguro de eliminar el avance? Si desea continuar
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

  formComponent = () => {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col">
                <AvanceIngenieriaForm
                  avance={this.state.avanceSeleccionado}
                  actualizarAvanceSeleccionado={
                    this.actualizarAvanceSeleccionado
                  }
                  onHideFormulario={this.onHideFormulario}
                  showSuccess={this.props.showSuccess}
                  showWarn={this.props.showWarn}
                  blockScreen={this.props.blockScreen}
                  unlockScreen={this.props.unlockScreen}
                  key={this.state.keyForm}
                  catalogoProyectos={this.state.catalogoProyectos}
                  catalogoPorcentajes={this.state.catalogoPorcentajes}
                  obtenerAvances={this.obtenerAvances}
                />
              </div>
            </div>
          </div>
        </div>
      </Fragment>
    )
  }

  loadData = () => {
    this.props.blockScreen()
    var self = this
    Promise.all([
      this.promiseObtenerProyectos(),
      this.promiseObtenerCatalogosPorcentajes(),
    ])
      .then(function ([proyectos, porcentajes]) {
        let porcentajesData = porcentajes.data
        if (porcentajesData.success === true) {
          let catalogoPorcentajes = self.props.buildDropdown(
            porcentajesData.result,
            "nombre"
          )
          self.setState({
            catalogoPorcentajes,
          })
        }

        let proyectosData = proyectos.data
        if (proyectosData.success === true) {
          let catalogoProyectos = self.props.buildDropdown(
            proyectosData.result,
            "Nombre"
          )
          self.setState({
            catalogoProyectos,
          })
        }
        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  obtenerAvances = () => {
    let fechaDesde = this.state.FechaDesde
    const errors = {}
    if (fechaDesde === "") {
      errors.FechaDesde = "Fecha desde es requerida"
      this.setState({ errors })
      return
    }
    this.setState({ errors })

    this.props.blockScreen()
    var self = this
    Promise.all([this.promiseObtenerAvances()])
      .then(function ([avances]) {
        let avancesData = avances.data
        if (avancesData.success === true) {
          self.setState({
            avances: avancesData.result,
          })
        }
        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  borrarFiltros = () => {
    this.setState({ FechaDesde: "" })
  }

  eliminarAvance = () => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PORCENTAJE_AVANCE_INGENIERIA}/Eliminar/${this.state.avanceSeleccionado.Id}`

    http
      .delete(url, {})
      .then((response) => {
        console.log(response)
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_AVANCE_INGENIERIA_ELIMINADO)
          this.onHideFormulario(true)
          this.obtenerAvances()
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

  mostrarFormulario = async (avance) => {
    this.setState({
      avanceSeleccionado: avance,
      screen: "form",
    })
  }

  mostrarConfirmacionParaEliminar = (avance) => {
    this.setState({
      avanceSeleccionado: avance,
      mostrarConfirmacion: true,
    })
  }

  onHideFormulario = (recargar = false) => {
    this.setState({
      screen: "list",
      avanceSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: Math.random(),
    })
    if (recargar) {
      this.obtenerAvances()
    }
  }

  promiseObtenerAvances = () => {
    let url = ""
    let params = `?fechaDesde=${this.state.FechaDesde}`
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PORCENTAJE_AVANCE_INGENIERIA}/ObtenerAvancesIngenieriaPorFecha${params}`
    return http.get(url)
  }

  promiseObtenerProyectos = () => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PORCENTAJE_AVANCE_INGENIERIA}/ObtenerProyectos`
    return http.get(url)
  }

  promiseObtenerCatalogosPorcentajes = () => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PORCENTAJE_AVANCE_INGENIERIA}/ObtenerCatalogos`
    return http.get(url)
  }

  actualizarAvanceSeleccionado = (avance) => {
    this.setState({ avanceSeleccionado: avance })
  }

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarAvance()}
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

const Container = Wrapper(AvanceIngenieriaContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("porcentaje_avance_ingenieria")
)
