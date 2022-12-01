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
import Vista from "./VistaListaDistribucion";
import { ScrollPanel } from "primereact-v2/scrollpanel";
import { MultiSelect } from "primereact-v2/multiselect";
import { FileUpload } from "primereact-v2/fileupload";
class DetalleCartaList extends React.Component {
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
      users: false,
      view_enviar: false,
      mailto: "",
      body:
        " Estimada Elena Adjunto envío copia de la carta N° 3808-B-LT-000541 Planificación de Personal Operativo - diciembre 2019, la cual recibirán a la brevedad en vuestras oficinas.   Agradezco vuestra gentil atención. Saludos cordiales",

      renderview: "details",
      ListIds: [],
      listDistribuciones: []
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.mostrarForm = this.mostrarForm.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind();
    this.isValid = this.isValid.bind();
    this.Enviar = this.Enviar.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.onBasicUpload = this.onBasicUpload.bind(this);
  }

  componentDidMount() {
    this.GetList();
    //  this.GetCatalogs();
    this.GetTransmittal();
  }
  Enviar = event => {
    console.log(event);
    event.preventDefault();
    this.props.blockScreen();

    axios
      .post("/proyecto/Carta/GetEnviar", {
        Id: document.getElementById("Id").className,
        body: this.state.body,
        ListIds: this.state.ListIds
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
  EnvioManual = () => {
    this.setState({view_enviar:false});
    console.log("ejecucion ");
    axios
      .post(
        "/Proyecto/Carta/GetMailto/",
        {Id:document.getElementById("Id").className,
        Ids:this.state.ListIds}
      )
      .then(response => {
        this.setState({ mailto: response.data });
        window.location.href = response.data;
      })
      .catch(error => {
        console.log(error);
      });
  };
  isValid = () => {
    const errors = {};

    if (this.state.uploadFile == "") {
      errors.uploadFile = "Campo Requerido";
    }

    if (this.state.descripcion == "") {
      errors.descripcion = "Campo Requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  isValidEdit = () => {
    const errors = {};
    if (this.state.descripcion == "") {
      errors.descripcion = "Campo Requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  GetList = () => {
    this.props.blockScreen();
    axios
      .post(
        "/Proyecto/Carta/ObtenerAdjuntos/" +
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
        "/Proyecto/Carta/ObtenerDetail/" +
          document.getElementById("Id").className,
        {}
      )
      .then(response => {
        console.log("carta", response.data);
        var datos = response.data.listDistribuciones.map(item => {
          return {
            label: item.nombre,
            dataKey: item.Id,
            value: item.Id
          };
        });
        this.setState({
          transmittal: response.data,
          listDistribuciones: datos
        });

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
      .post("/Proyecto/Carta/ObtenerEliminarDetalle/", {
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
                        <h5>Cartas Adjuntos:</h5>
                      </div>

                      <div className="col" align="right">
                        {/*this.state.transmittal != null &&
                          this.state.transmittal.tipo == 1 && (
                            <button
                              style={{ marginLeft: "0.3em" }}
                              className="btn btn-outline-primary"
                              onClick={() => this.MostrarFormEnviar()}
                            >
                              Enviar{" "}
                            </button>
                          )*/}
                        {this.state.transmittal != null &&
                          this.state.transmittal.nombretipoCarta == "ENVIADA" && (
                            <button
                              style={{
                                marginLeft: "0.3em",
                                marginTop: "0.1em"
                              }}
                              className="btn btn-outline-primary"
                              onClick={() => this.MostrarFormEnviar()}
                            >
                              Envio Manual
                            </button>
                          )}
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
                            <b>Número Carta:</b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.numeroCarta
                              : ""}
                          </h6>
                          
                          <h6>
                            <b>Clasificación:</b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.nombreClasificacion
                              : ""}
                          </h6>
                          <h6>
                            <b>Dirigido A:</b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.dirigidoA
                              : ""}
                          </h6>
                          <h6>
                            <b>Descripción:</b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.descripcion
                              : ""}
                          </h6>
                        </div>
                        <div className="col-6">
                          <h6>
                            <b>Asunto: </b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.asunto
                              : ""}
                          </h6>
                          <h6>
                            <b>Enviado por:</b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.enviadoPor
                              : ""}
                          </h6>
                          <h6>
                            <b>Tipo Destinatario:</b>{" "}
                            {this.state.transmittal != null
                              ? this.state.transmittal.nombretipoDestinatario
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
                      dataField="nombreArchivo"
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
                      Descripción
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
                   
                      <div className="row">
                        <div className="col">
                          {/** <FileUpload
                            chooseLabel="Seleccionar Archivo"
                            cancelLabel="Cancelar"
                            mode="basic"
                            name="uploadFile"
                            accept=".xlsx,.xls,image/*,.doc, .docx,.ppt, .pptx,.txt,.pdf, .zip,.rar"
                            onSelect={this.onBasicUpload}
                            
                          />*/}
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
                      <br />
                      <button
                              style={{
                                marginLeft: "0.3em",
                                marginTop: "0.1em"
                              }}
                              className="btn btn-outline-primary"
                              onClick={() => this.EnviarFormulario()}
                            >
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

                  </div>
                </Dialog>

                <Dialog
                  header="Envio de Carta"
                  visible={this.state.view_enviar}
                  onHide={this.OcultarFormEnviar}
                  modal={true}
                  width="720px"
                >
                  <ScrollPanel
                    style={{ width: "700px",height: "300px" }}
                    className="custombar1"
                  >
                    <div style={{ padding: "1em", lineHeight: "1.5" }}>
                      
                        
                       
                              <div className="row">
                                <div className="col">
                                  <MultiSelect
                                    value={this.state.ListIds}
                                    options={this.state.listDistribuciones}
                                    onChange={e =>
                                      this.setState({ ListIds: e.value })
                                    }
                                    style={{ width: "100%" }}
                                    defaultLabel="Seleccione.."
                                    filter={true}
                                    placeholder="Seleccione"
                                  />
                                </div>
                              </div>
                             
                     
                              <button
                              style={{
                                marginLeft: "0.3em",
                                marginTop: "0.1em"
                              }}
                              className="btn btn-outline-primary"
                              onClick={() => this.EnvioManual()}
                            >
                              Continuar
                            </button>
                      &nbsp;
                          <button
                            type="button"
                            className="btn btn-outline-primary"
                            icon="fa fa-fw fa-ban"
                            onClick={this.OcultarFormEnviar}
                          >
                            Cancelar
                          </button>
                      
                    </div>
                  </ScrollPanel>
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
  RedireccionarDetalle = () => {
    this.setState({ renderview: "details" });
  };
  OcultarFormEnviar = () => {
    this.setState({ view_enviar: false });
  };
  MostrarFormEnviar = () => {
    this.setState({
      view_enviar: true,
      
    });
  };
  DescargarWord = () => {
    console.log("Entro word");
    var Id = document.getElementById("Id").className;
    console.log(Id);
    return (window.location = `/Proyecto/TransmitalCabecera/ObtenerWord/${Id}`);
  };
  Redireccionar = () => {
    return (window.location = `/Proyecto/Carta/Index`);

    // return (window.location = `/Proyecto/TransmitalCabecera/IndexUsuarios/${Id}`);
  };
  onDownload = Id => {
    return (window.location = `/Proyecto/TransmitalCabecera/Descargar/${Id}`);
  };

  EnviarFormulario(event) {
    //event.preventDefault();
    if (this.state.action == "create") {
      if (!this.isValid()) {
        return;
      } else {
        const formData = new FormData();
        formData.append("Id", 0);
        formData.append("descripcion", this.state.descripcion);
        formData.append("CartaId", document.getElementById("Id").className);
        formData.append("ArchivoId", 0);
        formData.append("vigente", true);
        formData.append("UploadedFile", this.state.uploadFile);
        const multipart = {
          headers: {
            "content-type": "multipart/form-data"
          }
        };
        axios
          .post("/Proyecto/Carta/ObtenerCrearDetalle", formData, multipart)
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
        formData.append("descripcion", this.state.descripcion);

        formData.append("CartaId", document.getElementById("Id").className);

        formData.append("vigente", true);
        formData.append("ArchivoId", this.state.ArchivoId);

        if (this.state.uploadFile != "") {
          formData.append("UploadedFile", this.state.uploadFile);
        }
        const multipart = {
          headers: {
            "content-type": "multipart/form-data"
          }
        };
        axios
          .post("/Proyecto/Carta/ObtenerEditarDetalle", formData, multipart)
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
  onBasicUpload(event) {
    console.clear();
    console.log(event);
    if (event.files) {
      if (event.files.length > 0) {
        let uploadFile = event.files[0];

        this.setState({
          uploadFile: uploadFile
        });
      }
    }
    console.log(this.state.uploadFile);
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
const Container = wrapForm(DetalleCartaList);
ReactDOM.render(<Container />, document.getElementById("content"));
