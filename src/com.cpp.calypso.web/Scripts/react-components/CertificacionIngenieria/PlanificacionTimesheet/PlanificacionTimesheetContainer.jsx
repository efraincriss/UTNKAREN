import moment from "moment"
import { Button } from "primereact-v2/button"
import { Dialog } from "primereact-v3.3/dialog"
import { TabView, TabPanel } from "primereact-v3.3/tabview"
import React, { Fragment } from "react"
import { Calendar, momentLocalizer } from "react-big-calendar"
import ReactDOM from "react-dom"
import Wrapper from "../../Base/BaseWrapper"
import http from "../../Base/HttpService"
import {
  CONTROLLER_PLANIFICACION_TIMESHEET,
  FRASE_ERROR_GLOBAL,
  FRASE_PLANIFICACION_ANUAL_CREADA,
  FRASE_PLANIFICACION_DESCARGADA,
  FRASE_PLANIFICACION_ELIMINADA,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"
import { PlanificacionTimesheetForm } from "./PlanificacionTimesheetForm.jsx"
import { PlanificacionTimesheetTable } from "./PlanificacionTimesheetTable.jsx"
import axios from "axios"
import Field from "../../Base/Field-v2"

class PlanificacionTimesheetContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      eventos: [],
      planificaciones: [],
      planificacionSeleccionada: {},
      mostrarConfirmacion: false,
      mostrarFormulario: false,
      mostrarListado: false,
      Fecha: "",
      errors: {},
    }
  }

  componentDidMount() {
    this.loadData()
  }

  render() {
    const localizer = momentLocalizer(moment)

    let formats = {
      dayRangeHeaderFormat: ({ start, end }, culture, localizer) => {
        console.log(start)
        console.log(end)
        return (
          localizer.format(start, { date: "short" }, culture) +
          " — " +
          localizer.format(end, { date: "short" }, culture)
        )
      },
    }
    return (
      <div className="card">
        <div className="card-body">
          <div className="row">
            <TabView style={{ width: '100%' }}>
              <TabPanel header="Calendario">

                <div className="row" style={{ marginTop: "1em", marginBottom: "1em" }}>
                  <div className="col" align="right">
                    {/* <Button
                          style={{ marginLeft: "0.4em" }}
                          label="Ver Lista"
                          className="p-button-outlined"
                          onClick={() => this.mostrarListado()}
                          icon="pi pi-list"
                        /> */}
                    {/* <Button
                    style={{ marginLeft: "0.4em" }}
                    label="Calcular Planificación"
                    className="p-button-outlined"
                    onClick={() => this.calcularFeriadosDelAño()}
                    icon="pi pi-chart-bar"
                  />  */}
                    <Button
                      style={{ marginLeft: "0.4em" }}
                      label="Descargar Planificación"
                      className="p-button-outlined"
                      icon="pi pi-folder-open"
                      data-toggle="collapse"
                      data-target="#collapseExample"
                      aria-expanded="false"
                      aria-controls="collapseExample"
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col">
                    <div className="collapse" id="collapseExample">
                      <div className="card card-body">
                        <div className="row">
                          <div className="col">
                            <Field
                              name="Fecha"
                              value={this.state.Fecha}
                              label="Fecha"
                              type="date"
                              onChange={this.handleChange}
                              error={this.state.errors.Fecha}
                              edit={true}
                              readOnly={false}
                            />
                          </div>
                        </div>
                        <div className="row">
                          <div className="col">
                            <Button
                              style={{ marginLeft: "0.4em" }}
                              label="Descargar"
                              className="p-button-outlined"
                              onClick={() => this.descargarPlaficacion()}
                              icon="pi pi-folder-open"
                            />
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <hr />

                <div className="row">
                  <div className="col" style={{ height: "530px" }}>
                    <Calendar
                      localizer={localizer}
                      events={this.state.planificaciones}
                      startAccessor="start"
                      endAccessor="end"
                      onSelectEvent={this.onSelectEvent}
                      onDoubleClickEvent={this.onDoubleClickEvent}
                      views={["month", "week", "work_week", "day"]}
                      onSelectSlot={this.onSelectSlot}
                      selectable
                      culture="es"
                      eventPropGetter={(this.eventStyleGetter)}
                      messages={{
                        month: "Mes",
                        day: "Día",
                        today: "Hoy",
                        agenda: "Agenda",
                        allDay: "Todo el día",
                        previous: "Regresar",
                        next: "Siguiente",
                        date: "Fecha",
                        time: "Tiempo",
                        tomorrow: "Mañana",
                        week: "Semana",
                        work_week: "Semana Laboral",
                        yesterday: "Ayer",
                      }}
                    />
                  </div>
                </div>
              </TabPanel>

              <TabPanel header="Listado" style={{ height: "100%" }}>
                <div className="card">
                  <div className="card-body">

                    <div className="row" style={{ marginTop: "1em", marginBottom: "1em" }}>
                      <div className="col" align="right">
                        <Button
                          style={{ marginLeft: "0.4em" }}
                          label="Crear Planificación"
                          className="p-button-outlined"
                          onClick={() => this.onCreateNewEvent()}
                          icon="pi pi-chart-bar"
                        />

                      </div>
                    </div>

                    <hr />

                    <PlanificacionTimesheetTable
                      data={this.state.eventos}
                      editarPlanificacion={this.onEditPlanificacionOnTable}
                      mostrarConfirmacionParaEliminar={
                        this.onDeletePlanificacion
                      }
                    />
                  </div>
                </div>

              </TabPanel>

            </TabView>
            <Dialog
              header="Gestión de Planificaciones Timesheet"
              modal={true}
              visible={this.state.mostrarFormulario}
              style={{ width: "750px" }}
              onHide={this.onHideFormulario}
            >
              <PlanificacionTimesheetForm
                planificacion={this.state.planificacionSeleccionada}
                actualizarPlanificacionSeleccionada={
                  this.actualizarPlanificacionSeleccionada
                }
                onHideFormulario={this.onHideFormulario}
                showSuccess={this.props.showSuccess}
                showWarn={this.props.showWarn}
                blockScreen={this.props.blockScreen}
                unlockScreen={this.props.unlockScreen}
                key={this.state.keyForm}
                mostrarConfirmacionParaEliminar={
                  this.mostrarConfirmacionParaEliminar
                }
              />
            </Dialog>

            {/* <Dialog
                      header="Listado de Planificaciones"
                      modal={true}
                      visible={this.state.mostrarListado}
                      style={{ width: "750px" }}
                      onHide={this.onHideListado}
                    >
                      <PlanificacionTimesheetTable data={this.state.eventos} />
                    </Dialog> */}

            <Dialog
              header="Confirmación"
              visible={this.state.mostrarConfirmacion}
              modal
              style={{ width: "500px" }}
              footer={this.construirBotonesDeConfirmacion()}
              onHide={this.onHideConfirmacionEliminar}
            >
              <div className="confirmation-content">
                <div className="p-12">
                  <i
                    className="pi pi-exclamation-triangle p-mr-3"
                    style={{ fontSize: "2rem" }}
                  />
                  <p>
                    {" "}
                    ¿Está seguro de eliminar la planificación de esta fecha? Si desea continuar
                    presione ELIMINAR, caso contrario CANCELAR
                  </p>
                </div>
              </div>
            </Dialog>
          </div>
        </div>
      </div>
    )
  }

  onSelectEvent = (event) => {
    console.log(event)
    this.mostrarFormulario(event.resource)
  }

  onEditPlanificacionOnTable = (planificacion) => {
    this.mostrarFormulario(planificacion)
  }

  onDeletePlanificacion = (planificacion) => {
    this.setState({
      mostrarConfirmacion: true,
      planificacionSeleccionada: planificacion
    })
  }

  onDoubleClickEvent = (event) => {
    console.log(event)
    this.mostrarConfirmacionParaEliminar(event.resource)
  }

  onEventDrop = (event) => {
    console.log(event)
  }

  onEventResize = (event) => {
    console.log(event)
  }

  onSelectSlot = ({ start, end }) => {
    const Fecha = moment(start).format("YYYY-MM-DD")
    this.mostrarFormulario({ Fecha })
  }

  onCreateNewEvent = () => {
    this.mostrarFormulario({})
  }

  loadData = () => {
    this.props.blockScreen()
    var self = this
    Promise.all([this.obtenerPlanificaciones()])
      .then(function ([planificacion]) {
        let planificacionData = planificacion.data
        if (planificacionData.success === true) {
          var data = self.castPlanificaciones(planificacionData.result)
          self.setState({
            planificaciones: data,
            eventos: planificacionData.result,
          })
        }
        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  castPlanificaciones = (planificaciones) => {
    Date.prototype.addDays = function (days) {
      const date = new Date(this.valueOf())
      date.setDate(date.getDate() + days)
      return date
    }

    return planificaciones.map((planificacion) => {
      let color = '';
      switch (planificacion.TipoPlanificacion) {
        case 0:
          color = '303F9F'
          break;
        case 1:
          color = 'D32F2F'
          break;
        case 2:
          color = '512DA8'
          break;
        case 3:
          color = '0097A7'
          break;
        case 4:
          color = 'F57C00'
          break;
        case 5:
          color = 'FF5722'
          break;
        default:
          color = 'FFEB3B'
          break;
      }
      return {
        title: planificacion.Descripcion,
        start: new Date(planificacion.Fecha),
        end: new Date(planificacion.Fecha),
        allDay: true,
        resource: planificacion,
        hexColor: color
      }
    })
  }

  obtenerPlanificaciones = () => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PLANIFICACION_TIMESHEET}/ObtenerPlanificaciones`
    return http.get(url)
  }

  eliminarPlanificacion = () => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PLANIFICACION_TIMESHEET}/EliminarPlanificacion/${this.state.planificacionSeleccionada.Id}`

    http
      .delete(url, {})
      .then((response) => {
        console.log(response)
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_PLANIFICACION_ELIMINADA)
          this.onHideFormulario(true)
          this.onHideConfirmacionEliminar()
          this.loadData()
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

  calcularFeriadosDelAño = () => {
    this.props.blockScreen()
    let url = ""
    let year = new Date().getFullYear()
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PLANIFICACION_TIMESHEET}/CrearPlanificacionPorAño`

    http
      .post(url, {
        year,
      })
      .then((response) => {
        console.log(response)
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_PLANIFICACION_ANUAL_CREADA)
          this.loadData()
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

  descargarPlaficacion = () => {
    if (this.state.Fecha === "") {
      this.props.showWarn("Seleccione una fecha")
    } else {
      this.props.blockScreen()
      axios
        .post(
          `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_PLANIFICACION_TIMESHEET}/DescargarPlanificacionPorMes`,
          { fecha: this.state.Fecha },
          { responseType: "arraybuffer" }
        )
        .then((response) => {
          var nombre = response.headers["content-disposition"].split("=")

          const url = window.URL.createObjectURL(
            new Blob([response.data], {
              type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            })
          )
          const link = document.createElement("a")
          link.href = url
          link.setAttribute("download", nombre[1])
          document.body.appendChild(link)
          link.click()
          this.props.showSuccess(FRASE_PLANIFICACION_DESCARGADA)
          this.props.unlockScreen()
          this.setState({
            ContratoId: 0,
          })
        })
        .catch((error) => {
          console.log(error)
          this.props.showWarn(
            "Ocurrió un error al descargar el archivo, intentalo nuevamente"
          )
          this.props.unlockScreen()
        })
    }
  }

  mostrarFormulario = async (planificacion) => {
    this.setState({
      planificacionSeleccionada: planificacion,
      mostrarFormulario: true,
    })
  }

  mostrarConfirmacionParaEliminar = (planificacion) => {
    this.setState({
      mostrarConfirmacion: true,
    })
  }

  onHideFormulario = (recargar = false) => {
    this.setState({
      mostrarFormulario: false,
      planificacionSeleccionada: {},
      keyForm: Math.random(),
    })
    if (recargar) {
      this.loadData()
    }
  }

  mostrarListado = () => {
    this.setState({ mostrarListado: true })
  }

  onHideListado = () => {
    this.setState({ mostrarListado: false })
  }

  onHideConfirmacionEliminar = () => {
    this.setState({ mostrarConfirmacion: false })
  }

  actualizarPlanificacionSeleccionada = (planificacion) => {
    this.setState({ planificacionSeleccionada: planificacion })
  }

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarPlanificacion()}
          icon="pi pi-save"
        />
        <Button
          style={{ marginLeft: "0.4em" }}
          label="Cancelar"
          className="p-button-outlined"
          onClick={() => this.onHideConfirmacionEliminar()}
          icon="pi pi-ban"
        />
      </Fragment>
    )
  }

  handleChange = (event) => {
    const target = event.target
    const value = target.value
    const name = target.name
    this.setState({ [name]: value })
  }

  eventStyleGetter = (event, start, end, isSelected) => {
    console.log(event);
    var backgroundColor = '#' + event.hexColor;
    var style = {
        backgroundColor: backgroundColor,
        borderRadius: '0px',
        opacity: 0.8,
        color: 'white',
        border: '0px',
        display: 'block'
    };
    return {
        style: style
    };
}
}

const Container = Wrapper(PlanificacionTimesheetContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("planificacion_timesheet_container")
)
