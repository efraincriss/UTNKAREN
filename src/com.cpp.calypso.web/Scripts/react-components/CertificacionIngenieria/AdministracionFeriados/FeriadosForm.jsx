import { Button } from "primereact-v2/button"
import React from "react"
import config from "../../Base/Config"
import Field from "../../Base/Field-v2"
import http from "../../Base/HttpService"
import {
  CONTROLLER_FERIADOS,
  FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
  FRASE_FERIADO_ACTUALIZADA,
  FRASE_FERIADO_CREADA,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"

export class FeriadoForm extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      data: {
        Id: props.feriado.Id ? props.feriado.Id : 0,
        FechaInicio: props.feriado.FechaInicio ? props.feriado.FechaInicio : "",
        FechaFin: props.feriado.FechaFin ? props.feriado.FechaFin : "",
        Descripcion: props.feriado.Descripcion ? props.feriado.Descripcion : "",
        Horas: props.feriado.Horas ? props.feriado.Horas : 8,
        CreationTime: props.feriado.CreationTime
          ? props.feriado.CreationTime
          : new Date().toISOString(),
        CreatorUserId: props.feriado.CreatorUserId
          ? props.feriado.CreatorUserId
          : null,
      },
      errors: {},
      displayMessage: "",
      keyUpload: 98,
    }
  }

  componentWillReceiveProps(prevProps) {
    let updatedValues = {
      Id: prevProps.feriado.Id ? prevProps.feriado.Id : 0,
      FechaInicio: prevProps.feriado.FechaInicio
        ? prevProps.feriado.FechaInicio
        : "",
      FechaFin: prevProps.feriado.FechaFin ? prevProps.feriado.FechaFin : "",
      Descripcion: prevProps.feriado.Descripcion
        ? prevProps.feriado.Descripcion
        : "",
      Horas: prevProps.feriado.Horas ? prevProps.feriado.Horas : "",
      CreationTime: prevProps.feriado.CreationTime
        ? prevProps.feriado.CreationTime
        : new Date().toISOString(),
      CreatorUserId: prevProps.feriado.CreatorUserId
        ? prevProps.feriado.CreatorUserId
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
            <Field
              name="FechaInicio"
              value={this.state.data.FechaInicio}
              label="Fecha Inicio"
              type="date"
              onChange={this.handleChange}
              error={this.state.errors.FechaInicio}
              edit={true}
              readOnly={false}
            />
          </div>

          <div className="col">
            <Field
              name="FechaFin"
              value={this.state.data.FechaFin}
              label="Fecha Fin"
              type="date"
              onChange={this.handleChange}
              error={this.state.errors.FechaFin}
              edit={true}
              readOnly={false}
            />
          </div>
        </div>
        <div className="row">
          <div className="col">
            <Field
              name="Horas"
              value={this.state.data.Horas}
              label="Horas"
              type="text"
              onChange={this.handleChange}
              error={this.state.errors.Horas}
              edit={true}
              readOnly={false}
            />
          </div>
        </div>
        <div className="row">
          <div className="col">
            <Field
              name="Descripcion"
              value={this.state.data.Descripcion}
              label="Descripción"
              type="textarea"
              onChange={this.handleChange}
              error={this.state.errors.Descripcion}
              edit={true}
              readOnly={false}
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
          {this.state.data.Id > 0 && (
            <Button
              style={{ marginLeft: "0.4em" }}
              label="Eliminar"
              className="p-button-danger p-button-outlined"
              onClick={() =>
                this.props.mostrarConfirmacionParaEliminar(this.state.data)
              }
              icon="pi pi-save"
            />
          )}
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
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_FERIADOS}`
    if (this.state.data.Id > 0) {
      url += "/EditarFeriadoAsync"
    } else {
      url += "/CrearFeriadoAsync"
    }

    let data = Object.assign({}, this.state.data)
    const formData = new FormData()
    for (var key in data) {
      if (data[key] !== null) formData.append(key, data[key])
      else formData.append(key, "")
    }

    http
      .post(url, formData)
      .then((response) => {
        let data = response.data
        if (data.success === true) {
          if (this.state.data.Id > 0) {
            this.props.showSuccess(FRASE_FERIADO_ACTUALIZADA)
          } else {
            this.props.showSuccess(FRASE_FERIADO_CREADA)
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

  isValid = () => {
    const errors = {}
    let descripcion = this.state.data.Descripcion

    if (descripcion === "") {
      errors.Descripcion = "La descripción del feriado es requerida"
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
    this.props.actualizarFeriadoSeleccionado(updatedData)
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
