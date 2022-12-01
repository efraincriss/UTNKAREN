import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import Field from "../../Base/Field-v2";
import wrapForm from "../../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";

class RutaContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: true,
      data: [],
      visible: false,
      visibleeditar: false,

      errors: {},

      // PARADAS
      visibleparaeditar: false,
      visiblehorarioeditar: false,

      // Inputs del Formulario
      Identificadorparada: 0,
      IdentificadorHorario: 0,
      lugares: [],
      Codigo: "",
      Nombre: "",
      OrigenId: 0,
      DestinoId: 0,

      Duracion: 0,
      Sector: "",
      Descripcion: "",
      catalogoparadas: [],
      paradas: [],
      horarios: [],

      //RUTA PARADA
      ParadaId: 0,
      Distancia: 0.0,
      DistanciaParada: 0.0,
      ordinal: null,

      //rUTA Horario
      horas: null,
      danger: ""
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleChangeCorreo = this.handleChangeCorreo.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.handleChangeHora = this.handleChangeHora.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);
    this.OcultarFormularioEditar = this.OcultarFormularioEditar.bind(this);
    this.MostrarFormularioHorario = this.MostrarFormularioHorario.bind(this);
    this.alertMessage = this.alertMessage.bind(this);
    this.infoMessage = this.infoMessage.bind(this);
    this.Loading = this.Loading.bind(this);
    this.StopLoading = this.StopLoading.bind(this);

    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.EnviarFormularioEditarParada = this.EnviarFormularioEditarParada.bind(
      this
    );
    this.EnviarFormularioHorario = this.EnviarFormularioHorario.bind(this);
    this.EnviarFormularioHorarioEditar = this.EnviarFormularioHorarioEditar.bind(
      this
    );
    this.MostrarFormulario = this.MostrarFormulario.bind(this);
    this.MostrarFormularioParadaEditar = this.MostrarFormularioParadaEditar.bind(
      this
    );
    this.MostrarFormularioHorarioEditar = this.MostrarFormularioHorarioEditar.bind(
      this
    );
    this.OcultarFormularioHorarioEditar = this.OcultarFormularioHorarioEditar.bind(
      this
    );
    this.OcultarFormularioParadaEditar = this.OcultarFormularioParadaEditar.bind(
      this
    );

    //METODOS
    this.ObtenerRutas = this.ObtenerRutas.bind(this);
    this.ObtenerDetalleRuta = this.ObtenerDetalleRuta.bind(this);
    this.ObtenerHorariosRuta = this.ObtenerHorariosRuta.bind(this);
    this.ObtenerParadasRuta = this.ObtenerParadasRuta.bind(this);
    this.ObtenerListaParadas = this.ObtenerListaParadas.bind(this);
    this.generarBotones = this.generarBotones.bind(this);
    this.generarBotonesHorarios = this.generarBotonesHorarios.bind(this);
    this.VaciarCampos = this.VaciarCampos.bind(this);
    this.EliminarRutaParada = this.EliminarRutaParada.bind(this);
    this.EliminarRutaHorario = this.EliminarRutaHorario.bind(this);
    this.RedireccionarDetalle = this.RedireccionarDetalle.bind(this);
    this.isValid = this.isValid.bind(this);
    this.RecargarParadas = this.RecargarParadas.bind(this);
  }

  componentDidMount() {
    this.props.blockScreen();
    this.ObtenerDetalleRuta();
    this.ObtenerParadasRuta();
    this.ObtenerHorariosRuta();
    this.ObtenerListaParadas();
  }
  isValid() {
    const errors = {};

    if (this.state.ParadaId == 0) {
      errors.ParadaId = "Campo requerido";
    }
    if (
      this.state.ordinal == null ||
      (this.state.ordinal != null && this.state.ordinal <= 0)
    ) {
      errors.ordinal = "Campo requerido";
    }
    /*if (this.state.DistanciaParada == 0) {
      errors.DistanciaParada = 'Campo requerido';
    }
*/
    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }

  VaciarCampos() {
    this.setState({
      Identificador: 0,
      ParadaId: 0,
      DistanciaParada: 0.0,
      ordinal: null

      //rUTA Horario
    });
  }
  ObtenerRutas() {
    this.Loading();
    axios
      .post("/Transporte/Ruta/ListaRutas", {})
      .then(response => {
        console.log(response.data.result);

        this.setState({ data: response.data.result, blocking: false });
        this.StopLoading();
      })
      .catch(error => {
        console.log(error);
        this.StopLoading();
      });
  }

  ObtenerDetalleRuta() {
    console.log("-Detalle Ruta");
    axios
      .post("/Transporte/ruta/ObtenerDetallesRuta", {
        Id: document.getElementById("RutaId").className
      })
      .then(response => {
        if (response.data != "Error") {
          console.log(response.data);
          this.setState({
            Identificador: document.getElementById("RutaId").className,
            Codigo: response.data.Codigo,
            Nombre: response.data.Nombre,
            OrigenId: response.data.OrigenId,
            DestinoId: response.data.DestinoId,
            Distancia: response.data.Distancia,
            Duracion: response.data.Duracion,
            Sector: response.data.Sector != null && response.data.Sector.nombre,
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

  ObtenerParadasRuta() {
    console.log("Paradas por Ruta");
    this.Loading();

    axios
      .post("/Transporte/ruta/ObtenerParadasRuta", {
        id: document.getElementById("RutaId").className
      })
      .then(response => {
        this.setState({ paradas: response.data, blocking: false });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
      });
  }

  ObtenerHorariosRuta() {
    console.log("Horarios por Ruta");
    this.Loading();

    axios
      .post("/Transporte/ruta/ObtenerHorariosRuta/", {
        id: document.getElementById("RutaId").className
      })
      .then(response => {
        console.log(response.data);
        this.setState({ horarios: response.data, blocking: false });
        this.StopLoading();
      })
      .catch(error => {
        console.log(error);
        this.StopLoading();
      });

    this.StopLoading();
  }
  ObtenerListaParadas() {
    console.log("Paradas");
    this.Loading();

    axios
      .post("/Transporte/ruta/ObtenerParadas", {})
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.Nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ catalogoparadas: items, blocking: false });
        this.StopLoading();
      })
      .catch(error => {
        console.log(error);
        this.StopLoading();
      });

    this.StopLoading();
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };

  RedireccionarDetalle() {
    window.location.href = "/Transporte/Ruta/Index/";
  }
  EliminarRutaParada(Id) {
    console.log(Id);
    axios
      .post("/Transporte/RutaParada/Delete", {
        Id: Id
      })
      .then(response => {
        if (response.data != "Error") {
          this.props.showSuccess("Asignacion Eliminada Correctamente");
          this.ObtenerParadasRuta();
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurrió un Error");
      });
  }
  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }
  EliminarRutaHorario(Id) {
    console.log(Id);
    axios
      .post("/Transporte/RutaHorario/Delete", {
        Id: Id
      })
      .then(response => {
    
          if (response.data != "Error") {
            this.props.showSuccess(
              "Asignacion Horaria Eliminada Correctamente"
            );
            this.ObtenerHorariosRuta();
          }
        
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurrió un Error");
      });
  }

  generarBotones = (cell, row) => {
    return (
      <div>
        {/*  <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormularioParadaEditar(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar Parada"
        >
          <i className="fa fa-edit" />
        </button>
  */}
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.EliminarRutaParada(row.Id);
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar  Parada"
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };
  generarBotonesHorarios = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormularioHorarioEditar(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar Horario"
        >
          <i className="fa fa-edit" />
        </button>

        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar Parada"
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.EliminarRutaHorario(row.Id);
          }}
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };

  RecargarParadas = data => {
    this.setState({ paradas: data });
  };

  onAfterSaveCell(row, cellName, cellValue) {
    console.log(row);
    this.Loading();
    axios
      .post("/Transporte/RutaParada/Edit", {
        Id: row.Id,
        RutaId: row.RutaId,
        ParadaId: row.ParadaId,
        ordinal: row.ordinal,
        Distancia: row.Distancia
      })
      .then(response => {
        if (response.data == "OK") {
          axios
            .post("/Transporte/ruta/ObtenerParadasRuta", {
              id: document.getElementById("RutaId").className
            })
            .then(response => {
              this.RecargarParadas(response.data);
              this.StopLoading();
            })
            .catch(error => {
              console.log(error);
            });
        } else {
        }
      })
      .catch(error => {
        console.log(error);
      });
  }

  render() {
    const cellEditProp = {
      mode: "click",
      blurToSave: true,
      afterSaveCell: this.onAfterSaveCell.bind(this) // a hook for after saving cell
    };

    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={el => {
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
                      <h5>Ruta:</h5>
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
                      {/*   <div className="col-2">
                        <i className="fa fa-road fa-5x" />
                        </div>
                        */}
                      <div className="col-6">
                        <h6>
                          <b>Código:</b> {this.state.Codigo}
                        </h6>
                        <h6>
                          <b>Lugar Origen:</b> {this.state.NombreOrigen}
                        </h6>
                        <h6>
                          <b>Distancia:</b> {this.state.Distancia}
                          {" km"}
                        </h6>
                        <h6>
                          <b>Duración:</b> {this.state.Duracion}
                          {" minutos"}
                        </h6>
                      </div>

                      <div className="col-6">
                        <h6>
                          <b>Nombre:</b> {this.state.Nombre}
                        </h6>
                        <h6>
                          <b>Lugar Destino:</b> {this.state.NombreDestino}
                        </h6>
                        <h6>
                          <b>Descripción:</b> {this.state.Descripcion}
                        </h6>
                        <h6>
                          <b>Sector:</b> {this.state.Sector}
                        </h6>
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
                  Asignar Paradas {/* <i className="fa fa-bus fa-1x" />*/}
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
                  Asignar Horarios {/*<i className="fa fa-history fa-1x" />*/}
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
                    onClick={() => this.MostrarFormulario()}
                  >
                    Nueva Parada
                  </button>
                </div>
                <br />
                <BootstrapTable
                  data={this.state.paradas}
                  hover={true}
                  pagination={true}
                  cellEdit={cellEditProp}
                >
                  <TableHeaderColumn
                    dataField="any"
                    dataFormat={this.Secuencial}
                    editable={false}
                    width={"8%"}
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  >
                    Nº
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="NombreParada"
                    editable={false}
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Nombre
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="ordinal"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Orden
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="Latitud"
                    editable={false}
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Latitud
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="Longitud"
                    editable={false}
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Longitud
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Referencia"
                    editable={false}
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Referencia
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

              <div
                className="tab-pane fade"
                id="op"
                role="tabpanel"
                aria-labelledby="op-tab"
              >
                <div className="col" align="right">
                  <button
                    className="btn btn-outline-primary"
                    onClick={() => this.MostrarFormularioHorario()}
                  >
                    Nuevo Horario
                  </button>
                </div>
                <br />
                <BootstrapTable
                  data={this.state.horarios}
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
                    dataField="Horario"
                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Horario
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Id"
                    isKey
                    width={"15%"}
                    dataFormat={this.generarBotonesHorarios}
                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  >
                    Opciones
                  </TableHeaderColumn>
                </BootstrapTable>
              </div>
            </div>
          </div>
        </div>

        <Dialog
          header="Nueva Parada"
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
                    name="ParadaId"
                    required
                    value={this.state.ParadaId}
                    label="Parada"
                    options={this.state.catalogoparadas}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.ParadaId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  {/*<Field
                    name="DistanciaParada"
                    label="Distancia"
                    type={"number"}
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.DistanciaParada}
                    onChange={this.handleChange}
                    error={this.state.errors.DistanciaParada}
                  />
                  */}
                </div>

                <div className="col">
                  <Field
                    name="ordinal"
                    label="Ordinal"
                    type={"number"}
                    step="1"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.ordinal}
                    onChange={this.handleChange}
                    error={this.state.errors.ordinal}
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
          header="Editar Parada"
          visible={this.state.visibleparaeditar}
          width="730px"
          modal={true}
          onHide={this.OcultarFormularioParadaEditar}
        >
          <div>
            <form onSubmit={this.EnviarFormularioEditarParada} height="730px">
              <div className="row">
                <div className="col">
                  <Field
                    name="ParadaId"
                    required
                    value={this.state.ParadaId}
                    label="Parada"
                    options={this.state.catalogoparadas}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.ParadaId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  {/*<Field
                    name="DistanciaParada"
                    label="Distancia"
                    type={"number"}
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.DistanciaParada}
                    onChange={this.handleChange}
                    error={this.state.errors.DistanciaParada}
                  />*/}
                </div>

                <div className="col">
                  <Field
                    name="ordinal"
                    label="Ordinal"
                    type={"number"}
                    step="1"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.ordinal}
                    onChange={this.handleChange}
                    error={this.state.errors.ordinal}
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
                onClick={this.OcultarFormularioParadaEditar}
              >
                Cancelar
              </button>
            </form>
          </div>
        </Dialog>

        <Dialog
          header="Nuevo Horario"
          visible={this.state.visibleeditar}
          width="500px"
          modal={true}
          onHide={this.OcultarFormularioEditar}
        >
          <form onSubmit={this.EnviarFormularioHorario} height="800px">
            <div className="form-group">
              <label className="col-sm-12 col-form-label">* Horario</label>
              <input
                type="time"
                id="appt"
                name="horas"
                min="7:00"
                max="23:00"
                value={this.state.horas}
                className="form-control"
                onChange={this.handleChangeHora}
              />
              <div
                className={this.state.danger}
                style={{
                  display: "flex",
                  justifyContent: "center",
                  alignItems: "center",
                  height: "17px",
                  fontSize: "12px"
                }}
              >
                {this.state.errors.Horario}
              </div>
            </div>
            <hr />
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
        </Dialog>
        <Dialog
          header="Editar Horario"
          visible={this.state.visiblehorarioeditar}
          width="500px"
          modal={true}
          onHide={this.OcultarFormularioHorarioEditar}
        >
          <form onSubmit={this.EnviarFormularioHorarioEditar} height="800px">
            <div className="form-group">
              <label className="col-sm-12 col-form-label">* Horario</label>
              <input
                type="time"
                id="appt"
                name="horas"
                min="7:00"
                max="23:00"
                value={this.state.horas}
                className="form-control"
                onChange={this.handleChangeHora}
              />
              <div
                className={this.state.danger}
                style={{
                  display: "flex",
                  justifyContent: "center",
                  alignItems: "center",
                  height: "17px",
                  fontSize: "12px"
                }}
              >
                {this.state.errors.Horario}
              </div>
            </div>
            <hr />
            <button type="submit" className="btn btn-outline-primary">
              Guardar
            </button>
            &nbsp;
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.OcultarFormularioHorarioEditar}
            >
              Cancelar
            </button>
          </form>
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
    } else {
      axios
        .post("/Transporte/RutaParada/Create", {
          Id: 0,
          RutaId: document.getElementById("RutaId").className,
          ParadaId: this.state.ParadaId,
          ordinal: this.state.ordinal,
          Distancia: this.state.DistanciaParada
        })
        .then(response => {
          if (response.data == "OK") {
            this.props.showSuccess("Parada asignada a ruta correctamente");
            this.setState({ visible: false });
            this.ObtenerParadasRuta();
            this.StopLoading();
            this.VaciarCampos();
          } else {
            this.props.showWarn("Ocurrió un Error");
            this.StopLoading();
          }
        })
        .catch(error => {
          Editar;
          console.log(error);
          this.props.showWarn("Ocurrió un Error");
          this.StopLoading();
        });
    }
  }

  EnviarFormularioEditarParada(event) {
    event.preventDefault();
    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      axios
        .post("/Transporte/RutaParada/Edit", {
          Id: this.state.Identificadorparada,
          RutaId: document.getElementById("RutaId").className,
          ParadaId: this.state.ParadaId,
          ordinal: this.state.ordinal,
          Distancia: this.state.DistanciaParada
        })
        .then(response => {
          if (response.data == "OK") {
            this.props.showSuccess("Parada asignada a ruta correctamente");
            this.setState({ visibleparaeditar: false });
            this.ObtenerParadasRuta();
            this.StopLoading();
            this.VaciarCampos();
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
  EnviarFormularioHorario(event) {
    event.preventDefault();

    if (this.state.horas == null) {
      const errors = {};
      this.setState({ danger: "alert alert-danger" });
      errors.Horario = "Campo requerido";
      this.setState({ errors });
    } else {
      this.setState({ errors: {}, danger: "" });
      axios
        .post("/Transporte/RutaHorario/Create", {
          Id: 0,
          RutaId: document.getElementById("RutaId").className,
          Horario: this.state.horas
        })
        .then(response => {
          if (response.data == "OK") {
            this.props.showSuccess("Horario Ingresado Correctamente");
            this.setState({ visibleeditar: false });
            this.ObtenerHorariosRuta();
          } else if (response.data == "MISMOHORARIO") {
            this.props.showWarn("El Horario ya esta registrado en la ruta");
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
  EnviarFormularioHorarioEditar(event) {
    event.preventDefault();

    if (this.state.horas == null) {
      const errors = {};
      this.setState({ danger: "alert alert-danger" });
      errors.Horario = "Campo requerido";
      this.setState({ errors });
    } else {
      this.setState({ errors: {}, danger: "" });
      axios
        .post("/Transporte/RutaHorario/Edit", {
          Id: this.state.IdentificadorHorario,
          RutaId: document.getElementById("RutaId").className,
          Horario: this.state.horas
        })
        .then(response => {
          if (response.data == "OK") {
            this.props.showSuccess("Horario Ingresado Correctamente");
            this.setState({ visiblehorarioeditar: false });
            this.ObtenerHorariosRuta();
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

  MostrarFormularioParadaEditar(row) {
    this.setState({ errors: {} });
    console.log(row);
    this.setState({
      visibleparaeditar: true,
      Identificadorparada: row.Id,
      ParadaId: row.ParadaId,
      ordinal: row.ordinal,
      DistanciaParada: row.Distancia
    });
  }
  MostrarFormularioHorario() {
    this.setState({ visibleeditar: true, errors: {}, danger: "" });
  }
  MostrarFormularioHorarioEditar(row) {
    this.setState({ errors: {} });
    console.log(row);
    this.setState({
      visiblehorarioeditar: true,
      IdentificadorHorario: row.Id,
      horas: row.Horario
    });
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }
  handleChangeCorreo(event) {
    this.setState({ [event.target.name]: event.target.value.toLowerCase() });
  }
  handleChangeHora(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  Loading() {
    this.setState({ blocking: true });
  }

  StopLoading() {
    this.setState({ blocking: false });
  }

  OcultarFormulario() {
    this.setState({ visible: false });
    this.VaciarCampos();
  }
  OcultarFormularioParadaEditar() {
    this.setState({ visibleparaeditar: false });
    this.VaciarCampos();
  }

  MostrarFormulario() {
    this.setState({ visible: true });
  }
  OcultarFormularioEditar() {
    this.setState({ visibleeditar: false });
    this.VaciarCampos();
  }
  OcultarFormularioHorarioEditar() {
    this.setState({ visiblehorarioeditar: false });
    this.VaciarCampos();
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
const Container = wrapForm(RutaContainer);
ReactDOM.render(<Container />, document.getElementById("content"));
