import { Button } from "primereact-v2/button"
import React from "react"
import config from "../../Base/Config"
import Field from "../../Base/Field-v2"
import http from "../../Base/HttpService"
import {
  CONTROLLER_CARPETA,
  FRASE_CONTRATO_ACTUALIZADO,
  FRASE_CONTRATO_CREADO,
  FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
  MODULO_DOCUMENTOS,
} from "../../Base/Strings"
import { Checkbox } from "primereact-v2/checkbox"
import { Fragment } from "react"

export class CarpetaForm extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      data: {
        Id: props.carpeta.Id ? props.carpeta.Id : 0,
        Codigo: props.carpeta.Codigo ? props.carpeta.Codigo : "",
        NombreCorto: props.carpeta.NombreCorto ? props.carpeta.NombreCorto : "",
        NombreCompleto: props.carpeta.NombreCompleto
          ? props.carpeta.NombreCompleto
          : "",
        Descripcion: props.carpeta.Descripcion ? props.carpeta.Descripcion : "",
        EstadoId: props.carpeta.EstadoId ? props.carpeta.EstadoId : 0,
        Publicado: props.carpeta.Publicado ? props.carpeta.Publicado : false,
        CreationTime: props.carpeta.CreationTime
          ? props.carpeta.CreationTime
          : "",
        CreatorUserId: props.carpeta.CreatorUserId
          ? props.carpeta.CreatorUserId
          : null,
      },
      errors: {},
      displayMessage: "",
      keyUpload: 98,
    }
  }

  componentWillReceiveProps(prevProps) {
    let updatedValues = {
      Id: prevProps.carpeta.Id ? prevProps.carpeta.Id : 0,
      Codigo: prevProps.carpeta.Codigo ? prevProps.carpeta.Codigo : "",
      NombreCorto: prevProps.carpeta.NombreCorto
        ? prevProps.carpeta.NombreCorto
        : "",
      NombreCompleto: prevProps.carpeta.NombreCompleto
        ? prevProps.carpeta.NombreCompleto
        : "",
      Descripcion: prevProps.carpeta.Descripcion
        ? prevProps.carpeta.Descripcion
        : "",
      EstadoId: prevProps.carpeta.EstadoId ? prevProps.carpeta.EstadoId : 0,
      Publicado: prevProps.carpeta.Publicado
        ? prevProps.carpeta.Publicado
        : false,
      CreationTime: prevProps.carpeta.CreationTime
        ? prevProps.carpeta.CreationTime
        : "",
      CreatorUserId: prevProps.carpeta.CreatorUserId
        ? prevProps.carpeta.CreatorUserId
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
              name="Codigo"
              value={this.state.data.Codigo}
              label="* Código"
              edit={true}
              readOnly={this.state.data.Id > 0}
              type="text"
              onChange={this.handleChange}
              error={this.state.errors.Codigo}
            />
          </div>

          <div className="col">
            <Field
              name="NombreCorto"
              value={this.state.data.NombreCorto}
              label="* Nombre Corto"
              type="text"
              edit={true}
              readOnly={false}
              onChange={this.handleChange}
              error={this.state.errors.NombreCorto}
            />
          </div>
        </div>

        <div className="row">
          <div className="col">
            <Field
              name="NombreCompleto"
              value={this.state.data.NombreCompleto}
              label="* Nombre Completo"
              edit={true}
              readOnly={false}
              type="textarea"
              onChange={this.handleChange}
              error={this.state.errors.NombreCompleto}
            />
          </div>
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

        <div className="row align-items-center">
          <div className="col">
            <Field
              name="EstadoId"
              label="* Estado"
              type="select"
              options={this.props.catalogoEstadoContrato}
              edit={true}
              readOnly={false}
              value={this.state.data.EstadoId}
              onChange={this.onChangeDropdown}
              error={this.state.errors.EstadoId}
              placeholder="Seleccionar..."
            />
          </div>

          <div className="col">
            {this.state.data.Id > 0 && (
              <Fragment>
                <Checkbox
                  inputId="cb1"
                  checked={this.state.data.Publicado}
                  onChange={(e) => this.handleChange(e)}
                  name="Publicado"
                />
                <label htmlFor="cb1" className="p-checkbox-label">
                  Publicado
                </label>
              </Fragment>
            )}
          </div>
        </div>

        <hr />
        <div className="row" style={{ marginTop: "0.4em", "marginLeft": "0.1em" }}>
          <Button
            label="Guardar"
            className="p-button-outlined"
            onClick={() => this.handleSubmit()}
            icon="pi pi-save"
          />
          <Button
            style={{marginLeft: "0.4em"}}
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
    url = `${config.apiUrl}${MODULO_DOCUMENTOS}/${CONTROLLER_CARPETA}`
    if (this.state.data.Id > 0) {
      url += "/EditarCarpetaAsync"
    } else {
      url += "/CreateAsync"
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
            this.props.showSuccess(FRASE_CONTRATO_ACTUALIZADO)
          } else {
            this.props.showSuccess(FRASE_CONTRATO_CREADO)
          }
          this.props.onHideFormulario(true)
        } else {
          var message = $.fn.responseAjaxErrorToString(data)
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
    let estadoId = this.state.data.EstadoId
    let codigo = this.state.data.Codigo
    let nombreCorto = this.state.data.NombreCorto
    let nombreCompleto = this.state.data.NombreCompleto
    let descripcion = this.state.data.Descripcion

    if (estadoId === 0) {
      errors.EstadoId = "El campo Estado es requerido"
    }

    if (codigo === "") {
      errors.Codigo = "El campo Código es requerido"
    } else if (codigo.length > 20) {
      errors.Codigo = "Máximo 20 caracteres"
    }

    if (nombreCorto === "") {
      errors.NombreCorto = "El campo Nombre Corto es requerido"
    } else if (nombreCorto.length > 50) {
      errors.NombreCorto = "Máximo 50 caracteres"
    }

    if (nombreCompleto === "") {
      errors.NombreCompleto = "El campo Nombre Completo es requerido"
    } else if (nombreCompleto.length > 300) {
      errors.NombreCompleto = "Máximo 300 caracteres"
    }

    if (descripcion.length > 3000) {
      errors.Descripcion = "Máximo 3000 caracteres"
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
    this.props.actualizarCarpetaSeleccionada(updatedData)
    this.setState({
      data: updatedData,
    })
  }

  onChangeDropdown = (name, value) => {
    const updatedData = {
      ...this.state.data,
    }
    updatedData[name] = value
    this.props.actualizarCarpetaSeleccionada(updatedData)
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
