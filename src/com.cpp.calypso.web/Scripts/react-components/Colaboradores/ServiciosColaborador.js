import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/dialog";
import BlockUi from "react-block-ui";
import wrapForm from "../Base/BaseWrapper";
import Field from "../Base/Field-v2";
import { Checkbox } from 'primereact-v2/checkbox';
import { Card } from 'primereact-v2/card';
import {
  TIPO_GRUPO_PERSONAL,
  PASSCRYPTO
} from "../Base/Constantes";
import QRCode from "qrcode.react";
import CryptoJS from "crypto-js";

//Encriptación QR

export default class ServiciosColaborador extends React.Component {
  constructor(props) {
    super(props);
    this.state = {

      colaboradores: [],

      //BUSQUEDA
      GruposPersonales: [],
      GrupoPersonalId: 0,
      Identificacion: '',
      ApellidosNombres: '',

      //LOADING
      loading: false,
      loadingqr: false,

      /*Generación QR */
      QrDialog: false,
      iseleccionado: null,

      //VALIDACION CÉDULA
      checked: false,

      //ERRORES
      errors: {},

      //

      /*Generación QR */
      QrDialogE: false,
      DataColaborador: '',
      EncryptedData: '',

    };
    this.generateButton = this.generateButton.bind(this);
    this.validacionCedula = this.validacionCedula.bind(this);
    this.Servicios = this.Servicios.bind(this);
    this.GetColaboradoreBuscar = this.GetColaboradoreBuscar.bind(this);

    /* Obtener De Catálogo */

    this.GetCatalogos = this.GetCatalogos.bind(this);
    this.limpiarEstados = this.limpiarEstados.bind(this);

    /* Generación de QR */
    this.OcultarDialogQr = this.OcultarDialogQr.bind(this);
    this.MostrarDialogQr = this.MostrarDialogQr.bind(this);

    this.CrearQR = this.CrearQR.bind(this);
    this.DescargarQR = this.DescargarQR.bind(this);
    this.MostrarDialogQrE = this.MostrarDialogQrE.bind(this);
    this.permitirvalidacioncedula = this.permitirvalidacioncedula.bind(this);

    this.onChangeValue = this.onChangeValue.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }

  componentDidMount() {
    this.GetCatalogos();
    this.props.unlockScreen();
  }

