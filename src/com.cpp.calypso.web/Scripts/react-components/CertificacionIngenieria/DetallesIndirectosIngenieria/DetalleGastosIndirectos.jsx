import axios from "axios"
import { Button } from "primereact-v2/button"
import { FileUpload } from "primereact-v2/fileupload"
import { Dialog } from "primereact-v3.3/dialog"
import React, { Fragment } from "react"
import ReactDOM from "react-dom"
import Wrapper from "../../Base/BaseWrapper"
import Field from "../../Base/Field-v2"
import http from "../../Base/HttpService"
import {
  CONTROLLER_COLABORADOR_RUBRO,
  CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA,
  CONTROLLER_PORCENTAJE_INDIRECTOS_INGENIERIA,
  FRASE_DETALLE_INDIRECTO_ELIMINADO,
  FRASE_ERROR_GLOBAL,
  FRASE_PORCENTAJE_INDIRECTO_ELIMINADO,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"
import { DetallesIndirectosIngenieriaForm } from "./DetalleIndirectoIngenieriaForm.jsx"
import { DetalleIndirectoIngenieriaTable } from "./DetalleIndirectoIngenieriaTable.jsx"
import { PorcentajeIndirectosTable } from "./PorcentajeIndirectosTable.jsx"

export class DetalleGastosIndirectos extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      mostrarConfirmacion: false,
      porcentajeSeleccionado: {},
    }
  }

  render() {
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col">
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
                  <b>Colaborador: </b>{" "}
                  {this.props.detalleIndirecto.ColaboradorNombres}
                </p>
              </div>
              <div className="col">
                <p>
                  <b>Identificación: </b>{" "}
                  {this.props.detalleIndirecto.ColaboradorIdentificacion}
                </p>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <p>
                  <b>Fecha Desde: </b> {this.props.detalleIndirecto.FechaDesde}
                </p>
              </div>
              <div className="col">
                <p>
                  <b>Fecha Hasta: </b> {this.props.detalleIndirecto.FechaHasta}
                </p>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <p>
                  <b>Horas Laboradas: </b>{" "}
                  {this.props.detalleIndirecto.HorasLaboradas}
                </p>
              </div>
              <div className="col">
                <p>
                  <b>Días Laborados: </b>{" "}
                  {this.props.detalleIndirecto.DiasLaborados}
                </p>
              </div>
            </div>
          </div>
        </div>

        <div className="card">
          <div className="card-body">
            <div className="row" style={{ marginTop: "1em" }}>
              <div className="col" align="right">
                <Button
                  label="Nuevo Porcentaje"
                  className="p-button-outlined"
                  onClick={() =>
                    this.props.mostrarFormularioPocentajes({
                      DetalleIndirectosIngenieriaId:
                        this.props.detalleIndirecto.Id,
                    })
                  }
                  icon="pi pi-search"
                />
              </div>
            </div>
            <hr />

            <div className="row">
              <div className="col">
                <PorcentajeIndirectosTable
                  data={this.props.porcentajes}
                  mostrarFormularioPocentajes={
                    this.props.mostrarFormularioPocentajes
                  }
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
    )
  }

  mostrarConfirmacionParaEliminar = (porcentaje) => {
    this.setState({
      porcentajeSeleccionado: porcentaje,
      mostrarConfirmacion: true,
    })
  }

  onHideConfirmacion = (recargar = false) => {
    this.setState({
      mostrarConfirmacion: false,
    })
    if (recargar) {
      this.props.obtenerProcentajes()
    }
  }

  obtenerDetallesIndirectos = () => {
    let fechaInicio = this.state.FechaInicio
    let fechaFin = this.state.FechaFin
    const errors = {}

    if (fechaInicio !== "") {
      if (fechaFin === "") {
        errors.FechaFin = "Fecha de finalización es requerida"
        this.setState({ errors })
        return
      }
    }

    if (fechaFin !== "") {
      if (fechaInicio === "") {
        errors.FechaInicio = "Fecha de inicio es requerida"
        this.setState({ errors })
        return
      }
    }
    this.setState({ errors })

    this.props.blockScreen()
    var self = this
    Promise.all([this.promiseObtenerDetallesIndirectosIngenieria()])
      .then(function ([detalles]) {
        let detallesData = detalles.data
        if (detallesData.success === true) {
          self.setState({
            detalles: detallesData.result,
          })
        }
        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  eliminarPorcentajeIndirectoIngenieria = () => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PORCENTAJE_INDIRECTOS_INGENIERIA}/Eliminar/${this.state.porcentajeSeleccionado.Id}`

    http
      .delete(url, {})
      .then((response) => {
        console.log(response)
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_PORCENTAJE_INDIRECTO_ELIMINADO)
          this.onHideConfirmacion(true)
        } else {
          var message = data.result
          this.props.showWarn(message)
          this.props.unlockScreen()
        }
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(FRASE_ERROR_GLOBAL)
        this.props.unlockScreen()
      })
  }

  promiseObtenerDetallesIndirectosIngenieria = () => {
    let url = ""
    let params = `?fechaInicio=${this.state.FechaInicio}&fechaFin=${this.state.FechaFin}`
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA}/ObtenerIndirectosIngenieriaPorFechas${params}`
    return http.get(url)
  }

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
    )
  }
}
