import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { Card } from "primereact-v2/card";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import moment from "moment";
class ConsumoReporte extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      Proveedores: [],
      errors: {},
      errorsM: {},
      proveedorId: 0,
      proveedorIdM: 0,
      fecha: null,
      fechaInicio: null,
      fechaFin: null,
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.EnviarFormulario_two = this.EnviarFormulario_two.bind(this);
    this.ObtenerDatos = this.ObtenerDatos.bind(this);
    this.isValid = this.isValid.bind(this);
    this.isValidTwo = this.isValidTwo.bind(this);
  }

  componentDidMount() {
    this.ObtenerDatos();
  }

  isValid() {
    const errors = {};
    if (this.state.proveedorId == 0) {
      errors.proveedorId = "Campo Requerido";
    }
    if (this.state.fecha == null) {
      errors.fecha = "Campo Requerido";
    }
    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }

  isValidTwo() {
    const errorsM = {};
    if (this.state.proveedorIdM == 0) {
      errorsM.proveedorIdM = "Campo Requerido";
    }
    if (this.state.fechaInicio == null) {
      errorsM.fechaInicio = "Campo Requerido";
    }
    if (this.state.fechaFin == null) {
      errorsM.fechaFin = "Campo Requerido";
    }
    if (
      this.state.fechaFin != null &&
      this.state.fechaInicio != null &&
      this.state.fechaFin < this.state.fechaInicio
    ) {
      errorsM.fechaFin = "Fecha Hasta no puede se menor a Fecha Desde";
    }

    this.setState({ errorsM });
    return Object.keys(errorsM).length === 0;
  }

  VaciarCampos() {
    this.setState({
      proveedorId: 0,
      proveedorIdM: 0,
      fecha: null,
      fechaInicio: null,
      fechaFin: null,
      errors: {},
      errorsM: {},
    });
  }

  ObtenerDatos() {
    axios
      .post("/Proveedor/ReporteConsumo/GetProveedores", {})
      .then((response) => {
        console.log(response.data.result);
        var items = response.data.result.map((item) => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Proveedores: items });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error(
          "Ocurrió un Error al Consultar Proveedores de Alimentación",
          "Consulta"
        );
        this.props.unlockScreen();
      });
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };

  render() {
    return (
      <div>
        <div className="content-section implementation">
          <div className="row">
            <div className="col-6">
              <Card title="Reporte Diario">
                <form onSubmit={this.EnviarFormulario}>
                  <div className="row">
                    <div className="col">
                      <Field
                        name="proveedorId"
                        required
                        value={this.state.proveedorId}
                        label="Proveedor"
                        options={this.state.Proveedores}
                        type={"select"}
                        filter={true}
                        onChange={this.onChangeValue}
                        error={this.state.errors.proveedorId}
                        readOnly={false}
                        placeholder="Seleccione.."
                        filterPlaceholder="Seleccione.."
                      />
                      <Field
                        name="fecha"
                        label="Fecha"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fecha}
                        onChange={this.handleChange}
                        error={this.state.errors.fecha}
                      />
                    </div>
                  </div>
                  <button type="submit" className="btn btn-outline-primary">
                    Generar
                  </button>
                  &nbsp;
                </form>
              </Card>
            </div>
            <div className="col-6">
              <Card title="Reporte Mensual">
                <form onSubmit={this.EnviarFormulario_two}>
                  <div className="row">
                    <div className="col">
                      <Field
                        name="proveedorIdM"
                        required
                        value={this.state.proveedorIdM}
                        label="Proveedor"
                        options={this.state.Proveedores}
                        type={"select"}
                        filter={true}
                        onChange={this.onChangeValue}
                        error={this.state.errorsM.proveedorIdM}
                        readOnly={false}
                        placeholder="Seleccione.."
                        filterPlaceholder="Seleccione.."
                      />
                      <Field
                        name="fechaInicio"
                        label="Fecha Desde"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaInicio}
                        onChange={this.handleChange}
                        error={this.state.errorsM.fechaInicio}
                      />
                      <Field
                        name="fechaFin"
                        label="Fecha Hasta"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaFin}
                        onChange={this.handleChange}
                        error={this.state.errorsM.fechaFin}
                      />
                    </div>
                  </div>
                  <button type="submit" className="btn btn-outline-primary">
                    Generar
                  </button>
                  &nbsp;
                </form>
              </Card>
            </div>
          </div>
        </div>
      </div>
    );
  }

  EnviarFormulario(event) {
    event.preventDefault();

    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      // window.location.href ="http://45.35.14.17:8181/api/ReporteDiarioAlimentacion?proveedorId=" +this.state.proveedorId +"&fecha=" + moment(this.state.fecha).format("DD-MM-YYYY");
      window.location.href =
        "http://10.26.102.43:8181/api/ReporteDiarioAlimentacion?proveedorId=" +
        this.state.proveedorId +
        "&fecha=" +
        moment(this.state.fecha).format("DD-MM-YYYY");

    }
  }

  EnviarFormulario_two(event) {
    event.preventDefault();

    if (!this.isValidTwo()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      window.location.href = "http://10.26.102.43:8181/api/ReporteMensualAlimentacion?proveedorId=" + this.state.proveedorIdM + "&fechaInicio=" + moment(this.state.fechaInicio).format('DD-MM-YYYY') + "&fechaFin=" + moment(this.state.fechaFin).format('DD-MM-YYYY');

      /*window.location.href =
        "http://45.35.14.17:8181/api/ReporteMensualAlimentacion?proveedorId=" +
        this.state.proveedorIdM +
        "&fechaInicio=" +
        moment(this.state.fechaInicio).format("DD-MM-YYYY") +
        "&fechaFin=" +
        moment(this.state.fechaFin).format("DD-MM-YYYY");*/
    }
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }
}
const Container = wrapForm(ConsumoReporte);
ReactDOM.render(<Container />, document.getElementById("content"));