  render() {
    return (
      <div>
        <div className="row">
          <div className="col-4">
            <Field
              name="Identificacion"
              label="Identificación"
              edit={true}
              readOnly={false}
              value={this.state.Identificacion}
              onChange={this.handleChange}
              error={this.state.errors.Identificacion}
            />
          </div>
          <div className="col-5">
            <Field
              name="ApellidosNombres"
              label="Apellidos Nombres"
              edit={true}
              readOnly={false}
              value={this.state.ApellidosNombres}
              onChange={this.handleChange}
              error={this.state.errors.ApellidosNombres}
            />
          </div>
          {/*<div className="col-3">
            <Field
              name="GrupoPersonalId"
              value={this.state.GrupoPersonalId}
              label="Agrupación para Requisitos"
              options={this.state.GruposPersonales}
              type={"select"}
              onChange={this.onChangeValue}
              error={this.state.errors.GrupoPersonalId}
              readOnly={false}
              filter={true}
              placeholder="Seleccione"
              filterPlaceholder="Seleccione"
            />
   
          </div>
           */}
          <div className="col-3" style={{ paddingTop: '35px' }}>
            <button
              type="button"
              onClick={() => this.GetColaboradoreBuscar()}
              style={{ marginLeft: "0.2em" }}
              className="btn btn-outline-primary"
            >
              Buscar
            </button>
            <button
              type="button"
              onClick={() => this.limpiarEstados()}
              style={{ marginLeft: "0.3em" }}
              className="btn btn-outline-primary"
            >
              Cancelar
            </button>
          </div>
        </div>

        <div >
          <BootstrapTable
            data={this.state.colaboradores}
            hover={true}
            pagination={true}
          >
            <TableHeaderColumn
              width={"6%"}
              dataField="nro"
              isKey={true}
              dataAlign="center"
              headerAlign="center"
              dataSort={true}
              tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
              thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
            >
              N°
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="numero_legajo_definitivo"
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
              thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
            >
              Legajo
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="nombre_identificacion"
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
              thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
            >
              Tipo de Identificación
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="numero_identificacion"
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
              thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
            >
              No. de Identificación
            </TableHeaderColumn>
            <TableHeaderColumn
              width={"17%"}
              dataField="apellidos_nombres"
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
              thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
            >
              Apellidos
            </TableHeaderColumn>
            <TableHeaderColumn
              width={"17%"}
              dataField="nombres"
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
              thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
            >
              Nombres
            </TableHeaderColumn>
            <TableHeaderColumn
              width={"12%"}
              dataField="fechaIngresoFormat"
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
              thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
            >
              Fecha Ingreso
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="validacion_cedula"
              headerAlign="center"
              filter={{ type: "TextFilter", delay: 500 }}
              dataFormat={this.validacionCedula.bind(this)}
              dataSort={true}
              tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
              thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
            >
              Validación Cédula
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Operaciones"
              headerAlign="center"
              width={"20%"}
              tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
              thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
              dataFormat={this.generateButton.bind(this)}
            >
              Opciones
            </TableHeaderColumn>
          </BootstrapTable>
        </div>
        <Dialog
          header="Generación de QR"
          visible={this.state.QrDialogE}
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
                        <b>Información Colaborador</b><br /><br />
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
                      </Card><br />
                      <Card className="ui-card-shadow">
                        <b>Permite Validación por Cédula:</b> <Checkbox checked={this.state.checked} onChange={this.permitirvalidacioncedula} /><br />
                        <br />
                      </Card><br />
                      <Card className="ui-card-shadow">

                        <h6 className="text-gray-700">
                          <b>Fecha de Vigencia del QR: </b>{" "}
                          {this.state.iseleccionado != null
                            ? this.state.iseleccionado.fechavigenciacolaboradorqr
                            : ""}
                        </h6>
                      </Card>
                    </div>


                    <div className="col-xs-12 col-md-4">


                      <img href="./Views/LogosCPP/_cpp.png" id="FPImage12" height="350" width="350" /><br></br>


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
                  </div>
                </div>
              </Card>
            </div>
          </BlockUi>
        </Dialog>

        <Dialog
          header="Generación de QR"
          visible={this.state.QrDialog}
          style={{ width: "900PX" }}
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
                        <b>Información Colaborador</b><br /><br />
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
                        <h6 className="text-gray-700">
                          <b>ID SAP Global: </b>{" "}
                          {this.state.iseleccionado != null
                            ? this.state.iseleccionado.empleado_id_sap
                            : ""}
                        </h6>
                        <h6 className="text-gray-700">
                          <b>ID SAP Local: </b>{" "}
                          {this.state.iseleccionado != null
                            ? this.state.iseleccionado.empleado_id_sap_local
                            : ""}
                        </h6>
                      </Card><br />
                      {/*   <Card className="ui-card-shadow">
                        <b>Permite Validación por Cédula:</b> <Checkbox checked={this.state.checked} onChange={this.permitirvalidacioncedula} /><br />
                        <br />
                      </Card> */} <br />
                      <Card className="ui-card-shadow">

                        <h6 className="text-gray-700">
                          <b>Fecha de Vigencia del QR: </b>{" "}
                          {this.state.iseleccionado != null
                            ? this.state.iseleccionado.fechavigenciacolaboradorqr
                            : ""}
                        </h6>
                      </Card>
                    </div>


                    <div className="col-xs-12 col-md-4">

                      <QRCode value={this.state.EncryptedData} size={260} id="FPImage1" />
                      <br></br>
                      <div className="row">
                        {/**  <button
                          onClick={() => this.DescargarQR()}
                          type="button"
                          className="btn btn-outline-primary"
                          style={{ marginLeft: "3px" }}
                        >
                          Imprimir QR
                         </button>*/}
                        <button
                          onClick={() => this.OcultarDialogQr()}
                          type="button"
                          className="btn btn-outline-primary"
                          style={{ marginLeft: "3px" }}
                        > Cancelar
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </Card>
            </div>
          </BlockUi>
        </Dialog>


      </div>
    );
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  permitirvalidacioncedula(e) {
    console.log(e)
    this.setState({ loadingqr: true })
    if (this.state.iseleccionado.Id > 0) {


      axios
        .post("/RRHH/Colaboradores/CreateValidacionCedula/", {
          id: this.state.iseleccionado.Id
        })
        .then(response => {
          if (response.data == "OK") {
            this.setState({ checked: e.checked, loadingqr: false })
            this.props.showSuccess("Validación por cédula actualizado");
          } else {
            this.setState({ loadingqr: false })
            this.props.showWarning("Debe seleccionar un colaborador");
          }

        })
        .catch(error => {
          console.log(error);
          this.setState({ loadingqr: false })
          this.warnMessage("Algo salio mal!");
        });
    } else {
      this.props.showWarn("Ocurrió un error al actualizar campo validación");

    }
  }

  generateButton(cell, row) {
    return (
      <div>
        <button
          type="button"
          onClick={() => this.Servicios(row)}
          className="btn btn-outline-primary  fa fa-cogs"
          style={{ marginRight: "0.3em" }}
          data-toggle="tooltip"
          data-placement="top"
          title="Asignación de Servicios"
        >
        </button>
        <button
          className="btn btn-outline-info fa fa-qrcode"
          onClick={() => this.MostrarDialogQrE(row)}
          style={{ marginRight: "0.3em" }}
          data-toggle="tooltip"
          data-placement="top"
          title="Generación de QR"
        />

      </div>
    );
  }

  Guardar() {
    if (this.state.codigo_qr) {
      axios
        .post("/RRHH/Colaboradores/UpdateColaboradorQR/", {
          id: sessionStorage.getItem("id_colaborador"),
          validacion: this.state.permiteValidacion
        })
        .then(response => {
          this.showSuccess("Colaborador guardado con exito!");
          this.Regresar();
        })
        .catch(error => {
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
        id: row.Id
      })
      .then(response => {
        console.log(response.data);
        if (response.data[0] == "s_s") {

          this.props.showWarning("El colaborador no tiene servicios activos");
          this.setState({
            QrDialog: true,
            loadingqr: false,
            codigo_qr: response.data[1],
            display: "block"
          });
          this.props.unlockScreen();
          /* Se carga la imagen */

          document.getElementById("FPImage1").src =
            "data:image/jpg;base64," + response.data[1];
          this.props.unlockScreen();

        } else if (response.data[0] == "s_r") {

          this.props.showWarning("El colaborador no tiene reservas activas");
          this.setState({
            QrDialog: true,
            loadingqr: false,
            codigo_qr: response.data[1],
            display: "block"
          });
          this.props.unlockScreen();
          /* Se carga la imagen */

          document.getElementById("FPImage1").src =
            "data:image/jpg;base64," + response.data[1];
          this.props.unlockScreen();
        } else {

          this.setState({
            QrDialog: true,
            loadingqr: false,
            codigo_qr: response.data[1],
            display: "block"
          });
          this.props.unlockScreen();
          /* Se carga la imagen */

          document.getElementById("FPImage1").src =
            "data:image/jpg;base64," + response.data[1];

        }
      })
      .catch(error => {
        console.log(error);
      });
  }

  DescargarQR() {
    /*
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
*/
    var pngUrl = ($('FPImage1')[0]).toDataURL("image/png")
      .replace("image/png", "image/octet-stream");
    let downloadLink = document.createElement("a");
    downloadLink.href = pngUrl;
    downloadLink.download = this.state.iseleccionado != null
      ? this.state.iseleccionado.nombres_apellidos + ".jpg"
      : "QR.jpg";
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
  }

  GetCatalogos() {
    axios
      .get(`/RRHH/Colaboradores/GetByCodeApi/?code=${TIPO_GRUPO_PERSONAL}`, {})
      .then(response => {
        var items = response.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ GruposPersonales: items, loading: false });
      })
      .catch(error => {
        console.log(error);
      });
  }

