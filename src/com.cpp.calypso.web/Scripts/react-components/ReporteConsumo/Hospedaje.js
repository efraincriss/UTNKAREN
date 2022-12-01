import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { Card } from "primereact-v2/card";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import moment from "moment";
import { MultiSelect } from "primereact-v2/multiselect";
import { FRASE_ERROR_DIGITOSNOMBRES } from "../Base/Strings";
class Hospedaje extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      Proveedores: [],
      ProveedoresM: [],
      ProveedoresS: [],
      ProveedoresF: [],
      errors: {},
      errorsM: {},
      errorsS: {},
      errorsF: {},
      errorsCC: {},
      zonaId: [],
      Zonas: [],
      proveedorId: [],
      proveedorIdM: [],
      proveedorIdS: [],
      proveedorIdF: [],
      fecha: null,
      fechaInicio: null,
      fechaFin: null,
      fechaInicioS: null,
      fechaFinS: null,
      fechaInicioF: null,
      fechaFinF: null,

      fechaInicioCC: null,
      fechaFinCC: null,
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.EnviarFormulario_two = this.EnviarFormulario_two.bind(this);
    this.EnviarFormulario_tres = this.EnviarFormulario_tres.bind(this);
    this.EnviarFormulario_Cuatro = this.EnviarFormulario_Cuatro.bind(this);

    this.EnviarFormulario_CC = this.EnviarFormulario_CC.bind(this);
    this.ObtenerDatos = this.ObtenerDatos.bind(this);
    this.isValid = this.isValid.bind(this);
    this.isValidTwo = this.isValidTwo.bind(this);
    this.isValidTree = this.isValidTree.bind(this);
    this.isValidCC = this.isValidCC.bind(this);
  }

  componentDidMount() {
    this.ObtenerDatos();
  }
  isValidTree() {
    const errorsS = {};
    if (this.state.proveedorIdS == 0) {
      errorsS.proveedorIdS = "Campo Requerido";
    }
    if (this.state.fechaInicioS == null) {
      errorsS.fechaInicioS = "Campo Requerido";
    }
    if (this.state.fechaFinS == null) {
      errorsS.fechaFinS = "Campo Requerido";
    }
    if (
      this.state.fechaFinS != null &&
      this.state.fechaInicioS != null &&
      this.state.fechaFinS < this.state.fechaInicioS
    ) {
      errorsS.fechaFinS = "Fecha Hasta no puede se menor a Fecha Desde";
    }

    this.setState({ errorsS });
    return Object.keys(errorsS).length === 0;
  }

  isValidCC() {
    const errorsCC = {};
 
    if (this.state.fechaInicioCC == null) {
      errorsCC.fechaInicioS = "Campo Requerido";
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

  isValidCuatro() {
    const errorsF = {};
    if (this.state.proveedorIdF == 0) {
      errorsF.proveedorIdF = "Campo Requerido";
    }
    if (this.state.fechaInicioF == null) {
      errorsF.fechaInicioF = "Campo Requerido";
    }
    if (this.state.fechaFinF == null) {
      errorsF.fechaFinF = "Campo Requerido";
    }
    if (
      this.state.fechaFinF != null &&
      this.state.fechaInicioF != null &&
      this.state.fechaFinF < this.state.fechaInicioF
    ) {
      errorsF.fechaFinF = "Fecha Hasta no puede se menor a Fecha Desde";
    }

    this.setState({ errorsF });
    return Object.keys(errorsF).length === 0;
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
      .post("/Proveedor/ReporteConsumo/GetProveedoresHospedaje", {})
      .then((response) => {
        console.log(response.data.result);
        var items = response.data.result.map((item) => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Proveedores: items, ProveedoresM: items, ProveedoresS: items, ProveedoresF: items });
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
              <Card title="Reporte Serge(Ingresos y Salidas)">
                <form onSubmit={this.EnviarFormulario_tres}>
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
                        value={this.state.proveedorIdS}
                        options={this.state.ProveedoresS}
                        onChange={(e) =>
                          this.setState({ proveedorIdS: e.value })
                        }
                        style={{ width: "100%" }}
                        defaultLabel="Seleccione.."
                        filter={true}
                        placeholder="Seleccione"
                      />
                      <Field
                        name="fechaInicioS"
                        label="Fecha Desde"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaInicioS}
                        onChange={this.handleChange}
                        error={this.state.errorsS.fechaInicioS}
                      />
                      <Field
                        name="fechaFinS"
                        label="Fecha Hasta"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaFinS}
                        onChange={this.handleChange}
                        error={this.state.errorsS.fechaFinS}
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
              <Card title="Reporte Reservas no Finalizadas">
                <form onSubmit={this.EnviarFormulario_Cuatro}>
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
                        value={this.state.proveedorIdF}
                        options={this.state.ProveedoresF}
                        onChange={(e) =>
                          this.setState({ proveedorIdF: e.value })
                        }
                        style={{ width: "100%" }}
                        defaultLabel="Seleccione.."
                        filter={true}
                        placeholder="Seleccione"
                      />
                      <Field
                        name="fechaInicioF"
                        label="Fecha Desde"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaInicioF}
                        onChange={this.handleChange}
                        error={this.state.errorsF.fechaInicioF}
                      />
                      <Field
                        name="fechaFinF"
                        label="Fecha Hasta"
                        required
                        type="date"
                        edit={true}
                        readOnly={false}
                        value={this.state.fechaFinF}
                        onChange={this.handleChange}
                        error={this.state.errorsF.fechaFinF}
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
              <Card title="Reporte Consolidado">
                <form onSubmit={this.EnviarFormulario_CC}>
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
  EnviarFormulario_tres(event) {
    event.preventDefault();
    console.log("Ingreso");
    /* if (!this.isValidTree()) {
       abp.notify.error(
         "No ha ingresado los campos obligatorios  o existen datos inválidos.",
         "Validación"
       );
       return;
     } else {*/
    this.props.blockScreen();
    axios
      .get("/Proveedor/ReporteConsumo/ReporteHospedajeSerge", {
        params: {

          fechaInicio: moment(this.state.fechaInicioS).format("MM-DD-YYYY"),
          fechaFin: moment(this.state.fechaFinS).format("MM-DD-YYYY"),
          Ids: this.state.proveedorIdS.toString(),
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
    //}
  }
  EnviarFormulario_Cuatro(event) {
    event.preventDefault();
    console.log("Ingreso");
    /* if (!this.isValidTree()) {
       abp.notify.error(
         "No ha ingresado los campos obligatorios  o existen datos inválidos.",
         "Validación"
       );
       return;
     } else {*/
    this.props.blockScreen();
    axios
      .get("/Proveedor/ReporteConsumo/ReporteHospedajeIniciados", {
        params: {

          fechaInicio: moment(this.state.fechaInicioF).format("MM-DD-YYYY"),
          fechaFin: moment(this.state.fechaFinF).format("MM-DD-YYYY"),
          Ids: this.state.proveedorIdF.toString(),
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
    //}
  }

  EnviarFormulario_CC(event) {
    event.preventDefault();
    console.log("Ingreso");
    if (!this.isValidCC()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else{
      this.props.blockScreen();
    axios
      .get("/Proveedor/ReporteConsumo/ReporteHospedajeConsolidado", {
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
        .get("/Proveedor/ReporteConsumo/ReporteHospedaje", {
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
        .get("/Proveedor/ReporteConsumo/ReporteHospedajeMensual", {
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


  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }
}
const Container = wrapForm(Hospedaje);
ReactDOM.render(<Container />, document.getElementById("content"));
