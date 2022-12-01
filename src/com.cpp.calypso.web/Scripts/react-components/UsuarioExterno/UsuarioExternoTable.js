import React from "react";
import axios from "axios";
import moment, { now } from "moment";
import BlockUi from "react-block-ui";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";

export default class UsuarioExternoTable extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      visitas: [],
      loading: true,
      errores: [],
      tiposIdentificaciones: [],
      tipo_identificacion: "",
      nro_identificacion: "",
      consIsValid: false,
      consulta: false,
      nombres: "",
    };
    this.GetVisitas = this.GetVisitas.bind(this);
    this.ConsultaIdentificaciones = this.ConsultaIdentificaciones.bind(this);
    this.getFormSelectIdentificaciones = this.getFormSelectIdentificaciones.bind(
      this
    );

    this.handleChange = this.handleChange.bind(this);
    this.handleValidation = this.handleValidation.bind(this);

    this.generateButton = this.generateButton.bind(this);
    this.LoadUsuario = this.LoadUsuario.bind(this);
    this.Delete = this.Delete.bind(this);

    this.consultaBDD = this.consultaBDD.bind(this);
    this.limpiarEstados = this.limpiarEstados.bind(this);
  }

  componentDidMount() {
    this.GetVisitas();
    this.ConsultaIdentificaciones();
  }

  regresar() {}

  render() {
    const options = {
      withoutNoDataText: true,
    };
    return (
      <div>
        <div className="row">
          <div className="col">
            <BlockUi tag="div" blocking={this.state.loading}>
              <div className="row">
                <div className="col-5">
                  <div className="form-group">
                    <label htmlFor="observacion">
                      <b>No. de Identificación:</b>{" "}
                    </label>
                    <input
                      type="text"
                      id="nro_identificacion"
                      className="form-control"
                      value={this.state.nro_identificacion}
                      onChange={this.handleChange}
                      name="nro_identificacion"
                    />
                  </div>
                </div>
                <div className="col-5">
                  <div className="form-group">
                    <label htmlFor="text">
                      <b>Apellidos Nombres:</b>
                    </label>
                    <input
                      type="text"
                      minLength="3"
                      id="nombres"
                      className="form-control"
                      value={this.state.nombres}
                      name="nombres"
                      onChange={this.handleChange}
                    />
                  </div>
                </div>
                <div className="col-2" style={{ marginTop: "2.2%" }}>
                  <button
                    type="button"
                    onClick={() => this.consultaBDD()}
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
              <div className="row">
                <BootstrapTable
                  data={this.state.visitas}
                  hover={true}
                  pagination={true}
                  options={options}
                >
                  <TableHeaderColumn
                    width={"5%"}
                    dataField="nro"
                    isKey={true}
                    dataAlign="center"
                    headerAlign="center"
                    dataSort={true}
                    thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                  >
                    No.
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="nombreTipo"
                    headerAlign="center"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                    thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                  >
                    Tipo Externo
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="numero_identificacion"
                    headerAlign="center"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                    thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                  >
                    No. Identificación
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="apellidos_nombres"
                    thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    headerAlign="center"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Apellidos
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="nombres"
                    thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    headerAlign="center"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Nombres
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="responsable"
                    thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    dataFormat={this.formatResponsable.bind(this)}
                    headerAlign="center"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Responsable
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="fecha_inicio"
                    thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    dataFormat={this.formatFechaInicio.bind(this)}
                    headerAlign="center"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Fecha Inicio
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="fecha_fin"
                    thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    dataFormat={this.formatFechaFin.bind(this)}
                    headerAlign="center"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Fecha Fin
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="estado"
                    headerAlign="center"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                    thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                  >
                    Estado
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Operaciones"
                    headerAlign="center"
                    thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
                    width={"15%"}
                    dataFormat={this.generateButton.bind(this)}
                  >
                    Opciones
                  </TableHeaderColumn>
                </BootstrapTable>
              </div>
            </BlockUi>
          </div>
        </div>
      </div>
    );
  }

  consultaBDD() {
    /*if (!this.state.nro_identificacion && !this.state.nombres) {
            abp.notify.error('Debe seleccionar un criterio de busqueda!', 'Error');
        } else {
            if (this.state.nombres.length < 3 && this.state.nombres.length != 0) {
                abp.notify.error('Debe ingresar al menos tres caracteres para realizar la busqueda por apellidos nombres!', 'Error');
            } else {*/
    this.setState({ loading: true });

    var numeroIdentificacion = "";
    var nombres = "";

    if (this.state.nro_identificacion) {
      numeroIdentificacion = this.state.nro_identificacion;
    }

    if (this.state.nombres) {
      nombres = this.state.nombres;
    }

    axios
      .post("/RRHH/Colaboradores/GetUsuarioExternoBusquedaApi/", {
        numeroIdentificacion: numeroIdentificacion,
        nombres: nombres,
      })
      .then((response) => {
         console.log('ResponseDatos', response.data)
        if (response.data.length == 0) {
          this.setState({ loading: false });
          abp.notify.error(
            "No existe registros con la información ingresada",
            "Error"
          );
        } else {
          this.setState({ loading: false });
          this.procesaFormatoConsulta(response.data);
        }
      })
      .catch((error) => {
        console.log(error);
      });
    /* }
     }*/
  }

  procesaFormatoConsulta(data) {
    // console.log('data', data)
    var array = [];
    data.forEach((c) => {
      var v = {};
      v.Colaboradores = {};
      v.ColaboradoresResponsable = {};
      v.nombre_identificacion = c.nombre_identificacion;
      v.numero_identificacion = c.numero_identificacion;
      v.apellidos_nombres = c.apellidos_nombres;
      v.nombres = c.nombres;
      v.Colaboradores.Id = c.Id;
      v.nro = c.nro;
      if (c.visita != null) {
        v.ColaboradoresResponsable.nombres_apellidos =
          c.visita.ColaboradoresResponsable != null
            ? c.visita.ColaboradoresResponsable.nombres_apellidos != null
              ? c.visita.ColaboradoresResponsable.nombres_apellidos
              : ""
            : "";
        v.fecha_desde = c.visita.fecha_desde;
        v.fecha_hasta = c.visita.fecha_hasta;
        v.estado = c.visita.estado;
      } else {
        v.ColaboradoresResponsable.nombres_apellidos = null;
        v.fecha_desde = null;
        v.fecha_hasta = null;
        v.estado = c.estado!=null?c.estado:"";
      }
      v.nombreTipo = c.nombreTipo;
      array.push(v);
    });

    this.setState({ visitas: array });
  }

  limpiarEstados() {
    this.setState({
      nro_identificacion: "",
      nombres: "",
    });
    this.GetVisitas();
  }

  GetVisitas() {
    this.setState({ loading: true });
    axios
      .get("/RRHH/ColaboradoresVisita/GetUsuariosExternosApi/", {})
      .then((response) => {
        console.log(response.data);
        this.setState({ visitas: response.data, consulta: false });
        this.GetUsuariosRegistrados();
      })
      .catch((error) => {
        console.log(error);
        this.setState({ loading: false });
      });
  }

  GetUsuariosRegistrados() {
    axios
      .get("/RRHH/Colaboradores/GetUsuariosExternosApi/", {})
      .then((response) => {
        console.log("externos", response.data);
        // this.setState({ visitas: response.data, consulta: false })
        this.procesaConsulta(response.data);
        this.setState({ loading: false });
      })
      .catch((error) => {
        console.log(error);
        this.setState({ loading: false });
      });
  }

  SwitchEstado = (id) => {
    this.setState({ loading: true });
    axios
      .get("/RRHH/ColaboradoresVisita/GetSwitch/" + id, {})
      .then((response) => {
        if (response.data == "OK") {
          abp.notify.success("Se actualizo el ESTADO correctamente", "Aviso");
          this.GetVisitas();
          this.setState({ loading: false });
        } else {
          this.setState({ loading: false });
          abp.notify.error("Error al actualizar ESTADO", "Error");
        }
      })
      .catch((error) => {
        console.log(error);
        this.setState({ loading: false });
        abp.notify.error(
          "Existió un inconveniente inténtelo más tarde",
          "Error"
        );
      });
  };

  procesaConsulta(data) {
    var array = [];
    data.forEach((c) => {
      var existe = false;
      this.state.visitas.forEach((d) => {
        // console.log(d.ColaboradoresId, c.Id)
        if (d.ColaboradoresId == c.Id) {
          existe = true;
        }
      });
      // console.log('existe', existe)
      if (existe == false) {
        var v = {};
        v.Colaboradores = {};
        v.ColaboradoresResponsable = {};
        v.nombre_identificacion = c.nombre_identificacion;
        v.numero_identificacion = c.numero_identificacion;
        v.apellidos_nombres = c.apellidos_nombres;
        v.nombres = c.nombres;
        v.Colaboradores.Id = c.Id;
        v.ColaboradoresResponsable.nombres_apellidos = null;
        v.fecha_desde = null;
        v.fecha_hasta = null;
        v.estado = null;
        v.nro = null;
        v.estado_colaborador = c.estado_colaborador;
        v.nombreTipo = c.nombreTipo;
        array.push(v);
        this.state.visitas.unshift(v);
      }
    });
    if (array.length > 0) {
      var e = 1;
      this.state.visitas.forEach((c) => {
        c.nro = e;
        e++;
      });
      console.log("visitas", this.state.visitas);
    }
    console.log("array", array);
  }

  generateButton(cell, row) {
    // console.log('row', row)
    return (
      <div>
        {row != null && row.estado_colaborador === "ACTIVO" && (
          <button
            onClick={() => {
              if (
                window.confirm(
                  "Está seguro de Inactivar el Colaborador Externo?"
                )
              )
                this.SwitchEstado(row.Colaboradores.Id);
            }}
            data-toggle="tooltip"
            data-placement="top"
            title="Inactivar Colaborado Externo"
            type="button"
            className="btn btn-outline-warning btn-sm fa fa-exclamation"
            style={{ marginLeft: "0.2em" }}
          ></button>
        )}
        {row != null && row.estado_colaborador === "INACTIVO" && (
          <button
            onClick={() => {
              if (
                window.confirm("Está seguro de Activar el Colaborador Externo?")
              )
                this.SwitchEstadoro(w.Colaboradores.Id);
            }}
            data-toggle="tooltip"
            data-placement="top"
            title="Activar Colaborador Externo"
            type="button"
            className="btn btn-outline-success btn-sm fa fa-hand-pointer-o"
            style={{ marginLeft: "0.2em" }}
          ></button>
        )}
        <button
          onClick={() => this.LoadUsuario(row.Colaboradores.Id)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
          type="button"
          className="btn btn-outline-primary btn-sm fa fa-edit"
          style={{ marginLeft: "0.2em" }}
        ></button>
        <button
          onClick={() => this.CrearVisita(row.Colaboradores.Id, row)}
          data-toggle="tooltip"
          data-placement="top"
          title={row.estado == null ? "Registrar Visita" : "Editar Visita"}
          className="btn btn-outline-primary btn-sm fa fa-home"
          style={{ marginLeft: "0.2em" }}
        ></button>

        <button
          onClick={() => {
            if (window.confirm("Estás seguro?"))
              this.Delete(row.Colaboradores.Id);
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar Usuario Externo"
          className="btn btn-outline-danger btn-sm fa fa-trash"
          style={{ marginLeft: "0.2em" }}
        ></button>
      </div>
    );
  }

  formatResponsable(cell, row) {
    if (
      row.ColaboradoresResponsable != null &&
      row.ColaboradoresResponsable.nombres_apellidos != null
    ) {
      return row.ColaboradoresResponsable.nombres_apellidos;
    } else {
      return "";
    }
  }

  formatFechaInicio(cell, row) {
    if (row.fecha_desde != null) {
      return moment(row.fecha_desde).format("YYYY-MM-DD");
    } else {
      return null;
    }
  }

  formatFechaFin(cell, row) {
    if (row.fecha_hasta != null) {
      return moment(row.fecha_hasta).format("YYYY-MM-DD");
    } else {
      return null;
    }
  }

  formatEstado(cell, row) {
    if (row.visita != null) {
      return row.visita.estado;
    } else {
      return null;
    }
  }

  LoadUsuario(id) {
    console.log("id", id);
    sessionStorage.setItem("id_usuario_externo", id);
    this.props.Siguiente(2);
  }

  CrearVisita(id, row) {
    sessionStorage.setItem("id_usuario_externo_visita", id);
    sessionStorage.setItem("Colaborador", row);
    this.props.Siguiente(3);
  }

  Delete(id) {
    axios
      .post("/RRHH/ColaboradoresVisita/DeleteUsuarioExternoApi/", { id: id })
      .then((response) => {
        if (response.data === "ERROR") {
          abp.notify.error(
            "Existió un inconveniente inténtelo más tarde",
            "Error"
          );
        }
        if (response.data === "VISITAS") {
          abp.notify.error(
            "No se puede Eliminar tiene Visitas Registradas",
            "Error"
          );
        }
        if (response.data === "REQUISITOS") {
          abp.notify.error(
            "No se puede Eliminar tiene REquisitos Registrados",
            "Error"
          );
        }
        if (response.data === "OK") {
          abp.notify.success("Usuario Externo Eliminado", "Aviso");
          this.GetVisitas();
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }

  handleValidation() {
    let errors = {};
    this.state.consIsValid = true;

    if (!this.state.tipo_identificacion) {
      this.state.consIsValid = false;
      errors["tipo_identificacion"] =
        "El campo Tipo de Identificación es obligatorio.";
    } else {
      var catalogo = this.state.tiposIdentificaciones.filter(
        (c) => c.Id == Number.parseInt(this.state.tipo_identificacion)
      );
      console.log("catalogo", catalogo);
    }
    if (!this.state.nro_identificacion) {
      this.state.consIsValid = false;
      errors["nro_identificacion"] =
        "El campo No. de Identificación es obligatorio.";
    } else if (catalogo != null && catalogo[0].codigo == "CEDULA") {
      console.log("lenght", this.state.nro_identificacion.length);
      var cedula_valida = this.props.validarCedula(
        this.state.nro_identificacion
      );
      if (!cedula_valida) {
        this.state.consIsValid = false;
        errors["nro_identificacion"] = "No. de Identificación es inválido.";
      }
      if (!isFinite(this.state.nro_identificacion)) {
        this.state.consIsValid = false;
        errors["nro_identificacion"] = "El campo permite solo ingreso numérico";
      }
      if (this.state.nro_identificacion.length != 10) {
        this.state.consIsValid = false;
        errors["nro_identificacion"] = "El campo debe tener 10 dígitos";
      }
    }

    this.setState({ errores: errors });
  }

  ConsultaIdentificaciones() {
    // this.setState({ loading: true })
    axios
      .post("/RRHH/Colaboradores/GetCatalogosPorCodigoApi/", {
        codigo: "TIPOINDENTIFICACION",
      })
      .then((response) => {
        // console.log('tiposDestinos', response.data)
        this.setState({
          tiposIdentificaciones: response.data,
          // loading: false
        });
        this.getFormSelectIdentificaciones();
      })
      .catch((error) => {
        console.log(error);
        // this.setState({ loading: false })
      });
  }

  getFormSelectIdentificaciones() {
    return this.state.tiposIdentificaciones.map((item) => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value.toUpperCase() });
  }
}
