import axios from "axios"
import { Button } from "primereact-v2/button"
import { FileUpload } from "primereact-v2/fileupload"
import { Dialog } from "primereact-v3.3/dialog"
import React, { Fragment } from "react"
import config from "../../Base/Config"
import ReactDOM from "react-dom"
import Wrapper from "../../Base/BaseWrapper"
import Field from "../../Base/Field-v2"
import http from "../../Base/HttpService"
import { DataTable } from "primereact-v3.3/datatable"
import { TabView, TabPanel } from "primereact-v3.3/tabview"
import CurrencyFormat from "react-currency-format"
import { Column } from "primereact-v3.3/column"
import { MultiSelect } from "primereact-v3.3/multiselect"
import { ContextMenu } from "primereact-v3.3/contextmenu"
import { ListBox } from "primereact-v3.3/listbox"
import { InputText } from "primereact-v3.3/inputtext"
import { Card } from "primereact-v3.3/card"

import {
  CONTROLLER_REDISTRIBUCION,
  FRASE_ERROR_GLOBAL,
  MODULO_CERTIFICACION_INGENIERIA,
  CONTROLLER_GRUPO_CERTIFICACION,
} from "../../Base/Strings"

class RedistribucionProyectosContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      screen: "principal",
      screenDetalle: "viewE500",
      errors: {},
      action: "",

      /*Form */
      dataProyectos: [],
      proyectoSeleccionado: {},
      ProyectoId: 0,
      dataProyectosADistribuir: [],
      ProyectoADistribuirId: 0,
      ProyectoADistribuirMasivaId: 0,

      HorasADistribuir: 0,

      dataDirectos: [{}],
      dataDirectosSeleccionados: [],
      dataE500temporal: [{}],

      dataDistribucionTemporal: [{}],
      dataDistribucionTemporalAll: [{}],
      directoSeleccionado: null,

      verDistribucionForm: false,
      verDistribucionMasiva: false,
      ProyectoaDistribuir: {},

      totalHorasProyecto: 0,
      totalCertificado: 0,
      totalporCertificar: 0,
    }
    this.handleChange = this.handleChange.bind(this)
    this.onChangeValue = this.onChangeValue.bind(this)
    this.onChangeValueProyecto = this.onChangeValueProyecto.bind(this)
  }

  componentDidMount() {
    this.getProyectos()
    //this.bloquearDiv(true);
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    })
  }

  onChangeValueMasivo = (name, value) => {
    let ProyectoaDistribuir = this.state.dataProyectosADistribuir.filter(
      (c) => c.value == value
    )[0]

    console.log("ProyectoaDistribuir", ProyectoaDistribuir)
    this.setState(
      {
        [name]: value,
        ProyectoaDistribuir,
      },

      this.EnviarDirectosMasivoConfirmacion(ProyectoaDistribuir)
    )
  }
  onChangeValueProyecto = (name, value) => {
    this.setState({
      [name]: value,
    })
    this.obtenerDetallesDirectosProyecto(value)
    this.obtenerMontos(value)
  }
  mostrarFormularioDistribucion = () => {
    if (this.state.directoSeleccionado == null) {
      abp.message.warn("Debe seleccionar un registro directo", "Alerta")
      return
    }
    this.setState({
      ProyectoADistribuirId: 0,
      HorasADistribuir: 0,
      verDistribucionForm: true,
    })
  }

  render() {
    return <Fragment>{this.renderBody()}</Fragment>
  }
  bloquearDiv = (logic) => {
    $("#dcacl").children().prop("disabled", logic)
  }
  onSelectDirectos = (e) => {
    console.log('DirectosSeleccionados',e.value)
    this.setState({ dataDirectosSeleccionados: e.value });

     /*Suma Horas */
     let ArrayHoras = e.value.map((item) => {
      var horas = parseFloat(item.NumeroHoras);
      return { horas };
    });
    console.log('ArrayHoras',ArrayHoras)
    //ArrayHoras.shift(); //Delete NAN from component select Datatable Primereact
    let total = 0;
    for (let i of ArrayHoras) total += i.horas;
    console.log(total);
    this.setState({ totalporCertificar: total });
  };

  handleSubmitProyectoMasivo = () => {
    this.props.blockScreen()
    console.log("Enviar a Proyectos")

    let DirectosId = new Array()
    if (this.state.dataDirectosSeleccionados.length > 0) {
      DirectosId = this.state.dataDirectosSeleccionados.map((select) => {
        return select.Id
      })
    }
    let url = ""
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_REDISTRIBUCION}/CrearDistribucionMasiva`
    http
      .post(url, {
        Id: this.state.ProyectoADistribuirMasivaId,
        Directos: DirectosId,
      })
      .then((response) => {
        let data = response.data
        if (data === "OK") {
          this.props.showSuccess("Distribuidos a proyectos correctamente")
          if (this.state.ProyectoId > 0) {
            this.setState({
              ProyectoADistribuirMasivaId: 0,
              verDistribucionMasiva: false,
              dataDirectosSeleccionados: [],
            })
            this.obtenerDetallesDirectosProyecto(this.state.ProyectoId)
          }
        } else {
          var message = data.message
          this.props.showWarn(message)
          this.props.unlockScreen()
        }
        this.props.unlockScreen()
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(FRASE_ERROR_GLOBAL)
        this.props.unlockScreen()
      })

    console.log("DirectosSeleccionadosId", DirectosId.toString())
  }
  handleSubmitE500 = () => {
    console.log("E500 Enviar Proceso")

    this.props.blockScreen()

    let DirectosId = new Array()
    if (this.state.dataDirectosSeleccionados.length > 0) {
      DirectosId = this.state.dataDirectosSeleccionados.map((select) => {
        return select.Id
      })
    }
    let url = ""
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_REDISTRIBUCION}/CrearE500`
    http
      .post(url, {
        Directos: DirectosId,
      })
      .then((response) => {
        let data = response.data
        if (data === "OK") {
          this.props.showSuccess("Enviado a E500 Correctamente")
          if (this.state.ProyectoId > 0) {
            this.obtenerDetallesDirectosProyecto(this.state.ProyectoId)
          }
        } else {
          var message = data.message
          this.props.showWarn(message)
          this.props.unlockScreen()
        }
        this.props.unlockScreen()
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(FRASE_ERROR_GLOBAL)
        this.props.unlockScreen()
      })
  }
  abrirDistribucionMasivaProyectos = () => {
    if (this.state.dataDirectosSeleccionados.length == 0) {
      abp.message.warn(
        "Debe seleccionar al menos un registro directo",
        "Alerta"
      )
      return
    }
    this.setState({ verDistribucionMasiva: true })
  }

  EnviarDirectosMasivoConfirmacion = (ProyectoaDistribuir) => {
    console.log("ProyectoaDistribuir", ProyectoaDistribuir)
    if (this.state.dataDirectosSeleccionados.length == 0) {
      abp.message.warn(
        "Debe seleccionar al menos un registro directo",
        "Alerta"
      )
      return
    }
    let nombreProyecto =
      ProyectoaDistribuir != null && ProyectoaDistribuir != undefined
        ? ProyectoaDistribuir.label
        : " "
    console.log("NOMbre Proeycto", nombreProyecto)
    let message = "de enviar los registros seleccionados al proyecto ".concat(
      nombreProyecto != undefined ? nombreProyecto : ""
    )
    console.log("messageConfirm", message)
    let _this = this
    abp.message.confirm(message, "Está seguro", function (isConfirmed) {
      if (isConfirmed) {
        _this.handleSubmitProyectoMasivo()
      } else {
        _this.setState({ ProyectoADistribuirMasivaId: 0 })
      }
    })
  }

  abrirConfirmacionE500 = () => {
    console.log(
      "this.state.dataDirectosSeleccionados.length ",
      this.state.dataDirectosSeleccionados.length
    )
    console.log(
      "this.state.dataDirectosSeleccionados ",
      this.state.dataDirectosSeleccionados
    )
    if (this.state.dataDirectosSeleccionados.length == 0) {
      abp.message.warn(
        "Debe seleccionar al menos un registros directo",
        "Alerta"
      )
      return
    }
    let _this = this

    abp.message.confirm(
      "de enviar los registros señalados al proyecto E500",
      "Está seguro",
      function (isConfirmed) {
        if (isConfirmed) {
          _this.handleSubmitE500()
        }
      }
    )
  }
  cellEditor = (options) => {
    console.log("options", options)
    return this.textEditor(options)
  }
  textEditor = (options) => {
    return (
      <InputText
        type="text"
        value={options.value}
        onChange={(e) => options.editorCallback(e.target.value)}
      />
    )
  }

  mostrarDistribucion = () => {}

  ButtonTemplate = (rowData) => {
    return (
      <>
        <button
          onClick={() => this.onRowSelect(rowData)}
          className="btn btn-outline-primary"
          data-toggle="tooltip"
          data-placement="top"
          disabled={this.state.ProyectoId == 0 ? true : false}
          title="Distribuir a Proyectos"
        >
          <i className="fa fa-list" />
        </button>{" "}
      </>
    )
  }
  onDeleteRow = (rowData) => {
    console.log("RowDelte", rowData)
    let dataTemporalProyecto = this.state.dataDistribucionTemporal
    let dataTemporalAll = this.state.dataDistribucionTemporalAll

    let nuevoTemporal = dataTemporalProyecto.filter(
      (c) => c.key !== rowData.key
    )
    let nuevoTemporalAll = dataTemporalAll.filter((c) => c.key !== rowData.key)
    this.setState({
      dataDistribucionTemporal: nuevoTemporal,
      dataDistribucionTemporalAll: nuevoTemporalAll,
    })
  }
  ButtonEliminarTemplate = (rowData) => {
    return (
      <button
        className="btn btn-outline-danger btn-sm"
        style={{ marginRight: "0.2em" }}
        onClick={() => {
          if (
            window.confirm(
              `Esta acción eliminará el registro temporal, ¿Desea continuar?`
            )
          )
            this.onDeleteRow(rowData)
        }}
        data-toggle="tooltip"
        data-placement="top"
        title="Eliminar"
      >
        <i className="fa fa-trash" />
      </button>
    )
  }
  regresarListado = () => {
    this.setState({ screenDetalle: "viewE500" })
  }
  onCellEditComplete = (e) => {
    let { rowData, newValue, field, originalEvent: event } = e

    console.log("rowData", rowData)
    console.log("newValue", newValue)
    console.log("field", field)
    console.log("event", event)
  }
  ComponentePrincipal = () => {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <h5>Redistribución de Directos</h5>
            <div className="row">
              {this.state.screenDetalle === "viewE500" && (
                <div className="col" align="right">
                  {"  "}
                  <button
                    onClick={() => this.abrirConfirmacionE500()}
                    className="btn btn-outline-primary"
                  >
                    Enviar a E500
                  </button>
                  {"  "}
                  <button
                    onClick={() => this.abrirDistribucionMasivaProyectos()}
                    className="btn btn-outline-primary"
                  >
                    Distribuir a otro Proyecto
                  </button>
                </div>
              )}
            </div>

            <div className="row">
              <div
                className={this.state.verDistribucionMasiva ? "col-6" : "col"}
              >
                <Field
                  name="ProyectoId"
                  label="Proyecto Origen"
                  type="select"
                  options={this.state.dataProyectos}
                  edit={true}
                  readOnly={false}
                  value={this.state.ProyectoId}
                  onChange={this.onChangeValueProyecto}
                  error={this.state.errors.ProyectoId}
                  placeholder="Seleccione Proyecto"
                  required={true}
                  filter={true}
                />
              </div>
              {this.state.verDistribucionMasiva && (
                <div className="col-6">
                  <div className="row">
                    <div className="col">
                      <Field
                        name="ProyectoADistribuirMasivaId"
                        label="Proyecto Nuevo"
                        type="select"
                        options={this.state.dataProyectosADistribuir}
                        edit={true}
                        readOnly={false}
                        value={this.state.ProyectoADistribuirMasivaId}
                        onChange={this.onChangeValueMasivo}
                        error={this.state.errors.ProyectoADistribuirMasivaId}
                        placeholder="Seleccionar..."
                        required={true}
                        filter={true}
                      />
                    </div>
                  </div>
                </div>
              )}
            </div>
            {this.state.screenDetalle === "viewE500" && (
              <>
                <div className="row">
                  <div className="col-3">
                    <div className="callout callout-info">
                      <small className="text-muted">
                        Total Horas Proyecto Presupuestado HH
                      </small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={this.state.totalHorasProyecto.toFixed(2)}
                          displayType={"text"}
                          thousandSeparator={true}
                          decimalScale={2}
                          fixedDecimalScale={true}
                        />
                      </strong>
                    </div>
                  </div>
                  <div className="col-3">
                    <div className="callout callout-danger">
                      <small className="text-muted">Total Certificado HH</small>
                      <br />
                      <strong className="h4">
                      <CurrencyFormat
                          value={this.state.totalCertificado.toFixed(2)}
                          displayType={"text"}
                          thousandSeparator={true}
                          decimalScale={2}
                          fixedDecimalScale={true}
                        /> 
                      </strong>
                    </div>
                  </div>
                  <div className="col-3">
                    <div className="callout callout-success">
                      <small className="text-muted">Saldo Horas HH</small>
                      <br />
                      <strong className="h4">
                      <CurrencyFormat
                          value={this.state.totalHorasProyecto.toFixed(2) -
                            this.state.totalCertificado.toFixed(2)}
                          displayType={"text"}
                          thousandSeparator={true}
                          decimalScale={2}
                          fixedDecimalScale={true}
                        /> 
                      </strong>
                    </div>
                  </div>
                  <div className="col-3">
                    <div className="callout callout-warning">
                      <small className="text-muted">Total por Certificar HH</small>
                      <br />
                      <strong className="h4">
                      <CurrencyFormat
                          value={this.state.totalporCertificar.toFixed(2)}
                          displayType={"text"}
                          thousandSeparator={true}
                          decimalScale={2}
                          fixedDecimalScale={true}
                        />
                      </strong>
                    </div>
                  </div>
                </div>

                <div className="row">
                  <div className="col">
                    <label>
                      <strong>Detalles Directos</strong>
                    </label>
                    <br />

                    <DataTable
                      value={this.state.dataDirectos}
                      selection={this.state.dataDirectosSeleccionados}
                      style={{ fontSize: "10px" }}
                      paginator={true}
                      dataKey="Id"
                      rows={10}
                      rowsPerPageOptions={[5, 10, 20]}
                      onSelectionChange={(e) => this.onSelectDirectos(e)}
                      /* onRowSelect={(e) => this.onRowSelect(e)}*/
                    >
                      <Column
                        selectionMode="multiple"
                        headerStyle={{ width: "5em" }}
                      ></Column>

                      <Column
                        field="nombreColaborador"
                        header="Colaborador"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="formatFechaTrabajo"
                        header="Fecha"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column
                        field="NumeroHoras"
                        header="Horas"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="nombreLocacion"
                        header="Locación"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="nombreEstado"
                        header="Estado"
                        filterq
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="esCargaAutomatica"
                        header="Carga"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        header="Opciones"
                        body={this.ButtonTemplate}
                      ></Column>
                    </DataTable>
                  </div>
                </div>
              </>
            )}
            {this.state.screenDetalle == "viewDistribucion" && (
              <>
                <Card title="" subTitle="Detalle Directo">
                  <div className="row">
                    <div className="col" align="right">
                      <button
                        onClick={() => this.regresarListado()}
                        className="btn btn-outline-primary"
                      >
                        Regresar
                      </button>
                    </div>
                  </div>
                  <div className="row">
                    <div className="col">
                      <p>
                        <strong>Colaborador: </strong>
                        {this.state.directoSeleccionado != null
                          ? this.state.directoSeleccionado.nombreColaborador
                          : ""}
                      </p>
                      <p>
                        <strong>Horas: </strong>
                        {this.state.directoSeleccionado != null
                          ? this.state.directoSeleccionado.NumeroHoras
                          : ""}
                      </p>
                      <p>
                        <strong>Locación: </strong>
                        {this.state.directoSeleccionado != null
                          ? this.state.directoSeleccionado.nombreLocacion
                          : ""}
                      </p>
                    </div>
                    <div className="col">
                      <p>
                        <strong>Fecha: </strong>
                        {this.state.directoSeleccionado != null
                          ? this.state.directoSeleccionado.formatFechaTrabajo
                          : ""}
                      </p>
                      <p>
                        <strong>Estado: </strong>
                        {this.state.directoSeleccionado != null
                          ? this.state.directoSeleccionado.nombreEstado
                          : ""}
                      </p>
                      <p>
                        <strong>Proyecto: </strong>
                        {this.state.directoSeleccionado != null
                          ? this.state.directoSeleccionado.nombreProyecto
                          : ""}
                      </p>
                    </div>
                  </div>
                </Card>
                <br />
                <Card title="" subTitle="Proyectos A Distribuir">
                  <div className="row">
                    <div className="col" align="right">
                      <button
                        onClick={() =>
                          this.onGuardarSelect(this.state.directoSeleccionado)
                        }
                        className="btn btn-outline-primary"
                        disabled={this.state.ProyectoId == 0 ? true : false}
                        data-toggle="tooltip"
                        data-placement="top"
                        title="Guardar Distribución"
                      >
                        <i className="fa fa-floppy-o" />
                        Guardar Distribución
                      </button>{" "}
                      <button
                        onClick={() => this.mostrarFormularioDistribucion()}
                        className="btn btn-outline-primary"
                      >
                        Agregar
                      </button>
                    </div>
                  </div>
                  <br />
                  <div className="row">
                    <div className="col">
                      <DataTable
                        value={this.state.dataDistribucionTemporal}
                        style={{ fontSize: "10px" }}
                        paginator={true}
                        dataKey="Id"
                        rows={10}
                        rowsPerPageOptions={[5, 10, 20]}
                      >
                        <Column
                          field="nombreProyecto"
                          header="Proyecto"
                          filter
                          filterMatchMode="contains"
                          style={{ width: "60%" }}
                        ></Column>
                        <Column
                          field="Horas"
                          editor={(options) => this.cellEditor(options)}
                          onCellEditComplete={this.onCellEditComplete}
                          header="Horas"
                          filter
                          filterMatchMode="contains"
                        ></Column>
                        <Column
                          header="Opciones"
                          body={this.ButtonEliminarTemplate}
                          style={{ width: "10%" }}
                        ></Column>
                      </DataTable>
                    </div>
                  </div>
                </Card>
              </>
            )}
          </div>
          <Dialog
            header="Seleccione Proyecto a distribuir"
            visible={this.state.verDistribucionForm}
            modal
            style={{ width: "500px" }}
            footer={this.construirBotonesDistribucion()}
            onHide={this.onHideFormulario}
          >
            <div className="row">
              <div className="col">
                <Field
                  name="ProyectoADistribuirId"
                  label="Proyecto Nuevo"
                  type="select"
                  options={this.state.dataProyectosADistribuir}
                  edit={true}
                  readOnly={false}
                  value={this.state.ProyectoADistribuirId}
                  onChange={this.onChangeValue}
                  error={this.state.errors.ProyectoADistribuirId}
                  placeholder="Seleccionar..."
                  required={true}
                  filter={true}
                />
                <Field
                  name="HorasADistribuir"
                  value={this.state.HorasADistribuir}
                  label="Horas"
                  type="text"
                  onChange={this.handleChange}
                  error={this.state.errors.HorasADistribuir}
                  readOnly={false}
                  required={true}
                  edit={true}
                />
              </div>
            </div>
          </Dialog>
        </div>
      </Fragment>
    )
  }

  obtenerMontos = (Id) => {
    this.props.blockScreen()
    var self = this
    Promise.all([this.promiseObtenerMontos(Id)])
      .then(function ([detalles]) {
        let detallesData = detalles.data
        console.log("detallesDataCertificado", detallesData)
        if (detallesData) {
          self.setState({
            totalHorasProyecto: detallesData.totalHorasProyecto,
            totalCertificado: detallesData.totalTotalCertificado,
          })
        }
        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  promiseObtenerMontos = (Id) => {
    let url = ""
    let params = `?Id=${Id}`
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/ObtenerMontos${params}`
    return http.get(url)
  }

  handleChange(event) {
    if (event.target.files) {
      console.log(event.target.files)
      let files = event.target.files || event.dataTransfer.files
      if (files.length > 0) {
        let uploadFile = files[0]
        this.setState({
          uploadFile: uploadFile,
        })
      }
    } else {
      this.setState({ [event.target.name]: event.target.value })
    }
  }
  onHideFormulario = () => {
    this.setState({ verDistribucionForm: false })
  }

  onRowSelect = (event) => {
    console.log("rowSelect", event)
    let data = event
    this.setState({ directoSeleccionado: data })
    let TemporalProyecto = this.state.dataDistribucionTemporalAll.filter(
      (c) => c.Id == data.Id
    )
    this.setState({
      dataDistribucionTemporal: TemporalProyecto,
      screenDetalle: "viewDistribucion",
    })
  }

  onGuardarSelect = (data) => {
    console.log("DirectoSeleccionado", event)
    //  let data = event;
    // this.setState({ directoSeleccionado: data });

    if (this.state.directoSeleccionado == null) {
      this.props.warn("Debe seleccionar un directo")
      return
    }
    let TemporalProyecto = this.state.dataDistribucionTemporalAll.filter(
      (c) => c.Id == data.Id
    )
    if (TemporalProyecto.length === 0) {
      this.props.showWarn(
        "Debe registrar la distribución de horas por proyecto"
      )
      return
    }

    let url = ""
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_REDISTRIBUCION}/CrearDistribucion`
    http
      .post(url, {
        Id: this.state.directoSeleccionado.Id,
        temporales: TemporalProyecto,
      })
      .then((response) => {
        let data = response.data
        if (data == "OK") {
          this.props.showSuccess("Distribución realizada correctamente")
          if (this.state.ProyectoId > 0) {
            this.setState({ screen: "principal", screenDetalle: "viewE500" })
            this.obtenerDetallesDirectosProyecto(this.state.ProyectoId)
          }
        } else {
          var message = data.message
          this.props.showWarn(message)
          this.props.unlockScreen()
        }
        this.props.unlockScreen()
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(FRASE_ERROR_GLOBAL)
        this.props.unlockScreen()
      })
  }
  agregarItemTemporal = () => {
    var date = new Date()
    let uniqueId =
      parseFloat(date.getMilliseconds() * 100 * Math.random()) +
      parseFloat(date.getSeconds()) +
      parseFloat(date.getMinutes())
    console.log("uniqueId", uniqueId)

    console.log("AgregarTemporal")
    if (this.state.ProyectoADistribuirId == 0) {
      this.props.showWarn("Campo ProyectoADistribuirId Requerido")
      return
    }

    if (this.state.HorasADistribuir == 0) {
      this.props.showWarn("Campo Horas Requerido")
      return
    }

    let tableTemporal = this.state.dataDistribucionTemporal
    console.log("DatosTablaTemporal", tableTemporal)
    let ArrayHoras = tableTemporal.map((item) => {
      var horas = parseFloat(item.Horas)
      return { horas }
    })
    let totalHoras = 0
    let MaximoHoras = 0
    let MinimoHoras = 0
    for (let i of ArrayHoras) totalHoras += i.horas
    console.log("HorasTemporal", MaximoHoras)
    console.log("this.state.HorasADistribuir", this.state.HorasADistribuir)
    MaximoHoras =
      parseFloat(totalHoras) + parseFloat(this.state.HorasADistribuir)

    MinimoHoras =
      parseFloat(this.state.directoSeleccionado.NumeroHoras) -
      parseFloat(totalHoras)
    console.log("MaximoHoras", MaximoHoras)
    if (MaximoHoras > this.state.directoSeleccionado.NumeroHoras) {
      this.props.showWarn(
        "La distribución supera las " +
          this.state.directoSeleccionado.NumeroHoras +
          " horas totales del directo, máximo " +
          MinimoHoras +
          " horas para distribuir"
      )
      return
    }

    let Proyecto = this.state.dataProyectosADistribuir.filter(
      (c) => c.value == this.state.ProyectoADistribuirId
    )[0]
    const nombreProyecto = Proyecto.label
    const Horas = this.state.HorasADistribuir
    const Id = this.state.directoSeleccionado.Id
    const key = uniqueId
    const ProyectoADistribuirId = this.state.ProyectoADistribuirId
    this.state.dataDistribucionTemporal.push({
      key: key,
      Id: Id,
      nombreProyecto: nombreProyecto,
      Horas: Horas,
      ProyectoADistribuirId: ProyectoADistribuirId,
    })
    this.state.dataDistribucionTemporalAll.push({
      key: key,
      Id: Id,
      nombreProyecto: nombreProyecto,
      Horas: Horas,
      ProyectoADistribuirId: ProyectoADistribuirId,
    })
    this.onHideFormulario()
  }

  construirBotonesDistribucion = () => {
    return (
      <Fragment>
        <Button
          label="Agregar"
          className="p-button-primary p-button-outlined"
          onClick={() => this.agregarItemTemporal()}
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
  renderBody = () => {
    if (this.state.screen === "principal") {
      return this.ComponentePrincipal()
    }
  }

  getProyectos = () => {
    this.props.blockScreen()
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_REDISTRIBUCION}/APIProyectos`,
        {}
      )
      .then((response) => {
        console.log(response.data)
        var items = response.data.map((item) => {
          return {
            label: item.codigo + " - " + item.nombre_proyecto,
            dataKey: item.Id,
            value: item.Id,
          }
        })
        this.setState(
          { dataProyectos: items, dataProyectosADistribuir: items },
          this.props.unlockScreen()
        )
      })
      .catch((error) => {
        console.log(error)
      })
  }
  obtenerDetallesDirectosProyecto = (Id) => {
    this.props.blockScreen()
    var self = this
    Promise.all([this.promiseObtenerDirectosporProyectoId(Id)])
      .then(function ([directos]) {
        let directosData = directos.data
        if (directosData.success === true) {
          self.setState({
            dataDirectos: directosData.result,
          })
        }
        //self.bloquearDiv(false);
        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  promiseObtenerDirectosporProyectoId = (id) => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_REDISTRIBUCION}/ObtenerDirectosPorProyecto/${id}`
    return http.get(url)
  }
}

const Container = Wrapper(RedistribucionProyectosContainer)
ReactDOM.render(<Container />, document.getElementById("content"))
