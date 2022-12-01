import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import Field from "./Base/Field-v2";
import wrapForm from "./Base/BaseWrapper";
import { Dialog } from "primereact-v3.3/dialog";
import { Growl } from "primereact-v3.3/growl";
import { ScrollPanel } from "primereact-v2/scrollpanel";
import ReactQuill from "react-quill"; // ES6

class EnviarRequerimiento extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      visible: false,
      data: [],
      transmittal: null,
      requerimiento: null,
      errors: {},
      editable: true,
      types: [],
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
      asunto: "",
      mailto:"",
      body:
        " ",

      renderview: "details",
      modules: {
        toolbar: [
          [{ header: "1" }, { header: "2" }, { font: [] }],
          [{ size: [] }],
          ["bold", "italic", "underline", "strike", "blockquote"],
          [
            { list: "ordered" },
            { list: "bullet" },
            { indent: "-1" },
            { indent: "+1" }
          ],
          ["image"],
          ["clean"]
        ]
      },
      formats: [
        "header",
        "font",
        "size",
        "bold",
        "italic",
        "underline",
        "strike",
        "blockquote",
        "list",
        "bullet",
        "indent",
        "link",
        "image",
        "video"
      ]
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleChangeEditor = this.handleChangeEditor.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.mostrarForm = this.mostrarForm.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind();
    this.isValid = this.isValid.bind();
    this.Enviar = this.Enviar.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
  }

  componentDidMount() {
    this.GetTransmittal();
  }
  Enviar = event => {
    console.log(event);
    event.preventDefault();
    this.props.blockScreen();

    axios
      .post("/proyecto/Requerimiento/GetEnviar", {
        Id: document.getElementById("Id").className,
        asunto: this.state.asunto,
        body: this.state.body
      })
      .then(response => {
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
        "/Proyecto/Requerimiento/DetailsApi/" +
          document.getElementById("Id").className,
        {}
      )
      .then(response => {
        console.log("Requeimiento", response.data);
        this.setState({
          requerimiento: response.data
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
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={el => {
            this.growl = el;
          }}
          baseZIndex={1000}
        />

        <button
          style={{ marginRight: "0.3em", marginTop: "0.1em" }}
          className="btn btn-outline-primary"
          onClick={() => this.MostrarFormEnviar()}
        >
          {" "}
          Enviar
        </button>
        <button
          style={{ marginRight: "0.3em", marginTop: "0.1em" }}
          className="btn btn-outline-primary"
          onClick={() => this.EnvioManual()}
        >
          Envio Manual
        </button>

        <div className="row" align="left">
          <Dialog
            header="Envio de Requerimiento"
            visible={this.state.view_enviar}
            onHide={this.OcultarFormEnviar}
            style={{ width: "70vw" }}
          >
            {" "}
            <ScrollPanel
              style={{ width: "68vw", height: "500px" }}
              className="custombar1"
            >
              <div style={{ padding: "1em", lineHeight: "1.5" }}>
                <form onSubmit={this.Enviar}>
                  <div className="row">
                    <div className="col">
                      <label>
                        <b>NOMBRE LISTA DISTRIBUCIÓN: </b> REQUERIMIENTO
                        CONTRATOS
                      </label>
                      <hr />
                      <div className="form-group">
                        <label>
                          <b>Asunto:</b>
                        </label>
                        <br />
                        <textarea
                          rows="2"
                          type="text"
                          name="asunto"
                          className="form-control"
                          onChange={this.handleChange}
                          value={this.state.asunto}
                        />
                      </div>

                      <hr />
                      <div className="form-group">
                        <label>
                          <b>Descripción</b>
                        </label>
                        <br />
                        <ReactQuill
                          rows={10}
                          theme="snow"
                          value={this.state.body}
                          onChange={this.handleChangeEditor}
                          modules={this.state.modules}
                          formats={this.state.formats}
                        />
                      </div>

                      <hr />
                      <label>
                        <b>Correos Lista Distribución: </b>
                        {this.state.requerimiento != null
                          ? this.state.requerimiento.correos_lista_distribucion
                          : ""}
                      </label>
                    </div>
                  </div>
                  <button type="submit" className="btn btn-outline-primary">
                    Enviar
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
                </form>
              </div>
            </ScrollPanel>
          </Dialog>
        </div>
      </BlockUi>
    );
  }

  EnvioManual = () => {
    console.log('ejecucion ')
    axios
      .post(
        "/Proyecto/Requerimiento/GetMailto/" +
        document.getElementById("Id").className,
        {}
      )
      .then(response => {
        console.log("mailto:",response.data);
        this.setState({ mailto: response.data });
        window.location.href =response.data;
      })
      .catch(error => {
        console.log(error);
      });
  };
  RedireccionarDetalle = () => {
    this.setState({ renderview: "details" });
  };
  OcultarFormEnviar = () => {
    this.setState({ view_enviar: false });
  };
  MostrarFormEnviar = () => {
    this.setState({
      view_enviar: true,
      users: false,
      body: ""
      // renderview: "listadistribucion"
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
  handleChangeEditor(value) {
    console.log(value);
    this.setState({ body: value });
  }
  EnviarFormulario(event) {
    event.preventDefault();
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
const Container = wrapForm(EnviarRequerimiento);
ReactDOM.render(<Container />, document.getElementById("content-button"));
