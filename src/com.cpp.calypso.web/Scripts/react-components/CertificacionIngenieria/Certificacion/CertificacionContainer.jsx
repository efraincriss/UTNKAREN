import axios from "axios";
import { Button } from "primereact-v2/button";
import { FileUpload } from "primereact-v2/fileupload";
import { Dialog } from "primereact-v3.3/dialog";
import React, { Fragment } from "react";
import config from "../../Base/Config";
import ReactDOM from "react-dom";
import Wrapper from "../../Base/BaseWrapper";
import Field from "../../Base/Field-v2";
import http from "../../Base/HttpService";
import { DataTable } from "primereact-v3.3/datatable";
import { TabView, TabPanel } from "primereact-v3.3/tabview";
import CurrencyFormat from "react-currency-format";
import { Column } from "primereact-v3.3/column";
import { MultiSelect } from "primereact-v3.3/multiselect";
import { ContextMenu } from "primereact-v3.3/contextmenu";
import { ToggleButton } from 'primereact-v3.3/togglebutton';
import {
  CONTROLLER_COLABORADOR_RUBRO,
  CONTROLLER_GRUPO_CERTIFICACION,
  CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA,
  FRASE_DETALLE_INDIRECTO_ELIMINADO,
  FRASE_ERROR_GLOBAL,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings";
import { DetalleGrupo } from "./DetalleGrupo.jsx";
import { DetalleGasto } from "./DetalleGasto.jsx";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { GruposCertificadosTable } from "./GruposCertificadosTable.jsx";

class CertificacionContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      detalles: [],
      detalleSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: 12,
      screen: "list",
      errors: {},
      action: "create",
      FechaDesde: "",
      FechaHasta: "",
      certificados: [],
      certificadoSeleccionado: {},
      gastos: [],

      /*Form */
      Id: 0,
      FechaInicio: "",
      FechaFin: "",
      FechaCertificado: "",
      FechaGeneracion: new Date(),
      ClienteId: 0,
      Clientes: [],
      EstadoId: 0,

      puedeGenerar: true,
      /*Porcentaje Distribucion Parametro */
      PorcentajeMaximoDistribucion: 0,

      /*Lista Directos e Indirectos a Certificar */
      dataDirectos: [{}],
      directosSeleccionados: [{}],
      totalHorasDirectos: 0,
      montoTotalDirectos: 0,
      //Pendientes
      dataDirectosPendientes: [{}],
      directosSeleccionadosPendientes: [{}],
      totalHorasDirectosPendientes: 0,
      montoTotalDirectosPendientes: 0,

      dataIndirectos: [{}],
      indirectosSeleccionados: [{}],
      totalHorasIndirectos: 0,
      montoTotalIndirectos: 0,
      //Pendientes
      dataIndirectosPendientes: [{}],
      indirectosSeleccionadosPendientes: [{}],
      totalHorasIndirectosPendientes: 0,
      montoTotalIndirectosPendientes: 0,

      dataE500: [{}],
      E500Seleccionados: [{}],
      totalHorasE500: 0,
      montoTotalE500: 0,

      //Pendientes
      dataE500Pendientes: [{}],
      E500SeleccionadosPendientes: [{}],
      totalHorasE500Pendientes: 0,
      montoTotalE500Pendientes: 0,


      ClienteSeleccionado: {},

      distribucionIndirectos: [{}],
      indirectoSeleccionado: null,
      proyectosSeleccionados: [],

      proyectosADistribuir: [{}],
      certificadoValidado: false,
      viewdistribucion: false,
      selectedNodeKey: null,
      menu: [
        {
          label: "No Distribuir A",
          icon: "pi pi-search",
          command: () => {
            this.mostrarFormularioDistribucion(this.state.selectedNodeKey);
          },
        },
      ],

      totalHorasActCertificadas: 0,
      totalHorasAntCertificadas: 0,
      totalMontoActCertificado: 0,
      totalMontoAntCertificado: 0,
      totalMontoDirecto: 0,
      totalMontoIndirecto: 0,
      isUpdating: true,
      mostrarConfirmacionContinuarGeneracion: false,
      grupoAnterior: {},


      //Check Distribucion
      distribucionProyectos: [],
      mostrarDistribucion: false,
      viewParametrizacion: false


    };
    this.onChangeValue = this.onChangeValue.bind(this);
  }

  componentDidMount() {
    this.GetContratos();
  }

  onSelectDirectosSinCertificar = (e) => {
    this.setState({ directosSeleccionados: e.value });

    /*Suma Horas */
    let ArrayHoras = e.value.map((item) => {
      let horas = 0;
      if (Object.keys(item).length != 0) {
        horas = parseFloat(item.NumeroHoras);
      }
      return { horas };
    });
    //GrupoCertificadoIdArrayHoras.shift(); //Delete NAN from component select Datatable Primereact
    let total = 0;
    for (let i of ArrayHoras) total += i.horas;
    console.log(total);
    this.setState({ totalHorasDirectos: total });
    /*Suma Monto */
    let ArrayMontos = e.value.map((item) => {
      let monto = 0;
      if (Object.keys(item).length != 0) {
        monto = parseFloat(item.monto);
      }
      return { monto };
    });
    //ArrayMontos.shift();
    let montototal = 0;
    for (let i of ArrayMontos) montototal += i.monto;
    console.log(montototal);
    this.setState({ montoTotalDirectos: montototal });
  };

  onSelectDirectosSinCertificarPendientes = (e) => {
    this.setState({ directosSeleccionadosPendientes: e.value });

    /*Suma Horas */
    let ArrayHoras = e.value.map((item) => {
      let horas = 0;
      if (Object.keys(item).length != 0) {
        horas = parseFloat(item.NumeroHoras);
      }
      return { horas };
    });
    //GrupoCertificadoIdArrayHoras.shift(); //Delete NAN from component select Datatable Primereact
    let total = 0;
    for (let i of ArrayHoras) total += i.horas;
    console.log(total);
    this.setState({ totalHorasDirectosPendientes: total });
    /*Suma Monto */
    let ArrayMontos = e.value.map((item) => {
      let monto = 0;
      if (Object.keys(item).length != 0) {
        monto = parseFloat(item.monto);
      }
      return { monto };
    });
    //ArrayMontos.shift();
    let montototal = 0;
    for (let i of ArrayMontos) montototal += i.monto;
    console.log(montototal);
    this.setState({ montoTotalDirectosPendientes: montototal });
  };

  onSelectIndirectosSinCertificar = (e) => {
    this.setState({ indirectosSeleccionados: e.value });
    let ArrayHoras = e.value.map((item) => {
      let horas = 0;
      if (Object.keys(item).length != 0) {
        horas = parseFloat(item.HorasLaboradas * item.DiasLaborados);
      }
      return { horas };

    });
    //ArrayHoras.shift(); //Delete NAN from component select Datatable Primereact
    let total = 0;
    for (let i of ArrayHoras) total += i.horas;
    console.log(total);
    this.setState({ totalHorasIndirectos: total });

    /*Suma Monto */
    let ArrayMontos = e.value.map((item) => {
      let monto = 0;
      if (Object.keys(item).length != 0) {
        monto = parseFloat(item.monto);
      }
      return { monto };
    });
    //ArrayMontos.shift();
    let montototal = 0;
    for (let i of ArrayMontos) montototal += i.monto;
    console.log(montototal);
    this.setState({ montoTotalIndirectos: montototal });
  };
  onSelectIndirectosSinCertificarPendientes = (e) => {
    this.setState({ indirectosSeleccionadosPendientes: e.value });
    let ArrayHoras = e.value.map((item) => {
      let horas = 0;
      if (Object.keys(item).length != 0) {
        horas = parseFloat(item.HorasLaboradas * item.DiasLaborados);
      }
      return { horas };

    });
    //ArrayHoras.shift(); //Delete NAN from component select Datatable Primereact
    let total = 0;
    for (let i of ArrayHoras) total += i.horas;
    console.log(total);
    this.setState({ totalHorasIndirectosPendientes: total });

    /*Suma Monto */
    let ArrayMontos = e.value.map((item) => {
      let monto = 0;
      if (Object.keys(item).length != 0) {
        monto = parseFloat(item.monto);
      }
      return { monto };
    });
    //ArrayMontos.shift();
    let montototal = 0;
    for (let i of ArrayMontos) montototal += i.monto;
    console.log(montototal);
    this.setState({ montoTotalIndirectosPendientes: montototal });
  };
  onSelectE500 = (e) => {
    this.setState({ E500Seleccionados: e.value });
    console.log("E500Seleccionados", e.value);
    let ArrayHoras = e.value.map((item) => {
      console.log("Item", item);
      let horas = 0;
      if (Object.keys(item).length != 0) {
        horas = parseFloat(item.NumeroHoras);
      }

      return { horas };
    });

    //ArrayHoras.shift(); //Delete NAN from component select Datatable Primereact
    console.log("ArrayHorasE500", ArrayHoras);
    let total = 0;
    for (let i of ArrayHoras) total += i.horas;
    console.log(total);
    this.setState({ totalHorasE500: total });

    //Suma Monto
    let ArrayMontos = e.value.map((item) => {
      let monto = 0;
      if (Object.keys(item).length != 0) {
        monto = parseFloat(item.monto);
      }
      return { monto };
    });
    //ArrayMontos.shift();
    let montototal = 0;
    for (let i of ArrayMontos) montototal += i.monto;
    console.log(montototal);
    this.setState({ montoTotalE500: montototal });
  };
  onSelectE500Pendientes = (e) => {
    this.setState({ E500SeleccionadosPendientes: e.value });
    console.log("E500Seleccionados", e.value);
    let ArrayHoras = e.value.map((item) => {
      console.log("Item", item);
      let horas = 0;
      if (Object.keys(item).length != 0) {
        horas = parseFloat(item.NumeroHoras);
      }

      return { horas };
    });

    //ArrayHoras.shift(); //Delete NAN from component select Datatable Primereact
    console.log("ArrayHorasE500", ArrayHoras);
    let total = 0;
    for (let i of ArrayHoras) total += i.horas;
    console.log(total);
    this.setState({ totalHorasE500Pendientes: total });

    //Suma Monto
    let ArrayMontos = e.value.map((item) => {
      let monto = 0;
      if (Object.keys(item).length != 0) {
        monto = parseFloat(item.monto);
      }
      return { monto };
    });
    //ArrayMontos.shift();
    let montototal = 0;
    for (let i of ArrayMontos) montototal += i.monto;
    console.log(montototal);
    this.setState({ montoTotalE500Pendientes: montototal });
  };
  GetContratos = () => {
    this.props.blockScreen();
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/APICliente`,
        {}
      )
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Clientes: items }, this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
      });
  };

  handleChange = (name, value) => {
    this.setState({ [name]: value });
  };

  handleChangeFechas = (event) => {
    const target = event.target;
    const value = target.value;
    const name = target.name;
    this.setState({ [name]: value });
  };

  renderBody = () => {
    if (this.state.screen === "list") {
      return this.listComponent();
    } else if (this.state.screen === "Form") {
      return this.formComponent();
    } else if (this.state.screen === "detalleGrupo") {
      return this.detalleGrupoComponent();
    } else if (this.state.screen === "gastosDirectosForm") {
      return this.formularioGastosComponent();
    } else if (this.state.screen === "seleccionarDetallesForm") {
      return this.formularioSeleccionarDetallesCertificado();
    }
  };

  render() {
    return <Fragment>{this.renderBody()}</Fragment>;
  }

  listComponent = () => {
    const options = {
      sizePerPage: 5,

    };
    const Type = {
      true: 'SI',
      false: 'NO',
    };
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col">
                <Field
                  name="FechaDesde"
                  value={this.state.FechaDesde}
                  label="Fecha Inicio"
                  type="date"
                  onChange={this.handleChangeFechas}
                  error={this.state.errors.FechaDesde}
                  edit={true}
                  readOnly={false}
                />
              </div>

              <div className="col">
                <Field
                  name="FechaHasta"
                  value={this.state.FechaHasta}
                  label="Fecha Fin"
                  type="date"
                  onChange={this.handleChangeFechas}
                  error={this.state.errors.FechaHasta}
                  edit={true}
                  readOnly={false}
                />
              </div>
            </div>

            <div className="row" style={{ marginLeft: "0.1em" }}>
              <Button
                label="Buscar"
                className="p-button-outlined"
                onClick={() => this.obtenerGruposCertificados()}
                icon="pi pi-search"
              />
              <Button
                style={{ marginLeft: "0.4em" }}
                label="Borrar Filtros"
                className="p-button-outlined"
                onClick={() => this.borrarFiltros()}
                icon="pi pi-ban"
              />
            </div>

            <div className="row" style={{ marginTop: "1em" }}>
              <div className="col" align="right">
                {/*   <button
                  className="btn btn-outline-indigo"
                  style={{ marginLeft: "0.2em" }}
                  onClick={() => this.descargarResumen()}
                  data-toggle="tooltip"
                  data-placement="top"
                  title="Descargar Resumen Proyectos"
                >
                  <i className="fa fa-download"></i>
                </button>{" "}*/}
                <button
                  className="btn btn-outline-primary"
                  onClick={() => this.mostrarFormulario({})}
                >
                  Nuevo
                </button>
              </div>
            </div>
            <hr />

            <div className="row">
              <div className="col">
                <GruposCertificadosTable
                  data={this.state.detalles}
                  mostrarFormulario={this.mostrarFormulario}
                  mostrarConfirmacionParaEliminar={
                    this.mostrarConfirmacionParaEliminar
                  }
                  mostrarDetalleGrupo={this.mostrarDetalleGrupo}
                  descargarGrupodeCertificados={
                    this.descargarGrupodeCertificados
                  }
                  descargarGrupodeCertificados2={
                    this.descargarGrupodeCertificados2
                  }
                  mostrarDistribucionParametros={this.mostrarDistribucionParametros}
                />
              </div>
            </div>
            {/**#aq */}
            <Dialog header="Parametrización Distribución"
              visible={this.state.viewParametrizacion}
              style={{ width: '80vw' }} modal={true}
              onHide={() => this.setState({ viewParametrizacion: false })}>

              <BootstrapTable data={this.state.distribucionProyectos} hover={true} options={options} pagination={true}

              >
                <TableHeaderColumn
                  isKey
                  dataField="Codigo"
                  width={"16%"}
                  tdStyle={{
                    whiteSpace: "normal",
                    textAlign: "left",
                    fontSize: "11px",
                  }}
                  thStyle={{
                    whiteSpace: "normal",
                    textAlign: "left",
                    fontSize: "11px",
                  }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  dataSort={true}

                >Proyecto
                </TableHeaderColumn>
                <TableHeaderColumn

                  dataField="Nombre"
                  width={"38%"}
                  tdStyle={{
                    whiteSpace: "normal",
                    textAlign: "left",
                    fontSize: "11px",
                  }}
                  thStyle={{
                    whiteSpace: "normal",
                    textAlign: "left",
                    fontSize: "11px",
                  }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  dataSort={true}

                >Nombre
                </TableHeaderColumn>
                <TableHeaderColumn formatExtraData={Type} filter={{ type: 'SelectFilter', options: Type }} width={"15%"} dataField='AplicaIndirecto' dataFormat={this.activeFormatterDisable}>Aplica Indirectos</TableHeaderColumn>
                <TableHeaderColumn formatExtraData={Type} filter={{ type: 'SelectFilter', options: Type }} width={"15%"} dataField='AplicaViatico' dataFormat={this.activeFormatterViaticoDisable}>Aplica Viáticos</TableHeaderColumn>
                <TableHeaderColumn formatExtraData={Type} filter={{ type: 'SelectFilter', options: Type }} width={"15%"} dataField='AplicaE500' dataFormat={this.activeFormatter500Disable}>Aplica D. E500</TableHeaderColumn>


              </BootstrapTable>
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
                    ¿Está seguro de eliminar el registro? Si desea continuar
                    presione ELIMINAR, caso contrario CANCELAR
                  </p>
                </div>
              </div>
            </Dialog>
          </div>
        </div>
      </Fragment>
    );
  };

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };
  onChangeValueContrato = (name, value) => {
    this.setState({
      [name]: value,
      ClienteSeleccionado: this.state.Clientes.filter(
        (c) => c.value == value
      )[0],
    });
  };
  isValid = () => {
    const errors = {};
    let ContratoId = this.state.ContratoId;
    let ClienteId = this.state.ClienteId;
    let FechaInicio = this.state.FechaInicio;
    let FechaFin = this.state.FechaFin;
    let FechaCertificado = this.state.FechaCertificado;
    if (FechaCertificado === "") {
      errors.FechaCertificado = "El campo Fecha Certificado es requerido";
    }

    if (FechaInicio === "") {
      errors.FechaInicio = "El campo Fecha Inicio es requerido";
    }

    if (FechaFin === "") {
      errors.FechaFin = "El campo Fecha Fin es requerido";
    }
    if (ContratoId === 0) {
      errors.ContratoId = "El campo Contrato es requerido";
    }
    if (ClienteId === 0) {
      errors.ClienteId = "El campo Cliente es requerido";
    }

    if (FechaInicio !== "" && FechaFin !== "") {
      let fechaInicioDate = new Date(FechaInicio);
      let fechaFinDate = new Date(FechaFin);
      console.log(fechaInicioDate);
      if (fechaInicioDate > fechaFinDate) {
        errors.FechaFin = "El campo Fecha Fin debe ser mayor a Fecha Inicio";
      }
    }

    if (FechaCertificado !== "" && FechaFin !== "") {
      let fechaCertificadoDate = new Date(FechaCertificado);
      let fechaFinDate = new Date(FechaFin);
      console.log(fechaFinDate);
      if (fechaFinDate > fechaCertificadoDate) {
        errors.FechaCertificado =
          "El campo Certificado debe ser mayor a Fecha Fin";
      }
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  renderListadoGeneral = () => {
    this.setState({ screen: "list" });
  };

  handleSubmitMasivo = () => {
    this.props.blockScreen();
    let url = "";
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}`;
    if (this.state.action !== "crear") {
      url += "/Editar";
    } else {
      url += "/CrearMasivo";
    }
    console.log(url);

    axios
      .post(url, {
        Id: this.state.Id,
        ClienteId: this.state.ClienteId,
        FechaInicio: this.state.FechaInicio,
        FechaFin: this.state.FechaFin,
        FechaCertificado: this.state.FechaCertificado,
        FechaGeneracion: this.state.FechaGeneracion,
        EstadoId: this.state.EstadoId,
      })
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          this.props.showSuccess("Certificado Generado");
          this.obtenerGruposCertificados();
          this.onHideFormulario(false);
        } else {
          var message = data.message;
          this.props.showWarn(message);
          this.props.unlockScreen();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(FRASE_ERROR_GLOBAL);
        this.props.unlockScreen();
      });
  };
  validarCertificado = () => {
    console.log("formValidacionAction");
    console.log("certificadoValidado", certificadoValidado);
    this.setState({ certificadoValidado: true });
  };
  handleSubmit = () => {
    console.log("DirectosSeleccionados", this.state.directosSeleccionados);
    console.log("InDirectosSeleccionados", this.state.indirectosSeleccionados);
    if (this.state.directosSeleccionados.length === 0) {
      this.props.showWarn(
        "Debes seleccionar al menos un registro directo para continuar"
      );
      return;
    }
    if (!this.isValid()) {
      this.props.showWarn("Existen errores en el formulario", "Validaciones");
      return;
    }
    console.log("Action", this.state.action);

    this.props.blockScreen();
    let url = "";
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}`;

    let urlCrearGrupo = url + "/CrearGrupo";
    let urlGenerarCertificadosPorProyectosDirectos =
      url + "/GenerarCertificadosPorProyectosDirectos";
    let urlGenerarDistribucionIndirectos =
      url + "/GenerarDistribucionIndirectos";
    let urlGenerarDistribucionE500 = url + "/GenerarDistribucionE500";

    let DirectosId = new Array();
    if (this.state.directosSeleccionados.length > 0) {
      DirectosId = this.state.directosSeleccionados.map((select) => {
        return select.Id;
      });
    }
    let IndirectosId = new Array();
    if (this.state.indirectosSeleccionados.length > 0) {
      IndirectosId = this.state.indirectosSeleccionados.map((select) => {
        return select.Id;
      });
    }

    let E500Id = new Array();
    if (this.state.E500Seleccionados.length > 0) {
      E500Id = this.state.E500Seleccionados.map((select) => {
        return select.Id;
      });
    }

    let ProyectosId = new Array();
    if (this.state.directosSeleccionados.length > 0) {
      ProyectosId = this.state.directosSeleccionados.map((select) => {
        return select.ProyectoId;
      });
    }

    axios
      .post(url, {
        Id: this.state.Id,
        ClienteId: this.state.ClienteId,
        FechaInicio: this.state.FechaInicio,
        FechaFin: this.state.FechaFin,
        FechaCertificado: this.state.FechaCertificado,
        FechaGeneracion: this.state.FechaGeneracion,
        EstadoId: this.state.EstadoId,
        Directos: DirectosId,
        Indirectos: IndirectosId,
        E500: E500Id,
      })
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          this.props.showSuccess("Certificado Generado");
          this.obtenerGruposCertificados();
          this.onHideFormulario(false);
          this.obtenerDetallesSinCertificar();
        } else {
          var message = data.message;
          this.props.showWarn(message);
          this.props.unlockScreen();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(FRASE_ERROR_GLOBAL);
        this.props.unlockScreen();
      });
  };

  handleSubmitActualizarLocacionDirectos = () => {

    this.props.blockScreen();
    let url = "";
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}`;
    let urlActualizarLocacion = url + "/ActualizarCampoGrupo";

    axios
      .post(urlActualizarLocacion, {
        Id: this.state.Id,
        ClienteId: this.state.ClienteId,
        FechaInicio: this.state.FechaInicio,
        FechaFin: this.state.FechaFin,
        FechaCertificado: this.state.FechaCertificado,
        FechaGeneracion: this.state.FechaGeneracion,
        EstadoId: this.state.EstadoId,
      })
      .then((response) => {
        this.props.unlockScreen()
        this.props.showSuccess("Actualizado")
      }).catch((error) => {
        console.log(error);
        this.props.showWarn(FRASE_ERROR_GLOBAL);
        this.props.unlockScreen();
      });
  }

  abrirDistribucionProyectos = () => {
    if (this.state.directosSeleccionados === 0) {
      this.props.showWarning("Debe seleccionar registrose en la tabla inferior");
      return;
    }
    let DirectosId = new Array();
    if (this.state.directosSeleccionados.length > 0) {
      DirectosId = this.state.directosSeleccionados.map((select) => {
        return select.Id;
      });
    }

    let DirectosIdPendientes = new Array();
    if (this.state.directosSeleccionadosPendientes.length > 0) {
      DirectosIdPendientes = this.state.directosSeleccionadosPendientes.map((select) => {
        return select.Id;
      });
    }
    console.log('DirectosAntes', DirectosId);
    console.log('DirectosIdPendientes', DirectosIdPendientes);

    
    DirectosIdPendientes.forEach((Id) => {
      if (Id !== null && Id != undefined) {
        DirectosId.push(Id);
      }
    });
   // DirectosId.concat(DirectosIdPendientes);

    console.log('DirectosDespues', DirectosId);

    this.props.blockScreen();
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/APIProyectosDistribuibles`,
        {
          Directos: DirectosId
        }
      )
      .then((response) => {
        console.log(response.data);

        this.setState({ distribucionProyectos: response.data, mostrarDistribucion: true }, this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
      });


  }
  confirmarGeneracion = () => {
    let _this = this;
    abp.message.confirm(
      'Se procederá con la generación del certificado con los parámetros seleccionados',
      '¿Desea continuar?',
      function (isConfirmed) {
        if (isConfirmed) {
          _this.handleSubmitExtendido();
        }
      }
    );

  }

  handleSubmitExtendido = () => {

    console.log("DirectosSeleccionados", this.state.directosSeleccionados);
    console.log("InDirectosSeleccionados", this.state.indirectosSeleccionados);
    if (this.state.directosSeleccionados.length === 0) {
      this.props.showWarn(
        "Debes seleccionar al menos un registro directo para continuar"
      );
      return;
    }
    if (!this.isValid()) {
      this.props.showWarn("Existen errores en el formulario", "Validaciones");
      return;
    }
    console.log("Action", this.state.action);

    this.props.blockScreen();
    let url = "";
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}`;

    let urlCrearGrupo = url + "/CrearGrupo";
    let urlGenerarCertificadosPorProyectosDirectos =
      url + "/GenerarCertificadosPorProyectosDirectos";
    let urlGenerarDistribucionIndirectos =
      url + "/GenerarDistribucionE500Principal";
    let urlGenerarDistribucionIndirectos2 =
      url + "/GenerarDistribucionIndirectosPrincipal";
    let urlActualizarCabecerasGrupo = url + "/ActualizarCabecerasGrupo";
    let urlViatiacos = url + "/AgregarViaticos";


    let IndirectosId = new Array();
    if (this.state.indirectosSeleccionados.length > 0) {
      IndirectosId = this.state.indirectosSeleccionados.map((select) => {
        return select.Id;
      });
    }

    let E500Id = new Array();
    if (this.state.E500Seleccionados.length > 0) {
      E500Id = this.state.E500Seleccionados.map((select) => {
        return select.Id;
      });
    }

    //Pendientes
    let IndirectosIdPendientes = new Array();
    if (this.state.indirectosSeleccionadosPendientes.length > 0) {
      IndirectosIdPendientes = this.state.indirectosSeleccionadosPendientes.map((select) => {
        return select.Id;
      });
    }

    let E500IdPendientes = new Array();
    if (this.state.E500SeleccionadosPendientes.length > 0) {
      E500IdPendientes = this.state.E500SeleccionadosPendientes.map((select) => {
        return select.Id;
      });
    }

   // IndirectosId.concat(IndirectosIdPendientes);
    IndirectosIdPendientes.forEach((Id) => {
      if (Id !== null && Id != undefined) {
        IndirectosId.push(Id);
      }
    });


   // E500Id.concat(E500IdPendientes);
   E500IdPendientes.forEach((Id) => {
      if (Id !== null && Id != undefined) {
        E500Id.push(Id);
      }
    });

    let ProyectosId = new Array();

    if (this.state.directosSeleccionados.length > 0) {
      ProyectosId = this.state.directosSeleccionados.map((select) => {
        return select.ProyectoId;
      });
    }
    let ProyectosIdPendientes = new Array();
    //Pendientes
    if (this.state.directosSeleccionadosPendientes.length > 0) {
      ProyectosIdPendientes = this.state.directosSeleccionadosPendientes.map((select) => {
        return select.ProyectoId;
      });
    }


    ProyectosIdPendientes.forEach((Id) => {
      if (Id !== null && Id != undefined) {
        ProyectosId.push(Id);
      }
    });
   // ProyectosId.concat(ProyectosIdPendientes);

    console.log('ProyectosId',ProyectosId)
    ProyectosId = ProyectosId.filter(x=>x!=undefined);
    console.log('EliminadoUndefined',ProyectosId)
    ProyectosId = ProyectosId.filter((x, i, a) => a.indexOf(x) == i);
    console.log('ProyectosIdFilter',ProyectosId)
    axios
      .post(urlCrearGrupo, {
        Id: this.state.Id,
        ClienteId: this.state.ClienteId,
        FechaInicio: this.state.FechaInicio,
        FechaFin: this.state.FechaFin,
        FechaCertificado: this.state.FechaCertificado,
        FechaGeneracion: this.state.FechaGeneracion,
        EstadoId: this.state.EstadoId,
        distribucionProyectos: this.state.distribucionProyectos,
      })
      .then((response) => {
        let GrupoId = response.data;
        console.log("data", GrupoId);
        if (GrupoId > 0) {
          let countTotalProyectos = ProyectosId.length;
          console.log("Total Proyectos", countTotalProyectos);
          let count = 0;
          let _this = this;
          ProyectosId.forEach(function (ProyectoId) {


            console.log("ProyectoId", ProyectoId);

            let Directos = _this.state.directosSeleccionados.filter(
              (c) => c.ProyectoId == ProyectoId
            );
            console.log("Directos", Directos);

            let DirectosId = new Array();
            if (Directos.length > 0) {
              DirectosId = Directos.map((select) => {
                return select.Id;
              });
            }

            //Pendientes
            let DirectosPendientes = _this.state.directosSeleccionadosPendientes.filter(
              (c) => c.ProyectoId == ProyectoId
            );
            console.log("DirectosPendientes", DirectosPendientes);

            let DirectosIdPendientes = new Array();
            if (DirectosPendientes.length > 0) {
              DirectosIdPendientes = DirectosPendientes.map((select) => {
                return select.Id;
              });
            }


            DirectosIdPendientes.forEach((Id) => {
              if (Id !== null && Id != undefined) {
                DirectosId.push(Id);
              }
            });
          //  DirectosId.concat(DirectosIdPendientes);



            axios
              .post(urlGenerarCertificadosPorProyectosDirectos, {
                GrupoCertificadoId: GrupoId,
                ProyectoId: ProyectoId,
                Directos: DirectosId,
              })
              .then((response) => {
                let data = response.data;
                if (data.success === true) {
                  count = count + 1;
                  console.log("count", count);
                  console.log("Proyecto ", ProyectoId);

                  if (count === countTotalProyectos) {
                    console.log("Igual", count === countTotalProyectos);
                    axios
                      .post(urlGenerarDistribucionIndirectos, {
                        GrupoCertificadoId: GrupoId,
                        Indirectos: IndirectosId,
                        E500: E500Id,
                        distribucionProyectos: _this.state.distribucionProyectos

                      })
                      .then((response) => {
                        console.log("Distribucion E500 Completa");

                        axios
                          .post(urlActualizarCabecerasGrupo, {
                            GrupoCertificadoId: GrupoId,
                          })
                          .then((response) => {
                            console.log("Actualizacion Completa");
                            axios
                              .post(urlGenerarDistribucionIndirectos2, {
                                GrupoCertificadoId: GrupoId,
                                Indirectos: IndirectosId,
                                E500: E500Id,
                                distribucionProyectos: _this.state.distribucionProyectos
                              })
                              .then((response) => {
                                console.log("Ditribucion INdirectos Completa");
                                axios
                                  .post(urlViatiacos, {
                                    GrupoCertificadoId: GrupoId,
                                    distribucionProyectos: _this.state.distribucionProyectos
                                  })
                                  .then((response) => {

                                    console.log("Actualizacion Completa");
                                    console.log("Distribucion Viaticos Completa");
                                    axios
                                      .post(urlActualizarCabecerasGrupo, {
                                        GrupoCertificadoId: GrupoId,
                                      })
                                      .then((response) => {
                                        console.log("Actualizacion Completa");
                                        _this.props.unlockScreen();
                                        _this.obtenerGruposCertificados();
                                        _this.onHideFormulario(false);
                                      })
                                      .catch((error) => {
                                        console.log(error);
                                        _this.props.unlockScreen();
                                      });
                                  })
                                  .catch((error) => {
                                    console.log(error);
                                    _this.props.unlockScreen();
                                  });
                              })
                              .catch((error) => {
                                console.log(error);
                                _this.props.unlockScreen();
                              });


                          })
                          .catch((error) => {
                            console.log(error);
                            _this.props.unlockScreen();
                          });








                      })
                      .catch((error) => {
                        console.log(error);
                        _this.props.unlockScreen();
                      });
                  }
                }
              })
              .catch((error) => {
                console.log(error);
                _this.props.unlockScreen();
              });
          });
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(FRASE_ERROR_GLOBAL);
        this.props.unlockScreen();
      });
  };

  formComponent = () => {
    const footer = (
      <div className="row" align="right">
        <div className="col">
          <div>
            <button className="btn btn-outline-primary mr-4"
              onClick={() => this.renderGeneracion()}
            >
              Continuar
            </button>
            <button
              className="btn btn-outline-primary mr-4"
              type="button" aria-expanded="false"
              aria-controls="collapseExample"
              onClick={() => this.ocultarConfirmacion()}
            >
              Cancelar
            </button>
          </div>
        </div>
      </div>
    );
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col">
                <div className="row">
                  <div className="col">
                    <Field
                      name="FechaInicio"
                      label="Fecha Inicio"
                      required
                      type="date"
                      edit={true}
                      readOnly={false}
                      value={this.state.FechaInicio}
                      onChange={this.handleChangeFechas}
                      error={this.state.errors.FechaInicio}
                    />
                  </div>
                  <div className="col">
                    <Field
                      name="FechaFin"
                      label="Fecha Fin"
                      required
                      type="date"
                      edit={true}
                      readOnly={false}
                      value={this.state.FechaFin}
                      onChange={this.handleChangeFechas}
                      error={this.state.errors.FechaFin}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col">
                    <Field
                      name="ClienteId"
                      required
                      value={this.state.ClienteId}
                      label="Cliente"
                      options={this.state.Clientes}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValueContrato}
                      error={this.state.errors.ClienteId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                  </div>
                  <div className="col">
                    <Field
                      name="FechaCertificado"
                      label="Fecha Certificado"
                      required
                      type="date"
                      edit={true}
                      readOnly={false}
                      value={this.state.FechaCertificado}
                      onChange={this.handleChangeFechas}
                      error={this.state.errors.FechaCertificado}
                    />
                  </div>
                </div>
                <hr />
                <div className="row">
                  <div className="col">
                    {/*<button
                      onClick={() => this.handleSubmitMasivo()}
                      className="btn btn-outline-primary"
                    >
                      Generar Certificado Masivo
                 </button>}
            <button
                      onClick={() => this.handleSubmitActualizarLocacionDirectos()}
                      className="btn btn-outline-primary"
                    >
                      Actualizar Locacion Directos
                 </button>*/}
                    <button
                      onClick={() => this.validarEstadoCertificadoAnterior()}
                      className="btn btn-outline-primary pr-4"
                    >
                      Continuar Generación
                    </button>{" "}
                    <button
                      onClick={() => this.renderListadoGeneral()}
                      className="btn btn-outline-primary"
                    >
                      Cancelar
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <Dialog header="Confirmación" visible={this.state.mostrarConfirmacionContinuarGeneracion} width="350px" footer={footer} minY={70} onHide={this.ocultarConfirmacion} maximizable={true}>
          Existe un certificado anterior no aprobado ({this.state.grupoAnterior.FechaInicioDate} – {this.state.grupoAnterior.FechaFinDate}).
          ¿Desea continuar?
        </Dialog>
      </Fragment>
    );
  };

  detalleGrupoComponent = () => {
    return (
      <Fragment>
        <DetalleGrupo
          detallegrupo={this.state.detalleSeleccionado}
          onHideFormulario={this.onHideFormulario}
          mostrarFormularioGastos={this.mostrarFormularioGastos}
          aprobarCertificado={this.aprobarCertificado}
          certificados={this.state.certificados}
          showSuccess={this.props.showSuccess}
          showWarn={this.props.showWarn}
          blockScreen={this.props.blockScreen}
          unlockScreen={this.props.unlockScreen}
          obtenerDetallesGrupoCertificado={this.obtenerDetallesGrupoCertificado}
          obtenerGastosCertificado={this.obtenerGastosPorCertificado}
          totalHorasActCertificadas={this.state.totalHorasActCertificadas}
          totalHorasAntCertificadas={this.state.totalHorasAntCertificadas}
          totalMontoActCertificado={this.state.totalMontoActCertificado}
          totalMontoAntCertificado={this.state.totalMontoAntCertificado}
          totalMontoDirecto={this.state.totalMontoDirecto}
          totalMontoIndirecto={this.state.totalMontoIndirecto}
          afterColumnFilter={this.afterColumnFilter}
        />
      </Fragment>
    );
  };

  formularioGastosComponent = () => {
    console.log("formularioGastosComponent");
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col">
                <DetalleGasto
                  certificadoSeleccionado={this.state.certificadoSeleccionado}
                  onHideFormulario={this.onHideFormularioGasto}
                  gastos={this.state.gastos}
                  showSuccess={this.props.showSuccess}
                  showWarn={this.props.showWarn}
                  blockScreen={this.props.blockScreen}
                  unlockScreen={this.props.unlockScreen}
                  obtenerGastosCertificado={this.obtenerGastosPorCertificado}
                />
              </div>
            </div>
          </div>
        </div>
      </Fragment>
    );
  };

  formularioSeleccionarDetallesCertificado = () => {
    console.log("seleccionarDetallesFormInit");



    let total =
      this.state.montoTotalDirectos +
      this.state.montoTotalIndirectos +
      this.state.montoTotalE500 +
      this.state.montoTotalDirectosPendientes +
      this.state.montoTotalIndirectosPendientes +
      this.state.montoTotalE500Pendientes;
    console.log("total", total);
    let totalHoras =
      this.state.totalHorasDirectos +
      this.state.totalHorasIndirectos +
      this.state.totalHorasE500 + this.state.totalHorasDirectosPendientes +
      this.state.totalHorasIndirectosPendientes +
      this.state.totalHorasE500Pendientes;

    let sumaHorasDirectos = this.state.totalHorasDirectos + this.state.totalHorasDirectosPendientes;
    let sumaMontoDirectos = this.state.montoTotalDirectos + this.state.montoTotalDirectosPendientes;

    let sumaHorasIndirectos = this.state.totalHorasIndirectos + this.state.totalHorasIndirectosPendientes;
    let sumaMontoIndirectos = this.state.montoTotalIndirectos + this.state.montoTotalIndirectosPendientes;

    let sumaHorasE500 = this.state.totalHorasE500 + this.state.totalHorasE500Pendientes;
    let sumaMontoE500 = this.state.montoTotalE500 + this.state.montoTotalE500Pendientes;

    const options = {
      sizePerPage: 5,

    };
    const Type = {
      true: 'SI',
      false: 'NO',
    };
    return (
      <Fragment>
        <div className="card">
          <div className="card-body">
            <div className="row">
              <div className="col" align="right">
                <Button
                  label="Regresar"
                  className="p-button-outlined"
                  onClick={() => this.mostrarFormulario({})}
                  icon="pi pi-chevron-left"
                />
              </div>
            </div>
            <hr />

            <div className="row">
              <div className="col">
                <p>
                  <b>Fecha Inicio: </b> {this.state.FechaInicio}
                </p>
              </div>
              <div className="col">
                <p>
                  <b>Fecha Fin: </b> {this.state.FechaFin}
                </p>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <p>
                  <b>Fecha Certificado: </b> {this.state.FechaCertificado}
                </p>
              </div>
              <div className="col">
                <p>
                  <p>
                    <b>Porcentaje de Indirectos: </b>{" "}
                    {this.state.montoTotalDirectos > 0
                      ? (
                        (this.state.montoTotalIndirectos /
                          this.state.montoTotalDirectos) *
                        100
                      ).toFixed(3)
                      : 0}
                  </p>
                  {/** <b>Fecha Generación: </b> {this.state.FechaGeneracion}*/}
                </p>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <p>
                  <b>Cliente: </b>{" "}
                  {this.state.ClienteSeleccionado != null &&
                    this.state.ClienteSeleccionado !== undefined
                    ? this.state.ClienteSeleccionado.label
                    : ""}
                </p>
              </div>
            </div>

            <div className="row">
              <div className="col-3">
                <div className="callout callout-info">
                  <small className="text-muted">Total Directos</small>
                  <br />
                  <strong className="h4">
                    {sumaHorasDirectos.toFixed(2)} -
                    <CurrencyFormat
                      value={
                        sumaMontoDirectos.toFixed(2)
                          ? sumaMontoDirectos.toFixed(2)
                          : 0.0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
              <div className="col-3">
                <div className="callout callout-danger">
                  <small className="text-muted">Total Indirectos</small>
                  <br />
                  <strong className="h4">
                    {sumaHorasIndirectos.toFixed(2)} -
                    <CurrencyFormat
                      value={
                        sumaMontoIndirectos.toFixed(2)
                          ? sumaMontoIndirectos.toFixed(2)
                          : 0.0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
              <div className="col-3">
                <div className="callout callout-warning">
                  <small className="text-muted">Total E500</small>
                  <br />
                  <strong className="h4">
                    {sumaHorasE500.toFixed(2)}-
                    <CurrencyFormat
                      value={
                        sumaMontoE500.toFixed(2)
                          ? sumaMontoE500.toFixed(2)
                          : 0.0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
              <div className="col-3">
                <div className="callout callout-success">
                  <small className="text-muted">Total </small>
                  <br />
                  <strong className="h4">
                    {totalHoras.toFixed(2)} -
                    <CurrencyFormat
                      value={total.toFixed(2) ? total.toFixed(2) : 0.0}
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="card">
          <div className="card-body">
            <div className="col" align="right">
              <button
                onClick={() => this.descargarResumen()}
                className="btn btn-outline-warning"
              >
                Validar
              </button>
              {"  "}
              <button
                onClick={() => this.abrirDistribucionProyectos()}
                className="btn btn-outline-primary"
                disabled={this.state.puedeGenerar}
              >
                Continuar generación del Certificado
              </button>
            </div>
            <TabView className="tabview-custom">
              <TabPanel header="Directos">
                <div className="row">
                  <div className="col">
                    <DataTable
                      value={this.state.dataDirectos}
                      header="Directos sin certificar"
                      selection={this.state.directosSeleccionados}
                      style={{ fontSize: "10px" }}
                      paginator={true}
                      dataKey="Id"
                      rows={10}
                      rowsPerPageOptions={[5, 10, 20]}
                      onSelectionChange={(e) =>
                        this.onSelectDirectosSinCertificar(e)
                      }
                    >
                      <Column
                        selectionMode="multiple"
                        headerStyle={{ width: "3em" }}
                      ></Column>
                      <Column
                        field="CodigoProyecto"
                        header="Proyecto"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="nombreColaborador"
                        header="Colaborador"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="formatFechaTrabajo"
                        header="Fecha"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column
                        field="NumeroHoras"
                        header="Horas"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column field="tarifa" header="Tarifa" filter></Column>
                      <Column
                        field="nombreLocacion"
                        header="Locacion"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="nombreEstado"
                        header="Estado"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="esCargaAutomatica"
                        header="Carga"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                    </DataTable>
                  </div>
                </div>
              </TabPanel>
              <TabPanel header="Indirectos">
                <div className="row">
                  <div className="col">
                    <ContextMenu
                      appendTo={document.body}
                      model={this.state.menu}
                      ref={(el) => (this.cm = el)}
                      onHide={() => this.setState({ selectedNodeKey: null })}
                    />
                    <DataTable
                      value={this.state.dataIndirectos}
                      header="Indirectos sin certificar"
                      selection={this.state.indirectosSeleccionados}
                      style={{ fontSize: "10px" }}
                      paginator={true}
                      dataKey="Id"
                      rows={10}
                      rowsPerPageOptions={[5, 10, 20]}
                      onSelectionChange={(e) =>
                        this.onSelectIndirectosSinCertificar(e)
                      }
                      contextMenuSelection={this.state.selectedNodeKey}
                      onContextMenuSelectionChange={(e) =>
                        this.setState({ selectedNodeKey: e.value })
                      }
                      onContextMenu={(e) => this.cm.show(e.originalEvent)}
                    >
                      <Column
                        selectionMode="multiple"
                        headerStyle={{ width: "3em" }}
                      ></Column>
                      <Column
                        field="FechaDesdeString"
                        header="Fecha Desde"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column
                        field="FechaHastaString"
                        header="Fecha Hasta"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column field="mes" header="Mes" filter />
                      <Column
                        field="ColaboradorNombres"
                        header="Colaborador"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column field="ItemNombre" header="Item" filter />

                      <Column
                        field="DiasLaborados"
                        header="Dias Laborados"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column
                        field="HorasLaboradas"
                        header="Horas Laboradas"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column field="tarifa" header="Tarifa" filter></Column>
                      <Column
                        field="ProyectosCodigosString"
                        header="No Distribuir"
                        filter

                      />
                    </DataTable>
                  </div>
                </div>
              </TabPanel>
              <TabPanel header="Directos E500">
                <div className="row">
                  <div className="col">
                    <ContextMenu
                      appendTo={document.body}
                      model={this.state.menu}
                      ref={(el) => (this.cm = el)}
                      onHide={() => this.setState({ selectedNodeKey: null })}
                    />
                    <DataTable
                      value={this.state.dataE500}
                      header="Directos E500"
                      selection={this.state.E500Seleccionados}
                      style={{ fontSize: "10px" }}
                      paginator={true}
                      dataKey="Id"
                      rows={10}
                      rowsPerPageOptions={[5, 10, 20]}
                      onSelectionChange={(e) => this.onSelectE500(e)}
                      contextMenuSelection={this.state.selectedNodeKey}
                      onContextMenuSelectionChange={(e) =>
                        this.setState({ selectedNodeKey: e.value })
                      }
                      onContextMenu={(e) => this.cm.show(e.originalEvent)}
                    >
                      <Column
                        selectionMode="multiple"
                        headerStyle={{ width: "3em" }}
                      ></Column>

                      <Column
                        field="nombreColaborador"
                        header="Colaborador"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="formatFechaTrabajo"
                        header="Fecha"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column
                        field="NumeroHoras"
                        header="Horas"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column field="tarifa" header="Tarifa" filter></Column>
                      <Column
                        field="nombreLocacion"
                        header="Locacion"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="nombreEstado"
                        header="Estado"
                        filterMatchMode="contains"
                        filter
                      ></Column>
                    </DataTable>
                  </div>
                </div>
              </TabPanel>


              <TabPanel header="Directos Pendientes">
                <div className="row">
                  <div className="col">
                    <DataTable
                      value={this.state.dataDirectosPendientes}
                      header="Directos Pendientes de certificar"
                      selection={this.state.directosSeleccionadosPendientes}
                      style={{ fontSize: "10px" }}
                      paginator={true}
                      dataKey="Id"
                      rows={10}
                      rowsPerPageOptions={[5, 10, 20]}
                      onSelectionChange={(e) =>
                        this.onSelectDirectosSinCertificarPendientes(e)
                      }
                    >
                      <Column
                        selectionMode="multiple"
                        headerStyle={{ width: "3em" }}
                      ></Column>
                      <Column
                        field="CodigoProyecto"
                        header="Proyecto"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="nombreColaborador"
                        header="Colaborador"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="formatFechaTrabajo"
                        header="Fecha"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column
                        field="NumeroHoras"
                        header="Horas"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column field="tarifa" header="Tarifa" filter></Column>
                      <Column
                        field="nombreLocacion"
                        header="Locacion"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="nombreEstado"
                        header="Estado"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="esCargaAutomatica"
                        header="Carga"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                    </DataTable>
                  </div>
                </div>
              </TabPanel>

              <TabPanel header="Indirectos Pendientes">
                <div className="row">
                  <div className="col">

                    <DataTable
                      value={this.state.dataIndirectosPendientes}
                      header="Indirectos pendientes certificar"
                      selection={this.state.indirectosSeleccionadosPendientes}
                      style={{ fontSize: "10px" }}
                      paginator={true}
                      dataKey="Id"
                      rows={10}
                      rowsPerPageOptions={[5, 10, 20]}
                      onSelectionChange={(e) =>
                        this.onSelectIndirectosSinCertificarPendientes(e)
                      }

                    >
                      <Column
                        selectionMode="multiple"
                        headerStyle={{ width: "3em" }}
                      ></Column>
                      <Column
                        field="FechaDesdeString"
                        header="Fecha Desde"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column
                        field="FechaHastaString"
                        header="Fecha Hasta"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column field="mes" header="Mes" filter />
                      <Column
                        field="ColaboradorNombres"
                        header="Colaborador"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column field="ItemNombre" header="Item" filter />

                      <Column
                        field="DiasLaborados"
                        header="Dias Laborados"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column
                        field="HorasLaboradas"
                        header="Horas Laboradas"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column field="tarifa" header="Tarifa" filter></Column>
                      <Column
                        field="ProyectosCodigosString"
                        header="No Distribuir"
                        filter

                      />
                    </DataTable>
                  </div>
                </div>
              </TabPanel>

              <TabPanel header="Directos E500 Pendientes">
                <div className="row">
                  <div className="col">

                    <DataTable
                      value={this.state.dataE500Pendientes}
                      header="Directos E500 Pendientes"
                      selection={this.state.E500SeleccionadosPendientes}
                      style={{ fontSize: "10px" }}
                      paginator={true}
                      dataKey="Id"
                      rows={10}
                      rowsPerPageOptions={[5, 10, 20]}
                      onSelectionChange={(e) => this.onSelectE500Pendientes(e)}

                    >
                      <Column
                        selectionMode="multiple"
                        headerStyle={{ width: "3em" }}
                      ></Column>

                      <Column
                        field="nombreColaborador"
                        header="Colaborador"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="formatFechaTrabajo"
                        header="Fecha"
                        filter
                        filterMatchMode="contains"
                      />
                      <Column
                        field="NumeroHoras"
                        header="Horas"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column field="tarifa" header="Tarifa" filter></Column>
                      <Column
                        field="nombreLocacion"
                        header="Locacion"
                        filter
                        filterMatchMode="contains"
                      ></Column>
                      <Column
                        field="nombreEstado"
                        header="Estado"
                        filterMatchMode="contains"
                        filter
                      ></Column>
                    </DataTable>
                  </div>
                </div>
              </TabPanel>
            </TabView>


          </div>
        </div>
        <Dialog header="Parametrización Distribución"
          visible={this.state.mostrarDistribucion}
          style={{ width: '80vw' }} modal={true}
          onHide={() => this.setState({ mostrarDistribucion: false })}>
          <div className="row">
            <div className="col" align="right">
              <button
                onClick={() => this.confirmarGeneracion()}
                className="btn btn-outline-primary"
                disabled={this.state.puedeGenerar}
              >
                Generar Certificado
              </button>
            </div>
          </div>
          <hr></hr>
          <BootstrapTable data={this.state.distribucionProyectos} hover={true} options={options} pagination={true}

          >
            <TableHeaderColumn
              isKey
              dataField="Codigo"
              width={"16%"}
              tdStyle={{
                whiteSpace: "normal",
                textAlign: "left",
                fontSize: "11px",
              }}
              thStyle={{
                whiteSpace: "normal",
                textAlign: "left",
                fontSize: "11px",
              }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}

            >Proyecto
            </TableHeaderColumn>
            <TableHeaderColumn

              dataField="Nombre"
              width={"38%"}
              tdStyle={{
                whiteSpace: "normal",
                textAlign: "left",
                fontSize: "11px",
              }}
              thStyle={{
                whiteSpace: "normal",
                textAlign: "left",
                fontSize: "11px",
              }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}

            >Nombre
            </TableHeaderColumn>
            <TableHeaderColumn formatExtraData={Type} filter={{ type: 'SelectFilter', options: Type }} width={"15%"} dataField='AplicaIndirecto' dataFormat={this.activeFormatter}>Aplica Indirectos</TableHeaderColumn>
            <TableHeaderColumn formatExtraData={Type} filter={{ type: 'SelectFilter', options: Type }} width={"15%"} dataField='AplicaViatico' dataFormat={this.activeFormatterViatico}>Aplica Viáticos</TableHeaderColumn>
            <TableHeaderColumn formatExtraData={Type} filter={{ type: 'SelectFilter', options: Type }} width={"15%"} dataField='AplicaE500' dataFormat={this.activeFormatter500}>Aplica D. E500</TableHeaderColumn>


          </BootstrapTable>
        </Dialog>
        <Dialog
          header="Seleccionar los proyectos que no tendrán distribución"
          visible={this.state.viewdistribucion}
          modal
          style={{ width: "800px" }}
          footer={this.construirBotonesDeDistribucion()}
          onHide={this.onHideFormularioDistribucion}
        >
          <div className="row">
            <div className="col">
              <div className="row">
                <div className="col">
                  <p>
                    <strong>Identificación: </strong>
                    {this.state.indirectoSeleccionado != null
                      ? this.state.indirectoSeleccionado
                        .ColaboradorIdentificacion
                      : ""}
                  </p>
                </div>
                <div className="col">
                  <p>
                    <strong>Nombres: </strong>
                    {this.state.indirectoSeleccionado != null
                      ? this.state.indirectoSeleccionado.ColaboradorNombres
                      : ""}
                  </p>
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <p>
                    <strong>Horas Laboradas: </strong>
                    {this.state.indirectoSeleccionado != null
                      ? this.state.indirectoSeleccionado.HorasLaboradas
                      : ""}
                  </p>
                </div>
                <div className="col"></div>
              </div>
              <div>
                <MultiSelect
                  value={this.state.proyectosSeleccionados}
                  options={this.state.proyectosADistribuir}
                  filter={true}
                  style={{ width: "100%" }}
                  onChange={(e) =>
                    this.setState({ proyectosSeleccionados: e.value })
                  }
                  optionLabel="label"
                  placeholder="seleccionar los proyectos que no tendrán distribución"
                />
              </div>
            </div>
          </div>
        </Dialog>

      </Fragment>
    );
  };

  actualizarValor = (cell, row, esIndirecto) => {
    let nuevoTemporal = this.state.distribucionProyectos;
    let objIndex = nuevoTemporal.findIndex((obj => obj.Id == row.Id));


    if (esIndirecto) {
      nuevoTemporal[objIndex].AplicaIndirecto = !cell;
    } else {
      nuevoTemporal[objIndex].AplicaViatico = !cell;
    }


    this.setState({ distribucionProyectos: nuevoTemporal });
  }
  actualizarValorE500 = (cell, row) => {
    let nuevoTemporal = this.state.distribucionProyectos;
    let objIndex = nuevoTemporal.findIndex((obj => obj.Id == row.Id));



    nuevoTemporal[objIndex].AplicaE500 = !cell;



    this.setState({ distribucionProyectos: nuevoTemporal });
  }
  activeFormatter = (cell, row) => {

    return (
      <ToggleButton onLabel="Si" offLabel="No" onIcon="pi pi-check" offIcon="pi pi-times" checked={cell} onChange={() => this.actualizarValor(cell, row, true)} />
    );
  }
  activeFormatter500 = (cell, row) => {

    return (
      <ToggleButton onLabel="Si" offLabel="No" onIcon="pi pi-check" offIcon="pi pi-times" checked={cell} onChange={() => this.actualizarValorE500(cell, row)} />
    );
  }
  activeFormatterViatico = (cell, row) => {

    return (
      <ToggleButton onLabel="Si" offLabel="No" onIcon="pi pi-check" offIcon="pi pi-times" checked={cell} onChange={() => this.actualizarValor(cell, row, false)} />
    );
  }
  activeFormatterDisable = (cell, row) => {

    return (
      <ToggleButton onLabel="Si" offLabel="No" onIcon="pi pi-check" offIcon="pi pi-times" checked={cell} disabled />
    );
  }
  activeFormatter500Disable = (cell, row) => {

    return (
      <ToggleButton onLabel="Si" offLabel="No" onIcon="pi pi-check" offIcon="pi pi-times" checked={cell} disabled />
    );
  }
  activeFormatterViaticoDisable = (cell, row) => {

    return (
      <ToggleButton onLabel="Si" offLabel="No" onIcon="pi pi-check" offIcon="pi pi-times" checked={cell} disabled />
    );
  }
  afterColumnFilter = async (filterConds, result) => {
    if (result !== null) {
      let totalHorasActCertificadas = 0;
      let totalHorasAntCertificadas = 0;
      let totalMontoActCertificado = 0;
      let totalMontoAntCertificado = 0;
      let totalMontoDirecto = 0;
      let totalMontoIndirecto = 0;

      result.forEach(element => {
        totalHorasActCertificadas += element.HorasActualCertificadas;
        totalHorasAntCertificadas += element.HorasAnteriorCertificadas;
        totalMontoActCertificado += element.MontoActualCertificado;
        totalMontoAntCertificado += element.MontoAnteriorCertificado;
        totalMontoDirecto += element.TotalHorasDirectos;
        totalMontoIndirecto += element.TotalHorasIndirectos;
      });

      if (this.state.isUpdating) {
        this.setState({ isUpdating: false });
        await setTimeout(1000);
        this.setState({
          totalHorasActCertificadas,
          totalHorasAntCertificadas,
          totalMontoActCertificado,
          totalMontoAntCertificado,
          totalMontoDirecto,
          totalMontoIndirecto,
        })

        setTimeout(function () {
          this.setState({ isUpdating: true });
        }.bind(this), 1000);
      }

    }
  };

  validarEstadoCertificadoAnterior = () => {
    this.props.blockScreen();

    axios
      .get(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/ComprobarUltimoCertificadiEstaAprobado`
      )
      .then((response) => {
        console.log(response);
        if (response.data.result.Success === true) {
          this.setState({
            grupoAnterior: response.data.result.Grupo,
            mostrarConfirmacionContinuarGeneracion: true
          });
          this.props.unlockScreen();
        } else {
          this.renderGeneracion();
          this.props.unlockScreen();
        }
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error(
          "Ocurrió un error validando el último certificado",
          "Error"
        );
        this.props.unlockScreen();
      });
  }

  ocultarConfirmacion = () => {
    this.setState({
      mostrarConfirmacionContinuarGeneracion: false,
      grupoAnterior: {}
    });
  }

  renderGeneracion = () => {
    if (!this.isValid()) {
      this.props.showWarn("Existen errores en el formulario", "Validaciones");
      return;
    }
    this.ocultarConfirmacion();
    this.props.blockScreen();
    /** Llame api */
    this.props.blockScreen();
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/ValidarFechasCertificacion`,
        {
          fechaInicio: this.state.FechaInicio,
          fechaFin: this.state.FechaFin,
          fechaCertificado: this.state.FechaCertificado,
          clienteId: this.state.ClienteId,
        }
      )
      .then((response) => {
        console.log(response);
        if (response.data.success === false) {
          abp.notify.warn(response.data.message, "Advertencia");
          this.props.unlockScreen();
        } else {
          this.setState({ screen: "seleccionarDetallesForm" });
          console.log("Action", this.state.action);
          this.obtenerDetallesSinCertificar();
        }
        /*this.setState({ screen: "seleccionarDetallesForm" });
        console.log("Action", this.state.action);
        this.obtenerDetallesSinCertificar();*/
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error(
          "Ocurrió un error validando las fechas ingresadas",
          "Error"
        );
        this.props.unlockScreen();
      });
  };
  obtenerDetallesSinCertificar = () => {
    this.props.blockScreen();
    var self = this;
    Promise.all([this.promiseDetallesSinCertificar()])
      .then(function ([detalles]) {
        let detallesData = detalles.data;
        console.log("detallesData", detallesData);
        if (detallesData) {
          var proyectosADistribuir = detallesData.ProyectosADistribuir.map(
            (item) => {
              return {
                label: item.codigo + " - " + item.nombre_proyecto,
                dataKey: item.Id,
                value: item.Id,
              };
            }
          );
          self.setState({
            dataDirectos: detallesData.Directos,
            dataIndirectos: detallesData.Indirectos,
            proyectosADistribuir: proyectosADistribuir,
            PorcentajeMaximoDistribucion:
              detallesData.PorcentajeMaximoDistribucion,
            dataE500: detallesData.DirectosE500,

            dataDirectosPendientes: detallesData.DirectosPendientes,
            dataE500Pendientes: detallesData.DirectosE500Pendientes,
            dataIndirectosPendientes: detallesData.IndirectosPendientes

          });
        }
        self.props.unlockScreen();
      })
      .catch((error) => {
        self.props.unlockScreen();
        console.log(error);
      });
  };

  obtenerGruposCertificados = () => {
    let fechaInicio = this.state.FechaDesde;
    let fechaFin = this.state.FechaHasta;
    const errors = {};

    if (fechaInicio !== "") {
      if (fechaFin === "") {
        errors.FechaHasta = "Fecha Fin es requerida";
        this.setState({ errors });
        return;
      }
    }

    if (fechaFin !== "") {
      if (fechaInicio === "") {
        errors.FechaDesde = "Fecha Inicio es requerida";
        this.setState({ errors });
        return;
      }
    }
    if (fechaInicio !== "" && fechaFin !== "") {
      let fechaInicioDate = new Date(fechaInicio);
      let fechaFinDate = new Date(fechaFin);
      console.log(fechaInicioDate);
      if (fechaInicioDate > fechaFinDate) {
        errors.FechaHasta = "El campo Fecha Fin debe ser mayor a Fecha Inicio";
        this.setState({ errors });
        return;
      }
    }

    this.setState({ errors });

    this.props.blockScreen();
    var self = this;
    Promise.all([this.promiseGruposCertificados()])
      .then(function ([detalles]) {
        let detallesData = detalles.data;
        if (detallesData.success === true) {
          self.setState({
            detalles: detallesData.result,
          });
        }
        self.props.unlockScreen();
      })
      .catch((error) => {
        self.props.unlockScreen();
        console.log(error);
      });
  };

  borrarFiltros = () => {
    this.setState({ FechaDesde: "", FechaHasta: "" });
  };

  guardarDetalle = () => {
    let ArrayString = "";
    if (this.state.proyectosSeleccionados.length > 0) {
      var items = this.state.proyectosSeleccionados.map((item) => {
        return item.value;
      });
      ArrayString = items.toString();
    }

    console.clear();
    console.log("indirectoSeleccionado", this.state.indirectoSeleccionado);
    console.log("proyectosSeleccionados", this.state.proyectosSeleccionados);
    console.log("ArrayString", this.state.proyectosSeleccionados.toString());

    this.props.blockScreen();
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/GuardarIndirecto`,
        {
          Id: this.state.indirectoSeleccionado.Id,
          proyectos: ArrayString,
        }
      )
      .then((response) => {
        if (response.data == "OK") {
          abp.notify.success("Guardado", "Correcto");
          this.setState({ viewdistribucion: false });
          this.obtenerDetallesSinCertificar();
        } else {
          abp.notify.error(
            "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
            "Error"
          );
        }

        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error(error, "Error");
      });
  };

  eliminarDetalleIndirectoIngenieria = () => {
    this.props.blockScreen();
    let url = "";
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/Delete/${this.state.detalleSeleccionado.Id}`;

    axios
      .post(url, {})
      .then((response) => {
        console.log(response);
        let data = response.data;

        this.props.showSuccess("Eliminado Correctamente");
        this.onHideFormulario(false);
        this.obtenerGruposCertificados();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(FRASE_ERROR_GLOBAL);
        this.props.unlockScreen();
      });
  };

  mostrarDetalleGrupo = (detalle) => {
    console.log("GrupoCertificado", detalle);
    this.setState(
      {
        detalleSeleccionado: detalle,
        screen: "detalleGrupo",
      },
      this.obtenerDetallesGrupoCertificado
    );
  };
  mostrarDistribucionParametros = (detalle) => {
    console.log("GrupoCertificado", detalle);
    this.setState(
      {
        detalleSeleccionado: detalle,
        viewParametrizacion: true,
      },
      this.obtenerParametrizacion(detalle)
    );
  };

  mostrarFormularioGastos = (certificado) => {
    console.log("CertificadoTableRow", certificado);
    this.setState(
      {
        certificadoSeleccionado: certificado,
        screen: "gastosDirectosForm",
      },
      this.obtenerGastosPorCertificado(certificado)
    );
  };

  aprobarCertificado = (Id) => {
    this.props.blockScreen();
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/AprobarCertificado`,
        {
          Id: Id,
        }
      )
      .then((response) => {
        if (response.data == "OK") {
          abp.notify.success("Aprobado", "Correcto");

          this.obtenerDetallesGrupoCertificado();
        } else {
          abp.notify.error(
            "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
            "Error"
          );
        }

        this.props.unlockScreen();
      })
      .catch((error) => {
        this.props.unlockScreen();
        console.log(error);
        abp.notify.error(error, "Error");
      });
  };

  mostrarFormulario = async (detalle) => {
    if (Object.keys(detalle).length === 0) {
      console.log("NUEVO");
      this.setState({
        action: "crear",
        FechaInicio: "",
        FechaFin: "",
        FechaCertificado: "",
        ClienteId: 0,
        dataDirectos: [{}],
        directosSeleccionados: [{}],
        totalHorasDirectos: 0,
        montoTotalDirectos: 0,

        dataIndirectos: [{}],
        indirectosSeleccionados: [{}],
        totalHorasIndirectos: 0,
        montoTotalIndirectos: 0,

        dataE500: [{}],
        E500Seleccionados: [{}],
        totalHorasE500: 0,
        montoTotalE500: 0,

        distribucionProyectos: [],
        mostrarDistribucion: false,
        viewParametrizacion: false
      });
    }
    this.setState({
      detalleSeleccionado: detalle,
      screen: "Form",
    });
  };

  mostrarConfirmacionParaEliminar = (detalle) => {
    this.setState({
      detalleSeleccionado: detalle,
      mostrarConfirmacion: true,
    });
  };

  onHideFormulario = (recargar = false) => {
    this.setState({
      screen: "list",
      detalleSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: Math.random(),
    });
    if (recargar) {
      this.obtenerGruposCertificados();
    }
  };

  mostrarFormularioDistribucion = (rowValue) => {
    this.setState({
      proyectosSeleccionados: [],
    });
    console.log("RowValue", rowValue);
    this.setState({ viewdistribucion: true, indirectoSeleccionado: rowValue });
    console.log("arrayProyectos", rowValue);

    if (
      rowValue.distribucion_proyectos != null &&
      rowValue.distribucion_proyectos != undefined
    ) {
      this.setState(
        {
          proyectosSeleccionados: rowValue.distribucion_proyectos.split(","),
        },
        console.log("this.array", this.state.proyectosSeleccionados)
      );
    }
  };
  onHideFormularioDistribucion = () => {
    this.setState({ viewdistribucion: false });
  };

  onHideFormularioGasto = (recargar = false) => {
    this.setState({
      certificadoSeleccionado: {},
      mostrarConfirmacion: false,
      keyForm: Math.random(),
      screen: "detalleGrupo",
    });
    if (recargar) {
      this.obtenerDetallesGrupoCertificado();
    }
  };

  promiseDetallesSinCertificar = () => {
    let url = "";
    let params = `?fechaInicio=${this.state.FechaInicio}&fechaFin=${this.state.FechaFin}&ClienteId=${this.state.ClienteId}`;
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/ObtenerDetallesSinCertificarPorFechas${params}`;
    return http.get(url);
  };

  promiseGruposCertificados = () => {
    let url = "";
    let params = `?fechaInicio=${this.state.FechaDesde}&fechaFin=${this.state.FechaHasta}`;
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/ObtenerGruposPorFechas${params}`;
    return http.get(url);
  };

  promiseObtenerCertificadosporGrupoId = (id) => {
    let url = "";
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/ObtenerCertificadosPorGrupo/${id}`;
    return http.get(url);
  };
  promiseObtenerParametrizacionporGrupoId = (id) => {
    let url = "";
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/ObtenerParametrizacionPorGrupo/${id}`;
    return http.get(url);
  };

  promiseObtenerGastosporCertificadoId = (id) => {
    let url = "";
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/ObtenerGastosPorCertificado/${id}`;
    return http.get(url);
  };

  obtenerContratos = () => {
    let url = "";
    url = `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_RUBRO}/GetContratos`;
    return http.get(url);
  };

  actualizardetalleSeleccionado = (detalle) => {
    this.setState({ detalleSeleccionado: detalle });
  };

  actualizarPorcentajeSeleccionado = (porcentaje) => {
    this.setState({ porcentajeSeleccionado: porcentaje });
  };

  descargarFormatoCargaMasivaDeTarifas = () => {
    this.props.blockScreen();
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA}/DescargarPlantillaCargaMasivaGastosIndirectos`,
        {},
        { responseType: "arraybuffer" }
      )
      .then((response) => {
        var nombre = response.headers["content-disposition"].split("=");

        const url = window.URL.createObjectURL(
          new Blob([response.data], {
            type:
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          })
        );
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", nombre[1]);
        document.body.appendChild(link);
        link.click();
        this.props.showSuccess("Formato descargado exitosamente");
        this.props.unlockScreen();
        this.setState({
          ContratoId: 0,
        });
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(
          "Ocurrió un error al descargar el archivo, intentalo nuevamente"
        );
        this.props.unlockScreen();
      });
  };

  descargarGrupodeCertificados = (GrupoCertificadoId) => {
    //GrupoCertificadoId
    this.props.blockScreen();
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/DescargarGrupoCertificados/` +
        GrupoCertificadoId,
        {},
        { responseType: "arraybuffer" }
      )
      .then((response) => {
        var nombre = response.headers["content-disposition"].split("=");

        const url = window.URL.createObjectURL(
          new Blob([response.data], {
            type:
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          })
        );
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", nombre[1]);
        document.body.appendChild(link);
        link.click();
        this.props.showSuccess("Generado correctamente");
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(
          "Ocurrió un error al descargar el archivo, intentalo nuevamente"
        );
        this.props.unlockScreen();
      });
  };

  descargarGrupodeCertificados2 = (GrupoCertificadoId) => {
    //GrupoCertificadoId
    this.props.blockScreen();
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/DescargarGrupoCertificados2/` +
        GrupoCertificadoId,
        {},
        { responseType: "arraybuffer" }
      )
      .then((response) => {
        var nombre = response.headers["content-disposition"].split("=");

        const url = window.URL.createObjectURL(
          new Blob([response.data], {
            type:
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          })
        );
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", nombre[1]);
        document.body.appendChild(link);
        link.click();
        this.props.showSuccess("Generado correctamente");
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(
          "Ocurrió un error al descargar el archivo, intentalo nuevamente"
        );
        this.props.unlockScreen();
      });
  };


  descargarResumen = () => {

    let DirectosId = new Array();
    if (this.state.directosSeleccionados.length > 0) {
      DirectosId = this.state.directosSeleccionados.map((select) => {
        return select.Id;
      });
    }
    let IndirectosId = new Array();
    if (this.state.indirectosSeleccionados.length > 0) {
      IndirectosId = this.state.indirectosSeleccionados.map((select) => {
        return select.Id;
      });
    }

    let E500Id = new Array();
    if (this.state.E500Seleccionados.length > 0) {
      E500Id = this.state.E500Seleccionados.map((select) => {
        return select.Id;
      });
    }

    //pendientes
    let DirectosIdPendientes = new Array();
    if (this.state.directosSeleccionadosPendientes.length > 0) {
      DirectosIdPendientes = this.state.directosSeleccionadosPendientes.map((select) => {
        return select.Id;
      });
    }

    let IndirectosIdPendientes = new Array();
    if (this.state.indirectosSeleccionadosPendientes.length > 0) {
      IndirectosIdPendientes = this.state.indirectosSeleccionadosPendientes.map((select) => {
        return select.Id;
      });
    }
    let E500IdPendientes = new Array();
    if (this.state.E500SeleccionadosPendientes.length > 0) {
      E500IdPendientes = this.state.E500SeleccionadosPendientes.map((select) => {
        return select.Id;
      });
    }

    DirectosIdPendientes.forEach((Id) => {
      if (Id !== null && Id != undefined) {
        DirectosId.push(Id);
      }
    });

    //DirectosId.concat(DirectosIdPendientes);
   // IndirectosId.concat(IndirectosIdPendientes);

    IndirectosIdPendientes.forEach((Id) => {
      if (Id !== null && Id != undefined) {
        IndirectosId.push(Id);
      }
    });

   // E500IdPendientes.concat(E500IdPendientes);
    E500IdPendientes.forEach((Id) => {
      if (Id !== null && Id != undefined) {
        E500Id.push(Id);
      }
    });

    //GrupoCertificadoId
    this.props.blockScreen();
    axios
      .post(
        `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_GRUPO_CERTIFICACION}/Resumen/`,
        {

          Id: this.state.Id,
          ClienteId: this.state.ClienteId,
          FechaInicio: this.state.FechaInicio,
          FechaFin: this.state.FechaFin,
          FechaCertificado: this.state.FechaCertificado,
          FechaGeneracion: this.state.FechaGeneracion,
          EstadoId: this.state.EstadoId,
          Directos: DirectosId,
          Indirectos: IndirectosId,
          E500: E500Id

          /*DirectosPendientes:DirectosIdPendientes,
          IndirectosPendientes:IndirectosIdPendientes,
          E500Pendientes:E500IdPendientes*/
        },
        { responseType: "arraybuffer" }
      )
      .then((response) => {

        console.log('Valida', response)
        console.log('Data', response.data)
        if (response.data.byteLength <= 2) {
          console.log('OK', response.data)
          this.setState({ puedeGenerar: false });
          this.props.showSuccess("Validado Correctamente puede continuar con la generación");
          this.props.unlockScreen();
          return;
        } else {
          var nombre = response.headers["content-disposition"].split("=");

          const url = window.URL.createObjectURL(
            new Blob([response.data], {
              type:
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            })
          );
          const link = document.createElement("a");
          link.href = url;
          link.setAttribute("download", nombre[1]);
          document.body.appendChild(link);
          link.click();
          this.props.showWarn("Existen Validaciones revise el archivo adjunto");

        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(
          "Ocurrió un error al descargar el archivo, intentalo nuevamente"
        );
        this.props.unlockScreen();
      });
  };

  onBasicUpload = (event) => {
    var file = event.files[0];

    if (
      file.type ===
      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    ) {
      this.setState({ file });

      this.props.blockScreen();
      var formData = new FormData();
      formData.append("file", event.files[0]);

      axios
        .post(
          `/${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_DETALLES_INDIRECTOS_INGENIERIA}/CargaMasivaDeGastosIndirectosAsync`,
          formData,
          {
            headers: {
              "Content-Disposition": "attachment; filename=template.xlsx",
              "Content-Type":
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            },
            responseType: "arraybuffer",
          }
        )
        .then((response) => {
          var nombre = response.headers["content-disposition"].split("=");

          const url = window.URL.createObjectURL(
            new Blob([response.data], {
              type:
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            })
          );
          const link = document.createElement("a");
          link.href = url;
          link.setAttribute("download", nombre[1]);
          document.body.appendChild(link);
          link.click();
          this.obtenerDetallesIndirectos();
          this.props.showSuccess("Archivo procesado exitosamente");
        })
        .catch((error) => {
          console.log(error);
          this.props.showWarn(
            "Ocurrió un error al subir el archivo, intentalo nuevamente"
          );
          this.props.unlockScreen();
        });
    } else {
      this.props.showWarn("El formato de archivo es incorrecto");
    }
  };

  loadData = () => {
    this.props.blockScreen();
    var self = this;
    Promise.all([this.obtenerContratos()])
      .then(function ([contratos]) {
        let contratosData = contratos.data;
        if (contratosData.success === true) {
          let catalogoContratos = self.props.buildDropdown(
            contratosData.result,
            "nombrecompleto"
          );
          self.setState({
            catalogoContratos,
          });
        }
        self.props.unlockScreen();
      })
      .catch((error) => {
        self.props.unlockScreen();
        console.log(error);
      });
  };

  obtenerParametrizacion = (detalle) => {
    this.props.blockScreen();
    var self = this;
    Promise.all([
      this.promiseObtenerParametrizacionporGrupoId(
        detalle.Id
      ),
    ])
      .then(function ([certificados]) {
        let certificadosData = certificados.data;

        if (certificadosData.success === true) {
          self.setState({
            distribucionProyectos: certificadosData.result,
          });
        }
        self.props.unlockScreen();
      })
      .catch((error) => {
        self.props.unlockScreen();
        console.log(error);
      });

  }
  obtenerDetallesGrupoCertificado = () => {
    this.props.blockScreen();
    var self = this;
    Promise.all([
      this.promiseObtenerCertificadosporGrupoId(
        this.state.detalleSeleccionado.Id
      ),
    ])
      .then(function ([certificados]) {
        let certificadosData = certificados.data;

        if (certificadosData.success === true) {

          let totalHorasActCertificadas = 0;
          let totalHorasAntCertificadas = 0;
          let totalMontoActCertificado = 0;
          let totalMontoAntCertificado = 0;
          let totalMontoDirecto = 0;
          let totalMontoIndirecto = 0;

          certificadosData.result.forEach(element => {
            totalHorasActCertificadas += element.HorasActualCertificadas;
            totalHorasAntCertificadas += element.HorasAnteriorCertificadas;
            totalMontoActCertificado += element.MontoActualCertificado;
            totalMontoAntCertificado += element.MontoAnteriorCertificado;
            totalMontoDirecto += element.TotalHorasDirectos;
            totalMontoIndirecto += element.TotalHorasIndirectos;
          });
          self.setState({
            certificados: certificadosData.result,
            totalHorasActCertificadas,
            totalHorasAntCertificadas,
            totalMontoActCertificado,
            totalMontoAntCertificado,
            totalMontoDirecto,
            totalMontoIndirecto,
          });
        }
        self.props.unlockScreen();
      })
      .catch((error) => {
        self.props.unlockScreen();
        console.log(error);
      });
  };

  obtenerGastosPorCertificado = (certificado) => {
    console.log("CertificadoSeccionado", certificado);
    this.props.blockScreen();
    var self = this;
    Promise.all([this.promiseObtenerGastosporCertificadoId(certificado.Id)])
      .then(function ([gastos]) {
        let gastosData = gastos.data;
        if (gastosData.success === true) {
          self.setState({
            gastos: gastosData.result,
          });
        }
        self.props.unlockScreen();
      })
      .catch((error) => {
        self.props.unlockScreen();
        console.log(error);
      });
  };

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarDetalleIndirectoIngenieria()}
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
  construirBotonesDeDistribucion = () => {
    return (
      <Fragment>
        <Button
          label="Guardar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.guardarDetalle()}
          icon="pi pi-save"
        />
        <Button
          style={{ marginLeft: "0.4em" }}
          label="Cancelar"
          className="p-button-outlined"
          onClick={() => this.onHideFormularioDistribucion()}
          icon="pi pi-ban"
        />
      </Fragment>
    );
  };
}

const Container = Wrapper(CertificacionContainer);
ReactDOM.render(<Container />, document.getElementById("content"));
