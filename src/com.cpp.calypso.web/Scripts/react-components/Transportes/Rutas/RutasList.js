import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import Field from "../../Base/Field-v2";
import wrapForm from "../../Base/BaseWrapper";
import { SECTOR_TRANSPORTE } from "../../Base/Constantes";

import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";
import validationRules from '../../Base/validationRules';

class RutasList extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: true,
      data: [],
      visible: false,
      visibleeditar: false,
      errors: {},

      // Inputs del Formulario
      Identificador: 0,
      lugares: [],
      Codigo: "",
      Nombre: "",
      OrigenId: 0,
      DestinoId: 0,
      Distancia: null,
      Duracion: null,

      Sectores: [],
      SectorId: 0,
      Descripcion: "",
      EstadoId: 0
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleChangeCorreo = this.handleChangeCorreo.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.handleChangeIdentificacion = this.handleChangeIdentificacion.bind(
      this
    );
    this.OcultarFormulario = this.OcultarFormulario.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);
    this.OcultarFormularioEditar = this.OcultarFormularioEditar.bind(this);
    this.MostrarFormularioEditar = this.MostrarFormularioEditar.bind(this);
    this.alertMessage = this.alertMessage.bind(this);
    this.infoMessage = this.infoMessage.bind(this);
    this.Loading = this.Loading.bind(this);
    this.StopLoading = this.StopLoading.bind(this);

    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.EnviarFormularioEditar = this.EnviarFormularioEditar.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);

    //METODOS
    this.ObtenerRutas = this.ObtenerRutas.bind(this);
    this.ObtenerDetalleRuta = this.ObtenerDetalleRuta.bind(this);
    this.ObtenerCatalogos = this.ObtenerCatalogos.bind(this);
    this.generarBotones = this.generarBotones.bind(this);
    this.VaciarCampos = this.VaciarCampos.bind(this);
    this.EliminarRuta = this.EliminarRuta.bind(this);
    this.RedireccionarDetalle = this.RedireccionarDetalle.bind(this);
  }

  componentDidMount() {
    this.ObtenerRutas();
    this.ObtenerCatalogos();
  }

  isValid() {
    const errors = {};
    if (this.state.Codigo != null && this.state.Codigo.length > 20) {
      errors.Codigo = "'Ingresar m??ximo 20 d??gitos";
    }
    if (this.state.Nombre.length == 0) {
      errors.Nombre = "Campo Requerido";
    }
    if (this.state.Nombre != null && this.state.Nombre.length > 100) {
      errors.Nombre = "'Ingresar m??ximo 100 caracteres alfanum??ricos";
    }
    if (this.state.OrigenId <= 0) {
      errors.OrigenId = "Campo Requerido";
    }
    if (this.state.DestinoId <= 0) {
      errors.DestinoId = "Campo Requerido";
    }
    if (this.state.Distancia == null || this.state.Distancia != null && this.state.Distancia <= 0) {
      errors.Distancia = "Campo Requerido mayor a 0";
    }
    if (this.state.Distancia != null && this.state.Distancia > 0) {

      const integer = validationRules["isInt"]([], this.state.Distancia);
      if (!integer) {
        errors.Distancia = 'Ingresar un valor entero';
      }
      const enteros = validationRules["haveSevenEnteros"]([], this.state.Distancia);
      if (!enteros) {
        errors.Distancia = 'Campo requerido m??ximo 7 enteros con 2 decimales';
      }
    }
    if (this.state.SectorId <= 0) {
      errors.SectorId = "Campo Requerido";
    }
    if (this.state.Duracion == null || this.state.Duracion != null && this.state.Duracion <= 0) {
      errors.Duracion = "Campo Requerido mayor a 0";
    } if (this.state.Duracion != null && this.state.Duracion > 0) {
      const integer = validationRules["isInt"]([], this.state.Duracion);
      if (!integer) {
        errors.Duracion = 'Ingresar un valor entero';
      }

    }
    if (this.state.Descripcion != null && this.state.Descripcion.length > 400) {
      errors.Descripcion = "'Ingresar m??ximo 400 caracteres alfanum??ricos";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }
  VaciarCampos() {
    this.setState({
      Codigo: "",
      Nombre: "",
      OrigenId: 0,
      DestinoId: 0,
      Distancia: null,
      Duracion: null,
      SectorId: 0,
      Descripcion: "",

      EstadoId: 0
    });
  }
  ObtenerRutas() {
    this.props.blockScreen();
    axios
      .post("/Transporte/Ruta/ListaRutas", {})
      .then(response => {
        this.setState({ data: response.data.result, blocking: false });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  }

  ObtenerDetalleRuta(Id) {
    axios
      .post("/Transporte/ruta/ObtenerDetallesRuta", {
        Id: Id
      })
      .then(response => {
        if (response.data != "Error") {
          this.setState({
            Identificador: Id,
            Codigo: response.data.Codigo,
            Nombre: response.data.Nombre,
            OrigenId: response.data.OrigenId,
            DestinoId: response.data.DestinoId,
            Distancia: response.data.Distancia,
            Duracion: response.data.Duracion,
            SectorId: response.data.SectorId,
            Descripcion: response.data.Descripcion,

            EstadoId: 0,
            blocking: false
          });
        }
      })
      .catch(error => {
        console.log(error);
      });
  }
  Secuencial(cell, row, enumObject, index) {
    return (<div>{index + 1}</div>)
  }
  ObtenerCatalogos() {
    this.props.blockScreen();
    axios
      .post("/Transporte/ruta/Listalugares", {})
      .then(response => {
        var items = response.data.result.map(item => {
          return { label: item.Nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ lugares: items, blocking: false });
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
    axios
      .get(`/proyecto/catalogo/GetByCodeApi/?code=${SECTOR_TRANSPORTE}`, {})
      .then(response => {
        var items = response.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Sectores: items, blocking: false });
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });

    this.StopLoading();
  }
  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };

  RedireccionarDetalle(Id) {
    console.log(Id);
    window.location.href = "/Transporte/Ruta/Details/" + Id;
  }
  EliminarRuta(Id) {
    console.log(Id);
    axios
      .post("/Transporte/Ruta/Delete", {
        Id: Id
      })
      .then(response => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente");
          this.ObtenerRutas();
        }else if (response.data == "NOPUEDE"){
          this.props.showWarn("No se puede eliminar la Ruta tiene paradas y horarios asignados");
          
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurri?? un Error");
      });
  }

  generarBotones = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-success"
          onClick={() => this.RedireccionarDetalle(row.Id)}
          data-toggle="tooltip"
          data-placement="top"
          title="Asignar Paradas y Horarios"
        >
          <i className="fa fa-eye" />
        </button>
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormularioEditar(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar Ruta"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          onClick={() => {
            if (
              window.confirm(
                `Esta acci??n eliminar?? el registro, ??Desea continuar?`
              )
            )
              this.EliminarRuta(row.Id);
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar Ruta"
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
            onClick={this.MostrarFormulario}
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
              N??
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Codigo"
              width={"10%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              C??digo
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Nombre"
              width={"12%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Nombre
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="NombreOrigen"
              width={"12%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Lugar Origen
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="NombreDestino"
              width={"12%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Lugar Destino
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Id"
              isKey
              width={"15%"}
              dataFormat={this.generarBotones}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            >
              Opciones
            </TableHeaderColumn>
          </BootstrapTable>
        </div>

        <Dialog
          header="Nueva Ruta"
          visible={this.state.visible}
          onHide={this.OcultarFormulario}
          modal={true}
          style={{ maxHeight: '730px', width: '800px', overflow: 'auto' }}
        >
          <div>
            <form onSubmit={this.EnviarFormulario}>
              <div className="row">
                <div className="col">
                  <Field
                    name="Nombre"
                    label="Nombre"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Nombre}
                    onChange={this.handleChange}
                    style={{ textTransform: 'uppercase' }}
                    error={this.state.errors.Nombre}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="OrigenId"
                    required
                    value={this.state.OrigenId}
                    label="Lugar Origen"
                    options={this.state.lugares}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.OrigenId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />

                  <Field
                    name="Distancia"
                    label="Distancia (Km)"
                    type={"number"}
                    step="1"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Distancia}
                    onChange={this.handleChange}
                    error={this.state.errors.Distancia}

                  />


                  <Field
                    name="Duracion"
                    label="Duraci??n (minutos)"
                    type={"number"}
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Duracion}
                    onChange={this.handleChange}
                    step="1"
                    error={this.state.errors.Duracion}

                  />


                </div>

                <div className="col">

                  <Field
                    name="DestinoId"
                    required
                    value={this.state.DestinoId}
                    label="Lugar Destino"
                    options={this.state.lugares}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.DestinoId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <Field
                    name="SectorId"
                    required
                    value={this.state.SectorId}
                    label="Sector"
                    options={this.state.Sectores}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.SectorId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <Field
                    name="Descripcion"
                    label="Descripci??n"
                    edit={true}
                    readOnly={false}
                    value={this.state.Descripcion}
                    onChange={this.handleChange}
                    error={this.state.errors.Descripcion}
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
        <Dialog
          header="Editar Ruta"
          visible={this.state.visibleeditar}
          width="730px"
          modal={true}
          onHide={this.OcultarFormularioEditar}
        >
          <div>
            <form onSubmit={this.EnviarFormularioEditar} height="730px">
              <div className="row">
                <div className="col">
                  <Field
                    name="Codigo"
                    label="Codigo"
                    required
                    edit={true}
                    readOnly={true}
                    value={this.state.Codigo}
                    onChange={this.handleChange}
                    style={{ textTransform: 'uppercase' }}
                    error={this.state.errors.Codigo}
                  />
                  <Field
                    name="OrigenId"
                    required
                    value={this.state.OrigenId}
                    label="Lugar Origen"
                    options={this.state.lugares}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.OrigenId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <Field
                    name="Distancia"
                    label="Distancia"
                    type={"number"}
                    required
                    step="1"
                    edit={true}
                    readOnly={false}
                    value={this.state.Distancia}
                    onChange={this.handleChange}
                    error={this.state.errors.Distancia}
                  />
                  <Field
                    name="Duracion"
                    label="Duraci??n"
                    type={"number"}
                    step="1"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Duracion}
                    onChange={this.handleChange}
                    error={this.state.errors.Duracion}
                  />
                </div>

                <div className="col">
                  <Field
                    name="Nombre"
                    label="Nombre"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Nombre}
                    onChange={this.handleChange}
                    style={{ textTransform: 'uppercase' }}
                    error={this.state.errors.Nombre}
                  />
                  <Field
                    name="DestinoId"
                    required
                    value={this.state.DestinoId}
                    label="Lugar Destino"
                    options={this.state.lugares}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.DestinoId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <Field
                    name="SectorId"
                    required
                    value={this.state.SectorId}
                    label="Sector"
                    options={this.state.Sectores}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.SectorId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <Field
                    name="Descripcion"
                    label="Descripci??n"
                    edit={true}
                    readOnly={false}
                    value={this.state.Descripcion}
                    onChange={this.handleChange}
                    error={this.state.errors.Descripcion}
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
                onClick={this.OcultarFormularioEditar}
              >
                Cancelar
              </button>
            </form>
          </div>
        </Dialog>
      </BlockUi>
    )
  }

  EnviarFormulario(event) {
    event.preventDefault();

    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inv??lidos.",
        "Validaci??n"
      );
      return;
    } else {
      axios
        .post("/Transporte/Ruta/Create", {
          Id: 0,
          Codigo: "0",
          Descripcion: this.state.Descripcion,
          Nombre: this.state.Nombre.toUpperCase(),
          Distancia: this.state.Distancia,
          Duracion: this.state.Duracion,
          DestinoId: this.state.DestinoId,
          OrigenId: this.state.OrigenId,
          SectorId: this.state.SectorId
        })
        .then(response => {

          if (response.data == "Existe") {
            this.props.showWarn("El c??digo de la ruta ya existe");
            this.StopLoading();
          } else
            if (response.data == "ORIGENDESTIO") {
              this.props.showWarn("Lugar de Origen no puede ser igual al Lugar Destino");
              this.StopLoading();
            } else {
              if (response.data == "OK") {
                this.props.showSuccess("Ruta Creada");
                this.setState({ visible: false });
                this.ObtenerRutas();
                this.StopLoading();
                this.VaciarCampos();
              } else {
                this.props.showWarn("Ocurri?? un Error");
                this.StopLoading();
              }
            }
        })
        .catch(error => {
          console.log(error);
          this.props.showWarn("Ocurri?? un Error");
          this.StopLoading();
        });
    }
  }
  EnviarFormularioEditar(event) {
    event.preventDefault();
    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inv??lidos.",
        "Validaci??n"
      );
    } else {
      axios
        .post("/Transporte/Ruta/Edit", {
          Id: this.state.Identificador,
          Codigo: this.state.Codigo.toUpperCase(),
          Descripcion: this.state.Descripcion,
          Nombre: this.state.Nombre.toUpperCase(),
          Distancia: this.state.Distancia,
          Duracion: this.state.Duracion,
          DestinoId: this.state.DestinoId,
          OrigenId: this.state.OrigenId,
          SectorId: this.state.SectorId
        })
        .then(response => {
          if (response.data == "Existe") {
            this.alertMessage("El c??digo de la ruta ya existe");
            this.StopLoading();
          } else {
            if (response.data == "OK") {
              this.infoMessage("Editado Correctamente");
              this.setState({ visibleeditar: false });
              this.ObtenerRutas();
            } else {
              this.alertMessage("Ocurri?? un Error");
            }
          }
        })
        .catch(error => {
          console.log(error);
          this.alertMessage("Ocurri?? un Error");
        });
    }
  }
  MostrarFormulario() {
    this.setState({ visible: true });
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }
  handleChangeCorreo(event) {
    this.setState({ [event.target.name]: event.target.value.toLowerCase() });
  }
  handleChangeIdentificacion(event) {
    this.setState({ [event.target.name]: event.target.value });
  }
  handleChangeDuracion = (event) => {
    this.setState({ [event.target.name]: event.target.value.match(/[1-9][0-9]*/g) });
  }

  Loading() {
    this.setState({ blocking: true });
  }

  StopLoading() {
    this.setState({ blocking: false });
  }

  OcultarFormulario() {
    this.setState({ visible: false });
    this.VaciarCampos();
  }

  MostrarFormulario() {
    this.setState({ visible: true });
  }
  OcultarFormularioEditar() {
    this.setState({ visibleeditar: false });
    this.VaciarCampos();
  }

  MostrarFormularioEditar(cell) {
    this.setState({ errors: {} })
    console.log(cell.Id);
    if (cell.Id > 0) {
      this.ObtenerDetalleRuta(cell.Id);
    }
    this.setState({ visibleeditar: true });
  }

  showInfo() {
    this.growl.show({
      severity: "info",
      summary: "Informaci??n",
      detail: this.state.message
    });
  }

  infoMessage(msg) {
    this.setState({ message: msg }, this.showInfo);
  }

  showAlert() {
    this.growl.show({
      severity: "error",
      summary: "Alerta",
      detail: this.state.message
    });
  }

  alertMessage(msg) {
    this.setState({ message: msg }, this.showAlert);
  }
}
const Container = wrapForm(RutasList);
ReactDOM.render(<Container />, document.getElementById("content"));
