import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import Field from "../../Base/Field-v2";
import {
  FRASE_ERROR_SELECCIONAR_LUGARORIGEN,
  FRASE_ERROR_SELECCIONAR_LUGARDESTINO
} from "../../Base/Strings";
import Wrapper from "../../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";
import { Card } from "primereact-v2/card";

class VehiculosRutas extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: true,
      data: [],
      visible: false,
      visibleeditar: false,
      errors: {},

      // Inputs del Formulario
      Identificador: 0,
      lugares: [],
      Codigo: "",
      Nombre: "",
      OrigenId: 0,
      DestinoId: 0,
      Distancia: 0.0,
      Duracion: 0,
      Sector: "",
      Descripcion: "",

      EstadoId: 0,
      //Horario
      RutaId: 0,
      Rutas: [],
      Horarioid: 0,
      Horarios: [],
      HorarioName: "",
      NombreDestino: "",
      NombreOrigen: "",
      Vehiculos: [],
      VehiculoId: 0,
      Observacion: "",
      FechaDesde: "",
      FechaHasta: "",
      horas: ["00:00"],
      TipoVehiculo: ""
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleChangeCorreo = this.handleChangeCorreo.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.handleChangeIdentificacion = this.handleChangeIdentificacion.bind(
      this
    );
    this.OcultarFormulario = this.OcultarFormulario.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);
    this.OcultarFormularioEditar = this.OcultarFormularioEditar.bind(this);
    this.MostrarFormularioEditar = this.MostrarFormularioEditar.bind(this);
    this.alertMessage = this.alertMessage.bind(this);
    this.infoMessage = this.infoMessage.bind(this);
    this.Loading = this.Loading.bind(this);
    this.StopLoading = this.StopLoading.bind(this);

    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.EnviarFormularioEditar = this.EnviarFormularioEditar.bind(this);

    //METODOS
    this.ObtenerRutas = this.ObtenerRutas.bind(this);
    this.ObtenerDetalleRuta = this.ObtenerDetalleRuta.bind(this);
    this.ObtenerCatalogos = this.ObtenerCatalogos.bind(this);
    this.generarBotones = this.generarBotones.bind(this);
    this.VaciarCampos = this.VaciarCampos.bind(this);
    this.EliminarRuta = this.EliminarRuta.bind(this);
    this.RedireccionarDetalle = this.RedireccionarDetalle.bind(this);
    this.onChangeValueHorario = this.onChangeValueHorario.bind(this);

    //
    this.onChangeValueRuta = this.onChangeValueRuta.bind(this);
    this.ObtenerRutasHorarios = this.ObtenerRutasHorarios.bind(this);
  }

  componentDidMount() {
    this.ObtenerCatalogos();
    this.props.unlockScreen();
  }

  VaciarCampos() {
    this.setState({
      Codigo: "",
      Nombre: "",
      OrigenId: 0,
      DestinoId: 0,
      Distancia: 0.0,
      Duracion: 0,
      Sector: "",
      Descripcion: "",
      EstadoId: 0,
      NombreDestino: "",
      NombreOrigen: "",
      Observacion: "",

      //VEHICULOS RUTAS

      RutaId: 0,
      Rutas: [],
      Horarioid: 0,
      Horarios: [],
      horas: ["07:00"]
    });
  }
  ObtenerRutas() {
    axios
      .post("/Transporte/RutaHorarioVehiculo/ListaRutaHorasVehiculo", {})
      .then(response => {
        this.setState({ data: response.data.result, blocking: false });
      })
      .catch(error => {
        console.log(error);
      });
  }

  ObtenerRutasHorarios(value) {
    if (this.state.RutaId > 0 && value > 0) {
      axios
        .post("/Transporte/RutaHorarioVehiculo/ListaByRutaHorario", {
          rutaid: this.state.RutaId,
          horarioid: value
        })
        .then(response => {
          this.setState({ data: response.data.result, blocking: false });
        })
        .catch(error => {
          console.log(error);
        });
    }
  }

  ObtenerDetalleRuta(Id) {
    axios
      .post("/Transporte/Ruta/ObtenerDetallesRuta", {
        Id: Id
      })
      .then(response => {
        if (response.data != "Error") {
          this.setState({
            Identificador: Id,
            Codigo: response.data.Codigo,
            Nombre: response.data.Nombre,
            OrigenId: response.data.OrigenId,
            DestinoId: response.data.DestinoId,
            Distancia: response.data.Distancia,
            Duracion: response.data.Duracion,
            Sector:
              response.data.Sector != null ? response.data.Sector.nombre : "",
            Descripcion: response.data.Descripcion,
            NombreDestino: response.data.Destino.Nombre,
            NombreOrigen: response.data.Origen.Nombre,
            EstadoId: 0,
            blocking: false
          });
        }
      })
      .catch(error => {
        console.log(error);
      });
  }

  ObtenerCatalogos() {
    this.Loading();
    axios
      .post("/Transporte/Ruta/ListaRutas", {})
      .then(response => {
        var items = response.data.result.map(item => {
          return { label: item.Nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Rutas: items, blocking: false });
      })
      .catch(error => {
        console.log(error);
        this.StopLoading();
      });

    axios
      .post("/Transporte/Vehiculo/GetAll", {})
      .then(response => {
        var items = response.data.result.map(item => {
          return {
            label: item.CodigoEquipoInventario + " - " + item.NumeroPlaca,
            dataKey: item.Id,
            value: item.Id
          };
        });
        this.setState({ Vehiculos: items, blocking: false });
      })
      .catch(error => {
        console.log(error);
        this.StopLoading();
      });

    this.StopLoading();
  }
  onChangeValue = (name, value) => {
    if (name == "VehiculoId") {
      axios
        .get("/Transporte/Vehiculo/GetTipoVehiculo/" + value, {})
        .then(response => {
          this.setState({ TipoVehiculo: response.data });
        })
        .catch(error => {
          console.log(error);
        });
    }

    this.setState({
      [name]: value
    });
  };

  onChangeValueHorario = (name, value) => {
    this.setState({
      [name]: value
    });

    this.ObtenerRutasHorarios(value);
  };

  onChangeValueRuta = (name, value) => {
    this.setState({
      [name]: value
    });
    this.ObtenerDetalleRuta(value);
    axios
      .post("/Transporte/Ruta/ObtenerHorariosRuta", {
        id: value
      })
      .then(response => {
        var items = response.data.map(item => {
          return { label: item.Horario, isKey: item.Id, value: item.Id };
        });
        this.setState({
          Horarioid: 0,
          Horarios: items,
          blocking: false,
          data: [],
          VehiculoId: 0
        });
        this.StopLoading();
      })
      .catch(error => {
        console.log(error);
        this.StopLoading();
      });
  };

  RedireccionarDetalle(Id) {
    window.location.href = "/Transporte/Ruta/Details/" + Id;
  }
  EliminarRuta(Id) {
    axios
      .post("/Transporte/RutaHorarioVehiculo/Delete", {
        Id: Id
      })
      .then(response => {
        if (response.data === "REGISTROS") {
          this.props.showWarn("No se puede eliminar tiene consumos realizados");
        } else {
          if (response.data != "Error") {
            this.infoMessage("Eliminado Correctamente");
            this.ObtenerRutasHorarios(this.state.Horarioid);
          }
        }
      })
      .catch(error => {
        console.log(error);
        this.alertMessage("Ocurrió un Error");
      });
  }
  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }
  isValid() {
    const errors = {};
    if (this.state.VehiculoId == 0) {
      errors.VehiculoId = "Campo Requerido";
    }
    if (
      this.state.FechaDesde === "" ||
      (this.state.FechaDesde != "" && this.state.FechaDesde < new Date())
    ) {
      errors.FechaDesde = "Campo debe ser mayor a la fecha actual";
    }
    if (this.state.Observacion.length == 0) {
      errors.Observacion = "Campo Requerido";
    }
    if (this.state.Observacion != null && this.state.Observacion.length > 100) {
      errors.Observacion = "Campo debe tener maximo 100 dígitos";
    }
    if (
      this.state.FechaHasta === "" ||
      (this.state.FechaHasta != "" && this.state.FechaHasta < new Date())
    ) {
      errors.FechaHasta = "Campo debe ser mayor a la fecha actual";
    }
    if (
      this.state.FechaDesde != "" &&
      this.state.FechaDesde > this.state.FechaHasta
    ) {
      this.props.showWarn("La Fecha Hasta debe ser mayor a la Fecha Desde");
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }
  generarBotones = (cell, row) => {
    return (
      <div>
        {/* <button
          className="btn btn-outline-success"
          onClick={() => this.RedireccionarDetalle(row.Id)}
        >
          <i className="fa fa-eye" />
        </button>*/}
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormularioEditar(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar Ruta Vehículo"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger"
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar Ruta Vehículo"
          style={{ marginLeft: "0.3em" }}
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.EliminarRuta(row.Id);
          }}
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };
  render() {
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={el => {
            this.growl = el;
          }}
          baseZIndex={1000}
        />
        <div className="content-section implementation">
          <div>
            <div className="row">
              <div className="col">
                <Field
                  name="RutaId"
                  required
                  value={this.state.RutaId}
                  label="Ruta"
                  options={this.state.Rutas}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValueRuta}
                  error={this.state.errors.RutaId}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
              <div className="col">
                <Field
                  name="Horarioid"
                  required
                  value={this.state.Horarioid}
                  label="Horario"
                  options={this.state.Horarios}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValueHorario}
                  error={this.state.errors.Horarioid}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
            </div>
            <br />
            <div className="row">
              <div className="col-xs-12 col-md-6">
                <h6 className="text-gray-700">
                  <b>Nombre:</b> {this.state.Nombre}
                </h6>
                <h6 className="text-gray-700">
                  <b>Lugar Origen:</b> {this.state.NombreOrigen}
                </h6>
                <h6 className="text-gray-700">
                  <b>Duración:</b> {this.state.Duracion} {" minutos"}
                </h6>
              </div>

              <div className="col-xs-12 col-md-6">
                <h6 className="text-gray-700">
                  <b>Descripción:</b> {this.state.Descripcion}
                </h6>
                <h6 className="text-gray-700">
                  <b>Lugar Destino:</b> {this.state.NombreDestino}
                </h6>

                <h6 className="text-gray-700">
                  <b>Sector:</b> {this.state.Sector}
                </h6>
              </div>
            </div>
          </div>
        </div>
        <hr />
        <br />
        <div align="right">
          <button
            type="button"
            className="btn btn-outline-primary"
            icon="fa fa-fw fa-ban"
            onClick={this.MostrarFormulario}
          >
            Asignar Vehículo
          </button>
        </div>
        <br />
        <div>
          <BootstrapTable data={this.state.data} hover={true} pagination={true}>
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
              dataField="CodigoVehiculo"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Código Vehículo
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="PlacaVehiculo"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Placa
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="TipoVehiculo"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Tipo Vehículo
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="CapacidadVehiculo"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Capacidad
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="FechaDesdeTexto"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Fecha Desde
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="FechaHastaTexto"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Fecha Hasta
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="HorarioSalida"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Hora Salida
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="HoraLlegada"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Hora Llegada
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Id"
              isKey
              width={"15%"}
              dataFormat={this.generarBotones}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            >
              Opciones
            </TableHeaderColumn>
          </BootstrapTable>
        </div>

        <Dialog
          header="Nueva Asignación Vehículo Ruta"
          visible={this.state.visible}
          width="730px"
          modal={true}
          onHide={this.OcultarFormulario}
        >
          <div>
            <form onSubmit={this.EnviarFormulario} height="730px">
              <div className="row">
                <div className="col">
                  <Field
                    name="Nombre"
                    label="Ruta"
                    edit={false}
                    readOnly={true}
                    value={this.state.Nombre}
                    error={this.state.errors.Nombre}
                  />
                  <Field
                    name="Horarioid"
                    required
                    value={this.state.Horarioid}
                    label="Hora Salida"
                    options={this.state.Horarios}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValueHorario}
                    error={this.state.errors.Horarioid}
                    readOnly={true}
                    placeholder="Horario"
                    filterPlaceholder="Horario"
                  />
                </div>
                <div className="col">
                  <b>
                    {" "}
                    <label className="col-sm-12 col-form-label">Duración</label>
                  </b>
                  <br />
                  {this.state.Duracion}
                  {" minutos"}
                  <br /> <br />
                  <div className="form-group">
                    <label className="col-sm-12 col-form-label">
                      * Hora de LLegada
                    </label>
                    <input
                      type="time"
                      id="appt"
                      name="horas"
                      min="7:00"
                      max="23:00"
                      value={this.state.horas}
                      className="form-control"
                      onChange={this.handleChange}
                      required
                    />
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="VehiculoId"
                    required
                    value={this.state.VehiculoId}
                    label="Vehículo (Código - Placa)"
                    options={this.state.Vehiculos}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.VehiculoId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="TipoVehiculo"
                    label="Tipo de Vehículo"
                    edit={false}
                    readOnly={true}
                    value={this.state.TipoVehiculo}
                    onChange={this.handleChange}
                    error={this.state.errors.TipoVehiculo}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="FechaDesde"
                    label="Fecha Desde"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.FechaDesde}
                    onChange={this.handleChange}
                    error={this.state.errors.FechaDesde}
                  />
                </div>
                <div className="col">
                  <Field
                    name="FechaHasta"
                    label="Fecha Hasta"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.FechaHasta}
                    onChange={this.handleChange}
                    error={this.state.errors.FechaHasta}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="Observacion"
                    label="Observación"
                    type={"textarea"}
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
          header="Editar Asignación Vehículo Ruta"
          visible={this.state.visibleeditar}
          width="730px"
          modal={true}
          onHide={this.OcultarFormularioEditar}
        >
          <div>
            <form onSubmit={this.EnviarFormularioEditar} height="730px">
              <div className="row">
                <div className="col">
                  <Field
                    name="Nombre"
                    label="Ruta"
                    edit={false}
                    readOnly={true}
                    value={this.state.Nombre}
                    error={this.state.errors.Nombre}
                  />
                  <Field
                    name="Horarioid"
                    required
                    value={this.state.Horarioid}
                    label="Hora Salida"
                    options={this.state.Horarios}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValueHorario}
                    error={this.state.errors.Horarioid}
                    readOnly={true}
                    placeholder="Horario"
                    filterPlaceholder="Horario"
                  />
                  <Field
                    name="VehiculoId"
                    required
                    value={this.state.VehiculoId}
                    label="Vehículo (Código - Placa)"
                    options={this.state.Vehiculos}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.VehiculoId}
                    readOnly={false}
                    placeholder="Vehiculo"
                    filterPlaceholder="Vehiculo"
                  />
                  <Field
                    name="FechaDesde"
                    label="Fecha Desde"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.FechaDesde}
                    onChange={this.handleChange}
                    error={this.state.errors.FechaDesde}
                  />
                </div>

                <div className="col">
                  <div className="form-group">
                    <b>
                      <label className="col-sm-12 col-form-label">
                        Duración
                      </label>
                    </b>
                    <br />
                    {this.state.Duracion}
                    {" minutos"}
                  </div>
                  <br />
                  <div className="form-group">
                    <label className="col-sm-12 col-form-label">
                      * Hora de LLegada
                    </label>
                    <input
                      type="time"
                      id="appt"
                      name="horas"
                      min="7:00"
                      max="23:00"
                      value={this.state.horas}
                      className="form-control"
                      onChange={this.handleChange}
                      required
                    />
                  </div>
                  <Field
                    name="TipoVehiculo"
                    label="Tipo de Vehículo"
                    edit={false}
                    readOnly={true}
                    value={this.state.TipoVehiculo}
                    onChange={this.handleChange}
                    error={this.state.errors.TipoVehiculo}
                  />
                  <Field
                    name="FechaHasta"
                    label="Fecha Hasta"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.FechaHasta}
                    onChange={this.handleChange}
                    error={this.state.errors.FechaHasta}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="Observacion"
                    label="Observación"
                    edit={true}
                    type={"textarea"}
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
                onClick={this.OcultarFormularioEditar}
              >
                Cancelar
              </button>
            </form>
          </div>
        </Dialog>
      </BlockUi>
    );
  }

  EnviarFormulario(event) {
    event.preventDefault();
    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    }
    if (this.state.FechaHasta < this.state.FechaDesde) {
      this.props.showWarn("La Fecha Hasta debe ser mayor a la Fecha Desde");
    } else {
      axios
        .post("/Transporte/RutaHorarioVehiculo/Create", {
          Id: 0,
          RutaHorarioId: this.state.Horarioid,
          VehiculoId: this.state.VehiculoId,
          FechaDesde: this.state.FechaDesde,
          FechaHasta: this.state.FechaHasta,
          HoraLlegada: this.state.horas,
          Observacion: this.state.Observacion,
          horarioid: this.state.Horarioid,
          Estado: 1
        })
        .then(response => {
          if (response.data == "OK") {
            this.props.showSuccess("Ruta Asignada a Vehiculo ");
            this.setState({ visible: false });
            this.ObtenerRutasHorarios(this.state.Horarioid);
          } else if (response.data == "MISMARUTA") {
            this.props.showWarn(
              "El Vehículo ya esta asignado al mismo horario"
            );
            this.StopLoading();
          } else {
            this.props.showWarn("Ocurrió un Error");
            this.StopLoading();
          }
        })
        .catch(error => {
          console.log(error);
          this.props.showWarn("Ocurrió un Error");
          this.StopLoading();
        });
    }
  }
  EnviarFormularioEditar(event) {
    event.preventDefault();
    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    }
    if (this.state.FechaHasta < this.state.FechaDesde) {
      this.props.showWarn("La Fecha Hasta debe ser mayor a la Fecha Desde");
    } else {
      axios
        .post("/Transporte/RutaHorarioVehiculo/Edit", {
          Id: this.state.Identificador,
          RutaHorarioId: this.state.Horarioid,
          VehiculoId: this.state.VehiculoId,
          FechaDesde: this.state.FechaDesde,
          FechaHasta: this.state.FechaHasta,
          HoraLlegada: this.state.horas,
          Observacion: this.state.Observacion,
          horarioid: this.state.Horarioid,
          Estado: 1
        })
        .then(response => {
          if (response.data == "OK") {
            this.props.showSuccess("Editado Correctamente");
            this.setState({ visibleeditar: false });
            this.ObtenerRutasHorarios(this.state.Horarioid);
          } else {
            this.props.showWarn("Ocurrió un Error");
          }
        })
        .catch(error => {
          console.log(error);
          this.props.showWarn("Ocurrió un Error");
        });
    }
  }
  MostrarFormulario() {
    if (this.state.RutaId > 0 && this.state.Horarioid > 0) {
      this.setState({
        visible: true,
        VehiculoId: 0,
        TipoVehiculo: "",
        FechaDesde: "",
        FechaHasta: "",
        Observacion: ""
      });

      axios
        .post("/Transporte/RutaHorarioVehiculo/ObtenerHoraLLegada", {
          rutaid: this.state.RutaId,
          horarioid: this.state.Horarioid
        })
        .then(response => {
          this.setState({ horas: response.data });
        })
        .catch(error => {
          console.log(error);
        });
    } else {
      this.props.showWarn("Seleccione un Ruta y un Horario");
    }
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }
  handleChangeCorreo(event) {
    this.setState({ [event.target.name]: event.target.value.toLowerCase() });
  }
  handleChangeIdentificacion(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  Loading() {
    this.setState({ blocking: true });
  }

  StopLoading() {
    this.setState({ blocking: false });
  }

  OcultarFormulario() {
    this.setState({ visible: false, TipoVehiculo: "" });
    // this.VaciarCampos();
  }

  OcultarFormularioEditar() {
    this.setState({ visibleeditar: false });
  }

  MostrarFormularioEditar(row) {
    this.setState({ errors: {} });
    if (row.Id > 0) {
      this.setState({
        RutaHorarioId: row.RutaHorarioId,
        FechaDesde: row.FechaDesde,
        FechaHasta: row.FechaHasta,
        horas: row.HoraLlegada,
        Identificador: row.Id,
        VehiculoId: row.VehiculoId,
        Observacion: row.Observacion,
        TipoVehiculo: row.TipoVehiculo,

        visibleeditar: true
      });
    }
  }

  showInfo() {
    this.growl.show({
      severity: "info",
      summary: "Información",
      detail: this.state.message
    });
  }

  infoMessage(msg) {
    this.setState({ message: msg }, this.showInfo);
  }

  showAlert() {
    this.growl.show({
      severity: "error",
      summary: "Alerta",
      detail: this.state.message
    });
  }

  alertMessage(msg) {
    this.setState({ message: msg }, this.showAlert);
  }
}
const Container = Wrapper(VehiculosRutas);
ReactDOM.render(<Container />, document.getElementById("content"));
