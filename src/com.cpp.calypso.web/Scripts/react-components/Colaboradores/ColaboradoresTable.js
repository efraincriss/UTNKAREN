import React from "react";
import axios from "axios";
import moment, { now } from "moment";
import BlockUi from "react-block-ui";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Button } from "primereact/components/button/Button";

import { ESTADO_ENVIADO_SAP } from "./Codigos";

import Fotografia from "./Fotografia";
import Alta from "./Altas/Alta";
import AltaMasiva from "./Altas/AltaMasiva";

//Generación de Qr

import { Checkbox } from "primereact-v2/checkbox";
import { Card } from "primereact-v2/card";

export default class ColaboradoresTable extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      Id: "",
      nro: "",
      tipo_identificacion: "",
      nro_identificacion: "",
      apellidos: "",
      nombres: "",
      nombre_rol: "",
      visible: false,
      visible_foto: false,
      key_form: 23423,
      colaborador: [],
      colaboradoresInfoCompleta: [],
      envio_Manual: false,
      loading: false,
      filas: [],
      visible_carga_masiva: false,
      archivo: [],
      lista_errores: [],
      visible_alta: false,
      fecha_ingreso: "",
      empleado_id_sap: "",
      visible_alta_masiva: false,

      /*Generación QR */
      loadingqr: false,

      QrDialog: false,
      iseleccionado: null,

      //VALIDACION CÉDULA
      checked: false,
    };

    this.childFoto = React.createRef();
    this.generateButton = this.generateButton.bind(this);
    this.onHide = this.onHide.bind(this);
    this.showForm = this.showForm.bind(this);
    this.generateButton = this.generateButton.bind(this);
    this.onHideFoto = this.onHideFoto.bind(this);
    this.showFormFoto = this.showFormFoto.bind(this);
    this.Fotografia = this.Fotografia.bind(this);
    this.LoadColaborador = this.LoadColaborador.bind(this);
    this.Servicios = this.Servicios.bind(this);
    this.Delete = this.Delete.bind(this);

    this.onHideEnvioManual = this.onHideEnvioManual.bind(this);
    this.showFormEnvioManual = this.showFormEnvioManual.bind(this);
    this.onHideCargaMasiva = this.onHideCargaMasiva.bind(this);
    this.showFormCargaMasiva = this.showFormCargaMasiva.bind(this);
    this.onHideAlta = this.onHideAlta.bind(this);
    this.showFormAlta = this.showFormAlta.bind(this);
    this.onHideAltaMasiva = this.onHideAltaMasiva.bind(this);
    this.showFormAltaMasiva = this.showFormAltaMasiva.bind(this);

    this.GetColaboradores = this.GetColaboradores.bind(this);

    this.click = this.click.bind(this);
    this.EnvioManual = this.EnvioManual.bind(this);
    this.DatosEnvio = this.DatosEnvio.bind(this);
    this.onSelectAll = this.onSelectAll.bind(this);

    this.onBasicUploadAuto = this.onBasicUploadAuto.bind(this);
    this.EnvioCargaMasiva = this.EnvioCargaMasiva.bind(this);
    this.ListaErrores = this.ListaErrores.bind(this);
    this.clearCarga = this.clearCarga.bind(this);

    /* Generación de QR */
    this.validacionCedula = this.validacionCedula.bind(this);
    this.OcultarDialogQr = this.OcultarDialogQr.bind(this);
    this.MostrarDialogQr = this.MostrarDialogQr.bind(this);
    this.CrearQR = this.CrearQR.bind(this);
    this.DescargarQR = this.DescargarQR.bind(this);
    this.permitirvalidacioncedula = this.permitirvalidacioncedula.bind(this);

    this.onChangeValue = this.onChangeValue.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }

  componentDidMount() {
    this.click();
    this.GetColaboradores();
  }

  validacionCedula(cell, row) {
    return (
      <div>
        <label>{row.validacion_cedula == true ? "SI" : "NO"}</label>
      </div>
    );
  }

  render() {
    const tableStyle = { whiteSpace: "normal", fontSize: "11px" };
    const footer = (
      <div>
        <Button
          label="Enviar"
          icon="pi pi-check"
          onClick={() => this.EnvioManual()}
        />
        <Button
          label="Cancelar"
          icon="pi pi-times"
          onClick={() => this.onHideEnvioManual()}
          className="p-button-secondary"
        />
      </div>
    );
    const footerCarga = (
      <div>
        <Button
          label="Enviar"
          icon="pi pi-check"
          onClick={() => this.EnvioCargaMasiva()}
        />
        <Button
          label="Cancelar"
          icon="pi pi-times"
          onClick={() => this.onHideCargaMasiva()}
          className="p-button-secondary"
        />
      </div>
    );
    const options = {
      withoutNoDataText: true,
    };
    const selectRowProp = {
      mode: "checkbox",
      clickToSelect: true,
      onSelect: this.DatosEnvio,
      onSelectAll: this.onSelectAll,
    };
    return (
      <div>
        <BlockUi tag="div" blocking={this.props.loading || this.state.loading}>
          <BootstrapTable
            data={this.props.data}
            hover={true}
            pagination={true}
            options={options}
          >
            <TableHeaderColumn
              width={"5%"}
              dataField="nro"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              isKey={true}
              dataAlign="center"
              headerAlign="center"
              dataSort={true}
            >
              No.
            </TableHeaderColumn>
            <TableHeaderColumn
              width={"6%"}
              dataField="numero_legajo_temporal"
              dataFormat={this.formatLegajo.bind(this)}
              dataAlign="center"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              No. Legajo
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="nombre_identificacion"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Tipo Identificación
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="numero_identificacion"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              No. Identificación
            </TableHeaderColumn>
            <TableHeaderColumn
              /*width={'20%'}*/ dataField="apellidos_nombres"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Apellidos
            </TableHeaderColumn>
            <TableHeaderColumn
              /*width={'20%'}*/ dataField="nombres"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Nombres
            </TableHeaderColumn>
            <TableHeaderColumn
              /*width={'20%'}*/ dataField="fecha_ingreso"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              dataFormat={this.formatFechaIngreso.bind(this)}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Fecha Ingreso
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="nombre_grupo_personal"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Agrupación para Requisitos
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="nombre_destino"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Destino
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="estado"
              headerAlign="center"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Estado
            </TableHeaderColumn>
            <TableHeaderColumn
              width={"5%"}
              dataField="numeroHuellas"
              headerAlign="center"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Nº Huellas
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Operaciones"
              headerAlign="center"
              tdStyle={tableStyle}
              thStyle={tableStyle}
              width={"20%"}
              dataFormat={this.generateButton.bind(this)}
            >
              Opciones
            </TableHeaderColumn>
          </BootstrapTable>
          {/* <button type="button" onClick={() => this.showFormEnvioManual()}>click</button> */}
        </BlockUi>

        <Dialog
          header="Registrar Fotografía"
          visible={this.state.visible_foto}
          width="850px"
          modal={true}
          onHide={this.onHideFoto}
        >
          <Fotografia
            ref={this.childFoto}
            onHide={this.onHideFoto}
            Id={this.state.Id}
            tipo_identificacion={this.state.tipo_identificacion}
            nro_identificacion={this.state.nro_identificacion}
            nombres_apellidos={this.state.nombres}
            colaborador={this.state.colaborador}
          />
        </Dialog>

        <Dialog
          header="Envío Manual Alta de Colaboradores"
          visible={this.state.visible_envio_Manual}
          width="1000px"
          modal={true}
          footer={footer}
          onHide={this.onHideEnvioManual}
        >
          <BootstrapTable
            data={this.state.colaboradoresInfoCompleta}
            hover={true}
            pagination={true}
            options={options}
            selectRow={selectRowProp}
          >
            <TableHeaderColumn
              width={"6%"}
              dataField="nro"
              isKey={true}
              dataAlign="center"
              headerAlign="center"
              dataSort={true}
            >
              No.
            </TableHeaderColumn>
            <TableHeaderColumn
              width={"6%"}
              dataField="numero_legajo_temporal"
              dataFormat={this.formatLegajo.bind(this)}
              dataAlign="center"
              thStyle={{ whiteSpace: "normal" }}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              No. Legajo
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="nombre_identificacion"
              thStyle={{ whiteSpace: "normal" }}
              tdStyle={{ whiteSpace: "normal" }}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Tipo Identificación
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="numero_identificacion"
              thStyle={{ whiteSpace: "normal" }}
              tdStyle={{ whiteSpace: "normal" }}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              No. Identificación
            </TableHeaderColumn>
            <TableHeaderColumn
              width={"20%"}
              dataField="apellidos_nombres"
              thStyle={{ whiteSpace: "normal" }}
              tdStyle={{ whiteSpace: "normal" }}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Apellidos
            </TableHeaderColumn>
            <TableHeaderColumn
              width={"20%"}
              dataField="nombres"
              thStyle={{ whiteSpace: "normal" }}
              tdStyle={{ whiteSpace: "normal" }}
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Nombres
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="estado"
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Estado
            </TableHeaderColumn>
            {/* <TableHeaderColumn dataField='Operaciones' headerAlign="center" width={'17%'} dataFormat={this.generateButton.bind(this)}>Opciones</TableHeaderColumn> */}
          </BootstrapTable>
        </Dialog>

        <Dialog
          header="Carga Masiva de Información Básica"
          visible={this.state.visible_carga_masiva}
          width="850px"
          modal={true}
          footer={footerCarga}
          onHide={this.onHideCargaMasiva}
        >
          <div>
            <label htmlFor="label">
              * Archivo de Carga de Información Básica:{" "}
            </label>
            <br />
            <input
              type="file"
              id="fileCargaMasiva"
              accept=".xls,.xlsx"
              onChange={(e) => this.onBasicUploadAuto(e)}
              key={""}
            />
            <span style={{ color: "red", display: "inherit" }}></span>
          </div>
          <div>
            <br />
            {this.ListaErrores()}
          </div>
        </Dialog>

        <Dialog
          header="Datos de Alta"
          visible={this.state.visible_alta}
          width="850px"
          modal={true}
          onHide={this.onHideAlta}
        >
          <Alta /*ref={this.childFoto}*/
            onHide={this.onHideAlta}
            Id={this.state.Id}
            tipo_identificacion={this.state.tipo_identificacion}
            nro_identificacion={this.state.nro_identificacion}
            nombres_apellidos={this.state.nombres}
            fecha_ingreso={this.state.fecha_ingreso}
            colaborador={this.state.colaborador}
            empleado_id_sap={this.state.empleado_id_sap}
            GetColaboradores={this.props.GetColaboradores}
          />
        </Dialog>

        <Dialog
          header="Alta Masiva de Colaboradores"
          visible={this.state.visible_alta_masiva}
          width="850px"
          modal={true}
          onHide={this.onHideAltaMasiva}
        >
          <AltaMasiva
            onHide={this.onHideAltaMasiva}
            GetColaboradores={this.props.GetColaboradores}
          />
        </Dialog>
        <Dialog
          header="Generación de QR"
          visible={this.state.QrDialog}
          style={{ width: "800PX" }}
          modal={true}
          onHide={this.OcultarDialogQr}
        >
          <BlockUi tag="div" blocking={this.state.loadingqr}>
            <div>
              <Card className="ui-card-shadow">
                <div>
                  <div className="row">
                    <div className="col-xs-12 col-md-6">
                      <Card className="ui-card-shadow">
                        <b>Información Colaborador</b>
                        <br />
                        <br />
                        <h6 className="text-gray-700">
                          <b>No. de Identificación: </b>{" "}
                          {this.state.iseleccionado != null
                            ? this.state.iseleccionado.numero_identificacion
                            : ""}
                        </h6>
                        <h6 className="text-gray-700">
                          <b> Apellidos Nombres: </b>{" "}
                          {this.state.iseleccionado != null
                            ? this.state.iseleccionado.nombres_apellidos
                            : ""}
                        </h6>
                        <h6 className="text-gray-700">
                          <b>Destino: </b>{" "}
                          {this.state.iseleccionado != null
                            ? this.state.iseleccionado.nombreestancia
                            : ""}
                        </h6>
                        <h6 className="text-gray-700">
                          <b>Servicios: </b>{" "}
                          {this.state.iseleccionado != null
                            ? this.state.iseleccionado.serviciosvigentes
                            : ""}
                        </h6>
                        <h6 className="text-gray-700">
                          <b>Tiene Reservas Activas: </b>{" "}
                          {this.state.iseleccionado != null
                            ? this.state.iseleccionado.tienereservaactiva
                            : ""}
                        </h6>
                      </Card>
                      <br />
                      <Card className="ui-card-shadow">
                        <b>Permite Validación por Cédula:</b>{" "}
                        <Checkbox
                          checked={this.state.checked}
                          onChange={this.permitirvalidacioncedula}
                        />
                        <br />
                        <br />
                      </Card>
                      <br />
                      <Card className="ui-card-shadow">
                        <h6 className="text-gray-700">
                          <b>Fecha de Vigencia del QR: </b>{" "}
                          {this.state.iseleccionado != null
                            ? this.state.iseleccionado
                              .fechavigenciacolaboradorqr
                            : ""}
                        </h6>
                      </Card>
                    </div>
                    {/*
                                       <div className="col-xs-12 col-md-4">


                                            <img href="#" id="FPImage1" height="350" width="350" /><br></br>


                                            <div className="row" style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                                                <button
                                                    onClick={() => this.DescargarQR()}
                                                    type="button"
                                                    className="btn btn-outline-primary"
                                                    style={{ marginLeft: "3px" }}
                                                >
                                                    Imprimir QR
                         </button>
                                                <button
                                                    onClick={() => this.OcultarDialogQr()}
                                                    type="button"
                                                    className="btn btn-outline-primary"
                                                    style={{ marginLeft: "3px" }}
                                                > Cancelar
                       </button>
                                            </div>
                                        </div>
                                    */}
                  </div>
                </div>
              </Card>
            </div>
          </BlockUi>
        </Dialog>
      </div>
    );
  }
  /* QR MEtodos */
  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  permitirvalidacioncedula(e) {
    console.log(e);
    this.setState({ loadingqr: true });
    if (this.state.iseleccionado.Id > 0) {
      axios
        .post("/RRHH/Colaboradores/CreateValidacionCedula/", {
          id: this.state.iseleccionado.Id,
        })
        .then((response) => {
          if (response.data == "OK") {
            this.setState({ checked: e.checked, loadingqr: false });
            this.props.showSuccess("Validación por cédula actualizado");
          } else {
            this.setState({ loadingqr: false });
            this.props.showWarning("Debe seleccionar un colaborador");
          }
        })
        .catch((error) => {
          console.log(error);
          this.setState({ loadingqr: false });
          this.warnMessage("Algo salio mal!");
        });
    } else {
      this.props.showWarn("Ocurrió un error al actualizar campo validación");
    }
  }

  Guardar() {
    if (this.state.codigo_qr) {
      axios
        .post("/RRHH/Colaboradores/UpdateColaboradorQR/", {
          id: sessionStorage.getItem("id_colaborador"),
          validacion: this.state.permiteValidacion,
        })
        .then((response) => {
          this.showSuccess("Colaborador guardado con exito!");
          this.Regresar();
        })
        .catch((error) => {
          console.log(error);
          this.warnMessage("Algo salio mal!");
        });
    } else {
      this.warnMessage("Se debe generar el QR antes de guardar!");
    }
  }

  CrearQR(row) {
    console.log(row.Id);
    axios
      .post("/RRHH/Colaboradores/CreateQR/", {
        id: row.Id,
      })
      .then((response) => {
        console.log(response.data);
        if (response.data[0] == "s_s") {
          console.log("El colaborador no tiene servicios activos");
          this.props.showWarning("El colaborador no tiene servicios activos");
          this.setState({
            QrDialog: true,
            loadingqr: false,
            codigo_qr: response.data[1],
            display: "block",
          });
          this.props.unlockScreen();
          /* Se carga la imagen */

          document.getElementById("FPImage1").src =
            "data:image/jpg;base64," + response.data[1];
          this.props.unlockScreen();
        } else if (response.data[0] == "s_r") {
          console.log("El colaborador no tiene reservas activas");
          this.props.showWarning("El colaborador no tiene reservas activas");
          this.setState({
            QrDialog: true,
            loadingqr: false,
            codigo_qr: response.data[1],
            display: "block",
          });
          this.props.unlockScreen();
          /* Se carga la imagen */

          document.getElementById("FPImage1").src =
            "data:image/jpg;base64," + response.data[1];
          this.props.unlockScreen();
        } else {
          console.log("OK");
          this.setState({
            QrDialog: true,
            loadingqr: false,
            codigo_qr: response.data[1],
            display: "block",
          });
          this.props.unlockScreen();
          /* Se carga la imagen */

          document.getElementById("FPImage1").src =
            "data:image/jpg;base64," + response.data[1];
        }
      })
      .catch((error) => {
        console.log(error);
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
      this.warnMessage("Se debe generar el QR!");
    }
  }

  MostrarDialogQr(row) {
    this.props.blockScreen();
    console.log(row);
    this.CrearQR(row);
    this.setState({
      iseleccionado: row,
      loadingqr: true,
      checked: row.validacion_cedula,
      QrDialog: true,
    });
  }
  OcultarDialogQr() {
    this.setState({ QrDialog: false, iseleccionado: null });
    document.getElementById("FPImage1").src = "data:image/jpg;base64,";
  }
  /********* */

  clearCarga() {
    this.setState({
      lista_errores: [],
      archivo: [],
    });
    document.getElementById("fileCargaMasiva").value = "";
  }

  onBasicUploadAuto(event) {
    console.log(event.target.files[0]);
    var file = event.target.files[0];
    var a = {};
    console.log("type", file.type);
    if (file != null) {
      if (file >= 2 * 1024 * 1024) {
        this.props.warnMessage("El archivo solo puede ser de máximo 2MB");
        this.setState({ archivo: a });
        document.getElementById("fileCargaMasiva").value = "";
        return;
      } else if (
        !file.type.match(
          "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        ) &&
        !file.type.match("application/vnd.ms-excel")
      ) {
        this.props.warnMessage("No puede subir archivos de ese formato");
        // event.target.files = new FileList();
        this.setState({ archivo: a });
        document.getElementById("fileCargaMasiva").value = "";
        return;
      } else {
        const formData = new FormData();
        // const formData = {};
        // formData.append('UploadedFile', event.files[0])
        formData["UploadedFile"] = file;
        //this.setState({blocking: true})
        const config = {
          headers: {
            "content-type": "multipart/form-data",
          },
        };
        a.file = formData;
        a.config = config;

        // this.state.archivo.push(a);
        this.setState({ archivo: file });
        console.log("formData", formData);

        this.props.successMessage("Archivo Procesado con Exito");
        console.log("this.state.archivo", this.state.archivo);
      }
    } else {
      console.log("error llamada");
    }
  }

  EnvioCargaMasiva() {
    console.log(this.state.archivo);
    if (this.state.archivo.length == 0) {
      this.props.warnMessage("Seleccione un archivo!");
    } else {
      this.setState({ loading: true });
      console.log("lleno");
      const config = { headers: { "content-type": "multipart/form-data" } };
      const formData = new FormData();
      formData.append("UploadedFile", this.state.archivo);

      axios
        .post("/RRHH/Colaboradores/GetExcelCargaMasiva/", formData, config)
        .then((response) => {
          console.log("DATA ", response.data);

          if (response.data.success) {
            this.props.successMessage("Colaboradores Guardados!");
            this.props.GetColaboradores();
            this.onHideCargaMasiva();
          } else {
            this.setState({ lista_errores: response.data.errores });
            this.props.warnMessage("Se encontraron errores en el archivo!");
            this.ListaErrores();
          }
          this.setState({ loading: false });
        })
        .catch((error) => {
          this.setState({ loading: false });
          console.log("ERROR", error);
        });
    }
  }

  ListaErrores() {
    return this.state.lista_errores.map((item) => {
      return <div className="alert alert-danger">{item}</div>;
    });
  }

  formatLegajo(cell, row) {
    // console.log(row.Id, row.numero_legajo_temporal, row)
    if (row.numero_legajo_temporal != null) {
      var length = row.numero_legajo_temporal.length;
      switch (length) {
        case 1:
          var numero = "0000" + row.numero_legajo_temporal;
          return numero;
        case 2:
          var numero = "000" + row.numero_legajo_temporal;
          return numero;
        case 3:
          var numero = "00" + row.numero_legajo_temporal;
          return numero;
        case 4:
          var numero = "0" + row.numero_legajo_temporal;
          return numero;
        case 5:
          var numero = row.numero_legajo_temporal;
          return numero;
        default:
          var numero = row.numero_legajo_temporal;
          return numero;
      }
    }
  }

  EnvioManual() {
    this.setState({ loading: true });
    console.log(this.state.filas);
    axios
      .post("/RRHH/Colaboradores/GetAltaManualApi/", { ids: this.state.filas })
      .then((response) => {
        console.log("Response,", response);
        this.setState({ loading: false });
        if (response.data == "OK") {
          this.props.successMessage("Excel generado");
          this.GetColaboradores();
          this.props.GetColaboradores();
          this.onHideEnvioManual();
        } else {
          abp.notify.error("Algo salió mal", "Error");
        }
      })
      .catch((error) => {
        console.log(error);
        this.setState({ loading: false });
      });
  }

  DatosEnvio(row, isSelected, e) {
    var select = this.state.filas.slice();

    if (isSelected == true) {
      select.push(row.Id);
    } else {
      var i = select.findIndex((c) => c == row.Id);
      if (i > -1) {
        select.splice(i, 1);
      }
    }
    console.log(select);
    this.setState({
      filas: select,
    });
  }

  onSelectAll(isSelected, rows) {
    var select = [];
    if (isSelected == true) {
      rows.forEach((e) => {
        select.push(e.Id);
      });
      this.setState({
        filas: select,
      });
    } else {
      this.setState({
        filas: select,
      });
    }
    console.log(select);
  }

  click() {
    //Boton Carga Masiva
    var carga = document.createElement("button");
    carga.setAttribute("class", "btn btn-primary");
    carga.setAttribute("style", " margin-left: 0.3em");
    carga.setAttribute("id", "carga_masiva");
    carga.setAttribute("type", "button");
    carga.style.marginRight = "3px";
    carga.addEventListener("click", (e) => this.showFormCargaMasiva());
    // a.attachEvent('onClick',this.showFormEnvioManual);
    carga.textContent = "Carga Información Básica";
    document.getElementById("btntoolbar").prepend(carga);

    //Boton Envio Manul
    var a = document.createElement("button");
    a.setAttribute("class", "btn btn-primary");
    a.setAttribute("style", " margin-left: 0.3em");
    a.setAttribute("id", "envio_manual");
    a.setAttribute("type", "button");
    a.addEventListener("click", (e) => this.showFormEnvioManual());
    // a.attachEvent('onClick',this.showFormEnvioManual);
    a.textContent = "Envío Manual a SAP";
    document.getElementById("btntoolbar").prepend(a);

    //Boton Alta Masiva
    var alta = document.createElement("button");
    alta.setAttribute("class", "btn btn-primary");
    alta.setAttribute("style", " margin-left: 0.3em");
    alta.setAttribute("id", "alta_masiva");
    alta.setAttribute("type", "button");
    alta.style.marginRight = "3px";
    alta.addEventListener("click", (e) => this.showFormAltaMasiva());
    // a.attachEvent('onClick',this.showFormEnvioManual);
    alta.textContent = "Registro Información SAP";
    document.getElementById("btntoolbar").prepend(alta);
  }

  generateButton(cell, row) {
    // console.log(row)
    return (
      <div>
        {row.estado == "ACTIVO" && (
          <button
            className="btn btn-outline-success btn-sm fa fa-file-word-o"
            onClick={() => this.props.showFormCertificado(row.Id)}
            style={{ marginLeft: "0.2em" }}
            data-toggle="tooltip"
            data-placement="top"
            title="Certificado de Trabajo"
          ></button>
        )}
        {row.estado == "INACTIVO" && (
          <>
            <button
              className="btn btn-outline-indigo btn-sm fa fa-thumbs-o-up"
              onClick={() => this.LoadColaboradorReingreso(row.Id)}
              style={{ marginLeft: "0.2em" }}
              data-toggle="tooltip"
              data-placement="top"
              title="Reingreso"
            ></button>
            <button
              className="btn btn-outline-success btn-sm fa fa-file-word-o"
              onClick={() => this.props.showFormCertificado(row.Id)}
              style={{ marginLeft: "0.2em" }}
              data-toggle="tooltip"
              data-placement="top"
              title="Certificado de Trabajo"
            ></button>
          </>
        )}
        {row.estado !== "INACTIVO" && (
          <button
            onClick={() => this.LoadColaborador(row.Id)}
            data-toggle="tooltip"
            data-placement="top"
            title="Editar"
            type="button"
            className="btn btn-outline-primary btn-sm fa fa-edit"
            style={{ marginLeft: "0.2em" }}
          ></button>

        )}
        {row.estado !== "ALTA ANULADA" && (
          <>
            <button
              onClick={() => this.HuellaDigital(row.Id)}
              data-toggle="tooltip"
              data-placement="top"
              title="Huella Digital"
              className="btn btn-outline-primary btn-sm fa fa-paw"
              style={{ marginLeft: "0.2em" }}
            ></button>
            <button
              onClick={() => this.Fotografia(row.Id)}
              data-toggle="tooltip"
              data-placement="top"
              title="Fotografía"
              className="btn btn-outline-primary btn-sm fa fa-photo"
              style={{ marginLeft: "0.2em" }}
            ></button>
          </>
        )}
        <>
        {(row.estado == "ACTIVO" || row.estado == "ENVIADO SAP" || row.estado == "TEMPORAL"|| row.estado == "INFORMACION COMPLETA" ) && (
        <button
            className="btn btn-outline-danger btn-sm fa fa-level-down"
            onClick={() => this.props.viewdisable(row.Id)}
           style={{ marginLeft: "0.2em" }}
            data-toggle="tooltip"
            data-placement="top"
            title="Anular Alta"
          ></button>)}
          <button
            onClick={() => {
              if (window.confirm("Estás seguro?")) this.Delete(row);
            }}
            data-toggle="tooltip"
            data-placement="top"
            title="Eliminar"
            className="btn btn-outline-danger btn-sm fa fa-trash"
            style={{ marginLeft: "0.2em" }}
          ></button>
          
          <button
            onClick={() => this.showFormAlta(row.Id)}
            data-toggle="tooltip"
            data-placement="top"
            title="Alta Colaborador"
            className="btn btn-outline-primary btn-sm fa fa-thumbs-up"
            style={{
              marginLeft: "0.2em",
              visibility: row.estado == ESTADO_ENVIADO_SAP ? "initial" : "hidden",
            }}
          ></button>
        
        </>
      </div>

    );
  }

  formatFechaIngreso(cell, row) {
    return moment(row.fecha_ingreso).format("DD-MM-YYYY");
  }

  LoadColaborador(id) {
    sessionStorage.setItem("id_colaborador", id);
    return (window.location.href = "/RRHH/Colaboradores/Edit/");
  }
  LoadColaboradorReingreso = (id) => {
    sessionStorage.setItem("id_colaborador", id);
    sessionStorage.setItem("esReingreso", true);
    return (window.location.href = "/RRHH/Colaboradores/Reingreso?id=" + id);
  };

  Servicios(id) {
    sessionStorage.setItem("id_colaborador", id);
    return (window.location.href = "/RRHH/Colaboradores/Servicios/");
  }

  Delete(row) {
    console.log(row);
    if (row != null && row.numeroHuellas == 0) {
      axios
        .post("/RRHH/Colaboradores/DeleteApiAsync/" + row.Id, {})
        .then((response) => {
          if (response.data == "OK") {
            this.props.successMessage("Colaborador Eliminado!");
            this.props.GetColaboradores();
          } else {
            this.props.warnMessage("Algo salio mal.");
          }
        })
        .catch((error) => {
          console.log(error);
        });
    } else {
      abp.notify.error(
        "No se puede Eliminar Colaborador tiene Huellas Registradas",
        "Error"
      );
    }
  }

  HuellaDigital(id) {
    sessionStorage.setItem("id_colaborador", id);
    return (window.location.href = "/RRHH/Colaboradores/Huella/");
  }

  Fotografia(id) {
    axios
      .post("/RRHH/Colaboradores/GetColaboradorApi/" + id, {})
      .then((response) => {
        console.log(response);
        this.setState(
          {
            Id: id,
            tipo_identificacion: response.data.nombre_identificacion,
            nro_identificacion: response.data.numero_identificacion,
            nombres: response.data.nombres_apellidos,
            key_form: Math.random(),
            colaborador: response.data,
          },
          this.showFormFoto
        );
      })
      .catch((error) => {
        this.props.warnMessage("");
      });
  }

  GetColaboradores() {
    this.setState({ loading: true });
    axios
      .post("/RRHH/Colaboradores/ObtainFullInfo/", {})
      .then((response) => {
        // console.log(response.data )
        this.setState({
          colaboradoresInfoCompleta: response.data,
          loading: false,
        });
      })
      .catch((error) => {
        console.log(error);
        this.setState({ loading: false });
      });
  }

  onHide() {
    this.setState({ colaborador: [], visible: false });
  }

  showForm() {
    this.setState({ visible: true });
  }

  onHideFoto() {
    this.setState({ visible_foto: false });
  }

  showFormFoto() {
    this.setState({ visible_foto: true }, this.childFoto.current.getFotografia);
  }

  onHideEnvioManual() {
    this.setState({ visible_envio_Manual: false });
  }

  showFormEnvioManual() {
    // console.log("showFormEnvioManual")
    this.setState({ visible_envio_Manual: true });
  }

  onHideCargaMasiva() {
    this.setState({ visible_carga_masiva: false });
  }

  showFormCargaMasiva() {
    // console.log("showFormEnvioManual")
    this.clearCarga();
    this.setState({ visible_carga_masiva: true });
  }

  onHideAlta() {
    this.setState({ visible_alta: false });
  }

  showFormAlta(id) {
    axios
      .post("/RRHH/Colaboradores/GetColaboradorApi/" + id, {})
      .then((response) => {
        console.log(response);
        this.setState({
          Id: id,
          tipo_identificacion: response.data.nombre_identificacion,
          nro_identificacion: response.data.numero_identificacion,
          nombres: response.data.nombres_apellidos,
          key_form: Math.random(),
          colaborador: response.data,
          fecha_ingreso: moment(response.data.fecha_ingreso).format(
            "YYYY-MM-DD"
          ),
          visible_alta: true,
          empleado_id_sap: response.data.empleado_id_sap,
        });
      })
      .catch((error) => {
        this.props.warnMessage("");
      });
  }

  onHideAltaMasiva() {
    this.setState({ visible_alta_masiva: false });
  }

  showFormAltaMasiva() {
    // console.log("showFormEnvioManual")
    // this.clearCarga();
    this.setState({ visible_alta_masiva: true });
  }
}
