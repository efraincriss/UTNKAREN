import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import CurrencyFormat from "react-currency-format";
import Field from "../Base/Field-v2";
import moment from "moment";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v3.3/dialog";
import { Growl } from "primereact-v3.3/growl";
import { Card } from "primereact-v3.3/card";
import UploadPdfForm from "./UploadPdfForm";
class HistoricosCurva extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      errors: {},
      contratos: [],
      proyectos: [],
      ContratoId: 0,
      ProyectoId: 0,
      FechaInicio: null,
      FechaFin: null,
      disableFechaFin: false,
      //Subida de Archivo

      upload: false,
      uploadFile: "",
    };
    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.onChangeValueContrato = this.onChangeValueContrato.bind(this);
    this.onChangeValueProyecto = this.onChangeValueProyecto.bind(this);
    this.mostrarUpload = this.mostrarUpload.bind(this);
    this.ocultarUpload = this.ocultarUpload.bind(this);
  }

  componentDidMount() {
    this.GetListContratos();
  }
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
  ocultarUpload = () => {
    this.setState({ upload: false, uploadFile: "" });
  };

  GetListContratos = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/RdoCabecera/ApiContratos", {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.Codigo, dataKey: item.Id, value: item.Id };
        });
        this.setState({ contratos: items }, this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetListProyectos = (Id) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/RdoCabecera/ApiProyectos", {
        Id: Id,
      })
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return {
            label: item.codigo + " - " + item.nombre_proyecto,
            dataKey: item.Id,
            value: item.Id,
          };
        });
        this.setState({ proyectos: items }, this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetFechaMin = (Id) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/RdoCabecera/APIFechaMin", {
        Id: Id,
      })
      .then((response) => {
        console.log(response.data);

        if (response.data != null && response.data !== "NO_FECHA") {
          this.setState(
            { FechaFin: new Date(response.data) },
            this.props.unlockScreen()
          );
        } else {
          this.props.unlockScreen();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  handleChangeUploadFile = (event) => {
    if (event.files) {
      if (event.files.length > 0) {
        let uploadFile = event.files[0];
        this.setState(
          {
            uploadFile: uploadFile,
          },
          this.handleSubmitUploadFile
        );
      } else {
      this.props.showWarn('Debe Seleccionar un Archivo')
      }
    } else {
      this.props.showWarn('Debe Seleccionar un Archivo')
    }
  };

  onChangeValueProyecto = (name, value) => {
    this.setState({
      [name]: value,
    });
    this.GetFechaMin(value);
  };
  onChangeValueContrato = (name, value) => {
    this.setState({
      [name]: value,
    });
    this.GetListProyectos(value);
  };
  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };

  EnviarFormularioDetalle = () => {
    event.preventDefault();
    this.props.blockScreen();
    axios
      .get("/Proyecto/RdoCabecera/ApiExcel", {
        params: {
          ProyectoId: this.state.ProyectoId,
          FechaInicio: this.state.FechaInicio,
          FechaFin: this.state.FechaFin,
        },
        responseType: "arraybuffer",
      })
      .then((response) => {
        console.log(response.headers["content-disposition"]);
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
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  handleSubmitUploadFile = () => {
   this.props.blockScreen();
    console.log('bloqueo')
    const formData = new FormData();
    formData.append("UploadedFile", this.state.uploadFile);

    const config = {
      headers: {
        "content-type": "multipart/form-data",
      },
    };

    axios
      .post("/Proyecto/RdoCabecera/Actualizar/", formData, config)
      .then((response) => {
        console.log('respuesta')
        console.log(response);
        if (response.data == "OK") {
          this.setState({ upload: false });
          this.props.showSuccess("Fechas Cargadas Correctamente");
          this.props.unlockScreen();
        } else {
          this.setState({ upload: false });
          this.props.showWarn(response.data);
          this.props.unlockScreen();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

  mostrarUpload = () => {
    this.setState({ upload: true });
  };

  render() {
    return (
      <div>
        <div className="row">
          <div className="col" align="right">
            <button
              className="btn btn-outline-primary"
              style={{ marginLeft: "0.3em" }}
              onClick={() => this.mostrarUpload()}
            >
              <i className="fa fa-cloud-upload"> Subir Excel</i>
            </button>
          </div>
        </div>
        <div>
          <form onSubmit={this.EnviarFormularioDetalle}>
            <div className="row">
              <div className="col">
                <Field
                  name="ContratoId"
                  required
                  value={this.state.ContratoId}
                  label="Contrato"
                  options={this.state.contratos}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValueContrato}
                  error={this.state.errors.ContratoId}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
              <div className="col">
                <Field
                  name="ProyectoId"
                  required
                  value={this.state.ProyectoId}
                  label="Proyecto"
                  options={this.state.proyectos}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValueProyecto}
                  error={this.state.errors.ProyectoId}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
            </div>
            <div className="row">
              <div className="col">
                <Field
                  name="FechaInicio"
                  label="Fecha Inicial"
                  required
                  type="date"
                  edit={true}
                  readOnly={false}
                  value={this.state.FechaInicio}
                  onChange={this.handleChange}
                  error={this.state.errors.FechaInicio}
                />
              </div>
              <div className="col">
                <Field
                  name="FechaFin"
                  label="Fecha Final"
                  required
                  type="date"
                  edit={true}
                  readOnly={false}
                  value={this.state.FechaFin}
                  onChange={this.handleChange}
                  error={this.state.errors.FechaFin}
                />
              </div>
            </div>
            <button type="submit" className="btn btn-outline-primary">
              Descargar Plantilla
            </button>
            &nbsp;
          </form>
        </div>
        <Dialog
          header="Subir Historicos Curva"
          visible={this.state.upload}
          width="500px"
          modal={true}
          onHide={this.ocultarUpload}
        >
          <UploadPdfForm
            handleChange={this.handleChangeUploadFile}
            label="Anexo 10"
          />
        </Dialog>
      </div>
    );
  }
}
const Container = wrapForm(HistoricosCurva);
ReactDOM.render(<Container />, document.getElementById("content"));
