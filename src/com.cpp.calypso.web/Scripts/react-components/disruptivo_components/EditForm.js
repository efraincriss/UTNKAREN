import React from "react";
import axios from "axios";
import moment from "moment";
import TimePicker from "rc-time-picker";

const showSecond = true;
const str = showSecond ? "HH:mm:ss" : "HH:mm";

export default class EditForm extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      Id: props.Id,
      tipo_improductividad: props.tipo_improductividad,
      tipo_recurso: props.tipo_recurso,
      hora_inicio: props.hora_inicio,
      hora_fin: props.hora_fin,
      horas: props.horas,
      observaciones: props.observaciones,
      numero_recursos: props.numero_horas,
      numero_horas_hombres: props.numero_horas_hombres,
      fecha_inicio: "",
      fecha_fin: "",
      porcentaje_disruptivo: props.porcentaje_disruptivo,
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeHoraInicio = this.onChangeHoraInicio.bind(this);
    this.onChangeHoraFin = this.onChangeHoraFin.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.syncHoras = this.syncHoras.bind(this);
    this.getRecursosSelect = this.getRecursosSelect.bind(this);
  }

  componentWillReceiveProps() {
    this.setState({
      Id: this.props.Id,
      tipo_improductividad: this.props.tipo_improductividad,
      hora_inicio: this.props.hora_inicio,
      hora_fin: this.props.hora_fin,
      horas: this.props.horas,
      observaciones: this.props.observaciones,
      numero_recursos: this.props.numero_recursos,
      numero_horas_hombres: this.props.numero_horas_hombres,
      fecha_inicio: this.props.fecha_inicio,
      fecha_fin: this.props.fecha_fin,
      porcentaje_disruptivo:this.props.porcentaje_disruptivo
    });
  }

  render() {
    let dias_real =
      this.state.fecha_fin != null &&
      this.state.fecha_fin !== "" &&
      this.state.fecha_inicio != null &&
      this.state.fecha_inicio !== ""
        ? ((new Date(this.state.fecha_fin).getDate() -
            new Date(this.state.fecha_inicio).getDate())+1) *
          (this.state.porcentaje_disruptivo / 100)
        : "";
    console.log("dias_real", dias_real);
    return (
      <div>
        <form onSubmit={this.handleSubmit}>
          <div className="form-group">
            <label htmlFor="label">Improductividad</label>

            <select
              required
              onChange={this.handleChange}
              className="form-control"
              name="tipo_improductividad"
              value={this.state.tipo_improductividad}
            >
              <option value="">
                --- Selecciona un tipo de improductividad ---
              </option>
              {this.getFormSelect()}
            </select>
          </div>

          <div className="form-group">
            <label htmlFor="label">Tipo Recurso</label>

            <select
              value={this.state.tipo_recurso}
              required
              onChange={this.handleChange}
              className="form-control"
              name="tipo_recurso"
            >
              <option value="">--- Selecciona un tipo de recurso ---</option>
              {this.getRecursosSelect()}
            </select>
          </div>
        

          <div className="row">
            <div className="col">
              <div className="form-group">
                <label htmlFor="fecha_inicio">Fecha Inicio</label>
                <input
                  type="date"
                  id="fecha_inicio"
                  className="form-control"
                  value={this.state.fecha_inicio}
                  onChange={this.handleChange}
                  name="fecha_inicio"
                  required
                />
              </div>
            </div>
            <div className="col">
              <div className="form-group">
                <label htmlFor="horas">Fecha Fin</label>
                <input
                  type="date"
                  id="fecha_fin"
                  className="form-control"
                  value={this.state.fecha_fin}
                  onChange={this.handleChange}
                  name="fecha_fin"
                />
              </div>
            </div>
          </div>
          <div className="row">
            <div className="col-4">
              <div className="form-group">
                <label htmlFor="observacion">Porcentaje</label>
                <input
                  type="number"
                  id="porcentaje_disruptivo"
                  min="1"
                  max="100"
                  className="form-control"
                  value={this.state.porcentaje_disruptivo}
                  onChange={this.handleChange}
                  name="porcentaje_disruptivo"
                />
              </div>
            </div>
            <div className="col-2"></div>
            <div className="col-6">
              <div className="form-group">
                <label htmlFor="observacion">Dias Real</label>
                <input
                  type="number"
                  readOnly
                  className="form-control"
                  value={dias_real}
                  name="dias_real"
                />
              </div>
            </div>
          </div>

         
          <div className="form-group">
            <label htmlFor="observacion">Observaciones</label>
            <input
              type="text"
              id="observacion"
              className="form-control"
              value={this.state.observaciones}
              onChange={this.handleChange}
              name="observaciones"
            />
          </div>
          <div className="row">
            <div className="col-xd-1">
              <button type="submit" className="btn btn-outline-primary">
                Guardar
              </button>
            </div>
            <div className="col-xd-1">
              <button
                type="button"
                onClick={this.props.onHide}
                className="btn btn-outline-primary"
              >
                Cancelar
              </button>
            </div>
          </div>
        </form>
      </div>
    );
  }

  handleSubmit(event) {
    event.preventDefault();
    var fecha_i = this.state.fecha_inicio;
    var fecha_f = this.state.fecha_fin;
    if (this.state.fecha_inicio === "Invalid date") {
      fecha_i = "";
    }
    if (this.state.fecha_fin === "Invalid date") {
      fecha_f = "";
    }
    axios
      .post("/proyecto/ObraDisruptivo/CreateApi/", {
        Id: this.state.Id,
        TipoRecursoId: this.state.tipo_recurso,
        ProyectoId: document.getElementById("AvanceId").className,
        tipo_improductividad: this.state.tipo_improductividad,
        hora_inicio: this.state.hora_inicio.format(str),
        hora_fin: this.state.hora_fin.format(str),
        numero_horas: this.state.horas,
        numero_recursos: this.state.numero_recursos,
        numero_horas_hombres: this.state.horas * this.state.numero_recursos,
        observaciones: this.state.observaciones,
        fecha_inicio: fecha_i,
        fecha_fin: fecha_f,
        vigente: true,
        porcentaje_disruptivo:this.state.porcentaje_disruptivo
      })
      .then((response) => {
        if (response.data === "Ok") {
          this.props.updateTable();
          this.props.onHide();
          this.props.showSuccess("Disruptivo Ingresado");
        } else {
          this.props.showWarn("Revise los datos ingresados");
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("Algo salio mal");
      });
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  onChangeHoraFin(value) {
    this.setState({ hora_fin: value }, this.syncHoras);
  }

  onChangeHoraInicio(value) {
    this.setState({ hora_inicio: value }, this.syncHoras);
  }

  syncHoras() {
    var startTime = moment(this.state.hora_inicio, "HH:mm:ss");
    var endTime = moment(this.state.hora_fin, "HH:mm:ss");
    var duration = moment.duration(endTime.diff(startTime));
    var hours = parseInt(duration.asHours());
    this.setState({ horas: hours });
  }

  getFormSelect() {
    return this.props.catalogos.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getRecursosSelect() {
    return this.props.recursos.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }
}
