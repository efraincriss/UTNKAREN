import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { Card } from "primereact-v2/card";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import moment from "moment";
import { MultiSelect } from "primereact-v2/multiselect";
class ConsumoReporteExcel extends React.Component {
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
    this.EnviarFormulario_two = this.EnviarFormulario_two.bind(this);
    this.EnviarFormulario_twoConsolidado = this.EnviarFormulario_twoConsolidado.bind(this);
    this.EnviarFormulario_tree = this.EnviarFormulario_tree.bind(this);
    this.ObtenerDatos = this.ObtenerDatos.bind(this);
    this.isValid = this.isValid.bind(this);
    this.isValidTwo = this.isValidTwo.bind(this);
    this.isValidTwoConsolidado = this.isValidTwoConsolidado.bind(this);
    this.isValidTree = this.isValidTree.bind(this);
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

  isValidTwoConsolidado() {
    const errorsCC = {};
   
    if (this.state.fechaInicioCC == null) {
      errorsCC.fechaInicioCC = "Campo Requerido";
    }
    if (this.state.fechaFinCC == null) {
      errorsCC.fechaFinCC = "Campo Requerido";
    }
    if (
      this.state.fechaFinCC != null &&
      this.state.fechaInicioCC != null &&
      this.state.fechaFinCC < this.state.fechaInicioCC
    ) {
      errorsCC.fechaFinCC = "Fecha Hasta no puede se menor a Fecha Desde";
    }

    this.setState({ errorsCC });
    return Object.keys(errorsCC).length === 0;
  }
  
  isValidTree() {
    const errorsD = {};
   /* if (this.state.proveedorIdD == 0) {
      errorsD.proveedorIdD = "Campo Requerido";
    }*/
    if (this.state.fechaInicioD == null) {
      errorsD.fechaInicioD = "Campo Requerido";
    }
    if (this.state.fechaFinD == null) {
      errorsD.fechaFinD = "Campo Requerido";
    }
    if (
      this.state.fechaFinD != null &&
      this.state.fechaInicioD != null &&
      this.state.fechaFinD < this.state.fechaInicioD
    ) {
      errorsD.fechaFinD = "Fecha Hasta no puede se menor a Fecha Desde";
    }

    this.setState({ errorsD });
    return Object.keys(errorsD).length === 0;
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
        this.setState({ Zonas: items});
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
                      {/*<Field
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
                      />*/}
                      <MultiSelect
                        value={this.state.proveedorId}
                        options={this.state.Proveedores}
                        onChange={(e) =>
                          this.setState({ proveedorId: e.value })
                        }
                        style={{ width: "100%" }}
                        defaultLabel="Seleccione.."
                        filter={true}
                        placeholder="Seleccione"
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
                      {/*<Field
                        name="proveedorIdM"
                        required
                        value={this.state.proveedorIdM}
                        label="Proveedor"
                        options={this.state.ProveedoresM}
                        type={"select"}
                        filter={true}
                        onChange={this.onChangeValue}
                        error={this.state.errorsM.proveedorIdM}
                        readOnly={false}
                        placeholder="Seleccione.."
                        filterPlaceholder="Seleccione.."
                      />*/}
                      <MultiSelect
                        value={this.state.proveedorIdM}
                        options={this.state.ProveedoresM}
                        onChange={(e) =>
                          this.setState({ proveedorIdM: e.value })
                        }
                        style={{ width: "100%" }}
                        defaultLabel="Seleccione.."
                        filter={true}
                        placeholder="Seleccione"
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

          <div className="row">
         
            <div className="col-6">
              <Card title="Reporte de Dobles Consumos">
                <form onSubmit={this.EnviarFormulario_tree}>
                  <div className="row">
                    <div className="col">
                      <Field
                        name="fechaInicioD"
                        label="Fecha Desde"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaInicioD}
                        onChange={this.handleChange}
                        error={this.state.errorsD.fechaInicioD}
                      />
                      <Field
                        name="fechaFinD"
                        label="Fecha Hasta"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaFinD}
                        onChange={this.handleChange}
                        error={this.state.errorsD.fechaFinD}
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
            <Card title="Reporte Consolidado">
                <form onSubmit={this.EnviarFormulario_twoConsolidado}>
                  <div className="row">
                    <div className="col">
                      <Field
                        name="fechaInicioCC"
                        label="Fecha Desde"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaInicioCC}
                        onChange={this.handleChange}
                        error={this.state.errorsCC.fechaInicioCC}
                      />
                      <Field
                        name="fechaFinCC"
                        label="Fecha Hasta"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaFinCC}
                        onChange={this.handleChange}
                        error={this.state.errorsCC.fechaFinCC}
                      />
                       <MultiSelect
                        value={this.state.zonaId}
                        options={this.state.Zonas}
                        onChange={(e) =>
                          this.setState({ zonaId: e.value })
                        }
                        style={{ width: "100%" }}
                        defaultLabel="Seleccione Zona.."
                        filter={true}
                        placeholder="Seleccione Zona"
                      />
                    </div>
                  </div>
                  <br></br>
                  <button type="submit" className="btn btn-outline-primary">
                    Generar Consolidado
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
        .get("/Proveedor/ReporteConsumo/ReporteConsumoDiario", {
          params: {
            fecha: moment(this.state.fecha).format("MM-DD-YYYY"),
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

  EnviarFormulario_two(event) {
    event.preventDefault();

    if (!this.isValidTwo()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      this.props.blockScreen();
      axios
        .get("/Proveedor/ReporteConsumo/ReporteConsumoMensual", {
          params: {
          
            fechaInicio: moment(this.state.fechaInicio).format("MM-DD-YYYY"),
            fechaFin: moment(this.state.fechaFin).format("MM-DD-YYYY"),
            Ids: this.state.proveedorIdM.toString(),
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

   EnviarFormulario_twoConsolidado(event) {
    event.preventDefault();

    if (!this.isValidTwoConsolidado()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      this.props.blockScreen();
      axios
        .get("/Proveedor/ReporteConsumo/ReporteConsumoMensualConsolidado", {
          params: {
          
            fechaInicio: moment(this.state.fechaInicioCC).format("MM-DD-YYYY"),
            fechaFin: moment(this.state.fechaFinCC).format("MM-DD-YYYY"),
            zonaIds:this.state.zonaId.length==0?"":
             this.state.zonaId.toString(),
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
  EnviarFormulario_tree(event) {
    event.preventDefault();

    if (!this.isValidTree()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      this.props.blockScreen();
      axios
        .get("/Proveedor/ReporteConsumo/ReporteConsumoDuplicado", {
          params: {
          
            fechaInicio: moment(this.state.fechaInicioD).format("MM-DD-YYYY"),
            fechaFin: moment(this.state.fechaFinD).format("MM-DD-YYYY"),
            //Ids: this.state.proveedorIdD.toString(),
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
const Container = wrapForm(ConsumoReporteExcel);
ReactDOM.render(<Container />, document.getElementById("content"));
