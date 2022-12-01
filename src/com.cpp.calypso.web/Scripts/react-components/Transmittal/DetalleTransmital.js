import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";
import { Checkbox } from "primereact-v2/checkbox";
import UploadPdfForm from "./UploadPdfForm";
import Vista from "./VistaListaDistribucionT";
class DetalleTransmittalUserList extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      visible: false,
      data: [],
      transmittal: null,

      errors: {},
      editable: true,

      Id: 0,
      codigo_detalle: ".",
      descripcion: "",
      version: "A",
      nro_hojas: 0,
      nro_copias: 0,
      ArchivoId: 0,
      TransmitalId: 0,
      uploadFile: "",
      es_oferta: true,
      vigente: true,
      action: "create",
      renderview: "details"
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.mostrarForm = this.mostrarForm.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind();
    this.isValid = this.isValid.bind();
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
  }

  componentDidMount() {
    this.GetList();
    //  this.GetCatalogs();
    this.GetTransmittal();
  }

  isValid = () => {
    const errors = {};

    if (this.state.uploadFile == "") {
      errors.uploadFile = "Campo Requerido";
    }
    if (this.state.codigo_detalle == "") {
      errors.codigo_detalle = "Campo Requerido";
    }
    if (this.state.descripcion == "") {
      errors.descripcion = "Campo Requerido";
    }
    if (this.state.nro_hojas == 0) {
      errors.nro_hojas = "Campo Requerido";
    }
    if (this.state.nro_copias == 0) {
      errors.nro_copias = "Campo Requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  isValidEdit = () => {
    const errors = {};

    if (this.state.codigo_detalle == "") {
      errors.codigo_detalle = "Campo Requerido";
    }
    if (this.state.descripcion == "") {
      errors.descripcion = "Campo Requerido";
    }
    if (this.state.nro_hojas == 0) {
      errors.nro_hojas = "Campo Requerido";
    }
    if (this.state.nro_copias == 0) {
      errors.nro_copias = "Campo Requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  GetList = () => {
    this.props.blockScreen();
    axios
      .post(
        "/Proyecto/TransmitalCabecera/ObtenerAdjunto/" +
          document.getElementById("Id").className,
        {}
      )
      .then(response => {
        console.log(response.data);
        this.setState({ data: response.data });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetTransmittal = () => {
    this.props.blockScreen();
    axios
      .post(
        "/Proyecto/TransmitalCabecera/ObtenerDetail/" +
          document.getElementById("Id").className,
        {}
      )
      .then(response => {
        console.log("transmittal", response.data);
        this.setState({
          transmittal: response.data
        });
        if (response.data != null && response.data.tiene_ofertacomercial) {
          if (this.state.transmittal.tiene_oferta) {
            var number = this.state.transmittal.codigo_oferta_comercial.split(
              "-"
            );
            var code = number[number.length - 1];
            this.setState({
              codigo_detalle: "Anexo Oferta #" + parseInt(code)
            });
          } else {
            this.setState({
              codigo_detalle: response.data.codigo_oferta_comercial
            });
          }
        }
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

  GetCatalogs = () => {};
  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }
  cambiarcheck = e => {
    console.clear();
    console.log(this.state.transmittal);
    this.setState({ es_oferta: e.checked });
    if (e.checked) {
      if (this.state.transmittal.tiene_ofertacomercial) {
        if (this.state.transmittal.tiene_oferta) {
          var number = this.state.transmittal.codigo_oferta_comercial.split(
            "-"
          );
          var code = number[number.length - 1];
          this.setState({
            codigo_detalle: "Anexo Oferta #" + parseInt(code)
          });
        } else {
          this.setState({
            codigo_detalle: this.state.transmittal.codigo_oferta_comercial
          });
        }
      } else {
        var number = this.state.transmittal.codigo_oferta_comercial.split("-");
        var code = number[number.length - 1];
        this.setState({
          codigo_detalle: "Anexo Oferta #" + parseInt(code)
        });
      }
    } else {
      if (this.state.transmittal.tiene_ofertacomercial) {
        if (this.state.transmittal.tiene_oferta) {
          var number = this.state.transmittal.codigo_oferta_comercial.split(
            "-"
          );
          var code = number[number.length - 1];
          this.setState({
            codigo_detalle: "Anexo Oferta #" + parseInt(code)
          });
        } else {
          var number = this.state.transmittal.codigo_oferta_comercial.split(
            "-"
          );
          var code = number[number.length - 1];
          this.setState({
            codigo_detalle: "Anexo Oferta #" + parseInt(code)
          });
        }
      } else {
        var number = this.state.transmittal.codigo_oferta_comercial.split("-");
        var code = number[number.length - 1];
        this.setState({
          codigo_detalle: "Anexo Oferta #" + parseInt(code)
        });
      }
    }
  };

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };

  Eliminar = Id => {
    console.log(Id);
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerEliminarDetalle/", {
        Id: Id
      })
      .then(response => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente");
          this.GetList();
          this.GetTransmittal();
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
    console.log(row);
    if (row != null && row.Id > 0) {
      this.setState({
        Id: row.Id,
        descripcion: row.descripcion,
        version: row.version,
        nro_hojas: row.nro_hojas,
        nro_copias: row.nro_copias,
        es_oferta: row.es_oferta,
        action: "edit",
        visible: true,
        editable: false,
        codigo_detalle: row.codigo_detalle,
        ArchivoId: row.ArchivoId
      });
    } else {
      this.setState({
        Id: 0,
        descripcion: "",
        version: "A",
        nro_hojas: 0,
        nro_copias: 0,
        uploadFile: "",
        es_oferta: false,
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
          onClick={() => this.onDownload(row.ArchivoId)}
          data-toggle="tooltip"
          data-placement="top"
          title="Descargar Imagen"
        >
          <i className="fa fa-cloud-download"></i>
        </button>
      </div>
    );
  };
  RedireccionarDetalle = () => {
    this.setState({ renderview: "details" });
  };
  render() {
    if (this.state.renderview === "details") {
      return (
        <BlockUi tag="div" blocking={this.state.blocking}>
          <Growl
            ref={el => {
              this.growl = el;
            }}
            baseZIndex={1000}
          />
          <div className="row">
            <div style={{ width: "100%" }}>
              <div className="card">
                <div className="card-body">
                  <div className="content-section implementation">
                    <div className="row">
                      <div className="col-8">
                        <h5>Adjuntos Transmittal:</h5>
                      </div>

                      <div className="col" align="right">
                       {/** <button
                          style={{ marginLeft: "0.3em" }}
                          className="btn btn-outline-primary"
                          onClick={() => this.MostrarFormEnviar()}
                        >
                          Enviar{" "}
                        </button>
                        */} 
                        <button
                          className="btn btn-outline-indigo"
                          style={{ marginLeft: "0.3em" }}
                          onClick={() => this.DescargarWord()}
                        >
                          Descargar Transmittal{" "}
                          <i className="fa fa-cloud-download"></i>
                        </button>
                        <button
                          style={{ marginLeft: "0.3em" }}
                          className="btn btn-outline-primary"
                          onClick={() => this.Redireccionar()}
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
                            <b>Código:</b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.codigo_transmital
                              : ""}
                          </h6>
                          <h6>
                            <b>Descripción:</b>
                            {this.state.transmittal != null
                              ? this.state.transmittal.descripcion
                              : ""}
                          </h6>
                          <h6>
                            <b>Enviado por:</b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.enviado_por
                              : ""}
                          </h6>
                        </div>
                        <div className="col-6">
                          <h6>
                            <b>Digido a:</b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.dirigido_a
                              : ""}
                          </h6>
                          <h6>
                            <b>Con Copia:</b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.copia_a
                              : ""}
                          </h6>
                          <h6>
                            <b>Versión:</b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.version
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
            <ul className="nav nav-tabs" id="gestion_tabs" role="tablist">
              <li className="nav-item">
                <a
                  className="nav-link active"
                  id="gestion-tab"
                  data-toggle="tab"
                  href="#gestion"
                  role="tab"
                  aria-controls="home"
                  aria-expanded="true"
                >
                  Adjuntos {/* <i className="fa fa-bus fa-1x" />*/}
                </a>
              </li>
            </ul>
            <div className="tab-content" id="myTabContent">
              <div
                className="tab-pane fade show active"
                id="gestion"
                role="tabpanel"
                aria-labelledby="gestion-tab"
              >
                <div align="right">
                  <button
                    type="button"
                    className="btn btn-outline-primary"
                    icon="fa fa-fw fa-ban"
                    onClick={this.mostrarForm}
                  >
                    Nuevo
                  </button>
                </div>
                <br />
                <div>
                  <BootstrapTable
                    data={this.state.data}
                    hover={true}
                    pagination={true}
                  >
                    <TableHeaderColumn
                      dataField="any"
                      dataFormat={this.Secuencial}
                      width={"8%"}
                      tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    >
                      Nº
                    </TableHeaderColumn>
                    <TableHeaderColumn
                      dataField="codigo_detalle"
                      width={"12%"}
                      tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      filter={{ type: "TextFilter", delay: 500 }}
                      dataSort={true}
                    >
                      Código
                    </TableHeaderColumn>
                    <TableHeaderColumn
                      dataField="nombre_archivo"
                      width={"12%"}
                      tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      filter={{ type: "TextFilter", delay: 500 }}
                      dataSort={true}
                    >
                      Nombre
                    </TableHeaderColumn>
                    <TableHeaderColumn
                      dataField="descripcion"
                      width={"12%"}
                      tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      filter={{ type: "TextFilter", delay: 500 }}
                      dataSort={true}
                    >
                      nombre_archivo Descripción
                    </TableHeaderColumn>
                    <TableHeaderColumn
                      dataField="nro_copias"
                      width={"8%"}
                      tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      filter={{ type: "TextFilter", delay: 500 }}
                      dataSort={true}
                    >
                      Nro Copias
                    </TableHeaderColumn>

                    <TableHeaderColumn
                      dataField="nro_hojas"
                      width={"8%"}
                      tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      filter={{ type: "TextFilter", delay: 500 }}
                      dataSort={true}
                    >
                      Nro Hojas
                    </TableHeaderColumn>
                    <TableHeaderColumn
                      dataField="estado_es_oferta"
                      width={"8%"}
                      tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      filter={{ type: "TextFilter", delay: 500 }}
                      dataSort={true}
                    >
                      Es Oferta
                    </TableHeaderColumn>
                    <TableHeaderColumn
                      dataField="Id"
                      isKey
                      width={"10%"}
                      dataFormat={this.generarBotones}
                      thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                    >
                      Opciones
                    </TableHeaderColumn>
                  </BootstrapTable>
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
                        </div>
                        <div className="col">
                          <Field
                            name="codigo_detalle"
                            label="Código"
                            required
                            edit={true}
                            readOnly={false}
                            value={this.state.codigo_detalle}
                            onChange={this.handleChange}
                            error={this.state.errors.codigo_detalle}
                          />
                        </div>
                      </div>
                      <div className="row">
                        <div className="col">
                          <Field
                            name="descripcion"
                            label="Descripción"
                            required
                            edit={true}
                            readOnly={false}
                            value={this.state.descripcion}
                            onChange={this.handleChange}
                            error={this.state.errors.descripcion}
                          />
                        </div>
                      </div>
                      <div className="row">
                        <div className="col">
                          <Field
                            name="nro_hojas"
                            label="Nro Hojas"
                            required
                            edit={true}
                            type={"number"}
                            readOnly={false}
                            value={this.state.nro_hojas}
                            onChange={this.handleChange}
                            error={this.state.errors.nro_hojas}
                          />
                        </div>
                        <div className="col">
                          <Field
                            name="nro_copias"
                            label="Nro Copias"
                            required
                            edit={true}
                            type={"number"}
                            readOnly={false}
                            value={this.state.nro_copias}
                            onChange={this.handleChange}
                            error={this.state.errors.nro_copias}
                          />
                        </div>
                      </div>
                      {this.state.transmittal != null &&
                        !this.state.transmittal.tiene_oferta && (
                          <div className="row">
                            <div className="col">
                              <Checkbox
                                checked={this.state.es_oferta}
                                onChange={e => this.cambiarcheck(e)}
                              />{" "}
                              <label>Es Oferta?</label>
                            </div>
                          </div>
                        )}
                      <br />
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
        </BlockUi>
      );
    } else {
      return (
        <Vista
          transmittal={this.state.transmittal}
          showSuccess={this.props.showSuccess}
          showWarning={this.props.showWarning}
          showWarn={this.props.showWarn}
          blockScreen={this.props.blockScreen}
          unlockScreen={this.props.unlockScreen}
          RedireccionarDetalle={this.RedireccionarDetalle}
        />
      );
    }
  }
  MostrarFormEnviar = () => {
    this.setState({
      //view_enviar: true,
      users: false,
      body: "",
      renderview: "listadistribucion"
    });
  };
  DescargarWord = () => {
    console.log("Entro word");
    var Id = document.getElementById("Id").className;
    console.log(Id);
    return (window.location = `/Proyecto/TransmitalCabecera/ObtenerWord/${Id}`);
  };
  Redireccionar = () => {
    var Id2 = document.getElementById("OfertaComercialId").className;
    console.log(Id2);
    if (Id2 == 0) {
      return (window.location = `/Proyecto/TransmitalCabecera/IndexTransmital`);
    } else {
      return (window.location = `/Proyecto/OfertaComercial/DetailsOferta/${Id2}`);
    }

    // return (window.location = `/Proyecto/TransmitalCabecera/IndexUsuarios/${Id}`);
  };
  onDownload = Id => {
    return (window.location = `/Proyecto/TransmitalCabecera/Descargar/${Id}`);
  };

  EnviarFormulario(event) {
    event.preventDefault();
    if (this.state.action == "create") {
      if (!this.isValid()) {
        return;
      } else {
        const formData = new FormData();
        formData.append("Id", 0);
        formData.append("codigo_detalle", this.state.codigo_detalle);
        formData.append("descripcion", this.state.descripcion);
        formData.append("nro_copias", this.state.nro_copias);
        formData.append("nro_hojas", this.state.nro_hojas);
        formData.append(
          "TransmitalId",
          document.getElementById("Id").className
        );
        formData.append("ArchivoId", 0);
        formData.append("version", this.state.version);
        formData.append("es_oferta", this.state.es_oferta);
        formData.append("vigente", true);
        formData.append("UploadedFile", this.state.uploadFile);
        const multipart = {
          headers: {
            "content-type": "multipart/form-data"
          }
        };
        axios
          .post(
            "/Proyecto/TransmitalCabecera/ObtenerCrearDetalle",
            formData,
            multipart
          )
          .then(response => {
            if (response.data == "OK") {
              this.props.showSuccess(" Guardado Correctamente");
              this.setState({ visible: false, uploadFile: "" });
              this.GetList();
              this.GetTransmittal();
            } else if (response.data == "EXISTE_OFERTA") {
              this.props.showWarn("Ya existe un archivo de tipo oferta");
            }
          })
          .catch(error => {
            console.log(error);
            this.props.showWarn("Ocurrió un Error");
          });
      }
    } else {
      if (!this.isValidEdit()) {
        return;
      } else {
        console.log(this.state.version);
        const formData = new FormData();
        formData.append("Id", this.state.Id);
        formData.append("codigo_detalle", this.state.codigo_detalle);
        formData.append("descripcion", this.state.descripcion);
        formData.append("nro_copias", this.state.nro_copias);
        formData.append("nro_hojas", this.state.nro_hojas);
        formData.append(
          "TransmitalId",
          document.getElementById("Id").className
        );
        formData.append("version", "B");
        formData.append("vigente", true);
        formData.append("ArchivoId", this.state.ArchivoId);
        formData.append("es_oferta", this.state.es_oferta);
        if (this.state.uploadFile != "") {
          formData.append("UploadedFile", this.state.uploadFile);
        }
        const multipart = {
          headers: {
            "content-type": "multipart/form-data"
          }
        };
        axios
          .post(
            "/Proyecto/TransmitalCabecera/ObtenerEditarDetalle",
            formData,
            multipart
          )
          .then(response => {
            if (response.data == "OK") {
              this.props.showSuccess(" Guardado Correctamente");
              this.setState({ visible: false, uploadFile: "" });
              this.GetList();
              this.GetTransmittal();
            } else if (response.data == "EXISTE_OFERTA") {
              this.props.showWarn("Ya existe un archivo de tipo oferta");
            }
          })
          .catch(error => {
            console.log(error);
            this.props.showWarn("Ocurrió un Error");
          });
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
const Container = wrapForm(DetalleTransmittalUserList);
ReactDOM.render(<Container />, document.getElementById("content"));
