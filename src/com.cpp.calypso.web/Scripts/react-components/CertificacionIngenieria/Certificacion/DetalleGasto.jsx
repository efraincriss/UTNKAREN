import axios from "axios";
import { Button } from "primereact-v2/button";
import { FileUpload } from "primereact-v2/fileupload";
import { Dialog } from "primereact-v3.3/dialog";
import React, { Fragment } from "react";
import ReactDOM from "react-dom";
import Wrapper from "../../Base/BaseWrapper";
import Field from "../../Base/Field-v2";
import http from "../../Base/HttpService";
import CurrencyFormat from "react-currency-format";
import {
  CONTROLLER_COLABORADOR_RUBRO,
  CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA,
  CONTROLLER_PORCENTAJE_INDIRECTOS_INGENIERIA,
  FRASE_DETALLE_INDIRECTO_ELIMINADO,
  FRASE_ERROR_GLOBAL,
  FRASE_PORCENTAJE_INDIRECTO_ELIMINADO,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings";
import { GastosDirectosTable } from "./GastosDirectosTable.jsx";

export class DetalleGasto extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      mostrarConfirmacion: false,
      certificadoSeleccionado: {},
      totalHoras: 0.0,
      montoTotal: 0.0,
      isUpdating: true,
    };
  }

  render() {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col" align="right">
                <Button
                  label="Regresar"
                  className="p-button-outlined"
                  onClick={() => this.props.onHideFormulario()}
                  icon="pi pi-chevron-left"
                />
              </div>
            </div>
            <hr />

            <div className="row">
              <div className="col">
                <p>
                  <b>Contrato: </b>{" "}
                  {this.props.certificadoSeleccionado != null
                    ? this.props.certificadoSeleccionado.NombreContrato
                    : ""}
                </p>
              </div>
              <div className="col">
                <p>
                  <b>Proyecto: </b>{" "}
                  {this.props.certificadoSeleccionado != null
                    ? this.props.certificadoSeleccionado.NombreProyecto
                    : ""}
                </p>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <p>
                  <b>Número Certificado: </b>{" "}
                  {this.props.certificadoSeleccionado != null
                    ? this.props.certificadoSeleccionado.NumeroCertificado
                    : ""}
                </p>
              </div>
              <div className="col">
                <p>
                  <b>Estado: </b>{" "}
                  {this.props.certificadoSeleccionado != null
                    ? this.props.certificadoSeleccionado.EstadoString
                    : ""}
                </p>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <p>
                  <b>Monto Actual Certificado: </b>{" "}
                  {this.props.certificadoSeleccionado != null ? (
                    <CurrencyFormat
                      value={
                        this.props.certificadoSeleccionado
                          .MontoActualCertificado
                          ? this.props.certificadoSeleccionado
                              .MontoActualCertificado
                          : 0.0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  ) : (
                    ""
                  )}
                </p>
              </div>
              <div className="col">
                <p>
                  <b>Monto Anterior Certificado: </b>{" "}
                  {this.props.certificadoSeleccionado != null ? (
                    <CurrencyFormat
                      value={
                        this.props.certificadoSeleccionado
                          .MontoAnteriorCertificado
                          ? this.props.certificadoSeleccionado
                              .MontoAnteriorCertificado
                          : 0.0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  ) : (
                    ""
                  )}
                </p>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <p>
                  <b>Monto Total Directos: </b>{" "}
                  {this.props.certificadoSeleccionado != null ? (
                    <CurrencyFormat
                      value={
                        this.props.certificadoSeleccionado.TotalHorasDirectos
                          ? this.props.certificadoSeleccionado
                              .TotalHorasDirectos
                          : 0.0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  ) : (
                    ""
                  )}
                </p>
              </div>
              <div className="col">
                <p>
                  <b>Monto Total Indirectos: </b>{" "}
                  {this.props.certificadoSeleccionado != null ? (
                    <CurrencyFormat
                      value={
                        this.props.certificadoSeleccionado.TotalHorasIndirectos
                          ? this.props.certificadoSeleccionado
                              .TotalHorasIndirectos
                          : 0.0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  ) : (
                    ""
                  )}
                </p>
              </div>
            </div>
          </div>
        </div>

        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col-3">
                <div className="callout callout-info">
                  <small className="text-muted">Total Horas HH</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={this.state.totalHoras.toFixed(2)}
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
                  <small className="text-muted">Monto Total</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={this.state.montoTotal.toFixed(2)}
                      displayType={"text"}
                      thousandSeparator={true}
                      decimalScale={2}
                      fixedDecimalScale={true}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
            </div>
            <hr />

            <div className="row">
              <div className="col">
                <GastosDirectosTable
                 afterColumnFilter={this.afterColumnFilter}
                  data={this.props.gastos}
                  mostrarConfirmacionParaEliminar={
                    this.mostrarConfirmacionParaEliminar
                  }
                />
              </div>
            </div>

            <Dialog
              header="Confirmación"
              visible={this.state.mostrarConfirmacion}
              modal
              style={{ width: "500px" }}
              footer={this.construirBotonesDeConfirmacion()}
              onHide={this.onHideConfirmacion}
            >
              <div className="confirmation-content">
                <div className="p-12">
                  <i
                    className="pi pi-exclamation-triangle p-mr-3"
                    style={{ fontSize: "2rem" }}
                  />
                  <p>
                    {" "}
                    ¿Está seguro de eliminar el porcentaje? Si desea continuar
                    presione ELIMINAR, caso contrario CANCELAR
                  </p>
                </div>
              </div>
            </Dialog>
          </div>
        </div>
      </Fragment>
    );
  }
  afterColumnFilter = async (filterConds, result) => {
    console.log(filterConds)
    console.log(result)
    if (result != undefined) {
      /*Suma Horas */
      let ArrayHoras = result.map((item) => {
        var horas = parseFloat(item.TotalHoras);
        return { horas };
      });
      let total = 0;
      for (let i of ArrayHoras) total += i.horas;

      let ArrayMontos = result.map((item) => {
        var monto = parseFloat(item.MontoTotal);
        return { monto };
      });

      let montototal = 0;
      for (let i of ArrayMontos) montototal += i.monto;

  if (this.state.isUpdating) {
    //inicia
    this.setState({ isUpdating: false });
    await setTimeout(1000);
     this.setState({ totalHoras: total, montoTotal: montototal  });
    
    setTimeout(function () {
      this.setState({ isUpdating: true });
    }.bind(this), 1000);
  }

    }
  };

  mostrarConfirmacionParaEliminar = (porcentaje) => {
    this.setState({
      porcentajeSeleccionado: porcentaje,
      mostrarConfirmacion: true,
    });
  };

  onHideConfirmacion = (recargar = false) => {
    this.setState({
      mostrarConfirmacion: false,
    });
    if (recargar) {
      this.props.obtenerProcentajes();
    }
  };

  obtenerDetallesIndirectos = () => {
    let fechaInicio = this.state.FechaInicio;
    let fechaFin = this.state.FechaFin;
    const errors = {};

    if (fechaInicio !== "") {
      if (fechaFin === "") {
        errors.FechaFin = "Fecha de finalización es requerida";
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
    Promise.all([this.promiseObtenerDetallesIndirectosIngenieria()])
      .then(function ([detalles]) {
        let detallesData = detalles.data;
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

  eliminarPorcentajeIndirectoIngenieria = () => {
    this.props.blockScreen();
    let url = "";
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PORCENTAJE_INDIRECTOS_INGENIERIA}/Eliminar/${this.state.porcentajeSeleccionado.Id}`;

    http
      .delete(url, {})
      .then((response) => {
        console.log(response);
        let data = response.data;
        if (data.success === true) {
          this.props.showSuccess(FRASE_PORCENTAJE_INDIRECTO_ELIMINADO);
          this.onHideConfirmacion(true);
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

  promiseObtenerDetallesIndirectosIngenieria = () => {
    let url = "";
    let params = `?fechaInicio=${this.state.FechaInicio}&fechaFin=${this.state.FechaFin}`;
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA}/ObtenerIndirectosIngenieriaPorFechas${params}`;
    return http.get(url);
  };

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarPorcentajeIndirectoIngenieria()}
          icon="pi pi-save"
        />
        <Button
          style={{ marginLeft: "0.4em" }}
          label="Cancelar"
          className="p-button-outlined"
          onClick={() => this.onHideConfirmacion()}
          icon="pi pi-ban"
        />
      </Fragment>
    );
  };
}
