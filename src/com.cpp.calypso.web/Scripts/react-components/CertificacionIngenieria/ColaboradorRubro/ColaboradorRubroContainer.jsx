import { Button } from "primereact-v2/button"
import { Dialog } from "primereact-v3.3/dialog"
import React, { Fragment } from "react"
import ReactDOM from "react-dom"
import Wrapper from "../../Base/BaseWrapper"
import http from "../../Base/HttpService"
import {
  CONTROLLER_COLABORADOR_RUBRO,
  FRASE_ERROR_GLOBAL,
  FRASE_RUBRO_ELIMINADO,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"
import { ColaboradorRubroForm } from "./ColaboradorRubroForm.jsx"
import { ColaboradorRubroTable } from "./ColaboradorRubroTable.jsx"
import Field from "../../Base/Field-v2"
import { FileUpload } from "primereact-v2/fileupload"
import axios from "axios"
import { ColaboradorRubroFormMasivo } from "./ColaboradorRubroFormMasivo.jsx"

class ColaboradorRubroContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      rubros: [],
      rubroSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: 12,
      screen: "list",
      catalogoContratos: [],
      catalogoItems: [],
      catalogoRubros: [],
      ContratoId: 0,
      errors: {},
      FechaInicio: "",
      FechaFin: ""
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
    const value =  target.value
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
            <div
              className="row"
              style={{ marginTop: "1em", marginBottom: "1em" }}
            >
              <div className="col" align="right">
                <button
                  className="btn btn-outline-primary mr-4"
                  type="button"
                  data-toggle="collapse"
                  data-target="#collapseDescargaArchivo"
                  aria-expanded="false"
                  aria-controls="collapseDescargaArchivo"
                >
                  Descargar formato carga masiva
                </button>
                <button
                  className="btn btn-outline-primary mr-4"
                  type="button"
                  data-toggle="collapse"
                  data-target="#collapseExample"
                  aria-expanded="false"
                  aria-controls="collapseExample"
                >
                  Cargar Colaborador - Tarifas
                </button>
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

            <div className="row">
              <div className="col">
                <div className="collapse" id="collapseDescargaArchivo">
                  <div className="card card-body">
                    <div className="row">
                      <div className="col">
                        <Field
                          name="ContratoId"
                          label="Contrato"
                          type="select"
                          options={this.state.catalogoContratos}
                          edit={true}
                          readOnly={false}
                          value={this.state.ContratoId}
                          onChange={this.handleChange}
                          error={this.state.errors.ContratoId}
                          placeholder="Seleccionar..."
                        />
                      </div>
                    </div>
                    <div className="row" style={{ marginLeft: "0.1em" }}>
                      <Button
                        label="Descargar"
                        className="p-button-outlined"
                        onClick={() =>
                          this.descargarFormatoCargaMasivaDeTarifas()
                        }
                        icon="pi pi-save"
                      />
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
                onClick={() => this.obtenerRubrosColaborador()}
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
              <div className="col" align="right">
                <button
                  className="btn btn-outline-primary"
                  onClick={() => this.mostrarFormulario({})}
                >
                  Nuevo
                </button>
              </div>
            </div>
            <hr />
            <div className="row">
              <div className="col">
                <ColaboradorRubroTable
                  data={this.state.rubros}
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
                    ¿Está seguro de eliminar el rubro? Si desea continuar
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
                <ColaboradorRubroFormMasivo
                  rubro={this.state.rubroSeleccionado}
                  actualizarRubroSeleccionado={this.actualizarRubroSeleccionado}
                  onHideFormulario={this.onHideFormulario}
                  showSuccess={this.props.showSuccess}
                  showWarn={this.props.showWarn}
                  blockScreen={this.props.blockScreen}
                  unlockScreen={this.props.unlockScreen}
                  key={this.state.keyForm}
                  catalogoContratos={this.state.catalogoContratos}
                  catalogoItems={this.state.catalogoItems}
                  catalogoRubros={this.state.catalogoRubros}
                  obtenerRubrosPorcontrato={this.obtenerRubrosPorcontrato}
                  obtenerRubrosColaborador={this.obtenerRubrosColaborador}
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
    Promise.all([this.obtenerContratos(), this.obtenerItems()])
      .then(function ([contratos, items]) {

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

        let itemsData = items.data
        if (itemsData.success === true) {
          let catalogoItems = self.props.buildDropdown(
            itemsData.result,
            "nombre"
          )
          self.setState({
            catalogoItems,
          })
        }
        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  obtenerRubrosColaborador = () => {
    
    let fechaInicio = this.state.FechaInicio;
    let fechaFin = this.state.FechaFin;
    const errors = {}

    if (fechaInicio !== "") {
      if (fechaFin === "") {
        errors.FechaFin = "Fecha de finalización es requerida"
        this.setState({ errors })
        return;
      }
    }

    if (fechaFin !== "") {
      if (fechaInicio === "") {
        errors.FechaInicio = "Fecha de inicio es requerida"
        this.setState({ errors });
        return
      }
    }
    this.setState({errors})

    this.props.blockScreen()
    var self = this
    Promise.all([this.promiseObtenerRubrosColaborador()])
      .then(function ([rubros]) {
        let rubrosData = rubros.data
        if (rubrosData.success === true) {
          self.setState({
            rubros: rubrosData.result,
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
    this.setState({ FechaInicio: "", FechaFin: ""});
  }

  eliminarRubroColaborador = () => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_RUBRO}/EliminarColaboradorRubro/${this.state.rubroSeleccionado.Id}`

    http
      .delete(url, {})
      .then((response) => {
        console.log(response)
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_RUBRO_ELIMINADO)
          this.onHideFormulario(true)
          this.obtenerRubrosColaborador();

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

  mostrarFormulario = async (rubro) => {
    if (rubro.ContratoId !== undefined) {
      await this.obtenerRubrosPorcontrato(rubro.ContratoId)
    } else {
      this.setState({ catalogoRubros: [] })
    }

    this.setState({
      rubroSeleccionado: rubro,
      screen: "form",
    })
  }

  mostrarConfirmacionParaEliminar = (rubro) => {
    this.setState({
      rubroSeleccionado: rubro,
      mostrarConfirmacion: true,
    })
  }

  onHideFormulario = (recargar = false) => {
    this.setState({
      screen: "list",
      rubroSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: Math.random(),
    })
    if (recargar) {
      this.obtenerRubrosColaborador()
    }
  }

  promiseObtenerRubrosColaborador = () => {
    let url = ""
    let params = `?fechaInicio=${this.state.FechaInicio}&fechaFin=${this.state.FechaFin}`
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_RUBRO}/ObtenerColaboresRubrosConFiltro${params}`
    return http.get(url)
  }

  obtenerContratos = () => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_RUBRO}/GetContratos`
    return http.get(url)
  }

  obtenerItems = () => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_RUBRO}/ObtenerItems`
    return http.get(url)
  }

  obtenerRubrosPorcontrato = (contratoId) => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_RUBRO}/ObtenerRubrosPorContrato/${contratoId}`

    http
      .get(url)
      .then((response) => {
  
        let data = response.data
        if (data.success === true) {
          let catalogoRubros = data.result.map((i) => {
            return {
              label:i.Item.codigo+" - "+ i.Item.nombre,
              value: i.Id,
              tarifa: i.precio_unitario,
            }
          })
          this.setState({
            catalogoRubros,
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

  actualizarRubroSeleccionado = (rubro) => {
    this.setState({ rubroSeleccionado: rubro })
  }

  descargarFormatoCargaMasivaDeTarifas = () => {
    if (this.state.ContratoId === 0) {
      this.props.showWarn("El campo contrato es requerido")
      return
    }

    this.props.blockScreen()
    var formData = new FormData()
    formData.append("contratoId", this.state.ContratoId)

    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_RUBRO}/DescargarPlantillaCargaMasivaDeTarifas`,
        formData,
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
          `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_RUBRO}/CargaMasivaDeTarifas`,
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
          this.obtenerRubrosColaborador();
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

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarRubroColaborador()}
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

const Container = Wrapper(ColaboradorRubroContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("colaborador_rubro_container")
)