  limpiarEstados() {
    this.setState({
      colaboradores: [],
      GrupoPersonalId: 0,
      Identificacion: "",
      ApellidosNombres: "",
      loading: false
    })
  }

  validacionCedula(cell, row) {
    return (
      <div>
        <label>{row.validacion_cedula == true ? "SI" : "NO"}</label>
      </div>
    );
  }

  Servicios(data) {
    var fechaActual = moment().format("YYYY-MM-DD");
    let fechaIngresoMoment = moment(data.fecha_ingreso, "YYYY-MM-DD");
    let fechaIngreso = moment(fechaIngresoMoment).format("YYYY-MM-DD");
    console.log('Fechas',fechaIngreso+ " -act- " +fechaActual)
    console.log(fechaIngreso > fechaActual);
    if (fechaIngreso > fechaActual) {
      this.props.showWarn("La fecha de ingreso del Colaborador es superior a la actual, no se puede asignar servicios"
      );
      return;
    }



    return (window.location.href = "/RRHH/Colaboradores/CrearServiciosColaborador/" + data.Id);
  }

  MostrarDialogQr(row) {
    this.props.blockScreen();
    console.log(row);
    this.CrearQR(row);
    this.setState({ iseleccionado: row, loadingqr: true, checked: row.validacion_cedula, QrDialog: true });

  }

