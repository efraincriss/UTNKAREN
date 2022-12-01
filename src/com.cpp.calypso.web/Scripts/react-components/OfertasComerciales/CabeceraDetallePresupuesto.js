import React from "react";
import axios from "axios";
import { Growl } from "primereact/components/growl/Growl";
import moment from "moment";
import BlockUi from "react-block-ui";
import CurrencyFormat from "react-currency-format";
import { Dialog } from "primereact/components/dialog/Dialog";
import { MultiSelect } from "primereact/components/multiselect/MultiSelect";
import { Checkbox } from "primereact-v2/checkbox";
import { ScrollPanel } from "primereact-v2/scrollpanel";
export default class CabeceraDetallePrespuesto extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      contratodescripcion: "",
      codigo: "",
      descripcion: "",
      version: "",
      estado: 0,
      fecha_registro: null,
      monto_ofertado: 0.0,
      monto_aprobado: 0.0,
      monto_pendiente_aprobacion: 0.0,
      estado_a: "Pendiente de Actualización",
      blockingc: false,

      key: 0,
      //transmital
      visibletransmital: false,
      colaboradores: null,
      dirigido_a: [],
      copia_a: [],

      users: false,
      view_enviar: false,
      asunto: ".",
      body: "",
    };
    this.BotonesDeAprobacion = this.BotonesDeAprobacion.bind(this);
    this.ActualizarDatos = this.ActualizarDatos.bind(this);
    this.Anular = this.Anular.bind(this);
    this.Cancelar = this.Cancelar.bind(this);
    this.Enviar = this.Enviar.bind(this);
    this.GenerarTransmital = this.GenerarTransmital.bind(this);
    this.RedireccionarTransmital = this.RedireccionarTransmital.bind(this);
    this.GenerarDocumento = this.GenerarDocumento.bind(this);
    this.ConsultarColaboradores = this.ConsultarColaboradores.bind(this);
    this.MostrarFormularioTransmital = this.MostrarFormularioTransmital.bind(
      this
    );
    this.handleChange = this.handleChange.bind(this);
    this.onHide = this.onHide.bind(this);
  }
  componentDidMount() {
    this.openConfirmActualizarDatos();
  }
  render() {
    return (
      <BlockUi tag="div" blocking={this.state.blockingc}>
        <div className="row">
          <Growl
            ref={(el) => {
              this.growl = el;
            }}
            position="bottomright"
            baseZIndex={1000}
          ></Growl>
          <div style={{ width: "100%" }}>
            <div className="card">
              <div className="card-body">
                <div className="row" align="right">
                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => this.ActualizarDatos()}
                  >
                    Actualizar Datos
                  </button>
                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => this.DescargarMatriz()}
                  >
                    Oferta Económica
                  </button>
                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => this.DescargarCronograma()}
                  >
                    Cronograma
                  </button>
                  <a
                    href={this.props.referencia}
                    className="btn btn-outline-primary btn-sm"
                    style={{ marginLeft: "0.3em" }}
                  >
                    {" "}
                    Generar Documento Oferta
                  </a>
                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => this.MostrarFormularioTransmital()}
                  >
                    Generar Transmital
                  </button>
                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => this.RedireccionarTransmital()}
                  >
                    Ir al Transmital
                  </button>
                  {/* 
                  <div id="content-notificacion"></div>

                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => this.MostrarFormEnviar()}
                  >
                    Enviar
                  </button>
                  */}
                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => {
                      if (
                        window.confirm(
                          `Esta seguro continuar con el proceso de envio de la oferta, esta acción procesará su servidor de correos y actualizará el estado de la Oferta a Presentado, ¿Desea continuar?`
                        )
                      )
                        this.props.EnvioManual();
                    }}
                  >
                    Envio Manual
                  </button>

                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => this.Anular()}
                  >
                    Anular
                  </button>
                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => this.Cancelar()}
                  >
                    Cancelar
                  </button>
                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => this.props.MostrarEdicion()}
                  >
                    Editar
                  </button>
                  <button
                    style={{ marginLeft: "0.3em" }}
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => this.Redireccionar()}
                  >
                    Regresar
                  </button>
                </div>
                <hr />

                <div className="row">
                  <div className="col-xs-12 col-md-6">
                    <h6>
                      <b>Contrato:</b> {this.props.contratodescripcion}
                    </h6>
                    <h6>
                      <b>Código:</b> {this.props.data.codigo}
                    </h6>
                    <h6>
                      <b>Descripción:</b> {this.props.data.descripcion}
                    </h6>
                    <h6>
                      <b>Versión:</b> {this.props.data.version}
                    </h6>
                  </div>

                  <div className="col-xs-12 col-md-6">
                    <h6 key={this.state.key}>
                      <b>Estado Oferta Comercial:</b> {this.props.nombre_estado}
                    </h6>
                    <h6>
                      <b>Fecha Oferta:</b>{" "}
                      {moment(this.props.fecha_oferta).format("DD-MM-YYYY")}
                    </h6>
                    <h6>
                      <b>Fecha Ultimo Envío:</b>{" "}
                      {this.props.data.fecha_registro}
                    </h6>
                    <h6>
                      <b>¿Es la versión final ? :</b>{" "}
                      {this.props.data && this.props.data.es_final
                        ? this.props.data.es_final === 1
                          ? "SI"
                          : "NO"
                        : ""}
                    </h6>
                    <h6>
                      <b>Carperta Compartida:</b>{" "}
                      <strong>
                        <a href={this.props.data.link_documentum}>
                          {this.props.data.link_documentum}
                        </a>
                      </strong>
                    </h6>
                  </div>
                </div>

                <div className="row">
                  <div className="col-4">
                    <div className="callout callout-info">
                      <small className="text-muted">Monto Ofertado</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={this.props.monto_ofertado}
                          displayType={"text"}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>
                  <div className="col-4">
                    <div className="callout callout-danger">
                      <small className="text-muted">Monto Aprobado</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={this.props.monto_aprobado}
                          displayType={"text"}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>
                  <div className="col-4">
                    <div className="callout callout-warning">
                      <small className="text-muted">Monto P. Aprobación</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={this.props.monto_pendiente_aprobacion}
                          displayType={"text"}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>
                </div>
                {this.props.ModelMontos != null &&
                  !this.props.ModelMontos.success && (
                    <div>
                      {" "}
                      <label>Montos Requerimientos</label>
                      <div className="row">
                        <div className="col">
                          <div className="callout callout-primary">
                            <small className="text-muted">Ingenieria</small>
                            <br />
                            <strong className="h4">
                              <CurrencyFormat
                                value={
                                  this.props.ModelMontos != null
                                    ? this.props.ModelMontos.monto_ingenieria
                                    : 0
                                }
                                displayType={"text"}
                                thousandSeparator={true}
                                prefix={"$"}
                              />
                            </strong>
                          </div>
                        </div>
                        <div className="col">
                          <div className="callout callout-success">
                            <small className="text-muted">Construcción</small>
                            <br />
                            <strong className="h4">
                              <CurrencyFormat
                                value={
                                  this.props.ModelMontos != null
                                    ? this.props.ModelMontos.monto_contruccion
                                    : 0
                                }
                                displayType={"text"}
                                thousandSeparator={true}
                                prefix={"$"}
                              />
                            </strong>
                          </div>
                        </div>
                        <div className="col">
                          <div className="callout callout-info">
                            <small className="text-muted">Suministros</small>
                            <br />
                            <strong className="h4">
                              <CurrencyFormat
                                value={
                                  this.props.ModelMontos != null
                                    ? this.props.ModelMontos.monto_suministros
                                    : 0
                                }
                                displayType={"text"}
                                thousandSeparator={true}
                                prefix={"$"}
                              />
                            </strong>
                          </div>
                        </div>
                        <div className="col">
                          <div className="callout callout-warning">
                            <small className="text-muted">Subcontratos</small>
                            <br />
                            <strong className="h4">
                              <CurrencyFormat
                                value={
                                  this.props.ModelMontos != null
                                    ? this.props.ModelMontos.monto_subcontratos
                                    : 0
                                }
                                displayType={"text"}
                                thousandSeparator={true}
                                prefix={"$"}
                              />
                            </strong>
                          </div>
                        </div>

                        <div className="col">
                          <div className="callout callout-danger">
                            <small className="text-muted">Total</small>
                            <br />
                            <strong className="h4">
                              <CurrencyFormat
                                value={
                                  this.props.ModelMontos != null
                                    ? this.props.ModelMontos.monto_total
                                    : 0
                                }
                                displayType={"text"}
                                thousandSeparator={true}
                                prefix={"$"}
                              />
                            </strong>
                          </div>
                        </div>
                      </div>
                    </div>
                  )}
              </div>
            </div>
          </div>
        </div>

        <Dialog
          header="Seleccione Usuarios Transmital"
          modal={true}
          visible={this.state.visibletransmital}
          style={{ width: "450px", overflow: "auto" }}
          onHide={this.onHide}
          maximizable={true}
        >
          <div>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label>Drigido a</label>
                  <MultiSelect
                    value={this.state.dirigido_a}
                    options={this.state.colaboradores}
                    onChange={(e) => this.setState({ dirigido_a: e.value })}
                    style={{ width: "100%" }}
                    filter={true}
                    defaultLabel="Selecciona  Usuarios"
                    className="form-control"
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Con Copia a</label>
                  <MultiSelect
                    value={this.state.copia_a}
                    options={this.state.colaboradores}
                    onChange={(e) => this.setState({ copia_a: e.value })}
                    style={{ width: "100%" }}
                    filter={true}
                    defaultLabel="Selecciona  Usuarios"
                    className="form-control"
                  />
                </div>
              </div>
            </div>
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-folder-open"
              style={{ marginRight: "0.3em" }}
              onClick={this.GenerarTransmital}
            >
              {" "}
              Guardar
            </button>
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.onHide}
            >
              Cancelar
            </button>
          </div>
        </Dialog>

        <Dialog
          header="Enviar Adjuntos Oferta Comercial"
          visible={this.state.view_enviar}
          onHide={this.OcultarFormEnviar}
          modal={true}
          style={{ width: "70vw" }}
        >
          <ScrollPanel
            style={{ width: "68vw", height: "500px" }}
            className="custombar1"
          >
            <div style={{ padding: "1em", lineHeight: "1.5" }}>
              <form onSubmit={this.Enviar}>
                <div className="row">
                  <div className="col">
                    <label>
                      <b>NOMBRE LISTA DISTRIBUCIÓN: </b> OFERTA COMERCIAL
                    </label>
                    <hr />
                    <div className="form-group">
                      <label>
                        {" "}
                        <b>Asunto :</b>
                      </label>
                      <textarea
                        type="text"
                        name="asunto"
                        className="form-control"
                        onChange={this.handleChange}
                        value={this.state.asunto}
                      />
                    </div>
                    <hr />
                    <hr />
                    <div className="form-group">
                      <label>
                        {" "}
                        <b>Descripción: </b>
                      </label>
                      <textarea
                        rows={10}
                        type="text"
                        name="body"
                        className="form-control"
                        onChange={this.handleChange}
                        value={this.state.body}
                      />
                    </div>
                    <hr />
                    <Checkbox
                      checked={this.state.users}
                      onChange={(e) => this.setState({ users: e.checked })}
                    />{" "}
                    <label>Enviar a correos adjuntos en transmittal</label>
                    <hr />
                    <label>
                      <b>Correos Lista Distribución: </b>
                      {this.props.data != null
                        ? this.props.data.orden_proceder_enviada_por
                        : ""}
                    </label>
                  </div>
                </div>
                <hr />
                <button type="submit" className="btn btn-outline-primary">
                  Enviar
                </button>
                &nbsp;
                <button
                  type="button"
                  className="btn btn-outline-primary"
                  icon="fa fa-fw fa-ban"
                  onClick={this.OcultarFormEnviar}
                >
                  Cancelar
                </button>
              </form>
            </div>
          </ScrollPanel>
        </Dialog>
      </BlockUi>
    );
  }

  MostrarFormEnviar = () => {
    this.setState({
      view_enviar: true,
      users: false,

      body:
        "Estimados" +
        "\n" +
        "Adjunto a la presente la Propuesta de Trabajos " +
        this.props.data.descripcion +
        "/" +
        this.props.data.version +
        " y sus anexos:\n" +
        "* Anexo- Propuesta Económica\n* Anexo- Solicitud de Trabajos Adicionales.\nAdicionalmente adjunto el transmittal de envió de Documentos[" +
        this.props.codigoTransmital +
        "].\nQuedamos a vuestra disposición ante cualquier particularidad.",
    });
  };

  OcultarFormEnviar = () => {
    this.setState({ view_enviar: false });
  };
  Redireccionar() {
    window.location.href = "/Proyecto/OfertaComercial/";
  }

  onHide(event) {
    this.setState({ visibletransmital: false });
  }
  MostrarFormularioTransmital() {
    this.props.ObtenerTieneTransmital();
    console.log(this.props.tieneTransmital);
    if (this.props.tieneTransmital != null && !this.props.tieneTransmital) {
      console.log("False");
      this.setState({ visibletransmital: true });
      this.ConsultarColaboradores();
    } else {
      abp.notify.warn(
        "La Oferta Comercial ya tiene un transmital generado",
        "AVISO"
      );
    }
  }

  BotonesDeAprobacion() {
    if (this.props.Oferta) {
      if (
        this.props.Oferta.estado_aprobacion == 2 ||
        this.props.Oferta.estado_aprobacion === 0
      ) {
        return (
          <button
            style={{ marginLeft: "0.3em" }}
            className="btn btn-outline-primary"
            onClick={() => this.props.AprobarPresupuesto()}
          >
            Aprobar
          </button>
        );
      } else if (this.props.Oferta.estado_aprobacion == 1) {
        return (
          <button
            style={{ marginLeft: "0.3em" }}
            className="btn btn-outline-primary"
            onClick={() => this.props.DesaprobarPresupuesto()}
          >
            Desaprobar
          </button>
        );
      }
    }
  }

  Anular() {
    this.props.Loading();
    axios
      .post("/proyecto/OfertaComercial/GetAnular", {
        Id: this.props.OfertaIdActual,
      })
      .then((response) => {
        if ((response.data = "o")) {
          this.growl.show({
            life: 5000,
            severity: "success",
            summary: "",
            detail: "Oferta Comercial Anulado",
          });
          this.setState({ visiblepresupuesto: false, key: Math.random() });

          this.props.ConsultarPresupuesto();
          this.props.detalles_oferta();
          this.props.CalcularMonto();
          this.props.CancelarLoading();
        } else {
          this.props.CancelarLoading();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.CancelarLoading();
      });
  }

  Enviar() {
    this.props.Loading();
    axios
      .post("/proyecto/OfertaComercial/GetEnviar", {
        Id: this.props.OfertaIdActual,
        user_transmittal: this.state.users,
        body: this.state.body,
        asunto: this.state.asunto,
      })
      .then((response) => {
        if (response.data === "SIN_TRANSMITAL") {
          abp.notify.error(
            "No ha generado un transmittal para la oferta",
            "AVISO"
          );
          this.props.CancelarLoading();
        }
        if (response.data == "SIN_ARCHIVOS") {
          abp.notify.error(
            "No existen archivos adjuntos en el transmittal",
            "AVISO"
          );
          this.props.CancelarLoading();
        }

        if (response.data == "SIN_CORREOS") {
          abp.notify.error(
            "No existen correos en la Lista de Distribución",
            "AVISO"
          );
          this.props.CancelarLoading();
        }
        if (response.data == "OK") {
          abp.notify.success(
            "Adjuntos Enviados Corectamente- Oferta Comercial Emitida",
            "Éxito"
          );
          this.setState({
            visiblepresupuesto: false,
            view_enviar: false,
            key: Math.random(),
          });
          this.props.ConsultarPresupuesto();
          this.props.detalles_oferta();
          this.props.CalcularMonto();
          this.props.CancelarLoading();
        }
        if (response.data == "ERROR") {
          this.props.CancelarLoading();
        }
      })
      .catch((error) => {
        console.log(error);

        this.props.CancelarLoading();
      });
  }
  Cancelar() {
    this.props.Loading();
    axios
      .post("/proyecto/OfertaComercial/GetCancelar", {
        Id: this.props.OfertaIdActual,
      })
      .then((response) => {
        if ((response.data = "o")) {
          this.growl.show({
            life: 5000,
            severity: "success",
            summary: "",
            detail: "Cancelado",
          });
          this.setState({
            visiblepresupuesto: false,
            blockingc: false,
            key: Math.random(),
          });
          this.props.detalles_oferta();
          this.props.ConsultarPresupuesto();
          this.props.CalcularMonto();
          this.props.CancelarLoading();
        } else {
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.CancelarLoading();
      });
  }
  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  EnvioContractual = () => {
    console.log("Contractual");
    this.props.Loading();
    axios
      .post("/Proyecto/OfertaComercial/GetEnviarContrato", {})
      .then((response) => {
        abp.notify.warn(response.data);
      })
      .catch((error) => {
        console.log(error);
      });
  };
  openConfirmActualizarDatos = () => {
    let _this = this;
    abp.message.confirm(
      "Esta acción actualizará los datos de la cabecera. Si ya realizó esta acción presione Cancelar .¿Desea Continuar?",
      "¿Actualizar Datos?",
      function (isConfirmed) {
        if (isConfirmed) {
          _this.ActualizarDatos();
        }
      }
    );
  };
  ActualizarDatos() {
    this.props.Loading();
    axios
      .post("/proyecto/OfertaComercial/ActualizarDatos", {
        OfertaComercialId: this.props.OfertaIdActual,
        PresupuestoId: this.props.PresupuestoId,
        fecha_asignacion: new Date(),
        vigente: true,
      })
      .then((response) => {
        if (response.data == "o") {
          this.growl.show({
            life: 5000,
            severity: "success",
            summary: "",
            detail: "Datos Actualizados",
          });
          this.setState({
            visiblepresupuesto: false,
            estado_a: "Actualizado",
            blockingc: false,
          });
          this.props.ConsultarPresupuesto();
          this.props.detalles_oferta();
          this.props.CancelarLoading();
          this.props.CalcularMonto();
        } else if (response.data == "ne") {
          this.props.detalles_oferta();
          this.props.CalcularMonto();
          this.growl.show({
            life: 5000,
            severity: "warn",
            summary: "",
            detail: "Los Requerimientos Ligados No Estan En Estado Enviado",
          });
          this.props.CancelarLoading();
        } else {
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.CancelarLoading();
      });
  }
  DescargarMatriz() {
    this.props.Loading();
    axios
      .get(
        "/proyecto/OfertaComercial/GenerarPrespuesto?OfertaId=" +
          this.props.data.Id,
        { responseType: "arraybuffer" }
      )
      .then((response) => {
        console.log(response.headers["content-disposition"]);
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
        this.props.CancelarLoading();
        this.props.successMessage("Matriz Descargada");
      })
      .catch((error) => {
        console.log(error);
        this.props.CancelarLoading();
        this.props.warnMessage("Error al descargar la matriz");
      });
  }

  DescargarCronograma() {
    this.props.Loading();
    axios
      .get(
        "/proyecto/OfertaComercial/ExportarCronograma/" + this.props.data.Id,
        { responseType: "arraybuffer" }
      )
      .then((response) => {
        const url = window.URL.createObjectURL(
          new Blob([response.data], {
            type:
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          })
        );
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", "Cronograma.xlsx");
        document.body.appendChild(link);
        link.click();
        this.props.CancelarLoading();
        this.props.successMessage("Matriz Descargada");
      })
      .catch((error) => {
        console.log(error);
        this.props.CancelarLoading();
        this.props.warnMessage("Error al descargar la matriz");
      });
  }

  RedireccionarTransmital(id) {
    axios
      .post("/Proyecto/OfertaComercial/GetTransmital/", {
        Id: this.props.OfertaIdActual,
      })
      .then((response) => {
        console.log(response.data);
        if (response.data == "0") {
          this.growl.show({
            life: 5000,
            severity: "warn",
            summary: "",
            detail: "Genere un Transmital",
          });
        } else {
          window.location.href =
            "/Proyecto/TransmitalCabecera/Details/" +
            response.data.Id +
            "?id2=" +
            response.data.OfertaComercialId;
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }

  ConsultarColaboradores() {
    this.setState({ blocking: true });
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerTransmitalUsers", {})
      .then((response) => {
        console.log(response.data);
        var colaborador = response.data.map((item) => {
          return {
            label:
              item.apellidos +
              " " +
              item.nombres +
              " ( " +
              item.nombre_cliente +
              " )",
            dataKey: item.Id,
            value: item.Id,
          };
        });
        this.setState({ colaboradores: colaborador });
      })
      .catch((error) => {
        this.setState({ blocking: false });
        this.props.warnMessage("Ocurrió un error al  Colaboradores");
        console.log(error);
      });
  }

  GenerarTransmital() {
    this.props.Loading();
    axios
      .post("/Proyecto/TransmitalCabecera/CrearTransmitalOfertaComercial/", {
        Id: this.props.OfertaIdActual,
        ContratoId: 0,
        EmpresaId: 0,
        ClienteId: 0,
        OfertaComercialId: 0,
        fecha_emision: new Date(),
        tipo: "CO",
        tipo_formato: "I",
        tipo_proposito: "PA",
        descripcion: "",
        vigente: true,
        dirigido_a:
          this.state.dirigido_a != null
            ? this.state.dirigido_a.toString()
            : "0",
        copia_a:
          this.state.copia_a != null ? this.state.copia_a.toString() : "0",
        version: "",
        codigo_transmital: "000",
        codigo_carta: "",
        enviado_por: "Yo",
        vigente: true,
      })
      .then((response) => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.CancelarLoading();
          this.props.ObtenerTieneTransmital();
          this.growl.show({
            life: 5000,
            severity: "success",
            summary: "",
            detail: "Tranmisttal Generado",
          });
          this.onHide();
        } else if (response.data == "EXISTE") {
          this.props.CancelarLoading();
          this.growl.show({
            life: 5000,
            severity: "warn",
            summary: "",
            detail: "Ya se generó un Transmittal para la Oferta Comercial",
          });
          this.onHide();
        }
      })
      .catch((error) => {
        this.props.CancelarLoading();
        this.onHide();
        console.log(error);
      });
  }

  GenerarDocumento() {
    this.props.Loading();
    axios
      .post("/Proyecto/OfertaComercial/ListarWordOfertaComercial/", {
        id: this.props.OfertaIdActual,
      })
      .then((response) => {
        console.log(response.data);

        this.props.CancelarLoading();

        const url = window.URL.createObjectURL(
          new Blob([response.data], {
            type:
              "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
          })
        );
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", "OfertaComercial.docx");
        document.body.appendChild(link);
        link.click();

        this.growl.show({
          life: 5000,
          severity: "success",
          summary: "",
          detail: "Documento Generado",
        });
      })
      .catch((error) => {
        this.props.CancelarLoading();
        console.log(error);
      });
  }
}
