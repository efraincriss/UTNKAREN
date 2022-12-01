import React from "react";
import axios from "axios";
import moment from "moment";
import BlockUi from "react-block-ui";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Button } from "primereact/components/button/Button";
import {
  TIPO_IDENTIFICACION_PASAPORTE,
  TIPO_IDENTIFICACION_CEDULA,
  ESTADO_CIVIL_CASADO,
  ESTADO_CIVIL_SOLTERO,
  ESTADO_CIVIL_DIVORCIADO,
  REGISTRO_CIVIL_CASADO,
  REGISTRO_CIVIL_SOLTERO,
  REGISTRO_CIVIL_DIVORCIADO,
  GENERO_MUJER,
  GENERO_VARON,
  REGISTRO_CIVIL_MUJER,
  REGISTRO_CIVIL_HOMBRE,
  NACIONALIDAD_ECUATORIANA,
  REGISTRO_CIVIL_ECUATORIANA,
  PARENTESCO_HIJO,
  PARENTESCO_CONYUGE,
  PARENTESCO_CONVIVIENTE
} from "./Codigos";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";

export default class EditarCargaSocial extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      id_carga: "",
      nro: 0,
      parentesco: "",
      tipo_identificacion: "",
      nro_identificacion: "",
      primer_apellido: "",
      segundo_apellido: "",
      apellidos: "",
      nombres: "",
      genero: "",
      fecha_nacimiento: "",
      edad: "",
      pais: "",
      nacionalidad: "",
      estado_civil: "",
      fecha_matrimonio: "",
      discapacidad: false,
      tipo_dis: "",
      porcentaje_dis: "",
      errores: [],
      formIsValid: "",
      cargas: [],
      tiposParentesco: [],
      tiposIdentificacion: [],
      tiposNacionalidades: [],
      generos: [],
      estados: [],
      estado_dialog: false,
      tipoDiscapacidad: [],
      porcentajeDis: [],
      listaCargas: [],
      visible: false,
      loading: true,
      visible_fecha_matrimonio: false,
      consIsValid: false,
      errCons: [],
      datosConsumo: [],
      disable_consumo: false,
      disable_sexo: false,
      disable_estado_civil: false,
      disable_nacionalidad: false,
      disable_fecha_matrimonio: false,
      sexo_sugerido: "",
      nacionalidad_sugerida: "",
      estado_civil_sugerido: "",
      por_sustitucion: false,
      es_bdd: false,
      nombres_apellidos: "",
      carga_id_bdd: "",
      es_sustituto: false,
      //CAMPO COMPROBACION REGISTRO CIVIL,
      viene_registro_civil: false,
      editstate: false,
      createstate: false
    };

    this.abrirRegistroCarga = this.abrirRegistroCarga.bind(this);
    this.abrirRegistroCargaNuevo = this.abrirRegistroCargaNuevo.bind(this);
    this.cerrarRegistroCarga = this.cerrarRegistroCarga.bind(this);
    this.AlmacenarCargaSocia = this.AlmacenarCargaSocia.bind(this);
    this.ClearStates = this.ClearStates.bind(this);

    this.handleChange = this.handleChange.bind(this);
    this.handleValidation = this.handleValidation.bind(this);
    this.Guardar = this.Guardar.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleChangeFechaNac = this.handleChangeFechaNac.bind(this);
    this.handleChangeEstadoCivil = this.handleChangeEstadoCivil.bind(this);
    this.saveEdad = this.saveEdad.bind(this);
    this.handleChangeIden = this.handleChangeIden.bind(this);
    this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);

    this.Delete = this.Delete.bind(this);
    this.LoadCarga = this.LoadCarga.bind(this);
    this.VerificarSustituto = this.VerificarSustituto.bind(this);

    this.ObtenerCargaSocial = this.ObtenerCargaSocial.bind(this);

    this.consultarWS = this.consultarWS.bind(this);
    this.validationConsumo = this.validationConsumo.bind(this);
    this.procesarDatosConsumo = this.procesarDatosConsumo.bind(this);

    this.consultaBDD = this.consultaBDD.bind(this);
    this.procesarDatosBDD = this.procesarDatosBDD.bind(this);
    this.consultaPublica = this.consultaPublica.bind(this);
    this.procesarConsultaPublica = this.procesarConsultaPublica.bind(this);
  }

  componentDidMount() {}

  componentWillReceiveProps(nextProps) {
    this.state.cargas = nextProps.cargas;
    this.ObtenerCargaSocial();
  }

  ObtenerCargaSocial() {
    this.state.cargas.forEach(e => {
      e.fecha_nacimiento = moment(e.fecha_nacimiento).format("YYYY-MM-DD");
      e.id_carga = e.Id;
      this.state.nro = e.nro;
    });
    this.VerificarSustituto();
    this.setState({ loading: false });
  }

  consultaBDD() {
    this.setState({ es_bdd: true });
    this.validationConsumoBDD();

    if (this.state.consIsValid == true) {
      this.setState({
        loading: true,
        nombres_apellidos: "",
        primer_apellido: "",
        segundo_apellido: "",
        nombres: "",
        genero: "",
        parentesco: "",
        fecha_nacimiento: "",
        edad: "",
        pais: "",
        nacionalidad: "",
        estado_civil: "",
        fecha_matrimonio: "",
        disable_consumo: false,
        disable_estado_civil: false,
        disable_fecha_matrimonio: false,
        disable_nacionalidad: false,
        disable_sexo: false
      });

      axios
        .post("/RRHH/Colaboradores/GetInfoCargaSocialWSApi", {
          tipoIdentificacion: this.state.tipo_identificacion,
          cedula: this.state.nro_identificacion,
          idColaborador: this.props.id_colaborador
        })
        .then(response => {
          if (response.data == "NO") {
            this.consultaPublica();
          } else {
            this.props.successMessage(
              this.state.nro_identificacion + " Encontrado en base de datos"
            );
            this.procesarDatosBDD(response.data);
            if (response.data.ColaboradoresId != this.props.id_colaborador) {
              this.props.warnMessage(
                "Familiar se encuentra registrado con el Colaborador " +
                  response.data.Colaboradores.numero_identificacion +
                  " " +
                  response.data.Colaboradores.nombres_apellidos
              );
            }
          }
          console.log("bdd", response.data);
        })
        .catch(error => {
          console.log(error);
          this.setState({ loading: false });
        });
    } else {
      this.props.warnMessage("Complete los campos necesarios");
    }
  }

  consultaPublica() {
    axios
      .post("/Accesos/ConsultaPublica/ExisteCandidato", {
        identificacion: this.state.nro_identificacion
      })
      .then(response => {
        console.log("consulta publica", response.data);
        if (response.data.success == true) {
          this.procesarConsultaPublica(response.data.result);
        } else {
          this.setState({ loading: false });
          abp.notify.error("No se ha encontrado", "Error");
        }
      })
      .catch(error => {
        console.log(error);
        this.setState({ loading: false });
      });
  }

  procesarDatosBDD(data) {
    this.setState({
      disable_consumo: true,
      nombres_apellidos: data.nombres_apellidos,
      genero: data.idGenero,
      parentesco: data.parentesco_id,
      disable_sexo: true,
      fecha_nacimiento: moment(data.fecha_nacimiento).format("YYYY-MM-DD"),
      nacionalidad: data.pais_nacimiento,
      disable_nacionalidad: true,
      estado_civil: data.estado_civil,
      disable_estado_civil: true,
      fecha_matrimonio:
        data.fecha_matrimonio == null
          ? ""
          : moment(data.fecha_matrimonio).format("YYYY-MM-DD"),
      visible_fecha_matrimonio: data.fecha_matrimonio == null ? false : true,
      disable_fecha_matrimonio: data.fecha_matrimonio == null ? false : true,
      primer_apellido: data.primer_apellido,
      segundo_apellido:
        data.segundo_apellido == null ? "" : data.segundo_apellido,
      nombres: data.nombres,
      pais: data.PaisId,
      loading: false
    });
    this.saveEdad();
  }

  procesarConsultaPublica(datos) {
    this.setState({ datosConsumo: datos, disable_consumo: true });
    // this.state.nombres_apellidos = datos.Nombre
    var fech_nac = moment(datos.fecha_nacimiento, "YYYY-MM-DD");
    console.log(fech_nac);
    //Seleccionar el SEXO
    if (datos.sexo == REGISTRO_CIVIL_MUJER) {
      var sexo = this.props.generos.filter(c => c.codigo == GENERO_MUJER);
      this.setState({ genero: sexo[0].Id, disable_sexo: true });
    } else if (datos.sexo == REGISTRO_CIVIL_HOMBRE) {
      var sexo = this.props.generos.filter(c => c.codigo == GENERO_VARON);
      this.setState({ genero: sexo[0].Id, disable_sexo: true });
    } else {
      this.setState({
        sexo_sugerido: datos.sexo,
        disable_sexo: false,
        genero: ""
      });
    }
    //Seleccionar el ESTADO CIVIL
    if (datos.estado_civil == REGISTRO_CIVIL_CASADO) {
      var fecha_m = moment(datos.fecha_matrimonio, "YYYY-MM-DD");
      var estado = this.props.estados.filter(
        c => c.codigo == ESTADO_CIVIL_CASADO
      );
      this.setState({
        estado_civil: estado[0].Id,
        disable_estado_civil: true,
        fecha_matrimonio:
          datos.fecha_matrimonio == ""
            ? ""
            : moment(fecha_m).format("YYYY-MM-DD"),
        visible_fecha_matrimonio: true,
        disable_fecha_matrimonio: datos.fecha_matrimonio == "" ? false : true
      });
    } else if (datos.estado_civil == REGISTRO_CIVIL_DIVORCIADO) {
      var estado = this.props.estados.filter(
        c => c.codigo == ESTADO_CIVIL_DIVORCIADO
      );
      this.setState({ estado_civil: estado[0].Id, disable_estado_civil: true });
    } else if (datos.estado_civil == REGISTRO_CIVIL_SOLTERO) {
      var estado = this.props.estados.filter(
        c => c.codigo == ESTADO_CIVIL_SOLTERO
      );
      this.setState({ estado_civil: estado[0].Id, disable_estado_civil: true });
    } else {
      this.setState({
        estado_civil_sugerido: datos.estado_civil,
        disable_estado_civil: false,
        estado_civil: ""
      });
    }

    //Seleccionar NACIONALIDAD
    if (datos.nacionalidad == REGISTRO_CIVIL_ECUATORIANA) {
      var n = this.props.tiposNacionalidades.filter(
        c => c.codigo == NACIONALIDAD_ECUATORIANA
      );
      this.setState({ nacionalidad: n[0].Id, disable_nacionalidad: true });
    } else {
      this.setState({
        nacionalidad_sugerida: datos.nacionalidad,
        disable_nacionalidad: false,
        nacionalidad: ""
      });
    }

    this.setState({
      nombres_apellidos: datos.nombres_completos,
      fecha_nacimiento: moment(fech_nac).format("YYYY-MM-DD"),
      // fotografia: datos.Fotografia,
      viene_registro_civil: true,
      fecha_registro_civil: moment().format("YYYY-MM-DD HH:mm:ss"),
      loading: false
    });
    this.saveEdad();
  }

  consultarWS() {
    this.validationConsumo();

    if (this.state.consIsValid == true) {
      this.setState({
        loading: true,
        nombres_apellidos: "",
        primer_apellido: "",
        segundo_apellido: "",
        nombres: "",
        parentesco: "",
        genero: "",
        fecha_nacimiento: "",
        edad: "",
        pais: "",
        nacionalidad: "",
        estado_civil: "",
        fecha_matrimonio: "",
        disable_consumo: false,
        disable_estado_civil: false,
        disable_fecha_matrimonio: false,
        disable_nacionalidad: false,
        disable_sexo: false
      });
      axios
        .post("/RRHH/Colaboradores/Consumir/", {
          cedula: this.state.nro_identificacion
        })
        .then(response => {
          if (response.data === "SIN_RESPUESTA") {
            abp.notify.error(
              "Sin respuesta al Web Service verifique los datos de conección",
              "Error"
            );
            this.setState({ loading: false });
          } else {
            console.log(response.data.return);
            // this.setState({ obtenido: response.data.return})

            if (response.data.return.CodigoError != "000") {
              this.props.warnMessage("" + response.data.return.Error);
              this.setState({ loading: false });
            } else {
              this.props.successMessage(
                response.data.return.Error +
                  " : " +
                  this.state.nro_identificacion
              );
              this.procesarDatosConsumo(response.data.return);
              this.setState({ loading: false });
            }
          }
        })
        .catch(error => {
          console.log(error);
        });
    } else {
      this.props.warnMessage("Complete los campos necesarios");
    }
  }

  procesarDatosConsumo(datos) {
    this.setState({ datosConsumo: datos, disable_consumo: true });
    // this.state.nombres_apellidos = datos.Nombre
    var fech_nac = moment(
      this.state.datosConsumo.FechaNacimiento,
      "DD-MM-YYYY"
    );
    console.log(fech_nac);
    //Seleccionar el SEXO
    if (datos.Sexo == REGISTRO_CIVIL_MUJER) {
      var sexo = this.props.generos.filter(c => c.codigo == GENERO_MUJER);
      this.setState({ genero: sexo[0].Id, disable_sexo: true });
    } else if (datos.Sexo == REGISTRO_CIVIL_HOMBRE) {
      var sexo = this.props.generos.filter(c => c.codigo == GENERO_VARON);
      this.setState({ genero: sexo[0].Id, disable_sexo: true });
    } else {
      this.setState({
        sexo_sugerido: datos.Sexo,
        disable_sexo: false,
        genero: ""
      });
    }
    //Seleccionar el ESTADO CIVIL
    if (datos.EstadoCivil == REGISTRO_CIVIL_CASADO) {
      var fecha_m = moment(
        this.state.datosConsumo.FechaMatrimonio,
        "DD-MM-YYYY"
      );
      var estado = this.props.estados.filter(
        c => c.codigo == ESTADO_CIVIL_CASADO
      );
      this.setState({
        estado_civil: estado[0].Id,
        disable_estado_civil: true,
        fecha_matrimonio:
          this.state.datosConsumo.FechaMatrimonio == ""
            ? ""
            : moment(fecha_m).format("YYYY-MM-DD"),
        visible_fecha_matrimonio: true,
        disable_fecha_matrimonio:
          this.state.datosConsumo.FechaMatrimonio == "" ? false : true
      });
    } else if (datos.EstadoCivil == REGISTRO_CIVIL_DIVORCIADO) {
      var estado = this.props.estados.filter(
        c => c.codigo == ESTADO_CIVIL_DIVORCIADO
      );
      this.setState({ estado_civil: estado[0].Id, disable_estado_civil: true });
    } else if (datos.EstadoCivil == REGISTRO_CIVIL_SOLTERO) {
      var estado = this.props.estados.filter(
        c => c.codigo == ESTADO_CIVIL_SOLTERO
      );
      this.setState({ estado_civil: estado[0].Id, disable_estado_civil: true });
    } else {
      this.setState({
        estado_civil_sugerido: datos.EstadoCivil,
        disable_estado_civil: false,
        estado_civil: ""
      });
    }

    //Seleccionar NACIONALIDAD
    if (datos.Nacionalidad == REGISTRO_CIVIL_ECUATORIANA) {
      var n = this.props.tiposNacionalidades.filter(
        c => c.codigo == NACIONALIDAD_ECUATORIANA
      );
      this.setState({ nacionalidad: n[0].Id, disable_nacionalidad: true });
    } else {
      this.setState({
        nacionalidad_sugerida: datos.Nacionalidad,
        disable_nacionalidad: false,
        nacionalidad: ""
      });
    }

    this.setState({
      nombres_apellidos: this.state.datosConsumo.Nombre,
      fecha_nacimiento: moment(fech_nac).format("YYYY-MM-DD"),
      loading: false
    });
    this.saveEdad();
    this.setState({ viene_registro_civil: true });
  }

  validationConsumo() {
    let errors = {};
    this.state.consIsValid = true;

    if (!this.state.tipo_identificacion) {
      this.state.consIsValid = false;
      errors["tipo_identificacion"] = (
        <div className="alert alert-danger">
          El campo Tipo de Identificación es obligatorio.
        </div>
      );
    } else {
      var catalogo = this.props.tiposIdentificacion.filter(
        c => c.Id == Number.parseInt(this.state.tipo_identificacion)
      );
      console.log("catalogo", catalogo);
      if (catalogo[0].codigo != "CEDULA" && this.state.es_bdd == false) {
        this.state.consIsValid = false;
        errors["tipo_identificacion"] = (
          <div className="alert alert-danger">
            Consulta se realiza con CEDULA
          </div>
        );
      }
    }
    if (!this.state.nro_identificacion) {
      this.state.consIsValid = false;
      errors["nro_identificacion"] = (
        <div className="alert alert-danger">
          El campo No. de Identificación es obligatorio.
        </div>
      );
    } else if (catalogo != null && catalogo[0].codigo == "CEDULA") {
      console.log("lenght", this.state.nro_identificacion.length);
      var cedula_valida = this.props.validarCedula(
        this.state.nro_identificacion
      );
      /* if (!cedula_valida) {
                this.state.consIsValid = false;
                errors["nro_identificacion"] = <div className="alert alert-danger">No. de Identificación es inválido.</div>;
            }
            */
      if (!isFinite(this.state.nro_identificacion)) {
        this.state.consIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            El campo permite solo ingreso numérico
          </div>
        );
      }
      if (this.state.nro_identificacion.length != 10) {
        this.state.consIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            El campo debe tener 10 dígitos
          </div>
        );
      }
    }
    this.setState({ errCons: errors });
    console.log(errors);
  }

  validationConsumoBDD() {
    console.log("BDD validation");
    let errors = {};
    this.state.consIsValid = true;

    if (!this.state.tipo_identificacion) {
      this.state.consIsValid = false;
      errors["tipo_identificacion"] = (
        <div className="alert alert-danger">
          El campo Tipo de Identificación es obligatorio
        </div>
      );
    } else {
      var catalogo = this.props.tiposIdentificacion.filter(
        c => c.Id == Number.parseInt(this.state.tipo_identificacion)
      );
      console.log("catalogo", catalogo);
    }
    if (!this.state.nro_identificacion) {
      this.state.consIsValid = false;
      errors["nro_identificacion"] = (
        <div className="alert alert-danger">
          El campo No. de Identificación es obligatorio
        </div>
      );
    } else if (catalogo != null && catalogo[0].codigo == "CEDULA") {
      console.log("lenght", this.state.nro_identificacion.length);
      var cedula_valida = this.props.validarCedula(
        this.state.nro_identificacion
      );
      if (!cedula_valida) {
        this.state.consIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            No. de Identificación es inválido
          </div>
        );
      }
      if (!isFinite(this.state.nro_identificacion)) {
        this.state.consIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            El campo permite solo ingreso numérico
          </div>
        );
      }
      if (this.state.nro_identificacion.length != 10) {
        this.state.consIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            El campo debe tener 10 dígitos
          </div>
        );
      }
    }

    this.setState({ errCons: errors });
  }

  render() {
    const footer = (
      <div>
        <Button label="Guardar" icon="pi pi-check" onClick={this.Guardar} />
        <Button
          label="Cancelar"
          icon="pi pi-times"
          onClick={this.cerrarRegistroCarga}
          className="p-button-secondary"
        />
      </div>
    );
    const options = {
      withoutNoDataText: true
    };
    return (
      <BlockUi tag="div" blocking={this.state.loading}>
        <div>
          <div className="row">
            <div className="col-xs-12 col-md-12">
              <form onSubmit={this.handleSubmit}>
                <div className="row">
                  <div
                    className="col"
                    style={{
                      visibility:
                        this.props.colaborador.es_sustituto == true &&
                        this.state.es_sustituto == false
                          ? "visible"
                          : "hidden"
                    }}
                  >
                    <div className="alert alert-danger">
                      El colaborador tiene sustituto, recuerde registrarlo
                    </div>
                  </div>
                  <div className="col">
                    <button
                      type="button"
                      onClick={this.abrirRegistroCargaNuevo}
                      className="btn btn-outline-primary pull-right"
                    >
                      Agregar Familiar
                    </button>
                  </div>
                </div>
                <br />
                <div className="row">
                  <div className="col">
                    <BootstrapTable
                      data={this.state.cargas}
                      hover={true}
                      pagination={true}
                      options={options}
                      // containerStyle={{ width: '100%', overflowX: 'scroll' }}
                    >
                      <TableHeaderColumn
                        dataField="any"
                        dataFormat={this.Secuencial}
                        width={"8%"}
                        tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                        thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      >
                        Nº
                      </TableHeaderColumn>
                      {/* <TableHeaderColumn width={'6%'} dataField='nro'  dataAlign="center" headerAlign="center" dataAlign="center" dataSort={true}>No.</TableHeaderColumn> */}
                      <TableHeaderColumn
                        dataField="tipoIdentificacion"
                        thStyle={{ whiteSpace: "normal" }}
                        tdStyle={{ whiteSpace: "normal" }}
                        headerAlign="center"
                        dataSort={true}
                      >
                        Tipo Identificación
                      </TableHeaderColumn>
                      <TableHeaderColumn
                        dataField="nro_identificacion"
                        isKey={true}
                        thStyle={{ whiteSpace: "normal" }}
                        tdStyle={{ whiteSpace: "normal" }}
                        headerAlign="center"
                        dataSort={true}
                      >
                        Número Identificación
                      </TableHeaderColumn>
                      <TableHeaderColumn
                        width={"15%"}
                        dataField="apellidos"
                        thStyle={{ whiteSpace: "normal" }}
                        tdStyle={{ whiteSpace: "normal" }}
                        headerAlign="center"
                        dataSort={true}
                      >
                        Apellidos
                      </TableHeaderColumn>
                      <TableHeaderColumn
                        width={"15%"}
                        dataField="nombres"
                        thStyle={{ whiteSpace: "normal" }}
                        tdStyle={{ whiteSpace: "normal" }}
                        headerAlign="center"
                        dataSort={true}
                      >
                        Nombres
                      </TableHeaderColumn>
                      <TableHeaderColumn
                        dataField="parentesco"
                        thStyle={{ whiteSpace: "normal" }}
                        tdStyle={{ whiteSpace: "normal" }}
                        headerAlign="center"
                        dataSort={true}
                      >
                        Parentesco
                      </TableHeaderColumn>
                      <TableHeaderColumn
                        dataField="fecha_nacimiento"
                        thStyle={{ whiteSpace: "normal" }}
                        tdStyle={{ whiteSpace: "normal" }}
                        headerAlign="center"
                        dataAlign="center"
                        dataSort={true}
                      >
                        Fecha de Nacimiento
                      </TableHeaderColumn>
                      <TableHeaderColumn
                        dataField="nombre_genero"
                        thStyle={{ whiteSpace: "normal" }}
                        tdStyle={{ whiteSpace: "normal" }}
                        headerAlign="center"
                        dataAlign="center"
                        dataSort={true}
                      >
                        Género
                      </TableHeaderColumn>
                      <TableHeaderColumn
                        dataField="nombre_dis"
                        thStyle={{ whiteSpace: "normal" }}
                        htdStyle={{ whiteSpace: "normal" }}
                        eaderAlign="center"
                        dataAlign="center"
                        dataSort={true}
                      >
                        Discapacidad
                      </TableHeaderColumn>
                      <TableHeaderColumn
                        dataField="nombre_sustituto"
                        thStyle={{ whiteSpace: "normal" }}
                        tdStyle={{ whiteSpace: "normal" }}
                        headerAlign="center"
                        dataAlign="center"
                        dataSort={true}
                      >
                        Por Sustitución
                      </TableHeaderColumn>
                      <TableHeaderColumn
                        width={"13%"}
                        dataField="Operaciones"
                        dataFormat={this.generateButton.bind(this)}
                        headerAlign="center"
                        dataAlign="center"
                      >
                        Opciones
                      </TableHeaderColumn>
                    </BootstrapTable>
                  </div>
                </div>

                <br />
                <div className="row">
                  <div className="col">
                    <div className="form-group">
                      {/* <button type="submit" className="btn btn-outline-primary fa fa-save"> Guardar</button> */}
                      {/* <button onClick={() => this.Guardar()} type="button" className="btn btn-outline-primary fa fa-save" style={{ marginLeft: '3px' }}> Guardar</button> */}
                      <button
                        onClick={() => this.props.regresar()}
                        type="button"
                        className="btn btn-outline-primary fa fa-arrow-left"
                        style={{ marginLeft: "3px" }}
                      >
                        {" "}
                        Cancelar
                      </button>
                    </div>
                  </div>
                </div>
              </form>
            </div>
          </div>
          <Dialog
            header="Registro de Familiares"
            visible={this.state.visible}
            style={{ width: "50%" }}
            minY={70}
            footer={footer}
            onHide={this.cerrarRegistroCarga}
            maximizable={true}
          >
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="label">* Tipo de Identificación: </label>
                  <select
                    value={this.state.tipo_identificacion}
                    onChange={this.handleChangeIden}
                    className="form-control"
                    name="tipo_identificacion"
                  >
                    <option value="">Seleccione...</option>
                    {this.props.getFormSelectTipoIdent()}
                  </select>
                  {this.state.errores["tipo_identificacion"]}
                  {this.state.errCons["tipo_identificacion"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="observacion">* No. de Identificación: </label>
                  <input
                    type="text"
                    id="nro_identificacion"
                    className="form-control"
                    value={this.state.nro_identificacion}
                    onChange={this.handleChangeUpperCase}
                    name="nro_identificacion"
                    style={{ width: "67%", display: "inline" }}
                  />
                  {/*
                                    <button type="button" className="btn btn-outline-primary" onClick={this.consultaBDD} style={{ marginTop: '-0.5%' }}>BDD</button>
                                    */}
                  {this.state.createstate && (
                    <button
                      type="button"
                      className="btn btn-outline-primary"
                      onClick={this.consultarWS}
                      style={{ marginTop: "-0.5%", marginLeft: "1px" }}
                    >
                      WS
                    </button>
                  )}
                  {this.state.editstate && !this.state.viene_registro_civil && (
                    <button
                      type="button"
                      className="btn btn-outline-primary"
                      onClick={this.consultarWS}
                      style={{ marginTop: "-0.5%", marginLeft: "1px" }}
                    >
                      WS
                    </button>
                  )}
                  {this.state.errores["nro_identificacion"]}
                  {this.state.errCons["nro_identificacion"]}
                </div>
              </div>
            </div>

            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="nombres_apellidos">
                    <b>Apellidos Nombres:</b>{" "}
                  </label>
                  <input
                    type="text"
                    id="nombres_apellidos"
                    className="form-control"
                    value={this.state.nombres_apellidos}
                    onChange={this.handleChangeUpperCase}
                    name="nombres_apellidos"
                    disabled
                  />
                  {this.state.errores["nombres_apellidos"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="primer_apellido">* Primer Apellido: </label>
                  <input
                    type="text"
                    id="primer_apellido"
                    className="form-control"
                    value={this.state.primer_apellido}
                    onChange={this.handleChangeUpperCase}
                    name="primer_apellido"
                  />
                  {this.state.errores["primer_apellido"]}
                </div>
              </div>
            </div>

            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="segundo_apellido">* Segundo Apellido: </label>
                  <input
                    type="text"
                    id="segundo_apellido"
                    className="form-control"
                    value={this.state.segundo_apellido}
                    onChange={this.handleChangeUpperCase}
                    name="segundo_apellido"
                  />
                  {this.state.errores["segundo_apellido"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="observacion">* Nombres: </label>
                  <input
                    type="text"
                    id="nombres"
                    className="form-control"
                    value={this.state.nombres}
                    onChange={this.handleChangeUpperCase}
                    name="nombres"
                  />
                  {this.state.errores["nombres"]}
                </div>
              </div>
            </div>

            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="parentesco">* Parentesco: </label>
                  <select
                    value={this.state.parentesco}
                    onChange={this.handleChange}
                    className="form-control"
                    name="parentesco"
                  >
                    <option value="">Seleccione...</option>
                    {this.props.getFormSelectParentesco()}
                  </select>
                  {this.state.errores["parentesco"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="label">* Género: </label>
                  <span style={{ color: "red" }}>
                    {" "}
                    {this.state.sexo_sugerido}
                  </span>
                  <select
                    value={this.state.genero}
                    onChange={this.handleChange}
                    className="form-control"
                    name="genero"
                    disabled={this.state.disable_sexo}
                  >
                    <option value="">Seleccione...</option>
                    {this.props.getFormSelectGenero()}
                  </select>
                  {this.state.errores["genero"]}
                </div>
              </div>
            </div>

            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="horas">* Fecha de Nacimiento: </label>
                  <input
                    type="date"
                    id="fecha_nacimiento"
                    className="form-control"
                    value={this.state.fecha_nacimiento}
                    onChange={this.handleChangeFechaNac}
                    name="fecha_nacimiento"
                    disabled={this.state.disable_consumo}
                  />
                  {this.state.errores["fecha_nacimiento"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="observacion">
                    <b>* Edad:</b>{" "}
                  </label>
                  <input
                    type="text"
                    id="edad"
                    disabled="disabled"
                    className="form-control"
                    value={this.state.edad}
                    onChange={this.handleChange}
                    name="edad"
                  />
                </div>
              </div>
            </div>

            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="pais">* País de Nacimiento: </label>
                  <select
                    value={this.state.pais}
                    onChange={this.handleChange}
                    className="form-control"
                    name="pais"
                  >
                    <option value="">Seleccione...</option>
                    {this.props.getFormSelectNacionalidad()}
                  </select>
                  {this.state.errores["pais"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="label">* Nacionalidad: </label>
                  <span style={{ color: "red" }}>
                    {" "}
                    {this.state.nacionalidad_sugerida}
                  </span>
                  <select
                    value={this.state.nacionalidad}
                    onChange={this.handleChange}
                    className="form-control"
                    name="nacionalidad"
                    disabled={this.state.disable_nacionalidad}
                  >
                    <option value="">Seleccione...</option>
                    {this.props.getFormSelectNacionalidades()}
                  </select>
                  {this.state.errores["nacionalidad"]}
                </div>
              </div>
            </div>

            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="estado_civil">* Estado Civil: </label>
                  <span style={{ color: "red" }}>
                    {" "}
                    {this.state.estado_civil_sugerido}
                  </span>
                  <select
                    value={this.state.estado_civil}
                    onChange={this.handleChangeEstadoCivil}
                    className="form-control"
                    name="estado_civil"
                    disabled={this.state.disable_estado_civil}
                  >
                    <option value="">Seleccione...</option>
                    {this.props.getFormSelectEstadoCivil()}
                  </select>
                  {this.state.errores["estado_civil"]}
                </div>
              </div>
              <div
                className="col"
                style={{
                  visibility:
                    this.state.visible_fecha_matrimonio == true
                      ? "visible"
                      : "hidden"
                }}
              >
                <div className="form-group">
                  <label htmlFor="fecha_matrimonio">
                    * Fecha de Matrimonio:{" "}
                  </label>
                  <input
                    type="date"
                    id="fecha_matrimonio"
                    className="form-control"
                    value={this.state.fecha_matrimonio}
                    onChange={this.handleChange}
                    name="fecha_matrimonio"
                    disabled={this.state.disable_fecha_matrimonio}
                  />
                  {this.state.errores["fecha_matrimonio"]}
                </div>
              </div>
            </div>

            <div className="row">
              <div className="col">
                <div
                  className="form-group checkbox"
                  style={{ display: "inline-flex", marginTop: "32px" }}
                >
                  <label htmlFor="por_sustitucion" style={{ width: "285px" }}>
                    Por Sustitución?:{" "}
                  </label>
                  <input
                    type="checkbox"
                    id="por_sustitucion"
                    className="form-control"
                    checked={this.state.por_sustitucion}
                    onChange={this.handleInputChange}
                    name="por_sustitucion"
                    style={{ marginTop: "5px", marginLeft: "-90%" }}
                  />
                </div>
              </div>
              <div className="col">
                <div
                  className="form-group checkbox"
                  style={{ display: "inline-flex", marginTop: "32px" }}
                >
                  <label htmlFor="discapacidad" style={{ width: "285px" }}>
                    Discapacidad?:{" "}
                  </label>
                  <input
                    type="checkbox"
                    id="discapacidad"
                    className="form-control"
                    checked={this.state.discapacidad}
                    onChange={this.handleInputChange}
                    name="discapacidad"
                    style={{ marginTop: "5px", marginLeft: "-90%" }}
                  />
                </div>
              </div>
            </div>
            <div
              className="row"
              style={{
                visibility:
                  this.state.discapacidad == true ? "visible" : "hidden"
              }}
            >
              <div className="col">
                <div className="form-group">
                  <label htmlFor="tipo_dis">Tipo de Discapacidad: </label>
                  <select
                    value={this.state.tipo_dis}
                    onChange={this.handleChange}
                    className="form-control"
                    name="tipo_dis"
                  >
                    <option value="">Seleccione...</option>
                    {this.props.getFormSelectTipoDiscapacidad()}
                  </select>
                  {this.state.errores["tipo_dis"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="porcentaje_dis">
                    Porcentaje Discapacidad (%):{" "}
                  </label>
                  <input
                    type="number"
                    id="porcentaje_dis"
                    className="form-control"
                    value={this.state.porcentaje_dis}
                    onChange={this.handleChange}
                    name="porcentaje_dis"
                  />
                  {this.state.errores["porcentaje_dis"]}
                </div>
              </div>
            </div>
          </Dialog>
        </div>
      </BlockUi>
    );
  }

  generateButton(cell, row) {
    var Id = 0;
    if (row.id_carga > 0) {
      Id = row.id_carga;
    } else {
      Id = row.idCarga;
    }
    return (
      <div>
        <button
          onClick={() => this.LoadCarga(Id)}
          type="button"
          className="btn btn-outline-primary btn-sm"
          style={{ marginLeft: "0.2em" }}
        >
          Editar
        </button>
        <button
          onClick={() => this.Delete(Id)}
          type="button"
          className="btn btn-outline-danger btn-sm"
          style={{ marginLeft: "0.2em" }}
        >
          Eliminar
        </button>
      </div>
    );
  }

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  VerificarSustituto() {
    var existe = false;
    if (this.props.colaborador.es_sustituto == true) {
      this.state.cargas.forEach(c => {
        if (c.por_sustitucion == true) {
          existe = true;
        }
      });
      if (existe == true) {
        this.setState({ es_sustituto: true });
      }
    }
  }

  Delete(id) {
    console.log("id", id);

    var arrayCargas = Object.assign([], this.state.cargas);

    var index = arrayCargas.findIndex(c => c.id_carga === id);
    arrayCargas.splice(index, 1);
    console.log("arrayCargas", arrayCargas);
    var i = 1;
    arrayCargas.forEach(e => {
      e.nro = i++;
    });
    // this.setState({ cargas: arrayCargas });
    this.setState({ loading: true });
    axios
      .post("/RRHH/Colaboradores/DeleteCargaSocialApi/", {
        Id: id
      })
      .then(response => {
        this.setState({ loading: false });
        if (response.data == "OK") {
          this.props.CargasInfo(arrayCargas);
          this.props.successMessage("Familiar Eliminado!");
          this.setState({ cargas: arrayCargas });
          console.log(this.state.cargas);
        } else {
          this.props.warnMessage("Algo salió mal.");
        }
      })
      .catch(error => {
        this.setState({ loading: false });
        console.log(error);
        this.props.warnMessage("Algo salió mal.");
      });
    this.state.cargas = arrayCargas;
    this.VerificarSustituto();
  }

  LoadCarga(id) {
    this.setState({ editstate: true, createstate: false });
    console.log("id", id);
    // console.log('ddd', this.state.cargas);
    var carga = this.state.cargas.filter(c => c.id_carga === id);
    if (carga.length == 0) {
      carga = this.state.cargas.filter(c => c.idCarga === id);
    }

    this.abrirRegistroCarga();
    console.log(carga[0]);
    console.log("ccc", carga, carga.length);

    console.log("ccc0", id);
    this.state.id_carga = id;
    this.state.parentesco = carga[0].parentesco_id;
    this.state.tipo_identificacion = carga[0].idTipoIdentificacion;
    this.state.nro_identificacion = carga[0].nro_identificacion;
    this.state.primer_apellido = carga[0].primer_apellido;
    this.state.segundo_apellido = carga[0].segundo_apellido;
    this.state.nombres = carga[0].nombres;
    this.state.genero = carga[0].idGenero;
    this.state.fecha_nacimiento = carga[0].fecha_nacimiento;
    this.state.pais = carga[0].PaisId;
    this.state.nacionalidad = carga[0].pais_nacimiento;
    this.state.estado_civil = carga[0].estado_civil;
    this.state.fecha_matrimonio =
      carga[0].fecha_matrimonio == null ? "" : carga[0].fecha_matrimonio;
    this.state.discapacidad =
      carga[0].discapacidad === null ? false : carga[0].discapacidad;
    this.state.tipo_dis =
      carga[0].catalogo_tipo_discapacidad_id == null
        ? ""
        : carga[0].catalogo_tipo_discapacidad_id;
    this.state.porcentaje_dis =
      carga[0].catalogo_porcentaje_id == null
        ? ""
        : carga[0].catalogo_porcentaje_id;
    this.state.por_sustitucion =
      carga[0].por_sustitucion === null ? false : carga[0].por_sustitucion;
    this.state.viene_registro_civil =
      carga[0].viene_registro_civil === null
        ? false
        : carga[0].viene_registro_civil;

    this.saveEdad();
    console.log("ccc", this.state.id_carga);
  }

  abrirRegistroCarga() {
    this.setState({ visible: true });
  }
  abrirRegistroCargaNuevo() {
    this.setState({
      visible: true,
      createstate: true,
      editstate: false,
      viene_registro_civil: false
    });
  }

  cerrarRegistroCarga() {
    this.ClearStates();
    this.setState({ visible: false });
  }

  ClearStates() {
    this.setState({
      id_carga: 0,
      parentesco: "",
      tipo_identificacion: "",
      nro_identificacion: "",
      primer_apellido: "",
      segundo_apellido: "",
      nombres_apellidos: "",
      nombres: "",
      genero: "",
      fecha_nacimiento: "",
      edad: "",
      pais: "",
      nacionalidad: "",
      estado_civil: "",
      fecha_matrimonio: "",
      discapacidad: false,
      tipo_dis: "",
      porcentaje_dis: "",
      por_sustitucion: false,
      formIsValid: false,
      errores: []
    });
  }

  handleValidation() {
    let errors = {};
    this.state.formIsValid = true;

    if (!this.state.parentesco) {
      this.state.formIsValid = false;
      errors["parentesco"] = (
        <div className="alert alert-danger">
          El campo Parentesco es obligatorio.
        </div>
      );
    } else {
      var p = this.props.tiposParentesco.filter(
        c => c.Id == Number.parseInt(this.state.parentesco)
      );
      console.log(p);
      if (p != "") {
        var codigo = p[0].codigo.replace(/ /g, "");
        if (codigo == PARENTESCO_CONYUGE || codigo == PARENTESCO_CONVIVIENTE) {
          var conyuge = this.props.tiposParentesco.filter(
            c => c.codigo == PARENTESCO_CONYUGE
          );
          var conviviente = this.props.tiposParentesco.filter(
            c => c.codigo == PARENTESCO_CONVIVIENTE
          );
          var existe = false;
          this.state.cargas.forEach(e => {
            if (
              e.parentesco_id == conyuge[0].Id ||
              e.parentesco_id == conviviente[0].Id
            ) {
              existe = true;
            }
          });

          if (existe == true) {
            this.state.formIsValid = false;
            errors["parentesco"] = (
              <div className="alert alert-danger">
                No se permite ingresar más de un Cónyuge o Conviviente
              </div>
            );
          }
        }
      }
    }
    if (!this.state.tipo_identificacion) {
      this.state.formIsValid = false;
      errors["tipo_identificacion"] = (
        <div className="alert alert-danger">
          El campo Tipo de Identificación es obligatorio.
        </div>
      );
    } else {
      var catalogo = this.props.tiposIdentificacion.filter(
        c => c.Id == Number.parseInt(this.state.tipo_identificacion)
      );
    }
    if (!this.state.nro_identificacion) {
      this.state.formIsValid = false;
      errors["nro_identificacion"] = (
        <div className="alert alert-danger">
          El campo No. de Identificación es obligatorio.
        </div>
      );
    } else if (catalogo != null) {
      var cedula_valida = this.props.validarCedula(
        this.state.nro_identificacion
      );
      // console.log('xxx', cedula_valida);
      // console.log(this.state.nro_identificacion.length);

      if (catalogo[0].codigo == TIPO_IDENTIFICACION_CEDULA && !cedula_valida) {
        if (!isFinite(this.state.nro_identificacion)) {
          this.state.formIsValid = false;
          errors["nro_identificacion"] = (
            <div className="alert alert-danger">
              El campo permite solo ingreso numérico
            </div>
          );
        }
        if (this.state.nro_identificacion.length != 10) {
          this.state.consIsValid = false;
          errors["nro_identificacion"] = (
            <div className="alert alert-danger">
              El campo debe tener 10 dígitos
            </div>
          );
        }
        /* this.state.formIsValid = false;
                errors["nro_identificacion"] = <div className="alert alert-danger">No. de Identificación es inválido.</div>;
                */
      } else if (
        catalogo[0].codigo == TIPO_IDENTIFICACION_PASAPORTE &&
        this.state.nro_identificacion.length > 22
      ) {
        this.state.formIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            No. de Identificación es inválido.
          </div>
        );
      }
    }
    if (!this.state.primer_apellido) {
      this.state.formIsValid = false;
      errors["primer_apellido"] = (
        <div className="alert alert-danger">
          El campo Primer Apellido es obligatorio.
        </div>
      );
    }
    if (!this.state.segundo_apellido) {
      this.state.formIsValid = false;
      errors["segundo_apellido"] = (
        <div className="alert alert-danger">
          El campo Segundo Apellido es obligatorio.
        </div>
      );
    }
    if (!this.state.nombres) {
      this.state.formIsValid = false;
      errors["nombres"] = (
        <div className="alert alert-danger">
          El campo Nombres es obligatorio.
        </div>
      );
    }
    if (
      this.state.primer_apellido &&
      this.state.nombres &&
      this.state.nombres_apellidos
    ) {
      this.state.primer_apellido = this.state.primer_apellido.toUpperCase();
      this.state.segundo_apellido = this.state.segundo_apellido.toUpperCase();
      this.state.nombres = this.state.nombres.toUpperCase();
      var completo = (
        this.state.primer_apellido +
        this.state.segundo_apellido +
        this.state.nombres
      ).toUpperCase();
      var espacios = completo.replace(/ /g, "");
      console.log(espacios);
      if (this.state.nombres_apellidos.replace(/ /g, "") != espacios) {
        this.state.formIsValid = false;
        abp.notify.error(
          "Apellidos o nombres no coinciden con datos de Registro Civil",
          "Error"
        );
      }
    }
    if (!this.state.nombres_apellidos) {
      if (
        this.state.primer_apellido &&
        this.state.nombres &&
        this.state.segundo_apellido
      ) {
        this.state.nombres_apellidos = (
          this.state.primer_apellido +
          " " +
          this.state.segundo_apellido +
          " " +
          this.state.nombres
        ).toUpperCase();
      } else if (
        this.state.primer_apellido &&
        this.state.nombres &&
        this.state.segundo_apellido == ""
      ) {
        this.state.nombres_apellidos = (
          this.state.primer_apellido +
          " " +
          this.state.nombres
        ).toUpperCase();
      }
    }
    if (!this.state.genero) {
      this.state.formIsValid = false;
      errors["genero"] = (
        <div className="alert alert-danger">
          El campo Género es obligatorio.
        </div>
      );
    }
    if (!this.state.fecha_nacimiento) {
      this.state.formIsValid = false;
      errors["fecha_nacimiento"] = (
        <div className="alert alert-danger">
          El campo Fecha de Nacimiento es obligatorio.
        </div>
      );
    } else {
      var fecha = moment().format("YYYY-MM-DD");
      if (this.state.fecha_nacimiento > fecha) {
        this.state.formIsValid = false;
        errors["fecha_nacimiento"] = (
          <div className="alert alert-danger">
            La fecha ingresada no puede ser mayor a la fecha actual
          </div>
        );
      }
    }
    if (!this.state.pais) {
      this.state.formIsValid = false;
      errors["pais"] = (
        <div className="alert alert-danger">
          El campo País de Nacimiento es obligatorio.
        </div>
      );
    }
    if (!this.state.nacionalidad) {
      this.state.formIsValid = false;
      errors["nacionalidad"] = (
        <div className="alert alert-danger">
          El campo Nacionalidad es obligatorio.
        </div>
      );
    }
    if (this.state.estado_civil) {
      var catEstadoCivil = this.props.estados.filter(
        c => c.Id == Number.parseInt(this.state.estado_civil)
      );
      // console.log('catEstadoCivil', catEstadoCivil)
      var today = moment().format("YYYY-MM-DD");
      if (
        catEstadoCivil[0].codigo == ESTADO_CIVIL_CASADO &&
        !this.state.fecha_matrimonio
      ) {
        this.state.formIsValid = false;
        errors["fecha_matrimonio"] = (
          <div className="alert alert-danger">
            El campo Fecha de Matrimonio es obligatorio.
          </div>
        );
      } else if (
        catEstadoCivil[0].codigo == ESTADO_CIVIL_CASADO &&
        this.state.fecha_matrimonio > today
      ) {
        this.state.formIsValid = false;
        errors["fecha_matrimonio"] = (
          <div className="alert alert-danger">
            Fecha de Matrimonio no puede ser mayor a fecha actual.
          </div>
        );
      }
      /*if (p != '') {
                if ( catEstadoCivil[0].codigo == ESTADO_CIVIL_CASADO) {
                    this.state.formIsValid = false;
                    errors["estado_civil"] = <div className="alert alert-danger">No se permite registrar una carga con este estado.</div>;
                }
            }*/
    } else {
      this.state.formIsValid = false;
      errors["estado_civil"] = (
        <div className="alert alert-danger">
          El campo Estado Civil es obligatorio.
        </div>
      );
    }
    if (this.state.discapacidad == true) {
      if (!this.state.tipo_dis) {
        this.state.formIsValid = false;
        errors["tipo_dis"] = (
          <div className="alert alert-danger">
            El campo Tipo de Discapacidad es obligatorio.
          </div>
        );
      }
      if (!this.state.porcentaje_dis) {
        this.state.formIsValid = false;
        errors["porcentaje_dis"] = (
          <div className="alert alert-danger">
            El campo Porcentaje Discapacidad es obligatorio.
          </div>
        );
      } else {
        if (this.state.porcentaje_dis > 100 || this.state.porcentaje_dis <= 0) {
          this.state.formIsValid = false;
          errors["porcentaje_dis"] = (
            <div className="alert alert-danger">
              Debe ingresar un valor entero entre 1 y 100.
            </div>
          );
        }
      }
    }
    if (this.state.por_sustitucion == true) {
      if (p != "") {
        if (
          p[0].codigo == PARENTESCO_HIJO &&
          this.state.discapacidad == false
        ) {
          this.setState({ discapacidad: true });
          if (!this.state.tipo_dis) {
            this.state.formIsValid = false;
            errors["tipo_dis"] = (
              <div className="alert alert-danger">
                El campo Tipo de Discapacidad es obligatorio.
              </div>
            );
          }
          if (!this.state.porcentaje_dis) {
            this.state.formIsValid = false;
            errors["porcentaje_dis"] = (
              <div className="alert alert-danger">
                El campo Porcentaje Discapacidad es obligatorio.
              </div>
            );
          }
        }
      }
    }

    this.setState({ errores: errors });
    console.log(errors);
  }

  Guardar() {
    this.handleValidation();
    console.log(this.state.viene_registro_civil);
    if (this.state.formIsValid) {
      this.setState({ loading: true });
      // console.log(this.state.id_carga);
      axios
        .post("/RRHH/Colaboradores/CreateCargaSocialApi/", {
          tipoIdentificacion:
            this.state.id_carga > 0 ? this.state.id_carga : null,
          ColaboradoresId: this.props.id_colaborador,
          parentesco_id: this.state.parentesco,
          idTipoIdentificacion: this.state.tipo_identificacion,
          nro_identificacion: this.state.nro_identificacion,
          primer_apellido: this.state.primer_apellido,
          segundo_apellido: this.state.segundo_apellido,
          nombres: this.state.nombres,
          idGenero: this.state.genero,
          fecha_nacimiento: this.state.fecha_nacimiento,
          pais_nacimiento: this.state.nacionalidad,
          PaisId: this.state.pais,
          estado_civil: this.state.estado_civil,
          fecha_matrimonio: this.state.fecha_matrimonio,
          discapacidad: this.state.discapacidad,
          catalogo_tipo_discapacidad_id: this.state.tipo_dis,
          catalogo_porcentaje_id: this.state.porcentaje_dis,
          por_sustitucion: this.state.por_sustitucion,
          nombres_apellidos: this.state.nombres_apellidos,
          viene_registro_civil: this.state.viene_registro_civil
        })
        .then(response => {
          this.setState({ loading: false });
          console.log("familiaress", response.data);
          if (response.data == "FAMILIAR_REGISTRADO") {
            abp.notify.error("El Familiar ya se Encuentra Registrado", "Error");

            this.setState({ loading: false });
          } else if (response.data > 0) {
            this.AlmacenarCargaSocia(response.data);
            this.props.successMessage("Familiar Guardado!");
            this.setState({ loading: false });
          } else {
            abp.notify.error(response.data, "Error");

            this.setState({ loading: false });
          }
        })
        .catch(error => {
          this.setState({ loading: false });
          console.log(error);
          abp.notify.error("Algo salió mal.", "Error");
        });
    } else {
      abp.notify.error(
        "Se ha encontrado errores, por favor revisar el formulario",
        "Error"
      );
      this.setState({ loading: false });
    }
  }

  AlmacenarCargaSocia(id) {
    this.state.carga_id_bdd = id;
    // console.log(id, 'carga_bdd', this.state.carga_id_bdd)

    let cargas = {};
    var tipoIdent = this.props.tiposIdentificacion.filter(
      c => c.Id === parseInt(this.state.tipo_identificacion)
    );
    var par = this.props.tiposParentesco.filter(
      c => c.Id === parseInt(this.state.parentesco)
    );
    var genero = this.props.generos.filter(
      c => c.Id === parseInt(this.state.genero)
    );
    // console.log(genero)
    // console.log('tipoIdent', tipoIdent);
    // console.log(this.state.id_carga);
    if (this.state.id_carga != 0) {
      console.log("existente", this.state.cargas);
      cargas = this.state.cargas.filter(
        c => c.id_carga === this.state.id_carga
      );
      var indexCarga = this.state.cargas.findIndex(
        c => c.Id === this.state.id_carga
      );
      if (indexCarga < 0) {
        indexCarga = 0;
      }
      if (cargas.length == 0) {
        cargas = this.state.cargas.filter(
          c => c.idCarga === this.state.id_carga
        );
      }
      console.log("cargasAA", cargas);
      // console.log('indexCarga', indexCarga);
      cargas.parentesco_id = this.state.parentesco;
      cargas.parentesco = par[0].nombre;
      cargas.idTipoIdentificacion = this.state.tipo_identificacion;
      cargas.tipoIdentificacion = tipoIdent[0].nombre;
      cargas.nro_identificacion = this.state.nro_identificacion;
      cargas.primer_apellido = this.state.primer_apellido;
      cargas.segundo_apellido = this.state.segundo_apellido;
      cargas.apellidos =
        this.state.primer_apellido + " " + this.state.segundo_apellido;
      cargas.nombres = this.state.nombres;
      cargas.idGenero = this.state.genero;
      cargas.nombre_genero = genero[0].nombre;
      cargas.fecha_nacimiento = this.state.fecha_nacimiento;
      cargas.pais_nacimiento = this.state.nacionalidad;
      cargas.PaisId = this.state.pais;
      cargas.estado_civil = this.state.estado_civil;
      cargas.fecha_matrimonio = this.state.fecha_matrimonio;
      cargas.discapacidad = this.state.discapacidad;
      cargas.nombre_dis = this.state.discapacidad == true ? "SI" : "NO";
      cargas.catalogo_tipo_discapacidad_id = this.state.tipo_dis;
      cargas.catalogo_porcentaje_id = this.state.porcentaje_dis;
      cargas.idCarga = this.state.id_carga;
      cargas.id_carga = this.state.id_carga;
      cargas.nro = cargas[0].nro;
      cargas.por_sustitucion = this.state.por_sustitucion;
      cargas.nombre_sustituto =
        this.state.por_sustitucion == true ? "SI" : "NO";
      cargas.viene_registro_civil = this.state.viene_registro_civil;

      this.state.cargas[indexCarga] = cargas;
    } else {
      // console.log('nuevo');
      cargas = {
        id_carga: this.state.carga_id_bdd,
        nro: ++this.state.nro,
        ColaboradoresId: this.props.id_colaborador,
        parentesco_id: this.state.parentesco,
        parentesco: par[0].nombre,
        idTipoIdentificacion: this.state.tipo_identificacion,
        tipoIdentificacion: tipoIdent[0].nombre,
        nro_identificacion: this.state.nro_identificacion,
        primer_apellido: this.state.primer_apellido,
        segundo_apellido: this.state.segundo_apellido,
        nombres: this.state.nombres,
        idGenero: this.state.genero,
        nombre_genero: genero[0].nombre,
        fecha_nacimiento: this.state.fecha_nacimiento,
        pais_nacimiento: this.state.nacionalidad,
        PaisId: this.state.pais,
        estado_civil: this.state.estado_civil,
        fecha_matrimonio: this.state.fecha_matrimonio,
        discapacidad: this.state.discapacidad,
        nombre_dis: this.state.discapacidad == true ? "SI" : "NO",
        catalogo_tipo_discapacidad_id: this.state.tipo_dis,
        catalogo_porcentaje_id: this.state.porcentaje_dis,
        apellidos:
          this.state.primer_apellido + " " + this.state.segundo_apellido,
        por_sustitucion: this.state.por_sustitucion,
        nombre_sustituto: this.state.por_sustitucion == true ? "SI" : "NO",
        idCarga: id,
        viene_registro_civil: this.state.viene_registro_civil
      };
      // this.setState({
      //     cargas: [...this.state.cargas, cargas]
      // });

      this.state.cargas.push(cargas);
      // console.log('this.state.cargas', this.state.cargas)
      this.props.CargasInfo(this.state.cargas);
    }
    this.VerificarSustituto();
    this.cerrarRegistroCarga();
  }

  handleChangeFechaNac(event) {
    this.setState({ [event.target.name]: event.target.value }, this.saveEdad);
  }

  saveEdad() {
    var age = moment().diff(this.state.fecha_nacimiento, "years");
    if (!age) {
      age = "";
    }
    this.setState({ edad: age });
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  handleChangeIden(event) {
    this.setState({
      [event.target.name]: event.target.value,
      datosConsumo: [],
      nro_identificacion: "",
      nombres_apellidos: "",
      primer_apellido: "",
      segundo_apellido: "",
      nombres: "",
      genero: "",
      etnia: "",
      fecha_nacimiento: "",
      edad: "",
      pais: "",
      nacionalidad: "",
      estado_civil: "",
      fecha_matrimonio: "",
      disable_consumo: false,
      disable_estado_civil: false,
      disable_fecha_matrimonio: false,
      disable_nacionalidad: false,
      disable_sexo: false
    });
  }

  handleInputChange(event) {
    const target = event.target;
    const value = target.type === "checkbox" ? target.checked : target.value;
    const name = target.name;

    this.setState({
      [name]: value
    });
  }

  handleChangeEstadoCivil(event) {
    this.setState({ [event.target.name]: event.target.value });
    var estado = this.props.estados.filter(
      c => c.Id == Number.parseInt(event.target.value)
    );
    // console.log(estado)
    if (estado[0].codigo == ESTADO_CIVIL_CASADO) {
      this.setState({ visible_fecha_matrimonio: true });
    } else {
      this.setState({ visible_fecha_matrimonio: false });
    }
  }

  handleChangeUpperCase(event) {
    this.setState({ [event.target.name]: event.target.value.toUpperCase() });
  }
}
