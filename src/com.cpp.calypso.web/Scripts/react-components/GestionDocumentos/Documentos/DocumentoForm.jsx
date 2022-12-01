import { Button } from "primereact-v2/button"
import Field from "../../Base/Field-v2"
import React from "react"
import config from "../../Base/Config"

import http from "../../Base/HttpService"
import {
  CONTROLLER_CARPETA,
  CONTROLLER_DOCUMENTO,
  FRASE_DOCUMENTO_CREADO,
  FRASE_DOCUMENTO_ACTUALIZADO,
  FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
  MODULO_DOCUMENTOS,
} from "../../Base/Strings"
import { Checkbox } from "primereact-v2/checkbox"
import { Fragment } from "react"

export class DocumentoForm extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      data: {
        Id: props.documento.Id ? props.documento.Id : 0,
        Codigo: props.documento.Codigo ? props.documento.Codigo : "",
        Nombre: props.documento.Nombre ? props.documento.Nombre : "",
        DocumentoPadreId: props.documento.DocumentoPadreId ? props.documento.DocumentoPadreId : null,
        CarpetaId: props.contratoId ? props.contratoId : 0,
        CantidadPaginas: props.documento.CantidadPaginas
          ? props.documento.CantidadPaginas
          : 0,
          orden: props.documento.orden
          ? props.documento.orden
          : 1,
        TipoDocumentoId: props.documento.TipoDocumentoId
          ? props.documento.TipoDocumentoId
          : 0,
        EsImagen: props.documento.EsImagen ? props.documento.EsImagen : false,
        Imagen: props.documento.Imagen ? props.documento.Imagen : "",
        CreationTime: props.documento.CreationTime
          ? props.documento.CreationTime
          : "",
        CreatorUserId: props.documento.CreatorUserId
          ? props.documento.CreatorUserId
          : null,
      },
      errors: {},
      displayMessage: "",
      keyUpload: 98,
    }
  }

  componentWillReceiveProps(prevProps) {
    let updatedValues = {
      Id: prevProps.documento.Id ? prevProps.documento.Id : 0,
      Codigo: prevProps.documento.Codigo ? prevProps.documento.Codigo : "",
      Nombre: prevProps.documento.Nombre ? prevProps.documento.Nombre : "",
      DocumentoPadreId: prevProps.documento.DocumentoPadreId ? prevProps.documento.DocumentoPadreId : null,
      CarpetaId: prevProps.contratoId
        ? prevProps.contratoId
        : 0,
      CantidadPaginas: prevProps.documento.CantidadPaginas
        ? prevProps.documento.CantidadPaginas
        : 0,
        orden: prevProps.documento.orden
        ? prevProps.documento.orden
        : 1,
      TipoDocumentoId: prevProps.documento.TipoDocumentoId
        ? prevProps.documento.TipoDocumentoId
        : 0,
      EsImagen: prevProps.documento.EsImagen
        ? prevProps.documento.EsImagen
        : false,
      Imagen: prevProps.documento.Imagen ? prevProps.documento.Imagen : "",
      CreationTime: prevProps.documento.CreationTime
        ? prevProps.documento.CreationTime
        : "",
      CreatorUserId: prevProps.documento.CreatorUserId
        ? prevProps.documento.CreatorUserId
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
              name="Nombre"
              value={this.state.data.Nombre}
              label="* Nombre"
              type="text"
              edit={true}
              readOnly={false}
              onChange={this.handleChange}
              error={this.state.errors.Nombre}
            />
          </div>
        </div>

        <div className="row">
          <div className="col">
            <Field
              name="TipoDocumentoId"
              label="* Tipo Documento"
              type="select"
              options={this.props.catalogoTipoDocumento}
              edit={true}
              readOnly={false}
              value={this.state.data.TipoDocumentoId}
              onChange={this.onChangeDropdown}
              error={this.state.errors.TipoDocumentoId}
              placeholder="Seleccionar..."
            />
          </div>
          <div className="col">
            <Field
              name="CantidadPaginas"
              value={this.state.data.CantidadPaginas}
              label="* Número Páginas (Doc. Físico)"
              type="NUMBER"
              edit={true}
              readOnly={false}
              onChange={this.handleChange}
              error={this.state.errors.CantidadPaginas}
            />
          </div>
        </div>

        <div className="row">
          <div className="col">
            <Field
              name="DocumentoPadreId"
              label="Pertenece a"
              type="select"
              options={this.props.documentosAnexos}
              edit={true}
              readOnly={false}
              value={this.state.data.DocumentoPadreId}
              onChange={this.onChangeDropdown}
              error={this.state.errors.DocumentoPadreId}
              placeholder="Seleccionar..."
            />
          </div>
        </div>

        <div className="row align-items-center">
          <div className="col">
            <Fragment>
              <Checkbox
                inputId="cb1"
                checked={this.state.data.EsImagen}
                onChange={(e) => this.handleChange(e)}
                name="EsImagen"
              />
              <label htmlFor="cb1" className="p-checkbox-label">
                Es imagen?
              </label>
            </Fragment>
          </div>

          <div className="col">
            <Field
              name="orden"
              value={this.state.data.orden}
              label="orden"
              type="NUMBER"
              edit={true}
              readOnly={false}
              onChange={this.handleChange}
              error={this.state.errors.orden}
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
    url = `${config.apiUrl}${MODULO_DOCUMENTOS}/${CONTROLLER_DOCUMENTO}`
    if (this.state.data.Id > 0) {
      url += "/EditarDocumentoAsync"
    } else {
      url += "/CrearDocumentoAsync"
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
            this.props.showSuccess(FRASE_DOCUMENTO_ACTUALIZADO)
          } else {
            this.props.showSuccess(FRASE_DOCUMENTO_CREADO)
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
    let tipoDocumentoId = this.state.data.TipoDocumentoId
    let codigo = this.state.data.Codigo
    let nombre = this.state.data.Nombre
    let cantidadPaginas = this.state.data.CantidadPaginas


    if (tipoDocumentoId === 0) {
      errors.TipoDocumentoId = "El campo Tipo Documento es requerido"
    }

    if (codigo === "") {
      errors.Codigo = "El campo Código es requerido"
    } else if (codigo.length > 20) {
      errors.Codigo = "Máximo 20 caracteres"
    }

    if (nombre === "") {
      errors.Nombre = "El campo Nombre es requerido"
    } else if (nombre.length > 100) {
      errors.Nombre = "Máximo 100 caracteres"
    }

    if (cantidadPaginas.length > 4) {
      errors.CantidadPaginas = "Máximo 4 dígitos"
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
    this.props.actualizarDocumentoSeleccionado(updatedData)
    this.setState({
      data: updatedData,
    })
  }

  onChangeDropdown = (name, value) => {
    const updatedData = {
      ...this.state.data,
    }
    updatedData[name] = value
    this.props.actualizarDocumentoSeleccionado(updatedData)
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
