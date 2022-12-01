import React from "react";
import ReactDOM from "react-dom";
import Wrapper from "../../Base/BaseWrapper";
import Field from "../../Base/Field-v2";
import ReservasTable from "./ReservasTable";
import http from "../../Base/HttpService";
import {
  FRASE_ERROR_SELECCIONA_FECHA_INICIO,
  FRASE_ERROR_SELECCIONA_FECHA_FIN,
  FRASE_ERROR_EDITAR_RESERVA,
  FRASE_RESERVA_ELIMINADA,
} from "../../Base/Strings";
import { Dialog } from "primereact-v2/dialog";
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

      pass: "",
      viewenable: false,
      seleccionado: null,
      fechaActivacion: "",
      justificacion: "",
      startForm: false,
    };
  }

  componentDidMount() {
    this.props.unlockScreen();
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

          if (data.result === true) {
            abp.notify.success("Proceso guardado exitosamente", "Aviso");
            this.onHideview();
            var newParams = {};

            self.onRefreshData(newParams);
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
    }
  };

  render() {
    return (
      <div>
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
              consultarDetalles={this.consultarDetalles}
              seleccionarEditarReserva={this.seleccionarEditarReserva}
              eliminarReserva={this.eliminarReserva}
            />

            <Dialog
              header="Detalles de Reservas"
              visible={this.state.detallesDialog}
              width="750px"
              baseZIndex={1000}
              maximizable={true}
              dismissableMask={true}
              style={{ height: "660px", overflow: scroll }}
              minY={70}
              onHide={this.ocultarDialogDetalles}
            >
              <DetallesReservaTable data={this.state.detallesReserva} />
            </Dialog>

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
              header="Reserva"
              dismissableMask={true}
              visible={this.state.startForm}
              width="600px"
              modal={true}
              onHide={() => this.setState({ startForm: false })}
            ></Dialog>
            <form onSubmit={this.EnviarJustificacion}>
              <div className="row">
                <div className="col">
                  <Field
                    name="fechaActivacion"
                    label="Fecha"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fechaActivacion}
                    onChange={this.handleChange}
                    error={this.state.errors.fechaActivacion}
                  />
                  <Field
                    name="justificacion"
                    label="Justificación"
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
                onClick={this.onHide}
              >
                Cancelar
              </button>
            </form>
            <Dialog
              header="Iniciar/Finalizar Consumo"
              visible={this.state.viewenable}
              style={{ width: "50vw" }}
              modal
              onHide={this.onHideview}
            >
              <div>
                Está seguro de Iniciar/Finalizar el Consumo¿Desea continuar?
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
                  onClick={this.setState({ viewenable: false })}
                />
              </div>
            </Dialog>
          </div>
        </div>
      </div>
    );
  }

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
      url = `/Proveedor/ReservaHotel/ListarReservas?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;
      http
        .get(url)
        .then((response) => {
          let data = response.data;
          if (data.success === true) {
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

  consultarDetalles = (reservaId) => {
    event.preventDefault();
    this.props.blockScreen();
    let url = "";
    url = `/Proveedor/DetalleReserva/ListarDetallesPorReserva/${reservaId}`;
    http
      .get(url)
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          this.setState({ detallesReserva: data.result, detallesDialog: true });
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
        console.log(data);
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
    this.setState({ reserva, editarDialog: true });
  };

  handleChange = (event) => {
    this.setState({ [event.target.name]: event.target.value });
  };

  ocultarDialogDetalles = () => {
    this.setState({ detallesDialog: false });
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
