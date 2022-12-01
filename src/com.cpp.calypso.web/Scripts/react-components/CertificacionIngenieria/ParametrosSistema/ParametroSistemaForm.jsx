import { Button } from "primereact-v2/button"
import React from "react"
import config from "../../Base/Config"
import Field from "../../Base/Field-v2"
import http from "../../Base/HttpService"
import {
  CONTROLLER_COLABORADOR_INGENIERIA,
  CONTROLLER_PARAMETROS,
  FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
  FRASE_PARAMETRO_COLABORADOR_ACTUALIZADO,
  FRASE_PARAMETRO_COLABORADOR_CREADO,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"

export class ParametroSistemaForm extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      data: {
        Id: props.parametro.Id ? props.parametro.Id : 0,
        Codigo: props.parametro.Codigo ? props.parametro.Codigo : "",
        Nombre: props.parametro.Nombre ? props.parametro.Nombre : "",
        Descripcion: props.parametro.Descripcion
          ? props.parametro.Descripcion
          : "",
        Categoria: props.parametro.Categoria ? props.parametro.Categoria : 0,
        Tipo: props.parametro.Tipo ? props.parametro.Tipo : 0,
        Valor: props.parametro ? props.parametro.Valor : "",
        EsEditable: props.parametro.EsEditable
          ? props.parametro.EsEditable
          : false,
        TieneOpciones: props.parametro.TieneOpciones
          ? props.parametro.TieneOpciones
          : false,
        ModuloId: props.parametro.ModuloId ? props.parametro.ModuloId : null,
      },
      errors: {},
      displayMessage: "",
      keyUpload: 98,
    }
  }

  componentWillReceiveProps(prevProps) {
    let updatedValues = {
      Id: prevProps.parametro.Id ? prevProps.parametro.Id : 0,
      Codigo: prevProps.parametro.Codigo ? prevProps.parametro.Codigo : "",
      Nombre: prevProps.parametro.Nombre ? prevProps.parametro.Nombre : "",
      Descripcion: prevProps.parametro.Descripcion
        ? prevProps.parametro.Descripcion
        : "",
      Categoria: prevProps.parametro.Categoria
        ? prevProps.parametro.Categoria
        : 0,
      Tipo: prevProps.parametro.Tipo ? prevProps.parametro.Tipo : 0,
      Valor: prevProps.parametro ? prevProps.parametro.Valor : "",
      EsEditable: prevProps.parametro.EsEditable
        ? prevProps.parametro.EsEditable
        : false,
      TieneOpciones: prevProps.parametro.TieneOpciones
        ? prevProps.parametro.TieneOpciones
        : false,
      ModuloId: prevProps.parametro.ModuloId
        ? prevProps.parametro.ModuloId
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
            <h6 className="text-gray-700">
              <b>Nombre: </b>{" "}
              {this.props.parametro ? this.props.parametro.Nombre : ""}
            </h6>
          </div>
          <div className="col">
            <h6 className="text-gray-700">
              <b>Código: </b>{" "}
              {this.props.parametro ? this.props.parametro.Codigo : ""}
            </h6>
          </div>
        </div>

        <div className="row">
          <div className="col">
            <h6 className="text-gray-700">
              <b>Descripción: </b>{" "}
              {this.props.parametro ? this.props.parametro.Descripcion : ""}
            </h6>
          </div>
        </div>
        <div className="row">
          <div className="col">
            <h6 className="text-gray-700">
              <b>Tipo:</b>{" "}
              {this.props.parametro
                ? this.obtenerTipoParametroNombre(this.props.parametro.Tipo)
                : ""}
            </h6>
          </div>
        </div>
        <div className="row">
          <div className="col">
            {this.buildCampoValor(this.props.parametro.Tipo)}
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
            onClick={() => this.props.ocultarFormulario()}
            icon="pi pi-ban"
          />
        </div>
      </div>
    )
  }

  buildCampoValor = (tipo) => {
    if (tipo === "") {
      return <div></div>
    }

    if (tipo === 1 || tipo === 2) {
      return (
        <Field
          name="Valor"
          value={this.state.data.Valor}
          label="Valor"
          type="text"
          onChange={this.handleChange}
          error={this.state.errors.Valor}
          edit={true}
          readOnly={false}
        />
      )
    } else if (tipo === 3) {
      return (
        <Field
          name="Valor"
          label="Valor"
          labelOption=" (Si/No)"
          type="checkbox"
          value={this.obtenerValorBoolean(this.state.data.Valor)}
          edit={true}
          readOnly={false}
          error={this.state.errors.Valor}
          onChange={this.handleChange}
        />
      )
    } else if (tipo === 5) {
      return (
        <Field
          name="Valor"
          value={this.state.data.Valor}
          label="Valor"
          type="date"
          onChange={this.handleChange}
          error={this.state.errors.Valor}
          edit={true}
          readOnly={false}
        />
      )
    }
  }

  obtenerValorBoolean = (valor) => {
    if(typeof valor === 'boolean') {
      return valor
    } 
    return valor === "true" ? true : false
  }

  handleSubmit = () => {
    /*if (!this.isValid()) {
      this.showWarn(FRASE_ERROR_VALIDACIONES, "Validaciones")
      return
    }*/

    this.props.blockScreen()
    let url = ""
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PARAMETROS}/ActualizarParametroAsync`

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
            this.props.showSuccess(FRASE_PARAMETRO_COLABORADOR_ACTUALIZADO)
          } else {
            this.props.showSuccess(FRASE_PARAMETRO_COLABORADOR_CREADO)
          }
          this.props.ocultarFormulario(true)
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
    let valor = this.state.data.Valor

    if (valor === "") {
      errors.FechaDesde = "El campo valor es requerido"
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
    this.props.actualizarParametroSeleccionado(updatedData)
    this.setState({
      data: updatedData,
    })
  }

  obtenerTipoParametroNombre = (tipoParametro) => {
    switch (tipoParametro) {
      case 1:
        return "Número"
      case 2:
        return "Texto"
      case 3:
        return "Verdadero/Falso"
      case 4:
        return "Listado"
      case 5:
        return "Fecha"
      case 6:
        return "Objeto Serializado"
      case 7:
        return "Imágen"
      default:
        break
    }
  }

  showWarn = (displayMessage, type = "Error") => {
    this.setState({ displayMessage }, () => this.warn(type))
  }

  warn = (type) => {
    abp.notify.error(this.state.displayMessage, type)
  }
}
