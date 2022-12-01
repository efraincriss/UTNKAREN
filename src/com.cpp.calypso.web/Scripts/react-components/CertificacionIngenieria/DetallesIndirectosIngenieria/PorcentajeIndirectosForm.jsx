import { Button } from "primereact-v2/button"
import React from "react"
import config from "../../Base/Config"
import Field from "../../Base/Field-v2"
import http from "../../Base/HttpService"
import {
  CONTROLLER_PORCENTAJE_INDIRECTOS_INGENIERIA,
  FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
  FRASE_PORCENTAJE_INDIRECTO_ACTUALIZADO,
  FRASE_PORCENTAJE_INDIRECTO_CREADO,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"

export class PorcentajeIndirectosForm extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      data: {
        Id: props.porcentaje.Id ? props.porcentaje.Id : 0,
        DetalleIndirectosIngenieriaId: props.porcentaje
          .DetalleIndirectosIngenieriaId
          ? props.porcentaje.DetalleIndirectosIngenieriaId
          : 0,
        ContratoId: props.porcentaje.ContratoId
          ? props.porcentaje.ContratoId
          : 0,
        PorcentajeIndirecto: props.porcentaje.PorcentajeIndirecto
          ? props.porcentaje.PorcentajeIndirecto
          : "",
        Horas: props.porcentaje.Horas ? props.porcentaje.Horas : 0,
        CreationTime: props.porcentaje.CreationTime
          ? props.porcentaje.CreationTime
          : new Date().toISOString(),
        CreatorUserId: props.porcentaje.CreatorUserId
          ? props.porcentaje.CreatorUserId
          : null,
      },
      errors: {},
      displayMessage: "",
      keyUpload: 98,
      porcentajeAnterior: props.porcentaje.PorcentajeIndirecto
        ? props.porcentaje.PorcentajeIndirecto
        : 0,
    }
    this.busquedaRef = React.createRef()
  }

  componentWillReceiveProps(prevProps) {
    let updatedValues = {
      Id: prevProps.porcentaje.Id ? prevProps.porcentaje.Id : 0,
      DetalleIndirectosIngenieriaId: prevProps.porcentaje
        .DetalleIndirectosIngenieriaId
        ? prevProps.porcentaje.DetalleIndirectosIngenieriaId
        : 0,
      ContratoId: prevProps.porcentaje.ContratoId
        ? prevProps.porcentaje.ContratoId
        : 0,
      PorcentajeIndirecto: prevProps.porcentaje.PorcentajeIndirecto
        ? prevProps.porcentaje.PorcentajeIndirecto
        : "",
      Horas: prevProps.porcentaje.Horas ? prevProps.porcentaje.Horas : 0,
      CreationTime: prevProps.porcentaje.CreationTime
        ? prevProps.porcentaje.CreationTime
        : new Date().toISOString(),
      CreatorUserId: prevProps.porcentaje.CreatorUserId
        ? prevProps.porcentaje.CreatorUserId
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
          <div className="col-6">
            <Field
              name="ContratoId"
              label="Contrato"
              type="select"
              options={this.props.catalogoContratos}
              edit={true}
              readOnly={false}
              value={this.state.data.ContratoId}
              onChange={this.onChangeDropdown}
              error={this.state.errors.ContratoId}
              placeholder="Seleccionar..."
            />
          </div>
          <div className="col-6">
            <Field
              name="PorcentajeIndirecto"
              value={this.state.data.PorcentajeIndirecto}
              label="Porcentaje"
              type="text"
              onChange={this.handleChange}
              error={this.state.errors.PorcentajeIndirecto}
              readOnly={false}
              edit={true}
            />
          </div>
        </div>

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
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PORCENTAJE_INDIRECTOS_INGENIERIA}`
    if (this.state.data.Id > 0) {
      url += "/EditarPorcentajeIndirectoIngenieriaAsync"
    } else {
      url += "/CrearPorcentajeIndirectoIngenieriaAsync"
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
            this.props.showSuccess(FRASE_PORCENTAJE_INDIRECTO_ACTUALIZADO)
          } else {
            this.props.showSuccess(FRASE_PORCENTAJE_INDIRECTO_CREADO)
          }
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

  onChangeDropdown = (name, value) => {
    const updatedData = {
      ...this.state.data,
    }
    updatedData[name] = value
    this.props.actualizarPorcentajeSeleccionado(updatedData)
    this.setState({
      data: updatedData,
    })
  }

  isValid = () => {
    const errors = {}
    let contratoId = this.state.data.ContratoId
    let porcentaje = this.state.data.PorcentajeIndirecto
    let horas = this.state.data.Horas

    if (contratoId === 0) {
      errors.ContratoId = "El contrato es requerido"
    }

    if (porcentaje === "") {
      errors.PorcentajeIndirecto = "El campo porcentaje es requerido"
    }

    let totalRestante = 0
    /* Si es edicion debo restar el total de ese porcentaje */
    if (this.state.data.Id > 0) {
      totalRestante = (
        100 -
        this.props.totalPorcentaje +
        this.state.porcentajeAnterior
      ).toPrecision(4)
    } else {
      totalRestante = (100 - this.props.totalPorcentaje).toPrecision(4)
    }

    console.log("Porcentaje:", porcentaje)
    console.log("Restante:", totalRestante)
    console.log("Calculo:", parseFloat(porcentaje) > parseFloat(totalRestante))
    if (parseFloat(porcentaje) > parseFloat(totalRestante)) {
      console.log("Entro")
      errors.PorcentajeIndirecto =
        "El porcentaje no puede ser mayor a " + totalRestante.toString()
    }

    if (porcentaje > 100) {
      errors.PorcentajeIndirecto = "El porcentaje no puede ser mayor a 100"
    }

    if (porcentaje < 0) {
      errors.PorcentajeIndirecto = "El porcentaje no puede ser menor a 0"
    }

    this.setState({ errors })
    return Object.keys(errors).length === 0
  }

  handleChange = (event) => {
    const target = event.target
    const value = target.type === "checkbox" ? target.checked : target.value
    const name = target.name
    const updatedData = {
      ...this.state.data,
    }
    updatedData[name] = value
    this.props.actualizarPorcentajeSeleccionado(updatedData)
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
}
