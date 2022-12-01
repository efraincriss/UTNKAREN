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
    this.props.unlockScreen();
  }


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

            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.GetDocumentos}
              style={{ marginRight: "0.3em" }}
            >
              Documentos
            </button>
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.GetUsuarios}
              style={{ marginRight: "0.3em" }}
            >
              Usuarios Autorizados
            </button>
          </div>
        </div>
      </BlockUi>
    );
  }



  GetDocumentos = () => {
    this.props.blockScreen();
    axios
      .get("/Documentos/Carpeta/ReporteDocumentos", {
        params: {},
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
        this.props.showSuccess("Reporte generado correctamente");
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
        this.props.unlockScreen();
      });
  };

  GetUsuarios = () => {
    this.props.blockScreen();
    axios
      .get("/Documentos/Carpeta/ReporteUsuariosAut", {
        params: {},
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
        this.props.showSuccess("Reporte generado correctamente");
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
