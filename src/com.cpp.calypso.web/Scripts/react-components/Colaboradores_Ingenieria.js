import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import moment from "moment";
import { Dropdown } from "primereact/components/dropdown/Dropdown";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import Field from "./Base/Field-v2";

class Colaboradores_Ingenieria extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: true,
      data: [],
      message: "",
      visible: false,

      contratos: [],

      //formulario
      items_ingenieria: [],
      tipos: [
        { label: "DIRECTO", value: 0 },
        { label: "INDIRECTO", value: 1 }
      ],
      Id: 0,
      numero_identificacion: "",
      nombres: "",
      apellidos: "",
      ContratoId: 0,
      CargoId: 0,
      vigente: true,
      tipo: 0,
      action: "create",
      errors: {}
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);
    this.alertMessage = this.alertMessage.bind(this);
    this.infoMessage = this.infoMessage.bind(this);

    this.Loading = this.Loading.bind(this);
    this.StopLoading = this.StopLoading.bind(this);

    this.ConsultarColaboradores = this.ConsultarColaboradores.bind(this);
    this.ConsultarByContrato = this.ConsultarByContrato.bind(this);
    this.getContratos = this.getContratos.bind(this);
    this.GetColaboradorporcedula = this.GetColaboradorporcedula.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.isValid = this.isValid.bind(this);
  }

  componentDidMount() {
   // this.ConsultarColaboradores();
    this.getContratos();
  }

  isValid() {
    const errors = {};

    if (this.state.CargoId == 0) {
      errors.CargoId = "Campo requerido";
    }
    if (this.state.numero_identificacion === "") {
      errors.numero_identificacion = "Campo requerido";
    }
    if (this.state.apellidos === "") {
      errors.apellidos = "Campo requerido";
    }
    if (this.state.nombres === "") {
      errors.nombres = "Campo requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }

  GenerarBotones(cell, row) {
    return (
      <div>
        <button
          onClick={() => this.Redireccionar(row)}
          className="btn btn-sm btn-outline-indigo"
        >
          Ver
        </button>
      </div>
    );
  }
  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };
  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  render() {
    return (
      <div>
        <BlockUi tag="div" blocking={this.state.blocking}>
          <Growl
            ref={el => {
              this.growl = el;
            }}
            position="bottomright"
            baseZIndex={1000}
          ></Growl>
          <div className="col" align="left">
            <div className="form-group">
              <label htmlFor="label">Contrato</label>
              <Dropdown
                value={this.state.ContratoId}
                options={this.state.contratos}
                onChange={e => {
                  this.setState({ ContratoId: e.value }),
                    this.ConsultarByContrato(e.value);
                  this.Consultaritems(e.value);
                }}
                filter={true}
                filterPlaceholder="Selecciona un Contrato"
                filterBy="label,value"
                placeholder="Selecciona un Contrato"
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
              Nuevo
            </button>
          </div>
          <br></br>
          <BootstrapTable data={this.state.data} hover={true} pagination={true}>
            <TableHeaderColumn
              dataField="any"
              dataFormat={this.Secuencial}
              width={"8%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            >
              Nº
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="codigoCargo"
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Código
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="nombreCargo"
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Cargo
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="numero_identificacion"
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Nro. Ident
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="apellidos"
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Apellidos
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="nombres"
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Nombres
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="tipoColaborador"
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Tipo
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Id"
              isKey
              width={"15%"}
              dataFormat={this.generateButton.bind(this)}
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
            >
              Operaciones
            </TableHeaderColumn>
          </BootstrapTable>

          <Dialog
            header="Información Colaborador"
            visible={this.state.visible}
            width="600px"
            modal={true}
            onHide={this.OcultarFormulario}
          >
            <form onSubmit={this.EnviarFormulario} style={{ height: "400px" }}>
              <div className="row">
                <div className="col">
                  <Field
                    name="CargoId"
                    required
                    value={this.state.CargoId}
                    label="Cargos"
                    options={this.state.items_ingenieria}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.CargoId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <Field
                    name="apellidos"
                    label="Apellidos"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.apellidos}
                    onChange={this.handleChange}
                    error={this.state.errors.apellidos}
                  />

                  <Field
                    name="tipo"
                    required
                    value={this.state.tipo}
                    label="Tipo"
                    options={this.state.tipos}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.tipo}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="numero_identificacion"
                    label="Identificación"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.numero_identificacion}
                    onChange={this.handleChange}
                    error={this.state.errors.numero_identificacion}
                  />

                  <Field
                    name="nombres"
                    label="Nombres"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.nombres}
                    onChange={this.handleChange}
                    error={this.state.errors.nombres}
                  />
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
      </div>
    );
  }

  generateButton(cell, row) {
    return (
      <div>
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormulario(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.Eliminar(row.Id);
          }}
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  }

  ConsultarColaboradores() {
    axios
      .post("/Proyecto/Colaborador/Obtain", {})
      .then(response => {
        console.log(response.data);
        this.setState({ data: response.data, blocking: false });
      })
      .catch(error => {
        this.setState({ blocking: false });
        this.alertMessage("Ocurrió un error al consultar los Colaboradores");
        console.log(error);
      });
  }
  ConsultarByContrato(Id) {
    console.log("consultando");
    axios
      .post("/Proyecto/Colaborador/Obtainby/" + Id, {})
      .then(response => {
        console.log(response.data);
        this.setState({ data: response.data, blocking: false });
      })
      .catch(error => {
        this.setState({ blocking: false });
        this.alertMessage("Ocurrió un error al consultar los Colaboradores");
        console.log(error);
      });
  }
  Consultaritems(e) {
    console.log(e);
    axios
      .post("/Proyecto/Colaborador/ObtainDetails", {
        id: e
      })
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return {
            label: item.Item.codigo + "-" + item.Item.nombre,
            dataKey: item.Id,
            value: item.Id
          };
        });
        this.setState({ items_ingenieria: items });
      })
      .catch(error => {
        this.setState({ blocking: false });
        this.alertMessage("No existe un Preciario Vigente");
        console.log(error);
      });
  }
  getContratos() {
    axios
      .post("/Proyecto/Colaborador/ObtainContratos", {})
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.Codigo, dataKey: item.Id, value: item.Id };
        });
        this.setState({ contratos: items, blocking: false });
      })
      .catch(error => {
        console.log(error);
      });
  }

  GetColaboradorporcedula() {
    console.log(this.state.cedula);
    axios
      .post("/Proyecto/Colaborador/GetUsuarioporcedula", {
        cedula: this.state.cedula
      })
      .then(response => {
        if (response.data != "Error") {
          console.log(response.data);
          this.setState({
            usuario: response.data,
            tcu: response.data.Cuenta,
            nombres: response.data.Apellidos + " " + response.data.Nombres,
            usuario_i: response.data.Id
          });
        } else {
          this.setState({ usuario: null, tcu: "", nombres: "", usuario_i: 0 });
        }
      })
      .catch(error => {
        console.log(error);
      });
  }

  EnviarFormulario(event) {
    event.preventDefault();

    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      if (this.state.ContratoId > 0) {
        if (this.state.action == "create") {
          axios
            .post("/proyecto/Colaborador/ObtainCrear", {
              Id: this.state.Id,
              numero_identificacion: this.state.numero_identificacion,
              apellidos: this.state.apellidos,
              nombres: this.state.nombres,
              ContratoId: this.state.ContratoId,
              CargoId: this.state.CargoId,
              tipo: this.state.tipo,
              vigente: true
            })
            .then(response => {
              if ((response.data = "OK")) {
                this.infoMessage("Colaborador Creado");
                this.setState({ visible: false });
                if (this.state.ContratoId > 0) {
                  this.ConsultarByContrato(this.state.ContratoId);
                } else {
                  this.ConsultarColaboradores();
                }
              } else {
                this.alertMessage("Ocurrió un Error");
              }
            })
            .catch(error => {
              console.log(error);
              this.alertMessage("Ocurrió un Error");
            });
        } else {
          axios
            .post("/proyecto/Colaborador/ObtainEditar", {
              Id: this.state.Id,
              numero_identificacion: this.state.numero_identificacion,
              apellidos: this.state.apellidos,
              nombres: this.state.nombres,
              ContratoId: this.state.ContratoId,
              CargoId: this.state.CargoId,
              tipo: this.state.tipo,
              vigente: true
            })
            .then(response => {
              if ((response.data = "o")) {
                this.infoMessage("Colaborador Creado");
                this.setState({ visible: false });
                if (this.state.ContratoId > 0) {
                  this.ConsultarByContrato(this.state.ContratoId);
                } else {
                  this.ConsultarColaboradores();
                }
              } else {
                this.alertMessage("Ocurrió un Error");
              }
            })
            .catch(error => {
              console.log(error);
              this.alertMessage("Ocurrió un Error");
            });
        }
      } else {
        abp.notify.error("Debe Seleccionar un Contrato", "Validación");
      }
    }
  }

  Eliminar(Id) {
    axios
      .post("/proyecto/Colaborador/ObtainEliminar", {
        id: Id
      })
      .then(response => {
        if ((response.data = "OK")) {
          this.infoMessage("Colaborador Creado");
          this.setState({ visible: false });
          this.ConsultarColaboradores();
        } else {
          this.alertMessage("Ocurrió un Error");
        }
      })
      .catch(error => {
        console.log(error);
        this.alertMessage("Ocurrió un Error");
      });
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  Redireccionar(id) {
    window.location.href = "/Proyecto/OfertaPresupuesto/Details/" + id;
  }

  Loading() {
    this.setState({ blocking: true });
  }

  StopLoading() {
    this.setState({ blocking: false });
  }

  OcultarFormulario() {
    this.setState({ visible: false });
  }
  OcultarFormularioEditar() {
    this.setState({ visibleeditar: false });
  }
  MostrarFormulario(row) {
    console.log(row);

    if (row != null && row.Id > 0) {
      this.Consultaritems(row.ContratoId);
      this.setState({
        Id: row.Id,
        numero_identificacion: row.numero_identificacion,
        apellidos: row.apellidos,
        nombres: row.nombres,
        ContratoId: row.ContratoId,
        CargoId: row.CargoId,
        tipo: row.tipo,
        vigente: true,
        action: "edit",
        visible: true
      });
    } else {
      if (this.state.ContratoId > 0) {
        this.setState({
          Id: 0,
          numero_identificacion: "",
          apellidos: "",
          nombres: "",
          CargoId: 0,
          vigente: true,
          tipo: 0,
          action: "create",
          visible: true
        });
      } else {
        abp.notify.error("Debe Seleccionar un Contrato", "Validación");
      }
    }
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

  showAlert() {
    this.growl.show({
      severity: "warn",
      summary: "Alerta",
      detail: this.state.message
    });
  }

  alertMessage(msg) {
    this.setState({ message: msg }, this.showAlert);
  }
}

ReactDOM.render(
  <Colaboradores_Ingenieria />,
  document.getElementById("content")
);
