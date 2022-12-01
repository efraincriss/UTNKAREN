import { Button } from "primereact-v2/button"
import React, { Fragment } from "react"
import ReactDOM from "react-dom"
import Wrapper from "../../Base/BaseWrapper"
import { Calendar, momentLocalizer } from "react-big-calendar"
import moment from "moment"
import {
  CONTROLLER_FERIADOS,
  FRASE_ERROR_GLOBAL,
  FRASE_FERIADO_ELIMINADA,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings"
import { Dialog } from "primereact-v3.3/dialog"
import { FeriadoForm } from "./FeriadosForm.jsx"
import http from "../../Base/HttpService"
import { FeriadosTable } from "./FeriadosTable.jsx"

class FeriadosContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      eventos: [],
      feriados: [],
      feriadoSeleccionado: {},
      mostrarConfirmacion: false,
      mostrarFormulario: false,
      mostrarListado: false,
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
          <div className="row" style={{ marginTop: "1em" }}>
            <div className="col" align="right">
              <Button
                style={{ marginLeft: "0.4em" }}
                label="Ver Lista"
                className="p-button-outlined"
                onClick={() => this.mostrarListado()}
                icon="pi pi-list"
              />
            </div>
          </div>

          <hr />

          <div className="row">
            <div className="col" style={{ height: "530px" }}>
              <Calendar
                localizer={localizer}
                events={this.state.feriados}
                startAccessor="start"
                endAccessor="end"
                onSelectEvent={this.onSelectEvent}
                onDoubleClickEvent={this.onDoubleClickEvent}
                views={["month", "week", "work_week", "day"]}
                onSelectSlot={this.onSelectSlot}
                selectable
                culture="es"
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

          <Dialog
            header="Gestión de Feriados"
            modal={true}
            visible={this.state.mostrarFormulario}
            style={{ width: "750px" }}
            onHide={this.onHideFormulario}
          >
            <FeriadoForm
              feriado={this.state.feriadoSeleccionado}
              actualizarFeriadoSeleccionado={this.actualizarFeriadoSeleccionado}
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

          <Dialog
            header="Listado de Feriados"
            modal={true}
            visible={this.state.mostrarListado}
            style={{ width: "750px" }}
            onHide={this.onHideListado}
          >
            <FeriadosTable data={this.state.eventos} />
          </Dialog>

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
                  ¿Está seguro de eliminar el feriado? Si desea continuar
                  presione ELIMINAR, caso contrario CANCELAR
                </p>
              </div>
            </div>
          </Dialog>
        </div>
      </div>
    )
  }

  onSelectEvent = (event) => {
    this.mostrarFormulario(event.resource)
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
    const FechaInicio = moment(start).format("YYYY-MM-DD")
    const FechaFin = moment(end).subtract(1, "days").format("YYYY-MM-DD")
    this.mostrarFormulario({ FechaInicio, FechaFin, Horas: 8 })
  }

  loadData = () => {
    this.props.blockScreen()
    var self = this
    Promise.all([this.obtenerFeriados()])
      .then(function ([feriados]) {
        let feriadosData = feriados.data
        if (feriadosData.success === true) {
          var data = self.castFeriados(feriadosData.result)
          self.setState({
            feriados: data,
            eventos: feriadosData.result,
          })
        }
        self.props.unlockScreen()
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  castFeriados = (feriados) => {
    Date.prototype.addDays = function (days) {
      const date = new Date(this.valueOf())
      date.setDate(date.getDate() + days)
      return date
    }

    return feriados.map((feriado) => {
      return {
        title: feriado.Descripcion,
        start: new Date(feriado.FechaInicio),
        end: new Date(feriado.FechaFin).addDays(1),
        allDay: true,
        resource: feriado,
      }
    })
  }

  obtenerFeriados = () => {
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_FERIADOS}/ObtenerFeriados`
    return http.get(url)
  }

  eliminarFeriado = () => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_FERIADOS}/EliminarFeriado/${this.state.feriadoSeleccionado.Id}`

    http
      .delete(url, {})
      .then((response) => {
        console.log(response)
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_FERIADO_ELIMINADA)
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

  mostrarFormulario = async (feriado) => {
    this.setState({
      feriadoSeleccionado: feriado,
      mostrarFormulario: true,
    })
  }

  mostrarConfirmacionParaEliminar = (feriado) => {
    this.setState({
      mostrarConfirmacion: true,
    })
  }

  onHideFormulario = (recargar = false) => {
    this.setState({
      mostrarFormulario: false,
      feriadoSeleccionado: {},
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

  actualizarFeriadoSeleccionado = (feriado) => {
    this.setState({ feriadoSeleccionado: feriado })
  }

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarFeriado()}
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
}

const Container = Wrapper(FeriadosContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("dias_feriado_container")
)
