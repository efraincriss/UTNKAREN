import { Button } from "primereact-v2/button";
import React, { Fragment } from "react";
import config from "../../Base/Config";
import Field from "../../Base/Field-v2";
import http from "../../Base/HttpService";
import {
  CONTROLLER_COLABORADOR_INGENIERIA,
  CONTROLLER_FERIADOS,
  FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
  FRASE_FERIADO_ACTUALIZADA,
  FRASE_PARAMETRO_COLABORADOR_ACTUALIZADO,
  FRASE_PARAMETRO_COLABORADOR_CREADO,
  MODULO_CERTIFICACION_INGENIERIA,
} from "../../Base/Strings";
import { Checkbox } from "primereact-v2/checkbox"

export class ParametroForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data: {
        Id: props.parametro.Id ? props.parametro.Id : 0,
        FechaDesde: props.parametro.FechaDesde
          ? props.parametro.FechaDesde
          : "",
        FechaHasta: props.parametro.FechaHasta
          ? props.parametro.FechaHasta
          : "",
        ColaboradorId: props.parametro.ColaboradorId
          ? props.parametro.ColaboradorId
          : 0,
        DisciplinaId: props.parametro.DisciplinaId
          ? props.parametro.DisciplinaId
          : 0,
        ModalidadId: props.parametro.ModalidadId
          ? props.parametro.ModalidadId
          : 0,
        UbicacionId: props.parametro.UbicacionId
          ? props.parametro.UbicacionId
          : 0,
        AplicaViatico: props.parametro.AplicaViatico
          ? props.parametro.AplicaViatico
          : false,
        AplicaViaticoDirecto: props.parametro.AplicaViaticoDirecto
          ? props.parametro.AplicaViaticoDirecto
          : false,
        EsJornal: props.parametro.EsJornal
          ? props.parametro.EsJornal
          : false,
        EsGastoDirecto: props.parametro.EsGastoDirecto
          ? props.parametro.EsGastoDirecto
          : false,
        CategoriaID: props.parametro.CategoriaID
          ? props.parametro.CategoriaID
          : "",
        HorasPorDia: props.parametro.HorasPorDia
          ? props.parametro.HorasPorDia
          : 0,
        CreationTime: props.parametro.CreationTime
          ? props.parametro.CreationTime
          : new Date().toISOString(),
        CreatorUserId: props.parametro.CreatorUserId
          ? props.parametro.CreatorUserId
          : null,
      },
      errors: {},
      displayMessage: "",
      keyUpload: 98,
    };
  }

  componentWillReceiveProps(prevProps) {
    let updatedValues = {
      Id: prevProps.parametro.Id ? prevProps.parametro.Id : 0,
      FechaDesde: prevProps.parametro.FechaDesde
        ? prevProps.parametro.FechaDesde
        : "",
      FechaHasta: prevProps.parametro.FechaHasta
        ? prevProps.parametro.FechaHasta
        : "",
      ColaboradorId: prevProps.parametro.ColaboradorId
        ? prevProps.parametro.ColaboradorId
        : 0,
      DisciplinaId: prevProps.parametro.DisciplinaId
        ? prevProps.parametro.DisciplinaId
        : 0,
      ModalidadId: prevProps.parametro.ModalidadId
        ? prevProps.parametro.ModalidadId
        : 0,
      UbicacionId: prevProps.parametro.UbicacionId
        ? prevProps.parametro.UbicacionId
        : 0,

      AplicaViatico: prevProps.parametro.AplicaViatico
        ? prevProps.parametro.AplicaViatico
        : false,
      AplicaViaticoDirecto: prevProps.parametro.AplicaViaticoDirecto
        ? prevProps.parametro.AplicaViaticoDirecto
        : false,
      EsJornal: prevProps.parametro.EsJornal
        ? prevProps.parametro.EsJornal
        : false,
      EsGastoDirecto: prevProps.parametro.EsGastoDirecto
        ? prevProps.parametro.EsGastoDirecto
        : false,
      CategoriaID: prevProps.parametro.CategoriaID
        ? prevProps.parametro.CategoriaID
        : "",
      HorasPorDia: prevProps.parametro.HorasPorDia
        ? prevProps.parametro.HorasPorDia
        : 8,
      CreationTime: prevProps.parametro.CreationTime
        ? prevProps.parametro.CreationTime
        : new Date().toISOString(),
      CreatorUserId: prevProps.parametro.CreatorUserId
        ? prevProps.parametro.CreatorUserId
        : null,
    };
    this.setState({
      data: updatedValues,
    });
  }

  render() {
    return (
      <div>
        <div className="row">
          <div className="col">
            <Field
              name="FechaDesde"
              value={this.state.data.FechaDesde}
              label="Fecha Inicio"
              type="date"
              onChange={this.handleChange}
              error={this.state.errors.FechaDesde}
              edit={true}
              readOnly={false}
            />
          </div>

          <div className="col">
            <Field
              name="FechaHasta"
              value={this.state.data.FechaHasta}
              label="Fecha Fin"
              type="date"
              onChange={this.handleChange}
              error={this.state.errors.FechaHasta}
              edit={true}
              readOnly={false}
            />
          </div>
        </div>

        <div className="row">
          <div className="col-6">
            <Field
              name="DisciplinaId"
              label="Disciplina"
              type="select"
              options={this.props.catalogoDisciplina}
              edit={true}
              readOnly={false}
              value={this.state.data.DisciplinaId}
              onChange={this.onChangeDropdown}
              error={this.state.errors.DisciplinaId}
              placeholder="Seleccionar..."
            />
          </div>
          <div className="col-6">
            <Field
              name="ModalidadId"
              label="Modalidad"
              type="select"
              options={this.props.catalogoModalidad}
              edit={true}
              readOnly={false}
              value={this.state.data.ModalidadId}
              onChange={this.onChangeDropdown}
              error={this.state.errors.ModalidadId}
              placeholder="Seleccionar..."
            />
          </div>
        </div>

        <div className="row">
          <div className="col-6">
            <Field
              name="UbicacionId"
              label="Ubicación"
              type="select"
              options={this.props.catalogoUbicacion}
              edit={true}
              readOnly={false}
              value={this.state.data.UbicacionId}
              onChange={this.onChangeDropdown}
              error={this.state.errors.UbicacionId}
              placeholder="Seleccionar..."
            />
          </div>
          <div className="col-6">
            <Field
              name="CategoriaID"
              value={this.state.data.CategoriaID}
              label="I/D"
              type="text"
              onChange={this.handleChange}
              error={this.state.errors.CategoriaID}
              readOnly={false}
              edit={true}
            />
          </div>
        </div>
        <div className="row">
          <div className="col-6">
            <Field
              name="HorasPorDia"
              value={this.state.data.HorasPorDia}
              label="Horas Laboradas Día"
              type="NUMBER"
              min="0"
              max="8"
              onChange={this.handleChange}
              error={this.state.errors.HorasPorDia}
              readOnly={false}
              required={true}
              edit={true}
            />
          </div>
          <div className="col-6"></div>
        </div>
        <div className="row align-items-center">
          <div className="col">
            <Fragment>
              <Checkbox
                inputId="ab1"
                checked={this.state.data.AplicaViatico}
                onChange={(e) => this.handleChange(e)}
                name="AplicaViatico"
              />
              <label htmlFor="ab1" className="p-checkbox-label">
                Aplica Viático?
              </label>
            </Fragment>
          </div>

          <div className="col">
            <Fragment>
              <Checkbox
                inputId="bb1"
                checked={this.state.data.AplicaViaticoDirecto}
                onChange={(e) => this.handleChange(e)}
                name="AplicaViaticoDirecto"
              />
              <label htmlFor="bb1" className="p-checkbox-label">
                Aplica Descuesto Viático?
              </label>
            </Fragment>
          </div>
        </div>
        <div className="row align-items-center">
          <div className="col">
            <Fragment>
              <Checkbox
                inputId="cb1"
                checked={this.state.data.EsJornal}
                onChange={(e) => this.handleChange(e)}
                name="EsJornal"
              />
              <label htmlFor="cb1" className="p-checkbox-label">
                Es Jornal?
              </label>
            </Fragment>
          </div>

          <div className="col">
            <Fragment>
              <Checkbox
                inputId="cb1"
                checked={this.state.data.EsGastoDirecto}
                onChange={(e) => this.handleChange(e)}
                name="EsGastoDirecto"
              />
              <label htmlFor="cb1" className="p-checkbox-label">
                Es Gasto Directo?
              </label>
            </Fragment>
          </div>
        </div>

        <hr />
        <div
          className="row"
          style={{ marginTop: "0.4em", marginLeft: "0.1em" }}
        >
          <Button
            label="Guardar"
            className="p-button-outlined"
            onClick={() => this.handleSubmit()}
            icon="pi pi-save"
          />
          <Button
            style={{ marginLeft: "0.4em" }}
            label="Cancelar"
            className="p-button-outlined"
            onClick={() => this.props.ocultarFormulario()}
            icon="pi pi-ban"
          />
        </div>
      </div>
    );
  }

  handleSubmit = () => {
    if (!this.isValid()) {
      this.showWarn(FRASE_ERROR_VALIDACIONES, "Validaciones");
      return;
    }

    this.props.blockScreen();
    let url = "";
    url = `${config.apiUrl}${MODULO_CERTIFICACION_INGENIERIA}/${CONTROLLER_COLABORADOR_INGENIERIA}`;
    if (this.state.data.Id > 0) {
      url += "/EditarAsync";
    } else {
      url += "/CrearAsync";
    }

    console.log(url);

    let data = Object.assign({}, this.state.data);
    const formData = new FormData();
    for (var key in data) {
      if (data[key] !== null) formData.append(key, data[key]);
      else formData.append(key, "");
    }

    console.log("Enviando");

    http
      .post(url, formData)
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          if (this.state.data.Id > 0) {
            this.props.showSuccess(FRASE_PARAMETRO_COLABORADOR_ACTUALIZADO);
          } else {
            this.props.showSuccess(FRASE_PARAMETRO_COLABORADOR_CREADO);
          }
          this.props.ocultarFormulario(true);
        } else {
          var message = data.message;
          this.showWarn(message);
          this.props.unlockScreen();
        }
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.showWarn(FRASE_ERROR_GLOBAL);
        this.props.unlockScreen();
      });
  };

  isValid = () => {
    const errors = {};
    let fechaDesde = this.state.data.FechaDesde;
    let disciplina = this.state.data.DisciplinaId;
    let modalidad = this.state.data.ModalidadId;
    let ubicacion = this.state.data.UbicacionId;
    let categoria = this.state.data.CategoriaID;
    let horasPorDia = this.state.data.CategoriaID;
    if (fechaDesde === "") {
      errors.FechaDesde = "El campo fecha desde es requerido";
    }

    if(categoria!=="" && categoria!=="I"){
    if (disciplina === 0) {
      errors.DisciplinaId = "El campo disciplina es requerido";
    }
  }
    if (modalidad === 0) {
      errors.ModalidadId = "El campo modalidad es requerido";
    }

    if (ubicacion === 0) {
      errors.UbicacionId = "El campo ubicacion es requerido";
    }

    if (categoria === "") {
      errors.CategoriaID = "El campo categoria es requerido";
    }
    if (horasPorDia === 0) {
      errors.HorasPorDia = "El campo HorasPorDia es requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };

  handleChange = (event) => {
    console.log(event.target);
    const target = event.target;
    const value = target.type === "checkbox" ? target.checked : target.value;
    const name = target.name;
    const updatedData = {
      ...this.state.data,
    };
    updatedData[name] = value;
    this.props.actualizarParametroSeleccionado(updatedData);
    this.setState({
      data: updatedData,
    });
  };

  onChangeDropdown = (name, value) => {
    const updatedData = {
      ...this.state.data,
    };
    updatedData[name] = value;
    this.props.actualizarParametroSeleccionado(updatedData);
    this.setState({
      data: updatedData,
    });
  };

  showWarn = (displayMessage, type = "Error") => {
    this.setState({ displayMessage }, () => this.warn(type));
  };

  warn = (type) => {
    abp.notify.error(this.state.displayMessage, type);
  };
}
