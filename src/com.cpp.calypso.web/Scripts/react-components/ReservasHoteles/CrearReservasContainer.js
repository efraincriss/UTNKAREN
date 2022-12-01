import React from "react";
import ReactDOM from "react-dom";
import Wrapper from "../Base/BaseWrapper";
import EspaciosDisponiblesTable from "./EspaciosDisponiblesTable";
import http from "../Base/HttpService";
import { Dialog } from "primereact/components/dialog/Dialog";
import {
  FRASE_ERROR_SELECCIONA_FECHA_INICIO,
  FRASE_ERROR_SELECCIONA_FECHA_FIN,
  FRASE_RESERVA_CREADA,
  MODULO_PROVEEDOR,
  CONTROLLER_RESERVA_HOTEL,
} from "../Base/Strings";
import config from "../Base/Config";
import Field from "../Base/Field-v2";
import BuscarColaboradorContainer from "./BuscarColaborador/BuscarColaboradorContainer";
import moment from "moment";

export class CrearReservaContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data: [],
      fechaInicio: "",
      fechaFin: "",
      colaborador: {},
      visible: false,
      espacioId: 0,
      errors: {},
      colaboradorId: 0,
      diasjornada: "0",
    };

    this.busquedaRef = React.createRef();
  }

  componentDidMount() {
    this.getDiasJornadaCampo();
    this.props.unlockScreen();
  }

  render() {
    return (
      <div>
        <div className="row">
          <div className="col">
            <form onSubmit={this.consultarEspaciosDisponibles}>
              <div className="row">
                <div className="col-xs-6 col-md-5">
                  <Field
                    name="fechaInicio"
                    label="Fecha Inicio"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fechaInicio}
                    onChange={this.handleChangeFechaInicio}
                    error={this.state.errors.fechaInicio}
                  />
                </div>
                <div className="col-xs-6 col-md-5">
                  <Field
                    name="fechaFin"
                    label="Fecha Fin"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fechaFin}
                    onChange={this.handleChange}
                    error={this.state.errors.fechaFin}
                  />
                </div>

                <div className="col" style={{ paddingTop: "35px" }}>
                  <button type="submit" className="btn btn-outline-primary">
                    Buscar
                  </button>
                  &nbsp;
                </div>
              </div>
            </form>
            <hr />
          </div>
        </div>

        <div className="row" style={{ marginTop: "1em" }}>
          <div className="col">
            <EspaciosDisponiblesTable
              data={this.state.data}
              seleccionarEspacio={this.seleccionarEspacio}
            />

            <Dialog
              header="Colaborador"
              visible={this.state.visible}
              width="830px"
              modal={true}
              onHide={this.ocultarForm}
            >
              <BuscarColaboradorContainer
                ref={this.busquedaRef}
                seleccionarColaborador={this.seleccionarColaborador}
                unlockScreen={this.props.unlockScreen}
                blockScreen={this.props.blockScreen}
                showSuccess={this.props.showSuccess}
                showWarn={this.props.showWarn}
              />
            </Dialog>
          </div>
        </div>
      </div>
    );
  }

  crearEspacio = () => {
    this.props.blockScreen();
    if (this.state.fechaInicio === "") {
      this.props.showWarn(FRASE_ERROR_SELECCIONA_FECHA_INICIO);
      this.props.unlockScreen();
    } else if (this.state.fechaFin === "") {
      this.props.showWarn(FRASE_ERROR_SELECCIONA_FECHA_FIN);
      this.props.unlockScreen();
    } else {
      this.crearEspacioApi();
    }
  };

  crearEspacioApi = () => {
    let url = "";
    url = `${config.appUrl}${MODULO_PROVEEDOR}/${CONTROLLER_RESERVA_HOTEL}/CrearReservaApi`;
    var entity = {
      EspacioHabitacionId: this.state.espacioId,
      ColaboradorId: this.state.colaboradorId,
      fecha_desde: this.state.fechaInicio,
      fecha_hasta: this.state.fechaFin,
      fecha_registro: null,
    };
    http
      .post(url, entity)
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          if (data.created == true) {
            this.props.showSuccess(FRASE_RESERVA_CREADA);
            this.setState(
              {
                fechaInicio: "",
                fechaFin: "",
                colaborador: {},
                colaboradorId: 0,
                visible: false,
                data: [],
              },
              this.resetValues
            );
          } else {
            this.props.showWarn(data.errors);
          }
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

  resetValues = () => {
    this.busquedaRef.current.resetValues();
  };

  consultarEspaciosDisponibles = (event) => {
    event.preventDefault();
    if (this.state.fechaInicio === "") {
      this.props.showWarn(FRASE_ERROR_SELECCIONA_FECHA_INICIO);
      this.props.unlockScreen();
    } else if (this.state.fechaFin === "") {
      this.props.showWarn(FRASE_ERROR_SELECCIONA_FECHA_FIN);
      this.props.unlockScreen();
    } else if (moment(this.state.fechaInicio) > moment(this.state.fechaFin)) {
      this.props.showWarn("Fecha Inicio debe ser menor a Fecha Fin");
      this.props.unlockScreen();
    } else if (
      moment(this.state.fechaInicio) < moment(new Date()).startOf("day")
    ) {
      this.props.showWarn("Fecha de Inicio debe ser mayor a la Fecha Actual");
      this.props.unlockScreen();
    } else {
      this.props.blockScreen();
      let url = "";
      url = "/Proveedor/ReservaHotel/EspciosDisponibles";
      http
        .post(url, {
          fechaInicio: this.state.fechaInicio,
          fechaFin: this.state.fechaFin,
        })
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

  getDiasJornadaCampo = () => {
    this.props.blockScreen();
    let url = "";
    url = "/Proveedor/ReservaHotel/ObtenerDiasJornada";
    http
      .post(url, {})
      .then((response) => {
        let data = response.data;
        console.log("getDiasJornadaCampo", data);
        this.setState({ diasjornada: data });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

  handleChange = (event) => {
    this.setState({
      [event.target.name]: event.target.value,
      data: [],
    });
  };
  handleChangeFechaInicio = (event) => {
    this.setState({
      [event.target.name]: event.target.value,
      data: [],
      fechaFin: "",
    });

    let diasjornada = parseInt(this.state.diasjornada);
    var fecha = new Date(moment(event.target.value));
    console.log("Date", fecha);
    if (diasjornada > 0) {
      this.setState({
        fechaFin: new Date(fecha.setDate(fecha.getDate() + diasjornada)),
      });
    }
  };

  seleccionarColaborador = (colaboradorId) => {
    this.setState({ colaboradorId }, this.crearEspacio);
  };

  seleccionarEspacio = (espacioId) => {
    this.setState({ espacioId, visible: true });
  };

  ocultarForm = () => {
    this.setState({ visible: false, habitacion: {} }, this.resetValues);
  };

  mostrarForm = () => {
    this.setState({ visible: true });
  };
}

const Container = Wrapper(CrearReservaContainer);

ReactDOM.render(
  <Container />,
  document.getElementById("crear_reserva_container")
);
