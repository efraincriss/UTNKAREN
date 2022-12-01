import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";

class Reportes extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      contratos: [],
      clientes: [],
      ContratoId: 0,
      ClienteId: 0,
      errors: {},
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
  }

  componentDidMount() {
    this.GetClientes();
  }

  GetClientes = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/Contrato/GetClientes", {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ clientes: items });
        this.state.clientes.unshift({
          label: "Seleccione..",
          dataKey: 0,
          value: 0,
        });
        this.props.unlockScreen();
      })
      .catch((error) => {
        this.props.showWarn(error);
        console.log(error);
      });
  };
  GetContratos = (Id) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/Contrato/GetContratosbyCliente", {
        Id: Id,
      })
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.codigo, dataKey: item.Id, value: item.Id };
        });
        this.setState({ contratos: items });
        this.state.contratos.unshift({
          label: "Seleccione..",
          dataKey: 0,
          value: 0,
        });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
        this.props.unlockScreen();
      });
  };

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
    if (name != null && name == "ClienteId") {
      console.log(name);
      this.GetContratos(value);
    }
  };

  render() {
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={(el) => {
            this.growl = el;
          }}
          baseZIndex={1000}
        />

        <div className="row">
          <div className="col">
            <Field
              name="ClienteId"
              value={this.state.ClienteId}
              label="Cliente"
              options={this.state.clientes}
              type={"select"}
              filter={true}
              onChange={this.onChangeValue}
              error={this.state.errors.ClienteId}
              readOnly={false}
              placeholder="Seleccione.."
              filterPlaceholder="Seleccione.."
            />
          </div>
          <div className="col">
            <Field
              name="ContratoId"
              value={this.state.ContratoId}
              label="Contrato"
              options={this.state.contratos}
              type={"select"}
              filter={true}
              onChange={this.onChangeValue}
              error={this.state.errors.ContratoId}
              readOnly={false}
              placeholder="Seleccione.."
              filterPlaceholder="Seleccione.."
            />
          </div>
        </div>
        <div className="row">
          <div className="col">
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.GetReporteAdicionales}
              style={{ marginRight: "0.3em" }}
            >
              Adicionales
            </button>
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.GetReporteDetalleProyectos}
              style={{ marginRight: "0.3em" }}
            >
              Detallado Proyectos
            </button>
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.GetReporteSeguimientoComercial}
              style={{ marginRight: "0.3em" }}
            >
              Seguimiento Comercial
            </button>
            {/**<button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.GetStackedColumn}
              style={{ marginRight: "0.3em" }}
            >
              Stacked Column
            </button>{" "}*/}
             <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.GetReportePO}
              style={{ marginRight: "0.3em" }}
            >
              Ordenes de Servicio
            </button>
          </div>
        </div>
      </BlockUi>
    );
  }

  ReporteAdicionales = () => {
    return (window.location = `/Proyecto/Contrato/GetReporteAdicionales`);
  };
  GetReporteAdicionales = () => {
    this.props.blockScreen();
    axios
      .get("/Proyecto/Contrato/GetReporteAdicionales", {
        params: {
          ClienteId: this.state.ClienteId,
          ContratoId: this.state.ContratoId,
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
        this.props.showSuccess("Reporte Adicionales generado correctamente");
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
        this.props.unlockScreen();
      });
  };

  ReporteDetalleProyectos = () => {
    return (window.location = `/Proyecto/Contrato/GetReporteDetalleProyectos`);
  };
  GetReporteDetalleProyectos = () => {
    this.props.blockScreen();
    axios
      .get("/Proyecto/Contrato/GetReporteDetalleProyectos", {
        params: {
          ClienteId: this.state.ClienteId,
          ContratoId: this.state.ContratoId,
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
        this.props.showSuccess("Reporte Adicionales generado correctamente");
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
        this.props.unlockScreen();
      });
  };

  ReporteSeguimientoComercial = () => {
    return (window.location = `/Proyecto/Contrato/GetReporteSeguimientoOferta`);
  };
 
  GetStackedColumn = () => {
    this.props.blockScreen();
    axios
      .get("/Proyecto/Contrato/GetReporteChart", {
        params: {
          ClienteId: this.state.ClienteId,
          ContratoId: this.state.ContratoId,
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
        this.props.showSuccess("Reporte Adicionales generado correctamente");
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
        this.props.unlockScreen();
      });
  };

  GetReporteSeguimientoComercial = () => {
    this.props.blockScreen();
    axios
      .get("/Proyecto/Contrato/GetReporteSeguimientoOferta", {
        params: {
          ClienteId: this.state.ClienteId,
          ContratoId: this.state.ContratoId,
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
        this.props.showSuccess("Reporte Adicionales generado correctamente");
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
        this.props.unlockScreen();
      });
  };

  GetReportePO = () => {
    this.props.blockScreen();
    axios
      .get("/Proyecto/Contrato/GetReportePo", {
        params: {
          ClienteId: this.state.ClienteId,
          ContratoId: this.state.ContratoId,
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
        this.props.showSuccess("Reporte POS generado correctamente");
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
        this.props.unlockScreen();
      });
  };

  handleChange(event) {
    if (event.target.files) {
      console.log(event.target.files);
      let files = event.target.files || event.dataTransfer.files;
      if (files.length > 0) {
        let uploadFile = files[0];
        this.setState({
          uploadFile: uploadFile,
        });
      }
    } else {
      this.setState({ [event.target.name]: event.target.value });
    }
  }
}
const Container = wrapForm(Reportes);
ReactDOM.render(<Container />, document.getElementById("content"));
