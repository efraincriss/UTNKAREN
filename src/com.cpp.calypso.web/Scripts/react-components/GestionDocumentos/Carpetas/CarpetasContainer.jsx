import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import Wrapper from "../../Base/BaseWrapper";
import http from "../../Base/HttpService";
import {
  CONTROLLER_CARPETA,
  MODULO_DOCUMENTOS,
  FRASE_CONTRATO_ELIMINADO,
  FRASE_ERROR_GLOBAL,
} from "../../Base/Strings";
import { CarpetaForm } from "./CarpetasForm.jsx";
import { CarpetaTable } from "./CarpetaTable.jsx";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import Field from "../../Base/Field-v2";
import { Fragment } from "react";
import { Button } from "primereact-v2/button";
import { forEach } from "lodash";

class CarpetaContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      carpetas: [],
      catalogoEstadosContrato: [],
      carpetaSeleccionada: {},
      mostrarConfirmacion: false,
      keyForm: 12,
      pass: "",
      continue: false,
      viewenable: false,
      fechas: null,
      continuepass:false,
    };
    this.handleChange = this.handleChange.bind(this);
  }

  componentDidMount() {
    this.getFechasSync();
    this.loadData();
  }
  viewenable = () => {
    this.setState({ viewenable: true });
  };
  hideenable = () => {
    this.setState({ viewenable: false,continue:false,continuepass:false,pass:""});
  };
  GetSync = () => {
  
  };
  getFechasSync = () => {
    axios
      .post("/Proyecto/Contrato/GetFechasSincronizacion", {})
      .then((response) => {
        console.log(response.data);
        this.setState({ fechas: response.data });
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error("Error al obtener fechas de sincronización", "Error");
        this.props.unlockScreen();
      });
  };

  SyncPass = () => {
    console.log("SyncPass");
    this.props.blockScreen();
    axios
      .post("/Proyecto/Contrato/GetPass", {
        pass: this.state.pass,
      })
      .then((response) => {
        console.log(response.data);
        if (response.data != "OK") {
          abp.notify.error(
            "El código de seguridad es incorrecto",
            "Error"
          );
        } else {
          abp.notify.success("Correcto", "AVISO");
          this.setState({viewenable:false, continue: true,continuepass:true });
          this.getFechasSync();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error(
          "Existe un inconveniente inténtelo más tarde",
          "Error"
        );
        this.props.unlockScreen();
      });
  };

  SyncronizarCarpetas = () => {
    console.log("SincronizarCarpetas");
    this.props.blockScreen();
    axios
      .post("/Proyecto/Contrato/SyncCarpetas", {})
      .then((response) => {
        console.log(response.data);
        if (response.data != "OK") {
          abp.notify.error(
            "Existe un inconveniente inténtelo más tarde",
            "Error"
          );
        } else {
          abp.notify.success(
            "Información de carpetas actualizadas a la nube correctamente",
            "AVISO"
          );
          this.getFechasSync();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error(
          "Existe un inconveniente inténtelo más tarde",
          "Error"
        );
        this.props.unlockScreen();
      });
  };
  SyncronizarDocumentos = () => {
    console.log("SincronizarDocumentos");
    this.props.blockScreen();
    axios
      .post("/Proyecto/Contrato/SyncDocumentos", {})
      .then((response) => {
        console.log(response.data);
        if (response.data != "OK") {
          abp.notify.error(
            "Existe un inconveniente inténtelo más tarde",
            "Error"
          );
        } else {
          abp.notify.success(
            "Información de documentos actualizadas a la nube correctamente",
            "AVISO"
          );
          this.getFechasSync();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error(
          "Existe un inconveniente inténtelo más tarde",
          "Error"
        );
        this.props.unlockScreen();
      });
  };
  SyncronizarSecciones = () => {
    console.log("SincronizarSecciones");
    this.props.blockScreen();
    axios
      .post("/Proyecto/Contrato/SyncSecciones", {})
      .then((response) => {
        console.log(response.data);
        if (response.data != "OK") {
          abp.notify.error(
            "Existe un inconveniente inténtelo más tarde",
            "Error"
          );
        } else {
          abp.notify.success(
            "Información de secciones actualizadas a la nube correctamente",
            "AVISO"
          );
          this.getFechasSync();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error(
          "Existe un inconveniente inténtelo más tarde",
          "Error"
        );
        this.props.unlockScreen();
      });
  };
  SyncronizarImagenes = () => {
    console.log("SincronizarImagenes");
    this.props.blockScreen();
    axios
      .post("/Proyecto/Contrato/SyncImg", {})
      .then((response) => {
        console.log(response.data);
        if (response.data != "OK") {
          abp.notify.error(
            "Existe un inconveniente inténtelo más tarde",
            "Error"
          );
        } else {
          abp.notify.success(
            "Información de imagenes actualizadas a la nube correctamente",
            "AVISO"
          );
          this.getFechasSync();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error(
          "Existe un inconveniente inténtelo más tarde",
          "Error"
        );
        this.props.unlockScreen();
      });
  };
  SyncronizarUsuarios = () => {
    console.log("Sincronizar Usuarios");
    this.props.blockScreen();
    axios
      .post("/Proyecto/Contrato/SyncUsuarios", {})
      .then((response) => {
        console.log(response.data);
        /*if (response.data != "OK") {
          abp.notify.error(
            "Existe un inconveniente inténtelo más tarde",
            "Error"
          );
        } else {*/
        abp.notify.success(
          "Información de usuarios autorizados actualizadas a la nube correctamente",
          "AVISO"
        );
        this.getFechasSync();

        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error(
          "Existe un inconveniente inténtelo más tarde",
          "Error"
        );
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

  render() {
    return (
      <div className="card">
        <div className="card-body">
          <div className="row" style={{ marginTop: "1em" }}>
            <div className="col" align="right">
              <button
                className="btn btn-outline-primary"
                onClick={this.viewenable}
              >
                {" "}
                Sincronizar Nube{" "}
              </button>
              {/*<button
                className="btn btn-outline-primary"
                onClick={() => {
                  if (
                    window.confirm(
                      `Esta acción actualizará los datos del Azure, ¿Desea continuar?`
                    )
                  )
                    this.GetSync();
                }}
              >
                Actualizar Nube
              </button>
             */}{" "}
              <button
                className="btn btn-outline-primary"
                onClick={() => this.mostrarFormulario({})}
              >
                Nuevo Contrato
              </button>
            </div>
          </div>

          <hr />

          <div className="row">
            <div className="col">
              <CarpetaTable
                data={this.state.carpetas}
                mostrarFormulario={this.mostrarFormulario}
                mostrarConfirmacionParaEliminar={
                  this.mostrarConfirmacionParaEliminar
                }
              />
            </div>
          </div>
          <Dialog
            header="Gestión de Contratos"
            modal={true}
            visible={this.state.mostrarFormulario}
            style={{ width: "750px" }}
            onHide={this.onHideFormulario}
          >
            <CarpetaForm
              carpeta={this.state.carpetaSeleccionada}
              actualizarCarpetaSeleccionada={this.actualizarCarpetaSeleccionada}
              onHideFormulario={this.onHideFormulario}
              showSuccess={this.props.showSuccess}
              showWarn={this.props.showWarn}
              blockScreen={this.props.blockScreen}
              unlockScreen={this.props.unlockScreen}
              catalogoEstadoContrato={this.state.catalogoEstadosContrato}
              key={this.state.keyForm}
            />
          </Dialog>

          <Dialog
            header="Confirmación"
            visible={this.state.mostrarConfirmacion}
            modal
            style={{ width: "500px" }}
            footer={this.construirBotonesDeConfirmacion()}
            onHide={this.onHideFormulario}
          >
            <div className="confirmation-content">
              <div className="p-12">
                <i
                  className="pi pi-exclamation-triangle p-mr-3"
                  style={{ fontSize: "2rem" }}
                />
                <p>
                  {" "}
                  ¿Está seguro de eliminar el contrato{" "}
                  <b>{this.state.carpetaSeleccionada.Codigo}</b>? Si desea
                  continuar presione ELIMINAR, caso contrario CANCELAR
                </p>
              </div>
            </div>
          </Dialog>


          <Dialog
            header="Continuar Sincronización Nube"
            visible={this.state.viewenable}
            style={{ width: "65vw" }}
            modal
            onHide={this.hideenable}
          >
              {!this.state.continue && (
              <>
              <div>
              Para continuar con la sincronización digite la clave de seguridad
            </div>
            <br />

          
              <div className="row">
                <div className="col">
                  <Field
                    name="pass"
                    label="Código de Seguridad"
                    required
                    edit={true}
                    value={this.state.pass}
                    onChange={this.handleChange}
                  />
                </div>
              </div>
              <div className="row">
                  <div className="col">
                  <Button
                          label="Continuar"
                          icon="pi pi-user-plus"
                          className="p-button-primary"
                          onClick={()=>this.SyncPass()}
                        />
                  </div>
                
                </div>
              </>
            )}
           
          </Dialog>
       

          <Dialog
            header="Continuar Sincronización Nube"
            visible={this.state.continuepass}
            style={{ width: "65vw" }}
            modal
            onHide={this.hideenable}
          >        
            {this.state.continue && (
              <>
                <hr />
                <div className="row">
                  <div className="col">
                    <strong>
                      <span>Opción</span>
                    </strong>
                  </div>
                  <div className="col">
                    <strong>
                      <span>Ultimá Fecha Sincronización</span>
                    </strong>
                  </div>
                </div>
                <div className="row">
                  <div className="col">
                    <div className="row">
                      <div className="col">
                        <Button
                          label="Sincronizar Usuarios Autorizados"
                          icon="pi pi-user-plus"
                          style={{ width: "80%" }}
                          className="p-button-secondary"
                          onClick=
                          {() => {
                            if (
                              window.confirm(
                                `Esta seguro continuar con el proceso de sincronización de carpetas`
                              )
                            ){this.SyncronizarUsuarios();}
                          }}
                        />
                      </div>
                      <div className="col">
                        {this.state.fechas != null
                          ? this.state.fechas.fechaUsuarios
                          : ""}
                      </div>
                    </div>
                    <hr />
                    <div className="row">
                      <div className="col">
                        {" "}
                        <Button
                          label="Sincronizar Contratos"
                          icon="pi pi-folder"
                          style={{ width: "80%" }}
                          className="p-button-secondary"
                          onClick={() => {
                            if (
                              window.confirm(
                                `Esta seguro continuar con el proceso de sincronización de carpetas`
                              )
                            ) {
                              this.SyncronizarCarpetas();
                            }
                          }}
                        />{" "}
                      </div>
                      <div className="col">
                        {this.state.fechas != null
                          ? this.state.fechas.fechaCarpetas
                          : ""}
                      </div>
                    </div>
                    <hr />
                    <div className="row">
                      <div className="col">
                        <Button
                          label="Sincronizar Documentos"
                          icon="pi pi-bookmark"
                          style={{ width: "80%" }}
                          className="p-button-secondary"
                          onClick={() => {
                            if (
                              window.confirm(
                                `Esta seguro continuar con el proceso de sincronización de documentos`
                              )
                            ) {
                              this.SyncronizarDocumentos();
                            }
                          }}
                        />
                      </div>
                      <div className="col">
                        {this.state.fechas != null
                          ? this.state.fechas.fechaDocumentos
                          : ""}
                      </div>
                    </div>
                    <hr />
                    <div className="row">
                      <div className="col">
                        {" "}
                        <Button
                          label="Sincronizar Secciones"
                          icon="pi pi-tag"
                          style={{ width: "80%" }}
                          className="p-button-secondary"
                          onClick={() => {
                            if (
                              window.confirm(
                                `Esta seguro continuar con el proceso de sincronización de secciones`
                              )
                            ) {
                              this.SyncronizarSecciones();
                            }
                          }}
                        />
                      </div>
                      <div className="col">
                        {this.state.fechas != null
                          ? this.state.fechas.fechaSecciones
                          : ""}
                      </div>
                    </div>
                    <hr />
                    <div className="row">
                      <div className="col">
                        <Button
                          label="Sincronizar Imágenes"
                          icon="pi pi-images"
                          style={{ width: "80%" }}
                          className="p-button-secondary"
                          onClick={() => {
                            if (
                              window.confirm(
                                `Esta seguro continuar con el proceso de sincronización de Imagenes`
                              )
                            ) {
                              this.SyncronizarImagenes();
                            }
                          }}
                        />
                      </div>
                      <div className="col">
                        {this.state.fechas != null
                          ? this.state.fechas.fechaImagenes
                          : ""}
                      </div>
                    </div>
                  </div>{" "}
                </div>
                <div align="right">
                  {" "}
                  <Button
                    label="Cancelar"
                    icon="pi pi-times"
                    className="p-button-secondary"
                    onClick={this.hideenable}
                  />
                </div>
              </>
            )}
          </Dialog>
        </div>
      </div>
    );
  }

  loadData = () => {
    this.props.blockScreen();
    var self = this;
    Promise.all([this.obtenerCarpetas(), this.obtenerEstadosContrato()])
      .then(function ([carpetas, estados]) {
        let carpetasData = carpetas.data;
        if (carpetasData.success === true) {
          self.setState({
            carpetas: carpetasData.result,
          });
        }

        let estadosData = estados.data;
        if (estadosData.success === true) {
          let catalogoEstadosContrato = self.props.buildDropdown(
            estadosData.result,
            "nombre"
          );
          self.setState({
            catalogoEstadosContrato,
          });
        }
        self.props.unlockScreen();
      })
      .catch((error) => {
        self.props.unlockScreen();
        console.log(error);
      });
  };

  eliminarCarpeta = () => {
    this.props.blockScreen();
    let url = "";
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_CARPETA}/EliminarCarpeta/${this.state.carpetaSeleccionada.Id}`;

    http
      .delete(url, {})
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          this.props.showSuccess(FRASE_CONTRATO_ELIMINADO);
          this.onHideFormulario(true);
        } else {
          var message = data.result;
          this.props.showWarn(message);
          this.props.unlockScreen();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(FRASE_ERROR_GLOBAL);
        this.props.unlockScreen();
      });
  };

  mostrarFormulario = (carpeta) => {
    this.setState({
      carpetaSeleccionada: carpeta,
      mostrarFormulario: true,
    });
  };

  mostrarConfirmacionParaEliminar = (carpeta) => {
    this.setState({
      carpetaSeleccionada: carpeta,
      mostrarConfirmacion: true,
    });
  };

  onHideFormulario = (recargar = false) => {
    this.setState({
      mostrarFormulario: false,
      carpetaSeleccionada: {},
      mostrarConfirmacion: false,
      keyForm: Math.random(),
    });
    if (recargar) {
      this.loadData();
    }
  };

  obtenerCarpetas = () => {
    let url = "";
    url = `/${MODULO_DOCUMENTOS}/Carpeta/ObtenerTodas`;
    return http.get(url);
  };

  obtenerEstadosContrato = () => {
    let url = "";
    url = `/${MODULO_DOCUMENTOS}/Carpeta/ObtenerCatalogoEstados`;
    return http.get(url);
  };

  actualizarCarpetaSeleccionada = (carpeta) => {
    this.setState({ carpetaSeleccionada: carpeta });
  };

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarCarpeta()}
          icon="pi pi-save"
        />
        <Button
          style={{ marginLeft: "0.4em" }}
          label="Cancelar"
          className="p-button-outlined"
          onClick={() => this.onHideFormulario()}
          icon="pi pi-ban"
        />
      </Fragment>
    );
  };
}

const Container = Wrapper(CarpetaContainer);
ReactDOM.render(
  <Container />,
  document.getElementById("gestion_carpetas_container")
);
