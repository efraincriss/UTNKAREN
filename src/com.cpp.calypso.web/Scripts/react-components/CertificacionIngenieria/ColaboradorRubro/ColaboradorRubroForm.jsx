import { Button } from "primereact-v2/button"
import { Dialog } from "primereact-v3.3/dialog"
import React from "react"
import config from "../../Base/Config"
import Field from "../../Base/Field-v2"
import http from "../../Base/HttpService"
import {
  CONTROLLER_COLABORADOR_RUBRO, FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
  FRASE_RUBRO_ACTUALIZADO,
  FRASE_RUBRO_CREADO,
  MODULO_CERTIFICACION_INGENIERIA
} from "../../Base/Strings"
import BuscarColaboradorContainer from "../BuscarColaborador/BuscarColaboradorContainer"

export class ColaboradorRubroForm extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      data: {
        Id: props.rubro.Id ? props.rubro.Id : 0,
        ContratoId: props.rubro.ContratoId ? props.rubro.ContratoId : 0,
        ColaboradorId: props.rubro.ColaboradorId
          ? props.rubro.ColaboradorId
          : 0,
        Nombres: props.rubro.Nombres
          ? props.rubro.Nombres
          : "",
        RubroId: props.rubro.RubroId ? props.rubro.RubroId : 0,

        Tarifa: props.rubro.Tarifa ? props.rubro.Tarifa : 0.0,
        FechaInicio: props.rubro.FechaInicio ? props.rubro.FechaInicio : "",
        FechaFin: props.rubro.FechaFin ? props.rubro.FechaFin : "",
        CreationTime: props.rubro.CreationTime ? props.rubro.CreationTime : new Date().toISOString(),
        CreatorUserId: props.rubro.CreatorUserId
          ? props.rubro.CreatorUserId
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
      Id: prevProps.rubro.Id ? prevProps.rubro.Id : 0,
      ContratoId: prevProps.rubro.ContratoId ? prevProps.rubro.ContratoId : 0,
      ColaboradorId: prevProps.rubro.ColaboradorId
        ? prevProps.rubro.ColaboradorId
        : 0,
      Nombres: prevProps.rubro.Nombres
        ? prevProps.rubro.Nombres
        : "",
      RubroId: prevProps.rubro.RubroId ? prevProps.rubro.RubroId : 0,

      Tarifa: prevProps.rubro.Tarifa ? prevProps.rubro.Tarifa : 0.0,
      FechaInicio: prevProps.rubro.FechaInicio
        ? prevProps.rubro.FechaInicio
        : "",
      FechaFin: prevProps.rubro.FechaFin ? prevProps.rubro.FechaFin : "",
      CreationTime: prevProps.rubro.CreationTime
        ? prevProps.rubro.CreationTime
        : new Date().toISOString(),
      CreatorUserId: prevProps.rubro.CreatorUserId
        ? prevProps.rubro.CreatorUserId
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
              <b>Colaborador:</b> {this.state.data.Nombres}
            </p>
          </div>
        </div>
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
              onChange={this.onChangeContrato}
              error={this.state.errors.ContratoId}
              placeholder="Seleccionar..."
            />
          </div>
          <div className="col-6">
            <Field
              name="RubroId"
              label="Rubro Preciario"
              type="select"
              options={this.props.catalogoRubros}
              edit={true}
              readOnly={false}
              value={this.state.data.RubroId}
              onChange={this.onChangeRubro}
              error={this.state.errors.RubroId}
              placeholder="Seleccionar..."
            />
          </div>
        </div>

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
          <div className="col-6">
            <Field
              name="Tarifa"
              value={this.state.data.Tarifa}
              label="Tarifa Hora USD"
              type="text"
              onChange={this.handleChange}
              error={this.state.errors.Tarifa}
              readOnly={true}
            />
          </div>
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
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_RUBRO}`
    if (this.state.data.Id > 0) {
      url += "/EditarColaboradorRubroAsync"
    } else {
      url += "/CrearColaboradorRubroAsync"
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
            this.props.showSuccess(FRASE_RUBRO_ACTUALIZADO)
          } else {
            this.props.showSuccess(FRASE_RUBRO_CREADO)
          }
          this.props.obtenerRubrosColaborador()
          this.props.onHideFormulario(true)
        } else {
          var message = data.message;
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
    updatedData["Nombres"] = colaborador.nombres_apellidos
    this.props.actualizarRubroSeleccionado(updatedData)
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
    let contratoId = this.state.data.ContratoId
    let rubroId = this.state.data.RubroId
    let fechaInicio = this.state.data.FechaInicio

    if (colaboradorId === 0) {
      errors.ColaboradorId = "El Colaborador es requerido"
    }

    if (contratoId === 0) {
      errors.ContratoId = "El Contrato es requerido"
    }

    if (rubroId === 0) {
      errors.RubroId = "El Rubro Preciario es requerido"
    }

    if (fechaInicio === "") {
      errors.FechaInicio = "El campo Fecha Inicio es requerido"
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
    this.props.actualizarRubroSeleccionado(updatedData)
    this.setState({
      data: updatedData,
    })
  }

  onChangeRubro = (name, value) => {
    const updatedData = {
      ...this.state.data,
    }
    let tarifa = this.props.catalogoRubros.filter(
      (rubro) => rubro.value === value
    )[0]
    updatedData["Tarifa"] = tarifa.tarifa
    updatedData[name] = value
    this.props.actualizarRubroSeleccionado(updatedData)
    this.setState({
      data: updatedData,
    })
  }

  onChangeContrato = (name, value) => {
    const updatedData = {
      ...this.state.data,
    }
    updatedData[name] = value
    this.props.actualizarRubroSeleccionado(updatedData)
    this.setState({
      data: updatedData,
    })
    this.props.obtenerRubrosPorcontrato(value)
  }

  onChangeDropdown = (name, value) => {
    const updatedData = {
      ...this.state.data,
    }
    updatedData[name] = value
    this.props.actualizarRubroSeleccionado(updatedData)
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
