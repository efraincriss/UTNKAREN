import axios from "axios";
import { Button } from "primereact-v2/button";
import { FileUpload } from "primereact-v2/fileupload";
import { Dialog } from "primereact-v3.3/dialog";
import React, { Fragment } from "react";
import ReactDOM from "react-dom";
import Wrapper from "../../Base/BaseWrapper";
import Field from "../../Base/Field-v2";
import http from "../../Base/HttpService";
import {
  CONTROLLER_COLABORADOR_RUBRO,
  CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA,
  CONTROLLER_PORCENTAJE_INDIRECTOS_INGENIERIA,
  FRASE_DETALLE_INDIRECTO_ELIMINADO,
  FRASE_ERROR_GLOBAL,
  FRASE_PORCENTAJE_INDIRECTO_ELIMINADO,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings";
import { CertificadosTable } from "./CertificadosTable.jsx";
import CurrencyFormat from "react-currency-format";

export class DetalleGrupo extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      mostrarConfirmacion: false,
      certificadoSeleccionado: {},
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
                  <b>Fecha Inicio: </b>{" "}
                  {this.props.detallegrupo != null
                    ? this.props.detallegrupo.FechaInicioDate
                    : ""}
                </p>
              </div>
              <div className="col">
                <p>
                  <b>Fecha Fin: </b>{" "}
                  {this.props.detallegrupo != null
                    ? this.props.detallegrupo.FechaFinDate
                    : ""}
                </p>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <p>
                  <b>Fecha Certificado: </b>{" "}
                  {this.props.detallegrupo != null
                    ? this.props.detallegrupo.FechaCertificadoDate
                    : ""}
                </p>
              </div>
              <div className="col">
                <p>
                  <b>Fecha Generación: </b>{" "}
                  {this.props.detallegrupo != null
                    ? this.props.detallegrupo.FechaGeneracionDate
                    : ""}
                </p>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <p>
                  <b>Cliente: </b>{" "}
                  {this.props.detallegrupo != null
                    ? this.props.detallegrupo.NombreCliente
                    : ""}
                </p>
              </div>
            </div>
          </div>
        </div>

        <div className="card">
          <div className="card-body">
            <div className="row" style={{ marginTop: "1em" }}>
              <div className="col" align="right"></div>
            </div>

            <div className="row">

              <div className="col">
                <div className="callout callout-info">
                  <small className="text-muted">Total Horas Act Certificadas</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.props.totalHorasActCertificadas.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>

              <div className="col">
                <div className="callout callout-danger">
                  <small className="text-muted">Total Horas Ant Certificadas</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.props.totalHorasAntCertificadas.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>

              <div className="col">
                <div className="callout callout-warning">
                  <small className="text-muted">Total Monto Act Certificado</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.props.totalMontoActCertificado.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>

              <div className="col">
                <div className="callout callout-success">
                  <small className="text-muted">Total Monto Ant Certificado</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.props.totalMontoAntCertificado.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>

              <div className="col">
                <div className="callout callout-danger">
                  <small className="text-muted">Total Montos Directos</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.props.totalMontoDirecto.toFixed(2)
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={""}
                    />
                  </strong>
                </div>
              </div>

              <div className="col">
                <div className="callout callout-warning">
                  <small className="text-muted">Total Montos Indirectos</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.props.totalMontoIndirecto.toFixed(2)
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

            <div className="row">
              <div className="col">
                <CertificadosTable
                  afterColumnFilter={this.props.afterColumnFilter}
                  data={this.props.certificados}
                  aprobarCertificado={this.props.aprobarCertificado}
                  mostrarFormularioGastos={this.props.mostrarFormularioGastos}
                  mostrarConfirmacionParaEliminar={
                    this.mostrarConfirmacionParaEliminar
                  }
                  obtenerGastosCertificado={this.props.obtenerGastosCertificado}
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
