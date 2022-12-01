import React from "react";
import axios from "axios";
import moment, { now } from "moment";
import BlockUi from "react-block-ui";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Button } from "primereact/components/button/Button";

import { Card } from "primereact-v2/card";
import { Button2 } from "primereact-v2/button";
import {
  ENCARGADO_PERSONAL_MENSUALES,
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
  CODIGO_INCAPACIDAD_NOINCAPACITADO,
  CODIGO_SINIESTRO_NOINCAPACITADO
} from "./Codigos";
import Field from "../Base/Field-v2";
import { AssertionError } from "assert";

export default class InformacionGeneral extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      id_candidato: "",
      id_empleado: "",
      tipo_identificacion: "",
      nro_identificacion: "",
      nombres_apellidos: "",
      primer_apellido: "",
      segundo_apellido: "",
      nombres: "",
      fecha_nacimiento: "",
      fecha_ingreso: "",
      edad: "",
      genero: "",
      nacionalidad: "",
      pais: "",
      estado_civil: "",
      fecha_matrimonio: "",
      hijos: "",
      etnia: "",
      discapacidad: "",
      tipo_dis: "",
      porcentaje_dis: "",
      codigo_siniestro: "",
      formacion_educativa: "",
      formacion: "",
      institucion: "",
      titulo: "",
      telefono: "",
      celular: "",
      email: "",
      tipo_usuario: "",
      tiposIdentificacion: [],
      generos: [],
      etnias: [],
      estados: [],
      tipoDiscapacidad: [],
      codigoSiniestro: [],
      formacionAca: [],
      tiposFormaciones: [],
      tiposTitulos: [],
      tiposUsuarios: [],
      tiposNacionalidades: [],
      errores: [],
      formIsValid: false,
      id_colaborador: 0,
      estado: false,
      usuario_id: "",
      codigo_incapacidad: "",
      tiposIncapacidades: [],
      fecha_senecyt: "",
      loading: true,
      visible_fecha_matrimonio: false,
      codigo_dactilar: "",
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
      tiposEncargadoPersonal: [],
      tiposDestino: [],
      tiposProyectos: [],
      encargado_personal: "",
      destino: "",
      proyecto: "",
      viene_registro_civil: false,
      fecha_registro_civil: "",
      es_sustituto: false,
      fecha_sustituto_desde: "",
      empleado_id_sap_local: "",
      //Empresa
      empresa_id: 0,
      empresas: [],
      errors: {},

      //ESTADO VERIFAR REGISTRO CIVL
      confirmws: false,
      viewconfirm: false,

      //Compara el Nombre
      TipoIdentificacionName: "",

      viewexterno: false
    };

    this.getFormSelectTipoIdent = this.getFormSelectTipoIdent.bind(this);
    this.getFormSelectGenero = this.getFormSelectGenero.bind(this);
    this.getFormSelectEtnia = this.getFormSelectEtnia.bind(this);
    this.getFormSelectEstadoCivil = this.getFormSelectEstadoCivil.bind(this);
    this.getFormSelectTipoDiscapacidad = this.getFormSelectTipoDiscapacidad.bind(
      this
    );
    this.getFormSelectCodigoSiniestro = this.getFormSelectCodigoSiniestro.bind(
      this
    );
    this.getFormSelectFormacionEducativa = this.getFormSelectFormacionEducativa.bind(
      this
    );
    this.getFormSelectFormacion = this.getFormSelectFormacion.bind(this);
    this.getFormSelectTitulos = this.getFormSelectTitulos.bind(this);
    this.getFormSelectNacionalidades = this.getFormSelectNacionalidades.bind(
      this
    );
    this.getFormSelectCodigoIncapacidad = this.getFormSelectCodigoIncapacidad.bind(
      this
    );
    this.getFormSelectEncargadoPersonal = this.getFormSelectEncargadoPersonal.bind(
      this
    );
    this.getFormSelectDestinos = this.getFormSelectDestinos.bind(this);
    this.getFormSelectProyecto = this.getFormSelectProyecto.bind(this);

    this.handleChangeFechaNac = this.handleChangeFechaNac.bind(this);
    this.handleChangeEstadoCivil = this.handleChangeEstadoCivil.bind(this);
    this.handleChangeIden = this.handleChangeIden.bind(this);
    this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);
    this.handleChangeCapitalize = this.handleChangeCapitalize.bind(this);

    this.saveEdad = this.saveEdad.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.handleValidation = this.handleValidation.bind(this);
    this.Guardar = this.Guardar.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.abrirConfirmacion = this.abrirConfirmacion.bind(this);
    this.cerrarConfirmacion = this.cerrarConfirmacion.bind(this);

    this.GetCatalogos = this.GetCatalogos.bind(this);
    this.cargarCatalogos = this.cargarCatalogos.bind(this);

    this.GetParametroIdentificacion = this.GetParametroIdentificacion.bind(
      this
    );
    this.consultarWSHuella = this.consultarWSHuella.bind(this);
    this.validationConsumo = this.validationConsumo.bind(this);
    this.procesarDatosConsumo = this.procesarDatosConsumo.bind(this);
    this.convertirimagen = this.convertirimagen.bind(this);
    this.b64toBlob = this.b64toBlob.bind(this);

    this.consultaBDD = this.consultaBDD.bind(this);
    this.procesarDatosBDD = this.procesarDatosBDD.bind(this);
    this.validationConsumoBDD = this.validationConsumoBDD.bind(this);
    this.VerificaIdentificacionUnica = this.VerificaIdentificacionUnica.bind(
      this
    );
    this.consultaPublica = this.consultaPublica.bind(this);
    this.procesarConsultaPublica = this.procesarConsultaPublica.bind(this);
    this.renderboton = this.renderboton.bind(this);

    //Actualizacion
    this.onChangeValue = this.onChangeValue.bind(this);
    this.onClick = this.onClick.bind(this);
    this.onHide = this.onHide.bind(this);
  }

  onClick() {
    this.setState({ viewconfirm: true });
  }

  onHide() {
    this.setState({ viewconfirm: false });
  }
  onHideExterno = () => {
    this.setState({ viewexterno: false });
  };
  componentDidMount() {
    this.GetCatalogos();
    // this.GetParametroIdentificacion();
  }

  GetParametroIdentificacion() {
    axios
      .post("/RRHH/Colaboradores/GetParametroAPI", { codigo: "COL.TIP_IDE" })
      .then(response => {
        // this.setState({ nacionalidades: response.data })
        console.log("parametro", response.data);
      })
      .catch(error => {
        console.log(error);
      });
  }
  onChangeValue = (name, value) => {
    console.log(name);
    this.setState({
      [name]: value
    });
  };

  consultaBDD() {
    this.validationConsumoBDD();

    if (this.state.consIsValid == true) {
      this.setState({
        loading: true,
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
        disable_sexo: false,
        codigo_dactilar: "",
        formacion_educativa: "",
        formacion: "",
        institucion: "",
        titulo: "",
        fecha_senecyt: ""
      });
      axios
        .post("/RRHH/Colaboradores/GetInfoColaboradorWSApi", {
          tipoIdentificacion: this.state.tipo_identificacion,
          cedula: this.state.nro_identificacion,
          huella_dactilar: this.state.codigo_dactilar
        })
        .then(response => {
          if (response.data == "NO") {
            this.consultaPublica();
          } else if (response.data.length != undefined) {
            this.setState({ loading: false });
            abp.notify.error(response.data, "Error");
          } else {
            abp.notify.error(
              "EL Colaborador con nro de Identificación: " +
              this.state.nro_identificacion +
              "ya está registrado",
              "Aviso"
            );
            this.procesarDatosBDD(response.data);
          }
          console.log("bdd", response.data);
        })
        .catch(error => {
          console.log(error);
          this.setState({ loading: false });
        });
    } else {
      abp.notify.error("Complete los campos necesarios", "Error");
    }
  }

  consultaPublica() {
    console.log("CONSULTANDO TABLA CONSULTA PÚBLICA.............");
    axios
      .post("/RRHH/Colaboradores/GetConsulta", {
        identificacion: this.state.nro_identificacion
      })
      .then(response => {
        console.log(response.data);
        if (response.data.success == true) {
          this.procesarConsultaPublica(response.data.result);
        } else {
          abp.notify.error(
            "Colaborador No Encontrado en Consulta Pública",
            "Error"
          );
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
      genero: data.catalogo_genero_id,
      disable_sexo: true,
      fecha_nacimiento: moment(data.fecha_nacimiento).format("YYYY-MM-DD"),
      nacionalidad: data.pais_pais_nacimiento_id,
      disable_nacionalidad: true,
      estado_civil: data.catalogo_estado_civil_id,
      disable_estado_civil: true,
      fecha_matrimonio:
        data.fecha_matrimonio == null
          ? ""
          : moment(data.fecha_matrimonio).format("YYYY-MM-DD"),
      visible_fecha_matrimonio: data.fecha_matrimonio == null ? false : true,
      //  disable_fecha_matrimonio: data.fecha_matrimonio == null ? false : true,
      primer_apellido: data.primer_apellido,
      segundo_apellido:
        data.segundo_apellido == null ? "" : data.segundo_apellido,
      nombres: data.nombres,
      codigo_dactilar: data.codigo_dactilar == null ? "" : data.codigo_dactilar,
      numero_hijos: data.numero_hijos == null ? "" : data.numero_hijos,
      etnia: data.catalogo_etnia_id == null ? "" : data.catalogo_etnia_id,
      pais: data.PaisId,
      formacion_educativa:
        data.catalogo_formacion_educativa_id == null
          ? ""
          : data.catalogo_formacion_educativa_id,
      formacion: data.formacion == null ? "" : data.formacion,
      institucion: data.institucion_educativa,
      fecha_senecyt:
        data.fecha_registro_senecyt == null
          ? ""
          : moment(data.fecha_registro_senecyt).format("YYYY-MM-DD"),
      titulo: data.catalogo_titulo_id == null ? "" : data.catalogo_titulo_id,
      id_empleado: data.empleado_id_sap == 0 ? "" : data.empleado_id_sap,
      loading: false
    });
    this.saveEdad();
  }

  consultarWSHuella() {
    console.log('PROCESO CONSULTA', 'consultarWSHuella');
    event.preventDefault();
    this.validationConsumo();

    if (this.state.consIsValid == true) {
      this.state.codigo_dactilar = this.state.codigo_dactilar.toUpperCase();
      this.setState({
        loading: true,
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
      console.log('CONSULTA TABLE CONSULTA PUBLICA');


      axios
        .post("/RRHH/Colaboradores/ConsumirHuella/", {
          cedula: this.state.nro_identificacion,
          huella_dactilar: this.state.codigo_dactilar
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
            this.setState({ loading: false });
            if (response.data.return.CodigoError != "000") {
              abp.notify.error("" + response.data.return.Error, "Error");
            } else {
              abp.notify.success(
                response.data.return.Error +
                " : " +
                this.state.nro_identificacion,
                "Aviso"
              );
              this.procesarDatosConsumo(response.data.return);
            }
          }
        })
        .catch(error => {
          this.setState({ loading: false });
          console.log(error);
        });

      // this.props.consultarWSHuella(this.state.nro_identificacion, this.state.codigo_dactilar, true);
    } else {
      abp.notify.error("Complete los campos necesarios", "Error");
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
      var sexo = this.state.generos.filter(c => c.codigo == GENERO_MUJER);
      this.setState({ genero: sexo[0].Id, disable_sexo: true });
    } else if (datos.Sexo == REGISTRO_CIVIL_HOMBRE) {
      var sexo = this.state.generos.filter(c => c.codigo == GENERO_VARON);
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
      var estado = this.state.estados.filter(
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
      var estado = this.state.estados.filter(
        c => c.codigo == ESTADO_CIVIL_DIVORCIADO
      );
      this.setState({ estado_civil: estado[0].Id, disable_estado_civil: true });
    } else if (datos.EstadoCivil == REGISTRO_CIVIL_SOLTERO) {
      var estado = this.state.estados.filter(
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
      var n = this.state.tiposNacionalidades.filter(
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
      fotografia: datos.Fotografia,
      viene_registro_civil: true,
      fecha_registro_civil: moment().format("YYYY-MM-DD HH:mm:ss"),
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
      var sexo = this.state.generos.filter(c => c.codigo == GENERO_MUJER);
      this.setState({ genero: sexo[0].Id, disable_sexo: true });
    } else if (datos.sexo == REGISTRO_CIVIL_HOMBRE) {
      var sexo = this.state.generos.filter(c => c.codigo == GENERO_VARON);
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
      var estado = this.state.estados.filter(
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
        //disable_fecha_matrimonio: datos.fecha_matrimonio == "" ? false : true
      });
    } else if (datos.estado_civil == REGISTRO_CIVIL_DIVORCIADO) {
      var estado = this.state.estados.filter(
        c => c.codigo == ESTADO_CIVIL_DIVORCIADO
      );
      this.setState({ estado_civil: estado[0].Id, disable_estado_civil: true });
    } else if (datos.estado_civil == REGISTRO_CIVIL_SOLTERO) {
      var estado = this.state.estados.filter(
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
      var n = this.state.tiposNacionalidades.filter(
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
      fecha_registro_civil: moment(datos.fecha_consulta).format(
        "YYYY-MM-DD HH:mm:ss"
      ),
      loading: false
    });
    this.saveEdad();
  }

  validationConsumo() {
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
      var catalogo = this.state.tiposIdentificacion.filter(
        c => c.Id == Number.parseInt(this.state.tipo_identificacion)
      );
      console.log("catalogo", catalogo);
      if (catalogo[0].codigo != "CEDULA") {
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
          El campo No. de Identificación es obligatorio
        </div>
      );
    }

    if (this.state.TipoIdentificacionName === "CÉDULA") {
      console.log("ENTRO CEDULA", this.state.TipoIdentificacionName);

      console.log("lenght", this.state.nro_identificacion.length);
      var cedula_valida = this.validarcedula();
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
    if (this.state.TipoIdentificacionName === "PASAPORTE") {
      if (
        this.state.nro_identificacion != null &&
        this.state.nro_identificacion.length > 13
      ) {
        this.state.consIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            No. de Identificación no debe exceder de 13 dígitos{" "}
          </div>
        );
      }
    }

    if (!this.state.codigo_dactilar) {
      this.state.consIsValid = false;
      errors["codigo_dactilar"] = (
        <div className="alert alert-danger">
          El campo Código Dactilar es obligatorio
        </div>
      );
    } else {
      if (
        this.state.codigo_dactilar.length < 6 ||
        this.state.codigo_dactilar.length > 10
      ) {
        this.state.consIsValid = false;
        errors["codigo_dactilar"] = (
          <div className="alert alert-danger">
            El campo debe tener entre seis y diez dígitos
          </div>
        );
      }
    }

    this.setState({ errCons: errors });
  }

  validationConsumoBDD() {
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
      var catalogo = this.state.tiposIdentificacion.filter(
        c => c.Id == Number.parseInt(this.state.tipo_identificacion)
      );

      if (catalogo[0].codigo != "CEDULA") {
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
          El campo No. de Identificación es obligatorio
        </div>
      );
    } else if (catalogo != null && catalogo[0].codigo == "CEDULA") {
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

  convertirimagen(binary) {
    if (binary != null) {
      return (
        <img
          src={`data:image/jpeg;base64,${binary}`}
          height="140"
          width="140"
        />
      );
    } else {
      return "";
    }
  }

  render() {
    const headerexterno = (
      <img alt="Card" src="showcase/resources/demo/images/usercard.png" />
    );
    const footerexterno = (
      <span>
        <Button label="Save" icon="pi pi-check" />
        <Button
          label="Cancel"
          icon="pi pi-times"
          className="p-button-secondary"
        />
      </span>
    );

    const footer = (
      <div>
        <Button label="Yes" icon="pi pi-check" onClick={() => this.Guardar()} />
        <Button
          label="No"
          icon="pi pi-times"
          onClick={() => this.cerrarConfirmacion()}
          className="p-button-secondary"
        />
      </div>
    );
    return (
      <BlockUi tag="div" blocking={this.state.loading}>
        <div className="row">
          <div
            className="col-xs-10 col-md-10"
            style={{ display: "inline-block" }}
          >
            <form onSubmit={this.handleSubmit}></form>
            <div className="row">
              <div className="col">
                <Field
                  name="empresa_id"
                  required
                  value={this.state.empresa_id}
                  label="Empresa"
                  options={this.state.empresas}
                  type={"select"}
                  onChange={this.onChangeValue}
                  error={this.state.errors.empresa_id}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
                {this.state.errores["empresa_id"]}
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="empleado_id_sap_local">ID SAP Local:</label>
                  <input
                    type="number"
                    id="empleado_id_sap_local"
                    className="form-control"
                    value={this.state.empleado_id_sap_local}
                    onChange={this.handleChangeUpperCase}
                    name="empleado_id_sap_local"
                    disabled
                  />
                  {this.state.errores["empleado_id_sap_local"]}

                </div>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="id_candidato">* ID Candidato:</label>
                  <input
                    type="number"
                    id="id_candidato"
                    className="form-control"
                    value={this.state.id_candidato}
                    onChange={this.handleChangeUpperCase}
                    name="id_candidato"
                  />
                  {this.state.errores["id_candidato"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="id_empleado">ID Empleado (SAP Global):</label>
                  <input
                    type="number"
                    id="id_empleado"
                    className="form-control"
                    value={this.state.id_empleado}
                    onChange={this.handleChange}
                    name="id_empleado"
                    disabled
                  />
                  {this.state.errores["id_empleado"]}
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="fecha_ingreso">* Fecha de Ingreso: </label>
                  <input
                    type="date"
                    id="fecha_ingreso"
                    className="form-control"
                    value={this.state.fecha_ingreso}
                    onChange={this.handleChange}
                    name="fecha_ingreso"
                  />
                  {this.state.errores["fecha_ingreso"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="encargado_personal">
                    * Encargado de Personal:{" "}
                  </label>
                  <select
                    value={this.state.encargado_personal}
                    onChange={this.handleChange}
                    className="form-control"
                    name="encargado_personal"
                  >
                    <option value="">Seleccione...</option>
                    {this.getFormSelectEncargadoPersonal()}
                  </select>
                  {this.state.errores["encargado_personal"]}
                </div>
              </div>
            </div>

            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="destino">* Destino (Estancia): </label>
                  <select
                    value={this.state.destino}
                    onChange={this.handleChange}
                    className="form-control"
                    name="destino"
                  >
                    <option value="">Seleccione...</option>
                    {this.getFormSelectDestinos()}
                  </select>
                  {this.state.errores["destino"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="proyecto">* Proyecto: </label>
                  <select
                    value={this.state.proyecto}
                    onChange={this.handleChange}
                    className="form-control"
                    name="proyecto"
                  >
                    <option value="">Seleccione...</option>
                    {this.getFormSelectProyecto()}
                  </select>
                  {this.state.errores["proyecto"]}
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="label">* Tipo de Identificación: </label>
                  <select
                    value={this.state.tipo_identificacion}
                    onChange={this.handleChangeIden}
                    className="form-control"
                    name="tipo_identificacion"
                    required
                  >
                    <option value="">Seleccione...</option>
                    {this.getFormSelectTipoIdent()}
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
                    onBlur={this.VerificaIdentificacionUnica}
                    name="nro_identificacion"
                    required
                  />
                  {this.state.errores["nro_identificacion"]}
                  {this.state.errCons["nro_identificacion"]}
                </div>
              </div>
            </div>

            {this.state.confirmws && (
              <div className="row">
                <div className="col">
                  <div className="form-group">
                    <label htmlFor="codigo_dactilar">Código Dactilar: </label>
                    <input
                      type="text"
                      id="codigo_dactilar"
                      className="form-control"
                      value={this.state.codigo_dactilar}
                      onChange={this.handleChangeUpperCase}
                      name="codigo_dactilar"
                      /*style={{ width: '78%', display: 'inline' }}*/ required
                    />
                    {this.state.errores["codigo_dactilar"]}
                    {this.state.errCons["codigo_dactilar"]}
                  </div>
                </div>

                <div className="col">
                  <div div className="form-group" style={{ paddingTop: "7px" }}>
                    <button
                      type="button"
                      className="btn btn-outline-primary"
                      onClick={this.consultarWSHuella}
                      style={{ marginTop: "6%", marginLeft: "3px" }}
                    >
                      WS
                    </button>
                  </div>
                </div>
              </div>
            )}

            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="nombres_apellidos">Apellidos Nombres: </label>
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
                  <label htmlFor="segundo_apellido">Segundo Apellido: </label>
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
                    {this.getFormSelectGenero()}
                  </select>
                  {this.state.errores["genero"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="label">* Etnia: </label>
                  <select
                    value={this.state.etnia}
                    onChange={this.handleChange}
                    className="form-control"
                    name="etnia"
                  >
                    <option value="">Seleccione...</option>
                    {this.getFormSelectEtnia()}
                  </select>
                  {this.state.errores["etnia"]}
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
                  <label htmlFor="observacion">Edad: </label>
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
                    {this.getFormSelectNacionalidades()}
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
                    {this.getFormSelectEstadoCivil()}
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
                    Fecha de Matrimonio:{" "}
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
                <div className="form-group">
                  <label htmlFor="hijos">Número de Hijos: </label>
                  <input
                    type="number"
                    id="hijos"
                    className="form-control"
                    value={this.state.hijos}
                    onChange={this.handleChange}
                    name="hijos"
                  />
                  {this.state.errores["hijos"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="formacion_educativa">
                    Formación Educativa:{" "}
                  </label>
                  <select
                    value={this.state.formacion_educativa}
                    onChange={this.handleChange}
                    className="form-control"
                    name="formacion_educativa"
                  >
                    <option value="">Seleccione...</option>
                    {this.getFormSelectFormacionEducativa()}
                  </select>
                  {this.state.errores["formacion_educativa"]}
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="formacion">Formación: </label>
                  <select
                    value={this.state.formacion}
                    onChange={this.handleChange}
                    className="form-control"
                    name="formacion"
                  >
                    <option value="">Seleccione...</option>
                    {this.getFormSelectFormacion()}
                  </select>
                  {this.state.errores["formacion"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="institucion">
                    Nombre de la Institución Educativa:{" "}
                  </label>
                  <input
                    type="text"
                    id="institucion"
                    className="form-control"
                    value={this.state.institucion}
                    onChange={this.handleChangeUpperCase}
                    name="institucion"
                  />
                  {this.state.errores["institucion"]}
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="titulo">Título: </label>
                  <select
                    value={this.state.titulo}
                    onChange={this.handleChange}
                    className="form-control"
                    name="titulo"
                  >
                    <option value="">Seleccione...</option>
                    {this.getFormSelectTitulos()}
                  </select>
                  {this.state.errores["titulo"]}
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="fecha_senecyt">
                    Fecha de Registro Senecyt:{" "}
                  </label>
                  <input
                    type="date"
                    id="fecha_senecyt"
                    className="form-control"
                    value={this.state.fecha_senecyt}
                    onChange={this.handleChange}
                    name="fecha_senecyt"
                  />
                  {this.state.errores["fecha_senecyt"]}
                </div>
              </div>
            </div>

            <div className="row">
              <div className="col">
                <div
                  className="form-group checkbox"
                  style={{ display: "inline-flex", marginTop: "32px" }}
                >
                  <label htmlFor="discapacidad" style={{ width: "285px" }}>
                    ¿Discapacidad?:{" "}
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
              <div
                className="col"
                style={{
                  visibility:
                    this.state.discapacidad == true ? "visible" : "hidden"
                }}
              >
                <div className="form-group">
                  <label htmlFor="tipo_dis">Tipo de Discapacidad: </label>
                  <select
                    value={this.state.tipo_dis}
                    onChange={this.handleChange}
                    className="form-control"
                    name="tipo_dis"
                  >
                    <option value="">Seleccione...</option>
                    {this.getFormSelectTipoDiscapacidad()}
                  </select>
                  {this.state.errores["tipo_dis"]}
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
              <div className="col">
                <div className="form-group">
                  <label htmlFor="codigo_siniestro">Código Siniestro: </label>
                  <select
                    value={this.state.codigo_siniestro}
                    onChange={this.handleChange}
                    className="form-control"
                    name="codigo_siniestro"
                  >
                    <option value="">Seleccione...</option>
                    {this.getFormSelectCodigoSiniestro()}
                  </select>
                  {this.state.errores["codigo_siniestro"]}
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
                  <label htmlFor="codigo_incapacidad">
                    Código Incapacidad:{" "}
                  </label>
                  <select
                    value={this.state.codigo_incapacidad}
                    onChange={this.handleChange}
                    className="form-control"
                    name="codigo_incapacidad"
                  >
                    <option value="">Seleccione...</option>
                    {this.getFormSelectCodigoIncapacidad()}
                  </select>
                  {this.state.errores["codigo_incapacidad"]}
                </div>
              </div>
              <div className="col"></div>
            </div>

            <div
              className="row"
              style={{
                marginTop: this.state.discapacidad == true ? "0px" : "-170px"
              }}
            >
              <div className="col">
                <div
                  className="form-group checkbox"
                  style={{ display: "inline-flex", marginTop: "32px" }}
                >
                  <label htmlFor="es_sustituto" style={{ width: "285px" }}>
                    Es Sustituto?:{" "}
                  </label>
                  <input
                    type="checkbox"
                    id="es_sustituto"
                    className="form-control"
                    checked={this.state.es_sustituto}
                    onChange={this.handleInputChange}
                    name="es_sustituto"
                    style={{ marginTop: "5px", marginLeft: "-90%" }}
                  />
                </div>
              </div>
              <div
                className="col"
                style={{
                  visibility:
                    this.state.es_sustituto == true ? "visible" : "hidden"
                }}
              >
                <div className="form-group">
                  <label htmlFor="fecha_sustituto_desde">
                    Fecha Sustituto desde:{" "}
                  </label>
                  <input
                    type="date"
                    id="fecha_sustituto_desde"
                    className="form-control"
                    value={this.state.fecha_sustituto_desde}
                    onChange={this.handleChange}
                    name="fecha_sustituto_desde"
                  />
                  {this.state.errores["fecha_sustituto_desde"]}
                </div>
              </div>
            </div>

            <br />
            <div className="form-group">
              <div className="col">
                <button
                  type="button"
                  onClick={this.abrirConfirmacion}
                  className="btn btn-outline-primary fa fa-save"
                >
                  {" "}
                  Guardar
                </button>
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
          <div style={{ display: "inline-block" }}>
            {this.convertirimagen(this.state.datosConsumo.Fotografia)}
          </div>
          <Dialog
            header="Mensaje de Confirmación"
            visible={this.state.estado}
            footer={footer}
            width="50vw"
            onHide={this.cerrarConfirmacion}
          >
            <div>
              Está seguro de guardar la información registrada?. Se procederá a
              generar el número de legajo temporal del colaborador.
            </div>
            <br />
          </Dialog>
        </div>

        <Dialog
          header="Confirmación"
          visible={this.state.viewconfirm}
          style={{ width: "50vw" }}
          modal
          onHide={this.onHide}
        >
          <div>
            Puede acceder al Registro Civil para tomar la información del
            colaborador, si está de acuerdo presione el botón WS, caso contrario
            ingrese la información
          </div>
          <br />
          <Button
            label="De Acuerdo"
            icon="pi pi-check"
            onClick={this.renderboton}
          />{" "}
          <Button
            label="Continuar Manualmente"
            icon="pi pi-times"
            className="p-button-secondary"
            onClick={this.onHide}
          />
        </Dialog>
        <Dialog
          header="Colaborador Externo Encontrado"
          visible={this.state.viewexterno}
          style={{ width: "50vw" }}
          modal
          onHide={this.onHideExterno}
        >
          <div>
            El número de identificación ingresada en el formulario pertenece a
            un Colaborador Externo, se inactivará el registro existente para
            continuar con el nuevo registro, Está seguro de continuar?
          </div>
          <br />
          <div align="right">
            <Button
              label="SI"
              icon="pi pi-check"
              onClick={this.renderguardar}
            />{" "}
            <Button
              label="NO"
              icon="pi pi-times"
              className="p-button-secondary"
              onClick={this.onHideExterno}
            />
          </div>
        </Dialog>
      </BlockUi>
    );
  }

  VerificaIdentificacionUnica() {
    if (
      this.state.nro_identificacion != null &&
      this.state.nro_identificacion.length > 0
    ) {
      this.setState({ loading: true });
      var identificacion = this.state.nro_identificacion;
      var huella_dactilar = this.state.codigo_dactilar;
      axios
        .post("/RRHH/Colaboradores/GetIdUnico/", {
          numero: this.state.nro_identificacion
        })
        .then(response => {
          this.setState({ loading: false });
          if (response.data == "SI") {
            abp.notify.error(
              "El Número de Identificación ingresado ya existe en el Registro de Colaboradores",
              "Error"
            );
          }
          if (response.data == "INACTIVO") {
            abp.notify.error(
              "El Colaborador que intenta registrar esta INACTIVO se recomienda que sea reintegrado",
              "Error"
            );
          }
          if (response.data == "NO") {
            if (this.state.TipoIdentificacionName === "CÉDULA") {
              axios
                .post("/RRHH/Colaboradores/GetConsulta", {
                  identificacion: identificacion
                })
                .then(response => {
                  if (response.data.success == true) {
                    abp.notify.success(
                      "La información del colaborador ha sido tomada de la base de datos de candidatos.",
                      "Aviso"
                    );
                    this.procesarConsultaPublica(response.data.result);
                  } else {
                    console.log("No Existe en Consulta Publica");
                  }
                })
                .catch(error => {
                  console.log(error);
                  this.setState({ loading: false });
                });
            }
          }
        })
        .catch(error => {
          this.setState({ loading: false });
          console.log(error);
        });
    } else {
    }
  }

  abrirConfirmacion() {
    this.setState({ loading: true });
    axios
      .post("/RRHH/Colaboradores/GetExternoActivo/", {
        Id: this.state.nro_identificacion
      })
      .then(response => {
        if (response.data === "NO_EXTERNO") {
          this.handleValidation();
          if (this.state.formIsValid == true) {
            this.setState({ estado: true, viewexterno: false });
          } else {
            abp.notify.error(
              "Se ha encontrado errores, por favor revisar el formulario",
              "Error"
            );
          }
          this.setState({ loading: false });
        } else {
          this.setState({ viewexterno: true });
          this.setState({ loading: false });
        }
      })
      .catch(error => {
        console.log(error);
        this.setState({ loading: false, estado: false, viewexterno: true });
      });
  }

  cerrarConfirmacion() {
    this.setState({ estado: false, viewexterno: false });
  }
  renderboton() {
    this.setState({ confirmws: true, viewconfirm: false });
  }
  renderguardar = () => {
    this.handleValidation();
    if (this.state.formIsValid == true) {
      this.setState({ estado: true });
    } else {
      abp.notify.error(
        "Se ha encontrado errores, por favor revise el formulario",
        "Error"
      );
    }
  };



  handleValidation() {
    let errors = {};
    this.state.formIsValid = true;

    if (this.state.empresa_id == 0) {
      this.state.formIsValid = false;
      errors.empresa_id = "El campo Empresa es obligatorio";
      errors["empresa_id"] = (
        <div className="alert alert-danger">
          El campo Empresa es obligatorio
        </div>
      );
    }
    /* if (!this.state.empleado_id_sap_local) {
       this.state.formIsValid = false;
       errors["empleado_id_sap_local"] = (
         <div className="alert alert-danger">
           El campo ID SAP Local es obligatorio
         </div>
       );
     }*/

    if (!this.state.id_candidato) {
      this.state.formIsValid = false;
      errors["id_candidato"] = (
        <div className="alert alert-danger">
          El campo ID Candidato es obligatorio
        </div>
      );
    } else {
      if (!isFinite(this.state.id_candidato)) {
        this.state.formIsValid = false;
        errors["id_candidato"] = (
          <div className="alert alert-danger">
            El campo permite solo ingreso numérico
          </div>
        );
      }
      if (this.state.id_candidato < 0) {
        this.state.formIsValid = false;
        errors["id_candidato"] = (
          <div className="alert alert-danger">
            El campo no permite números negativos
          </div>
        );
      }
      if (this.state.id_candidato.length > 10) {
        this.state.formIsValid = false;
        errors["id_candidato"] = (
          <div className="alert alert-danger">
            El campo no puede tener más de diez dígitos
          </div>
        );
      }
    }
    if (!this.state.fecha_ingreso) {
      this.state.formIsValid = false;
      errors["fecha_ingreso"] = (
        <div className="alert alert-danger">
          El campo Fecha de Ingreso es obligatorio
        </div>
      );
    } else {


      var fecha = moment().format("YYYY-MM-DD");
      let fechaMoment = moment(fecha, "YYYY-MM-DD").add(10, 'days');
      let fechaFormat = moment(fechaMoment).format("YYYY-MM-DD");
      let fechaFormatMSG = moment(fechaMoment).format("DD-MM-YYYY");
      console.log('fechaActual', fecha);
      console.log('fechaActual10Dias', fechaFormat);
      let fechaIngresoFormat = moment(this.state.fecha_ingreso).format("YYYY-MM-DD");
      console.log('statefechaingreso', fechaIngresoFormat);
      console.log(fechaIngresoFormat > fechaFormat)


      if (this.state.fecha_ingreso > fechaFormat) {
        console.log('Fecha Mayo A 10 dias posteriores', fechaFormat);
        this.state.formIsValid = false;
        errors["fecha_ingreso"] = (
          <div className="alert alert-danger">
            La fecha ingresada no puede ser mayor a 10 días posteriores a la fecha actual - Fecha Máxima: {fechaFormatMSG}
          </div>
        );
      }

      /* if (this.state.fecha_ingreso > fecha) {
         this.state.formIsValid = false;
         errors["fecha_ingreso"] = (
           <div className="alert alert-danger">
             La fecha ingresada no puede ser mayor a la fecha actual
           </div>
         );
       }*/
    }
    if (!this.state.tipo_identificacion) {
      this.state.formIsValid = false;
      errors["tipo_identificacion"] = (
        <div className="alert alert-danger">
          El campo Tipo de Identificación es obligatorio
        </div>
      );
    } else {
      var catalogo = this.state.tiposIdentificacion.filter(
        c => c.Id == Number.parseInt(this.state.tipo_identificacion)
      );
    }
    if (!this.state.nro_identificacion) {
      this.state.formIsValid = false;
      errors["nro_identificacion"] = (
        <div className="alert alert-danger">
          El campo No. de Identificación es obligatorio
        </div>
      );
    }

    if (this.state.TipoIdentificacionName === "CÉDULA") {
      console.log("ENTRO CEDULA", this.state.TipoIdentificacionName);

      console.log("lenght", this.state.nro_identificacion.length);
      var cedula_valida = this.validarcedula();
      if (!cedula_valida) {
        this.state.formIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            No. de Identificación es inválido
          </div>
        );
      }
      if (!isFinite(this.state.nro_identificacion)) {
        this.state.formIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            El campo permite solo ingreso numérico
          </div>
        );
      }
      if (this.state.nro_identificacion.length != 10) {
        this.state.formIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            El campo debe tener 10 dígitos
          </div>
        );
      }
    }
    if (this.state.TipoIdentificacionName === "PASAPORTE") {
      if (
        this.state.nro_identificacion != null &&
        this.state.nro_identificacion.length > 13
      ) {
        this.state.formIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            No. de Identificación no debe exceder de 13 dígitos{" "}
          </div>
        );
      }
    }

    if (!this.state.primer_apellido) {
      this.state.formIsValid = false;
      errors["primer_apellido"] = (
        <div className="alert alert-danger">
          El campo Primer Apellido es obligatorio
        </div>
      );
    }
    if (!this.state.nombres) {
      this.state.formIsValid = false;
      errors["nombres"] = (
        <div className="alert alert-danger">
          El campo Nombres es obligatorio
        </div>
      );
    }

    if (this.state.hijos != "" && this.state.hijos < 0) {
      this.state.formIsValid = false;
      errors["hijos"] = (
        <div className="alert alert-danger">El Campo no puede ser negativo</div>
      );
    }
    if (
      this.state.primer_apellido &&
      this.state.nombres &&
      this.state.nombres_apellidos
    ) {
      this.state.primer_apellido = this.state.primer_apellido;
      this.state.segundo_apellido = this.state.segundo_apellido;
      this.state.nombres = this.state.nombres;
      var completo = (
        this.state.primer_apellido +
        this.state.segundo_apellido +
        this.state.nombres
      );
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
        );
      } else if (
        this.state.primer_apellido &&
        this.state.nombres &&
        this.state.segundo_apellido == ""
      ) {
        this.state.nombres_apellidos = (
          this.state.primer_apellido +
          " " +
          this.state.nombres
        );
      }
    }
    console.log(this.state.nombres_apellidos.replace(/ /g, ""));
    if (!this.state.genero) {
      this.state.formIsValid = false;
      errors["genero"] = (
        <div className="alert alert-danger">El campo Género es obligatorio</div>
      );
    }
    if (!this.state.etnia) {
      this.state.formIsValid = false;
      errors["etnia"] = (
        <div className="alert alert-danger">El campo Etnia es obligatorio</div>
      );
    }
    if (!this.state.fecha_nacimiento) {
      this.state.formIsValid = false;
      errors["fecha_nacimiento"] = (
        <div className="alert alert-danger">
          El campo Fecha de Nacimiento es obligatorio
        </div>
      );
    } else {
      if (this.state.edad < 18) {
        this.state.formIsValid = false;
        errors["fecha_nacimiento"] = (
          <div className="alert alert-danger">
            El usuario no puede ser menor de 18 años
          </div>
        );
      }
    }
    if (!this.state.pais) {
      this.state.formIsValid = false;
      errors["pais"] = (
        <div className="alert alert-danger">
          El campo País de Nacimiento es obligatorio
        </div>
      );
    }
    if (!this.state.nacionalidad) {
      this.state.formIsValid = false;
      errors["nacionalidad"] = (
        <div className="alert alert-danger">
          El campo Nacionalidad es obligatorio
        </div>
      );
    }
    if (this.state.estado_civil) {
      var catEstadoCivil = this.state.estados.filter(
        c => c.Id == Number.parseInt(this.state.estado_civil)
      );
      console.log("catEstadoCivil", catEstadoCivil);
      var today = moment().format("YYYY-MM-DD");
      if (
        catEstadoCivil[0].codigo == ESTADO_CIVIL_CASADO &&
        !this.state.fecha_matrimonio
      ) {
        this.state.formIsValid = false;
        errors["fecha_matrimonio"] = (
          <div className="alert alert-danger">
            El campo Fecha de Matrimonio es obligatorio
          </div>
        );
      } else if (
        catEstadoCivil[0].codigo == ESTADO_CIVIL_CASADO &&
        this.state.fecha_matrimonio > today
      ) {
        this.state.formIsValid = false;
        errors["fecha_matrimonio"] = (
          <div className="alert alert-danger">
            Fecha de Matrimonio no puede ser mayor a fecha actual
          </div>
        );
      }
    } else {
      this.state.formIsValid = false;
      errors["estado_civil"] = (
        <div className="alert alert-danger">
          El campo Estado Civil es obligatorio
        </div>
      );
    }

    if (this.state.discapacidad == true) {
      if (!this.state.tipo_dis) {
        this.state.formIsValid = false;
        errors["tipo_dis"] = (
          <div className="alert alert-danger">
            El campo Tipo de Discapacidad es obligatorio
          </div>
        );
      }
      if (!this.state.porcentaje_dis) {
        this.state.formIsValid = false;
        errors["porcentaje_dis"] = (
          <div className="alert alert-danger">
            El campo Porcentaje Discapacidad es obligatorio
          </div>
        );
      } else {
        if (this.state.porcentaje_dis > 100 || this.state.porcentaje_dis <= 0) {
          this.state.formIsValid = false;
          errors["porcentaje_dis"] = (
            <div className="alert alert-danger">
              Debe ingresar un valor entero entre 1 y 100
            </div>
          );
        }
      }
      if (!this.state.codigo_siniestro) {
        this.state.formIsValid = false;
        errors["codigo_siniestro"] = (
          <div className="alert alert-danger">
            El campo Codigo Siniestro es obligatorio
          </div>
        );
      }
      if (!this.state.formacion_educativa) {
        this.state.formIsValid = false;
        errors["formacion_educativa"] = (
          <div className="alert alert-danger">
            El campo Formación Educativa es obligatorio
          </div>
        );
      }
      if (!this.state.codigo_incapacidad) {
        this.state.formIsValid = false;
        errors["codigo_incapacidad"] = (
          <div className="alert alert-danger">
            El campo Código Incapacidad es obligatorio
          </div>
        );
      }
    }
    if (this.state.fecha_senecyt) {
      var fecha = moment().format("YYYY-MM-DD");
      if (this.state.fecha_senecyt > fecha) {
        this.state.formIsValid = false;
        errors["fecha_senecyt"] = (
          <div className="alert alert-danger">
            La fecha ingresada no puede ser mayor a la fecha actual
          </div>
        );
      }
    }

    if (!this.state.encargado_personal) {
      this.state.formIsValid = false;
      errors["encargado_personal"] = (
        <div className="alert alert-danger">
          El campo Encargado de Personal es obligatorio
        </div>
      );
    } else {
      var ep = this.state.tiposEncargadoPersonal.filter(
        c => c.Id == Number.parseInt(this.state.encargado_personal)
      );
      console.log(ep);
      if (ep[0].codigo.replace(/ /g, "") == ENCARGADO_PERSONAL_MENSUALES) {
        if (!this.state.formacion_educativa) {
          this.state.formIsValid = false;
          errors["formacion_educativa"] = (
            <div className="alert alert-danger">
              El campo Formación Educativa es obligatorio
            </div>
          );
        }
        if (!this.state.formacion) {
          this.state.formIsValid = false;
          errors["formacion"] = (
            <div className="alert alert-danger">
              El campo Formación es obligatorio
            </div>
          );
        }
        if (!this.state.institucion) {
          this.state.formIsValid = false;
          errors["institucion"] = (
            <div className="alert alert-danger">
              El campo Nombre de la Institución Educativa es obligatorio
            </div>
          );
        }
        if (!this.state.titulo) {
          this.state.formIsValid = false;
          errors["titulo"] = (
            <div className="alert alert-danger">
              El campo Título es obligatorio.
            </div>
          );
        }
      }
    }
    if (!this.state.destino) {
      this.state.formIsValid = false;
      errors["destino"] = (
        <div className="alert alert-danger">
          El campo Destino (Estancia) es obligatorio
        </div>
      );
    }
    if (!this.state.proyecto) {
      this.state.formIsValid = false;
      errors["proyecto"] = (
        <div className="alert alert-danger">
          El campo Proyecto es obligatorio
        </div>
      );
    }
    if (this.state.es_sustituto == true) {
      if (!this.state.fecha_sustituto_desde) {
        this.state.formIsValid = false;
        errors["fecha_sustituto_desde"] = (
          <div className="alert alert-danger">
            El campo Fecha Sustituto desde es obligatorio
          </div>
        );
      } else {
        var fecha = moment().format("YYYY-MM-DD");
        if (this.state.fecha_sustituto_desde > fecha) {
          this.state.formIsValid = false;
          errors["fecha_sustituto_desde"] = (
            <div className="alert alert-danger">
              La fecha ingresada no puede ser mayor a la fecha actual
            </div>
          );
        }
      }
    }

    this.setState({ errores: errors });

    return this.state.formIsValid;
  }
  validarcedula = () => {
    let errors = {};
    var cedula = this.state.nro_identificacion;

    //Preguntamos si la cedula consta de 10 digitos
    if (cedula.length == 10) {
      //Obtenemos el digito de la region que sonlos dos primeros digitos
      var digito_region = cedula.substring(0, 2);

      //Pregunto si la region existe ecuador se divide en 24 regiones
      if (digito_region >= 1 && digito_region <= 24) {
        // Extraigo el ultimo digito
        var ultimo_digito = cedula.substring(9, 10);

        //Agrupo todos los pares y los sumo
        var pares =
          parseInt(cedula.substring(1, 2)) +
          parseInt(cedula.substring(3, 4)) +
          parseInt(cedula.substring(5, 6)) +
          parseInt(cedula.substring(7, 8));

        //Agrupo los impares, los multiplico por un factor de 2, si la resultante es > que 9 le restamos el 9 a la resultante
        var numero1 = cedula.substring(0, 1);
        var numero1 = numero1 * 2;
        if (numero1 > 9) {
          var numero1 = numero1 - 9;
        }

        var numero3 = cedula.substring(2, 3);
        var numero3 = numero3 * 2;
        if (numero3 > 9) {
          var numero3 = numero3 - 9;
        }

        var numero5 = cedula.substring(4, 5);
        var numero5 = numero5 * 2;
        if (numero5 > 9) {
          var numero5 = numero5 - 9;
        }

        var numero7 = cedula.substring(6, 7);
        var numero7 = numero7 * 2;
        if (numero7 > 9) {
          var numero7 = numero7 - 9;
        }

        var numero9 = cedula.substring(8, 9);
        var numero9 = numero9 * 2;
        if (numero9 > 9) {
          var numero9 = numero9 - 9;
        }

        var impares = numero1 + numero3 + numero5 + numero7 + numero9;

        //Suma total
        var suma_total = pares + impares;

        //extraemos el primero digito
        var primer_digito_suma = String(suma_total).substring(0, 1);

        //Obtenemos la decena inmediata
        var decena = (parseInt(primer_digito_suma) + 1) * 10;

        //Obtenemos la resta de la decena inmediata - la suma_total esto nos da el digito validador
        var digito_validador = decena - suma_total;

        //Si el digito validador es = a 10 toma el valor de 0
        if (digito_validador == 10) var digito_validador = 0;

        //Validamos que el digito validador sea igual al de la cedula
        if (digito_validador == ultimo_digito) {
          // this.props.showSuccess('la cédula:' + cedula + ' es correcta');
          return true;
        } else {
          errors["nro_identificacion"] = (
            <div className="alert alert-danger">
              No. de Identificación es inválido
            </div>
          );
          this.setState({ formIsValid: false, errores: errors });

          return false;
        }
      } else {
        // imprimimos en consola si la region no pertenece
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            No. de Identificación no pertenece a ninguna región
          </div>
        );
        this.setState({ formIsValid: false, errores: errors });

        return false;
      }
    } else {
      //imprimimos en consola si la cedula tiene mas o menos de 10 digitos
      errors["nro_identificacion"] = (
        <div className="alert alert-danger">
          No. de Identificación debe tener 10 Dígitos
        </div>
      );
      this.setState({ formIsValid: false, errores: errors });

      return false;
    }
  };

  Guardar() {
    this.setState({ loading: true });
    axios
      .post("/RRHH/Colaboradores/CreateApiAsync/", {
        Id: this.state.id_colaborador == 0 ? null : this.state.id_colaborador,
        fecha_ingreso: this.state.fecha_ingreso,
        catalogo_tipo_identificacion_id: this.state.tipo_identificacion,
        numero_identificacion: this.state.nro_identificacion,
        primer_apellido: this.state.primer_apellido,
        segundo_apellido: this.state.segundo_apellido,
        nombres: this.state.nombres,
        fecha_nacimiento: this.state.fecha_nacimiento,
        catalogo_genero_id: this.state.genero,
        PaisId: this.state.pais,
        pais_pais_nacimiento_id: this.state.nacionalidad,
        catalogo_etnia_id: this.state.etnia,
        catalogo_estado_civil_id: this.state.estado_civil,
        fecha_matrimonio: this.state.fecha_matrimonio,
        catalogo_codigo_siniestro_id: this.state.codigo_siniestro,
        catalogo_formacion_educativa_id: this.state.formacion_educativa,
        numero_hijos: this.state.hijos == "" ? "0" : this.state.hijos,
        candidato_id_sap: this.state.id_candidato,
        usuario_id: 2,
        discapacidad: this.state.discapacidad,
        catalogo_tipo_discapacidad_id: this.state.tipo_dis,
        catalogo_porcentaje_id: this.state.porcentaje_dis,
        formacion: this.state.formacion,
        institucion_educativa: this.state.institucion,
        catalogo_titulo_id: this.state.titulo,
        estado: "TEMPORAL",
        catalogo_codigo_incapacidad_id: this.state.codigo_incapacidad,
        nombres_apellidos: this.state.nombres_apellidos,
        fecha_registro_senecyt: this.state.fecha_senecyt,
        codigo_dactilar: this.state.codigo_dactilar,
        nombres_apellidos: this.state.nombres_apellidos,
        catalogo_encargado_personal_id: this.state.encargado_personal,
        catalogo_destino_estancia_id: this.state.destino,
        ContratoId: this.state.proyecto,
        viene_registro_civil: this.state.viene_registro_civil,
        fecha_registro_civil: this.state.fecha_registro_civil,
        es_sustituto: this.state.es_sustituto,
        fecha_sustituto_desde:
          this.state.es_sustituto == true
            ? this.state.fecha_sustituto_desde
            : "",
        empresa_id: this.state.empresa_id,
        empleado_id_sap_local: this.state.empleado_id_sap_local
      })
      .then(response => {
        if (response.data == -1) {
          this.setState({ loading: false });
          abp.notify.error(
            "Número de Identificación ingresada ya existe",
            "Error"
          );
        } else if (response.data > 0) {
          this.cerrarConfirmacion();
          if (this.state.datosConsumo.Fotografia != null) {
            SSSS;
            this.subirArchivo(response.data);
            this.props.ColaboradorId(response.data);
          } else {
            this.setState({ loading: false });
            abp.notify.success("Colaborador Guardado!", "Aviso");
            this.setState({ id_colaborador: response.data });
            this.props.ColaboradorId(response.data);
            this.props.ChangeTab(this.state.destino);
            this.props.Siguiente(1);
          }
          this.props.CargaCategoriaEncargado(this.state.encargado_personal);
          this.props.CargaNomina(this.state.encargado_personal);
        } else {
          this.setState({ loading: false });
          abp.notify.error("Algo salió mal", "Error");
        }
      })
      .catch(error => {
        console.log(error);
        abp.notify.error("Algo salió mal", "Error");
        this.setState({ loading: false });
      });
  }

  subirArchivo(id) {
    var file = this.b64toBlob(this.state.datosConsumo.Fotografia, "image/png");
    console.log(file);
    const formData = new FormData();
    formData.append("idColaborador", id);
    formData.append("UploadedFile", file);
    const config = { headers: { "content-type": "multipart/form-data" } };

    axios
      .post("/RRHH/Colaboradores/CreateArchivoFotografia/", formData, config)
      .then(response => {
        this.setState({ loading: false });
        if (response.data == "OK") {
          abp.notify.success("Colaborador Guardado!", "Aviso");
          this.props.ChangeTab(this.state.destino);
          this.props.Siguiente(1);
        } else {
          abp.notify.error("Algo salió mal", "Error");
        }
      })
      .catch(error => {
        abp.notify.error("Algo salió mal", "Error");
      });
  }

  b64toBlob(b64Data, contentType) {
    var sliceSize = 512;

    var byteCharacters = atob(b64Data);
    var byteArrays = [];

    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      var slice = byteCharacters.slice(offset, offset + sliceSize);

      var byteNumbers = new Array(slice.length);
      for (var i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }

      var byteArray = new Uint8Array(byteNumbers);

      byteArrays.push(byteArray);
    }

    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
  }

  GetCatalogos() {
    axios
      .post("/RRHH/Colaboradores/GetEmpresasApi/", {})
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });
        this.setState({ empresas: items });
      })
      .catch(error => {
        console.log(error);
      });

    let codigos = [];

    codigos = [
      "TIPOINDENTIFICACION",
      "GENERO",
      "ETNIA",
      "ESTADOCIVIL",
      "TIPODISCAPACIDAD",
      "CODIGOSINIESTRO",
      "FORMACIONEDUCATIVA",
      "FORMACION",
      "TITULO",
      "NACIONALIDADES",
      "CODIGOINCAPACIDAD",
      "ENCARGADO",
      "DESTINOS",
      "PROYECTO"
    ];

    axios
      .post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
      .then(response => {
        this.cargarCatalogos(response.data);
      })
      .catch(error => {
        console.log(error);
      });
  }


  cargarCatalogos(data) {
    this.setState({ loading: false });
    data.forEach(e => {
      var catalogo = JSON.parse(e);
      var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
      switch (codigoCatalogo) {
        case "TIPOINDENTIFICACION":
          this.setState({ tiposIdentificacion: catalogo });
          this.getFormSelectTipoIdent();
          return;
        case "NACIONALIDADES":
          this.setState({ tiposNacionalidades: catalogo });
          this.getFormSelectNacionalidades();
          return;
        case "GENERO":
          this.setState({ generos: catalogo });
          this.getFormSelectGenero();
          return;
        case "ETNIA":
          this.setState({ etnias: catalogo });
          this.getFormSelectEtnia();
          return;
        case "ESTADOCIVIL":
          this.setState({ estados: catalogo });
          this.getFormSelectEstadoCivil();
          return;
        case "TIPODISCAPACIDAD":
          this.setState({ tipoDiscapacidad: catalogo });
          this.getFormSelectTipoDiscapacidad();
          return;
        case "CODIGOSINIESTRO":
          this.setState({ codigoSiniestro: catalogo });
          this.getFormSelectCodigoSiniestro();
          return;
        case "FORMACIONEDUCATIVA":
          this.setState({ formacionAca: catalogo });
          this.getFormSelectFormacionEducativa();
          return;
        case "FORMACION":
          this.setState({ tiposFormaciones: catalogo });
          this.getFormSelectFormacion();
          return;
        case "TITULO":
          this.setState({ tiposTitulos: catalogo });
          this.getFormSelectTitulos();
          return;
        case "CODIGOINCAPACIDAD":
          this.setState({ tiposIncapacidades: catalogo });
          this.getFormSelectCodigoIncapacidad();
          return;
        case "ENCARGADO":
          this.setState({ tiposEncargadoPersonal: catalogo });
          this.getFormSelectEncargadoPersonal();
          return;
        case "DESTINOS":
          this.setState({ tiposDestino: catalogo });
          this.getFormSelectDestinos();
          return;
        case "PROYECTO":
          this.setState({ tiposProyectos: catalogo });
          this.getFormSelectProyecto();
          return;
        default:
          console.log(codigoCatalogo);
          return;
      }
    });
  }

  getFormSelectProyecto() {
    return this.state.tiposProyectos.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectEncargadoPersonal() {
    return this.state.tiposEncargadoPersonal.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }
  getFormSelectEmpresas = () => {
    return this.state.empresas.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.razon_social}
        </option>
      );
    });
  };

  getFormSelectDestinos() {
    return this.state.tiposDestino.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectCodigoIncapacidad() {
    return this.state.tiposIncapacidades.map(item => {
      if (item.codigo.replace(/ /g, "") != CODIGO_INCAPACIDAD_NOINCAPACITADO) {
        return (
          <option key={Math.random()} value={item.Id}>
            {item.nombre}
          </option>
        );
      }
    });
  }

  getFormSelectNacionalidades() {
    return this.state.tiposNacionalidades.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectTipoIdent() {
    return this.state.tiposIdentificacion.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectGenero() {
    return this.state.generos.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectEtnia() {
    return this.state.etnias.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectEstadoCivil() {
    return this.state.estados.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectTipoDiscapacidad() {
    return this.state.tipoDiscapacidad.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectCodigoSiniestro() {
    return this.state.codigoSiniestro.map(item => {
      if (item.codigo.replace(/ /g, "") != CODIGO_SINIESTRO_NOINCAPACITADO) {
        return (
          <option key={Math.random()} value={item.Id}>
            {item.nombre}
          </option>
        );
      }
    });
  }

  getFormSelectFormacionEducativa() {
    return this.state.formacionAca.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectFormacion() {
    return this.state.tiposFormaciones.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
  }

  getFormSelectTitulos() {
    return this.state.tiposTitulos.map(item => {
      return (
        <option key={Math.random()} value={item.Id}>
          {item.nombre}
        </option>
      );
    });
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
    this.setState({ loading: true });
    this.setState({
      [event.target.name]: event.target.value,
      /*datosConsumo: [],
            nro_identificacion: '',
            codigo_dactilar: '',
            primer_apellido: '',
            segundo_apellido: '',
            nombres: '',
            genero: '',
            etnia: '',
            fecha_nacimiento: '',
            edad: '',
            pais: '',
            nacionalidad: '',
            estado_civil: '',
            fecha_matrimonio: '',
             */
      nombres_apellidos: "",
      disable_consumo: false,
      disable_estado_civil: false,
      disable_fecha_matrimonio: false,
      disable_nacionalidad: false,
      disable_sexo: false,
      codigo_dactilar: "",
      formacion_educativa: "",
      formacion: "",
      institucion: "",
      titulo: "",
      fecha_senecyt: ""
    });
    if (event.target.value > 0) {
      console.log(event.target.value);
      axios
        .get("/RRHH/Colaboradores/GetNamebyId/" + event.target.value, {})
        .then(response => {
          this.setState({ TipoIdentificacionName: response.data });
          console.log(this.state.TipoIdentificacionName);

          if (
            this.state.TipoIdentificacionName.length > 0 &&
            this.state.TipoIdentificacionName == "CÉDULA"
          ) {
            this.setState({ confirmws: true, loading: false });
          } else {
            console.log("TIPO PASAPORTE");
            this.setState({ confirmws: false, loading: false });
          }
        })
        .catch(error => {
          console.log(error);
          this.setState({ loading: false });
        });
    }
  }

  handleChangeEstadoCivil(event) {
    this.setState({ [event.target.name]: event.target.value });
    var estado = this.state.estados.filter(
      c => c.Id == Number.parseInt(event.target.value)
    );
    console.log(estado);
    if (estado[0].descripcion == "CASADO") {
      this.setState({ visible_fecha_matrimonio: true });
    } else {
      this.setState({ visible_fecha_matrimonio: false });
    }
  }

  handleInputChange(event) {
    const target = event.target;
    const value = target.type === "checkbox" ? target.checked : target.value;
    const name = target.name;

    this.setState({
      [name]: value
    });
  }

  handleChangeUpperCase(event) {
    this.setState({ [event.target.name]: event.target.value.toUpperCase() });
  }
  handleChangeCapitalize(event) {
    this.setState({ [event.target.name]: this.capitalize(event.target.value) });
  }
  capitalize = (word) => {
    if (word != undefined && word.length > 0) {
      word = this.removeAccents(word.toLowerCase());
      return word.replace(
        /\w\S*/g,
        function (txt) {
          return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
        }
      );
    } else {
      return "";
    }
  };
  removeAccents = (str) => {
    return str
      .normalize("NFD")
      .replace(
        /([^n\u0300-\u036f]|n(?!\u0303(?![\u0300-\u036f])))[\u0300-\u036f]+/gi,
        "$1"
      )
      .normalize();
  };

  toTitleCase = (str) => {

  }
}
