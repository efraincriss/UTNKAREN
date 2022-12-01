import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import moment from "moment";
import BlockUi from "react-block-ui";
import Field from "../Base/Field-v2";

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
} from "./Codigos";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Button } from "primereact/components/button/Button";

export default class EditarInformacionGeneral extends React.Component {
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
      discapacidad: false,
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
      errores: [],
      formIsValid: false,
      id_colaborador: "",
      estado: false,
      colaborador: [],
      usuario_id: "",
      codigo_incapacidad: "",
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
      sexo_sugerido: "",
      nacionalidad_sugerida: "",
      estado_civil_sugerido: "",
      fotografia: null,
      encargado_personal: "",
      destino: "",
      proyecto: "",
      viene_registro_civil: "",
      fecha_registro_civil: "",
      es_sustituto: false,
      fecha_sustituto_desde: "",
      msg: "",



      confirmws: false,

      //Empresa
      empresa_id: 0,

      empresas: [],
      errors: {},
      //Empresa
      empleado_id_sap_local: null,

      fecha_modificacion: "",

      fechaIngresoActualizada: false,
      fecha_ingreso_anterior: "",
      puedeContinuarActualizacion: false,
      mostrarMensajeActualizacionFecha: false,

      pass: "",
      viewdisable: false,
      urlApiBase: '/RRHH/Colaboradores/',


    };

    this.GetCatalogos = this.GetCatalogos.bind(this);
    this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);
    this.handleChangeCapitalize = this.handleChangeCapitalize.bind(this);
    this.handleChangeFechaNac = this.handleChangeFechaNac.bind(this);
    this.handleChangeEstadoCivil = this.handleChangeEstadoCivil.bind(this);
    this.saveEdad = this.saveEdad.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.handleChangeFechaIngreso = this.handleChangeFechaIngreso.bind(this);

    this.handleValidation = this.handleValidation.bind(this);
    this.Guardar = this.Guardar.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.abrirConfirmacion = this.abrirConfirmacion.bind(this);
    this.cerrarConfirmacion = this.cerrarConfirmacion.bind(this);

    this.consultarWSHuella = this.consultarWSHuella.bind(this);
    this.validationConsumo = this.validationConsumo.bind(this);
    this.procesarDatosConsumo = this.procesarDatosConsumo.bind(this);
    this.convertirimagen = this.convertirimagen.bind(this);
    // this.b64toBlob = this.b64toBlob.bind(this);

    this.consultaBDD = this.consultaBDD.bind(this);
    this.procesarDatosBDD = this.procesarDatosBDD.bind(this);
    this.getFotografia = this.getFotografia.bind(this);
    this.validationConsumoBDD = this.validationConsumoBDD.bind(this);
    this.onDesactivar = this.onDesactivar.bind(this);
  }

  componentDidMount() {
    this.GetCatalogos();
  }
  viewdisable = () => {
    console.log(Id);
    this.setState({ viewdisable: true });
  }

  onHideview = () => {

    this.setState({ viewdisable: false, pass: "" });
  }
  onDesactivar() {
    if (this.state.pass === "") {
      abp.notify.error("Debe ingresar el código de Seguridad", 'Error');

    } else {
      console.log('onDesactivar ');

      var self = this;
      self.setState({ blocking: true });

      let url = '';
      url = `${self.state.urlApiBase}/ValidarPassApi`;


      let data = {
        pass: this.state.pass
      };


      axios.post(url, data)
        .then((response) => {

          let data = response.data;

          if (data.result === true) {

            abp.notify.success("Clave de seguridad correcta", "Aviso");
            self.setState({ puedeContinuarActualizacion: true }, self.onHideview());
            self.Guardar();

            // self.GetColaboradores();

          } else {
            abp.notify.error("El código de seguridad es incorrecto, no está autorizado a continuar con el proceso", 'Error');
            //TODO: 
            //Presentar errores... 
            //var message = $.fn.responseAjaxErrorToString(data);
            // abp.notify.error(message, 'Error');
          }


          self.setState({ blocking: false });

        })
        .catch((error) => {
          console.log(error);

          self.setState({ blocking: false });
        });
    }


  }



  handleChangeFechaIngreso(event) {
    this.setState({ [event.target.name]: event.target.value });
  }
  validacionFechaActualizacionPeriodColaborador = () => {
    let errors = {};
    let fechaValida = false;
    if (!this.state.fecha_ingreso) {
      fechaValida = false;
      errors["fecha_ingreso"] = (
        <div className="alert alert-danger">
          El campo Fecha de Ingreso es obligatorio
        </div>
      );
    } else {
      var fecha = moment().format("YYYY-MM-DD");
      let fechaMoment = moment(fecha, "YYYY-MM-DD").add(10, 'days');
      let fechaFormat = moment(fechaMoment).format("YYYY-MM-DD");
      let fechaFormatmsg = moment(fechaMoment).format("DD-MM-YYYY")
      let fechaIngresoFormat = moment(this.state.fecha_ingreso).format("YYYY-MM-DD");
   
      console.log(fechaIngresoFormat > fechaFormat)
      if (this.state.fecha_ingreso > fechaFormat) {
        console.log('Fecha Mayo A 10 dias posteriores', fechaFormat);
        fechaValida = false;
        errors["fecha_ingreso"] = (
          <div className="alert alert-danger">
            La fecha ingresada no puede ser mayor a 10 días posteriores a la fecha actual - Fecha Máxima: {fechaFormatmsg}
          </div>
        );
      }

      axios
        .post("/RRHH/Colaboradores/GetValidarFechaPeriodoRegistrosAnteriores/", {
          Id: this.props.colaborador.Id,
          fecha: this.state.fecha_ingreso

        })
        .then(response => {
          console.log(response.data);
          if (response.data == "INCLUIDO_EN_PERIODO_ANTERIOR") {
            console.log('enreo INCLUIDO_EN_PERIODO_ANTERIOR');
            fechaValida = false;
            errors["fecha_ingreso"] = (
              <div className="alert alert-danger">
                La fecha ingresada ya se encuentra dentro de un período de contrato finalizado del colaborador
              </div>
            );
            this.setState({ errores: errors, formIsValid: fechaValida });
          } else if (response.data == "OK") {
            fechaValida = true;
            this.setState({ errores: errors, formIsValid: fechaValida });
          }

        })
        .catch(error => {
          console.log('entro cach')
          this.setState({ errores: errors, formIsValid: false });
          console.log(error);

        });
    }
    this.setState({ errores: errors, formIsValid: fechaValida });
  };

  GetCatalogos() {
    axios
      .post("/RRHH/Colaboradores/GetEmpresasApi/", {})
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
  }

  componentWillReceiveProps(nextProps) {
    // console.log('asdasfsf', nextProps.colaborador.fecha_registro_civil)
    this.setState({
      loading: true,
      id_empleado:
        nextProps.colaborador.empleado_id_sap != null
          ? nextProps.colaborador.empleado_id_sap
          : "",
      fecha_ingreso: moment(nextProps.colaborador.fecha_ingreso).format(
        "YYYY-MM-DD"
      ),
      tipo_identificacion:
        nextProps.colaborador.catalogo_tipo_identificacion_id,
      nro_identificacion: nextProps.colaborador.numero_identificacion,
      primer_apellido: nextProps.colaborador.primer_apellido,
      segundo_apellido:
        nextProps.colaborador.segundo_apellido != null
          ? nextProps.colaborador.segundo_apellido
          : "",
      nombres: nextProps.colaborador.nombres,
      fecha_nacimiento: moment(nextProps.colaborador.fecha_nacimiento).format(
        "YYYY-MM-DD"
      ),
      genero: nextProps.colaborador.catalogo_genero_id,
      nacionalidad: nextProps.colaborador.pais_pais_nacimiento_id,
      pais: nextProps.colaborador.PaisId,
      etnia: nextProps.colaborador.catalogo_etnia_id,
      estado_civil: nextProps.colaborador.catalogo_estado_civil_id,
      fecha_matrimonio:
        nextProps.colaborador.fecha_matrimonio == null ||
          nextProps.colaborador.fecha_matrimonio == ""
          ? ""
          : moment(nextProps.colaborador.fecha_matrimonio).format("YYYY-MM-DD"),
      discapacidad:
        nextProps.colaborador.discapacidad == null
          ? ""
          : nextProps.colaborador.discapacidad,
      tipo_dis:
        nextProps.colaborador.catalogo_tipo_discapacidad_id == null
          ? ""
          : nextProps.colaborador.catalogo_tipo_discapacidad_id,
      porcentaje_dis:
        nextProps.colaborador.catalogo_porcentaje_id == null
          ? ""
          : nextProps.colaborador.catalogo_porcentaje_id,
      codigo_siniestro:
        nextProps.colaborador.catalogo_codigo_siniestro_id != null &&
          nextProps.colaborador.discapacidad == true
          ? nextProps.colaborador.catalogo_codigo_siniestro_id
          : "",
      formacion_educativa:
        nextProps.colaborador.catalogo_formacion_educativa_id == null
          ? ""
          : nextProps.colaborador.catalogo_formacion_educativa_id,
      hijos: nextProps.colaborador.numero_hijos,
      formacion: nextProps.colaborador.formacion,
      institucion: nextProps.colaborador.institucion_educativa,
      titulo: nextProps.colaborador.catalogo_titulo_id,
      id_candidato: nextProps.colaborador.candidato_id_sap,
      empleado_id_sap_local: nextProps.colaborador.empleado_id_sap_local,
      codigo_incapacidad:
        nextProps.colaborador.catalogo_codigo_incapacidad_id != null &&
          nextProps.colaborador.discapacidad == true
          ? nextProps.colaborador.catalogo_codigo_incapacidad_id
          : "",
      nombres_apellidos:
        nextProps.colaborador.nombres_apellidos == null
          ? ""
          : nextProps.colaborador.nombres_apellidos,
      fecha_senecyt:
        nextProps.colaborador.fecha_registro_senecyt == null ||
          nextProps.colaborador.fecha_registro_senecyt == ""
          ? ""
          : moment(nextProps.colaborador.fecha_registro_senecyt).format(
            "YYYY-MM-DD"
          ),
      fecha_modificacion:
        nextProps.colaborador.fechaActualizacionFormat == null ||
          nextProps.colaborador.fechaActualizacionFormat == ""
          ? ""
          : moment(nextProps.colaborador.fechaActualizacionFormat).format(
            "YYYY-MM-DD"
          ),

      codigo_dactilar: nextProps.colaborador.codigo_dactilar,
      proyecto:
        nextProps.colaborador.ContratoId == 0
          ? ""
          : nextProps.colaborador.ContratoId,
      encargado_personal:
        nextProps.colaborador.catalogo_encargado_personal_id == null
          ? ""
          : nextProps.colaborador.catalogo_encargado_personal_id,
      destino:
        nextProps.colaborador.catalogo_destino_estancia_id == null
          ? ""
          : nextProps.colaborador.catalogo_destino_estancia_id,
      viene_registro_civil:
        nextProps.colaborador.viene_registro_civil == null
          ? false
          : nextProps.colaborador.viene_registro_civil,
      fecha_registro_civil:
        nextProps.colaborador.fecha_registro_civil == null
          ? ""
          : moment(nextProps.colaborador.fecha_registro_civil).format(
            "YYYY-MM-DD HH:mm:ss"
          ),
      es_sustituto:
        nextProps.colaborador.es_sustituto == null
          ? false
          : nextProps.colaborador.es_sustituto,
      fecha_sustituto_desde:
        nextProps.colaborador.fecha_sustituto_desde == null
          ? ""
          : moment(nextProps.colaborador.fecha_sustituto_desde).format(
            "YYYY-MM-DD"
          ),
      msg:
        nextProps.colaborador.viene_registro_civil != true
          ? ""
          : "Información de Registro Civil actualizada " +
          moment(nextProps.colaborador.fecha_registro_civil).format(
            "YYYY-MM-DD HH:mm:ss"
          ),
      empresa_id:
        nextProps.colaborador.empresa_id != null
          ? nextProps.colaborador.empresa_id
          : 0,
      loading: false,
    });
    this.saveEdad();
    if (
      (nextProps.colaborador.catalogo_estado_civil_id != null &&
        nextProps.colaborador.catalogo_estado_civil_id === 5447) ||
      nextProps.colaborador.fecha_matrimonio != null
    ) {
      this.setState({ visible_fecha_matrimonio: true });
    }
  }

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
      });
      axios
        .post("/RRHH/Colaboradores/GetInfoColaboradorWSApi", {
          tipoIdentificacion: this.state.tipo_identificacion,
          cedula: this.state.nro_identificacion,
          huella_dactilar: this.state.codigo_dactilar,
        })
        .then((response) => {
          // this.setState({ nacionalidades: response.data })
          if (response.data == "NO") {
            abp.notify.error("No se ha encontrado", "Error");
          } else {
            abp.notify.success(
              this.state.nro_identificacion + " Encontrado en base de datos",
              "Aviso"
            );
            this.procesarDatosBDD(response.data);
          }
          console.log("bdd", response.data);
        })
        .catch((error) => {
          console.log(error);
          this.setState({ loading: false });
        });
    } else {
      abp.notify.error("Complete los campos necesarios", "Error");
    }
  }

  getFotografia() {
    var origen = "CAR_ARC";

    this.setState({ loading: true });

    axios
      .post("/RRHH/Colaboradores/GetArchivoFotografiaApi/", {
        Idcolaborador: this.props.colaborador.Id,
        origen: origen,
      })
      .then((response) => {
        console.log(response);
        this.setState({ loading: false });
        if (response.data != "NO") {
          this.setState({ fotografia: response.data.hash });
        }
      })
      .catch((error) => {
        console.log(error);
        this.setState({ loading: false });
        this.warnMessage("Algo salio mal!");
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
      disable_fecha_matrimonio: data.fecha_matrimonio == null ? false : true,
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
      fecha_modificacion:
        data.fechaActualizacionFormat == null
          ? ""
          : moment(data.fechaActualizacionFormat).format("YYYY-MM-DD"),
      titulo: data.catalogo_titulo_id == null ? "" : data.catalogo_titulo_id,
      id_empleado: data.empleado_id_sap == 0 ? "" : data.empleado_id_sap,
      loading: false,
    });
    this.saveEdad();
  }

  consultarWSHuella() {
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
        disable_sexo: false,
      });

      axios
        .post("/RRHH/Colaboradores/ConsumirHuella/", {
          cedula: this.state.nro_identificacion,
          huella_dactilar: this.state.codigo_dactilar,
        })
        .then((response) => {
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
        .catch((error) => {
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
      var sexo = this.props.generos.filter((c) => c.codigo == GENERO_MUJER);
      this.setState({ genero: sexo[0].Id, disable_sexo: true });
    } else if (datos.Sexo == REGISTRO_CIVIL_HOMBRE) {
      var sexo = this.props.generos.filter((c) => c.codigo == GENERO_VARON);
      this.setState({ genero: sexo[0].Id, disable_sexo: true });
    } else {
      this.setState({
        sexo_sugerido: datos.Sexo,
        disable_sexo: false,
        genero: "",
      });
    }
    //Seleccionar el ESTADO CIVIL
    if (datos.EstadoCivil == REGISTRO_CIVIL_CASADO) {
      var fecha_m = moment(
        this.state.datosConsumo.FechaMatrimonio,
        "DD-MM-YYYY"
      );
      var estado = this.props.estados.filter(
        (c) => c.codigo == ESTADO_CIVIL_CASADO
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
          this.state.datosConsumo.FechaMatrimonio == "" ? false : true,
      });
    } else if (datos.EstadoCivil == REGISTRO_CIVIL_DIVORCIADO) {
      var estado = this.props.estados.filter(
        (c) => c.codigo == ESTADO_CIVIL_DIVORCIADO
      );
      this.setState({ estado_civil: estado[0].Id, disable_estado_civil: true });
    } else if (datos.EstadoCivil == REGISTRO_CIVIL_SOLTERO) {
      var estado = this.props.estados.filter(
        (c) => c.codigo == ESTADO_CIVIL_SOLTERO
      );
      this.setState({ estado_civil: estado[0].Id, disable_estado_civil: true });
    } else {
      this.setState({
        estado_civil_sugerido: datos.EstadoCivil,
        disable_estado_civil: false,
        estado_civil: "",
      });
    }

    //Seleccionar NACIONALIDAD
    if (datos.Nacionalidad == REGISTRO_CIVIL_ECUATORIANA) {
      var n = this.props.tiposNacionalidades.filter(
        (c) => c.codigo == NACIONALIDAD_ECUATORIANA
      );
      this.setState({ nacionalidad: n[0].Id, disable_nacionalidad: true });
    } else {
      this.setState({
        nacionalidad_sugerida: datos.Nacionalidad,
        disable_nacionalidad: false,
        nacionalidad: "",
      });
    }

    this.setState({
      nombres_apellidos: this.state.datosConsumo.Nombre,
      fecha_nacimiento: moment(fech_nac).format("YYYY-MM-DD"),
      fotografia: datos.Fotografia,
      viene_registro_civil: true,
      fecha_registro_civil: moment().format("YYYY-MM-DD HH:mm:ss"),
      loading: false,
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
      var catalogo = this.props.tiposIdentificacion.filter(
        (c) => c.Id == Number.parseInt(this.state.tipo_identificacion)
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
    } else if (catalogo != null && catalogo[0].codigo == "CEDULA") {
      console.log("lenght", this.state.nro_identificacion.length);
      var cedula_valida = this.props.validarcedulaEC(
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
        (c) => c.Id == Number.parseInt(this.state.tipo_identificacion)
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
    } else if (catalogo != null && catalogo[0].codigo == "CEDULA") {
      console.log("lenght", this.state.nro_identificacion.length);
      var cedula_valida = this.props.validarcedulaEC(
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
            <form onSubmit={this.handleSubmit}>
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
                    <label htmlFor="id_empleado">
                      <b>ID Empleado (SAP GLOBAL):</b>
                    </label>
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
                    <label htmlFor="fecha_ingreso">
                      <b>Fecha de Ingreso:</b>
                    </label>
                    <input
                      type="date"
                      id="fecha_ingreso"
                      className="form-control"
                      value={this.state.fecha_ingreso}
                      onChange={this.handleChangeFechaIngreso}
                      onBlur={this.validacionFechaActualizacionPeriodColaborador}
                      name="fecha_ingreso"
                      disabled={this.props.colaborador != null && this.props.colaborador.estado !== "ACTIVO" && this.props.colaborador.estado !== "INACTIVO" ? false : true}
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
                      {this.props.getFormSelectEncargadoPersonal()}
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
                      {this.props.getFormSelectDestinos()}
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
                      {this.props.getFormSelectProyecto()}
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
                      disabled
                      onChange={this.handleChangeIden}
                      className="form-control"
                      name="tipo_identificacion"
                      required
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
                    <label htmlFor="observacion">
                      * No. de Identificación:{" "}
                    </label>
                    <input
                      type="text"
                      id="nro_identificacion"
                      className="form-control"
                      disabled
                      value={this.state.nro_identificacion}
                      onChange={this.handleChangeUpperCase}
                      name="nro_identificacion"
                      required
                    />
                    {this.state.errores["nro_identificacion"]}
                    {this.state.errCons["nro_identificacion"]}
                  </div>
                </div>
              </div>
              {this.props.NombreTipoIdentificacion == "CÉDULA" && (
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
                    <div className="form-group" style={{ paddingTop: "8px" }}>
                      {/* <button type="button" className="btn btn-outline-primary" onClick={this.consultaBDD} style={{ marginTop: '6%' }}>BDD</button> */}
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
                    <label htmlFor="nombres_apellidos">
                      <b>Apellidos Nombres:</b>
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
                      {this.props.getFormSelectEtnia()}
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
                    <label htmlFor="observacion">
                      <b>Edad:</b>{" "}
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
                        : "hidden",
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
                      {this.props.getFormSelectFormacionEducativa()}
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
                      {this.props.getFormSelectFormacion()}
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
                      {this.props.getFormSelectTitulos()}
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
                  <div className="form-group">
                    <label htmlFor="fecha_mode">
                      Fecha Registro Mod:{" "}
                    </label>

                    <input
                      type="date"
                      id="fecha_mode"
                      className="form-control"
                      value={this.state.fecha_modificacion}
                      onChange={this.handleChange}
                      name="fecha_modificacion"
                      disabled

                    />


                  </div>
                </div>
                <div className="col">
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
                      this.state.discapacidad == true ? "visible" : "hidden",
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
                      {this.props.getFormSelectTipoDiscapacidad()}
                    </select>
                    {this.state.errores["tipo_dis"]}
                  </div>
                </div>
              </div>
              <div
                className="row"
                style={{
                  visibility:
                    this.state.discapacidad == true ? "visible" : "hidden",
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
                      {this.props.getFormSelectCodigoSiniestro()}
                    </select>
                    {this.state.errores["codigo_siniestro"]}
                  </div>
                </div>
              </div>
              <div
                className="row"
                style={{
                  visibility:
                    this.state.discapacidad == true ? "visible" : "hidden",
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
                      {this.props.getFormSelectCodigoIncapacidad()}
                    </select>
                    {this.state.errores["codigo_incapacidad"]}
                  </div>
                </div>
                <div className="col"></div>
              </div>

              <div
                className="row"
                style={{
                  marginTop: this.state.discapacidad == true ? "0px" : "-170px",
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
                      this.state.es_sustituto == true ? "visible" : "hidden",
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
                    onClick={() => this.Guardar()}
                    className="btn btn-outline-primary fa fa-save"
                    disabled={this.props.disable_button}
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
            </form>
          </div>
          <div className="col" style={{ display: "inline-block" }}>
            <div className="row">
              {this.state.fotografia == null
                ? ""
                : this.convertirimagen(this.state.fotografia)}
            </div>
            <div className="row">
              <label>{this.state.msg}</label>
            </div>
          </div>
          <Dialog
            header="Mensaje de Confirmación"
            visible={this.state.estado}
            footer={footer}
            width="350px"
            minY={70}
            onHide={this.cerrarConfirmacion}
            maximizable={true}
          >
            Está seguro de guardar la información registrada?. Se procederá a
            generar el número de legajo temporal del colaborador.
          </Dialog>

          <Dialog header="Modificación Fecha Ingreso" visible={this.state.viewdisable} style={{ width: '50vw' }} modal onHide={this.onHideview} >

            <div>Se requiere clave especial para realizar la modificación de la fecha de ingreso</div>
            <br />
            <Field
              name="pass"
              label="Código de Seguridad"
              required
              edit={true}
              value={this.state.pass}
              onChange={this.handleChange}


            /> <br />
            <div align="right">
              <Button label="SI" icon="pi pi-check" onClick={this.onDesactivar} />{" "}
              <Button label="NO" icon="pi pi-times" className="p-button-secondary" onClick={this.onHideview} />
            </div>
          </Dialog>
        </div>
      </BlockUi>
    );
  }

  abrirConfirmacion() {
    this.handleValidation();
    console.log(this.state.formIsValid);
    if (this.state.formIsValid == true && this.state.puedeContinuarActualizacion == true) {
      // this.setState({ estado: true });
      this.state.estado = true;
    }
  }

  cerrarConfirmacion() {
    this.setState({ estado: false });
  }

  handleValidation() {
    let errors = {};
    this.state.formIsValid = true;

    /* if (!this.state.empleado_id_sap_local) {
            this.state.formIsValid = false;
            errors["empleado_id_sap_local"] = <div className="alert alert-danger">El campo ID SAP Local es obligatorio</div>;
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
      /*var fecha = moment().format("YYYY-MM-DD");
      if (this.state.fecha_ingreso > fecha) {
        this.state.formIsValid = false;
        errors["fecha_ingreso"] = (
          <div className="alert alert-danger">
            La fecha ingresada no puede ser mayor a la fecha actual
          </div>
        );
      }*/
      var fecha = moment().format("YYYY-MM-DD");
      let fechaMoment = moment(fecha, "YYYY-MM-DD").add(10, 'days');
      let fechaFormat = moment(fechaMoment).format("YYYY-MM-DD")
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
            La fecha ingresada no puede ser mayor a 10 días posteriores a la fecha actual - Fecha Máxima: {fechaFormat}
          </div>
        );
      }

      axios
        .post("/RRHH/Colaboradores/GetValidarFechaPeriodoRegistrosAnteriores/", {
          Id: this.props.colaborador.Id,
          fecha: this.state.fecha_ingreso

        })
        .then(response => {
          console.log(response.data);
          if (response.data == "INCLUIDO_EN_PERIODO_ANTERIOR") {
            console.log('enreo INCLUIDO_EN_PERIODO_ANTERIOR');
            this.state.formIsValid = false;
            errors["fecha_ingreso"] = (
              <div className="alert alert-danger">
                La fecha ingresada ya se encuentra dentro de un período de contrato finalizado del colaborador
              </div>
            );
            this.setState({ errores: errors });
            return false;
          }

        })
        .catch(error => {
          console.log('entro cach')

          console.log(error);

        });






    }
    if (!this.state.tipo_identificacion) {
      this.state.formIsValid = false;
      errors["tipo_identificacion"] = (
        <div className="alert alert-danger">
          El campo Tipo de Identificación es obligatorio
        </div>
      );
    } else {
      var catalogo = this.props.tiposIdentificacion.filter(
        (c) => c.Id == Number.parseInt(this.state.tipo_identificacion)
      );
    }
    if (!this.state.nro_identificacion) {
      this.state.formIsValid = false;
      errors["nro_identificacion"] = (
        <div className="alert alert-danger">
          El campo No. de Identificación es obligatorio
        </div>
      );
    } else if (catalogo != null) {
      var cedula_valida = this.props.validarcedulaEC(
        this.state.nro_identificacion
      );
      console.log("xxx", cedula_valida);
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
        this.state.formIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            No. de Identificación es inválido
          </div>
        );
      } else if (
        catalogo[0].codigo == TIPO_IDENTIFICACION_PASAPORTE &&
        this.state.nro_identificacion.length > 22
      ) {
        this.state.formIsValid = false;
        errors["nro_identificacion"] = (
          <div className="alert alert-danger">
            No. de Identificación es inválido
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
    if (
      this.state.hijos != null &&
      this.state.hijos != "" &&
      this.state.hijos < 0
    ) {
      this.state.formIsValid = false;
      errors["hijos"] = (
        <div className="alert alert-danger">El Campo no puede ser negativo</div>
      );
    }

    //ES: Salir Validación Colaboradores.

    if (this.state.viene_registro_civil) {
      if (
        this.state.primer_apellido &&
        this.state.nombres &&
        this.state.nombres_apellidos
      ) {
        this.state.primer_apellido = this.state.primer_apellido;
        this.state.segundo_apellido = this.state.segundo_apellido;
        this.state.nombres = this.state.nombres;
        var completo =
          this.state.primer_apellido +
          this.state.segundo_apellido +
          this.state.nombres;
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
    } else {
      this.setState({
        nombres_apellidos:
          this.state.primer_apellido + " " + this.state.segundo_apellido,
      }) +
        " " +
        this.state.nombres;
    }

    /* ES: Validación */
    if (!this.state.nombres_apellidos) {
      if (
        this.state.primer_apellido &&
        this.state.nombres &&
        this.state.segundo_apellido
      ) {
        this.state.nombres_apellidos =
          this.state.primer_apellido +
          " " +
          this.state.segundo_apellido +
          " " +
          this.state.nombres;
      } else if (
        this.state.primer_apellido &&
        this.state.nombres &&
        this.state.segundo_apellido == ""
      ) {
        this.state.nombres_apellidos =
          this.state.primer_apellido + " " + this.state.nombres;
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
      var catEstadoCivil = this.props.estados.filter(
        (c) => c.Id == Number.parseInt(this.state.estado_civil)
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
      var ep = this.props.tiposEncargadoPersonal.filter(
        (c) => c.Id == Number.parseInt(this.state.encargado_personal)
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
    console.log('FEchaProps',this.props.colaborador.fecha_ingreso);
    let fechaIngresoFormat = moment(this.state.fecha_ingreso).format("YYYY-MM-DD");
    let fechaIngresoColaborador =moment(this.props.colaborador.fecha_ingreso).format("YYYY-MM-DD");
    console.log('fechaIngresoFormat',fechaIngresoFormat);
    console.log('fechaIngresoColaborador',fechaIngresoColaborador);
    if (fechaIngresoFormat!= fechaIngresoColaborador && this.state.puedeContinuarActualizacion === false) {
      this.state.formIsValid = false;
      this.setState({ viewdisable: true,mostrarMensajeActualizacionFecha:true});

    } else {
      this.setState({ puedeContinuarActualizacion: true });
    }

    this.setState({ errores: errors });

    return this.state.formIsValid;
  }

  Guardar() {
    this.handleValidation();

    if (this.state.formIsValid == true) {
      this.setState({ loading: true });
       this.props.colaborador.fecha_ingreso = this.state.fecha_ingreso;
      this.props.colaborador.catalogo_tipo_identificacion_id = this.state.tipo_identificacion;
      this.props.colaborador.numero_identificacion = this.state.nro_identificacion;
      this.props.colaborador.primer_apellido = this.state.primer_apellido;
      this.props.colaborador.segundo_apellido = this.state.segundo_apellido;
      this.props.colaborador.nombres = this.state.nombres;
      this.props.colaborador.fecha_nacimiento = this.state.fecha_nacimiento;
      this.props.colaborador.catalogo_genero_id = this.state.genero;
      this.props.colaborador.PaisId = this.state.pais;
      this.props.colaborador.pais_pais_nacimiento_id = this.state.nacionalidad;
      this.props.colaborador.catalogo_etnia_id = this.state.etnia;
      this.props.colaborador.catalogo_estado_civil_id = this.state.estado_civil;
      this.props.colaborador.fecha_matrimonio =
        this.state.fecha_matrimonio == "Invalid date" ||
          this.state.fecha_matrimonio == ""
          ? null
          : this.state.fecha_matrimonio;
      this.props.colaborador.discapacidad = this.state.discapacidad;
      this.props.colaborador.catalogo_tipo_discapacidad_id = this.state.tipo_dis;
      this.props.colaborador.catalogo_porcentaje_id = this.state.porcentaje_dis;
      this.props.colaborador.catalogo_codigo_siniestro_id = this.state.codigo_siniestro;
      this.props.colaborador.catalogo_formacion_educativa_id = this.state.formacion_educativa;
      this.props.colaborador.numero_hijos = this.state.hijos;
      this.props.colaborador.formacion = this.state.formacion;
      this.props.colaborador.institucion_educativa = this.state.institucion;
      this.props.colaborador.catalogo_titulo_id = this.state.titulo;
      this.props.colaborador.candidato_id_sap = this.state.id_candidato;
      this.props.colaborador.empleado_id_sap_local = parseInt(
        "" + this.state.empleado_id_sap_local
      );
      this.props.colaborador.catalogo_codigo_incapacidad_id = this.state.codigo_incapacidad;
      this.props.colaborador.nombres_apellidos = this.state.nombres_apellidos;
      this.props.colaborador.fecha_registro_senecyt =
        this.state.fecha_senecyt == "Invalid date" ||
          this.state.fecha_senecyt == ""
          ? null
          : this.state.fecha_senecyt;
      this.props.colaborador.codigo_dactilar = this.state.codigo_dactilar;
      this.props.colaborador.catalogo_encargado_personal_id = this.state.encargado_personal;
      this.props.colaborador.catalogo_destino_estancia_id = this.state.destino;
      this.props.colaborador.ContratoId = this.state.proyecto;
      this.props.colaborador.viene_registro_civil = this.state.viene_registro_civil;
      this.props.colaborador.fecha_registro_civil =
        this.state.fecha_registro_civil == ""
          ? null
          : this.state.fecha_registro_civil;
      this.props.colaborador.es_sustituto = this.state.es_sustituto;
      if (this.state.es_sustituto == true) {
        this.props.colaborador.fecha_sustituto_desde =
          this.state.fecha_sustituto_desde == "Invalid date"
            ? null
            : this.state.fecha_sustituto_desde;
      } else {
        this.props.colaborador.fecha_sustituto_desde = null;
      }
      console.log("this.props.Colaborador", this.props.colaborador);

      axios
        .post("/RRHH/Colaboradores/CreateEmpleoApi/", {
          colaborador: this.props.colaborador,
          empleado_id_sap_local: parseInt(
            "" + this.state.empleado_id_sap_local
          ),
        })
        .then((response) => {
          this.cerrarConfirmacion();
          this.setState({ loading: false,puedeContinuarActualizacion:false });
          abp.notify.success("Colaborador Guardado!", "Aviso");
        //  this.props.ConsultaColaborador();
          this.props.ChangeTab();
          this.props.Siguiente(1);
          this.props.CargaCategoriaEncargado(this.state.encargado_personal);
          this.props.CargaNomina(this.state.encargado_personal);

        })
        .catch((error) => {
          this.setState({ loading: false });
          console.log(error);
          abp.notify.error("Algo salió mal", "Error");
        });
    } else {
        if(this.state.mostrarMensajeActualizacionFecha===true){
        }else{
          abp.notify.error(
            "Se ha encontrado errores, por favor revisar el formulario",
            "Error"
          );
        }
      
    }
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
      codigo_dactilar: "",
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
    });
  }

  handleChangeEstadoCivil(event) {
    this.setState({ [event.target.name]: event.target.value });
    var estado = this.props.estados.filter(
      (c) => c.Id == Number.parseInt(event.target.value)
    );
    // console.log(estado)
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
      [name]: value,
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
      return word.replace(/\w\S*/g, function (txt) {
        return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
      });
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
}
