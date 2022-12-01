import { Button } from "primereact-v3.3/button";
import React from "react";
import config from "../../Base/Config";
import Field from "../../Base/Field-v3";
import http from "../../Base/HttpService";
import {
  CONTROLLER_CARPETA,
  CONTROLLER_DOCUMENTO,
  FRASE_DOCUMENTO_CREADO,
  FRASE_DOCUMENTO_ACTUALIZADO,
  FRASE_ERROR_GLOBAL,
  FRASE_ERROR_VALIDACIONES,
  MODULO_DOCUMENTOS,
  CONTROLLER_SECCION,
  FRASE_SECCION_ACTUALIZADA,
  FRASE_SECCION_CREADA,
} from "../../Base/Strings";
import { Checkbox } from "primereact-v3.3/checkbox";
import { Fragment } from "react";
import { ContentEditor } from "./ContentEditor";
import { Jodit } from "jodit";
import CryptoJS from "crypto-js";
export class SeccionForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data: {
        Id: props.seccion.Id ? props.seccion.Id : 0,
        Codigo: props.seccion.Codigo ? props.seccion.Codigo : "",
        Ordinal: props.seccion.Ordinal ? props.seccion.Ordinal : 0,
        NombreSeccion: props.seccion.NombreSeccion
          ? props.seccion.NombreSeccion
          : "",
        DocumentoId: props.documentoId ? props.documentoId : 0,
        Contenido: props.seccion.Contenido
          ? props.seccion.Contenido
          : "<p> </p>",
        Contenido_Plano: props.seccion.Contenido_Plano
          ? props.seccion.Contenido_Plano
          : "<p> </p>",
        SeccionPadreId: props.seccion.SeccionPadreId
          ? props.seccion.SeccionPadreId
          : null,
        NumeroPagina: props.seccion.NumeroPagina
          ? props.seccion.NumeroPagina
          : "",
        CreationTime: props.seccion.CreationTime
          ? props.seccion.CreationTime
          : "",
        CreatorUserId: props.seccion.CreatorUserId
          ? props.seccion.CreatorUserId
          : null,
      },
      errors: {},
      displayMessage: "",
      keyEditor: 983,
    };

    this.refComponent = React.createRef();
  }

  componentDidMount() {
    this.jodit = new Jodit(this.editorRef, {limitChars:17000, height: "450", width: "1000" });
    this.jodit.value = "";
    console.log("Props", this.props);
    this.jodit.value = this.state.data.Contenido;
  }
  componentWillReceiveProps(prevProps) {
    console.log("prevProps", prevProps);
    let updatedValues = {
      Id: prevProps.seccion.Id ? prevProps.seccion.Id : 0,
      Codigo: prevProps.seccion.Codigo ? prevProps.seccion.Codigo : "",
      Ordinal: prevProps.seccion.Ordinal ? prevProps.seccion.Ordinal : 0,
      NombreSeccion: prevProps.seccion.NombreSeccion
        ? prevProps.seccion.NombreSeccion
        : "",
      DocumentoId: prevProps.documentoId ? prevProps.documentoId : 0,
      Contenido: prevProps.seccion.Contenido
        ? prevProps.seccion.Contenido
        : "<p> </p>",
      Contenido_Plano: prevProps.seccion.Contenido_Plano
        ? prevProps.seccion.Contenido_Plano
        : "<p> </p>",
      SeccionPadreId: prevProps.seccion.SeccionPadreId
        ? prevProps.seccion.SeccionPadreId
        : null,
      NumeroPagina: prevProps.seccion.NumeroPagina
        ? prevProps.seccion.NumeroPagina
        : "",
      CreationTime: prevProps.seccion.CreationTime
        ? prevProps.seccion.CreationTime
        : "",
      CreatorUserId: prevProps.seccion.CreatorUserId
        ? prevProps.seccion.CreatorUserId
        : null,
    };
    this.setState({
      data: updatedValues,
      keyEditor: Math.random(),
    });
    this.jodit.value = "";

    console.log("this.props", this.props);
    this.jodit.value = this.state.data.Contenido;
  }

  render() {
    return (
      <div>
        <div className="row">
          <div className="col">
            <Field
              name="NombreSeccion"
              value={this.state.data.NombreSeccion}
              label="* Sección"
              edit={true}
              readOnly={false}
              type="text"
              onChange={this.handleChange}
              error={this.state.errors.NombreSeccion}
            />
          </div>
        </div>

        <div className="row">
          <div className="col">
            <textarea
              ref={(ref) => (this.editorRef = ref)}
              onChange={this.handleChangeContenido}
              value={this.state.data.Contenido}
            />
            {/**    <ContentEditor
              key={this.state.keyEditor}
              updatedContent={this.updatedContent}
              Contenido={this.state.data.Contenido}
              toolbarHidden={false}
            />*/}
          </div>
        </div>

        <div className="row">
          <div className="col">
            <Field
              name="NumeroPagina"
              value={this.state.data.NumeroPagina}
              label="* Página"
              type="number"
              edit={true}
              readOnly={false}
              onChange={this.handleChangeNumeropagina}
              error={this.state.errors.NumeroPagina}
            />
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
            onClick={() => this.props.onHideFormulario()}
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
    url = `${config.apiUrl}${MODULO_DOCUMENTOS}/${CONTROLLER_SECCION}`;
    if (this.state.data.Id > 0) {
      url += "/EditarSeccionAsync";
    } else {
      url += "/CrearDocumentoAsync";
    }

    console.log(url);
    const tempData = this.state.data;
    console.log("value", this.jodit.value);
    console.log("value", this.jodit.value == null);
    console.log("value", this.jodit.value.length === 0);
    if (this.jodit.value.length === 0) {
      tempData.Contenido = "<p> </p>";
      tempData.Contenido_Plano = "<p> </p>";
    } else {
      let baseContenido = btoa(unescape(encodeURIComponent(this.jodit.value)));
      const sinHTML = jQuery("<p>" + this.jodit.value + "</p>").text();
      tempData.Contenido = baseContenido;
      tempData.Contenido_Plano = sinHTML; // ciphertext;
    }

    let data = Object.assign({}, tempData);
    const formData = new FormData();
    for (var key in data) {
      if (data[key] !== null) formData.append(key, data[key]);
      else formData.append(key, "");
    }
    console.log("tempData", formData);

    console.log("Enviando");

    http
      .post(url, formData)
      .then((response) => {
        let data = response.data;
        if (data.success === true) {
          if (this.state.data.Id > 0) {
            this.props.showSuccess(FRASE_SECCION_ACTUALIZADA);
          } else {
            this.props.showSuccess(FRASE_SECCION_CREADA);
          }
          this.props.onHideFormulario(true);
        } else {
          let error = data.errors;

          console.log("error", error);
          if (error !== "MAXIM_ENCRYPT") {
            console.log("DISTINT");
            var message = $.fn.responseAjaxErrorToString(data);
            this.showWarn(message);
          } else {
            this.showWarn(
              "El texto que desea ingresar supera el numero de caracteres permitidos"
            );
          }
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
    let nombreSeccion = this.state.data.NombreSeccion;
    let numeroPagina = this.state.data.NumeroPagina;
    let ordinal = this.state.data.Ordinal;

    if (ordinal === "") {
      errors.Ordinal = "El campo Ordinal es requerido";
    } else if (ordinal.length > 3) {
      errors.Ordinal = "Máximo 3 dígitos";
    }

    if (nombreSeccion === "") {
      errors.NombreSeccion = "El campo Sección es requerido";
    } else if (nombreSeccion.length > 1000) {
      errors.NombreSeccion = "Máximo 1000 caracteres";
    }

    if (numeroPagina.length === 0) {
      errors.NumeroPagina = "Elcampo Numero pagina requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  handleChangeContenido = (event) => {
    console.log("event", event);
    //this.setState{this.state.data.Contenido:}
  };
  handleChangeNumeropagina = (event) => {
    const target = event.target;
    const name = target.name;
    const value = target.type === "checkbox" ? target.checked : target.value;
    const updatedData = this.state.data;
    updatedData.NumeroPagina = value;
    updatedData.Contenido = this.jodit.value;
    this.setState({
      data: updatedData,
    });

    console.log("this.state.data", this.state.data);
  };
  handleChange = (event) => {
    const target = event.target;
    const value = target.type === "checkbox" ? target.checked : target.value;
    const name = target.name;
    const updatedData = {
      ...this.state.data,
    };
    updatedData[name] = value;
    this.props.actualizarSeccionSeleccionada(updatedData);
    this.setState({
      data: updatedData,
    });
  };

  updatedContent = (content) => {
    this.setState({
      ContenidoEditor: content,
    });
  };

  onChangeDropdown = (name, value) => {
    const updatedData = {
      ...this.state.data,
    };
    updatedData[name] = value;
    this.props.actualizarSeccionSeleccionada(updatedData);
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
