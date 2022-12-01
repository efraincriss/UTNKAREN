import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";

class Ingenieria extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data: [],
      visible: false,
      errors: {},
      descripcion: "",
      editable: true,
      fechaEmision: new Date(),
      fechaCorte: new Date(new Date().getFullYear(),new Date().getMonth(),20),
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
    if (this.state.fechaEmision == null) {
      errors.fechaEmision = "Campo Requerido";
    }
    if (this.state.fechaCorte == null) {
      errors.fechaCorte = "Campo Requerido";
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
          style={{ marginLeft: "0.3em"}}
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
            style={{ marginLeft: "0.3em",marginTop:"1em" }}
            onClick={this.mostrarForm}
          >
            Generar Certificado Ingenieria
          </button>
        </div>

        <Dialog
          header="Nuevo"
          visible={this.state.visible}
          onHide={this.OcultarFormulario}
          modal={true}
          style={{ width: "400px", overflow: "auto" }}
        > <BlockUi tag="div" blocking={this.state.blocking}>
          <div>
            <form onSubmit={this.EnviarFormulario}>
              <div className="row">
                <div className="col">
           
                    <Field
                    name="fechaCorte"
                    label="Fecha Corte"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fechaCorte}
                    onChange={this.handleChange}
                    error={this.state.errors.fechaCorte}
                  />
                   
                   <Field
                    name="fechaEmision"
                    label="Fecha Desde"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fechaEmision}
                    onChange={this.handleChange}
                    error={this.state.errors.fechaEmision}
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
          </BlockUi>
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
     
        axios
        .post("/Proyecto/AvanceIngenieria/GenerarCertificado", {
          Id:document.getElementById("Id").className,
          fechaCorte:this.state.fechaCorte,
          fechaEmision:this.state.fechaEmision
        })
          .then(response => {
              console.log(response)
            if (response.data != "ERROR") {
              this.props.showSuccess("Generando Correctamente");
              this.setState({blocking:false})
              this.setState({ visible: false });
              window.location.href = "/Proyecto/AvanceIngenieria/DetailsProyecto/" + response.data;
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
const Container = wrapForm(Ingenieria);
ReactDOM.render(<Container />, document.getElementById("content"));
