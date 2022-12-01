import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";
import moment from "moment";
import { MultiSelect } from "primereact-v2/multiselect";
import { ScrollPanel } from "primereact-v2/scrollpanel";
import { Card } from "primereact-v2/card";
class Cartas extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      vista: "list",
      visible: false,
      errors: {},
      errorscarta: {},
      editable: true,
      editnum_carta: false,
      data: [],
      numtemporal: "",

      /* Formulario */

      action: "create",

      contratos: [],
      ContratoId: 0,
      empresas: [],

      clientes: [],
      types: [],

      ofertascomerciales: [],
      OfertaComercialId: null,
      fecha_emision: null,
      fecha_envio: null,
      Id: 0,
      ClienteId: 0,
      EmpresaId: 0,
      numero_carta: "",
      carta_origen: 0,
      carta_respuesta: 0,
      fecha_recepcion: "",
      //
      asunto: "",
      descripcion: "",
      enviado_por: "",
      requiere_respuesta: 0,
      id_area_responsable: 0,
      categoria: 0,
      estado: true,
      dirigido_a: [],
      copia_a: [],
      copia_oculta: [],
      vigente: true,
      TipoDestinatario: [
        { label: "Proveedores", value: 1 },
        { label: "Terceros", value: 2 },
        { label: "Clientes", value: 3 },
      ],

      tipo_destinatario: 0,

      OpcionesRespuesta: [
        { label: "Si", value: 1 },
        { label: "No", value: 0 },
      ],

      tipos: [
        { label: "Seleccione", value: "0" },
        { label: "Enviadas", value: "1" },
        { label: "Recibidas", value: "2" },
        { label: "No Utilizada", value: "3" },
      ],
      tipo: 0,

      colaboradores: [],
      listcartas: [],

      /* Nuevo User */
      cedula: "",
      apellidos: "",
      nombres: "",
      correo: "",
      actionuser: "create",
      clientes: [],
      visibleuser: false,
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.mostrarForm = this.mostrarForm.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind();
    this.isValid = this.isValid.bind();
    this.isValidN = this.isValidN.bind();
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.EnviarFormularioUser = this.EnviarFormularioUser.bind(this);
  }

  componentDidMount() {
    //this.GetListByContrato(-1, null);
    this.GetCatalogs();
  }
  OcultarFormularioUser = () => {
    this.setState({ visibleuser: false });
  };
  mostrarFormUser = () => {
    this.setState({
      cedula: "",
      apellidos: "",
      nombres: "",
      correo: "",
      ClienteId: 0,
      actionuser: "create",
      visibleuser: true,
    });
  };

  isValid = () => {
    console.log(this.state.dirigido_a.length);
    const errors = {};
    if (!this.state.editnum_carta) {
      if (this.state.fecha_envio == null) {
        errors.fecha_envio = "Campo Requerido";
      }
    }
    if (!this.state.editnum_carta) {
      if (this.state.dirigido_a.length === 0) {
        abp.notify.warn("Campo Dirigido a Requerido");
        return false;
      }
    }
    if (this.state.editnum_carta) {
      if (this.state.numero_carta.length == 0) {
        errors.numero_carta = "Campo Requerido";
      }
    }
    if(this.state.tipo_destinatario==0){
      errors.tipo_destinatario = "Campo Requerido";

    }
    if (this.state.asunto === "") {
      this.setState({ asunto: "" });
    }
    if (this.state.descripcion === "") {
      this.setState({ descripcion: "" });
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  isValidN = () => {
    const errorscarta = {};

    if (this.state.cedula == "") {
      errorscarta.cedula = "Campo Requerido";
    }
    if (this.state.ClienteId == 0) {
      errorscarta.ClienteId = "Campo Requerido";
    }
    if (this.state.apellidos == "") {
      errorscarta.apellidos = "Campo Requerido";
    }
    if (this.state.nombres == "") {
      errorscarta.nombres = "Campo Requerido";
    }
    if (this.state.correo == "") {
      errorscarta.correo = "Campo Requerido";
    }

    this.setState({ errorscarta });
    return Object.keys(errorscarta).length === 0;
  };
  GetList = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerListTransmital", {})
      .then((response) => {
        console.log(response.data);
        this.setState({ data: response.data });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetListByContrato = (tipo, tipod) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/Carta/ObtenerList/", {
        tipo: tipo,
        tipod: tipod,
      })
      .then((response) => {
        console.log("cartalist,numero de carta", response.data);
        this.setState({
          data: response.data.list,
        });
        if (tipo == 1) {
          console.log("TIPO ENVIADA");
          this.setState({
            numero_carta: response.data.numero_carta,
          });
        } else {
          this.setState({ numero_carta: "" });
        }

        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetOfertasByContrato = (Id) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerOfertbyContrato/" + Id, {})
      .then((response) => {
        console.log(response.data);
        var datos = response.data.map((item) => {
          return {
            label: item.codigo + " - " + item.descripcion,
            dataKey: item.Id,
            value: item.Id,
          };
        });
        this.setState({ ofertascomerciales: datos });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetCatalogs = () => {
    axios
      .post("/Proyecto/Carta/ObtenerEmpresas", {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });

        this.setState({ empresas: items });
      })
      .catch((error) => {
        console.log(error);
      });
    axios
      .post("/Proyecto/Carta/ObtenerClientes", {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });

        this.setState({ clientes: items });
      })
      .catch((error) => {
        console.log(error);
      });
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerTransmitalUsers", {})
      .then((response) => {
        console.log(response.data);
        var datos = response.data.map((item) => {
          return {
            label:
              item.apellidos +
              " " +
              item.nombres +
              " ( " +
              item.nombre_cliente +
              " )",
            dataKey: item.Id,
            value: item.Id,
          };
        });

        this.setState({ colaboradores: datos });
      })
      .catch((error) => {
        console.log(error);
      });

    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerTypes", {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ types: items });
      })
      .catch((error) => {
        console.log(error);
      });
    this.props.unlockScreen();
  };

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };

  onChangeValueContrato = (name, value) => {
    this.setState({
      [name]: value,
    });
    console.log("Tipo de Carta", value);
    this.GetListByContrato(value, null);

    if (value == 2) {
      this.setState({ editnum_carta: true });
    } else {
      this.setState({ editnum_carta: false });
    }
  };
  onChangeValueTipoDestinatario = (name, value) => {
    this.setState({
      [name]: value,
    });
    if (this.state.tipo > 0) {
      this.GetListByContrato(this.state.tipo, value);
    } else {
      this.GetListByContrato(-1, value);
    }
  };
  Eliminar = (Id) => {
    console.log(Id);
    axios
      .post("/Proyecto/Carta/ObtenerEliminar/", {
        Id: Id,
      })
      .then((response) => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente");
          this.GetListByContrato(this.state.tipo, null);
        } else if (response.data == "NOPUEDE") {
          this.props.showWarn("No se puedo Eliminar");
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("Ocurrió un Error");
      });
  };

  mostrarForm = (row) => {
    if (row != null && row.Id > 0) {
      var listdirigido = row.listdirigidos.map((item) => {
        return item.Id;
      });

      var listcopia = row.listcopia.map((item) => {
        return item.Id;
      });
      var listcopiaa = row.listcopiaoculta.map((item) => {
        return item.Id;
      });
      this.setState({
        Id: row.Id,
        vista: "form",
        fecha_envio: row.fecha_envio,
        ClienteId: row.ClienteId,
        EmpresaId: row.EmpresaId,
        numero_carta: row.numero_carta,
        carta_origen: row.carta_origen,
        carta_respuesta: row.carta_respuesta,
        fecha_recepcion: row.fecha_recepcion,
        fecha_envio: row.fecha_envio,
        asunto: row.asunto,
        tipo: row.tipo,
        descripcion: row.descripcion,
        enviado_por: row.enviado_por,
        requiere_respuesta: row.requiere_respuesta,
        id_area_responsable: row.id_area_responsable,
        categoria: row.categoria,
        estado: true,
        dirigido_a: listdirigido,
        copia_a: listcopia,
        copia_oculta: listcopiaa,
        vigente: true,
        tipo_destinatario: row.tipo_destinatario,
        action: "edit",
        visible: true,
        editable: false,
      });
    } else {
      if (this.state.tipo > 0) {
        this.setState({
          vista: "form",
          fecha_envio: null,
          Id: 0,
          ClienteId: 0,
          EmpresaId: 0,
          // numero_carta: ,
          carta_origen: 0,
          carta_respuesta: 0,
          fecha_recepcion: "",

          asunto: "",
          descripcion: "",
          enviado_por: ".",
          requiere_respuesta: 0,
          id_area_responsable: 0,
          categoria: 0,
          estado: true,
          dirigido_a: [],
          copia_a: [],
          copia_oculta: [],
          vigente: true,
          tipo_destinatario: 0,
          action: "create",
          visible: true,
          editable: true,
        });
      } else {
        this.props.showWarn("Seleccione un Tipo de Carta");
      }
    }
  };

  Redireccionar = (id) => {
    window.location.href = "/Proyecto/Carta/Details/" + id;
  };
  generarBotones = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-success"
          onClick={() => {
            this.Redireccionar(row.Id);
          }}
          style={{ float: "left", marginRight: "0.3em" }}
        >
          Ver
        </button>
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.mostrarForm(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
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
          title="Eliminar"
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };

  renderShowsTotal(start, to, total) {
    return (
      <p style={{ color: "black" }}>
        De {start} hasta {to}, Total Registros {total}
      </p>
    );
  }
  render() {
    const options = {
      exportCSVText: "Exportar",
      exportCSVSeparator: ";",
      sizePerPage: 10,
      noDataText: "No existen datos registrados",
      sizePerPageList: [
        {
          text: "10",
          value: 10,
        },
        {
          text: "20",
          value: 20,
        },
      ],
      paginationShowsTotal: this.renderShowsTotal, // Accept bool or function
    };
    const tableStyle = { whiteSpace: "normal", fontSize: "11px" };

    if (this.state.vista == "list") {
      return (
        <BlockUi tag="div" blocking={this.state.blocking}>
          <Growl
            ref={(el) => {
              this.growl = el;
            }}
            baseZIndex={1000}
          />
          <div className="row">
            <div className="col" align="left">
              <Field
                name="tipo"
                required
                value={this.state.tipo}
                label="Tipo Carta"
                options={this.state.tipos}
                type={"select"}
                filter={true}
                onChange={this.onChangeValueContrato}
                error={this.state.errors.tipo}
                readOnly={false}
                placeholder="Seleccione.."
                filterPlaceholder="Seleccione.."
              />
            </div>
            {/* <div className="col" align="left">
            <Field
              name="tipo_destinatario"
              required
              value={this.state.tipo_destinatario}
              label="Tipo Destinatario"
              options={this.state.TipoDestinatario}
              type={"select"}
              filter={true}
              onChange={this.onChangeValueTipoDestinatario}
              error={this.state.errors.tipo_destinatario}
              readOnly={false}
              placeholder="Seleccione.."
              filterPlaceholder="Seleccione.."
            />
          </div>
          */}
            <div className="col" align="right" style={{ paddingTop: "35px" }}>
              <div className="row">
                <div className="col">
                  {/*<label>
                  <b>SIGUIENTE CARTA: </b>
                  {this.state.numero_carta}
                </label>*/}
                </div>{" "}
                <div className="col">
                  <button
                    type="button"
                    className="btn btn-outline-primary"
                    icon="fa fa-fw fa-ban"
                    onClick={this.mostrarForm}
                  >
                    Nuevo
                  </button>
                </div>
              </div>
            </div>
          </div>
          <br />
          <div>
            <BootstrapTable
              data={this.state.data}
              hover={true}
              pagination={true}
              options={options}
              exportCSV={true}
              csvFileName={"Cartas_" + new Date().toString() + ".csv"}
            >
              <TableHeaderColumn
                dataField="Id"
                export={false}
                hidden
                tdStyle={tableStyle}
                thStyle={tableStyle}
                tdStyle={{ whiteSpace: "normal" }}
                thStyle={{ whiteSpace: "normal" }}
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                Id
              </TableHeaderColumn>
              <TableHeaderColumn
                export
                csvHeader="Nro de Carta"
                tdStyle={tableStyle}
                thStyle={tableStyle}
                dataField="numero_carta"
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                Código
              </TableHeaderColumn>
              <TableHeaderColumn
                export
                csvHeader="Fecha Envio"
                dataField="formatFechaEnvio"
                tdStyle={tableStyle}
                thStyle={tableStyle}
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                Fecha Envio
              </TableHeaderColumn>
              <TableHeaderColumn
                export
                csvHeader="Fecha Recepción"
                dataField="formatFechaRecepcion"
                tdStyle={tableStyle}
                thStyle={tableStyle}
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                Fecha Recepción
              </TableHeaderColumn>
              <TableHeaderColumn
                export
                csvHeader="Asunto"
                dataField="asunto"
                tdStyle={tableStyle}
                thStyle={tableStyle}
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                Asunto
              </TableHeaderColumn>
              <TableHeaderColumn
                export={false}
                csvHeader="Descripción"
                dataField="descripcion"
                tdStyle={tableStyle}
                thStyle={tableStyle}
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                Descripción
              </TableHeaderColumn>
              <TableHeaderColumn
                export={false}
                dataField="Id"
                width={"15%"}
                isKey
                dataFormat={this.generarBotones.bind(this)}
              ></TableHeaderColumn>
            </BootstrapTable>
          </div>
        </BlockUi>
      );
    } else if ((this.state.vista = "form")) {
      return (
        <Card
          title={
            this.state.action == "create" && !this.state.editnum_carta
              ? "Número de Carta: " + this.state.numero_carta
              : "Edición Carta" + this.state.numero_carta
          }
        >
          <form onSubmit={this.EnviarFormulario}>
            <div className="row">
              {this.state.editnum_carta && (
                <div className="col">
                  <Field
                    name="numero_carta"
                    label="Número Carta"
                    edit={true}
                    readOnly={false}
                    value={this.state.numero_carta}
                    onChange={this.handleChange}
                    error={this.state.errors.numero_carta}
                  />
                </div>
              )}
            </div>
            <div className="row">
              <div className="col">
                <Field
                  name="tipo_destinatario"
                  required
                  value={this.state.tipo_destinatario}
                  label="Tipo Destinatario"
                  options={this.state.TipoDestinatario}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValue}
                  error={this.state.errors.tipo_destinatario}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
              <div className="col">
                <Field
                  name="requiere_respuesta"
                  required
                  value={this.state.requiere_respuesta}
                  label="Requiere Respuesta"
                  options={this.state.OpcionesRespuesta}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValue}
                  error={this.state.errors.requiere_respuesta}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
            </div>
            <div className="row">
              <div className="col">
                <Field
                  name="fecha_envio"
                  label="Fecha Envio"
                  type="date"
                  edit={true}
                  readOnly={false}
                  value={this.state.fecha_envio}
                  onChange={this.handleChange}
                  error={this.state.errors.fecha_envio}
                />
              </div>
              <div className="col">
                <Field
                  name="fecha_recepcion"
                  label="Fecha Recepción"
                  type="date"
                  edit={true}
                  readOnly={false}
                  value={this.state.fecha_recepcion}
                  onChange={this.handleChange}
                  error={this.state.errors.fecha_recepcion}
                />
              </div>
            </div>
            <div className="row">
              <div className="col">
                <Field
                  name="asunto"
                  label="Asunto"
                  edit={true}
                  readOnly={false}
                  value={this.state.asunto}
                  onChange={this.handleChange}
                  error={this.state.errors.asunto}
                />
              </div>
              <div className="col">
                <Field
                  name="descripcion"
                  label="Descripción"
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
                  name="carta_origen"
                  value={this.state.carta_origen}
                  label="Respuesta A:"
                  options={this.state.listcartas}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValue}
                  error={this.state.errors.carta_origen}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
              <div className="col">
                <Field
                  name="carta_respuesta"
                  value={this.state.carta_respuesta}
                  label="Respondida con:"
                  options={this.state.listcartas}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValue}
                  error={this.state.errors.carta_respuesta}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
            </div>
            <div className="row">
              <div className="col">
                <label htmlFor="label">Dirigido a</label>
                <br />
                <MultiSelect
                  value={this.state.dirigido_a}
                  options={this.state.colaboradores}
                  onChange={(e) => this.setState({ dirigido_a: e.value })}
                  style={{ width: "100%" }}
                  defaultLabel="Seleccione.."
                  filter={true}
                  placeholder="Seleccione"
                />{" "}
              </div>
              <div className="col">
                <label htmlFor="label">Con Copia a</label>
                <br />
                <MultiSelect
                  value={this.state.copia_a}
                  options={this.state.colaboradores}
                  onChange={(e) => this.setState({ copia_a: e.value })}
                  style={{ width: "100%" }}
                  defaultLabel="Seleccione.."
                  filter={true}
                  placeholder="Seleccione"
                />
              </div>
            </div>
            <div className="row">
              <div className="col">
                <label htmlFor="label">Copia Oculta a</label>
                <br />
                <MultiSelect
                  value={this.state.copia_oculta}
                  options={this.state.colaboradores}
                  onChange={(e) => this.setState({ copia_oculta: e.value })}
                  style={{ width: "100%" }}
                  defaultLabel="Seleccione.."
                  filter={true}
                  placeholder="Seleccione"
                />{" "}
              </div>
              <div className="col"></div>
            </div>
            <div className="row">
              <div className="col"></div>
              <div className="col"></div>
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
            &nbsp; &nbsp;
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.mostrarFormUser}
            >
              Añadir Usuario
            </button>
          </form>

          <Dialog
            header="Nuevo"
            visible={this.state.visibleuser}
            onHide={this.OcultarFormularioUser}
            modal={true}
            style={{ width: "500px", overflow: "auto" }}
          >
            <div>
              <form onSubmit={this.EnviarFormularioUser}>
                <div className="row">
                  <div className="col">
                    <Field
                      name="cedula"
                      label="Identificación"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.cedula}
                      onChange={this.handleChange}
                      error={this.state.errorscarta.cedula}
                    />
                  </div>
                  <div className="col">
                    <Field
                      name="ClienteId"
                      required
                      value={this.state.ClienteId}
                      label="Usuario"
                      options={this.state.types}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValue}
                      error={this.state.errorscarta.ClienteId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col">
                    <Field
                      name="apellidos"
                      label="Apellidos"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.apellidos}
                      onChange={this.handleChange}
                      error={this.state.errorscarta.apellidos}
                    />
                  </div>
                  <div className="col">
                    <Field
                      name="nombres"
                      label="Nombres"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.nombres}
                      onChange={this.handleChange}
                      error={this.state.errorscarta.nombres}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col">
                    <Field
                      name="correo"
                      label="Correo Electrónico"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.correo}
                      onChange={this.handleChange}
                      error={this.state.errorscarta.correo}
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
                  onClick={this.OcultarFormularioUser}
                >
                  Cancelar
                </button>
              </form>
            </div>
          </Dialog>
        </Card>
      );
    }
  }

  onDownloaImagen = (Id) => {
    return (window.location = `/Proyecto/AvanceObra/Descargar/${Id}`);
  };
  EnviarFormularioUser = (event) => {
    event.preventDefault();

    if (!this.isValidN()) {
      return;
    } else {
      if (this.state.actionuser == "create") {
        axios
          .post("/Proyecto/TransmitalCabecera/ObtenerCrear", {
            Id: 0,
            cedula: this.state.cedula,
            apellidos: this.state.apellidos,
            nombres: this.state.nombres,
            correo: this.state.correo,
            ClienteId: this.state.ClienteId,
          })
          .then((response) => {
            if (response.data == "OK") {
              this.props.showSuccess("Guardado Correctamente");
              this.setState({ visibleuser: false });
              this.GetCatalogs();
            } else if (response.data == "MISMO") {
              this.props.showWarn(
                "Ya existe un usuario con la misma identificación"
              );
            }
          })
          .catch((error) => {
            console.log(error);
            this.props.showWarn("Ocurrió un Error");
          });
      } else {
        console.log(this.state.uploadFile);

        axios
          .post("/Proyecto/TransmitalCabecera/ObtenerEditar", {
            Id: this.state.Id,
            cedula: this.state.cedula,
            apellidos: this.state.apellidos,
            nombres: this.state.nombres,
            correo: this.state.correo,
            ClienteId: this.state.ClienteId,
          })
          .then((response) => {
            if (response.data == "OK") {
              this.props.showSuccess("Guardado Correctamente");
              this.setState({ visible: false });
              this.GetList();
            } else if (response.data == "MISMO") {
              this.props.showWarn(
                "Ya existe un usuario con la misma identificación"
              );
            }
          })
          .catch((error) => {
            console.log(error);
            this.props.showWarn("Ocurrió un Error");
            this.StopLoading();
          });
      }
    }
  };

  EnviarFormulario(event) {
    event.preventDefault();

    if (!this.isValid()) {
      return;
    } else {
      if (this.state.action == "create") {
        axios
          .post("/Proyecto/Carta/ObtenerCrear", {
            Id: 0,
            numero_carta: this.state.editnum_carta
              ? this.state.numero_carta
              : ".",
            carta_origen: this.state.carta_origen,
            carta_respuesta: this.state.carta_respuesta,
            fecha_envio:
              this.state.fecha_envio == null
                ? new Date()
                : this.state.fecha_envio,
            fecha_recepcion: this.state.fecha_recepcion,
            asunto: this.state.asunto,
            descripcion: this.state.descripcion,
            enviado_por: ".",
            requiere_respuesta: this.state.requiere_respuesta,
            id_area_responsable: 0,
            categoria: 0,
            estado: true,
            dirigido_a:
              this.state.dirigido_a != null && this.state.dirigido_a.length > 0
                ? this.state.dirigido_a.toString()
                : "0",
            copia_a:
              this.state.copia_a != null && this.state.copia_a.length > 0
                ? this.state.copia_a.toString()
                : "0",
            copia_oculta:
              this.state.copia_oculta != null &&
              this.state.copia_oculta.length > 0
                ? this.state.copia_oculta.toString()
                : "0",

            vigente: true,

            tipo_destinatario: this.state.tipo_destinatario,
            tipo: this.state.tipo,

            EmpresaId: this.state.EmpresaId,
            ClienteId: this.state.ClienteId,
          })
          .then((response) => {
            console.log(response.data);
            if (response.data == "OK") {
              this.props.showSuccess("Carta Creado");
              this.setState({ visible: false,vista:"form" });
              this.GetListByContrato(this.state.tipo, null);
            } else if (response.data == "EXISTE") {
              this.props.showWarn("Ya existe una carta con el mismo código");
            } else {
              this.props.showWarn("Ocurrió un Error");
            }
          })
          .catch((error) => {
            console.log(error);
            this.props.showWarn("Ocurrió un Error");
          });
      } else {
        axios
          .post("/Proyecto/Carta/ObtenerEditar", {
            Id: this.state.Id,
            numero_carta: this.state.numero_carta,
            carta_origen: this.state.carta_origen,
            carta_respuesta: this.state.carta_respuesta,
            fecha_recepcion: this.state.fecha_recepcion,
            fecha_envio:
              this.state.fecha_envio == null
                ? new Date()
                : this.state.fecha_envio,
            asunto: this.state.asunto,
            descripcion: this.state.descripcion,
            enviado_por: ".",
            requiere_respuesta: this.state.requiere_respuesta,
            id_area_responsable: 0,
            categoria: 0,
            estado: true,
            dirigido_a:
              this.state.dirigido_a != null && this.state.dirigido_a.length > 0
                ? this.state.dirigido_a.toString()
                : "0",
            copia_a:
              this.state.copia_a != null && this.state.copia_a.length > 0
                ? this.state.copia_a.toString()
                : "0",
            copia_oculta:
              this.state.copia_oculta != null &&
              this.state.copia_oculta.length > 0
                ? this.state.copia_oculta.toString()
                : "0",

            vigente: true,

            tipo_destinatario: this.state.tipo_destinatario,
            tipo: this.state.tipo,

            EmpresaId: this.state.EmpresaId,
            ClienteId: this.state.ClienteId,
          })
          .then((response) => {
            console.log(response.data);
            if (response.data == "OK") {
              this.props.showSuccess("Carta  Editado");
              this.setState({ visible: false });
              this.GetListByContrato(this.state.tipo, null);
            } else if (response.data == "EXISTE") {
              this.props.showWarn("Ya Existe una Carta con el mismo código");
            } else {
              this.props.showWarn("Ocurrió un Error");
            }
          })
          .catch((error) => {
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
          uploadFile: uploadFile,
        });
      }
    } else {
      this.setState({ [event.target.name]: event.target.value });
    }
  }

  OcultarFormulario = () => {
    this.setState({ visible: false, vista: "list" });
  };
}
const Container = wrapForm(Cartas);
ReactDOM.render(<Container />, document.getElementById("content"));
