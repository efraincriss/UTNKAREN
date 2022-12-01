import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";
import { Calendar } from "primereact-v2/calendar";
import { SelectButton } from "primereact-v2/selectbutton";
import ObraDisruptivo from "../ObraDisruptivo";
import moment from "moment";

import TimePicker from "rc-time-picker";

const showSecond = true;
const str = showSecond ? "HH:mm:ss" : "HH:mm";

class ComentarioList extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: false,
      visible: false,
      visiblepreci: false,
      //Detalles Proyecto
      Proyecto: null,
      esRSO: false,
      opcionesSeleccion: [
        { label: "RSO", value: true },
        { label: "RDO", value: false },
      ],
      errors: {},
      errorsprecipitacion: {},
      observaciones: [],
      actividadesre: [],
      actividadespr: [],
      precipitaciones: [],
      viewobservacion: false,
      //Formulario
      Id: 0,
      TipoObservaciones: [],
      TipoPrecipitacion: [
        { label: "DIURNA", value: 1 },
        { label: "NOCTURA", value: 2 },
      ],
      Fecha: new Date(),
      Observacion: "",
      FechaObservacion: new Date(),
      TipoObservacionId: 0,
      Tipo: 0,
      nombretab: "",
      nombredescripcion: "",
      accion: "create",

      IdPrecipitacion: 0,

      Hora_inicio: null,
      Hora_fin: null,
      Cantidad: 1,
      TipoPrecipitacionId: 1,
      accionprecipitacion: "create",
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);
    this.MostrarFormularioPrecipitacion = this.MostrarFormularioPrecipitacion.bind(
      this
    );
    this.OcultarFormularioPrecipitacion = this.OcultarFormularioPrecipitacion.bind(
      this
    );
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.EnviarFormularioPrecipitacion = this.EnviarFormularioPrecipitacion.bind(
      this
    );
    this.onChangeHoraInicio = this.onChangeHoraInicio.bind(this);
    this.onChangeHoraFin = this.onChangeHoraFin.bind(this);
    //METODOS

    this.ObtenerDetalleProyecto = this.ObtenerDetalleProyecto.bind(this);
    this.ObtenerDetalles = this.ObtenerDetalles.bind(this);
    this.ObtenerCatalogos = this.ObtenerCatalogos.bind(this);
    this.generarBotones = this.generarBotones.bind(this);
    this.generarBotonesrealizadas = this.generarBotonesrealizadas.bind(this);
    this.generarBotonesprogramadas = this.generarBotonesprogramadas.bind(this);
    this.generarBotonesprecipitacion = this.generarBotonesprecipitacion.bind(
      this
    );
    this.VaciarCampos = this.VaciarCampos.bind(this);
    this.RedireccionarDetalle = this.RedireccionarDetalle.bind(this);
    this.isValid = this.isValid.bind(this);
    this.isValidPrecipitacion = this.isValidPrecipitacion.bind(this);
  }

  componentDidMount() {
    this.ObtenerCatalogos();
    this.ObtenerDetalleProyecto();
    this.ObtenerDetalles();
  }
  isValid() {
    const errors = {};

    if (this.state.TipoObservacionId == 0) {
      errors.TipoObservacionId = "Campo requerido";
    }
    if (this.state.Observacion == null || this.state.Observacion == "") {
      errors.Observacion = "Campo requerido";
    }
    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }

  isValidPrecipitacion() {
    const errorsprecipitacion = {};

    if (this.state.TipoPrecipitacionId == 0) {
      errorsprecipitacion.Tipo = "Campo requerido";
    }
    if (this.state.Fecha == null || this.state.Fecha == "") {
      errorsprecipitacion.Fecha = "Campo requerido";
    }
    if (this.state.Cantidad == 0) {
      errorsprecipitacion.Cantidad = "Campo requerido";
    }
    this.setState({ errorsprecipitacion });
    return Object.keys(errorsprecipitacion).length === 0;
  }
  VaciarCampos() {
    this.setState({
      Id: 0,
      Observacion: "",
      FechaObservacion: new Date(),
      TipoObservacionId: 0,
      Tipo: 0,
      nombretab: "",
      accion: "create",
    });
  }
  ObtenerDetalleProyecto() {
    console.log("-Detalle Proyecto");
    axios
      .post("/Proyecto/ProyectoObservacion/GetDetallesProyecto", {
        Id: document.getElementById("ProyectoId").className,
      })
      .then((response) => {
        if (response.data != "Error") {
          console.log(response.data);
          this.setState({
            Proyecto: response.data,
            esRSO: response.data.es_RSO,
          });
          this.props.unlockScreen();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  }
  ObtenerCatalogos() {
    console.log("Catalogos");
    axios
      .post("/Proyecto/ProyectoObservacion/GetTipoObservaciones", {
        id: "TIPOOBSERVACION",
      })
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ TipoObservaciones: items });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(
          "Ocurrió un error al consultar Catálogo TIPO OBSERVACIÓN"
        );
      });
  }
  ObtenerDetalles() {
    console.log("Obsercaciones");
    axios
      .post("/Proyecto/ProyectoObservacion/GetAll", {
        id: document.getElementById("ProyectoId").className,
      })
      .then((response) => {
        console.log(response.data.result);
        this.setState({
          observaciones: response.data.result.observaciones,
          actividadesre: response.data.result.actividadesre,
          actividadespr: response.data.result.actividadespr,
          precipitaciones: response.data.result.precipitaciones,
        });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(
          "Ocurrió un error al consultar Observaciones y Actividades"
        );
      });
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };

  RedireccionarDetalle() {
    window.location.href = "/Proyecto/ProyectoObservacion/IndexProyectos";
  }

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  generarBotones = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormulario(row, 1)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.EliminarObservacion(row.Id);
          }}
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };
  generarBotonesrealizadas = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormulario(row, 2)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.EliminarObservacion(row.Id);
          }}
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };
  generarBotonesprogramadas = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormulario(row, 3)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.EliminarObservacion(row.Id);
          }}
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };

  generarBotonesprecipitacion = (cell, row) => {
    return (
      <div>
        {/*<button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormularioPrecipitacion(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
        >
          <i className="fa fa-edit" />
        </button>*/}
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.EliminarPrecipitacion(row.Id);
          }}
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };

  EliminarObservacion = (Id) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/ProyectoObservacion/Delete/" + Id, {})
      .then((response) => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente");
          this.ObtenerDetalles();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("Ocurrió un error al Eliminar");
      });
  };

  EliminarPrecipitacion = (Id) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/ProyectoObservacion/EliminarPrecipitacion/" + Id, {})
      .then((response) => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente");
          this.ObtenerDetalles();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("Ocurrió un error al Eliminar");
      });
  };

  actualizarValueRSO = (value) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/ProyectoObservacion/cambiarRSO/", {
        Id: this.state.Proyecto != null ? this.state.Proyecto.Id : 0,
        esRSO: value,
      })
      .then((response) => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("Actualizado Correctamente");
          this.ObtenerDetalleProyecto();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("Ocurrió un error al actualizar el campo es RSO");
      });
  };

  render() {
    return (
      <div>
        <Growl
          ref={(el) => {
            this.growl = el;
          }}
          baseZIndex={1000}
        />

        <div className="row">
          <div style={{ width: "100%" }}>
            <div className="card">
              <div className="card-body">
                <div className="content-section implementation">
                  <div className="row">
                    <div className="col-8">
                      <h5>Proyecto:</h5>
                    </div>

                    <div className="col-4" align="right">
                      <button
                        style={{ marginLeft: "0.3em" }}
                        className="btn btn-outline-primary"
                        onClick={() => this.RedireccionarDetalle()}
                      >
                        Regresar
                      </button>
                    </div>
                  </div>
                  <br />

                  <div>
                    <div className="row">
                      <div className="col-6">
                        <h6>
                          <b>Código:</b>{" "}
                          {this.state.Proyecto != null
                            ? this.state.Proyecto.codigo
                            : ""}
                        </h6>
                        <h6>
                          <b>Nombre:</b>
                          {this.state.Proyecto != null
                            ? this.state.Proyecto.nombre_proyecto
                            : ""}
                        </h6>
                        <h6>
                          <b>Descripción:</b>{" "}
                          {this.state.Proyecto != null
                            ? this.state.Proyecto.descripcion_proyecto
                            : ""}
                        </h6>
                      </div>
                      <div className="col-6">
                        <h6>
                          <strong>Tipo de Proyecto</strong>
                        </h6>
                        <SelectButton
                          value={this.state.esRSO}
                          options={this.state.opcionesSeleccion}
                          onChange={(e) => this.actualizarValueRSO(e.value)}
                        />
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="row">
          <div style={{ width: "100%" }}>
            <ul className="nav nav-tabs" id="gestion_tabs" role="tablist">
              <li className="nav-item">
                <a
                  className="nav-link active"
                  id="gestion-tab"
                  data-toggle="tab"
                  href="#gestion"
                  role="tab"
                  aria-controls="home"
                  aria-expanded="true"
                >
                  Observaciones {/* <i className="fa fa-bus fa-1x" />*/}
                </a>
              </li>
              <li className="nav-item">
                <a
                  className="nav-link"
                  id="op-tab"
                  data-toggle="tab"
                  href="#op"
                  role="tab"
                  aria-controls="home"
                  aria-expanded="true"
                >
                  Actividades Realizadas
                  {/*<i className="fa fa-history fa-1x" />*/}
                </a>
              </li>
              <li className="nav-item">
                <a
                  className="nav-link"
                  id="opp-tab"
                  data-toggle="tab"
                  href="#opp"
                  role="tab"
                  aria-controls="home"
                  aria-expanded="true"
                >
                  Actividades Programadas
                  {/*<i className="fa fa-history fa-1x" />*/}
                </a>
              </li>
              <li className="nav-item">
                <a
                  className="nav-link"
                  id="oppre-tab"
                  data-toggle="tab"
                  href="#oppre"
                  role="tab"
                  aria-controls="home"
                  aria-expanded="true"
                >
                  Precipitaciones{/*<i className="fa fa-history fa-1x" />*/}
                </a>
              </li>
              <li className="nav-item">
                <a
                  className="nav-link"
                  id="disrup-tab"
                  data-toggle="tab"
                  href="#disrup"
                  role="tab"
                  aria-controls="home"
                  aria-expanded="true"
                >
                  Disruptivos
                </a>
              </li>
            </ul>
            <div className="tab-content" id="myTabContent">
              <div
                className="tab-pane fade show active"
                id="gestion"
                role="tabpanel"
                aria-labelledby="gestion-tab"
              >
                <div className="col" align="right">
                  <button
                    className="btn btn-outline-primary"
                    onClick={() => this.MostrarFormulario(null, 1)}
                  >
                    Nueva Observación
                  </button>
                </div>
                <br />
                <BootstrapTable
                  data={this.state.observaciones}
                  hover={true}
                  pagination={true}
                >
                  <TableHeaderColumn
                    dataField="any"
                    dataFormat={this.Secuencial}
                    width={"8%"}
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  >
                    Nº
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Observacion"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Observación
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="FormatFecha"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Fecha Observación
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="NombreTipoObservacion"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Tipo Observación
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    isKey
                    dataField="Id"
                    width={"15%"}
                    dataFormat={this.generarBotones}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  >
                    {" "}
                    Opciones
                  </TableHeaderColumn>
                </BootstrapTable>
              </div>
              <div
                className="tab-pane fade"
                id="op"
                role="tabpanel"
                aria-labelledby="op-tab"
              >
                <div className="col" align="right">
                  <button
                    className="btn btn-outline-primary"
                    onClick={() => this.MostrarFormulario(null, 2)}
                  >
                    Nueva Actividad Realizada
                  </button>
                </div>
                <br />
                <BootstrapTable
                  data={this.state.actividadesre}
                  hover={true}
                  pagination={true}
                >
                  <TableHeaderColumn
                    dataField="any"
                    dataFormat={this.Secuencial}
                    width={"8%"}
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  >
                    Nº
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Observacion"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Observación
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="FormatFecha"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Fecha Observación
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="NombreTipoObservacion"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Tipo Observación
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    isKey
                    dataField="Id"
                    width={"15%"}
                    dataFormat={this.generarBotonesrealizadas}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  >
                    {" "}
                    Opciones
                  </TableHeaderColumn>
                </BootstrapTable>
              </div>
              <div
                className="tab-pane fade"
                id="opp"
                role="tabpanel"
                aria-labelledby="opp-tab"
              >
                <div className="col" align="right">
                  <button
                    className="btn btn-outline-primary"
                    onClick={() => this.MostrarFormulario(null, 3)}
                  >
                    Nueva Actividad Programada
                  </button>
                </div>
                <br />
                <BootstrapTable
                  data={this.state.actividadespr}
                  hover={true}
                  pagination={true}
                >
                  <TableHeaderColumn
                    dataField="any"
                    dataFormat={this.Secuencial}
                    width={"8%"}
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  >
                    Nº
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Observacion"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Observación
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="FormatFecha"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Fecha Observación
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="NombreTipoObservacion"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Tipo Observación
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    isKey
                    dataField="Id"
                    width={"15%"}
                    dataFormat={this.generarBotonesprogramadas}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  >
                    {" "}
                    Opciones
                  </TableHeaderColumn>
                </BootstrapTable>
              </div>
              <div
                className="tab-pane fade"
                id="oppre"
                role="tabpanel"
                aria-labelledby="oppre-tab"
              >
                <div className="col" align="right">
                  <button
                    className="btn btn-outline-primary"
                    onClick={() => this.MostrarFormularioPrecipitacion(null)}
                  >
                    Nueva Precipitacion
                  </button>
                </div>
                <br />
                <BootstrapTable
                  data={this.state.precipitaciones}
                  hover={true}
                  pagination={true}
                >
                  <TableHeaderColumn
                    dataField="any"
                    dataFormat={this.Secuencial}
                    width={"8%"}
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  >
                    Nº
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="fechaformat"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Fecha
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="nombretipo"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Tipo
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="horainicioformat"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Hora Inicio
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="horafinformat"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Hora Fin
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="CantidadDiaria"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Cantidad
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    isKey
                    dataField="Id"
                    width={"15%"}
                    dataFormat={this.generarBotonesprecipitacion}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  >
                    {" "}
                    Opciones
                  </TableHeaderColumn>
                </BootstrapTable>
              </div>

              <div
                className="tab-pane fade"
                id="disrup"
                role="tabpanel"
                aria-labelledby="disrup-tab"
              >
                <ObraDisruptivo />
              </div>
            </div>
          </div>
        </div>

        <Dialog
          header="Nueva Información"
          visible={this.state.visible}
          width="600px"
          modal={true}
          onHide={this.OcultarFormulario}
        >
          <div>
            <form onSubmit={this.EnviarFormulario} height="730px">
              <div className="row">
                <div className="col">
                  <Field
                    name="TipoObservacionId"
                    required
                    value={this.state.TipoObservacionId}
                    label="Tipo de Observación"
                    options={this.state.TipoObservaciones}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.TipoObservacionId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <Field
                    name="FechaObservacion"
                    label="Fecha"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.FechaObservacion}
                    onChange={this.handleChange}
                    error={this.state.errors.FechaObservacion}
                  />

                  <Field
                    name="Observacion"
                    label={this.state.nombredescripcion}
                    type={"textarea"}
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Observacion}
                    onChange={this.handleChange}
                    error={this.state.errors.Observacion}
                  />
                </div>
              </div>
              <button type="submit" className="btn btn-outline-primary">
                Guardar
              </button>
              &nbsp;
              <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                onClick={this.OcultarFormulario}
              >
                Cancelar
              </button>
            </form>
          </div>
        </Dialog>
        <Dialog
          header="Nueva Precipitacion"
          visible={this.state.visiblepreci}
          width="600px"
          modal={true}
          onHide={this.OcultarFormularioPrecipitacion}
        >
          <div>
            <form onSubmit={this.EnviarFormularioPrecipitacion} height="730px">
              <div className="row">
                <div className="col">
                  <Field
                    name="TipoPrecipitacionId"
                    required
                    value={this.state.TipoPrecipitacionId}
                    label="Tipo de Observación"
                    options={this.state.TipoPrecipitacion}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errorsprecipitacion.TipoPrecipitacionId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <Field
                    name="Fecha"
                    label="Fecha"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.Fecha}
                    onChange={this.handleChange}
                    error={this.state.errorsprecipitacion.Fecha}
                  />

                  <div className="row">
                    <div className="col">
                      <div className="form-group">
                        <label>Hora Inicio</label>
                        <br />
                        <Calendar
                          value={this.state.Hora_inicio}
                          onChange={(e) =>
                            this.setState({ Hora_inicio: e.value })
                          }
                          timeOnly={true}
                          hourFormat="12"
                        />
                      </div>
                    </div>
                    <div className="col">
                      <div className="form-group">
                        <label>Hora Fin</label>
                        <br />
                        <Calendar
                          value={this.state.Hora_fin}
                          onChange={(e) => this.setState({ Hora_fin: e.value })}
                          timeOnly={true}
                          hourFormat="12"
                        />
                      </div>
                    </div>
                  </div>

                  <Field
                    name="Cantidad"
                    label="Cantidad (mm)"
                    type={"number"}
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Cantidad}
                    onChange={this.handleChange}
                    error={this.state.errorsprecipitacion.Cantidad}
                  />
                </div>
              </div>
              <button type="submit" className="btn btn-outline-primary">
                Guardar
              </button>
              &nbsp;
              <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                onClick={this.OcultarFormulario}
              >
                Cancelar
              </button>
            </form>
          </div>
        </Dialog>
      </div>
    );
  }
  onChangeHoraFin(value) {
    this.setState({ Hora_fin: value });
  }

  onChangeHoraInicio(value) {
    console.log(value);
    this.setState({ Hora_inicio: value });
  }
  EnviarFormulario(event) {
    event.preventDefault();
    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      console.log(this.state.accion);
      if (this.state.accion == "create") {
        this.props.blockScreen();
        axios
          .post("/Proyecto/ProyectoObservacion/Create", {
            Id: 0,
            ProyectoId: document.getElementById("ProyectoId").className,
            TipoObservacionId: this.state.TipoObservacionId,
            Tipo: this.state.Tipo,
            FechaObservacion: this.state.FechaObservacion,
            Observacion: this.state.Observacion,
            vigente: true,
          })
          .then((response) => {
            console.log(response.data);
            if (response.data === "OK") {
              this.props.showSuccess("Creado Correctamente");
              this.ObtenerDetalles();

              this.setState({ visible: false });
            }
          })
          .catch((error) => {
            console.log(error);
            this.props.unlockScreen();
          });
      } else {
        this.props.blockScreen();
        axios
          .post("/Proyecto/ProyectoObservacion/Edit", {
            Id: this.state.Id,
            ProyectoId: document.getElementById("ProyectoId").className,
            TipoObservacionId: this.state.TipoObservacionId,
            Tipo: this.state.Tipo,
            FechaObservacion: this.state.FechaObservacion,
            Observacion: this.state.Observacion,
            vigente: true,
          })
          .then((response) => {
            console.log(response.data);
            if (response.data === "OK") {
              this.props.showSuccess("Editado Correctamente");
              this.ObtenerDetalles();

              this.setState({ visible: false });
            }
          })
          .catch((error) => {
            console.log(error);
            this.props.unlockScreen();
          });
      }
    }
  }

  EnviarFormularioPrecipitacion(event) {
    event.preventDefault();
    if (!this.isValidPrecipitacion()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      if (this.state.Hora_inicio == "") {
        abp.notify.error("Campo Hora Inicio es Obligatorio", "Validación");
        return;
      }
      if (this.state.Hora_fin == "") {
        abp.notify.error("Campo Hora Fin es Obligatorio", "Validación");
        return;
      }

      console.log(this.state.accion);
      if (this.state.accionprecipitacion == "create") {
        console.log(this.state.Hora_inicio.toLocaleTimeString());
        this.props.blockScreen();
        axios
          .post("/Proyecto/ProyectoObservacion/CreatePrecipitacion", {
            Id: 0,
            ProyectoId: document.getElementById("ProyectoId").className,
            Tipo: this.state.TipoPrecipitacionId,
            Fecha: this.state.Fecha,
            Hora_inicio: this.state.Hora_inicio.toLocaleTimeString(),
            Hora_fin: this.state.Hora_fin.toLocaleTimeString(),
            CantidadDiaria: this.state.Cantidad,
            CantidadAnterior: 0,
            CantidadAcumulad: 0,
            vigente: true,
          })
          .then((response) => {
            console.log(response.data);
            if (response.data === "OK") {
              this.props.showSuccess("Creado Correctamente");
              this.ObtenerDetalles();

              this.setState({ visiblepreci: false });
            }
          })
          .catch((error) => {
            console.log(error);
            this.props.unlockScreen();
          });
      } else {
        this.props.blockScreen();
        axios
          .post("/Proyecto/ProyectoObservacion/EditPrecipitacion", {
            Id: this.state.IdPrecipitacion,
            ProyectoId: document.getElementById("ProyectoId").className,
            Tipo: this.state.TipoPrecipitacion,
            Fecha: this.state.Fecha,
            Hora_inicio: this.state.Hora_inicio,
            Hora_fin: this.state.Hora_fin,
            CantidadDiaria: this.state.Cantidad,
            CantidadAnterior: 0,
            CantidadAcumulad: 0,
            vigente: true,
          })
          .then((response) => {
            console.log(response.data);
            if (response.data === "OK") {
              this.props.showSuccess("Editado Correctamente");
              this.ObtenerDetalles();

              this.setState({ visiblepreci: false });
            }
          })
          .catch((error) => {
            console.log(error);
            this.props.unlockScreen();
          });
      }
    }
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }
  OcultarFormulario() {
    this.setState({ visible: false, TipoObservacionId: 0, Observacion: "" });
  }
  OcultarFormularioPrecipitacion() {
    this.setState({ visiblepreci: false });
  }
  MostrarFormularioPrecipitacion(row) {
    if (row != undefined && row.Id > 0) {
      console.log("eDITAR");
      this.setState({
        IdPrecipitacion: row.Id,
        Cantidad: row.CantidadDiaria,
        TipoPrecipitacionId: row.Tipo,
        Fecha: row.Fecha,
        accionprecipitacion: "edit",
        visiblepreci: true,
      });
    } else {
      console.log("Nueva");
      this.setState({
        accionprecipitacion: "create",
        visiblepreci: true,
        Cantidad: 0,
        Fecha: new Date(),
        Hora_inicio: "",
        Hora_fin: "",
      });
    }
  }

  MostrarFormulario(row, type) {
    this.setState({ TipoObservacionId: 0, Observacion: "" });
    if (row != undefined && row.Id > 0) {
      console.log("eDITAR");
      this.setState({
        Id: row.Id,
        Observacion: row.Observacion,
        TipoObservacionId: row.TipoObservacionId,
        Tipo: type,
        FechaObservacion: row.FechaObservacion,
        nombretab: "Editar Información",
        accion: "edit",
        visible: true,
        nombredescripcion:
          type == 1
            ? "Observación"
            : type == 2
            ? "Actividad Realizada"
            : "Actividad Programada",
      });
    } else {
      console.log("Nueva");
      this.setState({
        nombretab: "Nueva Información",
        accion: "create",
        visible: true,
        Tipo: type,
        FechaObservacion: new Date(),
        nombredescripcion:
          type == 1
            ? "Observación"
            : type == 2
            ? "Actividad Realizada"
            : "Actividad Programada",
      });
    }
  }
}
const Container = wrapForm(ComentarioList);
ReactDOM.render(<Container />, document.getElementById("content"));
