import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";

class TransmittalUserList extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      visible: false,
      errors: {},
      editable: true,
      data: [],
      clientes: [],
      Id: 0,
      cedula: "",
      apellidos: "",
      nombres: "",
      correo: "",
      ClienteId: 0,
      action: "create"
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.mostrarForm = this.mostrarForm.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind();
    this.isValid = this.isValid.bind();
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
  }

  componentDidMount() {
    this.GetList();
    this.GetCatalogs();
  }

  isValid = () => {
    const errors = {};

    if (this.state.cedula == "") {
      errors.cedula = "Campo Requerido";
    }
    if (this.state.ClienteId == 0) {
      errors.ClienteId = "Campo Requerido";
    }
    if (this.state.apellidos == "") {
      errors.apellidos = "Campo Requerido";
    }
    if (this.state.nombres == "") {
      errors.nombres = "Campo Requerido";
    }
    if (this.state.correo == "") {
        errors.correo = "Campo Requerido";
      }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  GetList = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerTransmitalUsers", {})
      .then(response => {
        console.log(response.data);
        this.setState({ data: response.data });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetCatalogs = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerTypes", {})
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ clientes: items });
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

  Eliminar = Id => {
    console.log(Id);
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerDelete/", {
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
      this.setState({
        Id: row.Id,
        cedula: row.cedula,
        apellidos: row.apellidos,
        nombres: row.nombres,
        correo: row.correo,
        ClienteId: row.ClienteId,
        action: "edit",
        visible: true,
        editable: false
      });
    } else {
      this.setState({
        Id: 0,
        cedula: "",
        apellidos: "",
        nombres: "",
        correo: "",
        ClienteId: 0,
        action: "create",
        visible: true,
        editable: true
      });
    }
  };

  generarBotones = (cell, row) => {
    return (
      <div>
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
  render() {
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={el => {
            this.growl = el;
          }}
          baseZIndex={1000}
        />
        <div align="right">
          <button
            type="button"
            className="btn btn-outline-primary"
            icon="fa fa-fw fa-ban"
            onClick={this.mostrarForm}
          >
            Nuevo
          </button>
        </div>
        <br />
        <div>
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
              dataField="cedula"
              width={"12%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Identificación
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="apellidos"
              width={"12%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Apellidos
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="nombres"
              width={"12%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Nombres
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="correo"
              width={"12%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Correo
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="nombre_cliente"
              width={"12%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Usuario
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Id"
              isKey
              width={"10%"}
              dataFormat={this.generarBotones}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            >
              Opciones
            </TableHeaderColumn>
          </BootstrapTable>
        </div>

        <Dialog
          header="Nuevo"
          visible={this.state.visible}
          onHide={this.OcultarFormulario}
          modal={true}
          style={{ width: "500px", overflow: "auto" }}
        >
          <div>
            <form onSubmit={this.EnviarFormulario}>
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
                    error={this.state.errors.cedula}
                  />
                </div>
                <div className="col">
                  <Field
                    name="ClienteId"
                    required
                    value={this.state.ClienteId}
                    label="Usuario"
                    options={this.state.clientes}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.ClienteId}
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
                    error={this.state.errors.apellidos}
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
                    error={this.state.errors.nombres}
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
                    error={this.state.errors.correo}
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
                onClick={this.OcultarFormulario}
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
const Container = wrapForm(TransmittalUserList);
ReactDOM.render(<Container />, document.getElementById("content"));
