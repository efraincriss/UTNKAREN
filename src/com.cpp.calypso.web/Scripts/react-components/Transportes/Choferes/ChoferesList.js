import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import Field from "../../Base/Field-v2";
import wrapForm from "../../Base/BaseWrapper";
import { ScrollPanel } from 'primereact-v2/scrollpanel';
import moment from 'moment';
import {
  TIPO_IDENTIFICACION,
  GENERO,
  ESTADOCIVIL
} from "../../Base/Constantes";
import {
  FRASE_ERROR_SELECCIONAR_IDENTIFICACION,
  FRASE_ERROR_SELECCIONAR_TIPO_IDENTIFICACION,
  FRASE_ERROR_SELECCIONAR_PROVEEDOR,
  FRASE_ERROR_SELECCIONAR_ESTADOCIVIL,
  FRASE_ERROR_CELULAR,
  FRASE_ERROR_SELECCIONAR_GENERO,
  FRASE_ERROR_NOMBRES,
  FRASE_ERROR_APELLIDOS,
  FRASE_ERROR_DIGITOSCEDULA,
  FRASE_ERROR_DIGITOSCELULAR,
  FRASE_ERROR_DIGITOSNOMBRES,
  FRASE_ERROR_DIGITOSAPELLIDOS,
  FRASE_ERROR_DIGITOSCORREO
} from "../../Base/Strings";

import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import { Button } from "primereact-v2/button";
import validationRules from '../../Base/validationRules';
class ChoferesList extends React.Component {
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
      TipoIdentificaciones: [],
      TipoIdentificacionId: 0,
      TipoIdentificacionName: "",
      Identificacion: "",
      ApellidosNombres: "",
      Nombres: "",
      Apellidos: "",
      Mail: "",

      ProveedoresTransporte: [],
      ProveedorId: 0,
      Generos: [],
      GeneroId: 0,

      Celular: "",
      FechaNacimiento: null,
      Estado: "Activo",
      Estados: [{ label: "ACTIVO", value: 1 }, { label: "INACTIVO", value: 0 }],
      EstadoId: 0,
      ChoferSeleccionado: null,
      e: null, //encontrado

