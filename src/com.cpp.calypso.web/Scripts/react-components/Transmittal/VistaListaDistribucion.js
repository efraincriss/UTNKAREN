import React from "react";
import axios from "axios";
import { Dialog } from "primereact/components/dialog/Dialog";
import moment from "moment";
import { Button } from "primereact-v2/button";
import Field from "../Base/Field-v2";
import { InputText } from "primereact-v2/inputtext";
import { DataTable } from "primereact-v2/datatable";
import { Column } from "primereact-v2/column";
export default class Historicos extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data: [],
      correosseleccionados: [],
      /* Colaboradores Ausentismos */
      errors: {},
      displayDialog: false,
      nombres: " ",
      correo: " ",
      users: false,
      body: ""
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
  }

  componentDidMount() {
    this.GetListaDistribucion();
  }
  addNew = () => {
    this.setState({ displayDialog: true });
  };

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  DeleteAusentismos = row => {
    this.props.blockScreen();

    axios
      .post("/RRHH/ColaboradoresAusentismo/GetEliminarAusentismo/", {
        id: row.Id
      })
      .then(response => {
        console.clear();
        console.log(response.data);
        if (response.data === "OK") {
          this.GetAusentismos();
          abp.notify.success("Eliminado Correctamente", "Correcto");
        } else {
          this.props.unlockScreen();
          abp.notify.error(
            "Existe un inconveniente inténtelo más tarde",
            "Error"
          );
        }
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetListaDistribucion = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/Carta/ObtenerCorreosLista/", {})
      .then(response => {
        console.clear();
        console.log(response.data);
        console.log("Lista");
        this.setState({ data: response.data });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }
  mostrarEditar = row => {
    console.clear();
    console.log(row);
    this.setState({
      vieweditar: true,
      ColaboradoresAusentismo: row,
      fecha_inicio: row.fecha_inicio,
      fecha_fin: row.fecha_fin,
      observacion: row.observacion
    });
  };
  loadReintegro = row => {
    console.clear();
    console.log(row);
    this.setState({
      viewreintegro: true,
      ColaboradoresAusentismo: row
    });
  };

  AddCorreo = event => {
    event.preventDefault();
    console.log(this.state.data);

    this.state.data.push({
      nombres: this.state.nombres,
      correo: this.state.correo
    });
    this.setState({ displayDialog: false });
    console.log(this.state.data);
  };
  EnviarFormulario = event => {
    this.setState({ loading: true });
    event.preventDefault();
    axios
      .post("/Proyecto/Carta/GetEditAusentimo", {
        Id: this.state.ColaboradoresAusentismo.Id,
        fecha_inicio: this.state.fecha_inicio,
        fecha_fin: this.state.fecha_fin,
        observacion: this.state.observacion,
        catalogo_tipo_ausentismo_id: this.state.ColaboradoresAusentismo
          .catalogo_tipo_ausentismo_id,
        colaborador_id: this.state.ColaboradoresAusentismo.colaborador_id,
        estado: "ACTIVO"
      })
      .then(response => {
        if (response.data == "EXISTE") {
          abp.notify.error(
            "Ya existe un ausentismo activo del mismo tipo en las misma fechas",
            "ALERTA"
          );
          this.setState({ loading: false });
        } else {
          if (response.data == "OK") {
            abp.notify.success("Ausetismo Editado", "Aviso");
            this.setState({ vieweditar: false, errors: {} });
            this.GetAusentismos();
          } else {
            abp.notify.error(
              "Existe un incoveniente intentelo más tarde",
              "Error"
            );
            this.setState({ loading: false });
          }
        }
      })
      .catch(error => {
        console.log(error);
        abp.notify.error("Existe un incoveniente intentelo más tarde", "Error");
        this.setState({ loading: false });
      });
  };
  guardarReintegro() {
    event.preventDefault();

    if (!this.isValidR()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      this.props.blockScreen();
      console.log("file", this.state.uploadFileR);

      const formData = new FormData();
      formData.append("idAusentismo", this.state.ColaboradoresAusentismo.Id);
      formData.append("fecha_reintegro", this.state.fecha_reintegro);
      formData.append("motivo", this.state.motivo_reintegro);
      if (this.state.uploadFileR == "") {
        formData.append("UploadedFile", null);
      } else {
        formData.append("UploadedFile", this.state.uploadFileR);
      }
      const config = { headers: { "content-type": "multipart/form-data" } };
      axios
        .post("/RRHH/ColaboradoresAusentismo/CrearReintegro/", formData, config)
        .then(response => {
          this.props.unlockScreen();
          this.props.showSuccess("Reintegro registrado Correctamente");
          this.GetAusentismos();
          this.setState({ viewreintegro: false });
        })
        .catch(error => {
          this.props.unlockScreen();
          console.log(error);
        });
    }
  }
  Enviar = event => {
    console.log(event);

    this.props.blockScreen();
    console.log(this.state.correosseleccionados);
    axios
      .post("/proyecto/Carta/GetEnviar", {
        Id: document.getElementById("Id").className,
        user_transmittal: this.state.users,
        body: this.state.body,
        list: this.state.correosseleccionados
      })
      .then(response => {
        if (response.data === "SIN_TRANSMITAL") {
          abp.notify.error("No ha generado un carta ", "AVISO");
          this.props.unlockScreen();
        }
        if (response.data == "SIN_ARCHIVOS") {
          abp.notify.error("No existen archivos adjuntos en la carta", "AVISO");
          this.props.unlockScreen();
        }

        if (response.data == "SIN_CORREOS") {
          abp.notify.error(
            "No existen correos en la Lista de Distribución",
            "AVISO"
          );
          this.props.unlockScreen();
        }
        if (response.data == "OK") {
          abp.notify.success("Adjuntos Enviados Corectamente", "Éxito");
          this.setState({
            view_enviar: false
          });
          /*  this.props.ConsultarPresupuesto();
          this.props.detalles_oferta();
          this.props.CalcularMonto();*/
          this.props.unlockScreen();
        }
        if (response.data == "ERROR") {
          this.props.unlockScreen();
        }
      })
      .catch(error => {
        console.log(error);

        this.props.unlockScreen();
      });
  };

  mostrarUploadPDF = row => {
    this.setState({ uploadPDF: true, ColaboradoresAusentismo: row });
  };
  ocultarEditar = () => {
    this.setState({
      vieweditar: false,
      ColaboradoresAusentismo: null,
      errors: {}
    });
  };
  ocultarReintegro = () => {
    this.setState({
      viewreintegro: false,
      ColaboradoresAusentismo: null,
      errorsr: {}
    });
  };

  ocultarUploadPDF = () => {
    this.setState({ uploadPDF: false, uploadFile: "" });
  };

  onDownload = row => {
    if (
      row != null &&
      row.requisitos != null &&
      row.requisitos[0] != null &&
      row.requisitos[0].archivo_id != null &&
      row.requisitos[0].archivo_id > 0
    ) {
      return (window.location = `/RRHH/ColaboradoresAusentismo/GetArchivo/${row.requisitos[0].archivo_id}`);
    } else {
      this.props.showWarn("No posee Archivos Cargados");
    }
  };
  handleChangeUploadFile = event => {
    if (event.files) {
      if (event.files.length > 0) {
        let uploadFile = event.files[0];
        this.setState(
          {
            uploadFile: uploadFile
          },
          this.handleSubmitUploadFile
        );
      } else {
        abp.notify.error("Selecciona un Archivo", "Error");
      }
    } else {
      abp.notify.error("Selecciona un Archivo", "Error");
    }
  };

  handleSubmitUploadFile = () => {
    this.setState({ loading: true });
    const formData = new FormData();
    formData.append("uploadFile", this.state.uploadFile);
    formData.append("Id", this.state.ColaboradoresAusentismo.requisitos[0].Id);
    const config = {
      headers: {
        "content-type": "multipart/form-data"
      }
    };

    console.log("Comienza archivo baja");

    axios
      .post("/RRHH/ColaboradoresAusentismo/GetChangeArchivo", formData, config)
      .then(response => {
        let data = response.data;
        if (data.success === true) {
          this.setState({
            uploadFile: ""
          });
          abp.notify.success("Documento Cargado Correctamente!", "Aviso");
          this.ocultarUploadPDF();
          this.GetAusentismos();
        } else {
          var message = $.fn.responseAjaxErrorToString(data);
          abp.notify.error(message, "Error");
        }
      })
      .catch(error => {
        console.log(error);
        abp.notify.error("Existe un incoveniente intentelo más tarde", "Error");
        this.setState({ loading: false });
      });
  };

  generateButton(cell, row) {
    return (
      <div style={{ textAlign: "center" }}>
        {row.estado === "ACTIVO" && (
          <button
            title="Registrar Reintegro Anticipado"
            onClick={() => this.loadReintegro(row)}
            style={{ marginLeft: "0.3em" }}
            className="btn btn-outline-primary btn-sm fa fa-user"
          ></button>
        )}
        {
          //row.estado === "ACTIVO" &&
          <button
            className="btn btn-outline-primary btn-sm"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.mostrarEditar(row)}
            data-toggle="tooltip"
            data-placement="top"
            title="Editar Documento"
          >
            <i className="fa fa-pencil"></i>
          </button>
        }

        {
          //row.estado === "ACTIVO" &&
          <button
            className="btn btn-outline-indigo btn-sm"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.mostrarUploadPDF(row)}
            data-toggle="tooltip"
            data-placement="top"
            title="Subir Documento"
          >
            <i className="fa fa-cloud-upload"></i>
          </button>
        }
        {row != null &&
          row.requisitos != null &&
          row.requisitos[0] != null &&
          row.requisitos[0].archivo_id != null &&
          row.requisitos[0].archivo_id > 0 && (
            <button
              className="btn btn-outline-indigo btn-sm"
              style={{ marginLeft: "0.3em" }}
              onClick={() => this.onDownload(row)}
              data-toggle="tooltip"
              data-placement="top"
              title="Descargar Documento"
            >
              <i className="fa fa-cloud-download"></i>
            </button>
          )}
        {
          //row.estado === "ACTIVO" &&
          <button
            title="Eliminar"
            style={{ marginLeft: "0.3em" }}
            onClick={() => {
              if (window.confirm("¿Está seguro de eliminar la información?"))
                this.DeleteAusentismos(row);
            }}
            className="btn btn-outline-danger btn-sm fa fa-trash"
          ></button>
        }
      </div>
    );
  }
  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };
  dateFormatter = cell => {
    console.log(cell);
    if (cell != null) {
      var d = moment(new Date(cell).toLocaleDateString()).format("DD/MM/YYYY");
      return d;
    } else {
      return "";
    }
  };
  /* Cell Editing */
  onEditorValueChange = (props, value) => {
    let updatedCars = [...props.value];
    updatedCars[props.rowIndex][props.field] = value;
    this.setState({ cars1: updatedCars });
  };

  inputTextEditor = (props, field) => {
    return (
      <InputText
        type="text"
        value={props.rowData[field]}
        onChange={e => this.onEditorValueChange(props, e.target.value)}
      />
    );
  };
  correoEditor = props => {
    console.log(props);
    return this.inputTextEditor(props, "correo");
  };
  onClear = () => {
    this.setState({ uploadFileR: "" });
  };
  render() {
    return (
      <div>
        <div className="row">
          <div style={{ width: "100%" }}>
            <div className="card">
              <div className="card-body">
                <div className="content-section implementation">
                  <div className="row">
                    <div className="col-4">
                      <h5>Envio de Carta:</h5>
                    </div>

                    <div className="col-8" align="right">
                      <button
                        style={{ marginLeft: "0.3em" }}
                        className="btn btn-outline-primary"
                        onClick={() => this.Enviar()}
                      >
                        Enviar{" "}
                      </button>
                      <button
                        style={{ marginLeft: "0.3em" }}
                        className="btn btn-outline-primary"
                        onClick={() => this.props.RedireccionarDetalle()}
                      >
                        Regresar
                      </button>
                    </div>
                  </div>
                  <br />

                  <div>
                    <div className="row">
                      <div className="col-6">
                        <h6>
                          <b>Número Carta:</b>{" "}
                          {this.props.transmittal != null
                            ? this.props.transmittal.numero_carta
                            : ""}
                        </h6>
                        <h6>
                          <b>Descripción:</b>{" "}
                          {this.props.transmittal != null
                            ? this.props.transmittal.descripcion
                            : ""}
                        </h6>
                      </div>
                      <div className="col-6">
                        <h6>
                          <b>Asunto: </b>{" "}
                          {this.props.transmittal != null
                            ? this.props.transmittal.asunto
                            : ""}
                        </h6>
                        <h6>
                          <b>Enviado por:</b>{" "}
                          {this.props.transmittal != null
                            ? this.props.transmittal.enviado_por
                            : ""}
                        </h6>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="row">
          <div style={{ width: "100%" }}>
            <div className="card">
              <div className="card-body">
                <div className="content-section implementation">
                  <div className="row">
                    <div className="col">
                      <label>Descripción</label>
                      <textarea
                        type="text"
                        name="body"
                        className="form-control"
                        onChange={this.handleChange}
                        value={this.state.body}
                      />
                    </div>{" "}
                  </div>{" "}
                </div>
              </div>{" "}
              <hr />
              <div className="row">
                <div className="col"></div>
                <div className="col" align="right">
                  {" "}
                  <Button
                    style={{ float: "right" }}
                    label="Añadir"
                    icon="pi pi-plus"
                    onClick={this.addNew}
                  />
                </div>
              </div>
              <DataTable
                value={this.state.data}
                header="Correos Lista Distribución"
                selection={this.state.correosseleccionados}
                onSelectionChange={e =>
                  this.setState({ correosseleccionados: e.value })
                }
                editable={true}
                paginator={true}
                rows={15}
              >
                <Column selectionMode="multiple" style={{ width: "3em" }} />
                <Column field="nombres" header="Nombres" filter={true} />
                <Column
                  field="correo"
                  header="Correo"
                  editor={this.correoEditor}
                  filter={true}
                />
              </DataTable>
              <Dialog
                visible={this.state.displayDialog}
                width="300px"
                header="Nuevo Correo"
                modal={true}
                onHide={() => this.setState({ displayDialog: false })}
              >
                <div>
                  <form onSubmit={this.AddCorreo}>
                    <div className="row">
                      <div className="col">
                        <Field
                          name="nombres"
                          label="Apellidos y Nombres"
                          required
                          edit={true}
                          readOnly={false}
                          value={this.state.nombres}
                          onChange={this.handleChange}
                          error={this.state.errors.nombres}
                        />

                        <Field
                          name="correo"
                          label="Correo"
                          required
                          edit={true}
                          readOnly={false}
                          value={this.state.correo}
                          onChange={this.handleChange}
                          error={this.state.errors.correo}
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
            </div>
          </div>
        </div>
      </div>
    );
  }
}
