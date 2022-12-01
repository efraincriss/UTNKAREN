import { Button } from "primereact-v2/button"
import { Dialog } from "primereact-v3.3/dialog"
import React, { Fragment } from "react"
import config from "../../Base/Config"
import Field from "../../Base/Field-v2"
import http from "../../Base/HttpService"
import {
  CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA,
  FRASE_DETALLE_INDIRECTO_ACTUALIZADO,
  FRASE_DETALLE_INDIRECTO_CREADO,
  FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"
import BuscarColaboradorContainer from "../BuscarColaborador/BuscarColaboradorContainer"
import { Checkbox } from "primereact-v2/checkbox"

export class DetallesIndirectosIngenieriaForm extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      data: {
        Id: props.detalle.Id ? props.detalle.Id : 0,
        FechaDesde: props.detalle.FechaDesde ? props.detalle.FechaDesde : "",
        FechaHasta: props.detalle.FechaHasta ? props.detalle.FechaHasta : "",
        ColaboradorId: props.detalle.ColaboradorId
          ? props.detalle.ColaboradorId
          : 0,
        ColaboradorNombres: props.detalle.ColaboradorNombres
          ? props.detalle.ColaboradorNombres
          : "",
        ColaboradorRubroId: props.detalle.ColaboradorRubroId
          ? props.detalle.ColaboradorRubroId
          : 0,
        DiasLaborados: props.detalle.DiasLaborados
          ? props.detalle.DiasLaborados
          : 0,
        HorasLaboradas: props.detalle.HorasLaboradas
          ? props.detalle.HorasLaboradas
          : 0,
        EsViatico: props.detalle.EsViatico ? props.detalle.EsViatico : false,
        Certificado: props.detalle.Certificado
          ? props.detalle.Certificado
          : false,

        CreationTime: props.detalle.CreationTime
          ? props.detalle.CreationTime
          : new Date().toISOString(),
        CreatorUserId: props.detalle.CreatorUserId
          ? props.detalle.CreatorUserId
          : null,
      },
      errors: {},
      displayMessage: "",
      keyUpload: 98,
      showColaboradorModal: false,
      colaborador: {},
    }
    this.busquedaRef = React.createRef()
  }

  componentWillReceiveProps(prevProps) {
    let updatedValues = {
      Id: prevProps.detalle.Id ? prevProps.detalle.Id : 0,
      FechaDesde: prevProps.detalle.FechaDesde
        ? prevProps.detalle.FechaDesde
        : "",
      FechaHasta: prevProps.detalle.FechaHasta
        ? prevProps.detalle.FechaHasta
        : "",
      ColaboradorId: prevProps.detalle.ColaboradorId
        ? prevProps.detalle.ColaboradorId
        : 0,
      ColaboradorNombres: prevProps.detalle.ColaboradorNombres
        ? prevProps.detalle.ColaboradorNombres
        : "",
      ColaboradorRubroId: prevProps.detalle.ColaboradorRubroId
        ? prevProps.detalle.ColaboradorRubroId
        : 0,
      DiasLaborados: prevProps.detalle.DiasLaborados
        ? prevProps.detalle.DiasLaborados
        : 0,
      HorasLaboradas: prevProps.detalle.HorasLaboradas
        ? prevProps.detalle.HorasLaboradas
        : 0,
      EsViatico: prevProps.detalle.EsViatico
        ? prevProps.detalle.EsViatico
        : false,
      Certificado: prevProps.detalle.Certificado
        ? prevProps.detalle.Certificado
        : false,

      CreationTime: prevProps.detalle.CreationTime
        ? prevProps.detalle.CreationTime
        : new Date().toISOString(),
      CreatorUserId: prevProps.detalle.CreatorUserId
        ? prevProps.detalle.CreatorUserId
        : null,
    }
    this.setState({
      data: updatedValues,
    })
  }

  render() {
    return (
      <div>
        <div className="row">
          <div className="col">
            <Button
              label="Colaborador"
              className="p-button-outlined"
              onClick={() => this.onShowColaboradorModal()}
              icon="pi pi-users"
            />
          </div>
          <div className="col">
            <p>
              <b>Colaborador:</b> {this.state.data.ColaboradorNombres}
            </p>
          </div>
        </div>

        <div className="row">
          <div className="col">
            <Field
              name="FechaDesde"
              value={this.state.data.FechaDesde}
              label="Fecha Desde"
              type="date"
              onChange={this.handleChange}
              error={this.state.errors.FechaDesde}
              edit={true}
              readOnly={false}
            />
          </div>

          <div className="col">
            <Field
              name="FechaHasta"
              value={this.state.data.FechaHasta}
              label="Fecha Hasta"
              type="date"
              onChange={this.handleChange}
              error={this.state.errors.FechaHasta}
              edit={true}
              readOnly={false}
            />
          </div>
        </div>

        <div className="row">
          <div className="col-6">
            <Field
              name="HorasLaboradas"
              value={this.state.data.HorasLaboradas}
              label="Horas Laboradas"
              type="text"
              onChange={this.handleChange}
              error={this.state.errors.HorasLaboradas}
              readOnly={false}
              edit={true}
            />
          </div>
          <div className="col-6">
            <Field
              name="DiasLaborados"
              value={this.state.data.DiasLaborados}
              label="Días Laborados"
              type="text"
              onChange={this.handleChange}
              error={this.state.errors.DiasLaborados}
              readOnly={false}
              edit={true}
            />
          </div>
        </div>

        <div className="rol">
          <div className="col">
            <Fragment>
              <Checkbox
                inputId="cb1"
                checked={this.state.data.EsViatico}
                onChange={(e) => this.handleChange(e)}
                name="EsViatico"
              />
              <label htmlFor="cb1" className="p-checkbox-label">
                Es Viatico
              </label>
            </Fragment>
          </div>
          <div className="col"></div>
        </div>

        <Dialog
          header="Colaborador"
          visible={this.state.showColaboradorModal}
          style={{ width: "830px" }}
          onHide={this.onHideColaboradorModal}
        >
          <BuscarColaboradorContainer
            ref={this.busquedaRef}
            seleccionarColaborador={this.seleccionarColaborador}
            unlockScreen={this.props.unlockScreen}
            blockScreen={this.props.blockScreen}
            showSuccess={this.props.showSuccess}
            showWarn={this.props.showWarn}
          />
        </Dialog>

        <hr />
        <div
          className="row"
          style={{ marginTop: "0.4em", marginLeft: "0.1em" }}
        >
          <Button
            label="Guardar"
            className="p-button-outlined"
            onClick={() => this.handleSubmit()}
            icon="pi pi-save"
          />
          <Button
            style={{ marginLeft: "0.4em" }}
            label="Calculas días"
            className="p-button-outlined"
            onClick={() => this.consultarDiasLaborados()}
            icon="pi pi-tablet"
          />
          <Button
            style={{ marginLeft: "0.4em" }}
            label="Cancelar"
            className="p-button-outlined"
            onClick={() => this.props.onHideFormulario()}
            icon="pi pi-ban"
          />
        </div>
      </div>
    )
  }

  handleSubmit = () => {
    if (!this.isValid()) {
      this.showWarn(FRASE_ERROR_VALIDACIONES, "Validaciones")
      return
    }

    this.props.blockScreen()
    let url = ""
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA}`
    if (this.state.data.Id > 0) {
      url += "/EditarDetalleIndirectoIngenieriaAsync"
    } else {
      url += "/CrearDetalleIndirectoIngenieriaAsync"
    }

    console.log(url)

    let data = Object.assign({}, this.state.data)
    const formData = new FormData()
    for (var key in data) {
      if (data[key] !== null) formData.append(key, data[key])
      else formData.append(key, "")
    }

    console.log("Enviando")

    http
      .post(url, formData)
      .then((response) => {
        let data = response.data
        if (data.success === true) {
          if (this.state.data.Id > 0) {
            this.props.showSuccess(FRASE_DETALLE_INDIRECTO_ACTUALIZADO)
          } else {
            this.props.showSuccess(FRASE_DETALLE_INDIRECTO_CREADO)
          }
          this.props.obtenerDetallesIndirectos()
          this.props.onHideFormulario(true)
        } else {
          var message = data.message
          this.showWarn(message)
          this.props.unlockScreen()
        }
        this.props.unlockScreen()
      })
      .catch((error) => {
        console.log(error)
        this.showWarn(FRASE_ERROR_GLOBAL)
        this.props.unlockScreen()
      })
  }

  seleccionarColaborador = (colaborador) => {
    const updatedData = {
      ...this.state.data,
    }
    updatedData["ColaboradorId"] = colaborador.Id
    updatedData["ColaboradorNombres"] = colaborador.nombres_apellidos
    this.props.actualizardetalleSeleccionado(updatedData)
    this.setState(
      {
        data: updatedData,
        colaborador,
        showColaboradorModal: false,
      },
      this.resetColaboradorSearch
    )
  }

  isValid = () => {
    const errors = {}
    let colaboradorId = this.state.data.ColaboradorId
    let fechaInicio = this.state.data.FechaDesde
    let fechaFin = this.state.data.FechaHasta

    if (colaboradorId === 0) {
      errors.ColaboradorId = "El Colaborador es requerido"
    }

    if (fechaInicio === "") {
      errors.FechaDesde = "El campo Fecha Desde es requerido"
    }

    if (fechaFin === "") {
      errors.FechaHasta = "El campo Fecha Hasta es requerido"
    }

    this.setState({ errors })
    return Object.keys(errors).length === 0
  }

  handleChange = (event) => {
    console.log(event.target)
    const target = event.target
    const value = target.type === "checkbox" ? target.checked : target.value
    const name = target.name
    const updatedData = {
      ...this.state.data,
    }
    updatedData[name] = value
    this.props.actualizardetalleSeleccionado(updatedData)
    this.setState({
      data: updatedData,
    })
  }

  consultarDiasLaborados = () => {
    let fechaDesde = this.state.data.FechaDesde
    let fechaHasta = this.state.data.FechaHasta
    let colaboradorId = this.state.data.ColaboradorId

    if (fechaDesde != "" && fechaHasta != "" && colaboradorId > 0) {
      this.props.blockScreen()
      var self = this
      Promise.all([this.promiseObtenerDiasLaborados()])
        .then(function ([diasLaboradores]) {
          let diasLaboradosData = diasLaboradores.data
          if (diasLaboradosData.success === true) {
            const updatedData = {
              ...self.state.data,
            }
            updatedData["DiasLaborados"] = diasLaboradosData.contador
            updatedData["HorasLaboradas"] = diasLaboradosData.contador * 8
            self.props.actualizardetalleSeleccionado(updatedData)
            self.setState({
              data: updatedData,
            })
          } else {
            var message = diasLaboradosData.message
            self.props.showWarn(message)
          }
          self.props.unlockScreen()
        })
        .catch((error) => {
          self.props.unlockScreen()
          console.log(error)
        })
    }
  }

  promiseObtenerDiasLaborados = () => {
    let url = ""
    let params = `?fechaInicio=${this.state.data.FechaDesde}&fechaFin=${this.state.data.FechaHasta}&colaboradorId=${this.state.data.ColaboradorId}`
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA}/CalcularDiasLaborados${params}`
    return http.get(url)
  }

  onChangeDropdown = (name, value) => {
    const updatedData = {
      ...this.state.data,
    }
    updatedData[name] = value
    this.props.actualizardetalleSeleccionado(updatedData)
    this.setState({
      data: updatedData,
    })
  }

  showWarn = (displayMessage, type = "Error") => {
    this.setState({ displayMessage }, () => this.warn(type))
  }

  warn = (type) => {
    abp.notify.error(this.state.displayMessage, type)
  }

  onHideColaboradorModal = () => {
    this.setState({ showColaboradorModal: false }, this.resetColaboradorSearch)
  }

  onShowColaboradorModal = () => {
    this.setState({ showColaboradorModal: true })
  }

  resetColaboradorSearch = () => {
    this.busquedaRef.current.resetValues()
  }
}
