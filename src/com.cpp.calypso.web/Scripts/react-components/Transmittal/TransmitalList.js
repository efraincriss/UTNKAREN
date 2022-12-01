import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";
import moment from "moment";
import { MultiSelect } from "primereact-v2/multiselect";
class TransmittalList extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      visible: false,
      errors: {},
      errorscarta: {},
      editable: true,
      data: [],

      /* Formulario */

      action: "create",
      Id: 0,
      contratos: [],
      ContratoId: 0,
      empresas: [],
      EmpresaId: 0,
      clientes: [],
      ClienteId: 0,
      ofertascomerciales: [],
      OfertaComercialId: null,
      fecha_emision: null,

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

      colaboradores: [],

      /* Nuevo User */
      cedula: "",
      apellidos: "",
      nombres: "",
      correo: "",
      ClienteId: 0,
      actionuser: "create",
      clientes: [],
      visibleuser: false,
      types: [],
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.mostrarForm = this.mostrarForm.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind();
    this.isValid = this.isValid.bind();
    this.isValidCarta = this.isValidCarta.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.EnviarFormularioUser = this.EnviarFormularioUser.bind(this);
  }

  componentDidMount() {
    this.GetList();
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
      visibleuser: true
    });
  };

  isValid = () => {
    console.log(this.state.dirigido_a.length);
    const errors = {};

    if (this.state.descripcion == "") {
      errors.descripcion = "Campo Requerido";
    }
    if (this.state.fecha_emision == null) {
      errors.fecha_emision = "Campo Requerido";
    }
    if (this.state.dirigido_a.length === 0) {
      errors.dirigido_a = "Campo Requerido";
      abp.notify.warn("Campo Dirigido a Requerido");
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  isValidCarta = () => {
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
  EnviarFormularioUser = event => {
    event.preventDefault();

    if (!this.isValidCarta()) {
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
            ClienteId: this.state.ClienteId
          })
          .then(response => {
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
          .catch(error => {
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
            ClienteId: this.state.ClienteId
          })
          .then(response => {
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
          .catch(error => {
            console.log(error);
            this.props.showWarn("Ocurrió un Error");
            this.StopLoading();
          });
      }
    }
  };
  GetList = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerListTransmital", {})
      .then(response => {
        console.log(response.data);
        this.setState({
          data: response.data.list,
          codigo_transmital: response.data.codigo_transmital
        });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetListByContrato = Id => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerListByContrato/" + Id, {})
      .then(response => {
        console.log(response.data);
        this.setState({
          data: response.data.list,
          codigo_transmital: response.data.codigo_transmital
        });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetOfertasByContrato = Id => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerOfertbyContrato/" + Id, {})
      .then(response => {
        console.log(response.data);
        var datos = response.data.map(item => {
          return {
            label: item.codigo + " - " + item.version + " - " + item.descripcion,
            dataKey: item.Id,
            value: item.Id
          };
        });
        this.setState({ ofertascomerciales: datos });
        this.state.ofertascomerciales.unshift({
          label: "Seleccione..",
          dataKey: 0,
          value: null
        });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetCatalogs = () => {
    axios
      .post("/Proyecto/Contrato/GetContratosApi", {})
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.Codigo, dataKey: item.Id, value: item.Id };
        });

        this.setState({ contratos: items });
      })
      .catch(error => {
        console.log(error);
      });
    axios
      .post("/Proyecto/Carta/ObtenerClientes", {})
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });

        this.setState({ clientes: items });
      })
      .catch(error => {
        console.log(error);
      });
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerTransmitalUsers", {})
      .then(response => {
        console.log(response.data);
        var datos = response.data.map(item => {
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

        this.setState({ colaboradores: datos });
      })
      .catch(error => {
        console.log(error);
      });
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerTypes", {})
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ types: items });
      })
      .catch(error => {
        console.log(error);
      });
  };

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };

  onChangeValueContrato = (name, value) => {
    this.setState({
      [name]: value
    });
    this.GetListByContrato(value);
    this.GetOfertasByContrato(value);
  };
  Eliminar = Id => {
    console.log(Id);
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerDeleteTransmital/", {
        Id: Id
      })
      .then(response => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente");
          this.GetList();
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
    if (row != null && row.Id > 0) {
      console.clear();

      console.log(row);
      var listdirigido = row.listdirigidos.map(item => {
        return item.Id;
      });

      if (row.listcopia === null) {
        row.listcopia = []
      }
      var listcopia = row.listcopia.map(item => {
        return item.Id;
      });

      this.setState({
        Id: row.Id,
        ContratoId: row.ContratoId,
        EmpresaId: row.EmpresaId,
        ClienteId: row.ClienteId,
        OfertaComercialId: row.OfertaComercialId,
        fecha_emision: row.fecha_emision,
        version: row.version,
        enviado_por: row.enviado_por,
        descripcion: row.descripcion,
        dirigido_a: listdirigido,
        copia_a: listcopia,
        tipo_formato: row.tipo_formato,
        tipo_proposito: row.tipo_proposito,
        tipo: row.tipo,
        action: "edit",
        visible: true,
        editable: false
      });
      this.GetOfertasByContrato(row.ContratoId);
    } else {
      if (this.state.ContratoId > 0) {
        this.setState({
          Id: 0,
          ContratoId: this.state.ContratoId,
          EmpresaId: 0,
          ClienteId: 0,
          OfertaComercialId: null,
          fecha_emision: null,
          version: "",
          enviado_por: "",
          descripcion: "",
          dirigido_a: [],
          copia_a: [],
          tipo_formato: "I",
          tipo_proposito: "PA",
          tipo: "CO",
          action: "create",
          visible: true,
          editable: true
        });
      } else {
        this.props.showWarn("Seleccione un Contrato");
      }
    }
  };

  Redireccionar = id => {
    window.location.href = "/Proyecto/TransmitalCabecera/Details/" + id;
  };
  generarBotones = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-success"
          onClick={() => {
            this.Redireccionar(row.Id);
          }}
          style={{ marginRight: "0.3em" }}
        >
          Ver
        </button>
        {
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
        }
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
  render() {
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={el => {
            this.growl = el;
          }}
          baseZIndex={1000}
        />
        <div className="row">
          <div className="col" align="left">
            <Field
              name="ContratoId"
              required
              value={this.state.ContratoId}
              label="Contrato"
              options={this.state.contratos}
              type={"select"}
              filter={true}
              onChange={this.onChangeValueContrato}
              error={this.state.errors.ContratoId}
              readOnly={false}
              placeholder="Seleccione.."
              filterPlaceholder="Seleccione.."
            />
          </div>
          <div className="col" align="right" style={{ paddingTop: "35px" }}>
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
        <br />
        <div>
          <BootstrapTable data={this.state.data} hover={true} pagination={true}>
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
            >
              {" "}
              Oferta Comercial
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="version_oferta_comercial"
              tdStyle={{ whiteSpace: "normal" }}
              thStyle={{ whiteSpace: "normal" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              width={"8%"}
            >
              {" "}
              Versión
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
              dataField="format_fecha_emision"
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
              width={"15%"}
              dataFormat={this.generarBotones.bind(this)}
            ></TableHeaderColumn>
          </BootstrapTable>
        </div>

        <Dialog
          header="Nuevo"
          visible={this.state.visible}
          onHide={this.OcultarFormulario}
          modal={true}
          style={{ width: "900px", overflow: "auto" }}
        >
          <div>
            <form onSubmit={this.EnviarFormulario}>
              {this.state.action == "create" && (
                <label>
                  <b>Código de Transmittal: </b>
                  {this.state.codigo_transmital}
                </label>
              )}
              <div className="row">
                <div className="col">
                  <Field
                    name="OfertaComercialId"
                    value={this.state.OfertaComercialId}
                    label="Oferta Comercial"
                    options={this.state.ofertascomerciales}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.OfertaComercialId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="tipo_formato"
                    value={this.state.tipo_formato}
                    label="Formato"
                    options={this.state.tipo_formatos}
                    type={"select"}
                    filter={true}
                    required
                    onChange={this.onChangeValue}
                    error={this.state.errors.tipo_formato}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="tipo_proposito"
                    value={this.state.tipo_proposito}
                    label="Propósito"
                    options={this.state.tipo_propositos}
                    type={"select"}
                    filter={true}
                    required
                    onChange={this.onChangeValue}
                    error={this.state.errors.tipo_proposito}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="tipo"
                    value={this.state.tipo}
                    label="Tipo"
                    options={this.state.tipos}
                    type={"select"}
                    filter={true}
                    required
                    onChange={this.onChangeValue}
                    error={this.state.errors.tipo}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="descripcion"
                    label="Descripción"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.descripcion}
                    onChange={this.handleChange}
                    error={this.state.errors.nombres}
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
                    onChange={e => this.setState({ dirigido_a: e.value })}
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
                    onChange={e => this.setState({ copia_a: e.value })}
                    style={{ width: "100%" }}
                    defaultLabel="Seleccione.."
                    filter={true}
                    placeholder="Seleccione"
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="fecha_emision"
                    label="Fecha Emisión"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fecha_emision}
                    onChange={this.handleChange}
                    error={this.state.errors.fecha_emision}
                  />
                </div>
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
              &nbsp;
              <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                onClick={this.mostrarFormUser}
              >
                Añadir Usuario
              </button>
            </form>
          </div>
        </Dialog>

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
      </BlockUi>
    );
  }

  onDownloaImagen = Id => {
    return (window.location = `/Proyecto/AvanceObra/Descargar/${Id}`);
  };

  EnviarFormulario(event) {
    event.preventDefault();

    if (!this.isValid()) {
      return;
    } else {
      if (this.state.action == "create") {
        axios
          .post("/Proyecto/TransmitalCabecera/CrearTransmital", {
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
            dirigido_a:
              this.state.dirigido_a != null
                ? this.state.dirigido_a.toString()
                : "0",
            copia_a:
              this.state.copia_a != null ? this.state.copia_a.toString() : "0",
            version: "",
            codigo_transmital: "000",
            codigo_carta: "",
            enviado_por: "-",
            vigente: true
          })
          .then(response => {
            console.log(response.data);
            if (response.data == "OK") {
              this.props.showSuccess("Transmital Creado");
              this.setState({ visible: false });
              this.GetListByContrato(this.state.ContratoId);
            } else if (response.data == "EXISTE") {
              this.props.showWarn(
                "Ya Existe un Transmittal para la Oferta Comercial"
              );
            } else {
              this.props.showWarn("Ocurrió un Error");
            }
          })
          .catch(error => {
            console.log(error);
            this.props.showWarn("Ocurrió un Error");
          });
      } else {
        axios
          .post("/Proyecto/TransmitalCabecera/EditarTransmital", {
            Id: this.state.Id,
            ContratoId: this.state.ContratoId,
            EmpresaId: this.state.EmpresaId,
            ClienteId: this.state.ClienteId,
            OfertaComercialId: this.state.OfertaComercialId,
            fecha_emision: this.state.fecha_emision,
            tipo: this.state.tipo,
            tipo_formato: this.state.tipo_formato,
            tipo_proposito: this.state.tipo_proposito,
            descripcion: this.state.descripcion,
            dirigido_a:
              this.state.dirigido_a != null
                ? this.state.dirigido_a.toString()
                : "0",
            copia_a:
              this.state.copia_a != null ? this.state.copia_a.toString() : "0",
            version: "",
            codigo_transmital: this.state.codigo_transmital,
            codigo_carta: "",
            enviado_por: "-",
            vigente: true
          })
          .then(response => {
            console.log(response.data);
            if (response.data == "OK") {
              this.props.showSuccess("Transmital Editado");
              this.setState({ visible: false });
              this.GetListByContrato(this.state.ContratoId);

            } else if (response.data == "EXISTE") {
              this.props.showWarn(
                "Ya Existe un Transmittal para la Oferta Comercial"
              );
            } else {
              this.props.showWarn("Ocurrió un Error");
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
const Container = wrapForm(TransmittalList);
ReactDOM.render(<Container />, document.getElementById("content"));
