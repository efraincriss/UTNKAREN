import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";
import UploadPdfForm from "./UploadPdfForm";
import { create } from "domain";

class UploadAvance extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data: [],
      visible: false,
      errors: {},
      uploadFile: "",
      descripcion: "",
      editable: true,
      imagen: "",
      Id: 0,
      action: "create",
      fecha_emision: null,
      fecha_desde: null,
      fecha_hasta: null,
      blocking:false
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.mostrarForm = this.mostrarForm.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind();
    this.isValid = this.isValid.bind();
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
  }

  componentDidMount() {
    this.props.unlockScreen();
  }

  isValid = () => {
    const errors = {};
    if (this.state.editable) {
      if (this.state.uploadFile == null || this.state.uploadFile === "") {
        errors.uploadFile = "Campo Requerido";
      }
    }
    /*  if (this.state.descripción == '') {
            errors.descripción = "Campo Requerido";
        }*/

    if (this.state.fecha_emision == null) {
      errors.fecha_emision = "Campo Requerido";
    }
    if (this.state.fecha_desde == null) {
      errors.fecha_desde = "Campo Requerido";
    }
    if (this.state.fecha_hasta == null) {
      errors.fecha_hasta = "Campo Requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };

  Eliminar = Id => {
    console.log(Id);
    axios
      .post("/Proyecto/AvanceObra/DeleteArchivo", {
        Id: Id
      })
      .then(response => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente");
          this.ObtenerArchivos();
        } else if (response.data == "NOPUEDE") {
          this.props.showWarn("No se puedo Eliminar");
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurrió un Error");
      });
  };

  mostrarForm = row => {
    if (row != null && row.Id > 0) {
      this.setState({
        Id: row.Id,
        descripcion: row.descripcion,
        imagen: row.filebase64,
        action: "edit",
        visible: true,
        editable: false
      });
    } else {
      this.setState({
        Id: 0,
        descripcion: "",
        imagen: "",
        action: "create",
        visible: true,
        editable: true
      });
    }
  };

  generarBotones = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.mostrarForm(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar Descripción"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.Eliminar(row.Id);
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar Ruta"
        >
          <i className="fa fa-trash" />
        </button>
        <button
          className="btn btn-outline-indigo"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.onDownloaImagen(row.ArchivoId)}
          data-toggle="tooltip"
          data-placement="top"
          title="Descargar Imagen"
        >
          <i className="fa fa-cloud-download"></i>
        </button>
      </div>
    );
  };
  render() {
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={el => {
            this.growl = el;
          }}
          baseZIndex={1000}
        />
        <div align="right">
          <button
            type="button"
            className="btn btn-outline-primary"
            icon="fa fa-fw fa-ban"
            onClick={this.mostrarForm}
          >
            Carga Masiva Avances de Ingeniería
          </button>
        </div>

        <Dialog
          header="Nuevo"
          visible={this.state.visible}
          onHide={this.OcultarFormulario}
          modal={true}
          style={{ width: "700px", overflow: "auto" }}
        >
          <div>
            <form onSubmit={this.EnviarFormulario}>
              <div className="row">
                <div className="col">
           
                    <Field
                      name="uploadFile"
                      label="Documento"
                      type={"file"}
                      edit={true}
                      readOnly={false}
                      onChange={this.handleChange}
                      error={this.state.errors.uploadFile}
                    />
                       <Field
                    name="fecha_desde"
                    label="Fecha Desde"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fecha_desde}
                    onChange={this.handleChange}
                    error={this.state.errors.fecha_desde}
                  />
                    </div>
                   <div className="col">
                  <Field
                    name="fecha_emision"
                    label="Fecha Emisión"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fecha_emision}
                    onChange={this.handleChange}
                    error={this.state.errors.fecha_emision}
                  />
                 
                    <Field
                    name="fecha_hasta"
                    label="Fecha Hasta"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fecha_hasta}
                    onChange={this.handleChange}
                    error={this.state.errors.fecha_hasta}
                  />
                </div>
              </div>
              <button type="submit" className="btn btn-outline-primary">
                Guardar
              </button>
              &nbsp;
              <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                onClick={this.OcultarFormulario}
              >
                Cancelar
              </button>
            </form>
          </div>
        </Dialog>
      </BlockUi>
    );
  }

  onDownloaImagen = Id => {
    return (window.location = `/Proyecto/AvanceObra/SubirExcelMasivo/`);
  };

  EnviarFormulario(event) {
    event.preventDefault();

    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      this.setState({blocking:true})
      if (this.state.action == "create") {
        const formData = new FormData();
        formData.append("Id", 0);
        formData.append("fecha_desde", this.state.fecha_desde);
        formData.append("fecha_presentacion", this.state.fecha_emision);
        formData.append("fecha_hasta", this.state.fecha_hasta);
        formData.append("UploadedFile", this.state.uploadFile);
        const multipart = {
          headers: {
            "content-type": "multipart/form-data"
          }
        };
        axios
          .post("/Proyecto/AvanceIngenieria/SubirExcelMasivo", formData, multipart)
          .then(response => {
              console.log(response)
            if (response.data == "OK") {
              this.props.showSuccess("Generando Correctamente");
              this.setState({blocking:false})
              this.setState({ visible: false });
              this.props.unlockScreen();
              } else {
              this.props.showWarn("Ocurrió un Error");
            }
          })
          .catch(error => {
            console.log(error);
            this.setState({blocking:false})
            this.props.showWarn("Ocurrió un Error");
          });
      } else {
        console.log(this.state.uploadFile);

      }
    }
  }

  MostrarFormulario() {
    this.setState({ visible: true });
  }

  handleChange(event) {
    if (event.target.files) {
      console.log(event.target.files);
      let files = event.target.files || event.dataTransfer.files;
      if (files.length > 0) {
        let uploadFile = files[0];
        this.setState({
          uploadFile: uploadFile
        });
      }
    } else {
      this.setState({ [event.target.name]: event.target.value });
    }
  }

  OcultarFormulario = () => {
    this.setState({ visible: false });
  };
}
const Container = wrapForm(UploadAvance);
ReactDOM.render(<Container />, document.getElementById("content_upload"));
