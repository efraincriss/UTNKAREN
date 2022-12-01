import { Button } from "primereact-v2/button"
import React from "react"
import config from "../../Base/Config"
import Field from "../../Base/Field-v2"
import http from "../../Base/HttpService"
import {
    CONTROLLER_PORCENTAJE_AVANCE_INGENIERIA,
    FRASE_AVANCE_INGENIERIA_ACTUALIZADO,
    FRASE_AVANCE_INGENIERIA_CREADO,
    FRASE_ERROR_GLOBAL,
    FRASE_ERROR_VALIDACIONES, MODULO_CERTIFICACION_INGENIERIA
} from "../../Base/Strings"

export class AvanceIngenieriaForm extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      data: {
        Id: props.avance.Id ? props.avance.Id : 0,
        ProyectoId: props.avance.ProyectoId ? props.avance.ProyectoId : 0,
        FechaAvance: props.avance.FechaAvance ? props.avance.FechaAvance : "",
        CatalogoProcentajeId: props.avance.CatalogoProcentajeId
          ? props.avance.CatalogoProcentajeId
          : 0,
        ValorPorcentaje: props.avance.ValorPorcentaje
          ? props.avance.ValorPorcentaje
          : "",
        CreationTime: props.avance.CreationTime
          ? props.avance.CreationTime
          : new Date().toISOString(),

        CreatorUserId: props.avance.CreatorUserId
          ? props.avance.CreatorUserId
          : null,
      },
      errors: {},
      displayMessage: "",
      keyUpload: 98,
    }
    this.busquedaRef = React.createRef()
  }

  componentWillReceiveProps(prevProps) {
    let updatedValues = {
      Id: prevProps.avance.Id ? prevProps.avance.Id : 0,
      ProyectoId: prevProps.avance.ProyectoId ? prevProps.avance.ProyectoId : 0,
      FechaAvance: prevProps.avance.FechaAvance
        ? prevProps.avance.FechaAvance
        : "",
      CatalogoProcentajeId: prevProps.avance.CatalogoProcentajeId
        ? prevProps.avance.CatalogoProcentajeId
        : 0,
      ValorPorcentaje: prevProps.avance.ValorPorcentaje
        ? prevProps.avance.ValorPorcentaje
        : "",
      CreationTime: prevProps.avance.CreationTime
        ? prevProps.avance.CreationTime
        : new Date().toISOString(),
      CreatorUserId: prevProps.avance.CreatorUserId
        ? prevProps.avance.CreatorUserId
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
              filter showClear filterBy="name"
              name="ProyectoId"
              label="Proyecto"
              type="select"
              options={this.props.catalogoProyectos}
              edit={true}
              readOnly={false}
              value={this.state.data.ProyectoId}
              onChange={this.onChangeDropdown}
              error={this.state.errors.ProyectoId}
              placeholder="Seleccionar..."
            />
          </div>
          <div className="col-6">
            <Field
              name="CatalogoProcentajeId"
              label="Porcentaje"
              type="select"
              options={this.props.catalogoPorcentajes}
              edit={true}
              readOnly={false}
              value={this.state.data.CatalogoProcentajeId}
              onChange={this.onChangeDropdown}
              error={this.state.errors.CatalogoProcentajeId}
              placeholder="Seleccionar..."
            />
          </div>
        </div>

        <div className="row">
          <div className="col">
            <Field
              name="FechaAvance"
              value={this.state.data.FechaAvance}
              label="Fecha Avance"
              type="date"
              onChange={this.handleChange}
              error={this.state.errors.FechaAvance}
              edit={true}
              readOnly={false}
            />
          </div>

          <div className="col">
            <Field
              name="ValorPorcentaje"
              value={this.state.data.ValorPorcentaje}
              label="Valor Porcentaje"
              type="text"
              onChange={this.handleChange}
              error={this.state.errors.ValorPorcentaje}
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
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PORCENTAJE_AVANCE_INGENIERIA}`
    if (this.state.data.Id > 0) {
      url += "/EditarAvanceIngenieriaAsync"
    } else {
      url += "/CrearAvanceIngenieriaAsync"
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
            this.props.showSuccess(FRASE_AVANCE_INGENIERIA_ACTUALIZADO)
          } else {
            this.props.showSuccess(FRASE_AVANCE_INGENIERIA_CREADO)
          }
          this.props.obtenerAvances()
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
    let proyectoId = this.state.data.ProyectoId
    let catalogoPorcentaje = this.state.data.CatalogoProcentajeId
    let valorPorcentaje = this.state.data.ValorPorcentaje
    let fechaAvance = this.state.data.FechaAvance

    if (proyectoId === 0) {
      errors.ProyectoId = "El campo Proyecto es requerido"
    }

    if (catalogoPorcentaje === 0) {
      errors.CatalogoProcentajeId = "El campo porcentaje es requerido"
    }

    if (valorPorcentaje === "") {
      errors.ValorPorcentaje = "El campo valor es requerido"
    }

    if (fechaAvance === "") {
      errors.FechaAvance = "El campo Fecha Avance es requerido"
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
    this.props.actualizarAvanceSeleccionado(updatedData)
    this.setState({
      data: updatedData,
    })
  }

  onChangeDropdown = (name, value) => {
    const updatedData = {
      ...this.state.data,
    }
    updatedData[name] = value
    this.props.actualizarAvanceSeleccionado(updatedData)
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
