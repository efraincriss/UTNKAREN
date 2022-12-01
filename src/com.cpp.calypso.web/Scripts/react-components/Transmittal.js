import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import moment from "moment";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import { Dropdown } from "primereact/components/dropdown/Dropdown";
import { MultiSelect } from "primereact/components/multiselect/MultiSelect";

class Transmital extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      //tablas
      blocking: true,
      data: [],
      visible: false,

      //Datos Formulario
      contratos: [],
      ContratoId: 0,
      empresas: [],
      EmpresaId: null,
      clientes: [],
      ClienteId: null,
      ofertascomerciales: [],
      OfertaComercialId: null,
      fecha_emision: moment(new Date()).format("YYYY-MM-DD"),

      //
      version: "",
      enviado_por: "",

      descripcion: "",
      dirigido_a: [],
      copia_a: [],

      tipo_formatos: [
        { label: "Papel", value: "P" },
        { label: "Informático", value: "I" },
        { label: "Extranet", value: "X" }
      ],
      tipo_formato: "I",
      tipo_propositos: [
        { label: "Información", value: "PI" },
        { label: "Compras", value: "PC" },
        { label: "Aprobación", value: "PA" }
      ],
      tipo_proposito: "PA",
      tipos: [
        { label: "Minuta de Reunión", value: "MI" },
        { label: "Consulta Técnica", value: "TQ" },
        { label: "Procedimiento", value: "PR" },
        { label: "Carta", value: "LT" },
        { label: "Comercial", value: "CO" },
        { label: "Otros", value: "OT" }
      ],
      tipo: "CO",

      colaboradores: []
    };

    this.handleChange = this.handleChange.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);
    this.alertMessage = this.alertMessage.bind(this);
    this.infoMessage = this.infoMessage.bind(this);
    this.warnMessage = this.warnMessage.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);

    this.GenerarBotones = this.GenerarBotones.bind(this);

    //ofertaComercial
    this.ConsultarTransmital = this.ConsultarTransmital.bind(this);
    this.ConsultarTransmitalC = this.ConsultarTransmitalC.bind(this);
    this.ConsultarEmpresasClientes = this.ConsultarEmpresasClientes.bind(this);
    this.ConsultarColaboradores = this.ConsultarColaboradores.bind(this);
    this.ConsultarOfertasComerciales = this.ConsultarOfertasComerciales.bind(
      this
    );
    this.Redireccionar = this.Redireccionar.bind(this);
    this.limpiarcampos = this.limpiarcampos.bind(this);

    //
  }

  componentDidMount() {
    this.ConsultarTransmital();
    this.getContratos();
    this.ConsultarColaboradores();
  }

  GenerarBotones(cell, row) {
    return (
      <div>
        <button
          className="btn btn-outline-success btn-sm"
          onClick={() => {
            this.Redireccionar(row.Id);
          }}
          style={{ float: "left", marginRight: "0.3em" }}
        >
          Ver
        </button>
      </div>
    );
  }
  render() {
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={el => {
            this.growl = el;
          }}
          position="bottomright"
          baseZIndex={1000}
        ></Growl>

        <div className="row">
          <div className="col">
            <div className="form-group">
              <label htmlFor="label">Contrato</label>
              <Dropdown
                value={this.state.ContratoId}
                options={this.state.contratos}
                onChange={e => {
                  this.setState({ ContratoId: e.value }),
                    this.ConsultarTransmitalC(e.value);
                }}
                filter={true}
                filterPlaceholder="Selecciona un Contrato"
                filterBy="label,value"
                placeholder="Selecciona una Contrato"
                style={{ width: "100%" }}
              />
            </div>
          </div>
          <div className="col" align="right">
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.MostrarFormulario}
            >
              Nuevo Transmital
            </button>
          </div>
        </div>

        <br />
        <br />
        <BootstrapTable data={this.state.data} hover={true} pagination={true} >
          <TableHeaderColumn
            dataField="codigo_transmital"
            isKey
            tdStyle={{ whiteSpace: "normal" }}
            thStyle={{ whiteSpace: "normal" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Código
          </TableHeaderColumn>
          
          <TableHeaderColumn
            dataField="code"
            tdStyle={{ whiteSpace: "normal" }}
            thStyle={{ whiteSpace: "normal" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >   Oferta Comercial
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="descripcion"
            tdStyle={{ whiteSpace: "normal" }}
            thStyle={{ whiteSpace: "normal" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Descripción
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="empresa"
            tdStyle={{ whiteSpace: "normal" }}
            thStyle={{ whiteSpace: "normal" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Empresa
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="cliente"
            tdStyle={{ whiteSpace: "normal" }}
            thStyle={{ whiteSpace: "normal" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Cliente
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="fecha_emision"
            dataFormat={this.DateFormat}
            tdStyle={{ whiteSpace: "normal" }}
            thStyle={{ whiteSpace: "normal" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Fecha Emisión
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="Operaciones"
            width={"10%"}
            dataFormat={this.GenerarBotones.bind(this)}
          ></TableHeaderColumn>
        </BootstrapTable>

        <Dialog
          header="Generación de Transmital"
          visible={this.state.visible}
          width="900px"
          height="580px"
          modal={true}
          onHide={this.OcultarFormulario}
        >
          <form onSubmit={this.EnviarFormulario}>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="label">Empresa</label>
                  <Dropdown
                    value={this.state.EmpresaId}
                    options={this.state.empresas}
                    onChange={e => {
                      this.setState({ EmpresaId: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Seleccione Empresa"
                    filterBy="label,value"
                    placeholder="Seleccione Empresa"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="label">Ofertas Comerciales</label>
                  <Dropdown
                    value={this.state.OfertaComercialId}
                    options={this.state.ofertascomerciales}
                    onChange={e => {
                      this.setState({ OfertaComercialId: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona una Oferta Comercial"
                    filterBy="label,value"
                    placeholder="Selecciona una Oferta Comercial"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="label">Formato</label>
                  <Dropdown
                    value={this.state.tipo_formato}
                    options={this.state.tipo_formatos}
                    onChange={e => {
                      this.setState({ tipo_formato: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Formato"
                    filterBy="label,value"
                    placeholder="Selecciona Formato"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="label">Dirigido a</label>
                  <MultiSelect
                    value={this.state.dirigido_a}
                    options={this.state.colaboradores}
                    onChange={e => this.setState({ dirigido_a: e.value })}
                    style={{ width: "100%" }}
                    filter={true}
                    defaultLabel="Selecciona  Usuarios"
                    className="form-control"
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Con Copia a</label>
                  <MultiSelect
                    value={this.state.copia_a}
                    options={this.state.colaboradores}
                    onChange={e => this.setState({ copia_a: e.value })}
                    style={{ width: "100%" }}
                    filter={true}
                    defaultLabel="Selecciona  Usuarios"
                    className="form-control"
                  />
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="label">Cliente</label>
                  <Dropdown
                    value={this.state.ClienteId}
                    options={this.state.clientes}
                    onChange={e => {
                      this.setState({ ClienteId: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Seleccione Cliente"
                    filterBy="label,value"
                    placeholder="Seleccione Cliente"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Propósito</label>
                  <Dropdown
                    value={this.state.tipo_proposito}
                    options={this.state.tipo_propositos}
                    onChange={e => {
                      this.setState({ tipo_proposito: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Propósito"
                    filterBy="label,value"
                    placeholder="Selecciona Propósito"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Tipo</label>
                  <Dropdown
                    value={this.state.tipo}
                    options={this.state.tipos}
                    onChange={e => {
                      this.setState({ tipo: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Tipo"
                    filterBy="label,value"
                    placeholder="Selecciona Tipo"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>

                <div className="form-group">
                  <label>Descripción</label>
                  <input
                    type="text"
                    name="descripcion"
                    className="form-control"
                    onChange={this.handleChange}
                    value={this.state.descripcion}
                  />
                </div>

                <div className="form-group">
                  <label>Fecha Transmital</label>
                  <input
                    type="date"
                    id="no-filter"
                    name="fecha_emision"
                    className="form-control"
                    onChange={this.handleChange}
                    value={this.state.fecha_emision}
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
              </div>
            </div>
            <button
              type="submit"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-folder-open"
              style={{ marginRight: "0.3em" }}
            >
              Guardar
            </button>
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.OcultarFormulario}
            >
              Cancelar
            </button>
          </form>
        </Dialog>
      </BlockUi>
    );
  }

  EnviarFormulario(event) {
    event.preventDefault();
    if (this.state.ContratoId > 0) {
      if (this.state.EmpresaId > 0) {
        if (this.state.ClienteId > 0) {
          if (this.state.dirigido_a.length > 0) {
            axios
              .post("/proyecto/TransmitalCabecera/CrearTransmital", {
                Id: 0,
                ContratoId: this.state.ContratoId,
                EmpresaId: this.state.EmpresaId,
                ClienteId: this.state.ClienteId,
                OfertaComercialId: this.state.OfertaComercialId,
                fecha_emision: this.state.fecha_emision,
                tipo: this.state.tipo,
                tipo_formato: this.state.tipo_formato,
                tipo_proposito: this.state.tipo_proposito,
                descripcion: this.state.descripcion,
                vigente: true,
                dirigido_a:
                  this.state.dirigido_a != null
                    ? this.state.dirigido_a.toString()
                    : "0",
                copia_a:
                  this.state.copia_a != null
                    ? this.state.copia_a.toString()
                    : "0",
                version: "",
                codigo_transmital: "000",
                codigo_carta: "",
                enviado_por: "-",
                vigente: true
              })
              .then(response => {
                console.log(response.data);
                if (response.data == "OK") {
                  this.infoMessage("Transmital Creado");
                  this.setState({ visible: false });
                  this.ConsultarTransmitalC(this.state.ContratoId);
                } else if (response.data == "EXISTE") {
                  this.warnMessage(
                    "Ya Existe un Transmittal para la Oferta Comercial"
                  );
                } else {
                  this.warnMessage("Ocurrió un Error");
                }
              })
              .catch(error => {
                console.log(error);
                this.warnMessage("Ocurrió un Error");
              });
          } else {
            this.warnMessage("El Campo Dirigido a es obligatorio");
          }
        } else {
          this.alertMessage("Debe Seleccionar un Cliente");
        }
      } else {
        this.alertMessage("Debe Seleccionar un Empresa");
      }
    } else {
      this.alertMessage("Debe Seleccionar un Contrato");
    }
  }

  MostrarFormulario() {
    if (this.state.ContratoId > 0) {
      this.ConsultarEmpresasClientes(this.state.ContratoId);
      this.ConsultarOfertasComerciales(this.state.ContratoId);
      this.setState({ visible: true });
    } else {
      this.alertMessage("Debe Seleccionar un Contrato");
    }
  }
  DateFormat(cell, row) {
    if (cell == null) {
      return "DD/MM/YYYY";
    } else {
      var x = cell;
      return moment(x).format("DD/MM/YYYY");
    }
  }

  ConsultarTransmital() {
    this.setState({ blocking: true });
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerListTransmital", {})
      .then(response => {
        console.log("Listado Transmitals");
        console.log(response.data);
        this.setState({ data: response.data, blocking: false });
      })
      .catch(error => {
        this.setState({ blocking: false });
        this.alertMessage("Ocurrió un error al consultar Transmitals");
        console.log(error);
      });
  }
  ConsultarTransmitalC(id) {
    this.setState({ blocking: true });
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerListByContrato/", {
        Id: id
      })
      .then(response => {
        console.log("---------versiones-------------");
        console.log(response.data);
        this.setState({ data: [], data: response.data, blocking: false });
      })
      .catch(error => {
        this.setState({ blocking: false });
        this.alertMessage("Ocurrió un error al consultar");
        console.log(error);
      });
  }
  ConsultarEmpresasClientes(id) {
    console.log("Consultando Empresas");
    this.setState({ blocking: true });
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerEmpresaCliente/", {
        id: id
      })
      .then(response => {
        console.log(response.data);
        var empresa = response.data.Empresas.map(item => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });
        var cliente = response.data.Clientes.map(item => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });
        this.setState({
          empresas: empresa,
          clientes: cliente,
          blocking: false
        });
      })
      .catch(error => {
        this.setState({ blocking: false });
        this.alertMessage("Ocurrió un error al consultar EMpresas y Clientes");
        console.log(error);
      });
  }

  ConsultarColaboradores() {
    this.setState({ blocking: true });
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerTransmitalUsers/", {})
      .then(response => {
        console.log(response.data);
        var colaborador = response.data.map(item => {
          return {
            label:
              item.apellidos +
              " " +
              item.nombres +
              " ( " +
              item.nombre_cliente +
              " )",
            dataKey: item.Id,
            value: item.Id
          };
        });
        this.setState({ colaboradores: colaborador, blocking: false });
      })
      .catch(error => {
        this.setState({ blocking: false });
        this.alertMessage("Ocurrió un error al  Colaboradores");
        console.log(error);
      });
  }

  ConsultarOfertasComerciales(id) {
    this.setState({ blocking: true });
    axios
      .post("/Proyecto/OfertaComercial/ListarporContrato/", {
        Id: id
      })
      .then(response => {
        console.log("---------versiones-------------");
        console.log(response.data);
        var ofertas_comerciales = response.data.map(item => {
          return { label: item.codigo, dataKey: item.Id, value: item.Id };
        });
        this.setState({
          ofertascomerciales: ofertas_comerciales,
          blocking: false
        });
      })
      .catch(error => {
        this.setState({ blocking: false });
        this.alertMessage(
          "Ocurrió un error al consular las ofertas Comerciales"
        );
        console.log(error);
      });
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  Redireccionar(id) {
    window.location.href = "/Proyecto/TransmitalCabecera/Details/" + id;
  }
  limpiarcampos() {
    this.setState({
      //Datos Formulario
      empresas: [],
      EmpresaId: null,
      clientes: [],
      ClienteId: null,
      ofertascomerciales: [],
      OfertaComercialId: null,
      fecha_emision: moment(new Date()).format("YYYY-MM-DD"),

      //
      version: "",
      enviado_por: "",
      dirigido_a: "",
      copia_a: [],
      descripcion: ""
    });
  }

  Loading() {
    this.setState({ blocking: true });
  }

  StopLoading() {
    this.setState({ blocking: false });
  }

  OcultarFormulario() {
    this.setState({ visible: false });
    this.limpiarcampos();
  }

  showInfo() {
    this.growl.show({
      severity: "info",
      summary: "Información",
      detail: this.state.message
    });
  }

  infoMessage(msg) {
    this.setState({ message: msg }, this.showInfo);
  }
  showWarn() {
    this.growl.show({
      life: 5000,
      severity: "error",
      summary: "Error",
      detail: this.state.message
    });
  }
  showAlert() {
    this.growl.show({
      severity: "warn",
      summary: "Alerta",
      detail: this.state.message
    });
  }
  warnMessage(msg) {
    this.setState({ message: msg }, this.showWarn);
  }

  alertMessage(msg) {
    this.setState({ message: msg }, this.showAlert);
  }
  getContratos() {
    this.setState({ blocking: true });
    axios
      .post("/Proyecto/Contrato/GetContratosApi", {})
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.Codigo, dataKey: item.Id, value: item.Id };
        });
        this.setState({ contratos: items, blocking: false });
      })
      .catch(error => {
        console.log(error);
        this.setState({ blocking: false });
      });
  }
}

ReactDOM.render(<Transmital />, document.getElementById("content"));
