import { Button } from "primereact-v2/button"
import React from "react"
import config from "../../Base/Config"
import Field from "../../Base/Field-v3"
import http from "../../Base/HttpService"
import {
  CONTROLLER_FERIADOS,
  CONTROLLER_PLANIFICACION_TIMESHEET,
  FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
  FRASE_FERIADO_ACTUALIZADA,
  FRASE_PLANIFICACION_ACTUALIZADA,
  FRASE_PLANIFICACION_CREADA,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"

export class PlanificacionTimesheetForm extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      data: {
        Id: props.planificacion.Id ? props.planificacion.Id : 0,
        Fecha: props.planificacion.Fecha ? props.planificacion.Fecha : "",
        TipoPlanificacion: props.planificacion.TipoPlanificacion
          ? props.planificacion.TipoPlanificacion
          : "",
        Descripcion: props.planificacion.Descripcion
          ? props.planificacion.Descripcion
          : "",
        CreationTime: props.planificacion.CreationTime
          ? props.planificacion.CreationTime
          : new Date().toISOString(),
        CreatorUserId: props.planificacion.CreatorUserId
          ? props.planificacion.CreatorUserId
          : null,
      },
      errors: {},
      displayMessage: "",
      keyUpload: 98,
      catalogoTipoPlanificaciones: [
        { label: "Corte Ingeniería", value: 0 },
        { label: "Envío de TS para RS", value: 1 },
        { label: "RS Ingeniería", value: 2 },
        { label: "Corte ingeniería mensual", value: 3 },
        { label: "RS Ingeniería SC mensual", value: 4 },
        { label: "C. Certificación mensual", value: 5 },
      ],
    }
  }

  componentWillReceiveProps(prevProps) {
    console.log(prevProps.planificacion.TipoPlanificacion)
    let updatedValues = {
      Id: prevProps.planificacion.Id ? prevProps.planificacion.Id : 0,
      Fecha: prevProps.planificacion.Fecha ? prevProps.planificacion.Fecha : "",
      TipoPlanificacion: prevProps.planificacion.TipoPlanificacion !== undefined
        ? prevProps.planificacion.TipoPlanificacion
        : "",
      Descripcion: prevProps.planificacion.Descripcion
        ? prevProps.planificacion.Descripcion
        : "",
      CreationTime: prevProps.planificacion.CreationTime
        ? prevProps.planificacion.CreationTime
        : new Date().toISOString(),
      CreatorUserId: prevProps.planificacion.CreatorUserId
        ? prevProps.planificacion.CreatorUserId
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
              name="Fecha"
              value={this.state.data.Fecha}
              label="Fecha"
              type="date"
              onChange={this.handleChange}
              error={this.state.errors.Fecha}
              edit={true}
              readOnly={false}
            />
          </div>

          <div className="col">
            <Field
              name="TipoPlanificacion"
              label="Tipo"
              type="select"
              options={this.state.catalogoTipoPlanificaciones}
              edit={true}
              readOnly={false}
              value={this.state.data.TipoPlanificacion}
              onChange={this.onChangeDropdown}
              error={this.state.errors.TipoPlanificacion}
              placeholder="Seleccionar..."
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
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PLANIFICACION_TIMESHEET}`
    if (this.state.data.Id > 0) {
      url += "/EditarPlanificacionAsync"
    } else {
      url += "/CrearPlanificacionAsync"
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
            this.props.showSuccess(FRASE_PLANIFICACION_ACTUALIZADA)
          } else {
            this.props.showSuccess(FRASE_PLANIFICACION_CREADA)
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
    let tipoPlanificacion = this.state.data.TipoPlanificacion

    if (descripcion === "") {
      errors.Descripcion = "La descripción es requerida"
    }
    if (tipoPlanificacion === "") {
      errors.TipoPlanificacion = "El campo Tipo es requerido"
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
    this.props.actualizarPlanificacionSeleccionada(updatedData)
    this.setState({
      data: updatedData,
    })
  }

  onChangeDropdown = (name, value) => {
    const updatedData = {
      ...this.state.data,
    }
    let tipoPlanificacion = this.state.catalogoTipoPlanificaciones.filter(
      o => o.value === value
    );
    updatedData["Descripcion"] = tipoPlanificacion[0].label;
    updatedData[name] = value
    this.props.actualizarPlanificacionSeleccionada(updatedData)
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
