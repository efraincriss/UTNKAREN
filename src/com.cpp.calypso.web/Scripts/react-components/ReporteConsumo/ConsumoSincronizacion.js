import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { Card } from "primereact-v2/card";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import moment from "moment";
import { MultiSelect } from "primereact-v2/multiselect";
class ConsumoSincronizacion extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      Proveedores: [],
      ProveedoresM: [],
      ProveedoresD: [],

      errors: {},


      errorsM: {},
      errorsD: {},

      errorsCC: {},

      proveedorId: [],
      zonaId: [],
      Zonas: [],
      proveedorIdM: [],
      proveedorIdD: [],
      fecha: null,
      fechaInicio: null,
      fechaFin: null,

      fechaInicioD: null,
      fechaFinD: null,
      fechaInicioCC: null,
      fechaFinCC: null,
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.ObtenerDatos = this.ObtenerDatos.bind(this);
    this.isValid = this.isValid.bind(this);

  }

  componentDidMount() {
    this.ObtenerDatos();
  }

  isValid() {
    const errors = {};

    if (this.state.fechaInicio == null) {
      errors.fechaInicio = "Campo Requerido";
      abp.notify.error(
        "Campo Fecha Inicio Requerido",
        "Validación"
      );


    }
    if (this.state.fechaFin == null) {
      errors.fechaFin = "Campo Requerido";
      abp.notify.error(
        "Campo Fecha Fin Requerido",
        "Validación"
      );

    }

    if (
      this.state.fechaFin != null &&
      this.state.fechaInicio != null &&
      this.state.fechaFin < this.state.fechaInicio
    ) {
      abp.notify.error(
        "Fecha Hasta no puede se menor a Fecha Desde",
        "Validación"
      );
      errors.fechaFin = "Fecha Hasta no puede se menor a Fecha Desde";
    }


    return Object.keys(errors).length === 0;
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
        this.setState({ Proveedores: items, ProveedoresM: items, ProveedoresD: items });
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

    axios
      .post("/Proveedor/ReporteConsumo/GetZonas", {})
      .then((response) => {
        console.log(response.data.result);
        var items = response.data.result.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Zonas: items });
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
              <Card subTitle="Reporte Sincronizaciones">
                <form onSubmit={this.EnviarFormulario}>
                  <div className="row">
                    <div className="col">

                      <MultiSelect
                        value={this.state.proveedorId}
                        options={this.state.Proveedores}
                        onChange={(e) =>
                          this.setState({ proveedorId: e.value })
                        }
                        style={{ width: "100%" }}
                        defaultLabel="Seleccione Proveedor.."
                        filter={true}
                        placeholder="Seleccione Proveedor"
                      />

                      <div className="form-group">
                        <label>Fecha y Hora Inicial</label>
                        <input
                          type="datetime-local"
                          id="no-filter"
                          name="fechaInicio"
                          className="form-control"
                          onChange={this.handleChange}
                          required
                          value={this.state.fechaInicio}
                        />
                      </div>
                      <div className="form-group">
                        <label>Fecha y Hora Final</label>
                        <input
                          type="datetime-local"
                          id="no-filter"
                          name="fechaFin"
                          className="form-control"
                          onChange={this.handleChange}
                          required
                          value={this.state.fechaFin}
                        />
                      </div>


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
      this.props.blockScreen();
      console.log(this.state.proveedorId.toString());
      axios
        .get("/Seguridad/Usuario/ReporteSincronizaciones", {
          params: {
            fechaInicio: this.state.fechaInicio,
            fechaFin:this.state.fechaFin,
            Ids: this.state.proveedorId.toString(),
          },
          responseType: "arraybuffer",
        })
        .then((response) => {
          var nombre = response.headers["content-disposition"].split("=");

          const url = window.URL.createObjectURL(
            new Blob([response.data], {
              type:
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            })
          );
          const link = document.createElement("a");
          link.href = url;
          link.setAttribute("download", nombre[1]);
          document.body.appendChild(link);
          link.click();
          this.props.showSuccess("Correcto");
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.showWarn("Ocurrió un Error al descargar el Excel");
          this.props.unlockScreen();
        });
    }
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }
}
const Container = wrapForm(ConsumoSincronizacion);
ReactDOM.render(<Container />, document.getElementById("content"));