      danger: '',
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleChangeCorreo = this.handleChangeCorreo.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
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
    this.ObtenerChoferes = this.ObtenerChoferes.bind(this);
    this.ObtenerDetalleChofer = this.ObtenerDetalleChofer.bind(this);
    this.ObtenerCatalogos = this.ObtenerCatalogos.bind(this);
    this.ObtenerChoferIdentificacion = this.ObtenerChoferIdentificacion.bind(this);
    this.generarBotones = this.generarBotones.bind(this);
    this.VaciarCampos = this.VaciarCampos.bind(this);
    this.EliminarChofer = this.EliminarChofer.bind(this);
    this.validarcedula = this.validarcedula.bind(this);
  }

  componentDidMount() {
    this.props.blockScreen();
    this.ObtenerChoferes();
    this.ObtenerCatalogos();
    this.props.unlockScreen();
  }




  VaciarCampos() {
    this.setState({
      TipoIdentificacionId: 0,
      Identificacion: "",
      ApellidosNombres: "",
      Nombres: "",
      Apellidos: "",
      Mail: "",
      ProveedorId: 0,
      GeneroId: 0,
      Celular: "",
      FechaNacimiento: null,
      Estado: "Activo",
      errors: {},
      danger: ''
    });
  }
  ObtenerChoferes() {
    this.Loading();
    axios
      .post("/Transporte/Chofer/ListaChoferes", {})
      .then(response => {
        console.log(response.data.result);

        this.setState({ data: response.data.result, blocking: false });
        this.StopLoading();
      })
      .catch(error => {
        console.log(error);
        this.StopLoading();
      });
  }
  isValid() {
    const errors = {};
    if (this.state.TipoIdentificacionId == 0) {
      errors.TipoIdentificacionId = 'Campo requerido';
    }
    if (this.state.Identificacion == 0) {
      this.setState({ danger: "alert alert-danger" });
      errors.Identificacion = 'Campo requerido';
    }
    if (this.state.TipoIdentificacionName.length > 0 && this.state.TipoIdentificacionName == "CÉDULA"/*this.state.TipoIdentificacionId == 6422*/ && this.state.Identificacion != null
      && this.state.Identificacion.length > 10) {
      this.setState({ danger: "alert alert-danger" })
      errors.Identificacion = 'La cédula debe tener 10 dígitos';
    }
    if (this.state.TipoIdentificacionName.length > 0 && this.state.TipoIdentificacionName == "PASAPORTE" && this.state.Identificacion != null
      && this.state.Identificacion.length > 30) {
      this.setState({ danger: "alert alert-danger" })
      errors.Identificacion = 'El pasaporte debe tener máximo 30 caracteres alfánumericos';
    }
    if (this.state.Nombres.length == 0) {
      errors.Nombres = 'Campo requerido';
    }
    if (this.state.ProveedorId == 0) {
      errors.ProveedorId = 'Campo requerido';
    }
    if (this.state.Apellidos.length == 0) {
      errors.Apellidos = 'Campo requerido';
    }
    if (this.state.GeneroId == 0) {
      errors.GeneroId = 'Campo requerido';
    }
    if (this.state.Celular.length == 0) {
      errors.Celular = 'Campo requerido';
    }
    if (this.state.FechaNacimiento == null) {
      errors.FechaNacimiento = 'Campo requerido';
    }
    if (this.state.FechaNacimiento != null && moment(new Date()).diff(moment(this.state.FechaNacimiento), 'year') < 18) {
      errors.FechaNacimiento = 'La Edad debe ser mayor a 18 años';
    }
    if (this.state.Mail != null && this.state.Mail.length > 0) {
      const formatemail = validationRules["isEmail"]([], this.state.Mail);
      if (!formatemail) {
        errors.Mail = 'No es una dirección de correo válida';
      }

    }


    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }

  validarcedula() {
    var cedula = this.state.Identificacion;

    //Preguntamos si la cedula consta de 10 digitos
    if (cedula.length == 10) {

      //Obtenemos el digito de la region que sonlos dos primeros digitos
      var digito_region = cedula.substring(0, 2);

      //Pregunto si la region existe ecuador se divide en 24 regiones
      if (digito_region >= 1 && digito_region <= 24) {

        // Extraigo el ultimo digito
        var ultimo_digito = cedula.substring(9, 10);

        //Agrupo todos los pares y los sumo
        var pares = parseInt(cedula.substring(1, 2)) + parseInt(cedula.substring(3, 4)) + parseInt(cedula.substring(5, 6)) + parseInt(cedula.substring(7, 8));

        //Agrupo los impares, los multiplico por un factor de 2, si la resultante es > que 9 le restamos el 9 a la resultante
        var numero1 = cedula.substring(0, 1);
        var numero1 = (numero1 * 2);
        if (numero1 > 9) { var numero1 = (numero1 - 9); }

        var numero3 = cedula.substring(2, 3);
        var numero3 = (numero3 * 2);
        if (numero3 > 9) { var numero3 = (numero3 - 9); }

        var numero5 = cedula.substring(4, 5);
        var numero5 = (numero5 * 2);
        if (numero5 > 9) { var numero5 = (numero5 - 9); }

        var numero7 = cedula.substring(6, 7);
        var numero7 = (numero7 * 2);
        if (numero7 > 9) { var numero7 = (numero7 - 9); }

        var numero9 = cedula.substring(8, 9);
        var numero9 = (numero9 * 2);
        if (numero9 > 9) { var numero9 = (numero9 - 9); }

        var impares = numero1 + numero3 + numero5 + numero7 + numero9;

        //Suma total
        var suma_total = (pares + impares);

        //extraemos el primero digito
        var primer_digito_suma = String(suma_total).substring(0, 1);

        //Obtenemos la decena inmediata
        var decena = (parseInt(primer_digito_suma) + 1) * 10;

        //Obtenemos la resta de la decena inmediata - la suma_total esto nos da el digito validador
        var digito_validador = decena - suma_total;

        //Si el digito validador es = a 10 toma el valor de 0
        if (digito_validador == 10)
          var digito_validador = 0;

        //Validamos que el digito validador sea igual al de la cedula
        if (digito_validador == ultimo_digito) {
          // this.props.showSuccess('la cédula:' + cedula + ' es correcta');
          return true;
        } else {
          this.props.showWarn('la cédula:' + cedula + ' es incorrecta');
          return false;
        }

      } else {
        // imprimimos en consola si la region no pertenece
        this.props.showWarn('Esta cédula no pertenece a ninguna región');
        return false;
      }
    } else {
      //imprimimos en consola si la cedula tiene mas o menos de 10 digitos
      this.props.showWarn('Esta cédula debe tener 10 Dígitos');
      return false;
    }
  }

  ObtenerChoferIdentificacion() {
    if (this.state.TipoIdentificacionName.length > 0 && this.state.TipoIdentificacionName !== "CÉDULA" /*this.state.TipoIdentificacionId != 6422*/) {
      axios
        .post("/Transporte/Chofer/ObtenerChoferCedula", {
          cedula: this.state.Identificacion
        })
        .then(response => {
          if (response.data != "Error") {
            this.setState({
              e: response.data,
              TipoIdentificacionId: response.data.TipoIdentificacionId,
              Nombres: response.data.Nombres,
              Apellidos: response.data.Apellidos,
              ApellidosNombres: response.data.ApellidosNombres,
              FechaNacimiento: response.data.FechaNacimiento,
              Celular: response.data.Celular,
              Mail: response.data.Mail,
              GeneroId: response.data.GeneroId,
              blocking: false
            });
          }
        })
        .catch(error => {
          console.log(error);
        });
    }

    if (this.state.TipoIdentificacionName.length > 0 && this.state.TipoIdentificacionName == "CÉDULA" /*this.state.TipoIdentificacionId == 6422*/) {
      if (this.validarcedula()) {

        axios
          .post("/Transporte/Chofer/ObtenerChoferCedula", {
            cedula: this.state.Identificacion
          })
          .then(response => {
            if (response.data != "Error") {
              this.setState({
                e: response.data,
                TipoIdentificacionId: response.data.TipoIdentificacionId,
                Nombres: response.data.Nombres,
                Apellidos: response.data.Apellidos,
                ApellidosNombres: response.data.ApellidosNombres,
                FechaNacimiento: response.data.FechaNacimiento,
                Celular: response.data.Celular,
                Mail: response.data.Mail,
                GeneroId: response.data.GeneroId,
                blocking: false
              });
              this.props.showSuccess("Información  encontrada");
            } else {
              this.props.showWarn("Información no encontrada");

            }

            console.log(response.data);
            this.StopLoading();
          })
          .catch(error => {
            console.log(error);
          });
      }
    }
  }
  ObtenerDetalleChofer(Id) {
    console.log(Id);

    axios
      .post("/Transporte/Chofer/ObtenerDetallesChofer", {
        Id: Id
      })
      .then(response => {
        if (response.data != "Error") {
          this.setState({
            e: response.data,
            Identificador: Id,
            TipoIdentificacionId: response.data.TipoIdentificacionId,
            Identificacion: response.data.NumeroIdentificacion,
            Nombres: response.data.Nombres,
            Apellidos: response.data.Apellidos,
            ApellidosNombres: response.data.ApellidosNombres,
            FechaNacimiento: response.data.FechaNacimiento,
            Celular: response.data.Celular,
            Mail: response.data.Mail,
            GeneroId: response.data.GeneroId,
            ProveedorId: response.data.ProveedorId,

            EstadoId: response.data.Estado,
            blocking: false
          });
        }
      })
      .catch(error => {
        console.log(error);
      });
  }

  ObtenerCatalogos() {
    this.Loading();
    //TIPO IDENTIFICACION
    axios
      .get("/Proyecto/Catalogo/GetByCodeApi/?code=TIPOINDENTIFICACION", {})
      .then(response => {
        var items = response.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ TipoIdentificaciones: items, blocking: false });
      })
      .catch(error => {
        console.log(error);
      });

    axios
      .get("/proyecto/catalogo/GetByCodeApi/?code=GENERO", {})
      .then(response => {
        var items = response.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Generos: items, blocking: false });
      })
      .catch(error => {
        console.log(error);
      });
    axios
      .post("/Transporte/Chofer/ListaProveedoresTransporte", {})
      .then(response => {
        console.log(response.data.result);
        var items = response.data.result.map(item => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });
        this.setState({ ProveedoresTransporte: items, blocking: false });
      })
      .catch(error => {
        console.log(error);
      });

    this.StopLoading();
  }
  onChangeValue = (name, value) => {
    console.log(name)

    if (name == "TipoIdentificacionId") {

      axios
        .get("/proyecto/catalogo/GetNamebyId/" + value, {})
        .then(response => {

          this.setState({ TipoIdentificacionName: response.data });
          console.log(this.state.TipoIdentificacionName)
        })
        .catch(error => {
          console.log(error);
        });

    }
    this.setState({
      [name]: value
    });

  }
  EliminarChofer(Id) {
    console.log(Id);
    axios
      .post("/Transporte/Chofer/Delete", {
        Id: Id
      })
      .then(response => {
        if (response.data != "Error") {
          this.infoMessage("Eliminado Correctamente");
          this.OcultarFormularioEditar();
          this.ObtenerChoferes();
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

        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormularioEditar(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar Conductor"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar Conductor"
          onClick={() => { if (window.confirm(`Esta acción eliminará el registro, ¿Desea continuar?`)) this.EliminarChofer(row.Id); }}
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
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
              Nº
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="NombreProveedor"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Proveedor
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="NombreTipoIdentificacion"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Tipo Identificación
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="NumeroIdentificacion"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              No. Identificación
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="ApellidosNombres"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Apellidos Nombres
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="NombreEstado"
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Estado
            </TableHeaderColumn>

            <TableHeaderColumn
              isKey
              dataField="Id"
              width={"15%"}
              dataFormat={this.generarBotones}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            >  Opciones
            </TableHeaderColumn>
          </BootstrapTable>
        </div>

        <Dialog
          header="Nuevo Conductor"
          visible={this.state.visible}
          width="720px"
          onHide={this.OcultarFormulario}
          resizable={true}
          responsive={true}

          blockScroll={true}>
          <ScrollPanel style={{ width: '700px', height: '530px' }}>
            <div>
              <form onSubmit={this.EnviarFormulario}  >
                <div className="row">
                  <div className="col">
                    <Field
                      name="TipoIdentificacionId"
                      required
                      value={this.state.TipoIdentificacionId}
                      label="Tipo de Identificación"
                      options={this.state.TipoIdentificaciones}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValue}
                      error={this.state.errors.TipoIdentificacionId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />

                    <Field
                      name="Apellidos"
                      label="Apellidos"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.Apellidos}
                      onChange={this.handleChange}
                      style={{ textTransform: 'uppercase' }}
                      error={this.state.errors.Apellidos}
                    />

                    <Field
                      name="GeneroId"
                      required
                      value={this.state.GeneroId}
                      label="Género"
                      options={this.state.Generos}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValue}
                      error={this.state.errors.GeneroId}
                      readOnly={false}
                      filter={true}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                    <Field
                      name="Celular"
                      label="Celular"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.Celular}
                      onChange={this.handleChange}
                      error={this.state.errors.Celular}
                    />
                    <Field
                      name="Estado"
                      label="Estado"
                      edit={false}
                      readOnly={true}
                      value={this.state.Estado}
                      onChange={this.handleChange}
                      error={this.state.errors.Estado}
                    />



                  </div>

                  <div className="col">


                    <label className="col-sm-12 col-form-label">
                      * No. de Identificación
                    </label>
                    <div className="p-inputgroup">
                      <input
                        type="text"
                        name="Identificacion"
                        className="form-control"
                        onChange={this.handleChange}
                        // onBlur={this.ObtenerChoferIdentificacion}
                        value={this.state.Identificacion}
                      />
                      <Button type="button" icon="pi pi-search" className="p-button-info" onClick={() => this.ObtenerChoferIdentificacion()} tooltip="Buscar"

                      />
                    </div>

                    <div className={this.state.danger} style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '17px', fontSize: '10px' }}>{this.state.errors.Identificacion}</div>


                    <Field
                      name="Nombres"
                      label="Nombres"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.Nombres}
                      onChange={this.handleChange}
                      style={{ textTransform: 'uppercase' }}
                      error={this.state.errors.Nombres}
                    />
                    <Field
                      name="FechaNacimiento"
                      label="Fecha Nacimiento"
                      required
                      type="date"
                      edit={true}
                      readOnly={false}
                      value={this.state.FechaNacimiento}
                      onChange={this.handleChange}
                      error={this.state.errors.FechaNacimiento}
                    />

                    <Field
                      name="Mail"
                      label="Correo Electrónico"
                      edit={true}
                      readOnly={false}
                      value={this.state.Mail}
                      onChange={this.handleChangeCorreo}
                      error={this.state.errors.Mail}
                    />
                    <Field
                      name="ProveedorId"
                      required
                      value={this.state.ProveedorId}
                      label="Proveedor"
                      options={this.state.ProveedoresTransporte}
                      type={"select"}
                      onChange={this.onChangeValue}
                      error={this.state.errors.ProveedorId}
                      readOnly={false}
                      filter={true}
                      showClear={true}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
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
          </ScrollPanel>
        </Dialog>
        <Dialog
          header="Editar Conductor"
          visible={this.state.visibleeditar}
          width="720px"
          modal={true}
          onHide={this.OcultarFormularioEditar}
          resizable={true}
          responsive={true}

          blockScroll={true}
        >
          <ScrollPanel style={{ width: '700px', height: '530px' }}>
            <div>
              <form onSubmit={this.EnviarFormularioEditar}>
                <div className="row">
                  <div className="col">
                    <Field
                      name="TipoIdentificacionId"
                      required
                      value={this.state.TipoIdentificacionId}
                      label="Tipo de Identificación"
                      options={this.state.TipoIdentificaciones}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValue}
                      error={this.state.errors.TipoIdentificacionId}
                      readOnly={true}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                    <Field
                      name="Apellidos"
                      label="Apellidos"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.Apellidos}
                      style={{ textTransform: 'uppercase' }}
                      onChange={this.handleChange}
                      error={this.state.errors.Apellidos}
                    />


                    <Field
                      name="GeneroId"
                      required
                      value={this.state.GeneroId}
                      label="Género"
                      options={this.state.Generos}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValue}
                      error={this.state.errors.GeneroId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                    <Field
                      name="Celular"
                      label="Celular"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.Celular}
                      onChange={this.handleChange}
                      error={this.state.errors.Celular}
                    />
                    <Field
                      name="EstadoId"
                      required
                      value={this.state.EstadoId}
                      label="Estado"
                      options={this.state.Estados}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValue}
                      error={this.state.errors.EstadoId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Selecione.."
                    />
                  </div>

                  <div className="col">
                    <Field
                      name="Identificacion"
                      label="No. de Identificación"
                      required
                      edit={true}
                      readOnly={true}
                      value={this.state.Identificacion}
                      onChange={this.handleChange}
                      error={this.state.errors.Identificacion}
                    />
                    <Field
                      name="Nombres"
                      label="Nombres"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.Nombres}
                      onChange={this.handleChange}
                      style={{ textTransform: 'uppercase' }}
                      error={this.state.errors.Nombres}
                    />


                    <Field
                      name="FechaNacimiento"
                      label="Fecha Nacimiento"
                      required
                      type="date"
                      edit={true}
                      readOnly={false}
                      value={this.state.FechaNacimiento}
                      onChange={this.handleChange}
                      error={this.state.errors.FechaNacimiento}
                    />
                    <Field
                      name="Mail"
                      label="Correo Electrónico"
                      edit={true}
                      readOnly={false}
                      value={this.state.Mail}
                      onChange={this.handleChangeCorreo}
                      error={this.state.errors.Mail}
                    />


                    <Field
                      name="ProveedorId"
                      required
                      value={this.state.ProveedorId}
                      label="Proveedor"
                      options={this.state.ProveedoresTransporte}
                      type={"select"}
                      onChange={this.onChangeValue}
                      error={this.state.errors.ProveedorId}
                      readOnly={false}
                      filter={true}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
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
            </div></ScrollPanel>
        </Dialog>
      </BlockUi>
    );
  }

  EnviarFormulario(event) {
    this.setState({ danger: ' ' })
    event.preventDefault();
    if (!this.isValid()) {
      abp.notify.error("No ha ingresado los campos obligatorios  o existen datos inválidos.", 'Validación');
      return;
    } else if (this.state.Nombres.length > 50) {
      this.setState({ danger: ' ' })
      this.props.showWarn(FRASE_ERROR_DIGITOSNOMBRES);
    } else if (this.state.TipoIdentificacionName.length > 0 && this.state.TipoIdentificacionName == "CÉDULA"/*this.state.TipoIdentificacionId == 6422*/ && this.state.Identificacion != null
      && this.state.Identificacion.length > 10) {
      this.setState({ danger: ' ' })
      this.props.showWarn("La cédula debe tener 10 dígitos");
    }
    else if (this.state.TipoIdentificacionName.length > 0 && this.state.TipoIdentificacionName == "PASAPORTE" && this.state.Identificacion != null
      && this.state.Identificacion.length > 30) {
      this.setState({ danger: ' ' })
      this.props.showWarn("El pasaporte debe tener máximo 30 caracteres alfanuméricos");
    }


    else if (this.state.Apellidos.length > 50) {
      this.props.showWarn(FRASE_ERROR_DIGITOSAPELLIDOS);
      this.setState({ danger: ' ' })
    } else if (this.state.Celular.length > 20) {
      this.props.showWarn(FRASE_ERROR_DIGITOSCELULAR);
      this.setState({ danger: ' ' })
    } else if (this.state.Mail != null && this.state.Mail.length > 100) {
      this.props.showWarn(FRASE_ERROR_DIGITOSCORREO);
      this.setState({ danger: ' ' })
    } else {
      if (this.state.TipoIdentificacionName.length > 0 && this.state.TipoIdentificacionName == "CÉDULA") {
        this.setState({ danger: ' ' })
        if (this.validarcedula()) {
          this.setState({ danger: "" })
          axios
            .post("/Transporte/Chofer/Create", {
              Id: 0,
              ProveedorId: this.state.ProveedorId,
              TipoIdentificacionId: this.state.TipoIdentificacionId,
              NumeroIdentificacion: this.state.Identificacion,
              ApellidosNombres: this.state.Apellidos.toUpperCase() + " " + this.state.Nombres.toUpperCase(),
              Nombres: this.state.Nombres.toUpperCase(),
              Apellidos: this.state.Apellidos.toUpperCase(),
              GeneroId: this.state.GeneroId,
              FechaNacimiento: this.state.FechaNacimiento,
              Celular: this.state.Celular,
              Mail: this.state.Mail,
              Estado: this.state.Estado,
              FechaEstado: new Date()
            })
            .then(response => {
              if (response.data == "OK") {
                this.props.showSuccess("Conductor Creado");
                this.setState({ visible: false });
                this.ObtenerChoferes();
                this.StopLoading();
                this.VaciarCampos();
              } else if (response.data == "EXISTE") {
                this.props.showWarn(
                  "Ya existe un Conductor con el mismo Tipo de Identificación y No. de Identificación"
                );
                this.StopLoading();
              } else {
                this.props.showWarn("Ocurrió un Error");
                this.StopLoading();
              }
            })
            .catch(error => {
              console.log(error);
              this.props.showWarn("Ocurrió un Error");
              this.StopLoading();
            });

        }

      } else {

        this.setState({ danger: "" })
        axios
          .post("/Transporte/Chofer/Create", {
            Id: 0,
            ProveedorId: this.state.ProveedorId,
            TipoIdentificacionId: this.state.TipoIdentificacionId,
            NumeroIdentificacion: this.state.Identificacion,
            ApellidosNombres: this.state.Nombres.toUpperCase() + " " + this.state.Apellidos.toUpperCase(),
            Nombres: this.state.Nombres.toUpperCase(),
            Apellidos: this.state.Apellidos.toUpperCase(),
            GeneroId: this.state.GeneroId,
            FechaNacimiento: this.state.FechaNacimiento,
            Celular: this.state.Celular,
            Mail: this.state.Mail,
            Estado: this.state.Estado,
            FechaEstado: new Date()
          })
          .then(response => {
            if (response.data == "OK") {
              this.props.showSuccess("Coductor Creado");
              this.setState({ visible: false });
              this.ObtenerChoferes();
              this.StopLoading();
              this.VaciarCampos();
            } else if (response.data == "EXISTE") {
              this.props.showWarn(
                "Ya existe un Conductor con el mismo Tipo de Identificación y No. de Identificación"
              );
              this.StopLoading();
            } else {
              this.props.showWarn("Ocurrió un Error");
              this.StopLoading();
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

  EnviarFormularioEditar(event) {
    this.setState({ danger: ' ' })
    event.preventDefault();

    if (!this.isValid()) {
      abp.notify.error("No ha ingresado los campos obligatorios  o existen datos inválidos.", 'Validación');
      return;
    } else if (this.state.Nombres.length > 50) {

      this.props.showWarn(FRASE_ERROR_DIGITOSNOMBRES);
    } else if (this.state.TipoIdentificacionName.length > 0 && this.state.TipoIdentificacionName == "CÉDULA" && this.state.Identificacion != null
      && this.state.Identificacion.length > 10) {

      this.props.showWarn("La cédula debe tener 10 dígitos");
    }
    else if (this.state.TipoIdentificacionName.length > 0 && this.state.TipoIdentificacionName == "PASAPORTE" && this.state.Identificacion != null
      && this.state.Identificacion.length > 30) {

      this.props.showWarn("El pasaporte debe tener máximo 30 dígitos");
    }


    else if (this.state.Apellidos.length > 50) {
      this.props.showWarn(FRASE_ERROR_DIGITOSAPELLIDOS);
    } else if (this.state.Celular.length > 20) {
      this.props.showWarn(FRASE_ERROR_DIGITOSCELULAR);
    } else if (this.state.Mail != null && this.state.Mail.length > 100) {
      this.props.showWarn(FRASE_ERROR_DIGITOSCORREO);
    } else if (this.state.Mail != null && this.state.Mail.length > 100) {
      this.props.showWarn(FRASE_ERROR_DIGITOSCORREO);
    }

    else {

      if (this.state.TipoIdentificacionName.length > 0 && this.state.TipoIdentificacionName == "CÉDULA") {
        if (this.validarcedula()) {
          this.setState({ danger: "" })
          axios
            .post("/Transporte/Chofer/Edit", {
              Id: this.state.Identificador,
              ProveedorId: this.state.ProveedorId,
              TipoIdentificacionId: this.state.TipoIdentificacionId,
              NumeroIdentificacion: this.state.Identificacion,
              ApellidosNombres: this.state.Apellidos.toUpperCase() + " " + this.state.Nombres.toUpperCase(),
              Nombres: this.state.Nombres.toUpperCase(),
              Apellidos: this.state.Apellidos.toUpperCase(),
              GeneroId: this.state.GeneroId,
              FechaNacimiento: this.state.FechaNacimiento,
              Celular: this.state.Celular,
              Mail: this.state.Mail,
              Estado: this.state.EstadoId,
              FechaEstado: new Date()
            })
            .then(response => {
              if (response.data == "OK") {
                this.props.showSuccess("Conductor Editado");
                this.setState({ visible: false });
                this.ObtenerChoferes();
                this.StopLoading();
                this.VaciarCampos();
                this.OcultarFormularioEditar()
              } else if (response.data == "EXISTE") {
                this.props.showWarn(
                  "Ya existe un Conductor con la misma identificacion y asignado al mismo proveedor"
                );

                this.StopLoading();
              } else {
                this.props.showWarn("Ocurrió un Error");
                this.StopLoading();
              }
            })
            .catch(error => {
              console.log(error);
              this.props.showWarn("Ocurrió un Error");
              this.StopLoading();
            });

        }

      } else {

        axios
          .post("/Transporte/Chofer/Edit", {
            Id: this.state.Identificador,
            ProveedorId: this.state.ProveedorId,
            TipoIdentificacionId: this.state.TipoIdentificacionId,
            NumeroIdentificacion: this.state.Identificacion,
            ApellidosNombres: this.state.Nombres.toUpperCase() + " " + this.state.Apellidos.toUpperCase(),
            Nombres: this.state.Nombres.toUpperCase(),
            Apellidos: this.state.Apellidos.toUpperCase(),
            GeneroId: this.state.GeneroId,
            FechaNacimiento: this.state.FechaNacimiento,
            Celular: this.state.Celular,
            Mail: this.state.Mail,

            Estado: this.state.EstadoId,
            FechaEstado: new Date()
          })
          .then(response => {
            if (response.data == "OK") {
              this.props.showSuccess("Editado Correctamente");
              this.setState({ visibleeditar: false });
              this.ObtenerChoferes();
            } else if (response.data == "EXISTE") {
              this.alertMessage(
                "Ya existe un conductor con la misma identificacion y asignado al mismo proveedor"
              );
              this.StopLoading();
            } else {
              this.alertMessage("Ocurrió un Error");
            }
          })
          .catch(error => {
            console.log(error);
            this.alertMessage("Ocurrió un Error");
          });
      }
    }
  }
  MostrarFormulario() {
    this.OcultarFormulario();
    this.setState({ visible: true });
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }
  handleChangeCorreo(event) {
    this.setState({ [event.target.name]: event.target.value.toLowerCase() });
  }


  Loading() {
    this.setState({ blocking: true });
  }

  StopLoading() {
    this.setState({ blocking: false });
  }

  OcultarFormulario() {
    this.setState({ visible: false, errors: {}, danger: '' });
    this.VaciarCampos();
  }

  MostrarFormulario() {
    this.setState({ visible: true, errors: {}, danger: '' });
  }
  OcultarFormularioEditar() {
    this.setState({ visibleeditar: false, errors: {}, danger: '' });
    this.VaciarCampos();
  }

  MostrarFormularioEditar(cell) {
    this.setState({
      errors: {},
      ChoferSeleccionado: null,
      e: null, danger: ''
    })
    console.log(cell.Id);
    if (cell.Id > 0) {
      this.ObtenerDetalleChofer(cell.Id);
    }
    this.setState({ visibleeditar: true });
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
const Container = wrapForm(ChoferesList);
ReactDOM.render(<Container />, document.getElementById("content"));
