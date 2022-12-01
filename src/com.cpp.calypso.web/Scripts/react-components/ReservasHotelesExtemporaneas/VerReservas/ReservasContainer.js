import React from "react";
import ReactDOM from "react-dom";
import Wrapper from "../../Base/BaseWrapper";
import Field from "../../Base/Field-v2";
import ReservasTable from "./ReservasTable";
import http from "../../Base/HttpService";
import config from "../../Base/Config";
import {
  FRASE_ERROR_SELECCIONA_FECHA_INICIO,
  FRASE_ERROR_SELECCIONA_FECHA_FIN,
  FRASE_ERROR_EDITAR_RESERVA,
  FRASE_RESERVA_ELIMINADA,
} from "../../Base/Strings";
import { Dialog } from "primereact-v2/dialog";
import { Card } from 'primereact-v2/card';
import { Button } from "primereact-v2/button";
import DetallesReservaTable from "./DetallesReservaTable";
import EditarReservaForm from "./EditarReservaForm";
import moment from "moment";

export default class ReservasContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      fechaInicio: "",
      fechaFin: "",
      errors: {},
      data: [],
      editarDialog: false,
      detallesDialog: false,
      detallesReserva: [],
      reserva: {},
      reservaId: 0,

      pass: "",
      viewenable: false,
      seleccionado: null,
      fechaActivacion: "",
      esinicioconsumo: false,
      justificacion: "",
      startForm: false,
      accion: "",
      urlApiBase: "/proveedor/ReservaHotel/",

      vista: "lista",

    };
  }

  componentDidMount() {
    this.props.unlockScreen();
  }

  verDetalles = (reserva) => {
    console.log('Reserva ', reserva);
    this.setState({ seleccionado: reserva, vista: "detalles" }, this.verDetallesConsumos(reserva));

  }
  cerrarDetalles = () => {
    this.setState({ seleccionado: null, vista: "lista" });
  }

  onActivar = () => {
    if (this.state.pass === "") {
      abp.notify.error("Debe ingresar el código de Seguridad", "Error");
    } else {
      console.log("onActivar ");

      var self = this;
      self.props.blockScreen();

      let url = "";
      url = `${self.state.urlApiBase}/EnableDisableApi`;

      let data = {
        pass: this.state.pass,
      };

      http
        .post(url, data)
        .then((response) => {
          let data = response.data;
          console.log("data ", data);
          if (data.result === "OK") {
            this.ocultarDialogPass();
            this.setState({ startForm: true });
          } else {
            abp.notify.error("El código de seguridad es incorrecto", "Error");
            this.setState({ pass: "" });
            //TODO:
            //Presentar errores...
            //var message = $.fn.responseAjaxErrorToString(data);
            // abp.notify.error(message, 'Error');
          }

          self.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);

          self.setState({ blocking: false });
        });
    }
  };
  EnviarConsumo = () => {

    console.log(this.state.seleccionado);

    var self = this;
    self.props.blockScreen();

    let url = "";
    url = `${self.state.urlApiBase}/IniciarFinalizarConsumo`;

    if (
      !this.state.esinicioconsumo &&
      this.state.fechaActivacion > this.state.seleccionado.fecha_hasta
    ) {
      abp.notify.error(
        "La Fecha no puede superar a la fecha de Fin de la Reserva",
        "Error"
      );
      return;
    }
    if (
      !this.state.esinicioconsumo &&
      this.state.fechaActivacion < this.state.seleccionado.fecha_desde
    ) {
      abp.notify.error(
        "La Fecha  no puede ser menor a la fecha de inicio de la Reserva",
        "Error"
      );
      return;
    }

    let data = {
      Id: this.state.seleccionado.Id,
      inicio: this.state.esinicioconsumo,
      fecha: this.state.esinicioconsumo
        ? this.state.fechaActivacion
        : this.state.fechaActivacion,
      justificacion: this.state.justificacion,
    };

    http
      .post(url, data)
      .then((response) => {
        let data = response.data;
        console.log("result", data);
        if (data === "OK") {
          abp.notify.success("Proceso guardado exitosamente", "Aviso");

          this.ocultarDialogJustificacion();
          this.consultarReservas();
        } else {
          abp.notify.error("El código de seguridad es incorrecto", "Error");
          //TODO:
          //Presentar errores...
          //var message = $.fn.responseAjaxErrorToString(data);
          // abp.notify.error(message, 'Error');
        }

        self.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);

        self.setState({ blocking: false });
      });
  };

  EditarFecha = () => {

    console.log(this.state.seleccionado);

    var self = this;
    self.props.blockScreen();

    let url = "";
    url = `${self.state.urlApiBase}/EditarFecha`;

    if (
      !this.state.esinicioconsumo &&
      this.state.fechaActivacion > this.state.seleccionado.fecha_hasta
    ) {
      abp.notify.error(
        "La Fecha no puede superar a la fecha de Fin de la Reserva",
        "Error"
      );
      return;
    }
    if (
      !this.state.esinicioconsumo &&
      this.state.fechaActivacion < this.state.seleccionado.fecha_desde
    ) {
      abp.notify.error(
        "La Fecha  no puede ser menor a la fecha de inicio de la Reserva",
        "Error"
      );
      return;
    }

    let data = {
      Id: this.state.seleccionado.Id,
      inicio: this.state.esinicioconsumo,
      fecha: this.state.esinicioconsumo
        ? this.state.fechaActivacion
        : this.state.fechaActivacion,
      justificacion: this.state.justificacion,
    };

    http
      .post(url, data)
      .then((response) => {
        let data = response.data;
        console.log("result", data);
        if (data === "OK") {
          abp.notify.success("Proceso guardado exitosamente", "Aviso");

          this.ocultarDialogJustificacion();
          this.consultarReservas();
        } else {
          abp.notify.error("Error", "Error");
          //TODO:
          //Presentar errores...
          //var message = $.fn.responseAjaxErrorToString(data);
          // abp.notify.error(message, 'Error');
        }

        self.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);


        self.setState({ blocking: false });
      });
  };


  render() {
    return (
      <div>
        {this.state.vista === "lista" &&
          <>
            <div className="row">
              <div className="col">
                <form onSubmit={this.consultarReservas}>
                  <div className="row">
                    <div className="col-5">
                      <Field
                        name="fechaInicio"
                        label="Fecha Desde"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaInicio}
                        onChange={this.handleChange}
                        error={this.state.errors.fechaInicio}
                      />
                    </div>
                    <div className="col-5">
                      <Field
                        name="fechaFin"
                        label="Fecha Hasta"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaFin}
                        onChange={this.handleChange}
                        error={this.state.errors.fechaFin}
                      />
                    </div>
                    <div className="col-2" style={{ paddingTop: "35px" }}>
                      <button type="submit" className="btn btn-outline-primary">
                        Buscar
                      </button>
                      &nbsp;
                      <hr />
                    </div>
                  </div>
                </form>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <ReservasTable
                  data={this.state.data}
                  verDetalles={this.verDetalles}
                  consultarDetalles={this.consultarDetalles}
                  seleccionarEditarReserva={this.seleccionarEditarReserva}
                  eliminarReserva={this.eliminarReserva}
                  onDownload={this.onDownload}
                  onLoad={this.onLoad}
                  onEditarFecha={this.onEditarFecha}
                />
              
                <Dialog
                  header="Editar Reserva"
                  dismissableMask={true}
                  visible={this.state.editarDialog}
                  width="600px"
                  modal={true}
                  onHide={this.ocultarDialogEditar}
                >
                  <EditarReservaForm
                    consultarReservas={this.consultarReservas}
                    reserva={this.state.reserva}
                    ocultarDialogEditar={this.ocultarDialogEditar}
                    showSuccess={this.props.showSuccess}
                    showWarn={this.props.showWarn}
                    blockScreen={this.props.blockScreen}
                    unlockScreen={this.props.unlockScreen}
                    resetData={this.resetData}
                    fechaAnterior={this.state.fechaAnterior}
                  />
                </Dialog>

                <Dialog
                  header={
                    this.state.esinicioconsumo
                      ? "Iniciar Consumo"
                      : "Finalizar Consumo"
                  }
                  dismissableMask={true}
                  visible={this.state.startForm}
                  width="600px"
                  modal={true}
                  onHide={this.ocultarDialogJustificacion}
                >
                  <form onSubmit={this.state.accion == "" ? this.EnviarConsumo : this.EditarFecha}>
                    <div className="row">
                      <div className="col">

                        {/*   <Field
                      name="fechaActivacion"
                      label="Fecha"
                      required
                      type="datetime-local"
                      edit={true}
                      readOnly={false}
                      value={this.state.fechaActivacion}
                      onChange={this.handleChange}
                      error={this.state.errors.fechaActivacion}
                    />
                    */}
                        <div className="form-group">
                          <label>Fecha</label>
                          <input
                            type="datetime-local"
                            id="no-filter"
                            name="fechaActivacion"
                            className="form-control"
                            onChange={this.handleChange}
                            required
                            //value={moment(this.state.fecha_registro).format(
                            // "YYYY-MM-DD"
                            // )}
                            value={this.state.fechaActivacion}
                          />
                        </div>


                        <Field
                          name="justificacion"
                          label={
                            this.state.esinicioconsumo
                              ? "Justificación de inicio manual"
                              : "Justificación de finalización manual"
                          }
                          required
                          edit={true}
                          readOnly={false}
                          value={this.state.justificacion}
                          onChange={this.handleChange}
                          error={this.state.errors.justificacion}
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
                      onClick={this.ocultarDialogJustificacion}
                    >
                      Cancelar
                    </button>
                  </form>
                </Dialog>
                <Dialog
                  header={
                    this.state.esinicioconsumo
                      ? "Iniciar Consumo"
                      : "Finalizar Consumo"
                  }
                  visible={this.state.viewenable}
                  style={{ width: "50vw" }}
                  modal
                  onHide={this.ocultarDialogPass}
                >
                  <div>
                    <p>
                      <strong>
                        Está seguro de{" "}
                        {this.state.esinicioconsumo ? "Iniciar" : "Finalizar"} el
                        proceso manual de consumo?
                      </strong>
                    </p>
                  </div>
                  <br />
                  <Field
                    name="pass"
                    label="Código de Seguridad"
                    required
                    edit={true}
                    value={this.state.pass}
                    onChange={this.handleChange}
                  />
                  <br />
                  <div align="right">
                    <Button
                      label="SI"
                      icon="pi pi-check"
                      onClick={this.onActivar}
                    />{" "}
                    <Button
                      label="NO"
                      icon="pi pi-times"
                      className="p-button-secondary"
                      onClick={this.ocultarDialogPass}
                    />
                  </div>
                </Dialog>
              </div>
            </div>
          </>
        }
        {
          this.state.vista === "detalles" &&
          <>
             <div className="row">
                <div className="col-10"></div>
                <div className="col-2" align="right">
                  <button
                    style={{ width: "100%" }}
                    className="btn btn-outline-primary"
                    onClick={() => this.cerrarDetalles()}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Regresar"
                  >
                    Regresar
                  </button>
                </div>
              </div>
            <Card title="" subTitle="Detalles consumos hospedaje">
           
              <div className="row">
                <div className="col">
                  <div className="card border-default">
                    <div className="card-header">Reserva</div>
                    <div className="card-body">
                      <div className="row">
                        <div className="col-6">

                          <h6>
                            <b>Proveedor: </b>{" "}
                            {this.state.seleccionado != null
                              ? this.state.seleccionado.proveedor_razon_social
                              : ""}
                          </h6>
                          <h6>
                            <b>N° Habitación: </b>{" "}
                            {this.state.seleccionado != null
                              ? this.state.seleccionado.numero_habitacion
                              : ""}
                          </h6>
                          <h6>
                            <b>Tipo: </b>{" "}
                            {this.state.seleccionado != null
                              ? this.state.seleccionado.tipo_habitacion_nombre
                              : ""}
                          </h6>
                          <h6>
                            <b>Fecha Desde: </b>{" "}
                            {this.state.seleccionado != null
                              ? moment(this.state.seleccionado.fecha_desde).format(config.formatDate)
                              : ""}
                          </h6>
                          <h6>
                            <b>Fecha Hasta: </b>{" "}
                            {this.state.seleccionado != null
                              ? moment(this.state.seleccionado.fecha_hasta).format(config.formatDate)
                              : ""}
                          </h6>
                        </div>
                        <div className="col-6">
                          <h6>
                            <b>Colaborador: </b>{" "}
                            {this.state.seleccionado != null
                              ? this.state.seleccionado.colaborador_nombres
                              : ""}
                          </h6>
                          <h6>
                            <b>Grupo: </b>{" "}
                            {this.state.seleccionado != null
                              ? this.state.seleccionado.colaborador_grupo_personal
                              : ""}
                          </h6>
                          <h6>
                            <b>Extemporáneo: </b>{" "}
                            {this.state.seleccionado != null
                              ? this.state.seleccionado.es_extemporaneo
                              : ""}
                          </h6>
                          <h6>
                            <b>Inicio Manual: </b>{" "}
                            {this.state.seleccionado != null
                              ? this.state.seleccionado.iniciado_manual
                              : ""}
                          </h6>
                          <h6>
                            <b>Finalizado Manual: </b>{" "}
                            {this.state.seleccionado != null
                              ? this.state.seleccionado.finalizado_manual
                              : ""}
                          </h6>
                        </div>
                      </div>


                    </div>
                  </div>
                </div>
              </div>
              <hr></hr>
              <DetallesReservaTable
                consultarDetalles={this.consultarDetalles}
                ActualizarLavanderiaSi={this.ActualizarLavanderiaSi}
                ActualizarLavanderiaNo={this.ActualizarLavanderiaNo}
                data={this.state.detallesReserva}
                reservaId={this.state.reservaId}
                ActualizarDetalles={this.ActualizarDetalles}
                ActualizarDetallesNoConsumido={
                  this.ActualizarDetallesNoConsumido
                }
                showSuccess={this.props.showSuccess}
                showWarn={this.props.showWarn}
              />
            </Card>

          </>
        }



        {//#D 
        }

      </div>
    );
  }

  onDownload = (DocumentoId) => {
    return (window.location = `${config.appUrl}/proyecto/Archivo/Descargar/${DocumentoId}`);
  };

  onLoad = (row, esinicio) => {
    console.log("row", row);
    this.setState({
      viewenable: true,
      esinicioconsumo: esinicio,
      seleccionado: row,
    });
    if (!esinicio) {
      this.setState({
        fechaActivacion: row.fecha_fin_consumo,
        justificacion: row.justificacion_finalizacion_manual
      })
    }
  };
  onEditarFecha = (row, esinicio) => {
    console.log("row Editar Fecha", row);
    this.setState({
      startForm: true,
      esinicioconsumo: esinicio,
      accion: esinicio ? "editarFecha" : "",
      seleccionado: row,
      fechaActivacion: esinicio ? row.fecha_inicio_consumo : '',
      justificacion: esinicio ? row.justificacion_inicio_manual : justificacion_finalizacion_manual,
    });
  };
  consultarReservas = (event) => {
    event.preventDefault();
    this.props.blockScreen();
    if (this.state.fechaInicio === "") {
      this.props.showWarn(FRASE_ERROR_SELECCIONA_FECHA_INICIO);
      this.props.unlockScreen();
    } else if (this.state.fechaFin === "") {
      this.props.showWarn(FRASE_ERROR_SELECCIONA_FECHA_FIN);
      this.props.unlockScreen();
    } else if (moment(this.state.fechaInicio) > moment(this.state.fechaFin)) {
      this.props.showWarn("Fecha de inicio debe ser menor a la fecha de fin");
      this.props.unlockScreen();
    } else {
      let url = "";
      let fechaInicio = this.state.fechaInicio;
      let fechaFin = this.state.fechaFin;
      url = `/Proveedor/ReservaHotel/ListarReservasExtemporaneas?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;
      http
        .get(url)
        .then((response) => {
          let data = response.data;
          if (data.success === true) {
            console.log("resut", data.result);
            this.setState({ data: data.result });
          } else {
            var message = $.fn.responseAjaxErrorToString(data);
            this.props.showWarn(message);
          }

          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.unlockScreen();
        });
    }
  };

  resetData = () => {
    this.setState({ data: [] });
  };

  verDetallesConsumos = (row) => {
    let reservaId = row.Id;
    console.log(reservaId);
    event.preventDefault();
    this.props.blockScreen();
    let url = "";
    url = `/Proveedor/DetalleReserva/ListarDetallesPorReserva/${reservaId}`;
    http
      .get(url)
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          this.setState({
            detallesReserva: data.result,
            //detallesDialog: true,
            reservaId: reservaId,
          });
        } else {
          var message = $.fn.responseAjaxErrorToString(data);
          this.props.showWarn(message);
        }

        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  consultarDetalles = (reservaId) => {
    console.log(reservaId);
    event.preventDefault();
    this.props.blockScreen();
    let url = "";
    url = `/Proveedor/DetalleReserva/ListarDetallesPorReserva/${reservaId}`;
    http
      .get(url)
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          this.setState({
            detallesReserva: data.result,
            detallesDialog: true,
            reservaId: reservaId,
          });
        } else {
          var message = $.fn.responseAjaxErrorToString(data);
          this.props.showWarn(message);
        }

        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

  ActualizarDetalles = (data) => {
    console.log(data);

    event.preventDefault();
    this.props.blockScreen();
    let url = "";
    url = `/Proveedor/DetalleReserva/CambiarEstadoDetallesExtemporaneoConsumido/`;
    http
      .post(url, data)
      .then((response) => {
        this.props.showSuccess("Actualizado Correctamente");
        this.consultarDetalles(this.state.reservaId);
       
        this.setState({ detallesDialog: false }, this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  ActualizarDetallesNoConsumido = (data) => {
    console.log(data);
    event.preventDefault();
    this.props.blockScreen();
    let url = "";
    url = `/Proveedor/DetalleReserva/CambiarEstadoDetallesExtemporaneoNoConsumido/`;
    http
      .post(url, data)
      .then((response) => {
        this.props.showSuccess("Actualizado Correctamente");
        this.consultarDetalles(this.state.reservaId);
        
        this.setState({ detallesDialog: false },this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  ActualizarLavanderiaSi = (data) => {
    console.log(data);

    event.preventDefault();
    this.props.blockScreen();
    let url = "";
    url = `/Proveedor/DetalleReserva/CambiarEstadoLavanderia/`;
    http
      .post(url, data)
      .then((response) => {
        this.props.showSuccess("Actualizado Correctamente");
        this.consultarDetalles(this.state.reservaId);
    
        this.setState({ detallesDialog: false },    this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  ActualizarLavanderiaNo = (data) => {
    console.log(data);
    event.preventDefault();
    this.props.blockScreen();
    let url = "";
    url = `/Proveedor/DetalleReserva/CambiarEstadoNoLavanderia/`;
    http
      .post(url, data)
      .then((response) => {
        this.props.showSuccess("Actualizado Correctamente");
        this.consultarDetalles(this.state.reservaId);

        this.setState({ detallesDialog: false },    this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };


  eliminarReserva = (reservaId) => {
    this.props.blockScreen();
    let url = "";
    url = `/Proveedor/ReservaHotel/EliminarReserva`;
    http
      .post(url, {
        id: reservaId,
      })
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          this.resetData();
          this.props.showSuccess(FRASE_RESERVA_ELIMINADA);
        } else {
          this.props.showWarn(FRASE_ERROR_EDITAR_RESERVA);
        }

        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

  seleccionarEditarReserva = (reserva) => {
    console.log(reserva);
    this.setState({ reserva, editarDialog: true });
  };

  handleChange = (event) => {
    this.setState({ [event.target.name]: event.target.value });
  };

  ocultarDialogDetalles = () => {
    this.setState({ detallesDialog: false });
  };
  ocultarDialogJustificacion = () => {
    this.setState({ startForm: false });
  };

  ocultarDialogPass = () => {
    this.setState({ viewenable: false });
  };

  mostrarDialogDetalles = () => {
    this.setState({ detallesDialog: true });
  };

  ocultarDialogEditar = () => {
    this.setState({ editarDialog: false, fechaAnterior: "" });
  };

  mostrarDialogEditar = () => {
    this.setState({ editarDialog: true });
  };
}

const Container = Wrapper(ReservasContainer);

ReactDOM.render(
  <Container />,
  document.getElementById("gestion_reserva_container")
);
