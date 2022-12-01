import axios from "axios"
import { Button } from "primereact-v2/button"
import { FileUpload } from "primereact-v2/fileupload"
import { Dialog } from "primereact-v3.3/dialog"
import React, { Fragment } from "react"
import ReactDOM from "react-dom"
import Wrapper from "../../Base/BaseWrapper"
import Field from "../../Base/Field-v2"
import http from "../../Base/HttpService"
import {
  CONTROLLER_COLABORADOR_RUBRO,
  CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA,
  CONTROLLER_PORCENTAJE_INDIRECTOS_INGENIERIA,
  FRASE_DETALLE_INDIRECTO_ELIMINADO,
  FRASE_ERROR_GLOBAL,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"
import { DetalleGastosIndirectos } from "./DetalleGastosIndirectos.jsx"
import { DetallesIndirectosIngenieriaForm } from "./DetalleIndirectoIngenieriaForm.jsx"
import { DetalleIndirectoIngenieriaTable } from "./DetalleIndirectoIngenieriaTable.jsx"
import { PorcentajeIndirectosForm } from "./PorcentajeIndirectosForm.jsx"
import CurrencyFormat from "react-currency-format";

class DetallesIndirectosIngenieriaContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      detalles: [],
      detalleSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: 12,
      screen: "list",
      errors: {},
      FechaInicio: "",
      FechaFin: "",
      porcentajes: [],
      totalPorcentaje: 0,
      porcentajeSeleccionado: {},
      totalHoras: 0,
      totalRegistros: 0,
      isUpdating: true,
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
    } else if (this.state.screen === "indirectosForm") {
      return this.formComponent()
    } else if (this.state.screen === "detalleIndirectos") {
      return this.detalleIndirectoComponent()
    } else if (this.state.screen === "porcentajesIndirectosForm") {
      return this.formularioPorcentajesComponent()
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
            <div
              className="row"
              style={{ marginTop: "1em", marginBottom: "1em" }}
            >
              <div className="col" align="right">
                <Button
                  label="Descargar formato"
                  className="p-button-outlined"
                  onClick={() => this.descargarFormatoCargaMasivaDeTarifas()}
                  icon="pi pi-save"
                />
                <Button
                  style={{ marginLeft: "0.2em" }}
                  label="Cargar Detalles Indirectos"
                  className="p-button-outlined"
                  icon="pi pi-eject"
                  data-toggle="collapse"
                  data-target="#collapseExample"
                  aria-expanded="false"
                  aria-controls="collapseExample"
                />
              </div>
            </div>

            <div className="row">
              <div className="col">
                <div className="collapse" id="collapseExample">
                  <div className="card card-body">
                    <div className="row">
                      <div className="col">
                        <FileUpload
                          name="uploadedFile"
                          chooseLabel="Seleccionar"
                          cancelLabel="Cancelar"
                          uploadLabel="Cargar"
                          onUpload={this.onBasicUpload}
                          accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                          maxFileSize={1000000}
                          url=""
                        />
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col">
                <Field
                  name="FechaInicio"
                  value={this.state.FechaInicio}
                  label="Fecha Inicio"
                  type="date"
                  onChange={this.handleChangeFechas}
                  error={this.state.errors.FechaInicio}
                  edit={true}
                  readOnly={false}
                />
              </div>

              <div className="col">
                <Field
                  name="FechaFin"
                  value={this.state.FechaFin}
                  label="Fecha Fin"
                  type="date"
                  onChange={this.handleChangeFechas}
                  error={this.state.errors.FechaFin}
                  edit={true}
                  readOnly={false}
                />
              </div>
            </div>

            <div className="row" style={{ marginLeft: "0.1em" }}>
              <Button
                label="Buscar"
                className="p-button-outlined"
                onClick={() => this.obtenerDetallesIndirectos()}
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

            <div className="row" style={{ marginTop: "1em" }}>

            </div>

            <div className="row">

              <div className="col-3">
                <div className="callout callout-danger">
                  <small className="text-muted">Total HH</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.state.totalHoras.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>

              <div className="col-3">
                <div className="callout callout-warning">
                  <small className="text-muted">Total Registros</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.state.totalRegistros.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>

              <div className="col-3"></div>

              <div className="col-3" align="right">
                <button
                  className="btn btn-outline-primary align-middle"
                  style={{ marginTop: '1.3em' }}
                  type="button"
                  data-toggle="collapse"
                  aria-expanded="false"
                  onClick={() => this.mostrarFormulario({})}
                >
                  Nuevo
                </button>
              </div>
            </div>

            <hr />
            <div className="row">
              <div className="col">
                <DetalleIndirectoIngenieriaTable
                  afterColumnFilter={this.afterColumnFilter}
                  data={this.state.detalles}
                  mostrarFormulario={this.mostrarFormulario}
                  mostrarConfirmacionParaEliminar={
                    this.mostrarConfirmacionParaEliminar
                  }
                  mostrarDetalleGastoIndirecto={
                    this.mostrarDetalleGastoIndirecto
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
                    ¿Está seguro de eliminar el detalle? Si desea continuar
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
                <DetallesIndirectosIngenieriaForm
                  detalle={this.state.detalleSeleccionado}
                  actualizardetalleSeleccionado={
                    this.actualizardetalleSeleccionado
                  }
                  onHideFormulario={this.onHideFormulario}
                  showSuccess={this.props.showSuccess}
                  showWarn={this.props.showWarn}
                  blockScreen={this.props.blockScreen}
                  unlockScreen={this.props.unlockScreen}
                  key={this.state.keyForm}
                  obtenerDetallesIndirectos={this.obtenerDetallesIndirectos}
                />
              </div>
            </div>
          </div>
        </div>
      </Fragment>
    )
  }

  detalleIndirectoComponent = () => {
    return (
      <Fragment>
        <DetalleGastosIndirectos
          detalleIndirecto={this.state.detalleSeleccionado}
          onHideFormulario={this.onHideFormulario}
          mostrarFormularioPocentajes={this.mostrarFormularioPocentajes}
          porcentajes={this.state.porcentajes}
          showSuccess={this.props.showSuccess}
          showWarn={this.props.showWarn}
          blockScreen={this.props.blockScreen}
          unlockScreen={this.props.unlockScreen}
          obtenerProcentajes={this.obtenerProcentajes}
        />
      </Fragment>
    )
  }

  formularioPorcentajesComponent = () => {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col">
                <PorcentajeIndirectosForm
                  porcentaje={this.state.porcentajeSeleccionado}
                  actualizarPorcentajeSeleccionado={
                    this.actualizarPorcentajeSeleccionado
                  }
                  onHideFormulario={this.onHideFormularioPorcentaje}
                  showSuccess={this.props.showSuccess}
                  showWarn={this.props.showWarn}
                  blockScreen={this.props.blockScreen}
                  unlockScreen={this.props.unlockScreen}
                  key={this.state.keyForm}
                  catalogoContratos={this.state.catalogoContratos}
                  totalPorcentaje={this.state.totalPorcentaje}
                />
              </div>
            </div>
          </div>
        </div>
      </Fragment>
    )
  }

  afterColumnFilter = async (filterConds, result) => {
    if (result !== null) {
      let totalHoras = 0;
      let totalRegistros = 0;

      result.forEach(element => {
        totalHoras += (element.HorasLaboradas * element.DiasLaborados)
        totalRegistros += 1;
      });

      if (this.state.isUpdating) {
        this.setState({ isUpdating: false });
        await setTimeout(1000);
        this.setState({ totalHoras, totalRegistros })

        setTimeout(function () {
          this.setState({ isUpdating: true });
        }.bind(this), 1000);
      }

    }
  };

  obtenerDetallesIndirectos = () => {
    let fechaInicio = this.state.FechaInicio
    let fechaFin = this.state.FechaFin
    const errors = {}

    if (fechaInicio !== "") {
      if (fechaFin === "") {
        errors.FechaFin = "Fecha de finalización es requerida"
        this.setState({ errors })
        return
      }
    }

    if (fechaFin !== "") {
      if (fechaInicio === "") {
        errors.FechaInicio = "Fecha de inicio es requerida"
        this.setState({ errors })
        return
      }
    }
    this.setState({ errors })

    this.props.blockScreen()
    var self = this
    Promise.all([this.promiseObtenerDetallesIndirectosIngenieria()])
      .then(function ([detalles]) {
        let detallesData = detalles.data
        if (detallesData.success === true) {

          let totalHoras = 0;
          let totalRegistros = 0;

          detallesData.result.forEach(element => {
            totalHoras += (element.HorasLaboradas * element.DiasLaborados)
            totalRegistros += 1;
          });

          self.setState({
            detalles: detallesData.result,
            totalHoras,
            totalRegistros
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
    this.setState({ FechaInicio: "", FechaFin: "" })
  }

  eliminarDetalleIndirectoIngenieria = () => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA}/Eliminar/${this.state.detalleSeleccionado.Id}`

    http
      .delete(url, {})
      .then((response) => {
        console.log(response)
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_DETALLE_INDIRECTO_ELIMINADO)
          this.onHideFormulario(true)
          this.obtenerDetallesIndirectos()
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

  mostrarDetalleGastoIndirecto = (detalle) => {
    this.setState(
      {
        detalleSeleccionado: detalle,
        screen: "detalleIndirectos",
      },
      this.obtenerProcentajes
    )
  }

  mostrarFormularioPocentajes = (porcentaje) => {
    this.setState({
      porcentajeSeleccionado: porcentaje,
      screen: "porcentajesIndirectosForm",
    })
  }

  mostrarFormulario = async (detalle) => {
    this.setState({
      detalleSeleccionado: detalle,
      screen: "indirectosForm",
    })
  }

  mostrarConfirmacionParaEliminar = (detalle) => {
    this.setState({
      detalleSeleccionado: detalle,
      mostrarConfirmacion: true,
    })
  }

  onHideFormulario = (recargar = false) => {
    this.setState({
      screen: "list",
      detalleSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: Math.random(),
    })
    if (recargar) {
      this.obtenerDetallesIndirectos()
    }
  }

  onHideFormularioPorcentaje = (recargar = false) => {
    this.setState({
      porcentajeSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: Math.random(),
      screen: "detalleIndirectos",
    })
    if (recargar) {
      this.obtenerProcentajes()
    }
  }

  promiseObtenerDetallesIndirectosIngenieria = () => {
    let url = ""
    let params = `?fechaInicio=${this.state.FechaInicio}&fechaFin=${this.state.FechaFin}`
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA}/ObtenerIndirectosIngenieriaPorFechas${params}`
    return http.get(url)
  }

  promiseObtenerPorcentajesPorGastoIndirectoId = (id) => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PORCENTAJE_INDIRECTOS_INGENIERIA}/ObtenerPorcentajesDelDetalleIndirecto/${id}`
    return http.get(url)
  }

  obtenerContratos = () => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_RUBRO}/GetContratos`
    return http.get(url)
  }


  actualizardetalleSeleccionado = (detalle) => {
    this.setState({ detalleSeleccionado: detalle })
  }

  actualizarPorcentajeSeleccionado = (porcentaje) => {
    this.setState({ porcentajeSeleccionado: porcentaje })
  }

  descargarFormatoCargaMasivaDeTarifas = () => {
    this.props.blockScreen()
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA}/DescargarPlantillaCargaMasivaGastosIndirectos`,
        {},
        { responseType: "arraybuffer" }
      )
      .then((response) => {
        var nombre = response.headers["content-disposition"].split("=")

        const url = window.URL.createObjectURL(
          new Blob([response.data], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          })
        )
        const link = document.createElement("a")
        link.href = url
        link.setAttribute("download", nombre[1])
        document.body.appendChild(link)
        link.click()
        this.props.showSuccess("Formato descargado exitosamente")
        this.props.unlockScreen()
        this.setState({
          ContratoId: 0,
        })
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(
          "Ocurrió un error al descargar el archivo, intentalo nuevamente"
        )
        this.props.unlockScreen()
      })
  }

  onBasicUpload = (event) => {
    var file = event.files[0]

    if (
      file.type ===
      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    ) {
      this.setState({ file })

      this.props.blockScreen()
      var formData = new FormData()
      formData.append("file", event.files[0])

      axios
        .post(
          `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA}/CargaMasivaDeGastosIndirectosAsync`,
          formData,
          {
            headers: {
              "Content-Disposition": "attachment; filename=template.xlsx",
              "Content-Type":
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            },
            responseType: "arraybuffer",
          }
        )
        .then((response) => {
          var nombre = response.headers["content-disposition"].split("=")

          const url = window.URL.createObjectURL(
            new Blob([response.data], {
              type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            })
          )
          const link = document.createElement("a")
          link.href = url
          link.setAttribute("download", nombre[1])
          document.body.appendChild(link)
          link.click()
          this.obtenerDetallesIndirectos()
          this.props.showSuccess("Archivo procesado exitosamente")
        })
        .catch((error) => {
          console.log(error)
          this.props.showWarn(
            "Ocurrió un error al subir el archivo, intentalo nuevamente"
          )
          this.props.unlockScreen()
        })
    } else {
      this.props.showWarn("El formato de archivo es incorrecto")
    }
  }

  loadData = () => {
    this.props.blockScreen()
    var self = this
    Promise.all([this.obtenerContratos()])
      .then(function ([contratos]) {
        let contratosData = contratos.data
        if (contratosData.success === true) {
          let catalogoContratos = self.props.buildDropdown(
            contratosData.result,
            "nombrecompleto"
          )
          self.setState({
            catalogoContratos,
          })
        }
        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  obtenerProcentajes = () => {
    this.props.blockScreen()
    var self = this
    Promise.all([
      this.promiseObtenerPorcentajesPorGastoIndirectoId(
        this.state.detalleSeleccionado.Id
      ),
    ])
      .then(function ([porcentajes]) {
        let porcentajesData = porcentajes.data
        if (porcentajesData.success === true) {
          let totalPorcentajesDeDetalleIndirecto = 0;
          porcentajesData.result.forEach(element => {
            totalPorcentajesDeDetalleIndirecto += element.PorcentajeIndirecto
          });

          self.setState({
            porcentajes: porcentajesData.result,
            totalPorcentaje: totalPorcentajesDeDetalleIndirecto
          })
        }
        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarDetalleIndirectoIngenieria()}
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

const Container = Wrapper(DetallesIndirectosIngenieriaContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("detalles_indirectos_ingenieria_container")
)