  MostrarDialogQrE(row) {
    axios
      .post("/RRHH/Colaboradores/GetDataQrE/", {
        Id: row.Id
      })
      .then(response => {
        /* var data = JSON.stringify(response.data.result);
         // Encriptar
         var ciphertext = CryptoJS.AES.encrypt(data, PASSCRYPTO);
         console.log(ciphertext.toString());
 
         // Decrypt
         var bytes = CryptoJS.AES.decrypt(ciphertext.toString(), PASSCRYPTO);
         var plaintext = bytes.toString(CryptoJS.enc.Utf8);
         console.log(plaintext);s
         */
        this.setState({
          QrDialog: true,
          loadingqr: false,
          EncryptedData: response.data.toString()
        });

      })
      .catch(error => {
        console.log(error);
      });




    this.setState({ iseleccionado: row, loadingqr: true, checked: row.validacion_cedula, QrDialog: true });

  }


  OcultarDialogQr() {
    this.setState({ QrDialog: false, iseleccionado: null });
    document.getElementById("FPImage1").src = "data:image/jpg;base64,";
    this.GetColaboradoreBuscar();
  }


  GetColaboradoreBuscar() {
    if (this.state.Identificacion === '' && this.state.ApellidosNombres === '' && this.state.GrupoPersonalId == 0) {
      this.props.showWarn(
        "Seleccione un Criterio de Búsqueda"
      );
    } else {

      this.props.blockScreen();

      axios
        .post("/RRHH/Colaboradores/GetColaboradorFiltros/", {
          numeroIdentificacion: this.state.Identificacion,
          nombres: this.state.ApellidosNombres,
          grupoPersonal: this.state.GrupoPersonalId
        })
        .then(response => {
          console.log(response.data)
          if (response.data.length == 0) {
            this.setState({ loading: false, colaboradores: [] });
            this.props.showWarn(
              "No existe registros con la información ingresada"
            );
            this.props.unlockScreen();
          } else {
            this.setState({ loading: false, colaboradores: response.data });
            this.props.unlockScreen();
          }
        })
        .catch(error => {
          console.log(error);
          this.props.unlockScreen();
        });
    }
  }



}
const Container = wrapForm(ServiciosColaborador);
ReactDOM.render(
  <Container />,
  document.getElementById("content-servicios")
);
