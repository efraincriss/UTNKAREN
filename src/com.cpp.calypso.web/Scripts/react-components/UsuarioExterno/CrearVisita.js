import React from "react";
import axios from "axios";
import moment, { now } from "moment";
import BlockUi from "react-block-ui";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Button } from "primereact/components/button/Button";
import { MultiSelect } from "primereact/components/multiselect/MultiSelect";
import { Card } from "primereact-v2/card";
import { Checkbox } from "primereact-v2/checkbox";
import QRCode from "qrcode.react";
import CryptoJS from "crypto-js";
import { TIPO_GRUPO_PERSONAL, PASSCRYPTO } from "../Base/Constantes";
export default class CrearVisita extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      usuario: [],
      id_visita: "",
      id_responsable: "",
      id_colaborador: "",
      tipo_identificacion: "",
      nro_identificacion: "",
      nombres_apellidos: "",
      fecha_desde: "",
      fecha_hasta: "",
      empresa: "",
      responsable: "",
      motivo: "",
      errores: [],
      formIsValid: "",
      loading: false,
      responsables: [],
      visible_responsable: false,
      selected_responsable: [],
      tiposComidas: [],
      tiposServicios: [],
      tiposMovilizacion: [],
      alimentacion: false,
      tipo_alimentacion: [],
      alojamiento: false,
      movilizacion: false,
      tipo_movilizacion: "",
      lavanderia: false,
      servicios_colaborador: [],
      serviciosConsulta: [],
      comidas_col: [],
      movilizacionConsulta: [],
      comidas: [],
      visible_qr: false,

      //ERRORES
      errors: {},
      /*Generación QR */
      QrDialogE: false,
      DataColaborador: "",
      EncryptedData: "",

      //VALIDACION CÉDULA
      checked: false,
    };

    /* */
    this.DescargarQR = this.DescargarQR.bind(this);
    this.MostrarDialogQrE = this.MostrarDialogQrE.bind(this);
    this.permitirvalidacioncedula = this.permitirvalidacioncedula.bind(this);
    /* */

    this.childQR = React.createRef();
    this.titulo = this.titulo.bind(this);

    this.handleChange = this.handleChange.bind(this);
    this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleValidation = this.handleValidation.bind(this);
    this.Guardar = this.Guardar.bind(this);

    this.abrirConfirmacion = this.abrirConfirmacion.bind(this);
    this.cerrarConfirmacion = this.cerrarConfirmacion.bind(this);
    this.showQR = this.showQR.bind(this);
    this.onHideQR = this.onHideQR.bind(this);
    this.HuellaDigital = this.HuellaDigital.bind(this);

    this.ConsultaUsuarioExterno = this.ConsultaUsuarioExterno.bind(this);

    this.consultaBDD = this.consultaBDD.bind(this);
    this.CargaVisita = this.CargaVisita.bind(this);

    this.DatosEnvio = this.DatosEnvio.bind(this);
    this.AsignarResponsable = this.AsignarResponsable.bind(this);

    this.GetCatalogos = this.GetCatalogos.bind(this);
    this.cargarCatalogos = this.cargarCatalogos.bind(this);
    this.getFormSelectTipoMovilizacion = this.getFormSelectTipoMovilizacion.bind(
      this
    );

    this.arrayServicios = this.arrayServicios.bind(this);
    this.arrayComidas = this.arrayComidas.bind(this);
    this.GuardarServicios = this.GuardarServicios.bind(this);
    this.ConsultaComidas = this.ConsultaComidas.bind(this);
    this.ConsultaServiciosColaborador = this.ConsultaServiciosColaborador.bind(
      this
    );
    this.ConsultaMovilizaciones = this.ConsultaMovilizaciones.bind(this);
  }

  componentDidMount() {
    this.GetCatalogos();
    console.log(sessionStorage.getItem("Colaborador"));
  }

  MostrarDialogQrE() {
    console.log("DIALOGQR");
    if (this.state.id_colaborador > 0) {
      this.setState({ loading: true });
      axios
        .post("/RRHH/Colaboradores/GetDataQrExternos/", {
          Id: this.state.id_colaborador,
        })
        .then((response) => {
          console.log("data", response);
          this.setState(
            {
              EncryptedData: response.data.toString(),
            },
            console.log(this.state.EncryptedData)
          );
          this.setState({
            QrDialogE: true,
            loadingqr: false,
            loading: false,
          });
        })
        .catch((error) => {
          console.log(error);
          this.setState({ loading: false });
        });
    }

    //this.setState({ loadingqr: true, checked: row.validacion_cedula, QrDialog: true });
  }

  consultaBDD() {
    if (this.state.responsable != "") {
      this.setState({ loading: true });
      axios
        .post("/RRHH/Colaboradores/GetResponsable", {
          nombre: this.state.responsable,
        })
        .then((response) => {
          this.setState({ loading: false });
          if (response.data.length > 0) {
            this.setState({ responsables: response.data });
            this.abrirConfirmacion();
          } else {
            this.props.warnMessage("No existe responsable!");
          }
          console.log("bdd", response.data);
        })
        .catch((error) => {
          console.log(error);
          this.setState({ loading: false });
        });
    } else {
    }
  }

  render() {
    const footer = (
      <div>
        <Button
          label="Seleccionar"
          icon="pi pi-check"
          onClick={() => this.AsignarResponsable()}
        />
        <Button
          label="Cancelar"
          icon="pi pi-times"
          onClick={() => this.cerrarConfirmacion()}
          className="p-button-secondary"
        />
      </div>
    );
    const options = {
      withoutNoDataText: true,
    };
    const selectRowProp = {
      mode: "radio",
      clickToSelect: true,
      onSelect: this.DatosEnvio,
    };
    return (
      <div>
        <BlockUi tag="div" blocking={this.state.loading}>
          {/* <form onSubmit={this.handleSubmit}> */}
          <div className="row">
            <div className="col-xs-12 col-md-12">
              <div className="row">
                <div className="form-group col">
                  <label htmlFor="text">
                    <b>Tipo de Identificación:</b>{" "}
                    {this.state.tipo_identificacion}{" "}
                  </label>
                </div>
                <div className="form-group col">
                  <label htmlFor="text">
                    <b>No. de Identificación:</b>{" "}
                    {this.state.nro_identificacion}{" "}
                  </label>
                </div>
                <div className="form-group col">
                  <label htmlFor="text">
                    <b>Apellidos Nombres:</b> {this.state.nombres_apellidos}{" "}
                  </label>
                </div>
              </div>
              <h5>Información de Visita:</h5>
              <div className="row">
                <div className="col-4">
                  <div className="form-group">
                    <label htmlFor="fecha_desde">* Fecha Desde: </label>
                    <input
                      type="date"
                      id="fecha_desde"
                      className="form-control"
                      value={this.state.fecha_desde}
                      onChange={this.handleChange}
                      name="fecha_desde"
                    />
                    <span style={{ color: "red" }}>
                      {this.state.errores["fecha_desde"]}
                    </span>
                  </div>
                </div>
                <div className="col-3">
                  <div className="form-group">
                    <label htmlFor="fecha_hasta">* Fecha Hasta: </label>
                    <input
                      type="date"
                      id="fecha_hasta"
                      className="form-control"
                      value={this.state.fecha_hasta}
                      onChange={this.handleChange}
                      name="fecha_hasta"
                    />
                    <span style={{ color: "red" }}>
                      {this.state.errores["fecha_hasta"]}
                    </span>
                  </div>
                </div>
                <div className="col-5">
                  <div className="form-group">
                    <label htmlFor="responsable">* Responsable: </label>
                    <input
                      type="text"
                      id="responsable"
                      className="form-control"
                      value={this.state.responsable}
                      onChange={this.handleChangeUpperCase}
                      name="responsable"
                      style={{ width: "90%", display: "inline" }}
                    />
                    <button
                      type="button"
                      className="btn btn-outline-primary fa fa-search"
                      onClick={this.consultaBDD}
                      style={{ marginTop: "-1%" }}
                    ></button>
                    <span style={{ color: "red" }}>
                      {this.state.errores["responsable"]}
                    </span>
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col-4">
                  <div className="form-group">
                    <label htmlFor="empresa">* Empresa: </label>
                    <input
                      type="text"
                      id="empresa"
                      className="form-control"
                      value={this.state.empresa}
                      onChange={this.handleChangeUpperCase}
                      name="empresa"
                    />
                    <span style={{ color: "red" }}>
                      {this.state.errores["empresa"]}
                    </span>
                  </div>
                </div>
                <div className="col-8">
                  <div className="form-group">
                    <label htmlFor="motivo">* Motivo: </label>
                    <textarea
                      type="text"
                      id="motivo"
                      /*rows="1"*/ maxLength="200"
                      className="form-control"
                      value={this.state.motivo}
                      onChange={this.handleChange}
                      name="motivo"
                    />
                    <span style={{ color: "red" }}>
                      {this.state.errores["motivo"]}
                    </span>
                  </div>
                </div>
              </div>

              <h5>Información de Servicios:</h5>
              <div className="row">
                <div className="col-3">
                  <div
                    className="form-group checkbox"
                    style={{ display: "inline-flex", marginTop: "32px" }}
                  >
                    <label htmlFor="alimentacion" style={{ width: "285px" }}>
                      Alimentación:
                    </label>
                    <input
                      type="checkbox"
                      id="alimentacion"
                      className="form-control"
                      checked={this.state.alimentacion}
                      onChange={this.handleInputChange}
                      name="alimentacion"
                      style={{ marginTop: "5px", marginLeft: "-110px" }}
                    />
                  </div>
                </div>
                <div className="col-4">
                  <div className="form-group">
                    <div className="content-section implementation">
                      <label htmlFor="tipo_alimentacion">
                        * Tipo de Alimentación:{" "}
                      </label>
                      <MultiSelect
                        value={this.state.tipo_alimentacion}
                        optionLabel="nombre"
                        options={this.state.tiposComidas}
                        dataKey="Id"
                        defaultLabel="Seleccione..."
                        onChange={(e) =>
                          this.setState({ tipo_alimentacion: e.value })
                        }
                        disabled={!this.state.alimentacion}
                        style={{ width: "100%" }}
                        filter={true}
                      />
                    </div>
                    <span style={{ color: "red" }}>
                      {this.state.errores["tipo_alimentacion"]}
                    </span>
                  </div>
                </div>
                <div className="col-1"></div>
                <div className="col-4">
                  <div
                    className="form-group checkbox"
                    style={{ display: "inline-flex", marginTop: "32px" }}
                  >
                    <label htmlFor="alojamiento" style={{ width: "285px" }}>
                      Alojamiento:
                    </label>
                    <input
                      type="checkbox"
                      id="alojamiento"
                      className="form-control"
                      checked={this.state.alojamiento}
                      onChange={this.handleInputChange}
                      name="alojamiento"
                      style={{ marginTop: "5px", marginLeft: "-110px" }}
                    />
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col-3">
                  <div
                    className="form-group checkbox"
                    style={{ display: "inline-flex", marginTop: "32px" }}
                  >
                    <label htmlFor="movilizacion" style={{ width: "285px" }}>
                      Movilización:
                    </label>
                    <input
                      type="checkbox"
                      id="movilizacion"
                      className="form-control"
                      checked={this.state.movilizacion}
                      onChange={this.handleInputChange}
                      name="movilizacion"
                      style={{ marginTop: "5px", marginLeft: "-110px" }}
                    />
                  </div>
                </div>
                <div className="col-4">
                  <div className="form-group">
                    <label htmlFor="tipo_movilizacion">
                      * Tipo de Movilización:{" "}
                    </label>
                    <select
                      value={this.state.tipo_movilizacion}
                      onChange={this.handleChange}
                      className="form-control"
                      name="tipo_movilizacion"
                      disabled={!this.state.movilizacion}
                    >
                      <option value="">Seleccione...</option>
                      {this.getFormSelectTipoMovilizacion()}
                    </select>
                    <span style={{ color: "red" }}>
                      {this.state.errores["tipo_movilizacion"]}
                    </span>
                  </div>
                </div>
                <div className="col-1"></div>
                <div className="col-4">
                  <div
                    className="form-group checkbox"
                    style={{ display: "inline-flex", marginTop: "32px" }}
                  >
                    <label htmlFor="lavanderia" style={{ width: "285px" }}>
                      Lavandería:
                    </label>
                    <input
                      type="checkbox"
                      id="lavanderia"
                      className="form-control"
                      checked={this.state.lavanderia}
                      onChange={this.handleInputChange}
                      name="lavanderia"
                      style={{ marginTop: "5px", marginLeft: "-110px" }}
                    />
                  </div>
                </div>
              </div>

              <br />
              <div className="form-group">
                <div className="col">
                  <button
                    type="button"
                    onClick={this.Guardar}
                    className="btn btn-outline-primary"
                  >
                    Guardar
                  </button>
                  <button
                    type="button"
                    onClick={this.showQR}
                    className="btn btn-outline-primary"
                    style={{ marginLeft: "3px" }}
                  >
                    Generar QR
                  </button>
                  <button
                    type="button"
                    onClick={() =>
                      this.HuellaDigital(this.state.id_colaborador)
                    }
                    className="btn btn-outline-primary"
                    style={{ marginLeft: "3px" }}
                  >
                    Registrar Huella
                  </button>
                  <button
                    onClick={() => this.props.Regresar()}
                    type="button"
                    className="btn btn-outline-primary"
                    style={{ marginLeft: "3px" }}
                  >
                    Cancelar
                  </button>
                </div>
              </div>

              <Dialog
                header="Lista de Responsables"
                visible={this.state.visible_responsable}
                footer={footer}
                width="500px"
                minY={70}
                onHide={this.cerrarConfirmacion}
                maximizable={true}
              >
                <BootstrapTable
                  data={this.state.responsables}
                  hover={true}
                  pagination={true}
                  options={options}
                  selectRow={selectRowProp}
                >
                  <TableHeaderColumn
                    width={"15%"}
                    dataField="nro"
                    isKey={true}
                    dataAlign="center"
                    headerAlign="center"
                    dataSort={true}
                  >
                    No.
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="nombres_apellidos"
                    tdStyle={{ whiteSpace: "normal" }}
                    headerAlign="center"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Apellidos Nombres
                  </TableHeaderColumn>
                </BootstrapTable>
              </Dialog>

              <Dialog
                header="Generación de QR"
                visible={this.state.visible_qr}
                width="800px"
                onHide={this.onHideQR}
              >
                <div>
                  <Card className="ui-card-shadow">
                    <div>
                      <div className="row">
                        <div className="col-xs-12 col-md-6">
                          <div className="ui-card-shadow">
                            <b>Información Colaborador</b>
                            <br />
                            <br />
                            <h6 className="text-gray-700">
                              <b>No. de Identificación: </b>{" "}
                              {this.state.usuario != null
                                ? this.state.usuario.numero_identificacion
                                : ""}
                            </h6>
                            <h6 className="text-gray-700">
                              <b> Apellidos Nombres: </b>{" "}
                              {this.state.usuario != null
                                ? this.state.usuario.nombres_apellidos
                                : ""}
                            </h6>
                            <h6 className="text-gray-700">
                              <b>Destino: </b>{" "}
                              {this.state.usuario != null
                                ? this.state.usuario.nombreestancia
                                : ""}
                            </h6>
                            <h6 className="text-gray-700">
                              <b>Servicios: </b>{" "}
                              {this.state.usuario != null
                                ? this.state.usuario.serviciosvigentes
                                : ""}
                            </h6>
                            <h6 className="text-gray-700">
                              <b>Tiene Reservas Activas: </b>{" "}
                              {this.state.usuario != null
                                ? this.state.usuario.tienereservaactiva
                                : ""}
                            </h6>
                          </div>
                          <br />
                          <div className="ui-card-shadow">
                            <b>Permite Validación por Cédula:</b>{" "}
                            <Checkbox
                              checked={this.state.checked}
                              onChange={this.permitirvalidacioncedula}
                            />
                          </div>
                          <br />
                          <div>
                            <h6 className="text-gray-700">
                              <b>Fecha de Vigencia del QR: </b>{" "}
                              {this.state.usuario != null
                                ? this.state.usuario.fechavigenciacolaboradorqr
                                : ""}
                            </h6>
                          </div>
                        </div>

                        <div className="col-xs-12 col-md-4">
                          <QRCode
                            value={this.state.EncryptedData}
                            size={260}
                            id="FPImage1"
                          />

                          <div className="row">
                            <button
                              onClick={() => this.onHideQR()}
                              type="button"
                              className="btn btn-outline-primary"
                              style={{ marginLeft: "3px" }}
                            >
                              {" "}
                              Cancelar
                            </button>
                          </div>
                        </div>
                      </div>
                    </div>
                  </Card>
                </div>
              </Dialog>
            </div>
          </div>
          {/* </form> */}
        </BlockUi>
      </div>
    );
  }

  titulo() {
    document.getElementById("page_title").remove();
    document.getElementById("btntoolbar").remove();

    var a = document.createElement("i");
    a.setAttribute("class", "fa fa-unlock-alt");
    // a.innerHTML = ' Registro Ficha de Colaborador Externo';
    document.getElementsByClassName("card-header")[0].appendChild(a);

    var p = document.createElement("p");
    p.innerHTML = " Administración de Visitas de Colaborador Externo";
    p.style.display = "inline-block";
    document.getElementsByClassName("card-header")[0].appendChild(p);
  }

  ConsultaUsuarioExterno() {
    axios
      .post("/RRHH/Colaboradores/GetUsuarioExternoApi/", {
        id: sessionStorage.getItem("id_usuario_externo_visita"),
      })
      .then((response) => {
        console.log("consulta col", response.data);
        this.setState({
          usuario: response.data,
          id_colaborador: response.data.Id,
          tipo_identificacion: response.data.nombre_identificacion,
          nro_identificacion: response.data.numero_identificacion,
          nombres_apellidos: response.data.nombres_apellidos,
          DataColaborador: response.data.empleado_id_sap,
          checked: response.data.validacion_cedula,
          EncryptedData: response.data.numero_identificacion,
        });
        if (response.data.visita != null) {
          this.CargaVisita(
            response.data.visita,
            response.data.catalogo_destino_estancia_id
          );
        
          this.setState({ loading: false },  this.ConsultaServiciosColaborador());
        } else {
          this.setState({ loading: false });
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }

  CargaVisita(visita, destino) {
    this.setState({
      fecha_desde: moment(visita.fecha_desde).format("YYYY-MM-DD"),
      fecha_hasta: moment(visita.fecha_hasta).format("YYYY-MM-DD"),
      motivo: visita.motivo,
      destino: destino,
      responsable:visita.ColaboradoresResponsable!=null? visita.ColaboradoresResponsable.nombres_apellidos:"",
      id_responsable: visita.colaborador_responsable_id,
      id_visita: visita.Id,
      empresa: visita.empresa,
      loading: false,
    });
  }

  ConsultaServiciosColaborador() {
    this.setState({ loading: true });
    axios
      .post("/RRHH/Colaboradores/GetServiciosColaboradorApi/", {
        Id: this.state.id_colaborador,
      })
      .then((response) => {
        console.log("ConsultaServiciosColaborador", response.data);
        console.log(response.data.length);
        this.setState({
          serviciosConsulta: response.data,
        });
        if (response.data.length > 0) {
          response.data.forEach((e) => {
            if (e.Catalogo.codigo == "SALMUERZO") {
              this.ConsultaComidas(e.Id);
              this.setState({
                alimentacion: true,
              });
            } else if (e.Catalogo.codigo == "STRASPORTE") {
              this.ConsultaMovilizaciones(e.Id);
              this.setState({
                movilizacion: true,
              });
            } else if (e.Catalogo.codigo == "SLAVANDERIA") {
              this.setState({
                lavanderia: true,
              });
            } else if (e.Catalogo.codigo == "SHOSPEDAJE") {
              this.setState({
                alojamiento: true,
              });
            }
          });
          this.setState({ loading: false });
        }
        this.setState({ loading: false });
      })
      .catch((error) => {
        console.log(error);
        this.setState({ loading: false });
      });
  }

  ConsultaComidas(Id) {
    this.setState({ loading: true });
    axios
      .post("/RRHH/Colaboradores/GetComidasApi/", { id: Id })
      .then((response) => {
        // console.log('comidas', response.data)
        this.setState({
          comidas: response.data,
        });
        this.cargarDatosServicios();
      })
      .catch((error) => {
        console.log(error);
      });
  }

  ConsultaMovilizaciones(Id) {
    axios
      .post("/RRHH/Colaboradores/GetMovilizacionApi/", { id: Id })
      .then((response) => {
        console.log("movilizacion", response.data);
        this.setState({
          movilizacionConsulta: response.data,
        });
        // console.log(this.state.movilizacionConsulta )
        if (this.state.movilizacionConsulta != null) {
          console.log(
            this.state.movilizacionConsulta.catalogo_tipo_movilizacion_id > 0
          );
          if (
            this.state.movilizacionConsulta.catalogo_tipo_movilizacion_id > 0
          ) {
            this.setState({
              tipo_movilizacion: this.state.movilizacionConsulta
                .catalogo_tipo_movilizacion_id,
            });
            // this.state.tipo_movilizacion = this.state.movilizacionConsulta.catalogo_tipo_movilizacion_id;
          }
        }
        // console.log('asd',this.state.movilizacionConsulta.catalogo_tipo_movilizacion_id)
      })
      .catch((error) => {
        console.log(error);
      });
  }

  cargarDatosServicios() {
    let comidas = this.state.comidas.slice();
    let ids = [];
    comidas.forEach((e) => {
      var c = this.state.tiposComidas.filter(
        (c) => c.Id === e.tipo_alimentacion_id
      );
      ids.push(c[0]);
    });

    this.setState({
      tipo_alimentacion: (this.state.tipo_alimentacion = ids.slice()),
      loading: false,
    });
  }

  DatosEnvio(row, isSelected, e) {
    console.log(row);
    this.setState({
      selected_responsable: row,
    });
  }

  AsignarResponsable() {
    console.log("AsignarResponsable");
    console.log(this.state.selected_responsable);
    if (this.state.selected_responsable.length != []) {
      this.setState({
        id_responsable: this.state.selected_responsable.Id,
        responsable: this.state.selected_responsable.nombres_apellidos,
      });
      this.cerrarConfirmacion();
    } else {
      this.props.warnMessage("Seleccione un Responsable");
    }
  }

  handleValidation() {
    let errors = {};
    this.state.formIsValid = true;

    if (!this.state.fecha_desde) {
      this.state.formIsValid = false;
      errors["fecha_desde"] = "El campo Fecha Desde es obligatorio.";
    } else {
      var fecha = moment().format("YYYY-MM-DD");
      if (this.state.id_visita == "" && this.state.fecha_desde < fecha) {
        this.state.formIsValid = false;
        errors["fecha_desde"] =
          "La fecha ingresada no puede ser menor a la fecha actual.";
      }
      if (this.state.fecha_desde > this.state.fecha_hasta) {
        this.state.formIsValid = false;
        errors["fecha_desde"] =
          "La fecha ingresada no puede ser mayor a la Fecha Hasta.";
      }
    }

    if (!this.state.fecha_hasta) {
      this.state.formIsValid = false;
      errors["fecha_hasta"] = "El campo Fecha Hasta es obligatorio.";
    } else {
      var fecha = moment().format("YYYY-MM-DD");
      if (this.state.fecha_hasta < fecha) {
        this.state.formIsValid = false;
        errors["fecha_hasta"] =
          "La fecha ingresada no puede ser menor a la fecha actual";
      }
      if (this.state.fecha_hasta < this.state.fecha_desde) {
        this.state.formIsValid = false;
        errors["fecha_hasta"] =
          "La fecha ingresada no puede ser menor a Fecha Desde";
      }
    }

    if (!this.state.motivo) {
      this.state.formIsValid = false;
      errors["motivo"] = "El campo Motivo es obligatorio.";
    }
    if (!this.state.empresa) {
      this.state.formIsValid = false;
      errors["empresa"] = "El campo Empresa es obligatorio.";
    }
    if (!this.state.responsable) {
      this.state.formIsValid = false;
      errors["responsable"] = "El campo Responsable es obligatorio.";
    }

    if (this.state.alimentacion == true) {
      if (this.state.tipo_alimentacion.length == 0) {
        this.state.formIsValid = false;
        errors["tipo_alimentacion"] =
          "El campo Tipo de Alimentación es obligatorio.";
      }
    }
    if (this.state.movilizacion == true) {
      if (!this.state.tipo_movilizacion) {
        this.state.formIsValid = false;
        errors["tipo_movilizacion"] =
          "El campo Tipo de Movilización es obligatorio.";
      }
    }

    this.setState({ errores: errors });

    return this.state.formIsValid;
  }

  Guardar() {
    this.handleValidation();
    if (this.state.formIsValid == true) {
      console.log("true");
      this.arrayServicios();
      console.log("servicios_colaborador", this.state.servicios_colaborador);
      console.log("comidas_col", this.state.comidas_col);

      this.setState({ loading: true });
      if (this.state.id_visita == "") {
        axios
          .post("/RRHH/ColaboradoresVisita/CreateApiAsync/", {
            ColaboradoresId: this.state.id_colaborador,
            fecha_desde: this.state.fecha_desde,
            fecha_hasta: this.state.fecha_hasta,
            colaborador_responsable_id: this.state.id_responsable,
            motivo: this.state.motivo,
            estado: "ACTIVO",
            empresa: this.state.empresa,
            servicios: this.state.servicios_colaborador,
            idComidas: this.state.comidas_col,
            movilizacion: this.state.tipo_movilizacion,
          })
          .then((response) => {
            console.log("Guardar", response.data);
            this.GuardarServicios();
            setTimeout(() => {
              this.ConsultaUsuarioExterno();
            }, 1000);
           
         
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error("Algo salió mal.", "Error");
            this.setState({ loading: false });
          });
      } else {
        axios
          .post("/RRHH/ColaboradoresVisita/EditApiAsync/", {
            ColaboradoresId: this.state.id_colaborador,
            fecha_desde: this.state.fecha_desde,
            fecha_hasta: this.state.fecha_hasta,
            colaborador_responsable_id: this.state.id_responsable,
            motivo: this.state.motivo,
            empresa: this.state.empresa,
            Id: this.state.id_visita,
          })
          .then((response) => {
            console.log("Guardar", response.data);
            this.GuardarServicios();
            setTimeout(() => {
              this.ConsultaUsuarioExterno();
            }, 1000);
        
           
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error("Algo salió mal.", "Error");
            this.setState({ loading: false });
          });
      }
    } else {
      this.props.warnMessage(
        "Se ha encontrado errores, por favor revisar el formulario"
      );
    }
  }

  GuardarServicios() {
    if (this.state.servicios_colaborador.length > 0) {
      axios
        .post("/RRHH/Colaboradores/CreateServiciosApi/", {
          servicios: this.state.servicios_colaborador,
          idComidas: this.state.comidas_col,
          movilizacion:
            this.state.tipo_movilizacion != ""
              ? this.state.tipo_movilizacion
              : 0
        })
        .then((response) => {
          this.setState({ loading: false });
          console.log(response);
          abp.notify.success("Visita Guardada!", "Aviso");
          // this.props.Regresar();
        })
        .catch((error) => {
          this.setState({ loading: false });
          console.log(error);
          abp.notify.error("Algo salió mal.", "Error");
        });
    } else {
      abp.notify.success("Visita Guardada!", "Aviso");
      this.props.Regresar();
    }
  }

  arrayServicios() {
    let arrayServicios = [];
    // this.state.alimentacion ? servicios.push()

    if (this.state.alimentacion == true) {
      let s = {};
      var alimentacion = this.state.tiposServicios.filter(
        (c) => c.codigo == "SALMUERZO"
      );

      s.ColaboradoresId = this.state.id_colaborador;
      s.ServicioId = alimentacion[0].Id;
      s.nombre = "alimentacion";
      console.log("serviciosConsulta", this.state.serviciosConsulta);
      var a = this.state.serviciosConsulta.filter(
        (c) => c.Catalogo.codigo == "SALMUERZO"
      );
      // console.log('alimentacion',a)
      if (a.length > 0) {
        s.Id = a[0].Id;
        s.Servicio = a[0].Servicio;
      }
      // console.log('s',s)
      arrayServicios.push(s);
      this.arrayComidas();
    }
    if (this.state.alojamiento == true) {
      let s = {};
      var alojamiento = this.state.tiposServicios.filter(
        (c) => c.codigo == "SHOSPEDAJE"
      );
      // console.log('alojamiento',alojamiento)
      s.ColaboradoresId = this.state.id_colaborador;
      s.ServicioId = alojamiento[0].Id;
      s.nombre = "alojamiento";

      var a = this.state.serviciosConsulta.filter(
        (c) => c.Catalogo.codigo == "SHOSPEDAJE"
      );
      if (a.length > 0) {
        s.Id = a[0].Id;
        s.Servicio = a[0].Servicio;
      }
      // console.log('s',s)
      arrayServicios.push(s);
    }
    if (this.state.movilizacion == true) {
      let s = {};
      var movilizacion = this.state.tiposServicios.filter(
        (c) => c.codigo == "STRASPORTE"
      );
      // console.log('movilizacion',movilizacion)
      s.ColaboradoresId = this.state.id_colaborador;
      s.ServicioId = movilizacion[0].Id;
      s.nombre = "movilizacion";

      var a = this.state.serviciosConsulta.filter(
        (c) => c.Catalogo.codigo == "STRASPORTE"
      );
      if (a.length > 0) {
        s.Id = a[0].Id;
        s.Servicio = a[0].Servicio;
      }
      // console.log('s',s)
      arrayServicios.push(s);
    }
    if (this.state.lavanderia == true) {
      let s = {};
      var lavanderia = this.state.tiposServicios.filter(
        (c) => c.codigo == "SLAVANDERIA"
      );
      console.log('lavanderia', lavanderia)
      s.ColaboradoresId = this.state.id_colaborador;
      if (lavanderia[0] != null && lavanderia != undefined) {
        s.ServicioId = lavanderia[0].Id;
      }
      s.nombre = "lavanderia";

      var a = this.state.serviciosConsulta.filter(
        (c) => c.Catalogo.codigo == "SLAVANDERIA"
      );
      if (a.length > 0) {
        s.Id = a[0].Id;
        s.Servicio = a[0].Servicio;
      }
      // console.log('s',s)
      arrayServicios.push(s);
    }
    // console.log('arrayServicios',arrayServicios)
    this.state.servicios_colaborador = arrayServicios;
  }

  arrayComidas() {
    let comidas = this.state.tipo_alimentacion.slice();
    let ids = [];
    comidas.forEach((element) => {
      ids.push(element.Id);
    });
    this.state.comidas_col = ids.slice();
    // console.log('comidas_col', this.state.comidas_col);
  }
  permitirvalidacioncedula(e) {
    this.setState({ loadingqr: true });
    if (this.state.id_colaborador > 0) {
      axios
        .post("/RRHH/ColaboradoresVisita/CreateValidacionCedula/", {
          id: this.state.id_colaborador,
        })
        .then((response) => {
          if (response.data == "OK") {
            this.setState({ checked: e.checked, loadingqr: false });
            abp.notify.success("Validación por cédula actualizado", "Aviso");
          } else {
            this.setState({ loadingqr: false });
            abp.notify.error("Debe seleccionar un colaborador", "Error");
          }
        })
        .catch((error) => {
          console.log(error);
          this.setState({ loadingqr: false });
          abp.notify.error("Algo salió mal.", "Error");
        });
    } else {
      abp.notify.error(
        "Ocurrió un error al actualizar campo validación",
        "Error"
      );
    }
  }
  GetCatalogos(d) {
    let codigos = [];
    codigos = ["TIPOCOMIDA", "TIPODEMOVILIZACION", "SERVICIO"];

    axios
      .post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
      .then((response) => {
        //console.log(response.data);
        this.cargarCatalogos(response.data);
        this.setState({ loading: false });
      })
      .catch((error) => {
        console.log(error);
        this.setState({ loading: false });
      });
  }

  cargarCatalogos(data) {
    data.forEach((e) => {
      var catalogo = JSON.parse(e);
      var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;

      switch (codigoCatalogo) {
        case "TIPOCOMIDA":
          this.setState({ tiposComidas: catalogo });
          return;
        case "TIPODEMOVILIZACION":
          this.setState({ tiposMovilizacion: catalogo });
          this.getFormSelectTipoMovilizacion();
          return;
        case "SERVICIO":
          this.setState({ tiposServicios: catalogo });
          return;
      }
    });
  }

  getFormSelectTipoMovilizacion() {
    return this.state.tiposMovilizacion.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  HuellaDigital(id) {
    sessionStorage.setItem("id_colaborador", id);
    sessionStorage.setItem("regresar_huella", "visita");
    return (window.location.href = "/RRHH/Colaboradores/Huella/");
  }

  abrirConfirmacion() {
    this.setState({ visible_responsable: true });
  }

  cerrarConfirmacion() {
    this.setState({ visible_responsable: false });
  }

  showQR() {
    this.setState({ visible_qr: true }, this.MostrarDialogQrE);
  }

  onHideQR() {
    this.setState({ visible_qr: false });
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  handleChangeUpperCase(event) {
    this.setState({ [event.target.name]: event.target.value.toUpperCase() });
  }

  handleInputChange(event) {
    const target = event.target;
    const value = target.type === "checkbox" ? target.checked : target.value;
    const name = target.name;

    this.setState({
      [name]: value,
    });
  }
  DescargarQR() {
    if (this.state.codigo_qr) {
      var element = document.createElement("a");
      element.setAttribute("href", document.getElementById("FPImage1").src);
      element.setAttribute(
        "download",
        this.state.iseleccionado != null
          ? this.state.iseleccionado.nombres_apellidos + ".jpg"
          : "QR.jpg"
      );

      element.style.display = "none";
      document.body.appendChild(element);

      element.click();

      document.body.removeChild(element);
    } else {
      abp.notify.error("Se debe generar el QR!", "Error");
    }
  }
}
