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
import { Checkbox } from "primereact-v2/checkbox";
import { ScrollPanel } from "primereact-v2/scrollpanel";
import { Card } from "primereact-v2/card";
class CartasPrincipal extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      errors: {},
      vista: "list",
      editable: false,
      editnum_carta: false,
      tipoSeleccionado: null,

      /*Data List*/
      datos: [],

      /*Form */

      CartasExistentes: [],
      ColaboradoresLista: [],

      TiposCarta: [],
      TiposDestinatario: [],
      Clasificaciones: [],

      Id: 0,
      TipoCartaId: 0,
      TipoDestinatarioId: 0,
      ClasificacionId: 0,
      numeroCarta: "",
      fecha: null,
      fechaSello: null,
      asunto: "",
      enviadoPor: "",
      dirigidoA: [],
      requiereRespuesta: false,
      numeroCartaRecibida: "",
      numeroCartaEnviada: "",
      descripcion: "",
      linkCarta: "",
      referencia: "",
      vigente: true,
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.onChangeValueTiposCarta = this.onChangeValueTiposCarta.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
  }

  componentDidMount() {
    this.getCatalogos();
  }
  getList = (TipoCartaId) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/Carta/ObtenerList", {
        TipoCartaId: TipoCartaId,
      })
      .then((response) => {
        console.log("getList", response);
        var list = response.data.list;
        var num_secuencial = response.data.numero_carta;

        var tipoSeleccionado = this.state.TiposCarta.filter(
          (c) => c.value == TipoCartaId
        );

        console.log("TipoEncontrado", tipoSeleccionado);
        this.setState({ datos: list });
        if (
          tipoSeleccionado != null &&
          tipoSeleccionado.length > 0 &&
          tipoSeleccionado[0].code === "CARTA_ENVIADA"
        ) {
          this.setState({
            numeroCarta: num_secuencial,
            tipoSeleccionado: tipoSeleccionado[0],
          });
        } else {
          this.setState({
            tipoSeleccionado: tipoSeleccionado[0],
          });
        }

        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  getListCartas = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/Carta/ObtenerListExistentes", {})
      .then((response) => {
        console.log("GetList", response.data);
        this.setState({
          ColaboradoresLista: response.data.Dirigidos,
        });
        if (this.state.action !== "edit") {
          this.setState({
            enviadoPor: response.data.Usuario,
          });
        }

        if (
          this.state.tipoSeleccionado != null &&
          this.state.tipoSeleccionado.code !== "CARTA_ENVIADA"
        ) {
          this.setState({
            numeroCarta: "",
          });
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

  getCatalogos = () => {
    axios
      .post("/Proyecto/Carta/GetByCodeApi/?code=TIPO_CARTA", {})
      .then((response) => {
        console.log("TIPO_CARTA", response);
        var result = response.data.result.map((item) => {
          return {
            label: item.nombre,
            dataKey: item.Id,
            value: item.Id,
            code: item.codigo,
          };
        });
        this.setState({ TiposCarta: result });
      })
      .catch((error) => {
        console.log(error);
      });
    axios
      .post("/Proyecto/Carta/GetByCodeApi/?code=TIPO_DESTINATARIO_CARTA", {})
      .then((response) => {
        console.log("TIPO_DESTINATARIO_CARTA", response);
        var result = response.data.result.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ TiposDestinatario: result });
      })
      .catch((error) => {
        console.log(error);
      });
    axios
      .post("/Proyecto/Carta/GetByCodeApi/?code=CLASIFICACION_CARTA", {})
      .then((response) => {
        console.log("CLASIFICACION_CARTA", response);
        var result = response.data.result.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Clasificaciones: result });
      })
      .catch((error) => {
        console.log(error);
      });
    this.props.unlockScreen();
  };
  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };
  handleChange = (event) => {
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
  };
  onChangeValueTiposCarta = (name, value) => {
    this.setState({
      [name]: value,
    });
    console.log("TIPO_CARTA_SELECT", value);
    this.getList(value);
  };

  renderShowsTotal(start, to, total) {
    return (
      <p style={{ color: "black" }}>
        De {start} hasta {to}, Total Registros {total}
      </p>
    );
  }
  generarBotones = (cell, row) => {
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
        <button
          className="btn btn-outline-info btn-sm"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.mostrarForm(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger btn-sm"
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
  Redireccionar = (id) => {
    window.location.href = "/Proyecto/Carta/Details/" + id;
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
          this.getList(this.state.TipoCartaId);
        } else if (response.data == "NOPUEDE") {
          this.props.showWarn("No se puedo Eliminar");
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("Ocurrió un Error");
      });
  };

  OcultarFormulario = () => {
    this.setState({ vista: "list" });
  };
  mostrarForm = (row) => {
    this.getListCartas();
    console.log(row);
    if (row != null && row.Id > 0) {
      let dirigidoA = [];
      if (row.dirigidoA.includes(",")) {
        dirigidoA = row.dirigidoA.split(",");
      } else {
        dirigidoA = row.dirigidoA.split(",");
      }
      console.log("dirigidoA", dirigidoA);

      this.setState({
        Id: row.Id,
        vista: "form",
        action: "edit",
        editable: false,

        TipoCartaId: row.TipoCartaId,
        TipoDestinatarioId: row.TipoDestinatarioId,
        ClasificacionId: row.ClasificacionId,
        numeroCarta: row.numeroCarta,
        fecha: row.fecha,
        fechaSello: row.fechaSello,
        asunto: row.asunto,
        enviadoPor: row.enviadoPor,
        dirigidoA: dirigidoA,
        requiereRespuesta: row.requiereRespuesta,
        numeroCartaRecibida: row.numeroCartaRecibida,
        numeroCartaEnviada: row.numeroCartaEnviada,
        descripcion: row.descripcion,
        linkCarta: row.linkCarta,
        referencia: row.referencia,
        vigente: row.vigente,
      });
    } else {
      if (this.state.TipoCartaId > 0) {
        this.setState({
          vista: "form",
          action: "create",
          editable: true,
          Id: 0,
          //TipoCartaId: 0,
          TipoDestinatarioId: 0,
          ClasificacionId: 0,
          numeroCarta: this.state.numeroCarta,
          fecha: null,
          fechaSello: null,
          asunto: "",
          //enviadoPor: "",
          dirigidoA: "",
          requiereRespuesta: false,
          numeroCartaRecibida: "",
          numeroCartaEnviada: "",
          descripcion: "",
          linkCarta: "",
          referencia: "",
          vigente: true,
        });
      } else {
        this.props.showWarn("Seleccione un Tipo de Carta");
      }
    }
  };
  isValid = () => {
    console.log("TIPOSELECCIONADO", this.state.tipoSeleccionado);
    const errors = {};
    if (this.state.TipoDestinatarioId == 0) {
      console.log("Destinatario", this.state.TipoDestinatarioId);
      errors.TipoDestinatarioId = "Campo Requerido";
    }
    if (this.state.ClasificacionId == 0) {
      console.log("Destinatario", this.state.ClasificacionId);
      errors.ClasificacionId = "Campo Requerido";
    }
    if (
      this.state.tipoSeleccionado != null &&
      this.state.tipoSeleccionado.code === "CARTA_ENVIADA"
    ) {
      console.log("CartaEniada");
      if (this.state.numeroCarta.length == 0) {
        errors.numeroCarta = "Campo Requerido";
      }
      if (
        this.state.dirigidoA !== undefined &&
        this.state.dirigidoA.length === 0
      ) {
        abp.notify.warn("Campo Dirigido a Requerido");
        return false;
      }
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  EnviarFormulario(event) {
    event.preventDefault();
    if (!this.isValid()) {
      return;
    } else {
      this.props.blockScreen();
      if (this.state.action == "create") {
        axios
          .post("/Proyecto/Carta/ObtenerCrear", {
            Id: 0,
            TipoCartaId: this.state.TipoCartaId,
            TipoDestinatarioId: this.state.TipoDestinatarioId,
            ClasificacionId: this.state.ClasificacionId,
            numeroCarta: this.state.numeroCarta,
            fecha: this.state.fecha,
            fechaSello: this.state.fechaSello,
            asunto: this.state.asunto,
            enviadoPor: this.state.enviadoPor,
            dirigidoA: this.state.dirigidoA.toString(),
            requiereRespuesta: this.state.requiereRespuesta,
            numeroCartaRecibida: this.state.numeroCartaRecibida,
            numeroCartaEnviada: this.state.numeroCartaEnviada,
            descripcion: this.state.descripcion,
            linkCarta: this.state.linkCarta,
            referencia: this.state.referencia,
            vigente: this.state.vigente,
          })
          .then((response) => {
            console.log(response.data);
            if (response.data == "OK") {
              this.props.showSuccess("Carta Creado");
              this.setState({ vista: "list" });
              this.getList(this.state.TipoCartaId);
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
            TipoCartaId: this.state.TipoCartaId,
            TipoDestinatarioId: this.state.TipoDestinatarioId,
            ClasificacionId: this.state.ClasificacionId,
            numeroCarta: this.state.numeroCarta,
            fecha: this.state.fecha,
            fechaSello: this.state.fechaSello,
            asunto: this.state.asunto,
            enviadoPor: this.state.enviadoPor,
            dirigidoA:
              this.state.dirigidoA != null && this.state.dirigidoA !== undefined
                ? this.state.dirigidoA.toString()
                : null,
            requiereRespuesta: this.state.requiereRespuesta,
            numeroCartaRecibida: this.state.numeroCartaRecibida,
            numeroCartaEnviada: this.state.numeroCartaEnviada,
            descripcion: this.state.descripcion,
            linkCarta: this.state.linkCarta,
            referencia: this.state.referencia,
            vigente: this.state.vigente,
          })
          .then((response) => {
            console.log(response.data);
            if (response.data == "OK") {
              this.props.showSuccess("Carta  Editado");
              this.setState({ vista: "list" });
              this.getList(this.state.TipoCartaId);
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

    if (this.state.vista === "list") {
      return (
        <div>
          <div className="row">
            <div className="col" align="left">
              <Field
                name="TipoCartaId"
                required
                value={this.state.TipoCartaId}
                label="Tipo Carta"
                options={this.state.TiposCarta}
                type={"select"}
                filter={true}
                onChange={this.onChangeValueTiposCarta}
                error={this.state.errors.TipoCartaId}
                readOnly={false}
                placeholder="Seleccione.."
                filterPlaceholder="Seleccione.."
              />
            </div>

            <div className="col" align="right" style={{ paddingTop: "35px" }}>
              <div className="row">
                <div className="col"></div>{" "}
                <div className="col">
                  <button
                    type="button"
                    className="btn btn-outline-primary"
                    icon="fa fa-fw fa-ban"
                    onClick={this.mostrarForm}
                  >
                    Nueva
                  </button>
                </div>
              </div>
            </div>
          </div>
          <br />
          <div>
            <BootstrapTable
              data={this.state.datos}
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
                export={false}
                csvHeader="Clasificación"
                dataField="nombreClasificacion"
                tdStyle={tableStyle}
                thStyle={tableStyle}
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                Clasificación
              </TableHeaderColumn>
              <TableHeaderColumn
                export
                csvHeader="Nro de Carta"
                tdStyle={tableStyle}
                thStyle={tableStyle}
                dataField="numeroCarta"
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                N° de Carta
              </TableHeaderColumn>
              <TableHeaderColumn
                export
                csvHeader="Fecha Envío/Recepción"
                dataField="formatFecha"
                tdStyle={tableStyle}
                thStyle={tableStyle}
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                Fecha Envío/Recepción
              </TableHeaderColumn>
              <TableHeaderColumn
                export
                csvHeader="Fecha Sello"
                dataField="formatFechaSello"
                tdStyle={tableStyle}
                thStyle={tableStyle}
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                Fecha Sello
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
                csvHeader="Dirigido A"
                dataField="dirigidoA"
                tdStyle={tableStyle}
                thStyle={tableStyle}
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                Dirigido A
              </TableHeaderColumn>
              <TableHeaderColumn
                export={false}
                csvHeader="Enviado Por"
                dataField="enviadoPor"
                tdStyle={tableStyle}
                thStyle={tableStyle}
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
              >
                Enviado Por
              </TableHeaderColumn>

              <TableHeaderColumn
                export={false}
                dataField="Id"
                isKey
                dataFormat={this.generarBotones.bind(this)}
              ></TableHeaderColumn>
            </BootstrapTable>
          </div>
        </div>
      );
    } else if (this.state.vista === "form") {
      return (
        <Card
          subTitle={
            this.state.tipoSeleccionado != null
              ? this.state.tipoSeleccionado.label
              : ""
          }
        >
          <form onSubmit={this.EnviarFormulario}>
            <div className="row">
              <div className="col">
                {this.state.tipoSeleccionado != null &&
                this.state.tipoSeleccionado.code === "CARTA_ENVIADA" ? (
                  <label>
                    <strong>Número Carta: </strong>
                    {this.state.numeroCarta}
                  </label>
                ) : (
                  <Field
                    name="numeroCarta"
                    label="Número Carta"
                    edit={true}
                    readOnly={false}
                    value={this.state.numeroCarta}
                    onChange={this.handleChange}
                    error={this.state.errors.numeroCarta}
                  />
                )}
              </div>
            </div>
            <div className="row">
              <div className="col">
                <Field
                  name="TipoDestinatarioId"
                  required
                  value={this.state.TipoDestinatarioId}
                  label="Tipo Destinatario"
                  options={this.state.TiposDestinatario}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValue}
                  error={this.state.errors.TipoDestinatarioId}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
              <div className="col">
                <Field
                  name="ClasificacionId"
                  required
                  value={this.state.ClasificacionId}
                  label="Clasificación"
                  options={this.state.Clasificaciones}
                  type={"select"}
                  filter={true}
                  onChange={this.onChangeValue}
                  error={this.state.errors.ClasificacionId}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
            </div>
            <div className="row">
              <div className="col">
                <Field
                  name="fecha"
                  label="Fecha Envío/ Recepción"
                  type="date"
                  edit={true}
                  readOnly={false}
                  value={this.state.fecha}
                  onChange={this.handleChange}
                  error={this.state.errors.fecha}
                />
              </div>
              <div className="col">
                <Field
                  name="fechaSello"
                  label="Fecha Sello"
                  type="date"
                  edit={true}
                  readOnly={false}
                  value={this.state.fechaSello}
                  onChange={this.handleChange}
                  error={this.state.errors.fechaSello}
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
                  name="enviadoPor"
                  label="Enviado Por"
                  edit={true}
                  readOnly={false}
                  value={this.state.enviadoPor}
                  onChange={this.handleChange}
                  error={this.state.errors.enviadoPor}
                />{" "}
              </div>
            </div>
            <div className="row">
              <div className="col">
                <Field
                  name="descripcion"
                  label="Descripción"
                  type="TEXTAREA"
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
                  name="numeroCartaEnviada"
                  label="# de Carta Enviada"
                  edit={true}
                  readOnly={false}
                  value={this.state.numeroCartaEnviada}
                  onChange={this.handleChange}
                  error={this.state.errors.numeroCartaEnviada}
                />
              </div>
              <div className="col">
                <Field
                  name="numeroCartaRecibida"
                  label="# de Carta Recibida"
                  edit={true}
                  readOnly={false}
                  value={this.state.numeroCartaRecibida}
                  onChange={this.handleChange}
                  error={this.state.errors.numeroCartaRecibida}
                />
              </div>
            </div>
            <div className="row">
              <div className="col">
                <label htmlFor="label">Dirigido a</label>
                <br />
                <MultiSelect
                  value={this.state.dirigidoA}
                  options={this.state.ColaboradoresLista}
                  onChange={(e) => this.setState({ dirigidoA: e.value })}
                  style={{ width: "100%" }}
                  defaultLabel="Seleccione.."
                  filter={true}
                  placeholder="Seleccione"
                />{" "}
              </div>
            </div>
            <div className="row">
              <div className="col">
                <label htmlFor="label">Requiere Respuesta</label>
                <br />
                <Checkbox
                  checked={this.state.requiereRespuesta}
                  onChange={(e) =>
                    this.setState({ requiereRespuesta: e.checked })
                  }
                />
              </div>
              <div className="col"></div>
            </div>
            <hr />
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
          </form>
        </Card>
      );
    } else {
      return <div>None</div>;
    }
  }
}
const Container = wrapForm(CartasPrincipal);
ReactDOM.render(<Container />, document.getElementById("content"));
