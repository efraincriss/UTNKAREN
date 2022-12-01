import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import Field from "../../Base/Field-v2";
import wrapForm from "../../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";
class ColaboradorRutaList extends React.Component {
  constructor(props) {

    //nuevos
    super(props);
    this.state = {
      blocking: true,
      data: [],
      visible: false,
      visibleeditar: false,
      errors: {},
 //nuevos
      // Inputs del Formulario
      Identificador: 0,
      lugares: [],
      Codigo: "",
      Nombre: "",
      OrigenId: 0,
      DestinoId: 0,
      Distancia: 0.0,
      Duracion: 0,
      Sector: "",
      Descripcion: "",
      NombreDestino: "",
      NombreOrigen: "",
      OrigenDestino: "",
      EstadoId: 0,

      //rUTA
      Identificacion: "",
      Colaborador: null,
      RutasHorarios: [],
      RutaHorarioId: 0,
      Observacion: '',
      NombreRuta:'',
      NombreHorario:''
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
    this.props.blockScreen();
    this.ObtenerCatalogos();
    this.ObtenerRutas();

  }

  VaciarCampos() {
    this.setState({
      Observacion: '',
      RutaHorarioId: 0,
      NombreOrigen: '',
      NombreDestino: '',
      Duracion: 0,
      NombreRuta:''
    });
  }
  ObtenerRutas() {
    console.log("compilando o q hace")
    this.Loading();

    axios
      .post("/Transporte/ColaboradorRuta/ObtenerColaborador", {
        identificacion: document.getElementById("numero_identificacion").className
      })
      .then(response => {
        console.log(response.data);

        this.setState({ Colaborador: response.data, Identificacion: response.data.Identificacion });
        if (this.state.Colaborador.Id > 0) {
          axios
            .post("/Transporte/ColaboradorRuta/ListaColaboradoresRutas", {
              id: response.data.Id
            })
            .then(response => {
              console.log(response.data.result);

              this.setState({ data: response.data.result, blocking: false });
              this.StopLoading();
              this.props.unlockScreen();
            })
            .catch(error => {
              console.log(error);
              this.StopLoading();
            });


          axios
            .post("/Transporte/ColaboradorRuta/ObtenerRutaHorarios/"+response.data.Id, {})
            .then(response => {
              console.log(response.data);
              var items = response.data.map(item => {
                return {
                  label: item.NombreRuta + " - " + item.Horario,
                  dataKey: item.Id,
                  value: item.Id
                };
              });
              this.setState({ RutasHorarios: items, blocking: false });
            })
            .catch(error => {
              console.log(error);
              this.StopLoading();
            });

        } else {
          this.setState({ data: [], blocking: false });
          this.props.unlockScreen();
        }

      })
      .catch(error => {
        console.log(error);
        this.StopLoading();
        this.props.unlockScreen();
      });

  }

  ObtenerDetalleRuta(Id) {
    console.log(Id);

    axios
      .post("/Transporte/ColaboradorRuta/ObtenerDetallesRuta", {
        Id: Id
      })
      .then(response => {
        if (response.data != "Error") {
          console.log(response.data);
          this.setState({
            Identificador: Id,
            Codigo: response.data.Codigo,
            Nombre: response.data.Nombre,
            OrigenId: response.data.OrigenId,
            DestinoId: response.data.DestinoId,
            Distancia: response.data.Distancia,
            Duracion: response.data.Duracion,
            Sector: response.data.Sector.nombre,
            Descripcion: response.data.Descripcion,
            NombreDestino: response.data.Destino.Nombre,
            NombreOrigen: response.data.Origen.Nombre,
            OrigenDestino: response.data.Origen.Nombre + " - " + response.data.Destino.Nombre,
            EstadoId: 0,
            blocking: false
          });
        }
      })
      .catch(error => {
        console.log(error);
      });
  }

  ObtenerCatalogos() {




  }
  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });

    this.ObtenerDetalleRuta(value);
  };

  RedireccionarDetalle(Id) {
    console.log(Id);
    window.location.href = "/Transporte/ColaboradorRuta/BuscarColaborador/";
  }
  EliminarRuta(Id) {
    console.log(Id);
    axios
      .post("/Transporte/ColaboradorRuta/Delete", {
        Id: Id
      })
      .then(response => {
        if (response.data != "Error") {
          this.infoMessage("Eliminado Correctamente");
          this.ObtenerRutas();
        }
      })
      .catch(error => {
        console.log(error);
        this.alertMessage("Ocurrió un Error");
      });
  }
  Secuencial(cell, row, enumObject, index) {
    return (<div>{index + 1}</div>)
  }
  generarBotones = (cell, row) => {
    return (
      <div>
        {/* <button
          className="btn btn-outline-success"
          onClick={() => this.RedireccionarDetalle(row.Id)}
        >
          <i className="fa fa-eye" />
        </button>
        */}
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormularioEditar(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar Ruta Colaborador"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar Ruta Colaborador"
          onClick={() => { if (window.confirm(`Esta acción eliminará el registro, ¿Desea continuar?`)) this.EliminarRuta(row.Id); }}
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };

  isValid() {
    const errors = {};

    if (this.state.RutaHorarioId == 0) {
      errors.RutaHorarioId = 'Campo requerido';
    }
    if (this.state.Observacion.length == 0) {
      errors.Observacion = 'Campo requerido';
    }
    if (this.state.Observacion.length > 100) {
      errors.Observacion = 'El campo debe tener máximo 100 caracteres alfanuméricos';
    }
    this.setState({ errors });
    return Object.keys(errors).length === 0;

  }
  render() {
        return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={el => {
            this.growl = el;
          }}
          baseZIndex={1000}
        />
        <div className="content-section implementation">

          <div>
            <div className="row">
              <div className="col">
              </div>
              <div className="col" align="right" >
                <button
                  type="button"
                  className="btn btn-outline-primary"
                  icon="fa fa-fw fa-ban"
                  onClick={this.RedireccionarDetalle}
                >
                  Regresar
                  </button>
              </div>
            </div>
            <br />
            <div className="row">
              <div className="col-xs-12 col-md-6">
                <h6 className="text-gray-700">
                  <b>Apellidos Nombres:</b>{" "}
                  {this.state.Colaborador != null
                    ? this.state.Colaborador.NombresApellidos
                    : ""}
                </h6>
                <h6 className="text-gray-700">
                  <b>No. de Identificación:</b>{" "}
                  {this.state.Colaborador != null
                    ? this.state.Colaborador.Identificacion
                    : ""}
                </h6>
                <h6 className="text-gray-700">
                  <b>Provincia:</b>{" "}
                  {this.state.Colaborador != null
                    ? this.state.Colaborador.ProvinciaNombre
                    : ""}
                </h6>
                <h6 className="text-gray-700">
                  <b>Ciudad:</b>{" "}
                  {this.state.Colaborador != null
                    ? this.state.Colaborador.Ciudad
                    : ""}
                </h6>
                <h6 className="text-gray-700">
                  <b>Parroquia:</b>{" "}
                  {this.state.Colaborador != null
                    ? this.state.Colaborador.Parroquia
                    : ""}
                </h6>
                <h6 className="text-gray-700">
                  <b>Comunidad:</b>{" "}
                  {this.state.Colaborador != null
                    ? this.state.Colaborador.Comuna
                    : ""}
                </h6>
              </div>

              <div className="col-xs-12 col-md-6">
                <h6 className="text-gray-700">
                  <b>Encargado de Personal :</b>{" "}
                  {this.state.Colaborador != null
                    ? this.state.Colaborador.EncargadoPersonal
                    : ""}
                </h6>
                <h6 className="text-gray-700">
                  <b>ID SAP:</b>{" "}
                  {this.state.Colaborador != null ? this.state.Colaborador.IdSap
                    : ""}
                </h6>
                <h6 className="text-gray-700">
                  <b>Calle:</b>{" "}
                  {this.state.Colaborador != null
                    ? this.state.Colaborador.Calle
                    : ""}
                </h6>
                <h6 className="text-gray-700">
                  <b>Teléfono:</b>{" "}
                  {this.state.Colaborador != null
                    ? this.state.Colaborador.Telefonos
                    : ""}
                </h6>
                <h6 className="text-gray-700">
                  <b>N. Casa:</b>{" "}
                  {this.state.Colaborador != null
                    ? this.state.Colaborador.NumeroCasa
                    : ""}
                </h6>
              </div>
            </div>
          </div>

        </div>
        <hr />
        <div align="right">
          <button
            type="button"
            className="btn btn-outline-primary"
            icon="fa fa-fw fa-ban"
            onClick={this.MostrarFormulario}
          >
            Asignar Ruta
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
              dataField="NombreRuta"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Rutas
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Horario"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Horario
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="NombreOrigen"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Lugar Origen
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="NombreDestino"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Lugar Destino
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Sector"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Sector
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
          header="Nueva Asignación"
          visible={this.state.visible}
          width="730px"
          modal={true}
          onHide={this.OcultarFormulario}
        >
          <div>
            <form onSubmit={this.EnviarFormulario} height="730px">
              <div className="row">
                <div className="col">
                  <Field
                    name="Identificacion"
                    label="No. de Identificación"
                    edit={false}
                    readOnly={true}
                    value={this.state.Identificacion}
                    error={this.state.errors.Identificacion}
                  />

                  <Field
                    name="RutaHorarioId"
                    required
                    value={this.state.RutaHorarioId}
                    label="Ruta - Horario"
                    options={this.state.RutasHorarios}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.RutaHorarioId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <div className="form-group">
                    <label className="col-form-label">
                      <b> Lugar Origen</b>
                    </label><br />
                    {this.state.NombreOrigen}
                  </div>
                  <div className="form-group">
                    <label className="col-form-label">
                      <b> Sector</b>
                    </label><br />
                    {this.state.Sector} {" "}
                  </div>

                </div>

                <div className="col">
                  <Field
                    name="Colaborador"
                    label="Apellidos Nombres"
                    edit={false}
                    readOnly={true}
                    value={this.state.Colaborador != null ? this.state.Colaborador.NombresApellidos : ""}
                  />


                  <div className="form-group">
                    <label className="col-form-label">
                      <b> Duración</b>
                    </label><br />
                    {this.state.Duracion} {" "}minutos
                  </div>
                  <div className="form-group">
                    <label className="col-form-label">
                      <b>Lugar Destino</b>
                    </label><br />
                    {this.state.NombreDestino}
                  </div>
                  <Field
                    name="Observacion"
                    label="Observación"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Observacion}
                    onChange={this.handleChange}
                    error={this.state.errors.Observacion}
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
                    name="Identificacion"
                    label="No. de Identificación"
                    edit={false}
                    readOnly={true}
                    value={this.state.Identificacion}
                    error={this.state.errors.Identificacion}
                  />
                   <b><label>Ruta - Horario:
                    
                    </label></b><br/>
                   {this.state.NombreRuta}{" "}{this.state.NombreHorario}
                   {/*<Field
                    name="RutaHorarioId"
                    required
                    value={this.state.RutaHorarioId}
                    label="Ruta - Horario"
                    options={this.state.RutasHorarios}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.RutaHorarioId}
                    readOnly={true}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                   />*/}

                </div>

                <div className="col">
                  <Field
                    name="Colaborador"
                    label="Apellidos Nombres"
                    edit={false}
                    readOnly={true}
                    value={this.state.Colaborador != null ? this.state.Colaborador.NombresApellidos : ""}
                  />

                  <Field
                    name="Observacion"
                    label="Observación"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Observacion}
                    onChange={this.handleChange}
                    error={this.state.errors.Observacion}
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
    );
  }

  EnviarFormulario(event) {
    event.preventDefault();

    if (!this.isValid()) {
      this.props.showValidation("No ha ingresado los campos obligatorios  o existen datos inválidos.");
      return;
    } else {
      this.props.blockScreen();
      axios
        .post("/Transporte/ColaboradorRuta/Create", {
          Id: 0,
          ColaboradorId: this.state.Colaborador.Id,
          RutaHorarioId: this.state.RutaHorarioId,
          Observacion: this.state.Observacion
        })
        .then(response => {
          if (response.data == "OK") {
            this.props.showSuccess("Ruta Asignada a Colaborador");
            this.setState({ visible: false });
            this.ObtenerRutas();
            this.VaciarCampos();
            this.StopLoading();
            this.props.unlockScreen();

          } else {
            this.props.showWarn("Ocurrió un Error");
            this.StopLoading();
            this.props.unlockScreen();
          }
        })
        .catch(error => {
          console.log(error);
          this.props.showWarn("Ocurrió un Error");
          this.StopLoading();
          this.props.unlockScreen();
        });
    }
  }
  EnviarFormularioEditar(event) {
    event.preventDefault();

    if (!this.isValid()) {
      this.props.showValidation("No ha ingresado los campos obligatorios  o existen datos inválidos.");
      return;
    } else {
      axios
        .post("/Transporte/ColaboradorRuta/Edit", {
          Id: this.state.Identificador,
          ColaboradorId: this.state.Colaborador.Id,
          RutaHorarioId: this.state.RutaHorarioId,
          Observacion: this.state.Observacion
        })
        .then(response => {
          if (response.data == "OK") {
            this.props.showSuccess("Editado Correctamente");
            this.setState({ visibleeditar: false });
            this.ObtenerRutas();
            this.props.unlockScreen();
          } else {
            this.props.showWarn("Ocurrió un Error");
            this.props.unlockScreen();
          }
        })
        .catch(error => {
          console.log(error);
          this.props.showWarn("Ocurrió un Error");
          this.props.unlockScreen();
        });
    }
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
    if (this.state.Identificacion.length > 0) {
      this.setState({ visible: true, OrigenDestino: '', Duracion: 0 });
    } else {
      this.props.showWarn("Debe buscar un Colaborador");
    }
  }
  OcultarFormularioEditar() {
    this.setState({ visibleeditar: false });
    this.VaciarCampos();
  }

  MostrarFormularioEditar(row) {
    console.log(row.Id);
    if (row.Id > 0) {
      this.setState({
        visibleeditar: true,
        Identificador: row.Id,
        RutaHorarioId: row.RutaHorarioId,
        ColaboradorId: row.ColaboradorId,
        Observacion: row.Observacion,
        NombreRuta:row.NombreRuta,
        NombreHorario:row.Horario
      });
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
      severity: "error",
      summary: "Alerta",
      detail: this.state.message
    });
  }

  alertMessage(msg) {
    this.setState({ message: msg }, this.showAlert);
  }
}
const Container = wrapForm(ColaboradorRutaList);
ReactDOM.render(<Container />, document.getElementById("content"));
