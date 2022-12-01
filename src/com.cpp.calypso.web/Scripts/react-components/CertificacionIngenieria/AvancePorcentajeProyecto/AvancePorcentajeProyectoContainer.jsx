import { Button } from "primereact-v2/button";
import { Dialog } from "primereact-v3.3/dialog";
import React, { Fragment } from "react";
import ReactDOM from "react-dom";
import Wrapper from "../../Base/BaseWrapper";
import http from "../../Base/HttpService";
import config from "../../Base/Config";


import {
  CONTROLLER_DETALLES_INGENIERIA,
  CONTROLLER_AVANCES_PROYECTO,
  MODULO_CERTIFICACION_INGENIERIA,
  FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
} from "../../Base/Strings";
import { DetallesIngenieriaTable } from "./DetallesIngenieriaTable.jsx";
import Field from "../../Base/Field-v2";
import { Checkbox } from "primereact-v2/checkbox";

import { FileUpload } from "primereact-v2/fileupload";
import axios from "axios";
import BuscarColaboradorContainer from "./BuscarColaborador/BuscarColaboradorContainer";

class AvancePorcentajeProyectoContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      detalles: [],
      detalleSeleccionado: {},
      mostrarConfirmacion: false,
      showColaboradorModal: false,
      screen: "list",
      action: "",

      /*FORM */
      Id: 0,
      Identificacion: "",
      ColaboradorId: 0,
      e: null,
      Colaborador: null,
      TipoRegistroId: 0,
      CodigoProyecto: "",
      ProyectoId: 0,
      NumeroHoras: 0,
      NumeroHoras1: 0,
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
      FechaCarga: "",

      AvancePrevistoAnteriorIB: 0.0,
      AvanceRealAnteriorIB: 0.0,
      AvancePrevistoActualIB: 0.0,
      AvanceRealActualIB: 0.0,

      AvancePrevistoAnteriorID: 0.0,
      AvanceRealAnteriorID: 0.0,
      AvancePrevistoActualID: 0.0,
      AvanceRealActualID: 0.0,

      AsbuiltActual: 0.0,
      AsbuiltAnterior: 0.0,
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.isValid = this.isValid.bind(this);
  }

  componentDidMount() {
    const elements = []; //..some array

    const items = [];
    for (const [index, value] of elements.entries()) {
      items.push(<Element key={index} />);
    }
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
  };

  handleChange(event) {
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
  buscarDatos = (value, fechaTrabajo) => {
    this.props.blockScreen();
    axios
      .post("/CertificacionIngenieria/AvancePorcentajeProyecto/ObtenerDto", {
        Id: value,
        fecha: fechaTrabajo === "" ? this.state.FechaTrabajo : fechaTrabajo,
      })
      .then((response) => {
        console.log("Response", response);
        this.setState({
          e: response.data,
          AvancePrevistoActualIB: response.data.AvancePrevistoActualIB,
          AvanceRealActualIB: response.data.AvanceRealActualIB,
          AvancePrevistoActualID: response.data.AvancePrevistoActualID,
          AvanceRealActualID: response.data.AvanceRealActualID,
          AsbuiltActual: response.data.AsbuiltActual,
        });

        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

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
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row" style={{ marginTop: "1em" }}>
              <div className="col" align="right">
                {/* <button
                  className="btn btn-outline-primary mr-4"
                  type="button"
                  data-toggle="collapse"
                  aria-expanded="false"
                  onClick={() => this.descargarFormatoCargaMasiva()}
                >
                  Descargar formato carga masiva
               </button>*/}
                <button
                  className="btn btn-outline-primary mr-4"
                  type="button"
                  data-toggle="collapse"
                  data-target="#collapseExample"
                  aria-expanded="false"
                  aria-controls="collapseExample"
                >
                  Cargar Porcentajes
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
                  name="FechaCarga"
                  value={this.state.FechaCarga}
                  label="Fecha Carga"
                  type="date"
                  onChange={this.handleChangeFechas}
                  error={this.state.errors.FechaCarga}
                  edit={true}
                  readOnly={false}
                />
              </div>

              <div className="col"></div>
            </div>

            <div className="row" style={{ marginLeft: "0.1em" }}>
              <Button
                label="Buscar"
                className="p-button-outlined"
                onClick={() => this.obtenerDetalles()}
                icon="pi pi-search"
              />
            </div>

            <div className="row" style={{ marginTop: "1em" }}>
              <div className="col" align="right">
                <button
                  className="btn btn-outline-primary mr-4"
                  type="button"
                  data-toggle="collapse"
                  aria-expanded="false"
                  onClick={() => this.mostrarFormulario({})}
                >
                  Nuevo
                </button>
              </div>
            </div>
            <hr />
            <div className="row">
              <div className="col">
                <DetallesIngenieriaTable
                  data={this.state.detalles}
                  mostrarFormulario={this.mostrarFormulario}
                  mostrarConfirmacionParaEliminar={
                    this.mostrarConfirmacionParaEliminar
                  }
                  EnviarValidado={this.EnviarValidado}
                />
              </div>
            </div>

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
          </div>
        </div>
      </Fragment>
    );
  };

  isValid = () => {
    const errors = {};

    if (this.state.ProyectoId === 0) {
      errors.ProyectoId = "El campo es requerido";
    }

    if (this.state.FechaTrabajo === "") {
      errors.FechaTrabajo = "El campo es requerido";
    }
    /*if (this.state.EtapaId === 0) {
      errors.EtapaId = "El campo es requerido";
    }*/
    if (this.state.e != null) {
      if (this.state.e.AvanceRealAnterior > this.state.NumeroHoras1) {
        errors.JustificacionActualizacion =
          "El campo Avance Real Actual no puede ser menor al Avance Real Anterior";
      }
    }
    console.log(errors);
    if (this.state.action === "edit") {
      console.log("EntroEditar", this.state.action);
      if (this.state.JustificacionActualizacion === "") {
        errors.JustificacionActualizacion = "El campo es requerido";
      }
    }

    if (this.state.AvanceRealActualIB > 1) {
      errors.AvanceRealActualIB =
        "El campo % Avance Real Actual IB no debe ser mayor a 1";
    }
    if (this.state.AvanceRealActualID > 1) {
      errors.AvanceRealActualID =
        "El campo % Avance Real Actual ID no debe ser mayor a 1";
    }
    if (this.state.AvancePrevistoActualIB > 1) {
      errors.AvancePrevistoActualIB =
        "El campo % Avance Previsto Actual IB no debe ser mayor a 1";
    }
    if (this.state.AvancePrevistoActualIB > 1) {
      errors.AvancePrevistoActualIB =
        "El campo % Avance Previsto Actual IB no debe ser mayor a 1";
    }
    if (this.state.AvancePrevistoActualID > 1) {
      errors.AvancePrevistoActualID =
        "El campo % Avance Previsto Actual ID no debe ser mayor a 1";
    }
    if (this.state.AsbuiltActual > 1) {
      errors.AsbuiltActual =
        "El campo % AsBuilt Actual ID no debe ser mayor a 1";
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
    console.log("action", this.state.action);
    let url = "";
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_AVANCES_PROYECTO}`;
    if (this.state.action === "edit") {
      url += "/Editar";
    } else {
      url += "/Crear";
    }

    console.log(url);

    http
      .post(url, {
        Id: this.state.Id,
        ProyectoId: this.state.ProyectoId,
        CertificadoId: this.state.CertificadoId,
        FechaCertificado: this.state.FechaTrabajo,
        Justificacion: this.state.JustificacionActualizacion,
        AvancePrevistoActualIB: this.state.AvancePrevistoActualIB,
        AvanceRealActualIB: this.state.AvanceRealActualIB,
        AvancePrevistoActualID: this.state.AvancePrevistoActualID,
        AvanceRealActualID: this.state.AvanceRealActualID,
        AsbuiltActual: this.state.AsbuiltActual,
      })
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          this.props.showSuccess("Completada Correctamente");
          this.onHideFormulario();
          this.obtenerDetalles();
        } else {
          this.props.showWarn(
            "ya existe un registro con fecha de certificacion mayor a la que intenta registrar"
          );
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
      Id: 0,
      Identificacion: "",
      ColaboradorId: 0,
      Colaborador: null,
      TipoRegistroId: 0,
      CodigoProyecto: "",
      ProyectoId: 0,
      NumeroHoras: 0,
      NumeroHoras1: 0,
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
      AvancePrevistoAnteriorIB: 0.0,
      AvanceRealAnteriorIB: 0.0,
      AvancePrevistoActualIB: 0.0,
      AvanceRealActualIB: 0.0,

      AvancePrevistoAnteriorID: 0.0,
      AvanceRealAnteriorID: 0.0,
      AvancePrevistoActualID: 0.0,
      AvanceRealActualID: 0.0,

      AsbuiltActual: 0.0,
      AsbuiltAnterior: 0.0,
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
  onChangeValueProyecto = (name, value) => {
    if (this.state.FechaTrabajo === "") {
      this.props.showWarn("El campo fecha es requerido");
      return;
    }

    console.log(name);
    console.log(value);
    this.setState({
      [name]: value,
    });
    this.buscarDatos(value, "");
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
                    <Field
                      name="FechaTrabajo"
                      value={this.state.FechaTrabajo}
                      label="Fecha Certificado"
                      type="date"
                      onChange={this.handleChangeFechas}
                      error={this.state.errors.FechaTrabajo}
                      edit={true}
                      readOnly={false}
                      required={true}
                    />
                  </div>
                  <div className="col">
                    <Field
                      name="ProyectoId"
                      label="Proyecto"
                      type="select"
                      options={this.state.Proyectos}
                      edit={true}
                      readOnly={false}
                      value={this.state.ProyectoId}
                      onChange={this.onChangeValueProyecto}
                      error={this.state.errors.ProyectoId}
                      placeholder="Seleccionar..."
                      required={true}
                      filter={true}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col-6">
                    <p>
                      <strong>Contrato: </strong>{" "}
                      {this.state.e != null ? this.state.e.nombreContrato : ""}
                    </p>
                    <p>
                      <strong>% Avance Previsto Anterior IB: </strong>
                      {this.state.e != null
                        ? this.state.e.AvancePrevistoActualIB
                        : ""}
                    </p>
                    <p>
                      <strong>% Avance Real Anterior IB: </strong>
                      {this.state.e != null
                        ? this.state.e.AvanceRealActualIB
                        : ""}
                    </p>
                    <p>
                      <strong>% AsBuilt Anterior: </strong>
                      {this.state.e != null ? this.state.e.AsbuiltActual : ""}
                    </p>
                  </div>
                  <div className="col-6">
                    <p>
                      <strong>Proyecto: </strong>
                      {this.state.e != null ? this.state.e.nombreProyecto : ""}
                    </p>

                    <p>
                      <strong>% Avance Previsto Anterior ID: </strong>
                      {this.state.e != null
                        ? this.state.e.AvancePrevistoActualID
                        : ""}
                    </p>
                    <p>
                      <strong>% Avance Real Actual ID: </strong>
                      {this.state.e != null
                        ? this.state.e.AvanceRealActualID
                        : ""}
                    </p>
                  </div>
                </div>

                <div className="row">
                  <div className="col-6">
                    <Field
                      name="AvanceRealActualIB"
                      value={this.state.AvanceRealActualIB}
                      label="% Avance Real Actual IB"
                      type="NUMBER"
                      min="0"
                      max="1"
                      onChange={this.handleChange}
                      error={this.state.errors.AvanceRealActualIB}
                      readOnly={false}
                      required={true}
                      edit={true}
                    />
                  </div>
                  <div className="col-6">
                    <Field
                      name="AvanceRealActualID"
                      value={this.state.AvanceRealActualID}
                      label="% Avance Real Actual ID"
                      type="NUMBER"
                      min="0"
                      max="1"
                      onChange={this.handleChange}
                      error={this.state.errors.AvanceRealActualID}
                      readOnly={false}
                      required={true}
                      edit={true}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col-6">
                    <Field
                      name="AvancePrevistoActualIB"
                      value={this.state.AvancePrevistoActualIB}
                      label="% Avance Previsto Actual IB"
                      type="NUMBER"
                      min="0"
                      max="1"
                      onChange={this.handleChange}
                      error={this.state.errors.AvancePrevistoActualIB}
                      readOnly={false}
                      required={true}
                      edit={true}
                    />
                  </div>
                  <div className="col-6">
                    <Field
                      name="AvancePrevistoActualID"
                      value={this.state.AvancePrevistoActualID}
                      label="% Avance Previsto Actual ID"
                      type="NUMBER"
                      onChange={this.handleChange}
                      error={this.state.errors.AvancePrevistoActualID}
                      readOnly={false}
                      min="0"
                      max="1"
                      required={true}
                      edit={true}
                    />
                  </div>
                </div>

                <div className="row">
                  <div className="col-6">
                    <Field
                      name="AsbuiltActual"
                      value={this.state.AsbuiltActual}
                      label="% AsBuilt Actual"
                      type="NUMBER"
                      min="0"
                      max="1"
                      onChange={this.handleChange}
                      error={this.state.errors.AsbuiltActual}
                      readOnly={false}
                      required={true}
                      edit={true}
                    />
                  </div>
                  <div className="col-6"></div>
                </div>
                <div className="row">
                  <div className="col"></div>
                </div>

                <div className="row">
                  <div className="col">
                    <Field
                      name="JustificacionActualizacion"
                      value={this.state.JustificacionActualizacion}
                      label="Justificación"
                      type="text"
                      onChange={this.handleChange}
                      error={this.state.errors.JustificacionActualizacion}
                      readOnly={false}
                      edit={true}
                    />
                  </div>
                </div>
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
    this.props.blockScreen();
    var self = this;
    Promise.all([this.PromiseDetalles()])
      .then(function ([listdetalles]) {
        let detallesData = listdetalles.data;
        if (detallesData.success === true) {
          self.setState({
            detalles: detallesData.result,
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
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_AVANCES_PROYECTO}/Delete/${this.state.detalleSeleccionado.Id}`;

    http
      .post(url, {})
      .then((response) => {
        console.log(response);
        let data = response.data;

        if (data.success === true) {
          console.log("data", data.result);
          if (data.result === "OK") {
            this.props.showSuccess("Eliminado Correctamente");
            console.log("Eliminado");
            this.onHideFormulario(true);
            this.obtenerDetalles();
          }
          if (data.result === "CERTIFICADO_MAYOR") {
            this.props.showWarn(
              "No se puede eliminar debido a que existe un registro con fecha de carga mayor en el proyecto"
            );
          }
        } else {
          var message = data.result;
          console.log("message", message);
          if (message === "CERTIFICADO_MAYOR") {
            this.props.showWarn(
              "No se puede eliminar debido a que existe un registro con fecha de carga mayor en el proyecto"
            );
          }

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
        e: null,
        Id: 0,
        action: "nuevo",
      });
    } else {
      console.log("EDIT");
      this.setState({
        detalleSeleccionado: detalle,
        screen: "form",
        action: "edit",
        Id: detalle.Id,
        ProyectoId: detalle.ProyectoId,
        NumeroHoras: detalle.AvancePrevistoActual,
        NumeroHoras1: detalle.AvanceRealActual,
        FechaTrabajo: detalle.FechaCertificado,
        CertificadoId: detalle.CertificadoId,
        JustificacionActualizacion: detalle.Justificacion,

        AvancePrevistoAnteriorIB: detalle.AvancePrevistoAnteriorIB,
        AvanceRealAnteriorIB: detalle.AvanceRealAnteriorIB,

        AvancePrevistoAnteriorID: detalle.AvancePrevistoAnteriorID,
        AvanceRealAnteriorID: detalle.AvanceRealAnteriorID,
        AvancePrevistoActualIB: detalle.AvancePrevistoActualIB,
        AvanceRealActualIB: detalle.AvanceRealActualIB,
        AvancePrevistoActualID: detalle.AvancePrevistoActualID,
        AvanceRealActualID: detalle.AvanceRealActualID,
        AsbuiltActual: detalle.AsbuiltActual,
      });

      if (detalle.ProyectoId != null) {
        this.onChangeValueProyecto;
      }
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
    let params = `?fechaCarga=${this.state.FechaCarga}`;
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_AVANCES_PROYECTO}/GetDetalles${params}`;
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
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_AVANCES_PROYECTO}/DescargarPlantillaCargaMasiva`,
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
          `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_AVANCES_PROYECTO}/CargaMasivaDetalles`,
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
}

const Container = Wrapper(AvancePorcentajeProyectoContainer);
ReactDOM.render(
  <Container />,
  document.getElementById("avances_porcentaje_proyecto")
);
