import { Button } from "primereact-v2/button";
import { Dialog } from "primereact-v3.3/dialog";
import React, { Fragment } from "react";
import ReactDOM from "react-dom";
import Wrapper from "../../Base/BaseWrapper";
import http from "../../Base/HttpService";
import config from "../../Base/Config";
import {
  CONTROLLER_DETALLES_INGENIERIA,
  MODULO_CERTIFICACION_INGENIERIA,
  FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
} from "../../Base/Strings";
import { DetallesIngenieriaTable } from "./DetallesIngenieriaTable.jsx";
import { DetallesIngenieriaTableE500 } from "./DetallesIngenieriaTableE500.jsx";
import Field from "../../Base/Field-v2";
import { Checkbox } from "primereact-v2/checkbox";

import { FileUpload } from "primereact-v2/fileupload";
import axios from "axios";
import BuscarColaboradorContainer from "./BuscarColaborador/BuscarColaboradorContainer";
import CurrencyFormat from "react-currency-format";
import { forEach } from "lodash-es";
import { TabView, TabPanel } from "primereact-v3.3/tabview";


class DetallesDirectosIngenieraContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      detalles: [],
      detallesE500: [],
      detalleSeleccionado: {},
      mostrarConfirmacion: false,
      showColaboradorModal: false,
      screen: "list",
      action: "",

      /*FORM */
      Id: 0,
      Identificacion: "",
      ColaboradorId: 0,
      Colaborador: null,
      TipoRegistroId: 0,
      CodigoProyecto: "",
      ProyectoId: 0,
      NumeroHoras: 0,
      NombreEjecutante: "",
      FechaTrabajo: "",
      Observaciones: "",
      EtapaId: 0,
      EspecialidadId: 0,
      EstadoRegistroId: 0,
      LocacionId: 0,
      ModalidadId: 0,
      EsDirecto: false,
      CertificadoId: null,
      JustificacionActualizacion: "",
      CargaAutomatica: false,
      FechaCarga: new Date(),
      Secuencial: "",

      catalogoTipoRegistro: [],
      Proyectos: [],
      catalogoEtapa: [],
      catalogoEspecialidad: [],
      catalogoEstadoRegistro: [],
      catalogoLocacion: [],
      catalogoModalidad: [],

      catalogoEstado: [],
      EstadoId: 0,
      errors: {},
      FechaInicio: "",
      FechaFin: "",

      totalHoras: 0,
      totalRegistros: 0,
      isUpdating: true,
      totalHorasE500: 0,
      totalRegistrosE500: 0,
      isUpdatingE500: true,

      ultimaCargaTimeSheet: {},
      mostrarConfirmacionCargaCompleta: false,

      secuencialCarga:1,
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.isValid = this.isValid.bind(this);
  }

  componentDidMount() {
    this.loadCatalogos();
  }

  loadCatalogos = () => {
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INGENIERIA}/GetCatalogosIngenieria`,
        {}
      )
      .then((response) => {
        console.log("Catalogos", response);
        let data = response.data;
        if (data.success) {
          let result = data.result;
          let catalogoEstadoRegistro = result.catalogoEstadoRegistro;
          let catalogoTipoRegistro = result.catalogoTipoRegistro;
          let catalogoEtapa = result.catalogoEtapa;
          let catalogoEspecialidad = result.catalogoEspecialidad;
          let catalogoLocacion = result.catalogoLocacion;
          let catalogoModalidad = result.catalogoModalidad;
          let catalogoEstado = result.catalogoEstado;
          this.setState({
            catalogoEstadoRegistro,
            catalogoTipoRegistro,
            catalogoEtapa,
            catalogoEspecialidad,
            catalogoLocacion,
            catalogoModalidad,
            catalogoEstado,
          });
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
      });
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INGENIERIA}/GetProyectos`,
        {}
      )
      .then((response) => {
        console.log("Proyectos", response);
        let data = response.data;
        if (data.success) {
          let result = data.result;
          let Proyectos = result;
          this.setState({
            Proyectos,
          });
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
      });
      axios
      .get(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INGENIERIA}/ObtenerUltimaCargaTimesheet`,
        {}
      )
      .then((response) => {
        console.log("UltimaCargaTimesheet", response);
        let data = response.data;
        if (data.success) {
          let result = data.result;
          this.setState({
            ultimaCargaTimeSheet: data.result,
          });
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
      });

      axios
      .get(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INGENIERIA}/GetSecuencial`,
        {}
      )
      .then((response) => {
  
        let data = response.data;
       
         
          this.setState({
          secuencialCarga: data,
          });
        
   
      })
      .catch((error) => {
        console.log(error);
      });




      
  };
 

  handleChange(event) {
    console.log("Event", event);
    if (event.target.files) {
      console.log(event.target.files);
      let files = event.target.files || event.dataTransfer.files;
      if (files.length > 0) {
        let uploadFile = files[0];
        this.setState({
          uploadFile: uploadFile,
        });
      }
    } else {
      this.setState({ [event.target.name]: event.target.value });
    }
  }
  handleChangeFechas = (event) => {
    const target = event.target;
    const value = target.value;
    const name = target.name;
    this.setState({ [name]: value });
  };

  renderBody = () => {
    if (this.state.screen === "list") {
      return this.listComponent();
    } else {
      return this.formComponent();
    }
  };

  render() {
    return <Fragment>{this.renderBody()}</Fragment>;
  }

  listComponent = () => {
    let horasTotales=this.state.totalHoras!=undefined && this.state.totalHorasE500!=undefined?
                (this.state.totalHoras+this.state.totalHorasE500):0;
                let registrosTotales=this.state.totalRegistros!=undefined && this.state.totalRegistrosE500!=undefined?
                (this.state.totalRegistros+this.state.totalRegistrosE500):0;
    
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row" style={{ marginTop: "1em" }}>
            <div className="col" >
            <p>
                  <label>Secuencial para carga: </label>{" "}
                 <b style={{fontSize:"14px"}}> {this.state.secuencialCarga}</b>
                </p>
            </div>
              <div className="col" align="right">
            {/**  <button
                  className="btn btn-outline-primary mr-4"
                  type="button"
                  data-toggle="collapse"
                  aria-expanded="false"
                  onClick={() => this.descargarFormatoCargaMasiva()}
                >
                  Descargar formato carga masiva
                </button>
                <button
                  className="btn btn-outline-primary mr-4"
                  type="button"
                  data-toggle="collapse"
                  data-target="#collapseExample"
                  aria-expanded="false"
                  aria-controls="collapseExample"
                >
                  Cargar Detalles
                </button> */}
                <button
                  className="btn btn-outline-primary mr-4"
                  type="button"
                  data-toggle="collapse"
                  aria-expanded="false"
                  onClick={() => this.mostrarConfirmacionCargaCompleta()}
                >
                  Carga Completa
                </button>
              </div>
            </div>
            <hr />
            <div className="collapse" id="collapseExample">
              <div className="card card-body">
                <div className="row">
                  <div className="col">
                    <FileUpload
                      name="uploadedFile"
                      chooseLabel="Seleccionar"
                      cancelLabel="Cancelar"
                      uploadLabel="Cargar"
                      onUpload={this.onBasicUpload}
                      accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                      maxFileSize={1000000}
                      url=""
                    />
                  </div>
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <Field
                  name="FechaInicio"
                  value={this.state.FechaInicio}
                  label="Fecha Inicio"
                  type="date"
                  onChange={this.handleChangeFechas}
                  error={this.state.errors.FechaInicio}
                  edit={true}
                  readOnly={false}
                />
              </div>

              <div className="col">
                <Field
                  name="FechaFin"
                  value={this.state.FechaFin}
                  label="Fecha Fin"
                  type="date"
                  onChange={this.handleChangeFechas}
                  error={this.state.errors.FechaFin}
                  edit={true}
                  readOnly={false}
                />
              </div>
            </div>
           
            <div className="row" style={{ marginLeft: "0.1em" }}>
              <Button
                label="Buscar"
                className="p-button-outlined"
                onClick={() => this.obtenerDetalles()}
                icon="pi pi-search"
              />
              <Button
                style={{ marginLeft: "0.4em" }}
                label="Limpiar"
                className="p-button-outlined"
                onClick={() => this.borrarFiltros()}
                icon="pi pi-ban"
              />
            </div>

            <div className="row" style={{ marginTop: "1em" }}>

            </div>

            <div className="row">

              <div className="col-2">
                <div className="callout callout-danger">
                  <small className="text-muted">Total HH Directos</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.state.totalHoras.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>

              <div className="col-2">
                <div className="callout callout-warning">
                  <small className="text-muted">Total Registros Directos</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.state.totalRegistros.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>
              <div className="col-2">
                <div className="callout callout-danger">
                  <small className="text-muted">Total E500 HH</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.state.totalHorasE500.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>

              <div className="col-2">
                <div className="callout callout-warning">
                  <small className="text-muted">Total Registros E500</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.state.totalRegistrosE500.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>
              <div className="col-2">
                <div className="callout callout-default">
                  <small className="text-muted">Total HH </small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        horasTotales.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>
              <div className="col-2">
                <div className="callout callout-default">
                  <small className="text-muted">Total Registros </small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        registrosTotales.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>

             
            </div>
  
            <hr />
            <TabView className="tabview-custom">
              <TabPanel header="Directos">
              <div className="row">
              <div className="col">
              </div>
              <div className="col-3" align="right">
                <button
                  className="btn btn-outline-primary align-middle"
                  style={{ marginTop: '1.3em' }}
                  type="button"
                  data-toggle="collapse"
                  aria-expanded="false"
                  onClick={() => this.mostrarFormulario({})}
                >
                  Nuevo
                </button>
              </div>
              </div>
              <div className="row">
              <div className="col">
                <DetallesIngenieriaTable
                  data={this.state.detalles}
                  afterColumnFilter={this.afterColumnFilter}
                  mostrarFormulario={this.mostrarFormulario}
                  mostrarConfirmacionParaEliminar={
                    this.mostrarConfirmacionParaEliminar
                  }
                  EnviarValidado={this.EnviarValidado}
                />
              </div>
            </div>

              </TabPanel>
                <TabPanel header="Directos E500">
                <div className="row">
              <div className="col">
                <DetallesIngenieriaTableE500
                  data={this.state.detallesE500}
                  afterColumnFilter={this.afterColumnFilterE500}
                 /* mostrarFormulario={this.mostrarFormulario}
                  mostrarConfirmacionParaEliminar={
                    this.mostrarConfirmacionParaEliminar
                  }
                  EnviarValidado={this.EnviarValidado}*/
                />
              </div>
            </div>
              </TabPanel>
            </TabView>



           
            <Dialog
              header="Confirmación"
              visible={this.state.mostrarConfirmacion}
              modal
              style={{ width: "500px" }}
              footer={this.construirBotonesDeConfirmacion()}
              onHide={this.onHideFormulario}
            >
              <div className="confirmation-content">
                <div className="p-12">
                  <i
                    className="pi pi-exclamation-triangle p-mr-3"
                    style={{ fontSize: "2rem" }}
                  />
                  <p>
                    {" "}
                    ¿Está seguro de eliminar el registro? Si desea continuar
                    presione ELIMINAR, caso contrario CANCELAR
                  </p>
                </div>
              </div>
            </Dialog>

            <Dialog
              header="Confirmación"
              visible={this.state.mostrarConfirmacionCargaCompleta}
              modal
              style={{ width: "500px" }}
              footer={this.construirBotonesDeConfirmacionCargaCompleta()}
              onHide={this.onHideCargaCompleta}
            >
              <div className="confirmation-content">
                <div className="p-12">
                  <i
                    className="pi pi-exclamation-triangle p-mr-3"
                    style={{ fontSize: "2rem" }}
                  />
                  <p>
                    {" "}
                    ¿Está seguro de confirmar que la carga con fecha final { this.state.ultimaCargaTimeSheet ? this.state.ultimaCargaTimeSheet.FechaFinal : "" } está completa? 
                  </p>
                </div>
              </div>
            </Dialog>
          </div>
        </div>
      </Fragment>
    );
  };

  

  afterColumnFilter = async (filterConds, result) => {
    if (result !== null) {
      let totalHoras = 0;
      let totalRegistros = 0;

      result.forEach(element => {
        totalHoras += element.NumeroHoras
        totalRegistros += 1;
      });

      if (this.state.isUpdating) {
        this.setState({ isUpdating: false });
        await setTimeout(1000);
        this.setState({ totalHoras, totalRegistros })

        setTimeout(function () {
          this.setState({ isUpdating: true });
        }.bind(this), 1000);
      }

    }
  };

  afterColumnFilterE500 = async (filterConds, result) => {
    if (result !== null) {
      let totalHorasE500 = 0;
      let totalRegistrosE500 = 0;

      result.forEach(element => {
        totalHorasE500 += element.NumeroHoras
        totalRegistrosE500 += 1;
      });

      if (this.state.isUpdatingE500) {
        this.setState({ isUpdatingE500: false });
        await setTimeout(1000);
        this.setState({ totalHorasE500, totalRegistrosE500 })

        setTimeout(function () {
          this.setState({ isUpdatingE500: true });
        }.bind(this), 1000);
      }

    }
  };

  isValid = () => {
    const errors = {};

    if (this.state.NombreEjecutante == 0) {
      errors.NombreEjecutante = "El campo es requerido";
    }
    if (this.state.Identificacion === "") {
      errors.Identificacion = "El campo es requerido";
    }
    if (this.state.ColaboradorId === 0) {
      errors.ColaboradorId = "El campo es requerido";
    }
    if (this.state.TipoRegistroId === 0) {
      errors.TipoRegistroId = "El campo es requerido";
    }
    if (this.state.ProyectoId === 0) {
      errors.ProyectoId = "El campo es requerido";
    }
    if (this.state.NombreEjecutante === "") {
      errors.NombreEjecutante = "El campo es requerido";
    }
    if (this.state.FechaTrabajo === "") {
      errors.FechaTrabajo = "El campo es requerido";
    }
    if (this.state.EtapaId === 0) {
      errors.EtapaId = "El campo es requerido";
    }
    if (this.state.EspecialidadId === 0) {
      errors.EspecialidadId = "El campo es requerido";
    }
    if (this.state.EstadoRegistroId === 0) {
      errors.EstadoRegistroId = "El campo es requerido";
    }
    if (this.state.LocacionId === 0) {
      errors.LocacionId = "El campo es requerido";
    }
    if (this.state.ModalidadId === 0) {
      errors.ModalidadId = "El campo es requerido";
    }
    if (this.state.action === "edit") {
      console.log('EntroEditar', this.state.action);
      if (this.state.JustificacionActualizacion === "") {
        errors.JustificacionActualizacion = "El campo es requerido";
      }
    }
    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };

  handleSubmit = () => {
    if (!this.isValid()) {
      this.props.showWarn(FRASE_ERROR_VALIDACIONES, "Validaciones");
      return;
    }
    this.props.blockScreen();
    let url = "";
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INGENIERIA}`;
    if (this.state.Id > 0) {
      url += "/Editar";
    } else {
      url += "/Crear";
    }

    console.log(url);

    http
      .post(url, {
        Id: this.state.Id,
        Identificacion: this.state.Identificacion,
        ColaboradorId: this.state.ColaboradorId,
        TipoRegistroId: this.state.TipoRegistroId,
        CodigoProyecto: this.state.CodigoProyecto,
        ProyectoId: this.state.ProyectoId,
        NumeroHoras: this.state.NumeroHoras,
        NombreEjecutante: this.state.NombreEjecutante,
        FechaTrabajo: this.state.FechaTrabajo,
        Observaciones: this.state.Observaciones,
        EtapaId: this.state.EtapaId,
        EspecialidadId: this.state.EspecialidadId,
        EstadoRegistroId: this.state.EstadoRegistroId,
        LocacionId: this.state.LocacionId,
        ModalidadId: this.state.ModalidadId,
        EsDirecto: this.state.EsDirecto,
        CertificadoId: this.state.CertificadoId,
        JustificacionActualizacion: this.state.JustificacionActualizacion,
        CargaAutomatica: this.state.CargaAutomatica,
        Secuencial: this.state.Secuencial,
        FechaCarga: new Date(),

      })
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          this.props.showSuccess("Completada Correctamente");
          this.onHideFormulario();
          this.obtenerDetalles();
        } else {
          var message = $.fn.responseAjaxErrorToString(data);
          this.props.showWarn(message);
          this.props.unlockScreen();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(FRASE_ERROR_GLOBAL);
        this.props.unlockScreen();
      });
  };

  onInitCompo = () => {
    console.log("Init");
    this.setState({
      Identificacion: "",
      ColaboradorId: 0,
      Colaborador: null,
      TipoRegistroId: 0,
      CodigoProyecto: "",
      ProyectoId: 0,
      NumeroHoras: 0,
      NombreEjecutante: "",
      FechaTrabajo: "",
      Observaciones: "",
      EtapaId: 0,
      EspecialidadId: 0,
      EstadoRegistroId: 0,
      LocacionId: 0,
      ModalidadId: 0,
      EsDirecto: false,
      CertificadoId: null,
      Secuencial: ""
    });
  };

  onHideColaboradorModal = () => {
    this.setState({ showColaboradorModal: false }, this.resetColaboradorSearch);
  };
  onShowColaboradorModal = () => {
    this.setState({ showColaboradorModal: true });
  };
  onChangeValue = (name, value) => {
    console.log(name);
    console.log(value);
    this.setState({
      [name]: value,
    });
  };
  formComponent = () => {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col">
                <div className="row">
                  <div className="col">
                    <p>
                      <b>Ejecutante</b>{" "}
                    </p>
                  </div>
                  <div className="col">
                    <Button
                      label="Buscar Ejecutante"
                      className="p-button-outlined"
                      onClick={() => this.onShowColaboradorModal()}
                      icon="pi pi-search"
                    />
                  </div>
                </div>
                <hr />
                <div className="row">
                  <div className="col">
                    <Field
                      name="Identificacion"
                      value={this.state.Identificacion}
                      label="CI Ejecutante"
                      type="text"
                      onChange={this.handleChange}
                      error={this.state.errors.Identificacion}
                      readOnly={true}
                      edit={false}
                    />
                  </div>
                  <div className="col">
                    <Field
                      name="NombreEjecutante"
                      value={this.state.NombreEjecutante}
                      label="Nombres Completos"
                      type="text"
                      onChange={this.handleChange}
                      error={this.state.errors.NombreEjecutante}
                      readOnly={true}
                      edit={true}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col-6">
                    <Field
                      name="TipoRegistroId"
                      label="Tipo Registro"
                      type="select"
                      options={this.state.catalogoTipoRegistro}
                      edit={true}
                      readOnly={false}
                      value={this.state.TipoRegistroId}
                      onChange={this.onChangeValue}
                      error={this.state.errors.TipoRegistroId}
                      placeholder="Seleccionar..."
                      required={true}
                    />
                  </div>
                  <div className="col-6">
                    <Field
                      name="ProyectoId"
                      label="Proyecto"
                      type="select"
                      options={this.state.Proyectos}
                      edit={true}
                      readOnly={false}
                      value={this.state.ProyectoId}
                      onChange={this.onChangeValue}
                      error={this.state.errors.ProyectoId}
                      placeholder="Seleccionar..."
                      required={true}
                      filter={true}
                    />
                  </div>
                </div>

                <div className="row">
                  <div className="col-6">
                    <Field
                      name="FechaTrabajo"
                      value={this.state.FechaTrabajo}
                      label="Fecha Trabajo"
                      type="date"
                      onChange={this.handleChangeFechas}
                      error={this.state.errors.FechaTrabajo}
                      edit={true}
                      readOnly={false}
                      required={true}
                    />
                  </div>
                  <div className="col-6">
                    <Field
                      name="NumeroHoras"
                      value={this.state.NumeroHoras}
                      label="Horas"
                      type="text"
                      onChange={this.handleChange}
                      error={this.state.errors.NumeroHoras}
                      readOnly={false}
                      required={true}
                      edit={true}
                    />
                  </div>
                </div>

                <div className="row">
                  <div className="col-6">
                    <Field
                      name="EtapaId"
                      label="Etapa"
                      type="select"
                      options={this.state.catalogoEtapa}
                      edit={true}
                      readOnly={false}
                      value={this.state.EtapaId}
                      onChange={this.onChangeValue}
                      error={this.state.errors.EtapaId}
                      placeholder="Seleccionar..."
                      required={true}
                    />
                  </div>
                  <div className="col-6">
                    <Field
                      name="EspecialidadId"
                      label="Especialidad"
                      type="select"
                      options={this.state.catalogoEspecialidad}
                      edit={true}
                      readOnly={false}
                      value={this.state.EspecialidadId}
                      onChange={this.onChangeValue}
                      error={this.state.errors.EspecialidadId}
                      placeholder="Seleccionar..."
                      required={true}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col-6">
                    <Field
                      name="EstadoRegistroId"
                      label="Estado"
                      type="select"
                      options={this.state.catalogoEstadoRegistro}
                      edit={false}
                      readOnly={true}
                      value={this.state.EstadoRegistroId}
                      onChange={this.onChangeValue}
                      error={this.state.errors.EstadoRegistroId}
                      placeholder="Seleccionar..."
                      required={true}
                    />
                  </div>
                  <div className="col-6">
                    <Field
                      name="LocacionId"
                      label="Locación"
                      type="select"
                      options={this.state.catalogoLocacion}
                      edit={true}
                      readOnly={false}
                      value={this.state.LocacionId}
                      onChange={this.onChangeValue}
                      error={this.state.errors.LocacionId}
                      placeholder="Seleccionar..."
                      required={true}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col-6">
                    <Field
                      name="ModalidadId"
                      label="Modalidad"
                      type="select"
                      options={this.state.catalogoModalidad}
                      edit={true}
                      readOnly={false}
                      value={this.state.ModalidadId}
                      onChange={this.onChangeValue}
                      error={this.state.errors.ModalidadId}
                      placeholder="Seleccionar..."
                      required={true}
                    />
                  </div>
                  <div className="col-6">
                    <Checkbox
                      style={{ marginTop: "0.5px" }}
                      checked={this.state.EsDirecto}
                      onChange={(e) => this.setState({ EsDirecto: e.checked })}
                    />{" "}
                    &nbsp; Es Directo
                  </div>
                </div>
                <div className="row">
                  <div className="col">
                    <Field
                      name="Observaciones"
                      value={this.state.Observaciones}
                      label="Observaciones"
                      type="text"
                      onChange={this.handleChange}
                      error={this.state.errors.Observaciones}
                      readOnly={false}
                      edit={true}
                    />
                  </div>
                </div>
                {this.state.action === "edit" && (
                  <div className="row">
                    <div className="col">
                      <Field
                        name="JustificacionActualizacion"
                        value={this.state.JustificacionActualizacion}
                        label="Justificación Cambio"
                        type="text"
                        onChange={this.handleChange}
                        error={this.state.errors.JustificacionActualizacion}
                        readOnly={false}
                        edit={true}
                        required={true}
                      />
                    </div>
                  </div>
                )}
                {this.state.action === "edit" && (
                  <div className="row">
                    <div className="col">
                      <Field
                        name="Secuencial"
                        value={this.state.Secuencial}
                        label="Secuencial"
                        type="text"
                        onChange={this.handleChange}
                        error={this.state.errors.Secuencial}
                        readOnly={true}
                        edit={true}
                        required={false}
                      />
                    </div>
                    <div></div>
                  </div>
                )}

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
                onClick={() => this.onHideFormulario()}
                icon="pi pi-ban"
              />
            </div>
          </div>
          <Dialog
            header="Ejecutante"
            visible={this.state.showColaboradorModal}
            style={{ width: "830px" }}
            onHide={this.onHideColaboradorModal}
          >
            <BuscarColaboradorContainer
              seleccionarColaborador={this.seleccionarColaborador}
              unlockScreen={this.props.unlockScreen}
              blockScreen={this.props.blockScreen}
              showSuccess={this.props.showSuccess}
              showWarn={this.props.showWarn}
            />
          </Dialog>
        </div>
      </Fragment>
    );
  };

  EnviarValidado = (Id) => {
    console.log(Id);
    axios
      .post(
        "/CertificacionIngenieria/DetalleDirectoIngenieria/ActualizarEstado/",
        {
          Id: Id,
        }
      )
      .then((response) => {
        if (response.data.result) {
          this.props.showSuccess("Actualizado Estado Correctamente");
          this.obtenerDetalles();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("Ocurrió un Error");
      });
  };
  seleccionarColaborador = (colaborador) => {
    console.log("colaborador", colaborador);
    this.setState({
      Colaborador: colaborador,
      ColaboradorId: colaborador.Id,
      Identificacion: colaborador.Identificacion,
      NombreEjecutante: colaborador.NombreCompleto,
      showColaboradorModal: false,
    });
  };
  handleInputChange = (event) => {
    console.log("EventInput", event);
  };
  seachColaborador = (search) => {
    if (search.length > 0) {
      this.props.blockScreen();
      axios
        .post(
          `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INGENIERIA}/ObtenerColaborador`,
          {
            search: search,
          }
        )
        .then((response) => {
          console.log("Response", response);

          this.setState({ Colaboradores: response.data });
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.unlockScreen();
        });
    }
  };
  obtenerDetalles = () => {
    let fechaInicio = this.state.FechaInicio;
    let fechaFin = this.state.FechaFin;
    const errors = {};

    if (fechaInicio !== "") {
      if (fechaFin === "") {
        errors.FechaFin = "Fecha de fin es requerida";
        this.setState({ errors });
        return;
      }
    }

    if (fechaFin !== "") {
      if (fechaInicio === "") {
        errors.FechaInicio = "Fecha de inicio es requerida";
        this.setState({ errors });
        return;
      }
    }
    this.setState({ errors });

    this.props.blockScreen();
    var self = this;
    Promise.all([this.PromiseDetalles()])
      .then(function ([listdetalles]) {
  
        let detallesData = listdetalles.data;
    
        if (detallesData.success === true) {
          let totalHoras = 0;
          let totalRegistros = 0;
          let totalHorasE500 = 0;
          let totalRegistrosE500 = 0;
          let detallesDirectos = detallesData.result.Directos;
          let detallesE500 = detallesData.result.DirectosE500;
        
          detallesDirectos.forEach(element => {
            totalHoras += element.NumeroHoras
            totalRegistros += 1;
          });
          detallesE500.forEach(element => {
            totalHorasE500 += element.NumeroHoras
            totalRegistrosE500 += 1;
          });

          self.setState({
            detalles: detallesDirectos,
            detallesE500:detallesE500,
            totalHoras,
            totalRegistros,
            totalHorasE500,
            totalRegistrosE500
          });

        }
        self.props.unlockScreen();
      })
      .catch((error) => {
        self.props.unlockScreen();
        console.log(error);
      });
  };

  borrarFiltros = () => {
    this.setState({ FechaInicio: "", FechaFin: "" });
  };

  eliminarDetalle = () => {
    this.props.blockScreen();
    let url = "";
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INGENIERIA}/EliminarDetalle/${this.state.detalleSeleccionado.Id}`;

    http
      .post(url, {})
      .then((response) => {
        console.log(response);
        let data = response.data;
        if (data.success === true && data.result === "OK") {
          this.props.showSuccess(FRASE_RUBRO_ELIMINADO);
          this.onHideFormulario(true);
        } else {
          var message = data.result;
          this.props.showWarn(message);
          this.props.unlockScreen();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(FRASE_ERROR_GLOBAL);
        this.props.unlockScreen();
      });
  };

  mostrarFormulario = (detalle) => {
    console.log("DetalleActionEdit", detalle);

    if (Object.keys(detalle).length === 0) {
      console.log("NUEVO");
      this.setState({
        screen: "form",
      });
      let EN_REVISION = this.state.catalogoEstadoRegistro.filter(
        (c) => c.label == "EN REVISIÓN"
      );
      console.log("EN_REVISION", EN_REVISION);
      let Id = EN_REVISION != null ? EN_REVISION[0]["value"] : 0;
      console.log("Id", EN_REVISION[0]["value"]);
      this.setState({ screen: "form", EstadoRegistroId: Id });
      console.log("EstadoRegistroId", this.state.EstadoRegistroId);
    } else {
      console.log("EDIT");
      this.setState({
        detalleSeleccionado: detalle,
        screen: "form",
        action: "edit",
        Id: detalle.Id,
        Identificacion: detalle.Identificacion,
        ColaboradorId: detalle.ColaboradorId,
        TipoRegistroId: detalle.TipoRegistroId,
        CodigoProyecto: detalle.CodigoProyecto,
        ProyectoId: detalle.ProyectoId,
        NumeroHoras: detalle.NumeroHoras,
        NombreEjecutante: detalle.NombreEjecutante,
        FechaTrabajo: detalle.FechaTrabajo,
        Observaciones: detalle.Observaciones,
        EtapaId: detalle.EtapaId,
        EspecialidadId: detalle.EspecialidadId,
        EstadoRegistroId: detalle.EstadoRegistroId,
        LocacionId: detalle.LocacionId,
        ModalidadId: detalle.ModalidadId,
        EsDirecto: detalle.EsDirecto,
        CertificadoId: detalle.CertificadoId,
        JustificacionActualizacion: detalle.JustificacionActualizacion,
        CargaAutomatica: detalle.CargaAutomatica,
        FechaCarga: detalle.FechaCarga,
        Secuencial: detalle.Secuencial
      });
    }
  };

  mostrarConfirmacionParaEliminar = (detalle) => {
    this.setState({
      detalleSeleccionado: detalle,
      mostrarConfirmacion: true,
    });
  };

  onHideFormulario = () => {
    this.setState(
      {
        screen: "list",
        detalleSeleccionado: {},
        mostrarConfirmacion: false,
      },
      this.onInitCompo()
    );
  };

  PromiseDetalles = () => {
    let url = "";
    let params = `?fechaInicio=${this.state.FechaInicio}&fechaFin=${this.state.FechaFin}`;
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INGENIERIA}/GetDetalles${params}`;
    return http.get(url);
  };

  actualizarSeleccionado = (detalle) => {
    this.setState({ detalleSeleccionado: detalle });
  };

  descargarFormatoCargaMasiva = () => {
    this.props.blockScreen();
    var formData = new FormData();
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INGENIERIA}/DescargarPlantillaCargaMasiva`,
        formData,
        { responseType: "arraybuffer" }
      )
      .then((response) => {
        var nombre = response.headers["content-disposition"].split("=");

        const url = window.URL.createObjectURL(
          new Blob([response.data], {
            type:
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          })
        );
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", nombre[1]);
        document.body.appendChild(link);
        link.click();
        this.props.showSuccess("Formato descargado exitosamente");
        this.props.unlockScreen();
        this.setState({
          ContratoId: 0,
        });
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(
          "Ocurrió un error al descargar el archivo, intentalo nuevamente"
        );
        this.props.unlockScreen();
      });
  };

  onBasicUpload = (event) => {
    var file = event.files[0];

    if (
      file.type ===
      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    ) {
      this.setState({ file });

      this.props.blockScreen();
      var formData = new FormData();
      formData.append("file", event.files[0]);

      axios
        .post(
          `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INGENIERIA}/CargaMasivaDetalles`,
          formData,
          {
            headers: {
              "Content-Disposition": "attachment; filename=template.xlsx",
              "Content-Type":
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            },
            responseType: "arraybuffer",
          }
        )
        .then((response) => {
          var nombre = response.headers["content-disposition"].split("=");

          const url = window.URL.createObjectURL(
            new Blob([response.data], {
              type:
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            })
          );
          const link = document.createElement("a");
          link.href = url;
          link.setAttribute("download", nombre[1]);
          document.body.appendChild(link);
          link.click();
          this.obtenerDetalles();
          this.props.showSuccess("Archivo procesado exitosamente");
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.showWarn(
            "Ocurrió un error al subir el archivo, intentalo nuevamente"
          );
          this.props.unlockScreen();
        });
    } else {
      this.props.showWarn("El formato de archivo es incorrecto");
    }
  };

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarDetalle()}
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
    );
  };

  construirBotonesDeConfirmacionCargaCompleta = () => {
    return (
      <Fragment>
        <Button
          label="Continuar"
          className="p-button-warning p-button-outlined"
          onClick={() => this.confirmarCargaCompleta()}
          icon="pi pi-save"
        />
        <Button
          style={{ marginLeft: "0.4em" }}
          label="Cancelar"
          className="p-button-outlined"
          onClick={() => this.onHideCargaCompleta()}
          icon="pi pi-ban"
        />
      </Fragment>
    );
  };

  mostrarConfirmacionCargaCompleta = () => {
    if (this.state.ultimaCargaTimeSheet !== null && this.state.ultimaCargaTimeSheet.Id !== 0) {
      this.setState({
        mostrarConfirmacionCargaCompleta: true
      })
    } else {
      this.props.showWarn("No se encontró una carga de timesheet pendiente de aprobación", "Validaciones");
    }
  }

  confirmarCargaCompleta = () => {
    console.log(this.state.ultimaCargaTimeSheet)
    this.props.blockScreen();
    let url = "";
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INGENIERIA}/ValidarCargaTimesheetAsync`;

    http
      .post(url, {
        Id: this.state.ultimaCargaTimeSheet.Id,
      })
      .then((response) => {
        let data = response.data;
        console.log(data);
        if (data.success === true) {
          this.props.showSuccess("Carga confirmada exitosamente");
          this.onHideCargaCompleta();
        } else {
          this.props.showWarn("Ocurrió un error al validar la carga, intenténtelo más tarde.");
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(FRASE_ERROR_GLOBAL);
        this.props.unlockScreen();
      });
  }

  onHideCargaCompleta = () => {
    this.setState({
      mostrarConfirmacionCargaCompleta: false
    })
  }
}

const Container = Wrapper(DetallesDirectosIngenieraContainer);
ReactDOM.render(
  <Container />,
  document.getElementById("detalles_directos_ingenieria")
);
